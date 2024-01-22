using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Otel
{
    public partial class loglar : UserControl
    {
        public loglar()
        {
            InitializeComponent();
        }

        private SqlConnection yeni = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");

        private void button1_Click(object sender, EventArgs e)
        {
            notlar ntlr = new notlar();
            if (!Baslangic.Instance.pnlcontainer.Controls.ContainsKey("notlar"))
            {
                ntlr.Dock = DockStyle.Fill;
                Baslangic.Instance.pnlcontainer.Controls.Add(ntlr);
            }
            Baslangic.Instance.pnlcontainer.Controls["notlar"].BringToFront();
            Baslangic.Instance.pnlcontainer.Controls["notlar"].Show();

            this.Hide();
        }

        private void groupBox1_VisibleChanged(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();
            SqlCommand komut2 = new SqlCommand();
            komut2.CommandText = "Select log_no as 'İşlem Numarası', kullanici as 'Yetkili' ,islem as 'Yapılan İşlem',islemtarihi as 'İşlem Tarihi'  from Logtb";
            komut2.Connection = yeni;
            SqlDataReader oku2 = komut2.ExecuteReader();
            DataTable tablo2 = new DataTable();
            tablo2.Load(oku2); dataGridView1.DataSource = tablo2;
            dataGridView1.AllowUserToAddRows = false;
            yeni.Close();
        }

        private void loglar_Load(object sender, EventArgs e)
        {
        }

        public static int yer;
        public static string icerik;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            yeni.Close();
            yeni.Open();
            yer = Int32.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());

            SqlCommand cmd2 = new SqlCommand("select * from Logtb where log_no= '" + yer + "'", yeni);
            SqlDataReader oku2 = cmd2.ExecuteReader();

            while (oku2.Read())
            {
                icerik = (oku2["aciklama"].ToString());
            }

            richTextBox2.Text = icerik;
            yeni.Close();
        }
    }
}