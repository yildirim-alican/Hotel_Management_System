using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Otel
{
    public partial class anasayfa : UserControl
    {
        public anasayfa()
        {
            InitializeComponent();
        }

        private SqlConnection yeni = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");

        public static int toplamodasayisi;
        public static int aktifodasayisi;
        public static int sonuc;
        public static int sonuc2;
        public static int musterisayisi;
        public static int odasayisi;
        public static int toplammusterisayisi;
        public static int tursayisi;
        public static int tytk;
        public static int cytk;

        private void anasayfa_Load(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();

            SqlCommand komut5 = new SqlCommand();
            komut5.CommandText = "select COUNT(*) as 'toplam' from Odalar";
            komut5.Connection = yeni;
            SqlDataReader oku = komut5.ExecuteReader();

            while (oku.Read())
            {
                toplamodasayisi = Convert.ToInt32(oku["toplam"].ToString());
            }

            yeni.Close();
            yeni.Open();

            SqlCommand komut55 = new SqlCommand();
            komut55.CommandText = "select COUNT(*) as 'Güncel' from Odalar where Doluluk= 1";
            komut55.Connection = yeni;
            SqlDataReader oku5 = komut55.ExecuteReader();

            while (oku5.Read())
            {
                aktifodasayisi = Convert.ToInt32(oku5["Güncel"].ToString());
            }

            label2.Text = aktifodasayisi.ToString() + " / " + toplamodasayisi.ToString();

            yeni.Close();

            circularProgressBar1.Value = aktifodasayisi;
            circularProgressBar1.Minimum = 0;
            circularProgressBar1.Maximum = toplamodasayisi;

            sonuc = (aktifodasayisi * 100) / toplamodasayisi;
            circularProgressBar1.Text = sonuc.ToString();

            //

            yeni.Close();
            yeni.Open();

            SqlCommand komut52 = new SqlCommand();
            komut52.CommandText = "select count(*) as 'aktifmusteri' from Musteri where not Oda_no=0";
            komut52.Connection = yeni;
            SqlDataReader oku2 = komut52.ExecuteReader();

            while (oku2.Read())
            {
                musterisayisi = Convert.ToInt32(oku2["aktifmusteri"].ToString());
            }

            label9.Text = musterisayisi.ToString();

            //

            yeni.Close();
            yeni.Open();

            SqlCommand komut4 = new SqlCommand();
            komut4.CommandText = "select count(*) as 'toplamtur' from Oda_Turleri ";
            komut4.Connection = yeni;
            SqlDataReader oku4 = komut4.ExecuteReader();

            while (oku4.Read())
            {
                odasayisi = Convert.ToInt32(oku4["toplamtur"].ToString());
            }

            for (int i = 0; i < odasayisi; i++)
            {
                yeni.Close();
                yeni.Open();
                string sorgu = "select Oda_Turu from Oda_Turleri order by Oda_Turu";
                SqlDataAdapter adp = new SqlDataAdapter(sorgu, yeni);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                string sorgu2 = "select count(*) as 'Sayı'  from Odalar where Oda_Turu= '" + ds.Tables[0].Rows[i][0] + "'";
                SqlDataAdapter adp2 = new SqlDataAdapter(sorgu2, yeni);
                DataSet ds2 = new DataSet();
                adp2.Fill(ds2);

                SqlCommand cmd2 = new SqlCommand("Select * from Oda_Turleri where Oda_Turu = '" + ds.Tables[0].Rows[i][0] + "'", yeni);
                SqlDataReader oku22 = cmd2.ExecuteReader();

                while (oku22.Read())
                {
                    cytk = Convert.ToInt32(oku22["ciftytk"].ToString());

                    tytk = Convert.ToInt32(oku22["tekytk"].ToString());
                }

                tursayisi = Convert.ToInt32(ds2.Tables[0].Rows[0][0]) * ((cytk * 2) + tytk);

                toplammusterisayisi += tursayisi;
            }
            label9.Text = musterisayisi.ToString() + " / " + toplammusterisayisi.ToString();

            yeni.Close();

            circularProgressBar2.Value = musterisayisi;
            circularProgressBar2.Minimum = 0;
            circularProgressBar2.Maximum = toplammusterisayisi;

            sonuc2 = (musterisayisi * 100) / toplammusterisayisi;
            circularProgressBar2.Text = sonuc2.ToString();

            textBox1.Text = DateTime.Now.ToLongDateString();
        }

        private void anasayfa_VisibleChanged(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();

            SqlCommand komut5 = new SqlCommand();
            komut5.CommandText = "select COUNT(*) as 'toplam' from Odalar";
            komut5.Connection = yeni;
            SqlDataReader oku = komut5.ExecuteReader();

            while (oku.Read())
            {
                toplamodasayisi = Convert.ToInt32(oku["toplam"].ToString());
            }

            yeni.Close();
            yeni.Open();

            SqlCommand komut55 = new SqlCommand();
            komut55.CommandText = "select COUNT(*) as 'Güncel' from Odalar where Doluluk= 1";
            komut55.Connection = yeni;
            SqlDataReader oku5 = komut55.ExecuteReader();

            while (oku5.Read())
            {
                aktifodasayisi = Convert.ToInt32(oku5["Güncel"].ToString());
            }

            label2.Text = aktifodasayisi.ToString() + " / " + toplamodasayisi.ToString();

            yeni.Close();

            circularProgressBar1.Value = aktifodasayisi;
            circularProgressBar1.Minimum = 0;
            circularProgressBar1.Maximum = toplamodasayisi;

            sonuc = (aktifodasayisi * 100) / toplamodasayisi;
            circularProgressBar1.Text = sonuc.ToString();

            //

            yeni.Close();
            yeni.Open();

            SqlCommand komut52 = new SqlCommand();
            komut52.CommandText = "select count(*) as 'aktifmusteri' from Musteri where not Oda_no=0";
            komut52.Connection = yeni;
            SqlDataReader oku2 = komut52.ExecuteReader();

            while (oku2.Read())
            {
                musterisayisi = Convert.ToInt32(oku2["aktifmusteri"].ToString());
            }

            label9.Text = musterisayisi.ToString();

            //

            yeni.Close();
            yeni.Open();

            SqlCommand komut4 = new SqlCommand();
            komut4.CommandText = "select count(*) as 'toplamtur' from Oda_Turleri ";
            komut4.Connection = yeni;
            SqlDataReader oku4 = komut4.ExecuteReader();

            while (oku4.Read())
            {
                odasayisi = Convert.ToInt32(oku4["toplamtur"].ToString());
            }

            toplammusterisayisi = 0;

            for (int i = 0; i < odasayisi; i++)
            {
                yeni.Close();
                yeni.Open();
                string sorgu = "select Oda_Turu from Oda_Turleri order by Oda_Turu";
                SqlDataAdapter adp = new SqlDataAdapter(sorgu, yeni);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                string sorgu2 = "select count(*) as 'Sayı'  from Odalar where Oda_Turu= '" + ds.Tables[0].Rows[i][0] + "'";
                SqlDataAdapter adp2 = new SqlDataAdapter(sorgu2, yeni);
                DataSet ds2 = new DataSet();
                adp2.Fill(ds2);

                SqlCommand cmd2 = new SqlCommand("Select * from Oda_Turleri where Oda_Turu = '" + ds.Tables[0].Rows[i][0] + "'", yeni);
                SqlDataReader oku22 = cmd2.ExecuteReader();

                while (oku22.Read())
                {
                    cytk = Convert.ToInt32(oku22["ciftytk"].ToString());

                    tytk = Convert.ToInt32(oku22["tekytk"].ToString());
                }

                tursayisi = Convert.ToInt32(ds2.Tables[0].Rows[0][0]) * ((cytk * 2) + tytk);

                toplammusterisayisi += tursayisi;
            }
            label9.Text = musterisayisi.ToString() + " / " + toplammusterisayisi.ToString();

            yeni.Close();

            circularProgressBar2.Value = musterisayisi;
            circularProgressBar2.Minimum = 0;
            circularProgressBar2.Maximum = toplammusterisayisi;

            sonuc2 = (musterisayisi * 100) / toplammusterisayisi;
            circularProgressBar2.Text = sonuc2.ToString();

            textBox1.Text = DateTime.Now.ToLongDateString();
        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {
        }

        private void textBox3_MouseEnter(object sender, EventArgs e)
        {
        }

        private void textBox3_MouseClick(object sender, MouseEventArgs e)
        {
            textBox3.SelectAll();
        }

        public static int faturasayisi;

        public static DateTime giris5;
        public static DateTime cikis5;
        public static string tur5;
        public static int fiy5;
        public static int tytk5;
        public static int cytk5;
        public static int toplamytk5;
        public static int odenen;
        public static int ekstra5;
        public static int odemesayisi;

        private faturabilgi ftrblg = new faturabilgi();

        private void button2_Click(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();

            SqlCommand komut5 = new SqlCommand("select count(*) from Hesap where Oda_No='" + textBox3.Text + "'", yeni);

            if (Convert.ToInt32(komut5.ExecuteScalar()) <= 0)

            {
                MessageBox.Show("Geçersiz Bir Oda Numarası Girdiniz.");

                foreach (Control item in this.Controls)
                {
                    if (item is TextBox)
                    {
                        TextBox tbox = (TextBox)item;
                        tbox.Clear();
                    }
                }
            }
            else
            {
                yeni.Close();
                yeni.Open();

                SqlCommand cmd = new SqlCommand("Select * from Hesap where Oda_No = '" + textBox3.Text + "'  and Durum = 1 ", yeni);
                SqlDataReader oku = cmd.ExecuteReader();

                while (oku.Read())
                {
                    giris5 = Convert.ToDateTime(oku["Giris_Tarihi"].ToString());
                    cikis5 = Convert.ToDateTime(oku["Cikis_Tarihi"].ToString());
                }

                ftrblg.label1.Text = "Oda " + textBox3.Text;
                ftrblg.dateTimePicker1.Value = giris5;
                ftrblg.dateTimePicker2.Value = cikis5;

                yeni.Close();
                yeni.Open();

                SqlCommand cmd2 = new SqlCommand("Select * from Odalar as o left join Oda_Turleri as ot on o.Oda_Turu=ot.Oda_Turu where o.Oda_No = '" + textBox3.Text + "'", yeni);
                SqlDataReader oku2 = cmd2.ExecuteReader();

                while (oku2.Read())
                {
                    tur5 = oku2["Oda_Turu"].ToString();
                    fiy5 = Convert.ToInt32(oku2["fiyat"].ToString());
                    cytk5 = Convert.ToInt32(oku2["ciftytk"].ToString());

                    tytk5 = Convert.ToInt32(oku2["tekytk"].ToString());
                }

                toplamytk5 = 2 * cytk5 + tytk5;
                ftrblg.label10.Text = tur5;
                ftrblg.label13.Text = fiy5.ToString() + " TL";
                ftrblg.label2.Text = Convert.ToString(fiy5 * Convert.ToInt32(ftrblg.label8.Text) + " TL");

                yeni.Close();
                yeni.Open();

                SqlCommand komut90 = new SqlCommand();
                komut90.CommandText = "Select  Musteri_no as 'Müşteri No' , ad+' '+Soyad as 'Ad Soyad' from Musteri where Oda_no = " + textBox3.Text + " ";
                komut90.Connection = yeni;
                SqlDataReader oku200 = komut90.ExecuteReader();
                DataTable tablo200 = new DataTable();
                tablo200.Load(oku200);
                ftrblg.dataGridView1.DataSource = tablo200;
                ftrblg.dataGridView1.AllowUserToAddRows = false;

                string sorgu = "Select * from Musteri where Oda_No = '" + textBox3.Text + "'";
                SqlDataAdapter adp = new SqlDataAdapter(sorgu, yeni);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                ftrblg.label21.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                ftrblg.label23.Text = toplamytk5.ToString();
                ftrblg.dataGridView1.ClearSelection();

                SqlCommand komut50 = new SqlCommand();
                komut50.CommandText = "Select  Musteri_no as 'Müş No' , Odeme_Tutari as 'Ödeme Tutarı (TL)', Odeme_Tarihi as 'Ödeme Tarihi', Oda_Giris from Fatura where Oda_no = " + textBox3.Text + "  ";
                komut50.Connection = yeni;
                SqlDataReader oku50 = komut50.ExecuteReader();
                DataTable tablo50 = new DataTable();
                tablo50.Load(oku50);
                ftrblg.dataGridView2.DataSource = tablo50;
                ftrblg.dataGridView2.Columns[3].Visible = false;
                ftrblg.dataGridView2.AllowUserToAddRows = false;

                SqlCommand komut22 = new SqlCommand();
                komut22.CommandText = "Select  Ad,Soyad from Musteri where Oda_no = " + textBox3.Text + "";
                komut22.Connection = yeni;
                ftrblg.comboBox1.Items.Clear();
                ftrblg.comboBox1.Items.Add("Müşteri Seçiniz");
                SqlDataReader isimver;
                isimver = komut22.ExecuteReader();
                while (isimver.Read())
                {
                    ftrblg.comboBox1.Items.Add(isimver["Ad"] + " " + isimver["Soyad"]);
                }
                ftrblg.comboBox1.SelectedIndex = 0;
                ftrblg.comboBox2.SelectedIndex = 0;

                //select sum(Odeme_Tutari) from Fatura where Oda_no=116

                yeni.Close();
                yeni.Open();

                SqlCommand komut16 = new SqlCommand();
                komut16.CommandText = "select count(Toplam) as sayi6 from Ekstra where Oda_no=" + textBox3.Text + " ";
                komut16.Connection = yeni;
                SqlDataReader oku16 = komut16.ExecuteReader();

                while (oku16.Read())
                {
                    odemesayisi = Convert.ToInt32(oku16["sayi6"].ToString());
                }

                if (odemesayisi > 0)
                {
                    yeni.Close();
                    yeni.Open();
                    string sorgu2 = "select sum(Toplam) from Ekstra where Oda_no=" + textBox3.Text + " ";
                    SqlDataAdapter adp22 = new SqlDataAdapter(sorgu2, yeni);
                    DataSet ds2 = new DataSet();
                    adp22.Fill(ds2);

                    odenen = Convert.ToInt32(ds2.Tables[0].Rows[0][0].ToString());
                    ftrblg.label11.Text = ds2.Tables[0].Rows[0][0].ToString() + " TL";
                }
                else
                {
                    ftrblg.label11.Text = "0 TL";
                }

                ftrblg.label17.Text = Convert.ToString((Convert.ToInt32(ftrblg.label2.Text.Replace("TL", ""))) + (Convert.ToInt32(ftrblg.label11.Text.Replace("TL", ""))) + " TL");

                yeni.Close();
                yeni.Open();

                SqlCommand komut15 = new SqlCommand();
                komut15.CommandText = "select count(Odeme_Tutari) as sayi5 from Fatura where Oda_no=" + textBox3.Text + " ";
                komut15.Connection = yeni;
                SqlDataReader oku15 = komut15.ExecuteReader();

                while (oku15.Read())
                {
                    odemesayisi = Convert.ToInt32(oku15["sayi5"].ToString());
                }

                if (odemesayisi > 0)
                {
                    yeni.Close();
                    yeni.Open();
                    string sorgu2 = "select sum(Odeme_Tutari) from Fatura where Oda_no=" + textBox3.Text + " ";
                    SqlDataAdapter adp22 = new SqlDataAdapter(sorgu2, yeni);
                    DataSet ds2 = new DataSet();
                    adp22.Fill(ds2);

                    odenen = Convert.ToInt32(ds2.Tables[0].Rows[0][0].ToString());
                    ftrblg.label26.Text = ds2.Tables[0].Rows[0][0].ToString() + " TL";
                    ftrblg.label19.Text = Convert.ToString((Convert.ToInt32(ftrblg.label17.Text.Replace("TL", ""))) - (Convert.ToInt32(ds2.Tables[0].Rows[0][0].ToString()))) + " TL";
                }
                else
                {
                    ftrblg.label26.Text = "0 TL";
                    ftrblg.label19.Text = ftrblg.label17.Text;
                }

                if (ftrblg.label19.Text == "0 TL")
                {
                    ftrblg.checkBox1.Checked = true;
                    ftrblg.checkBox1.Text = "Ödeme Tamamlandı";
                }
                else
                {
                    ftrblg.checkBox1.Checked = false;
                    ftrblg.checkBox1.Text = "Ödeme Tamamlanmadı";
                }

                if (!Baslangic.Instance.pnlcontainer.Controls.ContainsKey("faturabilgi"))
                {
                    ftrblg.Dock = DockStyle.Fill;
                    Baslangic.Instance.pnlcontainer.Controls.Add(ftrblg);
                }
                Baslangic.Instance.pnlcontainer.Controls["faturabilgi"].BringToFront();
                Baslangic.Instance.pnlcontainer.Controls["faturabilgi"].Show();

                this.Hide();

                yeni.Close();
            }
        }

        private Yenirez yrez = new Yenirez();

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Baslangic.Instance.pnlcontainer.Controls.ContainsKey("Yenirez"))
            {
                yrez.Dock = DockStyle.Fill;
                Baslangic.Instance.pnlcontainer.Controls.Add(yrez);
            }
            Baslangic.Instance.pnlcontainer.Controls["Yenirez"].Hide();
            Baslangic.Instance.pnlcontainer.Controls["Yenirez"].BringToFront();
            Baslangic.Instance.pnlcontainer.Controls["Yenirez"].Show();

            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            musekle mekle = new musekle();
            mekle.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();

            SqlCommand komut = new SqlCommand();
            komut.CommandText = "SELECT TOP 1 Not_icerik FROM Notlar ORDER BY not_tarih DESC";
            komut.Connection = yeni;
            SqlDataReader oku = komut.ExecuteReader();

            while (oku.Read())
            {
                label3.Text = oku["Not_icerik"].ToString();
            }

            yeni.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            try
            {
                yeni.Close();
                yeni.Open();

                SqlCommand komut = new SqlCommand();
                komut.CommandText = "SELECT m.Ad + ' ' + m.Soyad AS 'AdSoyad' FROM Hesap AS h LEFT JOIN Musteri AS m ON h.Musteri_no = m.Musteri_no WHERE h.Durum = 0 AND h.Giris_Tarihi <= GETDATE() AND h.Cikis_Tarihi >= GETDATE() ORDER BY h.Giris_Tarihi, h.Oda_No";
                komut.Connection = yeni;
                SqlDataReader oku = komut.ExecuteReader();

                // label4'ü temizle
                label4.Text = "";

                while (oku.Read())
                {
                    // Her rezervasyonu label4'e ekleyerek göster
                    label4.Text += $"{oku["AdSoyad"]}\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
            finally
            {
                yeni.Close();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            try
            {
                yeni.Close();
                yeni.Open();

                SqlCommand komut = new SqlCommand();
                komut.CommandText = "SELECT m.Ad + ' ' + m.Soyad AS 'AdSoyad' FROM Hesap AS h LEFT JOIN Musteri AS m ON h.Musteri_no = m.Musteri_no WHERE h.Durum = 0 AND h.Cikis_Tarihi = CAST(GETDATE() AS DATE) ORDER BY h.Giris_Tarihi, h.Oda_No";
                komut.Connection = yeni;
                SqlDataReader oku = komut.ExecuteReader();

                // label5'i temizle
                label5.Text = "";

                while (oku.Read())
                {
                    // Her müşteriyi label5'e ekleyerek göster
                    label5.Text += $"{oku["AdSoyad"]}\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
            finally
            {
                yeni.Close();
            }
        }

    }
}