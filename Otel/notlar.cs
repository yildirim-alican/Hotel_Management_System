using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Otel
{
    public partial class notlar : UserControl
    {
        public notlar()
        {
            InitializeComponent();
        }

        private SqlConnection yeni = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox2.Enabled = true;
            button4.Text = "Kaydet";
            button3.Text = "İptal";

            try
            {
                yeni.Close();
                yeni.Open();

                SqlCommand komut = new SqlCommand();
                komut.CommandText = "UPDATE Notlar SET Not_icerik = @NotIcerik WHERE Not_No = @NotNo";
                komut.Parameters.AddWithValue("@NotIcerik", richTextBox2.Text);
                komut.Parameters.AddWithValue("@NotNo", yer);
                komut.Connection = yeni;
                komut.ExecuteNonQuery();

                MessageBox.Show("Not başarıyla güncellendi");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
            finally
            {
                yeni.Close();
                richTextBox2.Enabled = true;
                button4.Text = "Düzenle";
                button3.Text = "Notu Sil";
            }
        }

        private void notlar_Load(object sender, EventArgs e)
        {
        }

        private void notlar_VisibleChanged(object sender, EventArgs e)
        {
            richTextBox2.Enabled = false;
            button4.Text = "Düzenle";
            button3.Text = "Notu Sil";

            yeni.Close();
            yeni.Open();
            SqlCommand komut = new SqlCommand();
            komut.CommandText = "select Not_No as 'Not Numarası', Baslik as 'Başlık',not_tarih as 'Oluşturma Tarihi' from Notlar order by not_tarih";
            komut.Connection = yeni;
            SqlDataReader oku = komut.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku); dataGridView1.DataSource = tablo;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();

            SqlCommand komut = new SqlCommand();
            komut.CommandText = "insert into Notlar(Baslik,Gosterim,not_tarih,Not_icerik) values('" + textBox1.Text + "','" + dateTimePicker1.Value.ToString("MM/dd/yyyy HH:mm:ss") + "','" + Convert.ToDateTime(DateTime.Now.ToShortDateString()).ToString("MM/dd/yyyy HH:mm:ss") + "','" + richTextBox1.Text + "') ";
            komut.Connection = yeni;

            komut.ExecuteNonQuery();

            {
                textBox1.Clear();

                MessageBox.Show(" Not başarıyla eklendi");
            }

            yeni.Close();

            yeni.Open();
            SqlCommand komut2 = new SqlCommand();
            komut2.CommandText = "select Not_No as 'Not Numarası', Baslik as 'Başlık',not_tarih as 'Oluşturma Tarihi' from Notlar order by not_tarih";
            komut2.Connection = yeni;
            SqlDataReader oku2 = komut2.ExecuteReader();
            DataTable tablo2 = new DataTable();
            tablo2.Load(oku2); dataGridView1.DataSource = tablo2;
            dataGridView1.AllowUserToAddRows = false;
        }

        public static int yer;
        public static string icerik;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            yeni.Close();
            yeni.Open();
            yer = Int32.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());

            SqlCommand cmd2 = new SqlCommand("Select * from Notlar where Not_No = '" + yer + "'", yeni);
            SqlDataReader oku2 = cmd2.ExecuteReader();

            while (oku2.Read())
            {
                icerik = (oku2["Not_icerik"].ToString());
            }

            richTextBox2.Text = icerik;
            yeni.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loglar lgr = new loglar();
            if (!Baslangic.Instance.pnlcontainer.Controls.ContainsKey("loglar"))
            {
                lgr.Dock = DockStyle.Fill;
                Baslangic.Instance.pnlcontainer.Controls.Add(lgr);
            }
            Baslangic.Instance.pnlcontainer.Controls["loglar"].Hide();
            Baslangic.Instance.pnlcontainer.Controls["loglar"].BringToFront();
            Baslangic.Instance.pnlcontainer.Controls["loglar"].Show();

            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                yeni.Close();
                yeni.Open();

                SqlCommand komut = new SqlCommand();
                komut.CommandText = "DELETE FROM Notlar WHERE Not_No = @NotNo";
                komut.Parameters.AddWithValue("@NotNo", yer);
                komut.Connection = yeni;
                komut.ExecuteNonQuery();

                MessageBox.Show("Not başarıyla silindi");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
            finally
            {
                yeni.Close();
                richTextBox2.Clear();
            }
        }
    }
}