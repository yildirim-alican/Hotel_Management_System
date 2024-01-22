using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Otel
{
    public partial class lokantafiyat : UserControl
    {
        public lokantafiyat()
        {
            InitializeComponent();
        }

        private SqlConnection yeni = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");

        private void button1_Click(object sender, EventArgs e)
        {
            yeni.Open();

            SqlCommand komut4 = new SqlCommand("select count(*) from Lokanta_fiyat where Yemek_isimi=@yisim1", yeni);
            SqlParameter yisim1 = new SqlParameter();
            yisim1.ParameterName = "@yisim1";
            yisim1.SqlDbType = SqlDbType.VarChar;
            yisim1.Size = 50;
            yisim1.Value = textBox1.Text;
            komut4.Parameters.Add(yisim1);
            komut4.Parameters.AddWithValue("@yisim", textBox1.Text);

            if (Convert.ToInt32(komut4.ExecuteScalar()) > 0)

            {
                MessageBox.Show("Bu kayıt veritabanında zaten var.");
            }
            else
            {
                SqlCommand komut2 = new SqlCommand();
                komut2.CommandText = "insert into Lokanta_fiyat(Yemek_isimi,Yemek_fiyati) values(@yisim,@yfiyat)";
                komut2.Connection = yeni;

                SqlParameter yisim = new SqlParameter();
                yisim.ParameterName = "@yisim";
                yisim.SqlDbType = SqlDbType.VarChar;
                yisim.Size = 50;
                yisim.Value = textBox1.Text;
                komut2.Parameters.Add(yisim);

                SqlParameter yfiyat = new SqlParameter();
                yfiyat.ParameterName = "@yfiyat";
                yfiyat.SqlDbType = SqlDbType.VarChar;
                yfiyat.Size = 50;
                yfiyat.Value = textBox2.Text + " TL";
                komut2.Parameters.Add(yfiyat);

                komut2.ExecuteNonQuery();
                MessageBox.Show("kayıt eklendi");

                SqlCommand komut3 = new SqlCommand();
                komut3.CommandText = "Select Yemek_isimi as 'Ürün Adı',Yemek_fiyati as 'Ürün Fiyatı' from Lokanta_fiyat ORDER BY Yemek_isimi ASC";
                komut3.Connection = yeni;
                SqlDataReader oku = komut3.ExecuteReader();
                DataTable tablo = new DataTable();
                tablo.Load(oku); dataGridView1.DataSource = tablo;
                dataGridView1.AllowUserToAddRows = false;

                foreach (Control item in this.Controls)
                {
                    if (item is TextBox)
                    {
                        TextBox tbox = (TextBox)item;
                        tbox.Clear();
                    }
                }
            }

            yeni.Close();
        }

        private void lokantafiyat_Load(object sender, EventArgs e)
        {
            yeni.Open();

            SqlCommand komut = new SqlCommand();
            komut.CommandText = "Select Yemek_isimi as 'Ürün Adı',Yemek_fiyati as 'Ürün Fiyatı' from Lokanta_fiyat ORDER BY Yemek_isimi ASC";
            komut.Connection = yeni;
            SqlDataReader oku = komut.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku); dataGridView1.DataSource = tablo;
            dataGridView1.AllowUserToAddRows = false;

            yeni.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            yeni.Open();

            string kayit = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            SqlDataAdapter baglan = new SqlDataAdapter("DELETE from Lokanta_fiyat where Yemek_isimi = '" + kayit + "'", yeni);

            DataTable tablo2 = new DataTable();
            baglan.Fill(tablo2);

            SqlCommand komut2 = new SqlCommand();
            komut2.CommandText = "Select Yemek_isimi as 'Ürün Adı',Yemek_fiyati as 'Ürün Fiyatı' from Lokanta_fiyat ORDER BY Yemek_isimi ASC";
            komut2.Connection = yeni;
            SqlDataReader oku = komut2.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku); dataGridView1.DataSource = tablo;
            dataGridView1.AllowUserToAddRows = false;

            yeni.Close();
        }

        public class genel555
        {
            public static string secim555;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Text = "";

            switch (comboBox2.SelectedItem.ToString().Trim())
            {
                case "Ad Soyad":
                    genel555.secim555 = "m.Ad+m.Soyad";
                    break;

                case "Müşteri Numarası":
                    genel555.secim555 = "h.Musteri_no";
                    break;

                case "Oda Numarası":
                    genel555.secim555 = "h.Oda_No";
                    break;

                case "Arama Kriterini Seçin":
                    genel555.secim555 = "10";
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (genel555.secim555 == "10")
            {
                MessageBox.Show("Arama Yapabilmek İçin Lütfen Bir Kriter Seçiniz");
            }
            else
            {
                yeni.Close();
                yeni.Open();
                string dgr = textBox3.Text.Replace(" ", "");

                SqlCommand komut = new SqlCommand();
                komut.CommandText = "select  m.Musteri_no as 'Müş No' , ad+ ' '+ soyad as 'Ad Soyad', m.Oda_no as 'Oda No' from Musteri as m join Hesap as h on m.Musteri_no=h.Musteri_no where " + genel555.secim555 + " Like '%" + dgr + "%' and h.Durum=1  order by m.oda_no DESC ";
                komut.Connection = yeni;
                SqlDataReader oku = komut.ExecuteReader();
                DataTable tablo = new DataTable();
                tablo.Load(oku); dataGridView2.DataSource = tablo;
                dataGridView2.AllowUserToAddRows = false;

                yeni.Close();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (genel555.secim555 == "10")
            {
                MessageBox.Show("Arama Yapabilmek İçin Lütfen Bir Kriter Seçiniz");
                textBox1.Text = "";
            }
            else
            {
                yeni.Close();
                yeni.Open();
                string dgr = textBox3.Text.Replace(" ", "");
                SqlCommand komut = new SqlCommand();
                komut.CommandText = "select  m.Musteri_no as 'Müş No' , ad+ ' '+ soyad as 'Ad Soyad', m.Oda_no as 'Oda No' from Musteri as m join Hesap as h on m.Musteri_no=h.Musteri_no where " + genel555.secim555 + " Like '%" + dgr + "%' and h.Durum=1  order by m.oda_no DESC ";
                komut.Connection = yeni;
                SqlDataReader oku = komut.ExecuteReader();
                DataTable tablo = new DataTable();
                tablo.Load(oku); dataGridView2.DataSource = tablo;
                dataGridView2.AllowUserToAddRows = false;

                //    yeni.Close();
            }
        }

        public static string Ad5;
        public static string Soyad5;
        public static string mus5;
        public static string oda5;

        public static int yer;

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            yer = Int32.Parse(dataGridView2.CurrentRow.Cells[0].Value.ToString());
            yeni.Close();
            yeni.Open();
            SqlCommand cmd = new SqlCommand("Select * from Musteri where Musteri_no = '" + yer + "'", yeni);
            SqlDataReader oku = cmd.ExecuteReader();

            while (oku.Read())
            {
                Ad5 = oku["ad"].ToString();
                Soyad5 = oku["soyad"].ToString();
                mus5 = oku["Musteri_no"].ToString();
                oda5 = oku["Oda_no"].ToString();
            }

            label5.Text = Ad5 + " " + Soyad5;
            label15.Text = oda5;

            yeni.Close();
            yeni.Open();

            SqlCommand komut24 = new SqlCommand();
            komut24.CommandText = "select Ekstra_No as 'Ekstra No', Yemek_isimi as 'Ürün',Yemek_fiyati as 'Fiyat',Adet as 'Adet',Tarih from Ekstra where Musteri_no = " + yer + " ";
            komut24.Connection = yeni;
            SqlDataReader oku824 = komut24.ExecuteReader();
            DataTable tablo24 = new DataTable();
            tablo24.Load(oku824);
            dataGridView3.DataSource = tablo24;
            dataGridView3.AllowUserToAddRows = false;

            yeni.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string yer2 = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            string yer3 = dataGridView1.CurrentRow.Cells[1].Value.ToString();

            label11.Text = yer2;
            label10.Text = yer3;

            if (textBox4.Text == "")
            {
                label7.Text = "0 TL";
            }
            else
            {
                int ada = Convert.ToInt32(yer3.Replace("TL", ""));
                int ada2 = Convert.ToInt32(textBox4.Text);

                label7.Text = Convert.ToString(ada * ada2) + " TL";
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                label7.Text = "0 TL";
            }
            else
            {
                int ada = Convert.ToInt32(label10.Text.Replace("TL", ""));
                int ada2 = Convert.ToInt32(textBox4.Text);

                label7.Text = Convert.ToString(ada * ada2) + " TL";
            }
        }

        private void lokantafiyat_VisibleChanged(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = 0;

            yeni.Close();
            yeni.Open();

            SqlCommand komut = new SqlCommand();
            komut.CommandText = "select  m.Musteri_no as 'Müş No' , ad+ ' '+ soyad as 'Ad Soyad', m.Oda_no as 'Oda No' from Musteri as m join Hesap as h on m.Musteri_no=h.Musteri_no where  h.Durum=1  order by m.oda_no DESC";
            komut.Connection = yeni;
            SqlDataReader oku8 = komut.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku8); dataGridView2.DataSource = tablo;
            dataGridView2.AllowUserToAddRows = false;

            yeni.Close();
            yeni.Open();

            SqlCommand komut2 = new SqlCommand();
            komut2.CommandText = "select Ekstra_No as 'Ekstra No', Yemek_isimi as 'Ürün',Yemek_fiyati as 'Fiyat',Adet as 'Adet',Tarih from Ekstra where Musteri_no = " + yer + " ";
            komut2.Connection = yeni;
            SqlDataReader oku82 = komut2.ExecuteReader();
            DataTable tablo2 = new DataTable();
            tablo2.Load(oku82);
            dataGridView3.DataSource = tablo2;
            dataGridView3.AllowUserToAddRows = false;

            yeni.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();

            SqlCommand komut2 = new SqlCommand();
            komut2.CommandText = "insert into Ekstra(Musteri_no,Oda_No,Yemek_fiyati,Yemek_isimi,Adet,Tarih,Toplam) values(@yMusteri_no,@yOda_No,@yYemek_fiyati,@yYemek_isimi,@yAdet,@yTarih,@yToplam)";
            komut2.Connection = yeni;

            SqlParameter yMusteri_no = new SqlParameter();
            yMusteri_no.ParameterName = "@yMusteri_no";
            yMusteri_no.SqlDbType = SqlDbType.VarChar;
            yMusteri_no.Size = 50;
            yMusteri_no.Value = yer;
            komut2.Parameters.Add(yMusteri_no);

            SqlParameter yOda_No = new SqlParameter();
            yOda_No.ParameterName = "@yOda_No";
            yOda_No.SqlDbType = SqlDbType.Int;
            yOda_No.Size = 50;
            yOda_No.Value = Convert.ToInt32(label15.Text);
            komut2.Parameters.Add(yOda_No);

            SqlParameter yYemek_fiyati = new SqlParameter();
            yYemek_fiyati.ParameterName = "@yYemek_fiyati";
            yYemek_fiyati.SqlDbType = SqlDbType.Int;
            yYemek_fiyati.Size = 50;
            yYemek_fiyati.Value = Convert.ToInt32(label10.Text.Replace("TL", ""));
            komut2.Parameters.Add(yYemek_fiyati);

            SqlParameter yYemek_isimi = new SqlParameter();
            yYemek_isimi.ParameterName = "@yYemek_isimi";
            yYemek_isimi.SqlDbType = SqlDbType.VarChar;
            yYemek_isimi.Size = 50;
            yYemek_isimi.Value = label11.Text;
            komut2.Parameters.Add(yYemek_isimi);

            SqlParameter yAdet = new SqlParameter();
            yAdet.ParameterName = "@yAdet";
            yAdet.SqlDbType = SqlDbType.VarChar;
            yAdet.Size = 50;
            yAdet.Value = textBox4.Text;
            komut2.Parameters.Add(yAdet);

            SqlParameter yTarih = new SqlParameter();
            yTarih.ParameterName = "@yTarih";
            yTarih.SqlDbType = SqlDbType.Date;
            yTarih.Size = 50;
            yTarih.Value = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            komut2.Parameters.Add(yTarih);

            SqlParameter yToplam = new SqlParameter();
            yToplam.ParameterName = "@yToplam";
            yToplam.SqlDbType = SqlDbType.Int;
            yToplam.Size = 50;
            yToplam.Value = Convert.ToInt32(label7.Text.Replace("TL", ""));
            komut2.Parameters.Add(yToplam);

            komut2.ExecuteNonQuery();

            MessageBox.Show("Ekstra Eklendi");

            yeni.Close();
            yeni.Open();

            SqlCommand komut24 = new SqlCommand();
            komut24.CommandText = "select Ekstra_No as 'Ekstra No', Yemek_isimi as 'Ürün',Yemek_fiyati as 'Fiyat',Adet as 'Adet',Tarih from Ekstra where Musteri_no = " + yer + " ";
            komut24.Connection = yeni;
            SqlDataReader oku824 = komut24.ExecuteReader();
            DataTable tablo24 = new DataTable();
            tablo24.Load(oku824);
            dataGridView3.DataSource = tablo24;
            dataGridView3.AllowUserToAddRows = false;

            yeni.Close();
        }

        private void textBox4_MouseClick(object sender, MouseEventArgs e)
        {
            textBox4.SelectAll();
        }
    }
}