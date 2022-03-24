using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace Clients
{
    public partial class MainWindow : Form
    {
        string numberTel;
        string dateofadd;
        string selected_fio;

        bool selectElement = false;

        int count = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            LoadDataBase();

            deketeToolStripMenuItem.Enabled = false;

            //...........................SETTONGS TO DO.......................
            MainListView.Columns[0].Width = 100;
            MainListView.Columns[1].Width = this.Width / 5;
            //----------------------------------------------------------------
        }

        public void LoadDataBase()
        {
            MainListView.Items.Clear();
            count = 0;
            ImageList imageList = new ImageList();

            // устанавливаем размер изображений
            imageList.ImageSize = new Size(100, 100);

            // устанавливаем в listView1 список изображений imageList
            MainListView.SmallImageList = imageList;

            using (var connection = new SqliteConnection("Data Source=usersdata.db"))
            {
                connection.Open();

                string sqlExpression = "SELECT * FROM clients";

                SqliteCommand command = new SqliteCommand(sqlExpression, connection);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read())   // построчно считываем данные
                        {
                            var photo_obj = reader.GetValue(0);
                            var fio_obj = reader.GetValue(1);
                            var birth_obj = reader.GetValue(2);
                            var zone_obj = reader.GetValue(3);
                            var tel_obj = reader.GetValue(4);
                            var prim_obj = reader.GetValue(5);
                            var dateofadd_obj = reader.GetValue(6);

                            numberTel = tel_obj.ToString();
                            dateofadd = dateofadd_obj.ToString();

                            // заполняем список изображениями
                            imageList.Images.Add(new Bitmap(photo_obj.ToString()));

                            // создадим объект ListViewItem (строку) для listView1
                            ListViewItem listViewItem = new ListViewItem(new string[] 
                            { "", fio_obj.ToString(), birth_obj.ToString(), zone_obj.ToString(), numberTel, prim_obj.ToString() });

                            // индекс изображения из imageList для данной строки listViewItem
                            listViewItem.ImageIndex = count;

                            // добавляем созданный элемент listViewItem (строку) в listView1
                            MainListView.Items.Add(listViewItem);

                            count++;
                        }
                    }
                }
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddClient addClient = new AddClient();
            addClient.Show();
            //this.Enabled = false;
        }

        private void UpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDataBase();
        }

        private void deketeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectElement)
            {
                string sqlExpression = "DELETE  FROM clients WHERE fio=" + "'" + selected_fio + "'";
                using (var connection = new SqliteConnection("Data Source=usersdata.db"))
                {
                    connection.Open();

                    SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                    int number = command.ExecuteNonQuery();
                }
                selectElement = false;
                LoadDataBase();
            }
        }

        private void MainListView_SelectedIndexChanged_UsingItems(object sender, System.EventArgs e)
        {
            ListView.SelectedListViewItemCollection selected_item = this.MainListView.SelectedItems;

            foreach (ListViewItem item in selected_item)
            {
                selected_fio = item.SubItems[1].Text;
                selectElement = true;
                deketeToolStripMenuItem.Enabled = selectElement;
            }

            if (selected_item.Count == 0) 
            {
                selectElement = false;
                deketeToolStripMenuItem.Enabled = selectElement;
            }
        }
    }
}
