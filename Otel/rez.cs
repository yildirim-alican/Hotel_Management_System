using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Otel
{
    public partial class rez : UserControl
    {
        public rez()
        {
            InitializeComponent();
        }

        private SqlConnection yeni = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");

        private void rez_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();

            SqlCommand komut = new SqlCommand();
            komut.CommandText = "insert into Musteri(Ad,Soyad,Cinsiyet,Medeni_Hal,Telefon_no,E_posta,Kimlik_no,Dogum_tarihi,Kimlik_seri_no,anne,baba,adres) values(@dAd,@dSoyad,@dCinsiyet,@dMedeni_Hal,@dTelefon_no,@dE_posta,@dKimlik_no,@dDogum_tarihi,@dKimlik_seri_no,@danne,@dbaba,@dadres) ";
            komut.Connection = yeni;

            SqlParameter dAd = new SqlParameter();
            dAd.ParameterName = "@dAd";
            dAd.SqlDbType = SqlDbType.VarChar;
            dAd.Size = 50;
            dAd.Value = textBox1.Text;
            komut.Parameters.Add(dAd);

            SqlParameter dSoyad = new SqlParameter();
            dSoyad.ParameterName = "@dSoyad";
            dSoyad.SqlDbType = SqlDbType.VarChar;
            dSoyad.Size = 50;
            dSoyad.Value = textBox2.Text;
            komut.Parameters.Add(dSoyad);

            SqlParameter dCinsiyet = new SqlParameter();
            dCinsiyet.ParameterName = "@dCinsiyet";
            dCinsiyet.SqlDbType = SqlDbType.VarChar;
            dCinsiyet.Size = 50;
            dCinsiyet.Value = rezcns.Text;
            komut.Parameters.Add(dCinsiyet);

            SqlParameter dMedeni_Hal = new SqlParameter();
            dMedeni_Hal.ParameterName = "@dMedeni_Hal";
            dMedeni_Hal.SqlDbType = SqlDbType.VarChar;
            dMedeni_Hal.Size = 50;
            dMedeni_Hal.Value = comboBox2.Text;
            komut.Parameters.Add(dMedeni_Hal);

            SqlParameter dTelefon_no = new SqlParameter();
            dTelefon_no.ParameterName = "@dTelefon_no";
            dTelefon_no.SqlDbType = SqlDbType.VarChar;
            dTelefon_no.Size = 50;
            dTelefon_no.Value = maskedTextBox1.Text;
            komut.Parameters.Add(dTelefon_no);

            SqlParameter dE_posta = new SqlParameter();
            dE_posta.ParameterName = "@dE_posta";
            dE_posta.SqlDbType = SqlDbType.VarChar;
            dE_posta.Size = 50;
            dE_posta.Value = textBox8.Text;
            komut.Parameters.Add(dE_posta);

            SqlParameter dKimlik_no = new SqlParameter();
            dKimlik_no.ParameterName = "@dKimlik_no";
            dKimlik_no.SqlDbType = SqlDbType.VarChar;
            dKimlik_no.Size = 50;
            dKimlik_no.Value = textBox11.Text;
            komut.Parameters.Add(dKimlik_no);

            SqlParameter dDogum_tarihi = new SqlParameter();
            dDogum_tarihi.ParameterName = "@dDogum_tarihi";
            dDogum_tarihi.SqlDbType = SqlDbType.VarChar;
            dDogum_tarihi.Size = 50;
            dDogum_tarihi.Value = maskedTextBox2.Text;
            komut.Parameters.Add(dDogum_tarihi);

            SqlParameter dKimlik_seri_no = new SqlParameter();
            dKimlik_seri_no.ParameterName = "@dKimlik_seri_no";
            dKimlik_seri_no.SqlDbType = SqlDbType.VarChar;
            dKimlik_seri_no.Size = 50;
            dKimlik_seri_no.Value = textBox12.Text;
            komut.Parameters.Add(dKimlik_seri_no);

            SqlParameter danne = new SqlParameter();
            danne.ParameterName = "@danne";
            danne.SqlDbType = SqlDbType.VarChar;
            danne.Size = 50;
            danne.Value = textBox10.Text;
            komut.Parameters.Add(danne);

            SqlParameter dbaba = new SqlParameter();
            dbaba.ParameterName = "@dbaba";
            dbaba.SqlDbType = SqlDbType.VarChar;
            dbaba.Size = 50;
            dbaba.Value = textBox4.Text;
            komut.Parameters.Add(dbaba);

            SqlParameter dadres = new SqlParameter();
            dadres.ParameterName = "@dadres";
            dadres.SqlDbType = SqlDbType.VarChar;
            dadres.Size = 50;
            dadres.Value = richTextBox1.Text;
            komut.Parameters.Add(dadres);

            komut.ExecuteNonQuery();

            yeni.Close();
            yeni.Open();

            SqlCommand komut8 = new SqlCommand();
            komut8.CommandText = "insert into Logtb(islem,kullanici,aciklama,islemtarihi) values('Müşteri Kayıt','" + hesap.yhesap + "',@dacik,'" + Convert.ToDateTime(DateTime.Now.ToShortDateString()).ToString("MM/dd/yyyy HH:mm:ss") + "') ";
            komut8.Connection = yeni;

            SqlParameter dacik = new SqlParameter();
            dacik.ParameterName = "@dacik";
            dacik.SqlDbType = SqlDbType.VarChar;
            dacik.Size = 50;
            dacik.Value = textBox1.Text + " " + textBox2.Text + " Kişisinin Kayıdı alındı ";
            komut8.Parameters.Add(dacik);

            komut8.ExecuteNonQuery();

            {
                textBox1.Clear();
                textBox2.Clear();
                maskedTextBox1.Clear();
                textBox11.Clear();
                maskedTextBox2.Clear();
                textBox8.Clear();
                textBox12.Clear();
                textBox10.Clear();
                textBox4.Clear();
                richTextBox1.Clear();

                MessageBox.Show(" Kayıt başarıyla eklendi");
            }

            yeni.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
                && !char.IsSeparator(e.KeyChar);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
                && !char.IsSeparator(e.KeyChar);
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
             && !char.IsSeparator(e.KeyChar);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar)
             && !char.IsSeparator(e.KeyChar);
        }
    }
}