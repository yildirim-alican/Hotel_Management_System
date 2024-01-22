using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Otel
{
    public partial class musekle : Form
    {
        public musekle()
        {
            InitializeComponent();
        }

        private SqlConnection yeni = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");

        public static int yeri2;

        private void musekle_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            yeni.Close();
            yeni.Open();
            SqlCommand komut = new SqlCommand();
            komut.CommandText = "Select Musteri_no as 'Müşteri Numarası' , Ad,Soyad   from Musteri where Oda_no is null ORDER BY Musteri_no DESC ";
            komut.Connection = yeni;
            SqlDataReader oku = komut.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku); dataGridView2.DataSource = tablo;
            dataGridView2.AllowUserToAddRows = false;
            yeni.Close();
        }

        public class genel
        {
            public static string secim;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (genel.secim == "10")
            {
                MessageBox.Show("Arama Yapabilmek İçin Lütfen Bir Kriter Seçiniz");
            }
            else
            {
                yeni.Close();
                yeni.Open();
                string dgr = textBox1.Text;
                SqlCommand komut = new SqlCommand();
                komut.CommandText = "Select Musteri_no as 'Müşteri Numarası', Ad ,Soyad  from Musteri where " + genel.secim + " Like '%" + dgr + "%' and Oda_no is null ORDER BY Musteri_no DESC ";
                komut.Connection = yeni;
                SqlDataReader oku = komut.ExecuteReader();
                DataTable tablo = new DataTable();
                tablo.Load(oku); dataGridView2.DataSource = tablo;
                dataGridView2.AllowUserToAddRows = false;

                yeni.Close();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (genel.secim == "10")
            {
                MessageBox.Show("Arama Yapabilmek İçin Lütfen Bir Kriter Seçiniz");
            }
            else
            {
                if (e.KeyCode == Keys.Enter)
                {
                    yeni.Close();
                    yeni.Open();
                    string dgr = textBox1.Text;
                    SqlCommand komut = new SqlCommand();
                    komut.CommandText = "Select Musteri_no as 'Müşteri Numarası', Ad ,Soyad from Musteri where " + genel.secim + " Like '%" + dgr + "%' and Oda_no is null  ORDER BY Musteri_no DESC ";
                    komut.Connection = yeni;
                    SqlDataReader oku = komut.ExecuteReader();
                    DataTable tablo = new DataTable();
                    tablo.Load(oku); dataGridView2.DataSource = tablo;
                    dataGridView2.AllowUserToAddRows = false;

                    yeni.Close();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedItem.ToString().Trim())
            {
                case "Ad":
                    genel.secim = "Ad";
                    break;

                case "Soyad":
                    genel.secim = "Soyad";
                    break;

                case "Oda Numarası":
                    genel.secim = "Ad";
                    break;

                case "Tc Kimlik Numarası":
                    genel.secim = "Kimlik_No";
                    break;

                case "Müşteri Numarası":
                    genel.secim = "Musteri_no";
                    break;

                case "Arama Kriterini Seçin":
                    genel.secim = "10";
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label4.Text == label5.Text)
            {
                MessageBox.Show("Bu Odaya Daha Fazla Müşteri Ekleyemessiniz");
            }
            else
            {
                yeni.Close();
                yeni.Open();

                SqlCommand komut2 = new SqlCommand();
                komut2.CommandText = "insert into Hesap(Musteri_no,Giris_Tarihi,Cikis_Tarihi,Oda_No) values(@yMusteri_no,@yGiris_Tarihi,@yCikis_Tarihi,@yOda_No)";
                komut2.Connection = yeni;

                SqlParameter yMusteri_no = new SqlParameter();
                yMusteri_no.ParameterName = "@yMusteri_no";
                yMusteri_no.SqlDbType = SqlDbType.Int;
                yMusteri_no.Size = 50;
                yMusteri_no.Value = yeri2;
                komut2.Parameters.Add(yMusteri_no);

                SqlParameter yOda_No = new SqlParameter();
                yOda_No.ParameterName = "@yOda_No";
                yOda_No.SqlDbType = SqlDbType.Int;
                yOda_No.Size = 50;
                yOda_No.Value = Convert.ToInt32(label1.Text);
                komut2.Parameters.Add(yOda_No);

                SqlParameter yGiris_Tarihi = new SqlParameter();
                yGiris_Tarihi.ParameterName = "@yGiris_Tarihi";
                yGiris_Tarihi.SqlDbType = SqlDbType.Date;
                yGiris_Tarihi.Size = 50;
                yGiris_Tarihi.Value = label2.Text;
                komut2.Parameters.Add(yGiris_Tarihi);

                SqlParameter yCikis_Tarihi = new SqlParameter();
                yCikis_Tarihi.ParameterName = "@yCikis_Tarihi";
                yCikis_Tarihi.SqlDbType = SqlDbType.Date;
                yCikis_Tarihi.Size = 50;
                yCikis_Tarihi.Value = label3.Text;
                komut2.Parameters.Add(yCikis_Tarihi);

                komut2.ExecuteNonQuery();
                MessageBox.Show("Kişi Odaya Eklendi");

                SqlCommand komut3 = new SqlCommand();
                komut3.CommandText = "UPDATE Musteri SET Oda_no = '" + label1.Text + "' where Musteri_no = '" + yeri2 + "'";
                komut3.Connection = yeni;

                komut3.ExecuteNonQuery();

                this.Close();
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            yeri2 = Int32.Parse(dataGridView2.CurrentRow.Cells[0].Value.ToString());
        }
    }
}