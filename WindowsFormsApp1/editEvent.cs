using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    public partial class editEvent : Form
    {
        DataAccess dataAccess;
        String username;
        public editEvent()
        {
            InitializeComponent();
        }

        public editEvent(String uname)
        {
            InitializeComponent();
            dataAccess = new DataAccess();
            this.username = uname;
            dataAccess = new DataAccess();
            dataGridView1.RowTemplate.Height = 150;
            dataGridView1.AllowUserToAddRows = false;
            loadEvent();
        }

        private void editEvent_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        public void loadEvent()
        {

            DataTable dataTable = new DataTable();
            string sql = "Select id,date,moddate,importance,story,image from event where username='" + username + "'";
            SqlDataAdapter sda = new SqlDataAdapter(sql, dataAccess.connection);
            sda.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
            dataAccess.Dispose();
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[rowIndex];

            textBox2.Text = row.Cells[0].Value.ToString();
            comboBox1.SelectedItem = row.Cells[3].Value.ToString();
            textBox1.Text = row.Cells[4].Value.ToString();
            byte[] img = (byte[])row.Cells[5].Value;
            MemoryStream ms = new MemoryStream(img);
            pictureBox1.Image = Image.FromStream(ms);

        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                MessageBox.Show("Empty Fields..");
            }
            else
            {

                Image img = pictureBox1.Image;
                byte[] arr;
                ImageConverter converter = new ImageConverter();
                arr = (byte[])converter.ConvertTo(img, typeof(byte[]));

                try
                {
                    dataAccess = new DataAccess();
                    string sql = "UPDATE event SET importance=@importance, moddate=@moddate,story=@story,image=@image Where id ='" + textBox2.Text + "'";
                    SqlCommand commandImage = new SqlCommand(sql, dataAccess.connection);
                    
                    commandImage.Parameters.AddWithValue("@importance", comboBox1.SelectedItem.ToString());
                    commandImage.Parameters.AddWithValue("@moddate", dateTimePicker1.Text);
                    commandImage.Parameters.AddWithValue("@story", textBox1.Text);
                    commandImage.Parameters.AddWithValue("@image", arr);
                    int result = commandImage.ExecuteNonQuery();
                    dataAccess.Dispose();
                    if (result == 1)
                    {
                        MessageBox.Show("New Story create successfully.");
                        textBox1.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Error occured..");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error occured.." + ex);
                }
            }
        }

        public void clearText()
        {
            textBox2.Text = "";
            textBox1.Text = "";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void imageLoad_Click(object sender, EventArgs e)
        {
            String imageLocation = "";
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(*.jpg)|*.jpg| PNG Files(*.png)|*.png| All Files(*.*)|*.*";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    imageLocation = dialog.FileName;
                    imagePathTxt.Text = dialog.FileName;

                    pictureBox1.ImageLocation = imageLocation;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error Occured-" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadEvent();
        }
    }
}
