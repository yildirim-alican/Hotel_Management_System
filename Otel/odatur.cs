using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Otel
{
    public partial class odatur : UserControl
    {
        public odatur()
        {
            InitializeComponent();
            groupBox2 = new GroupBox();

        }

        private SqlConnection yeni = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");

        public static string Oda_Turu;
        public static int tekytk;
        public static int ciftytk;
        public static int fiyat;
        public static string ek;

        private void odatur_Load(object sender, EventArgs e)
        {
            groupBox1.Show();
            groupBox2.Hide();
            yeni.Open();

            SqlCommand komut = new SqlCommand();
            komut.CommandText = "Select Oda_Turu as 'Oda Türü' , tekytk as 'Tek Kişilik Yatak Sayısı',ciftytk as 'Çift Kişilik Yatak Sayısı',fiyat as 'Oda Fiyatı'  from Oda_Turleri ORDER BY Oda_Turu ASC";
            komut.Connection = yeni;
            SqlDataReader oku = komut.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku); dataGridView1.DataSource = tablo;
            dataGridView1.AllowUserToAddRows = false;

            yeni.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            yeni.Open();

            SqlCommand komut4 = new SqlCommand("select count(*) from Oda_Turleri where Oda_Turu=@otno1", yeni);
            SqlParameter otno1 = new SqlParameter();
            otno1.ParameterName = "@otno1";
            otno1.SqlDbType = SqlDbType.VarChar;
            otno1.Size = 50;
            otno1.Value = textBox1.Text;
            komut4.Parameters.Add(otno1);
            komut4.Parameters.AddWithValue("@otno", textBox1.Text);

            if (Convert.ToInt32(komut4.ExecuteScalar()) > 0)

            {
                MessageBox.Show("Bu kayıt veritabanında zaten var.");
            }
            else
            {
                SqlCommand komut2 = new SqlCommand();
                komut2.CommandText = "insert into Oda_Turleri(Oda_Turu,tekytk,ciftytk,fiyat) values(@otno,@tek,@cift,@fiy)";
                komut2.Connection = yeni;

                SqlParameter otno = new SqlParameter();
                otno.ParameterName = "@otno";
                otno.SqlDbType = SqlDbType.VarChar;
                otno.Value = textBox1.Text;
                komut2.Parameters.Add(otno);

                SqlParameter tek = new SqlParameter();
                tek.ParameterName = "@tek";
                tek.SqlDbType = SqlDbType.VarChar;
                tek.Size = 50;
                tek.Value = textBox2.Text;
                komut2.Parameters.Add(tek);

                SqlParameter cift = new SqlParameter();
                cift.ParameterName = "@cift";
                cift.SqlDbType = SqlDbType.VarChar;
                cift.Value = textBox3.Text;
                komut2.Parameters.Add(cift);

                SqlParameter fiy = new SqlParameter();
                fiy.ParameterName = "@fiy";
                fiy.SqlDbType = SqlDbType.VarChar;
                fiy.Size = 50;
                fiy.Value = textBox4.Text;
                komut2.Parameters.Add(fiy);

                komut2.ExecuteNonQuery();
                MessageBox.Show("kayıt eklendi");

                SqlCommand komut3 = new SqlCommand();
                komut3.CommandText = "Select Oda_Turu as 'Oda Türü' , tekytk as 'Tek Kişilik Yatak Sayısı',ciftytk as 'Çift Kişilik Yatak Sayısı',fiyat as 'Oda Fiyatı'  from Oda_Turleri ORDER BY Oda_Turu ASC";
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

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();

            yeni.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            groupBox1.Show();
            button6.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            groupBox2.Show();
            button7.Hide();

            yeni.Open();

            // Get the value from the first cell of the selected row in dataGridView1
            string yer = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            SqlCommand cmd = new SqlCommand("Select * from Oda_Turleri where Oda_Turu = '" + yer + "'", yeni);

            SqlDataReader oku = cmd.ExecuteReader();

            while (oku.Read())
            {
                Oda_Turu = oku["Oda_Turu"].ToString();
                tekytk = Convert.ToInt32(oku["tekytk"].ToString());
                ciftytk = Convert.ToInt32(oku["ciftytk"].ToString());
                fiyat = Convert.ToInt32(oku["fiyat"].ToString());
                ek = oku["ek"].ToString();
            }

            // Assign the retrieved values to the controls in groupBox2
            textBox6.Text = Oda_Turu;
            textBox7.Text = Convert.ToString(tekytk);
            textBox8.Text = Convert.ToString(ciftytk);
            textBox9.Text = Convert.ToString(fiyat);
            label5.Text = Oda_Turu;

            yeni.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();

            groupBox1.Hide();
            button6.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();

            groupBox2.Hide();
            button7.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            odalar od = new odalar();

            if (!Baslangic.Instance.pnlcontainer.Controls.ContainsKey("Musyonet"))
            {
                od.Dock = DockStyle.Fill;
                Baslangic.Instance.pnlcontainer.Controls.Add(od);
            }

            Baslangic.Instance.pnlcontainer.Controls["Odalar"].Show();
            Baslangic.Instance.pnlcontainer.Controls["Odalar"].BringToFront();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            yeni.Open();
            string komut = "UPDATE Oda_Turleri SET Oda_Turu = '" + textBox6.Text + "' , tekytk = '" + textBox7.Text + "', ciftytk = '" + textBox8.Text + "', fiyat = '" + textBox9.Text + "' where Oda_Turu = '" + label5.Text + "'";
            SqlCommand kmt = new SqlCommand(komut, yeni);
            kmt.ExecuteNonQuery();

            string komut5 = "UPDATE Odalar SET Oda_Turu = '" + textBox6.Text + "' where Oda_Turu = '" + label5.Text + "'";
            SqlCommand kmt5 = new SqlCommand(komut5, yeni);
            kmt5.ExecuteNonQuery();

            groupBox2.Hide();
            button7.Show();

            SqlCommand komut2 = new SqlCommand();
            komut2.CommandText = "Select Oda_Turu as 'Oda Türü' , tekytk as 'Tek Kişilik Yatak Sayısı',ciftytk as 'Çift Kişilik Yatak Sayısı',fiyat as 'Oda Fiyatı' from Oda_Turleri ORDER BY Oda_Turu ASC";
            komut2.Connection = yeni;
            SqlDataReader oku = komut2.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku); dataGridView1.DataSource = tablo;
            dataGridView1.AllowUserToAddRows = false;

            MessageBox.Show("Değişiklikler Kaydedildi");

            yeni.Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            groupBox2.Show();
            button7.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();
            string kayit = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            SqlDataAdapter baglan = new SqlDataAdapter("DELETE from Oda_Turleri where Oda_Turu = '" + kayit + "'", yeni);
            DataTable tablo2 = new DataTable();
            baglan.Fill(tablo2);

            SqlCommand komut = new SqlCommand();
            komut.CommandText = "Select Oda_Turu as 'Oda Türü' , tekytk as 'Tek Kişilik Yatak Sayısı',ciftytk as 'Çift Kişilik Yatak Sayısı',fiyat as 'Oda Fiyatı'  from Oda_Turleri ORDER BY Oda_Turu ASC";
            komut.Connection = yeni;
            SqlDataReader oku = komut.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku); dataGridView1.DataSource = tablo;
            dataGridView1.AllowUserToAddRows = false;

            yeni.Close();
        }
    }
}