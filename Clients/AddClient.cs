using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace Clients
{
    public partial class AddClient : Form
    {
        string filename = "'" + "images/blank.jpg" + "'";
        string fio;
        string birth;
        string zone;
        string numberTel;
        string prim;
        string dateofadd;

        string newPath;
        bool avatarWasEnter = false;
        public AddClient()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = Image.FromFile("images/blank.jpg");
        }

        private void DataBaseAdd() 
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=usersdata.db"))
                {
                    connection.Open();

                    SqliteCommand command = new SqliteCommand();
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO clients (photo, fio, birth, zone, number, prim, dateofadd) VALUES (" + filename + "," + fio + "," + birth + "," + zone + "," + numberTel + "," + prim + "," + dateofadd + ")";
                    int number = command.ExecuteNonQuery();

                    MessageBox.Show("Клиент успешно добавлен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    MainWindow mainWindow = new MainWindow();
                    Enabled = true;
                    mainWindow.LoadDataBase();
                    
                    this.Close();
                }
            }
            catch { MessageBox.Show("Ошибка", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            MainWindow mainWindow = new MainWindow
            {
                Enabled = true
            };
            this.Close();
        }

        private void openPhoto_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Image files(*.jpg)|*.jpg";

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);

                filename = ofd.FileName;
                newPath = @"images/" + ofd.SafeFileName;
                avatarWasEnter = true;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (avatarWasEnter)
            {
                string path = filename;
                FileInfo fileInf = new FileInfo(path);
                if (fileInf.Exists)
                {
                    fileInf.CopyTo(newPath, true);
                    filename = "'" + newPath + "'";
                }
                avatarWasEnter = false;
            }

            fio = "'" + fioTextbox.Text + "'";
            birth = "'" + birthTextbox.Text + "'";
            zone = "'" + ZoneTextbox.Text + "'";
            numberTel = "'" + TeltextBox.Text + "'";
            prim = "'" + PrimtextBox.Text + "'";
            dateofadd = "'" + DateTime.Today.ToShortDateString() + "'";

            DataBaseAdd();
        }
    }
}
