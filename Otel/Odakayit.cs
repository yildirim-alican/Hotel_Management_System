using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Otel
{
    public partial class Odakayit : UserControl
    {
        public Odakayit()
        {
            InitializeComponent();
        }

        private SqlConnection yeni = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");

        private void Odakayit_Load(object sender, EventArgs e)
        {
            dateTimePicker2.Value = dateTimePicker1.Value.AddDays(1);
        }

        public static DateTime bTarih;
        public static DateTime kTarih;
        public static DateTime gTarih;

        private void Odakayit_VisibleChanged(object sender, EventArgs e)
        {
            bTarih = Convert.ToDateTime(dateTimePicker1.Value);
            kTarih = Convert.ToDateTime(dateTimePicker2.Value);
            gTarih = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            yeni.Close();
            yeni.Open();

            SqlCommand komut = new SqlCommand();
            komut.CommandText = "Select Musteri_no as 'Müşteri Numarası' , Ad,Soyad   from Musteri where Oda_no is null ORDER BY Musteri_no DESC ";
            komut.Connection = yeni;
            SqlDataReader oku = komut.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku); dataGridView2.DataSource = tablo;
            dataGridView2.AllowUserToAddRows = false;

            SqlCommand komut222 = new SqlCommand();
            komut222.CommandText = "select distinct o.Kat_No as ' Kat' , o.Oda_No as 'Oda No',o.Oda_Turu as ' Oda Türü' from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where o.Doluluk='0' and  ((h.Giris_Tarihi IS NULL) or not((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')) and not(o.Oda_No = any(select distinct o.Oda_No  from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where   ((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "'))))) order by o.Oda_No ";

            komut222.Connection = yeni;
            SqlDataReader oku222 = komut222.ExecuteReader();
            DataTable tablo222 = new DataTable();
            tablo222.Load(oku222); dataGridView1.DataSource = tablo222;
            dataGridView1.AllowUserToAddRows = false;
            comboBox1.SelectedIndex = 0;

            SqlCommand komut12 = new SqlCommand();
            komut12.CommandText = "Select Oda_Turu  from Oda_Turleri ORDER BY Oda_Turu ASC";
            komut12.Connection = yeni;
            comboBox3.Items.Clear();
            comboBox3.Items.Add(" ");
            SqlDataReader ot;
            ot = komut12.ExecuteReader();
            while (ot.Read())
            {
                comboBox3.Items.Add(ot["Oda_Turu"]);
            }

            yeni.Close();

            label4.Text = "Seçim Bekleniyor";
            label11.Text = "Seçim Bekleniyor";
            label10.Text = "Seçim Bekleniyor";
            label13.Text = "Seçim Bekleniyor";
            label16.Text = "    ";
            label23.Text = "0";
            label21.Text = "0";
            label25.Text = "Seçim Bekleniyor";
            textBox1.Text = "";
            listBox1.Items.Clear();
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;

            button3.Visible = false;
            label24.Visible = false;
            groupBox2.Visible = true;
            dateTimePicker2.Enabled = true;
            groupBox4.Visible = false;
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
        }

        public class genel
        {
            public static string secim;
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
                komut.CommandText = "Select Musteri_no as 'Müşteri Numarası', Ad ,Soyad  from Musteri where " + genel.secim + " Like '%" + dgr + "%' and Oda_no is null ORDER BY Musteri_no DESC ";
                komut.Connection = yeni;
                SqlDataReader oku = komut.ExecuteReader();
                DataTable tablo = new DataTable();
                tablo.Load(oku); dataGridView2.DataSource = tablo;
                dataGridView2.AllowUserToAddRows = false;

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
                    komut.CommandText = "Select Musteri_no as 'Müşteri Numarası', Ad ,Soyad from Musteri where " + genel.secim + " Like '%" + dgr + "%' and Oda_no is null  ORDER BY Musteri_no DESC ";
                    komut.Connection = yeni;
                    SqlDataReader oku = komut.ExecuteReader();
                    DataTable tablo = new DataTable();
                    tablo.Load(oku); dataGridView2.DataSource = tablo;
                    dataGridView2.AllowUserToAddRows = false;

                    yeni.Close();
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
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

        public class genel2
        {
            public static string secim2;
            public static string secim3;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();
            genel2.secim2 = comboBox2.Text;
            genel2.secim3 = comboBox3.Text;

            SqlCommand komut = new SqlCommand();
            if (!(comboBox2.SelectedIndex == 0))
            {
                if (comboBox3.SelectedIndex == 0 || comboBox3.SelectedIndex == -1)
                {
                    komut.CommandText = "select distinct o.Kat_No as ' Kat' , o.Oda_No as 'Oda No',o.Oda_Turu as ' Oda Türü' from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where o.Doluluk='0' and ((o.Kat_No = '" + genel2.secim2 + "') and    ((h.Giris_Tarihi IS NULL) or not((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')) and not(o.Oda_No = any(select distinct o.Oda_No  from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where   ((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')))))) order by o.Oda_No ";
                    komut.Connection = yeni;
                    SqlDataReader oku = komut.ExecuteReader();
                    DataTable tablo = new DataTable();
                    tablo.Load(oku); dataGridView1.DataSource = tablo;
                    dataGridView1.AllowUserToAddRows = false;
                }
                else
                {
                    komut.CommandText = "select distinct o.Kat_No as ' Kat' , o.Oda_No as 'Oda No',o.Oda_Turu as ' Oda Türü' from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No  where o.Doluluk='0' and ((o.Kat_No = '" + genel2.secim2 + "' and o.Oda_Turu = '" + genel2.secim3 + "') and   ((h.Giris_Tarihi IS NULL) or not((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')) and not(o.Oda_No = any(select distinct o.Oda_No  from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where   ((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')))))) order by o.Oda_No ";
                    komut.Connection = yeni;
                    SqlDataReader oku = komut.ExecuteReader();
                    DataTable tablo = new DataTable();
                    tablo.Load(oku); dataGridView1.DataSource = tablo;
                    dataGridView1.AllowUserToAddRows = false;
                }
            }
            else
            {
                if (comboBox3.SelectedIndex == 0 || comboBox3.SelectedIndex == -1)
                {
                    SqlCommand komut2 = new SqlCommand();
                    komut2.CommandText = "select distinct o.Kat_No as ' Kat' , o.Oda_No as 'Oda No',o.Oda_Turu as ' Oda Türü' from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where o.Doluluk='0' and  ((h.Giris_Tarihi IS NULL) or not((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')) and not(o.Oda_No = any(select distinct o.Oda_No  from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where   ((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "'))))) order by o.Oda_No ";
                    komut2.Connection = yeni;
                    SqlDataReader oku2 = komut2.ExecuteReader();
                    DataTable tablo2 = new DataTable();
                    tablo2.Load(oku2); dataGridView1.DataSource = tablo2;
                    dataGridView1.AllowUserToAddRows = false;
                }
                else
                {
                    SqlCommand komut6 = new SqlCommand();
                    komut6.CommandText = "select distinct o.Kat_No as ' Kat' , o.Oda_No as 'Oda No',o.Oda_Turu as ' Oda Türü' from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No  where o.Doluluk='0' and ((o.Oda_Turu = '" + genel2.secim3 + "')  and  ((h.Giris_Tarihi IS NULL) or not((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')) and not(o.Oda_No = any(select distinct o.Oda_No  from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where   ((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')))))) order by o.Oda_No ";
                    komut6.Connection = yeni;
                    SqlDataReader oku6 = komut6.ExecuteReader();
                    DataTable tablo6 = new DataTable();
                    tablo6.Load(oku6); dataGridView1.DataSource = tablo6;
                    dataGridView1.AllowUserToAddRows = false;
                }
            }
            yeni.Close();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();
            genel2.secim2 = comboBox2.Text;
            genel2.secim3 = comboBox3.Text;

            SqlCommand komut = new SqlCommand();
            if (!(comboBox3.SelectedIndex == 0))
            {
                if (comboBox2.SelectedIndex == 0 || comboBox2.SelectedIndex == -1)
                {
                    komut.CommandText = "select distinct o.Kat_No as ' Kat' , o.Oda_No as 'Oda No',o.Oda_Turu as ' Oda Türü' from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No  where o.Doluluk='0' and ((o.Oda_Turu = '" + genel2.secim3 + "') and (h.Giris_Tarihi IS NULL) or not((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')) and not(o.Oda_No = any(select distinct o.Oda_No  from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where   ((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "'))))) order by o.Oda_No ";
                    komut.Connection = yeni;
                    SqlDataReader oku = komut.ExecuteReader();
                    DataTable tablo = new DataTable();
                    tablo.Load(oku); dataGridView1.DataSource = tablo;
                    dataGridView1.AllowUserToAddRows = false;
                }
                else
                {
                    komut.CommandText = "select distinct o.Kat_No as ' Kat' , o.Oda_No as 'Oda No',o.Oda_Turu as ' Oda Türü' from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where o.Doluluk='0' and ((o.Kat_No = '" + genel2.secim2 + "' and o.Oda_Turu = '" + genel2.secim3 + "') and  ((h.Giris_Tarihi IS NULL) or not((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')) and not(o.Oda_No = any(select distinct o.Oda_No  from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where   ((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')))))) order by o.Oda_No ";
                    komut.Connection = yeni;
                    SqlDataReader oku = komut.ExecuteReader();
                    DataTable tablo = new DataTable();
                    tablo.Load(oku); dataGridView1.DataSource = tablo;
                    dataGridView1.AllowUserToAddRows = false;
                }
            }
            else
            {
                if (comboBox2.SelectedIndex == 0 || comboBox2.SelectedIndex == -1)
                {
                    SqlCommand komut2 = new SqlCommand();
                    komut2.CommandText = "select distinct o.Kat_No as ' Kat' , o.Oda_No as 'Oda No',o.Oda_Turu as ' Oda Türü' from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where o.Doluluk='0' and  ((h.Giris_Tarihi IS NULL) or not((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')) and not(o.Oda_No = any(select distinct o.Oda_No  from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where   ((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "'))))) order by o.Oda_No ";
                    komut2.Connection = yeni;
                    SqlDataReader oku2 = komut2.ExecuteReader();
                    DataTable tablo2 = new DataTable();
                    tablo2.Load(oku2); dataGridView1.DataSource = tablo2;
                    dataGridView1.AllowUserToAddRows = false;
                }
                else
                {
                    komut.CommandText = "select distinct o.Kat_No as ' Kat' , o.Oda_No as 'Oda No',o.Oda_Turu as ' Oda Türü' from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No  where o.Doluluk='0' and ((o.Kat_No = '" + genel2.secim2 + "') and   ((h.Giris_Tarihi IS NULL) or not((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')) and not(o.Oda_No = any(select distinct o.Oda_No  from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where   ((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')))))) order by o.Oda_No ";
                    komut.Connection = yeni;
                    SqlDataReader oku = komut.ExecuteReader();
                    DataTable tablo = new DataTable();
                    tablo.Load(oku); dataGridView1.DataSource = tablo;
                    dataGridView1.AllowUserToAddRows = false;
                }
            }
            yeni.Close();
        }

        public static string Ad;
        public static string Soyad;
        public static string mus;
        public static string Oda_Turu;
        public static int fiy;
        public static int tytk;
        public static int cytk;
        public static int toplamytk;
        public static string ad5;
        public static string soyad5;
        public static string musteri5;
        public static DateTime odat;

        public static int indext;

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            yeni.Close();
            yeni.Open();
            int yer = Int32.Parse(dataGridView2.CurrentRow.Cells[0].Value.ToString());

            SqlCommand cmd = new SqlCommand("Select * from Musteri where Musteri_no = '" + yer + "'", yeni);
            SqlDataReader oku = cmd.ExecuteReader();

            while (oku.Read())
            {
                Ad = oku["Ad"].ToString();
                Soyad = oku["Soyad"].ToString();
                mus = oku["Musteri_no"].ToString();
            }

            label4.Text = Ad + " " + Soyad;
            label16.Text = mus;
        }

        public static int yerbil;

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            yeni.Close();
            yeni.Open();

            int yer2 = Int32.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString());
            string yer = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            yerbil = yer2;
            indext = dataGridView1.CurrentRow.Index - 1;

            SqlCommand cmd2 = new SqlCommand("Select * from Oda_Turleri where Oda_Turu = '" + yer + "'", yeni);
            SqlDataReader oku2 = cmd2.ExecuteReader();

            while (oku2.Read())
            {
                Oda_Turu = oku2["Oda_Turu"].ToString();

                fiy = Convert.ToInt32(oku2["fiyat"].ToString());

                cytk = Convert.ToInt32(oku2["ciftytk"].ToString());

                tytk = Convert.ToInt32(oku2["tekytk"].ToString());
            }

            toplamytk = 2 * cytk + tytk;
            label23.Text = Convert.ToString(toplamytk);
            label11.Text = Convert.ToString(yer2);
            label10.Text = Oda_Turu;
            label13.Text = Convert.ToString(fiy) + " TL";
            label25.Text = Convert.ToString(toplamytk);

            yeni.Close();
            yeni.Open();

            listBox1.Items.Clear();
            string sorgu = "Select * from Musteri as m left join Hesap as h on m.Musteri_no=h.Musteri_no where  m.Oda_No = '" + yerbil + "' and h.Giris_Tarihi ='" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "'  ";
            SqlDataAdapter adp = new SqlDataAdapter(sorgu, yeni);
            DataSet ds = new DataSet();
            adp.Fill(ds);

            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                listBox1.Items.Add(ds.Tables[0].Rows[i][2].ToString() + " " + ds.Tables[0].Rows[i][3].ToString());
            }

            label21.Text = Convert.ToString(ds.Tables[0].Rows.Count);

            yeni.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label11.Text == "Seçim Bekleniyor" || label4.Text == "Seçim Bekleniyor")
            {
                if (label11.Text == "Seçim Bekleniyor" && label4.Text != "Seçim Bekleniyor")
                {
                    MessageBox.Show("Oda seçimini gerçekleştirmeden kayıt ekleyemessiniz");
                }
                else if (label11.Text != "Seçim Bekleniyor" && label4.Text == "Seçim Bekleniyor")
                {
                    MessageBox.Show("Müşteri seçimini gerçekleştirmeden kayıt ekleyemessiniz");
                }
                else
                {
                    MessageBox.Show("Müşteri ve oda seçimini gerçekleştirmeden kayıt ekleyemessiniz");
                }
            }
            else
            {
                if (label21.Text == label23.Text)
                {
                    MessageBox.Show("Bu Odaya Daha Fazla Müşteri Ekleyemessiniz");
                }
                else
                {
                    yeni.Close();
                    yeni.Open();

                    SqlCommand komut2 = new SqlCommand();
                    komut2.CommandText = "insert into Hesap(Musteri_no,Giris_Tarihi,Cikis_Tarihi,Oda_No) values(@yMusteri_no,@yGiris_Tarihi,@yCikis_Tarihi,@yOda_No)";
                    komut2.Connection = yeni;

                    SqlParameter yMusteri_no = new SqlParameter();
                    yMusteri_no.ParameterName = "@yMusteri_no";
                    yMusteri_no.SqlDbType = SqlDbType.VarChar;
                    yMusteri_no.Size = 50;
                    yMusteri_no.Value = label16.Text;
                    komut2.Parameters.Add(yMusteri_no);

                    SqlParameter yOda_No = new SqlParameter();
                    yOda_No.ParameterName = "@yOda_No";
                    yOda_No.SqlDbType = SqlDbType.Int;
                    yOda_No.Size = 50;
                    yOda_No.Value = Convert.ToInt32(label11.Text);
                    komut2.Parameters.Add(yOda_No);

                    SqlParameter yGiris_Tarihi = new SqlParameter();
                    yGiris_Tarihi.ParameterName = "@yGiris_Tarihi";
                    yGiris_Tarihi.SqlDbType = SqlDbType.Date;
                    yGiris_Tarihi.Size = 50;
                    yGiris_Tarihi.Value = dateTimePicker1.Value.ToShortDateString();
                    komut2.Parameters.Add(yGiris_Tarihi);

                    SqlParameter yCikis_Tarihi = new SqlParameter();
                    yCikis_Tarihi.ParameterName = "@yCikis_Tarihi";
                    yCikis_Tarihi.SqlDbType = SqlDbType.Date;
                    yCikis_Tarihi.Size = 50;
                    yCikis_Tarihi.Value = dateTimePicker2.Value.ToShortDateString();
                    komut2.Parameters.Add(yCikis_Tarihi);

                    komut2.ExecuteNonQuery();
                    MessageBox.Show("Kişi Odaya Eklendi");

                    SqlCommand komut3 = new SqlCommand();
                    komut3.CommandText = "UPDATE Musteri SET Oda_no = '" + label11.Text + "' where Musteri_no = '" + label16.Text + "'";
                    komut3.Connection = yeni;

                    komut3.ExecuteNonQuery();

                    SqlCommand komut4 = new SqlCommand();
                    komut4.CommandText = "UPDATE Odalar SET Doluluk = 1 where Oda_No = '" + label11.Text + "'";
                    komut4.Connection = yeni;

                    komut4.ExecuteNonQuery();

                    SqlCommand komut44 = new SqlCommand();
                    komut44.CommandText = "UPDATE Hesap SET Durum = 1 where Oda_No = '" + label11.Text + "' and Giris_Tarihi='" + bTarih.ToString("MM / dd / yyyy HH: mm: ss") + "'";
                    komut44.Connection = yeni;

                    komut44.ExecuteNonQuery();

                    SqlCommand komut5 = new SqlCommand();
                    komut5.CommandText = "Select Musteri_no as 'Müşteri Numarası' , Ad,Soyad   from Musteri where Oda_no is null ORDER BY Musteri_no DESC ";
                    komut5.Connection = yeni;
                    SqlDataReader oku = komut5.ExecuteReader();
                    DataTable tablo = new DataTable();
                    tablo.Load(oku); dataGridView2.DataSource = tablo;
                    dataGridView2.AllowUserToAddRows = false;

                    SqlCommand komut6 = new SqlCommand();
                    komut6.CommandText = "select distinct o.Kat_No as ' Kat' , o.Oda_No as 'Oda No',o.Oda_Turu as ' Oda Türü' from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where o.Doluluk='0' and  ((h.Giris_Tarihi IS NULL) or not((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')) and not(o.Oda_No = any(select distinct o.Oda_No  from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where   ((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "'))))) order by o.Oda_No ";
                    komut6.Connection = yeni;
                    SqlDataReader oku2 = komut6.ExecuteReader();
                    DataTable tablo2 = new DataTable();
                    tablo2.Load(oku2); dataGridView1.DataSource = tablo2;
                    dataGridView1.AllowUserToAddRows = false;

                    label4.Text = "Seçim Bekleniyor";
                    label16.Text = "    ";

                    comboBox2.SelectedIndex = 0;
                    comboBox3.SelectedIndex = 0;

                    yeni.Close();
                    yeni.Open();

                    listBox1.Items.Clear();
                    string sorgu = "Select * from Musteri as m left join Hesap as h on m.Musteri_no=h.Musteri_no where  m.Oda_No = '" + yerbil + "' and h.Giris_Tarihi ='" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "'  ";
                    SqlDataAdapter adp = new SqlDataAdapter(sorgu, yeni);
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                    {
                        listBox1.Items.Add(ds.Tables[0].Rows[i][2].ToString() + " " + ds.Tables[0].Rows[i][3].ToString());
                    }

                    label21.Text = Convert.ToString(ds.Tables[0].Rows.Count);

                    yeni.Close();

                    dataGridView2.ClearSelection();
                    //dataGridView1.Rows[indext].Selected = true;

                    if (label21.Text == label23.Text)
                    {
                        button3.Visible = false;
                        label24.Visible = false;
                        groupBox2.Visible = true;
                        dateTimePicker2.Enabled = true;
                        listBox1.Items.Clear();

                        label23.Text = "0";
                        label21.Text = "0";
                        label25.Text = "Seçim Bekleniyor";
                        groupBox4.Visible = false;

                        dataGridView1.ClearSelection();
                        dataGridView2.ClearSelection();

                        label11.Text = "Seçim Bekleniyor";
                        label10.Text = "Seçim Bekleniyor";
                        label13.Text = "Seçim Bekleniyor";
                        label16.Text = "    ";
                    }
                    else
                    {
                        dateTimePicker2.Enabled = false;
                        button3.Visible = true;
                        label24.Visible = true;
                        groupBox2.Visible = false;
                        groupBox4.Visible = true;
                    }
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            bTarih = Convert.ToDateTime(dateTimePicker1.Text);
            kTarih = Convert.ToDateTime(dateTimePicker2.Text);
            gTarih = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            if (bTarih < gTarih)
            {
                MessageBox.Show("Geçmiş Bir Tarihi Seçemessiniz");
                dateTimePicker1.Value = gTarih;
            }
            else
            {
                TimeSpan Sonuc = kTarih - bTarih;
                label8.Text = Sonuc.TotalDays.ToString();
                dateTimePicker2.Value = dateTimePicker1.Value.AddDays(1);
            }

            yeni.Close();
            yeni.Open();

            SqlCommand komut2 = new SqlCommand();
            komut2.CommandText = "select distinct o.Kat_No as ' Kat' , o.Oda_No as 'Oda No',o.Oda_Turu as ' Oda Türü' from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where o.Doluluk='0' and  ((h.Giris_Tarihi IS NULL) or not((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')) and not(o.Oda_No = any(select distinct o.Oda_No  from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where   ((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "'))))) order by o.Oda_No ";
            komut2.Connection = yeni;
            SqlDataReader oku2 = komut2.ExecuteReader();
            DataTable tablo2 = new DataTable();
            tablo2.Load(oku2); dataGridView1.DataSource = tablo2;
            dataGridView1.AllowUserToAddRows = false;

            yeni.Close();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            bTarih = Convert.ToDateTime(dateTimePicker1.Text);
            kTarih = Convert.ToDateTime(dateTimePicker2.Text);

            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            textBox2.Text = "";

            if (kTarih <= bTarih)
            {
                MessageBox.Show("Geçmiş Bir Tarihi Seçemessiniz");
                dateTimePicker2.Value = dateTimePicker1.Value.AddDays(1);
            }
            else
            {
                TimeSpan Sonuc = kTarih - bTarih;
                label8.Text = Sonuc.TotalDays.ToString();
            }

            yeni.Close();
            yeni.Open();

            SqlCommand komut2 = new SqlCommand();
            komut2.CommandText = "select distinct o.Kat_No as ' Kat' , o.Oda_No as 'Oda No',o.Oda_Turu as ' Oda Türü' from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where o.Doluluk='0' and  ((h.Giris_Tarihi IS NULL) or not((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')) and not(o.Oda_No = any(select distinct o.Oda_No  from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where   ((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "'))))) order by o.Oda_No ";
            komut2.Connection = yeni;
            SqlDataReader oku2 = komut2.ExecuteReader();
            DataTable tablo2 = new DataTable();
            tablo2.Load(oku2); dataGridView1.DataSource = tablo2;
            dataGridView1.AllowUserToAddRows = false;

            yeni.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;

            yeni.Close();
            yeni.Open();
            SqlCommand komut = new SqlCommand();
            komut.CommandText = "select distinct o.Kat_No as ' Kat' , o.Oda_No as 'Oda No',o.Oda_Turu as ' Oda Türü' from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where o.Doluluk='0' and o.Oda_No Like '" + textBox2.Text + "%' and  ((h.Giris_Tarihi IS NULL) or not((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "')) and not(o.Oda_No = any(select distinct o.Oda_No  from Odalar as o  left join Hesap as h on o.Oda_No=h.Oda_No   where   ((h.Giris_Tarihi < '" + kTarih.ToString("MM/dd/yyyy HH:mm:ss") + "' and h.Cikis_Tarihi > '" + bTarih.ToString("MM/dd/yyyy HH:mm:ss") + "'))))) order by o.Oda_No ";
            komut.Connection = yeni;
            SqlDataReader oku = komut.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku); dataGridView1.DataSource = tablo;
            dataGridView1.AllowUserToAddRows = false;
            yeni.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            label24.Visible = false;
            groupBox2.Visible = true;
            groupBox4.Visible = false;
            dateTimePicker2.Enabled = true;
            label4.Text = "Seçim Bekleniyor";
            label11.Text = "Seçim Bekleniyor";
            label10.Text = "Seçim Bekleniyor";
            label13.Text = "Seçim Bekleniyor";
            label16.Text = "    ";
            label23.Text = "0";
            label21.Text = "0";
            label25.Text = "Seçim Bekleniyor";
            listBox1.Items.Clear();
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;

            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();

            dateTimePicker2.Value = dateTimePicker1.Value.AddDays(1);
        }
    }
}