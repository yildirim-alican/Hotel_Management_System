using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Otel
{
    public partial class Giriş : Form
    {
        public Giriş()
        {
            InitializeComponent();
        }

        private SqlConnection yeni = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");

        public static string icerik;
        public static string isim;

        private void button1_Click(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();

            SqlCommand komut5 = new SqlCommand("select count(*) from Yetkili where Kullanici_adi='" + textBox1.Text + "'", yeni);

            if (Convert.ToInt32(komut5.ExecuteScalar()) <= 0)

            {
                MessageBox.Show("Geçersiz Yada Eksik Bir Kullanıcı Adı Girdiniz.");

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
                SqlCommand cmd2 = new SqlCommand("Select * from Yetkili where Kullanici_adi = '" + textBox1.Text + "'", yeni);
                SqlDataReader oku2 = cmd2.ExecuteReader();

                while (oku2.Read())
                {
                    icerik = (oku2["Sifre"].ToString());
                    isim = (oku2["Yetkili_isim"].ToString());
                }

                if (icerik == textBox2.Text)
                {
                    hesap.yhesap = isim;
                    Baslangic menu = new Baslangic();
                    menu.Show();
                    this.Hide();

                    yeni.Close();
                    yeni.Open();
                    SqlCommand komut8 = new SqlCommand();
                    komut8.CommandText = "insert into Logtb(islem,kullanici,aciklama,islemtarihi) values('Sisteme Giriş','" + hesap.yhesap + "',@dacik,'" + Convert.ToDateTime(DateTime.Now.ToShortDateString()).ToString("MM/dd/yyyy HH:mm:ss") + "') ";
                    komut8.Connection = yeni;

                    SqlParameter dacik = new SqlParameter();
                    dacik.ParameterName = "@dacik";
                    dacik.SqlDbType = SqlDbType.VarChar;
                    dacik.Size = 50;
                    dacik.Value = hesap.yhesap + " Sisteme Giriş Yaptı ";
                    komut8.Parameters.Add(dacik);

                    komut8.ExecuteNonQuery();
                    yeni.Close();
                }
                else
                {
                    MessageBox.Show("Yanlış Yada Eksik Şifre Girdiniz");
                }
            }
        }

        private void Giriş_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}