using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Otel
{
    public partial class odalar : UserControl
    {
        public odalar()
        {
            InitializeComponent();
        }

        private SqlConnection yeni = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");

        private void odalar_Load(object sender, EventArgs e)
        {
            yeni.Open();

            SqlCommand komut = new SqlCommand();
            komut.CommandText = "Select Kat_No as 'Kat' , Oda_No as 'Oda Numarası',Oda_Turu as 'Oda Türü' from Odalar ORDER BY Kat_No ASC";
            komut.Connection = yeni;
            SqlDataReader oku = komut.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku); dataGridView1.DataSource = tablo;
            dataGridView1.AllowUserToAddRows = false;

            SqlCommand komut2 = new SqlCommand();
            komut2.CommandText = "Select Oda_Turu  from Oda_Turleri ORDER BY Oda_Turu ASC";
            komut2.Connection = yeni;
            comboBox1.Items.Clear();
            SqlDataReader ot;
            ot = komut2.ExecuteReader();
            while (ot.Read())
            {
                comboBox1.Items.Add(ot["Oda_Turu"]);
            }

            yeni.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            yeni.Open();

            SqlCommand komut4 = new SqlCommand("select count(*) from Odalar where Oda_No=@ono1", yeni);
            SqlParameter ono1 = new SqlParameter();
            ono1.ParameterName = "@ono1";
            ono1.SqlDbType = SqlDbType.VarChar;
            ono1.Size = 50;
            ono1.Value = textBox1.Text;
            komut4.Parameters.Add(ono1);
            komut4.Parameters.AddWithValue("@ono", textBox1.Text);

            if (Convert.ToInt32(komut4.ExecuteScalar()) > 0)

            {
                MessageBox.Show("Bu kayıt veritabanında zaten var.");
            }
            else
            {
                SqlCommand komut2 = new SqlCommand();
                komut2.CommandText = "insert into Odalar(Oda_No,Kat_No,Oda_Turu) values(@ono,@katno,@OdaTuru)";
                komut2.Connection = yeni;

                SqlParameter ono = new SqlParameter();
                ono.ParameterName = "@ono";
                ono.SqlDbType = SqlDbType.VarChar;
                ono.Value = textBox1.Text;
                komut2.Parameters.Add(ono);

                SqlParameter katno = new SqlParameter();
                katno.ParameterName = "@katno";
                katno.SqlDbType = SqlDbType.VarChar;
                katno.Size = 50;
                katno.Value = comboBox2.Text;
                komut2.Parameters.Add(katno);

                SqlParameter OdaTuru = new SqlParameter();
                OdaTuru.ParameterName = "@OdaTuru";
                OdaTuru.SqlDbType = SqlDbType.VarChar;
                OdaTuru.Size = 50;
                OdaTuru.Value = comboBox1.Text;
                komut2.Parameters.Add(OdaTuru);

                komut2.ExecuteNonQuery();
                MessageBox.Show("kayıt eklendi");

                SqlCommand komut3 = new SqlCommand();
                komut3.CommandText = "Select Kat_No as 'Kat' , Oda_No as 'Oda Numarası',Oda_Turu as 'Oda Türü' from Odalar ORDER BY Kat_No ASC";
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

        private void button2_Click(object sender, EventArgs e)
        {
            yeni.Open();

            string kayit = dataGridView1.CurrentRow.Cells[1].Value.ToString();

            SqlDataAdapter baglan = new SqlDataAdapter("DELETE from Odalar where Oda_No = '" + kayit + "'", yeni);
            DataTable tablo2 = new DataTable();
            baglan.Fill(tablo2);

            SqlCommand komut2 = new SqlCommand();
            komut2.CommandText = "Select Kat_No as 'Kat' , Oda_No as 'Oda Numarası',Oda_Turu as 'Oda Türü' from Odalar ORDER BY Kat_No ASC";
            komut2.Connection = yeni;
            SqlDataReader oku = komut2.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku); dataGridView1.DataSource = tablo;
            dataGridView1.AllowUserToAddRows = false;

            yeni.Close();
        }

        private odatur odtr = new odatur();

        private void button3_Click(object sender, EventArgs e)
        {
            if (!Baslangic.Instance.pnlcontainer.Controls.ContainsKey("odatur"))
            {
                odtr.Dock = DockStyle.Fill;
                Baslangic.Instance.pnlcontainer.Controls.Add(odtr);
            }
            Baslangic.Instance.pnlcontainer.Controls["odatur"].BringToFront();

            this.Hide();

            yeni.Close();
        }

        private void odalar_VisibleChanged(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();

            SqlCommand komut22 = new SqlCommand();
            komut22.CommandText = "Select Kat_No as 'Kat' , Oda_No as 'Oda Numarası',Oda_Turu as 'Oda Türü' from Odalar ORDER BY Kat_No ASC";
            komut22.Connection = yeni;
            SqlDataReader oku = komut22.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku); dataGridView1.DataSource = tablo;
            dataGridView1.AllowUserToAddRows = false;

            SqlCommand komut = new SqlCommand();
            komut.CommandText = "Select Oda_Turu  from Oda_Turleri ORDER BY Oda_Turu ASC";
            komut.Connection = yeni;
            comboBox1.Items.Clear();
            SqlDataReader ot;
            ot = komut.ExecuteReader();
            while (ot.Read())
            {
                comboBox1.Items.Add(ot["Oda_Turu"]);
            }

            yeni.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OdaKontrol okontrol = new OdaKontrol();
            okontrol.Show();
        }
    }
}