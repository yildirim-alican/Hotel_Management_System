using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Otel
{
    public partial class musduzenle : UserControl
    {
        public musduzenle()
        {
            InitializeComponent();
        }

        private SqlConnection yeni = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");

        private void button1_Click(object sender, EventArgs e)
        {
            yeni.Open();
            string komut = "UPDATE Musteri SET Ad = '" + dznad.Text + "' ,Kimlik_seri_No= '" + textBox12.Text + "' , anne= '" + textBox10.Text + "', baba = '" + textBox4.Text + "', adres= '" + richTextBox1.Text + "', Soyad = '" + dznsoyad.Text + "', Cinsiyet = '" + dzncmbcns.Text + "', Dogum_tarihi = '" + maskedTextBox2.Text + "', Medeni_Hal = '" + dznmdnhlcmbx.Text + "', Telefon_no = '" + maskedTextBox1.Text + "', E_Posta = '" + dznep.Text + "', Kimlik_no = '" + dzntc.Text + "' where Musteri_no = '" + label10.Text + "'";
            SqlCommand kmt = new SqlCommand(komut, yeni);
            kmt.ExecuteNonQuery();
            yeni.Close();

            Musyonet msyn = new Musyonet();

            if (!Baslangic.Instance.pnlcontainer.Controls.ContainsKey("Musyonet"))
            {
                msyn.Dock = DockStyle.Fill;
                Baslangic.Instance.pnlcontainer.Controls.Add(msyn);
            }

            Baslangic.Instance.pnlcontainer.Controls["Musyonet"].Show();
            Baslangic.Instance.pnlcontainer.Controls["Musyonet"].BringToFront();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Musyonet msyn = new Musyonet();

            if (!Baslangic.Instance.pnlcontainer.Controls.ContainsKey("Musyonet"))
            {
                msyn.Dock = DockStyle.Fill;
                Baslangic.Instance.pnlcontainer.Controls.Add(msyn);
            }
            Baslangic.Instance.pnlcontainer.Controls["Musyonet"].Show();
            Baslangic.Instance.pnlcontainer.Controls["Musyonet"].BringToFront();
        }
    }
}