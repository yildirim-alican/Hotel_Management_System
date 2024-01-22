using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Otel
{
    public partial class Musyonet : UserControl
    {
        public Musyonet()
        {
            InitializeComponent();
        }

        private SqlConnection yeni = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");
        private musduzenle musdzn = new musduzenle();

        public static string Musteri_no;
        public static string Ad;
        public static string Soyad;
        public static string Cinsiyet;
        public static string Dogum_tarihi;
        public static string Medeni_Hal;
        public static string Telefon_no;
        public static string E_Posta;
        public static string Kimlik_no;

        public static string Kimlik_seri_No;
        public static string anne;
        public static string baba;
        public static string adres;
        public static string Oda_no;

        private void dznbtn_Click(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();
            int yer = Int32.Parse(dataGridView1.CurrentRow.Cells[5].Value.ToString());

            SqlCommand cmd = new SqlCommand("Select * from Musteri where Musteri_no = '" + yer + "'", yeni);
            SqlDataReader oku = cmd.ExecuteReader();

            while (oku.Read())
            {
                Kimlik_no = oku["Kimlik_no"].ToString();
                Musteri_no = oku["Musteri_no"].ToString();
                Ad = oku["Ad"].ToString();
                Soyad = oku["Soyad"].ToString();
                Cinsiyet = oku["Cinsiyet"].ToString();
                Dogum_tarihi = oku["Dogum_tarihi"].ToString();
                Medeni_Hal = oku["Medeni_Hal"].ToString();
                Telefon_no = oku["Telefon_no"].ToString();
                E_Posta = oku["E_Posta"].ToString();

                Kimlik_seri_No = oku["Kimlik_seri_No"].ToString();
                anne = oku["anne"].ToString();
                baba = oku["baba"].ToString();
                adres = oku["adres"].ToString();
                Oda_no = oku["Oda_no"].ToString();
            }

            musdzn.dznad.Text = Ad;
            musdzn.dznsoyad.Text = Soyad;
            musdzn.dzncmbcns.Text = Cinsiyet;
            musdzn.maskedTextBox2.Text = Dogum_tarihi;
            musdzn.dznmdnhlcmbx.Text = Medeni_Hal;
            musdzn.maskedTextBox1.Text = Telefon_no;
            musdzn.dznep.Text = E_Posta;
            musdzn.dzntc.Text = Kimlik_no;
            musdzn.label10.Text = Musteri_no;

            musdzn.textBox12.Text = Kimlik_seri_No;
            musdzn.textBox10.Text = anne;
            musdzn.textBox4.Text = baba;
            musdzn.richTextBox1.Text = adres;

            yeni.Close();
            yeni.Open();
            SqlCommand komut9 = new SqlCommand();
            komut9.CommandText = "Select islem_no as 'Kayıt Numarası', Musteri_no as 'Müşteri No',Giris_Tarihi as 'Giriş Tarihi', Cikis_Tarihi as ' Çıkış Tarihi', Oda_No as 'Oda No' from Hesap where Musteri_no = " + yer + " ORDER BY islem_no DESC ";
            komut9.Connection = yeni;
            SqlDataReader oku2 = komut9.ExecuteReader();
            DataTable tablo2 = new DataTable();
            tablo2.Load(oku2); musdzn.dataGridView1.DataSource = tablo2;
            musdzn.dataGridView1.AllowUserToAddRows = false;
            musdzn.dataGridView1.Columns[1].Visible = false;

            if (!Baslangic.Instance.pnlcontainer.Controls.ContainsKey("musduzenle"))
            {
                musdzn.Dock = DockStyle.Fill;
                Baslangic.Instance.pnlcontainer.Controls.Add(musdzn);
            }
            Baslangic.Instance.pnlcontainer.Controls["musduzenle"].BringToFront();

            this.Hide();

            yeni.Close();
        }

        public class genel
        {
            public static string secim;
        }

        private void tablodoldur()
        {
            yeni.Close();
            yeni.Open();

            SqlCommand komut = new SqlCommand();
            komut.CommandText = "Select Kimlik_No as 'Kimlik Numarası', Ad,Soyad, Cinsiyet, Telefon_no as ' Telefon Numarası', Musteri_no as 'Müşteri Numarası' from Musteri ORDER BY Musteri_no DESC ";
            komut.Connection = yeni;
            SqlDataReader oku = komut.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku); dataGridView1.DataSource = tablo;
            dataGridView1.AllowUserToAddRows = false;

            yeni.Close();
        }

        private void Musyonet_Load(object sender, EventArgs e)
        {
            tablodoldur();

            comboBox1.SelectedIndex = 0;
        }

        private void Musyonet_VisibleChanged(object sender, EventArgs e)
        {
            tablodoldur();
            textBox1.Text = "";
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
                komut.CommandText = "Select Kimlik_No as 'Kimlik Numarası', Ad ,Soyad, Cinsiyet, Telefon_no as ' Telefon Numarası', Musteri_no as 'Müşteri Numarası' from Musteri where " + genel.secim + " Like '%" + dgr + "%'  ORDER BY Musteri_no DESC ";
                komut.Connection = yeni;
                SqlDataReader oku = komut.ExecuteReader();
                DataTable tablo = new DataTable();
                tablo.Load(oku); dataGridView1.DataSource = tablo;
                dataGridView1.AllowUserToAddRows = false;

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
                    komut.CommandText = "Select Kimlik_No as 'Kimlik Numarası', Ad ,Soyad, Cinsiyet, Telefon_no as ' Telefon Numarası', Musteri_no as 'Müşteri Numarası' from Musteri where " + genel.secim + " Like '%" + dgr + "%'  ORDER BY Musteri_no DESC ";
                    komut.Connection = yeni;
                    SqlDataReader oku = komut.ExecuteReader();
                    DataTable tablo = new DataTable();
                    tablo.Load(oku); dataGridView1.DataSource = tablo;
                    dataGridView1.AllowUserToAddRows = false;

                    yeni.Close();
                }
            }
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
    }
}