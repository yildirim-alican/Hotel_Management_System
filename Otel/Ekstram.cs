using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Otel
{
    public partial class Ekstram : Form
    {
        public Ekstram()
        {
            InitializeComponent();
        }

        private SqlConnection yeni = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");

        public static string kac2;

        private void Ekstram_Load(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();

            SqlCommand komut24 = new SqlCommand();
            komut24.CommandText = "select m.Ad+' '+m.Soyad as 'Ad Soyad',Yemek_isimi as 'Ürün',Yemek_fiyati as 'Fiyat (TL)',Adet as 'Adet',Tarih,e.Ekstra_No as 'Ekstra No',e.Toplam as 'Toplam Fiyat (TL)' from Ekstra as e Left join Musteri as m on m.Musteri_no=e.Musteri_no where e.Oda_No = " + label1.Text.Substring(4) + " ";
            komut24.Connection = yeni;
            SqlDataReader oku824 = komut24.ExecuteReader();
            DataTable tablo24 = new DataTable();
            tablo24.Load(oku824);
            dataGridView3.DataSource = tablo24;
            dataGridView3.AllowUserToAddRows = false;

            //select sum(Toplam) from Ekstra where Oda_No = 130

            SqlCommand komut25 = new SqlCommand();
            komut25.CommandText = "select sum(Toplam) as 'toplam' from Ekstra where Oda_No = " + label1.Text.Substring(4) + " ";
            komut25.Connection = yeni;

            SqlDataReader oku825 = komut25.ExecuteReader();
            if (oku825.HasRows)
            {
                oku825.Read();
                label3.Text = oku825["toplam"].ToString() + " TL";
            }

            yeni.Close();
            yeni.Open();
            SqlCommand komut22 = new SqlCommand();
            komut22.CommandText = "Select  Ad,Soyad from Musteri where Oda_no = " + label1.Text.Substring(4) + "";
            komut22.Connection = yeni;
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Tüm Oda");
            SqlDataReader isimver;
            isimver = komut22.ExecuteReader();
            while (isimver.Read())
            {
                comboBox1.Items.Add(isimver["Ad"] + " " + isimver["Soyad"]);
            }

            comboBox1.SelectedIndex = 0;

            yeni.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();
            string sorgu = "Select * from Musteri where Oda_No = '" + label1.Text.Substring(4) + "'";
            SqlDataAdapter adp6 = new SqlDataAdapter(sorgu, yeni);
            DataSet ds = new DataSet();
            adp6.Fill(ds);
            int kactir = comboBox1.SelectedIndex;

            if (kactir == 0)
            {
                yeni.Close();
                yeni.Open();

                SqlCommand komut24 = new SqlCommand();
                komut24.CommandText = "select m.Ad+' '+m.Soyad as 'Ad Soyad',Yemek_isimi as 'Ürün',Yemek_fiyati as 'Fiyat (TL)',Adet as 'Adet',Tarih,e.Ekstra_No as 'Ekstra No',e.Toplam as 'Toplam Fiyat (TL)' from Ekstra as e Left join Musteri as m on m.Musteri_no=e.Musteri_no where e.Oda_No = " + label1.Text.Substring(4) + " ";
                komut24.Connection = yeni;
                SqlDataReader oku824 = komut24.ExecuteReader();
                DataTable tablo24 = new DataTable();
                tablo24.Load(oku824);
                dataGridView3.DataSource = tablo24;
                dataGridView3.AllowUserToAddRows = false;

                //select sum(Toplam) from Ekstra where Oda_No = 130

                SqlCommand komut25 = new SqlCommand();
                komut25.CommandText = "select sum(Toplam) as 'toplam' from Ekstra where Oda_No = " + label1.Text.Substring(4) + " ";
                komut25.Connection = yeni;

                SqlDataReader oku825 = komut25.ExecuteReader();
                if (oku825.HasRows)
                {
                    oku825.Read();
                    label3.Text = oku825["toplam"].ToString() + " TL";
                }
                yeni.Close();
            }
            else
            {
                kac2 = ds.Tables[0].Rows[comboBox1.SelectedIndex - 1][0].ToString();
                yeni.Close();
                yeni.Open();

                SqlCommand komut24 = new SqlCommand();
                komut24.CommandText = "select m.Ad+' '+m.Soyad as 'Ad Soyad',Yemek_isimi as 'Ürün',Yemek_fiyati as 'Fiyat (TL)',Adet as 'Adet',Tarih,e.Ekstra_No as 'Ekstra No',e.Toplam as 'Toplam Fiyat (TL)' from Ekstra as e Left join Musteri as m on m.Musteri_no=e.Musteri_no where e.Oda_No = " + label1.Text.Substring(4) + " and m.Musteri_no= '" + kac2 + "' ";
                komut24.Connection = yeni;
                SqlDataReader oku824 = komut24.ExecuteReader();
                DataTable tablo24 = new DataTable();
                tablo24.Load(oku824);
                dataGridView3.DataSource = tablo24;
                dataGridView3.AllowUserToAddRows = false;

                //select sum(Toplam) from Ekstra where Oda_No = 130

                SqlCommand komut25 = new SqlCommand();
                komut25.CommandText = "select sum(Toplam) as 'toplam' from Ekstra where Oda_No = " + label1.Text.Substring(4) + "  and Musteri_no= '" + kac2 + "' ";
                komut25.Connection = yeni;

                SqlDataReader oku825 = komut25.ExecuteReader();
                if (oku825.HasRows)
                {
                    oku825.Read();
                    label3.Text = oku825["toplam"].ToString() + " TL";
                }

                yeni.Close();
            }
            yeni.Close();
        }
    }
}