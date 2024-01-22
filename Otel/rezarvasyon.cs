using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Otel
{
    public partial class rezarvasyon : UserControl
    {
        public rezarvasyon()
        {
            InitializeComponent();
        }

        private SqlConnection yeni = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");

        private Yenirez yrez = new Yenirez();
        public static DateTime gTarih;

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

        private void rezarvasyon_VisibleChanged(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();
            gTarih = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            SqlCommand komut = new SqlCommand();
            komut.CommandText = "select m.Musteri_no as 'Müş No',m.Ad+' '+Soyad as 'Ad Soyad',h.Giris_Tarihi as 'Giriş Tarihi', h.Cikis_Tarihi as 'Çıkış Tarihi',h.Oda_No as 'Oda No' from Hesap as h left join Musteri as m on h.Musteri_no=m.Musteri_no where h.Durum=0 order by Giris_Tarihi,h.Oda_No  ";
            komut.Connection = yeni;
            SqlDataReader oku = komut.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku); dataGridView2.DataSource = tablo;
            dataGridView2.AllowUserToAddRows = false;

            SqlCommand komut2 = new SqlCommand();
            komut2.CommandText = "select Mus_no as 'Müş No', AdSoyad as 'Ad Soyad', Giris as 'Giriş Tarihi', Cikis as 'Çıkış Tarihi', Oda_No as  'Oda No'  from Reziptal   order by Giris,Oda_No ";
            komut2.Connection = yeni;
            SqlDataReader oku2 = komut2.ExecuteReader();
            DataTable tablo2 = new DataTable();
            tablo2.Load(oku2); dataGridView1.DataSource = tablo2;
            dataGridView1.AllowUserToAddRows = false;

            SqlCommand komut20 = new SqlCommand();
            komut20.CommandText = "INSERT INTO Reziptal Select m.Musteri_no ,(m.Ad + ' ' + Soyad),h.Giris_Tarihi , h.Cikis_Tarihi,h.Oda_No FROM Hesap as h LEFT JOIN  Musteri as m on h.Musteri_no = m.Musteri_no where Giris_Tarihi<'" + gTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and Durum = 0 ";
            komut20.Connection = yeni;
            komut20.ExecuteNonQuery();

            SqlCommand komut7 = new SqlCommand();
            komut7.CommandText = "delete  from Hesap where Giris_Tarihi<'" + gTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and Durum=0 ";
            komut7.Connection = yeni;
            komut7.ExecuteNonQuery();

            groupBox2.Hide();
            groupBox1.Height = 455;

            button4.Location = new Point(button4.Location.X, 525);
            button2.Location = new Point(button2.Location.X, 525);
            button3.Location = new Point(button3.Location.X, 525);
            button4.Text = "İptal Olmuş Rezervasyonları Görüntüle";
            dm = 0;
            yer0 = 0;
        }

        public static int dm = 0;

        private void button4_Click(object sender, EventArgs e)
        {
            if (dm == 0)
            {
                groupBox2.Show();
                groupBox1.Height = 300;

                button4.Location = new Point(button4.Location.X, 373);
                button2.Location = new Point(button2.Location.X, 373);
                button3.Location = new Point(button3.Location.X, 373);
                button4.Text = "İptal Olmuş Rezervasyonları Gizle";
                dm = 1;
            }
            else
            {
                groupBox2.Hide();
                groupBox1.Height = 455;

                button4.Location = new Point(button4.Location.X, 525);
                button2.Location = new Point(button2.Location.X, 525);
                button3.Location = new Point(button3.Location.X, 525);
                button4.Text = "İptal Olmuş Rezervasyonları Görüntüle";
                dm = 0;
            }
            yeni.Close();
        }

        public static int yer0;
        public static string yer1;
        public static DateTime yer2;
        public static DateTime yer3;
        public static int yer4;

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            yer0 = Int32.Parse(dataGridView2.CurrentRow.Cells[0].Value.ToString());
            yer1 = dataGridView2.CurrentRow.Cells[1].Value.ToString();
            yer2 = DateTime.Parse(dataGridView2.CurrentRow.Cells[2].Value.ToString());
            yer3 = DateTime.Parse(dataGridView2.CurrentRow.Cells[3].Value.ToString());
            yer4 = Int32.Parse(dataGridView2.CurrentRow.Cells[4].Value.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();

            if (yer0 == 0)
            {
                MessageBox.Show("Rezervasyon Seçilmedi");
            }
            else
            {
                SqlCommand komut5 = new SqlCommand();
                komut5.CommandText = "insert into Reziptal(Mus_no,AdSoyad,Giris,Cikis,Oda_No) values(" + yer0 + ",'" + yer1 + "','" + yer2.ToString("MM/dd/yyyy") + "','" + yer3.ToString("MM/dd/yyyy") + "'," + yer4 + ") ";
                komut5.Connection = yeni;
                komut5.ExecuteNonQuery();

                SqlCommand komut6 = new SqlCommand();
                komut6.CommandText = "delete from Hesap where Musteri_no=" + yer0 + " and Giris_Tarihi='" + yer2.ToString("MM/dd/yyyy") + "' and Cikis_Tarihi='" + yer3.ToString("MM/dd/yyyy") + "' and Oda_No=" + yer4 + "  ";
                komut6.Connection = yeni;
                komut6.ExecuteNonQuery();

                SqlCommand komut8 = new SqlCommand();
                komut8.CommandText = "select m.Musteri_no as 'Müş No',m.Ad+' '+Soyad as 'Ad Soyad',h.Giris_Tarihi as 'Giriş Tarihi', h.Cikis_Tarihi as 'Çıkış Tarihi',h.Oda_No as 'Oda No' from Hesap as h left join Musteri as m on h.Musteri_no=m.Musteri_no where h.Durum=0 order by Giris_Tarihi,h.Oda_No  ";
                komut8.Connection = yeni;
                SqlDataReader oku8 = komut8.ExecuteReader();
                DataTable tablo8 = new DataTable();
                tablo8.Load(oku8); dataGridView2.DataSource = tablo8;
                dataGridView2.AllowUserToAddRows = false;

                SqlCommand komut2 = new SqlCommand();
                komut2.CommandText = "select Mus_no as 'Müş No', AdSoyad as 'Ad Soyad', Giris as 'Giriş Tarihi', Cikis as 'Çıkış Tarihi', Oda_No as  'Oda No'  from Reziptal   order by Giris,Oda_No ";
                komut2.Connection = yeni;
                SqlDataReader oku2 = komut2.ExecuteReader();
                DataTable tablo2 = new DataTable();
                tablo2.Load(oku2); dataGridView1.DataSource = tablo2;
                dataGridView1.AllowUserToAddRows = false;

                yer0 = 0;
            }
        }
    }
}