using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Otel
{
    public partial class yetkili : UserControl
    {
        public yetkili()
        {
            InitializeComponent();
            flowLayoutPanel1.AllowDrop = true;
            flowLayoutPanel2.AllowDrop = true;

            flowLayoutPanel1.DragEnter += panel_DragEnter;
            flowLayoutPanel2.DragEnter += panel_DragEnter;

            flowLayoutPanel1.DragDrop += panel_DragDrop;
            flowLayoutPanel2.DragDrop += panel_DragDrop;

            button2.MouseDown += button2_MouseDown;
            button5.MouseDown += button5_MouseDown;
            button6.MouseDown += button6_MouseDown;
            button7.MouseDown += button7_MouseDown;
            button8.MouseDown += button8_MouseDown;
        }

        private SqlConnection yeni = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");

        private void yetkili_Load(object sender, EventArgs e)
        {
            // button2.Parent = flowLayoutPanel2;
            //if (button2.Parent == flowLayoutPanel2)
            // {
            //     MessageBox.Show("Başardın");
            // }
            //else
            // {
            //     MessageBox.Show("Yine Başardın ki amk");
            // }
        }

        private void panel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void panel_DragDrop(object sender, DragEventArgs e)
        {
            ((Button)e.Data.GetData(typeof(Button))).Parent = (Panel)sender;
        }

        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            button5.DoDragDrop(button5, DragDropEffects.Move);
        }

        private void button6_MouseDown(object sender, MouseEventArgs e)
        {
            button6.DoDragDrop(button6, DragDropEffects.Move);
        }

        private void button7_MouseDown(object sender, MouseEventArgs e)
        {
            button7.DoDragDrop(button7, DragDropEffects.Move);
        }

        private void button8_MouseDown(object sender, MouseEventArgs e)
        {
            button8.DoDragDrop(button8, DragDropEffects.Move);
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            button2.DoDragDrop(button2, DragDropEffects.Move);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();

            SqlCommand komut = new SqlCommand();
            komut.CommandText = "insert into Yetkili(Yetkili_isim,Kullanici_adi,Sifre,Yetkili_Telefon,Departman) values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + maskedTextBox1.Text + "','" + textBox4.Text + "') ";
            komut.Connection = yeni;

            komut.ExecuteNonQuery();

            {
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                maskedTextBox1.Clear();

                MessageBox.Show(" Kayıt başarıyla eklendi");
            }

            groupBox5.Hide();

            yeni.Close();
            yeni.Open();
            SqlCommand komut2 = new SqlCommand();
            komut2.CommandText = "select Yetkili_No as 'Yetkili Numarası' , Yetkili_isim as 'İsim Soyisim',Departman as 'Departman',Yetkili_Telefon as 'Telefon Numarası' from Yetkili ";
            komut2.Connection = yeni;
            SqlDataReader oku2 = komut2.ExecuteReader();
            DataTable tablo2 = new DataTable();
            tablo2.Load(oku2); dataGridView2.DataSource = tablo2;
            dataGridView2.AllowUserToAddRows = false;

            yeni.Close();
        }

        private void groupBox4_VisibleChanged(object sender, EventArgs e)
        {
        }

        private void yetkili_VisibleChanged(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();
            SqlCommand komut = new SqlCommand();
            komut.CommandText = "select Yetkili_No as 'Yetkili Numarası' , Yetkili_isim as 'İsim Soyisim',Departman as 'Departman',Yetkili_Telefon as 'Telefon Numarası' from Yetkili ";
            komut.Connection = yeni;
            SqlDataReader oku = komut.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku); dataGridView2.DataSource = tablo;
            dataGridView2.AllowUserToAddRows = false;

            yeni.Close();

            groupBox5.Hide();
            groupBox2.Hide();
        }

        public static int yetki1;
        public static int yetki2;
        public static int yetki3;
        public static int yetki4;
        public static int yetki5;

        private void button4_Click(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();
            groupBox2.Visible = true;
            groupBox1.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();

            int yer = Int32.Parse(dataGridView2.CurrentRow.Cells[0].Value.ToString());
            SqlCommand cmd2 = new SqlCommand("Select * from Yetkili where Yetkili_No = '" + yer + "'", yeni);
            SqlDataReader oku2 = cmd2.ExecuteReader();

            label8.Text = yer.ToString();

            while (oku2.Read())
            {
                yetki1 = Convert.ToInt32(oku2["yetki1"].ToString());

                yetki2 = Convert.ToInt32(oku2["yetki2"].ToString());

                yetki3 = Convert.ToInt32(oku2["yetki3"].ToString());

                yetki4 = Convert.ToInt32(oku2["yetki4"].ToString());

                yetki5 = Convert.ToInt32(oku2["yetki5"].ToString());
            }

            if (yetki1 == 1)
            {
                button5.Parent = flowLayoutPanel2;
            }
            else
            {
                button5.Parent = flowLayoutPanel1;
            }

            if (yetki2 == 1)
            {
                button6.Parent = flowLayoutPanel2;
            }
            else
            {
                button6.Parent = flowLayoutPanel1;
            }

            if (yetki3 == 1)
            {
                button7.Parent = flowLayoutPanel2;
            }
            else
            {
                button7.Parent = flowLayoutPanel1;
            }

            if (yetki4 == 1)
            {
                button8.Parent = flowLayoutPanel2;
            }
            else
            {
                button8.Parent = flowLayoutPanel1;
            }

            if (yetki5 == 1)
            {
                button2.Parent = flowLayoutPanel2;
            }
            else
            {
                button2.Parent = flowLayoutPanel1;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBox5.Visible = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            groupBox5.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            groupBox2.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button5.Parent == flowLayoutPanel2)
            {
                yeni.Close();
                yeni.Open();
                SqlCommand komut455 = new SqlCommand();
                komut455.CommandText = "UPDATE Yetkili SET yetki1 = 1 where Yetkili_No =  '" + label8.Text + "'";
                komut455.Connection = yeni;

                komut455.ExecuteNonQuery();
                yeni.Close();
            }
            else
            {
                yeni.Close();
                yeni.Open();
                SqlCommand komut455 = new SqlCommand();
                komut455.CommandText = "UPDATE Yetkili SET yetki1 = 0 where Yetkili_No =  '" + label8.Text + "'";
                komut455.Connection = yeni;

                komut455.ExecuteNonQuery();
                yeni.Close();
            }

            if (button6.Parent == flowLayoutPanel2)
            {
                yeni.Close();
                yeni.Open();
                SqlCommand komut455 = new SqlCommand();
                komut455.CommandText = "UPDATE Yetkili SET yetki2 = 1 where Yetkili_No =  '" + label8.Text + "'";
                komut455.Connection = yeni;

                komut455.ExecuteNonQuery();
                yeni.Close();
            }
            else
            {
                yeni.Close();
                yeni.Open();
                SqlCommand komut455 = new SqlCommand();
                komut455.CommandText = "UPDATE Yetkili SET yetki2 = 0 where Yetkili_No =  '" + label8.Text + "'";
                komut455.Connection = yeni;

                komut455.ExecuteNonQuery();
                yeni.Close();
            }

            if (button7.Parent == flowLayoutPanel2)
            {
                yeni.Close();
                yeni.Open();
                SqlCommand komut455 = new SqlCommand();
                komut455.CommandText = "UPDATE Yetkili SET yetki3 = 1 where Yetkili_No =  '" + label8.Text + "'";
                komut455.Connection = yeni;

                komut455.ExecuteNonQuery();
                yeni.Close();
            }
            else
            {
                yeni.Close();
                yeni.Open();
                SqlCommand komut455 = new SqlCommand();
                komut455.CommandText = "UPDATE Yetkili SET yetki3 = 0 where Yetkili_No =  '" + label8.Text + "'";
                komut455.Connection = yeni;

                komut455.ExecuteNonQuery();
                yeni.Close();
            }

            if (button8.Parent == flowLayoutPanel2)
            {
                yeni.Close();
                yeni.Open();
                SqlCommand komut455 = new SqlCommand();
                komut455.CommandText = "UPDATE Yetkili SET yetki4 = 1 where Yetkili_No =  '" + label8.Text + "'";
                komut455.Connection = yeni;

                komut455.ExecuteNonQuery();
                yeni.Close();
            }
            else
            {
                yeni.Close();
                yeni.Open();
                SqlCommand komut455 = new SqlCommand();
                komut455.CommandText = "UPDATE Yetkili SET yetki4 = 0 where Yetkili_No =  '" + label8.Text + "'";
                komut455.Connection = yeni;

                komut455.ExecuteNonQuery();
                yeni.Close();
            }

            if (button2.Parent == flowLayoutPanel2)
            {
                yeni.Close();
                yeni.Open();
                SqlCommand komut455 = new SqlCommand();
                komut455.CommandText = "UPDATE Yetkili SET yetki5 = 1 where Yetkili_No =  '" + label8.Text + "'";
                komut455.Connection = yeni;

                komut455.ExecuteNonQuery();
                yeni.Close();
            }
            else
            {
                yeni.Close();
                yeni.Open();
                SqlCommand komut455 = new SqlCommand();
                komut455.CommandText = "UPDATE Yetkili SET yetki5 = 0 where Yetkili_No =  '" + label8.Text + "'";
                komut455.Connection = yeni;

                komut455.ExecuteNonQuery();
                yeni.Close();
            }

            groupBox2.Hide();

            MessageBox.Show("Yetkiler Güncellendi");
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            groupBox2.Hide();
        }
    }
}