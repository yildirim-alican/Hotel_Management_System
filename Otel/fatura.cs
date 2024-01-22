using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Otel
{
    public partial class fatura : UserControl
    {
        public fatura()
        {
            InitializeComponent();
        }

        private SqlConnection yeni = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");

        public static string alma;
        public static int faturasayisi;

        public static DateTime giris5;
        public static DateTime cikis5;
        public static string tur5;
        public static int fiy5;
        public static int tytk5;
        public static int cytk5;
        public static int toplamytk5;
        public static int odenen;
        public static int ekstra5;
        public static int odemesayisi;

        private faturabilgi ftrblg = new faturabilgi();

        protected void button_Click(object sender, EventArgs e)
        {
            ListBox button = sender as ListBox;

            alma = button.Items[1].ToString().Substring(12);

            yeni.Close();
            yeni.Open();

            SqlCommand cmd = new SqlCommand("Select * from Hesap where Oda_No = '" + alma + "'  and Durum = 1 ", yeni);
            SqlDataReader oku = cmd.ExecuteReader();

            while (oku.Read())
            {
                giris5 = Convert.ToDateTime(oku["Giris_Tarihi"].ToString());
                cikis5 = Convert.ToDateTime(oku["Cikis_Tarihi"].ToString());
            }

            ftrblg.label1.Text = "Oda " + alma;
            ftrblg.dateTimePicker1.Value = giris5;
            ftrblg.dateTimePicker2.Value = cikis5;

            yeni.Close();
            yeni.Open();

            SqlCommand cmd2 = new SqlCommand("Select * from Odalar as o left join Oda_Turleri as ot on o.Oda_Turu=ot.Oda_Turu where o.Oda_No = '" + alma + "'", yeni);
            SqlDataReader oku2 = cmd2.ExecuteReader();

            while (oku2.Read())
            {
                tur5 = oku2["Oda_Turu"].ToString();
                fiy5 = Convert.ToInt32(oku2["fiyat"].ToString());
                cytk5 = Convert.ToInt32(oku2["ciftytk"].ToString());

                tytk5 = Convert.ToInt32(oku2["tekytk"].ToString());
            }

            toplamytk5 = 2 * cytk5 + tytk5;
            ftrblg.label10.Text = tur5;
            ftrblg.label13.Text = fiy5.ToString() + " TL";
            ftrblg.label2.Text = Convert.ToString(fiy5 * Convert.ToInt32(ftrblg.label8.Text) + " TL");

            yeni.Close();
            yeni.Open();

            SqlCommand komut90 = new SqlCommand();
            komut90.CommandText = "Select  Musteri_no as 'Müşteri No' , ad+' '+Soyad as 'Ad Soyad' from Musteri where Oda_no = " + alma + " ";
            komut90.Connection = yeni;
            SqlDataReader oku200 = komut90.ExecuteReader();
            DataTable tablo200 = new DataTable();
            tablo200.Load(oku200);
            ftrblg.dataGridView1.DataSource = tablo200;
            ftrblg.dataGridView1.AllowUserToAddRows = false;

            string sorgu = "Select * from Musteri where Oda_No = '" + alma + "'";
            SqlDataAdapter adp = new SqlDataAdapter(sorgu, yeni);
            DataSet ds = new DataSet();
            adp.Fill(ds);

            ftrblg.label21.Text = Convert.ToString(ds.Tables[0].Rows.Count);
            ftrblg.label23.Text = toplamytk5.ToString();
            ftrblg.dataGridView1.ClearSelection();

            SqlCommand komut50 = new SqlCommand();
            komut50.CommandText = "Select  Musteri_no as 'Müş No' , Odeme_Tutari as 'Ödeme Tutarı (TL)', Odeme_Tarihi as 'Ödeme Tarihi', Oda_Giris from Fatura where Oda_no = " + alma + "  ";
            komut50.Connection = yeni;
            SqlDataReader oku50 = komut50.ExecuteReader();
            DataTable tablo50 = new DataTable();
            tablo50.Load(oku50);
            ftrblg.dataGridView2.DataSource = tablo50;
            ftrblg.dataGridView2.Columns[3].Visible = false;
            ftrblg.dataGridView2.AllowUserToAddRows = false;

            SqlCommand komut22 = new SqlCommand();
            komut22.CommandText = "Select  Ad,Soyad from Musteri where Oda_no = " + alma + "";
            komut22.Connection = yeni;
            ftrblg.comboBox1.Items.Clear();
            ftrblg.comboBox1.Items.Add("Müşteri Seçiniz");
            SqlDataReader isimver;
            isimver = komut22.ExecuteReader();
            while (isimver.Read())
            {
                ftrblg.comboBox1.Items.Add(isimver["Ad"] + " " + isimver["Soyad"]);
            }
            ftrblg.comboBox1.SelectedIndex = 0;
            ftrblg.comboBox2.SelectedIndex = 0;

            //select sum(Odeme_Tutari) from Fatura where Oda_no=116

            yeni.Close();
            yeni.Open();

            SqlCommand komut16 = new SqlCommand();
            komut16.CommandText = "select count(Toplam) as sayi6 from Ekstra where Oda_no=" + alma + " ";
            komut16.Connection = yeni;
            SqlDataReader oku16 = komut16.ExecuteReader();

            while (oku16.Read())
            {
                odemesayisi = Convert.ToInt32(oku16["sayi6"].ToString());
            }

            if (odemesayisi > 0)
            {
                yeni.Close();
                yeni.Open();
                string sorgu2 = "select sum(Toplam) from Ekstra where Oda_no=" + alma + " ";
                SqlDataAdapter adp22 = new SqlDataAdapter(sorgu2, yeni);
                DataSet ds2 = new DataSet();
                adp22.Fill(ds2);

                odenen = Convert.ToInt32(ds2.Tables[0].Rows[0][0].ToString());
                ftrblg.label11.Text = ds2.Tables[0].Rows[0][0].ToString() + " TL";
            }
            else
            {
                ftrblg.label11.Text = "0 TL";
            }

            ftrblg.label17.Text = Convert.ToString((Convert.ToInt32(ftrblg.label2.Text.Replace("TL", ""))) + (Convert.ToInt32(ftrblg.label11.Text.Replace("TL", ""))) + " TL");

            yeni.Close();
            yeni.Open();

            SqlCommand komut15 = new SqlCommand();
            komut15.CommandText = "select count(Odeme_Tutari) as sayi5 from Fatura where Oda_no=" + alma + " ";
            komut15.Connection = yeni;
            SqlDataReader oku15 = komut15.ExecuteReader();

            while (oku15.Read())
            {
                odemesayisi = Convert.ToInt32(oku15["sayi5"].ToString());
            }

            if (odemesayisi > 0)
            {
                yeni.Close();
                yeni.Open();
                string sorgu2 = "select sum(Odeme_Tutari) from Fatura where Oda_no=" + alma + " ";
                SqlDataAdapter adp22 = new SqlDataAdapter(sorgu2, yeni);
                DataSet ds2 = new DataSet();
                adp22.Fill(ds2);

                odenen = Convert.ToInt32(ds2.Tables[0].Rows[0][0].ToString());
                ftrblg.label26.Text = ds2.Tables[0].Rows[0][0].ToString() + " TL";
                ftrblg.label19.Text = Convert.ToString((Convert.ToInt32(ftrblg.label17.Text.Replace("TL", ""))) - (Convert.ToInt32(ds2.Tables[0].Rows[0][0].ToString()))) + " TL";
            }
            else
            {
                ftrblg.label26.Text = "0 TL";
                ftrblg.label19.Text = ftrblg.label17.Text;
            }

            if (ftrblg.label19.Text == "0 TL")
            {
                ftrblg.checkBox1.Checked = true;
                ftrblg.checkBox1.Text = "Ödeme Tamamlandı";
            }
            else
            {
                ftrblg.checkBox1.Checked = false;
                ftrblg.checkBox1.Text = "Ödeme Tamamlanmadı";
            }

            if (!Baslangic.Instance.pnlcontainer.Controls.ContainsKey("faturabilgi"))
            {
                ftrblg.Dock = DockStyle.Fill;
                Baslangic.Instance.pnlcontainer.Controls.Add(ftrblg);
            }
            Baslangic.Instance.pnlcontainer.Controls["faturabilgi"].BringToFront();
            Baslangic.Instance.pnlcontainer.Controls["faturabilgi"].Show();

            this.Hide();

            yeni.Close();
        }

        private void fatura_VisibleChanged(object sender, EventArgs e)
        {
            panel1.Controls.Clear();

            comboBox2.SelectedIndex = 0;
            textBox1.Text = "";
            yeni.Close();
            yeni.Open();

            SqlCommand komut5 = new SqlCommand();
            komut5.CommandText = "select COUNT(DISTINCT Oda_No) as sayi from Musteri ";
            komut5.Connection = yeni;
            SqlDataReader oku = komut5.ExecuteReader();

            while (oku.Read())
            {
                faturasayisi = Convert.ToInt32(oku["sayi"].ToString());
            }

            int x = 0;
            int y = 0;

            for (int i = 0; i < faturasayisi; i++)
            {
                ListBox lst = new ListBox();
                lst.Height = 122;
                lst.Width = 140;
                lst.HorizontalScrollbar = true;
                lst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));

                lst.Click += new EventHandler(button_Click);

                yeni.Close();
                yeni.Open();
                string sorgu = "Select DISTINCT h.Oda_No from Hesap as h left join Odalar as o on h.Oda_No=o.Oda_No where o.Doluluk=1 order by h.Oda_No";
                SqlDataAdapter adp = new SqlDataAdapter(sorgu, yeni);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                //yeni.Close();

                string sorgu2 = "Select * from Musteri where Oda_No = '" + ds.Tables[0].Rows[i][0] + "'";
                SqlDataAdapter adp2 = new SqlDataAdapter(sorgu2, yeni);
                DataSet ds2 = new DataSet();
                adp2.Fill(ds2);

                string sorgu3 = "select DISTINCT Cikis_Tarihi from Hesap where Oda_No = '" + ds.Tables[0].Rows[i][0] + "' and Durum=1";
                SqlDataAdapter adp3 = new SqlDataAdapter(sorgu3, yeni);
                DataSet ds3 = new DataSet();
                adp3.Fill(ds3);
                DateTime guTarih = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                DateTime buTarih = Convert.ToDateTime(ds3.Tables[0].Rows[0][0]);

                if (buTarih <= guTarih)
                {
                    lst.BackColor = System.Drawing.Color.Crimson;
                    lst.ForeColor = System.Drawing.SystemColors.Window;
                }
                else
                {
                    lst.BackColor = System.Drawing.SystemColors.Window;
                    lst.ForeColor = System.Drawing.SystemColors.WindowText;
                }
                lst.Items.Add(" ");
                lst.Items.Add("        Oda " + ds.Tables[0].Rows[i][0]);
                lst.Items.Add(" ");

                for (int a = 0; a <= ds2.Tables[0].Rows.Count - 1; a++)
                {
                    lst.Items.Add(" " + ds2.Tables[0].Rows[a][2].ToString() + " " + ds2.Tables[0].Rows[a][3].ToString());
                }

                if (x < 520)
                {
                    lst.Location = new Point(x, y);
                    panel1.Controls.Add(lst);
                    x += 150;
                }
                else
                {
                    x = 0;
                    y += 132;
                    lst.Location = new Point(x, y);
                    panel1.Controls.Add(lst);
                    x += 150;
                }

                yeni.Close();
            }

            yeni.Close();
            yeni.Open();

            SqlCommand komut = new SqlCommand();
            komut.CommandText = "select  m.Musteri_no as 'Müş No' , ad+ ' '+ soyad as 'Ad Soyad', m.Oda_no as 'Oda No' from Musteri as m join Hesap as h on m.Musteri_no=h.Musteri_no where h.Durum=1 order by m.oda_no DESC";
            komut.Connection = yeni;
            SqlDataReader oku8 = komut.ExecuteReader();
            DataTable tablo = new DataTable();
            tablo.Load(oku8); dataGridView1.DataSource = tablo;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void dataGridView1_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            dataGridView1.ClearSelection();

            int rowSelected = e.RowIndex;
            if (e.RowIndex != -1)
            {
                this.dataGridView1.Rows[rowSelected].Selected = true;
            }
            e.ContextMenuStrip = contextMenuStrip1;
        }

        public static string Musteri_no2;
        public static string Ad2;
        public static string Soyad2;
        public static string Cinsiyet2;
        public static string Dogum_tarihi2;
        public static string Medeni_Hal2;
        public static string Telefon_no2;
        public static string E_Posta2;
        public static string Kimlik_no2;

        public static string Kimlik_seri_No2;
        public static string anne2;
        public static string baba2;
        public static string adres2;
        public static string Oda_no2;

        private duzenle2 musdzn2 = new duzenle2();

        private void müşteriBilgilerineGitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();
            int yer = Int32.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());

            SqlCommand cmd = new SqlCommand("Select * from Musteri where Musteri_no = '" + yer + "'", yeni);
            SqlDataReader oku = cmd.ExecuteReader();

            while (oku.Read())
            {
                Kimlik_no2 = oku["Kimlik_no"].ToString();
                Musteri_no2 = oku["Musteri_no"].ToString();
                Ad2 = oku["Ad"].ToString();
                Soyad2 = oku["Soyad"].ToString();
                Cinsiyet2 = oku["Cinsiyet"].ToString();
                Dogum_tarihi2 = oku["Dogum_tarihi"].ToString();
                Medeni_Hal2 = oku["Medeni_Hal"].ToString();
                Telefon_no2 = oku["Telefon_no"].ToString();
                E_Posta2 = oku["E_Posta"].ToString();

                Kimlik_seri_No2 = oku["Kimlik_seri_No"].ToString();
                anne2 = oku["anne"].ToString();
                baba2 = oku["baba"].ToString();
                adres2 = oku["adres"].ToString();
                Oda_no2 = oku["Oda_no"].ToString();
            }

            musdzn2.dznad.Text = Ad2;
            musdzn2.dznsoyad.Text = Soyad2;
            musdzn2.dzncmbcns.Text = Cinsiyet2;
            musdzn2.maskedTextBox2.Text = Dogum_tarihi2;
            musdzn2.dznmdnhlcmbx.Text = Medeni_Hal2;
            musdzn2.maskedTextBox1.Text = Telefon_no2;
            musdzn2.dznep.Text = E_Posta2;
            musdzn2.dzntc.Text = Kimlik_no2;
            musdzn2.label10.Text = Musteri_no2;

            musdzn2.textBox12.Text = Kimlik_seri_No2;
            musdzn2.textBox10.Text = anne2;
            musdzn2.textBox4.Text = baba2;
            musdzn2.richTextBox1.Text = adres2;

            yeni.Close();
            yeni.Open();
            SqlCommand komut9 = new SqlCommand();
            komut9.CommandText = "Select islem_no as 'Kayıt Numarası', Musteri_no as 'Müşteri No',Giris_Tarihi as 'Giriş Tarihi', Cikis_Tarihi as ' Çıkış Tarihi', Oda_No as 'Oda No' from Hesap where Musteri_no = " + yer + " ORDER BY islem_no DESC ";
            komut9.Connection = yeni;
            SqlDataReader oku2 = komut9.ExecuteReader();
            DataTable tablo2 = new DataTable();
            tablo2.Load(oku2); musdzn2.dataGridView1.DataSource = tablo2;
            musdzn2.dataGridView1.AllowUserToAddRows = false;
            musdzn2.dataGridView1.Columns[1].Visible = false;

            if (!Baslangic.Instance.pnlcontainer.Controls.ContainsKey("duzenle2"))
            {
                musdzn2.Dock = DockStyle.Fill;
                Baslangic.Instance.pnlcontainer.Controls.Add(musdzn2);
            }

            Baslangic.Instance.pnlcontainer.Controls["duzenle2"].BringToFront();

            yeni.Close();
        }

        public class genel55
        {
            public static string secim55;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (genel55.secim55 == "10")
            {
                MessageBox.Show("Arama Yapabilmek İçin Lütfen Bir Kriter Seçiniz");
            }
            else
            {
                yeni.Close();
                yeni.Open();
                string dgr = textBox1.Text.Replace(" ", "");

                SqlCommand komut = new SqlCommand();
                komut.CommandText = "select  m.Musteri_no as 'Müş No' , ad+ ' '+ soyad as 'Ad Soyad', m.Oda_no as 'Oda No' from Musteri as m join Hesap as h on m.Musteri_no=h.Musteri_no where " + genel55.secim55 + " Like '%" + dgr + "%'  order by m.oda_no DESC ";
                komut.Connection = yeni;
                SqlDataReader oku = komut.ExecuteReader();
                DataTable tablo = new DataTable();
                tablo.Load(oku); dataGridView1.DataSource = tablo;
                dataGridView1.AllowUserToAddRows = false;

                yeni.Close();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";

            switch (comboBox2.SelectedItem.ToString().Trim())
            {
                case "Ad Soyad":
                    genel55.secim55 = "m.Ad+m.Soyad";
                    break;

                case "Müşteri Numarası":
                    genel55.secim55 = "h.Musteri_no";
                    break;

                case "Oda Numarası":
                    genel55.secim55 = "h.Oda_No";
                    break;

                case "Arama Kriterini Seçin":
                    genel55.secim55 = "10";
                    break;
            }
        }

        private void faturaBilgilerineGitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();

            alma = dataGridView1.CurrentRow.Cells[2].Value.ToString();

            SqlCommand cmd = new SqlCommand("Select * from Hesap where Oda_No = '" + alma + "'", yeni);
            SqlDataReader oku = cmd.ExecuteReader();

            while (oku.Read())
            {
                giris5 = Convert.ToDateTime(oku["Giris_Tarihi"].ToString());
                cikis5 = Convert.ToDateTime(oku["Cikis_Tarihi"].ToString());
            }

            ftrblg.label1.Text = "Oda " + alma;
            ftrblg.dateTimePicker1.Value = giris5;
            ftrblg.dateTimePicker2.Value = cikis5;

            yeni.Close();
            yeni.Open();

            SqlCommand cmd2 = new SqlCommand("Select * from Odalar as o left join Oda_Turleri as ot on o.Oda_Turu=ot.Oda_Turu where o.Oda_No = '" + alma + "'", yeni);
            SqlDataReader oku2 = cmd2.ExecuteReader();

            while (oku2.Read())
            {
                tur5 = oku2["Oda_Turu"].ToString();
                fiy5 = Convert.ToInt32(oku2["fiyat"].ToString());
            }

            ftrblg.label10.Text = tur5;
            ftrblg.label13.Text = fiy5.ToString();
            ftrblg.label2.Text = Convert.ToString(fiy5 * Convert.ToInt32(ftrblg.label8.Text) + " TL");

            yeni.Close();
            yeni.Open();

            SqlCommand komut90 = new SqlCommand();
            komut90.CommandText = "Select  ad+' '+Soyad as 'Ad Soyad',Musteri_no as 'Müşteri No' from Musteri where Oda_no = " + alma + " ";
            komut90.Connection = yeni;
            SqlDataReader oku200 = komut90.ExecuteReader();
            DataTable tablo200 = new DataTable();
            tablo200.Load(oku200);
            ftrblg.dataGridView1.DataSource = tablo200;
            ftrblg.dataGridView1.AllowUserToAddRows = false;

            SqlCommand komut50 = new SqlCommand();
            komut50.CommandText = "Select  Musteri_no as 'Müş No' , Odeme_Tutari as 'Ödeme Tutarı (TL)', Odeme_Tarihi as 'Ödeme Tarihi', Oda_Giris from Fatura where Oda_no = " + alma + "  ";
            komut50.Connection = yeni;
            SqlDataReader oku50 = komut50.ExecuteReader();
            DataTable tablo50 = new DataTable();
            tablo50.Load(oku50);
            ftrblg.dataGridView2.DataSource = tablo50;
            ftrblg.dataGridView2.Columns[3].Visible = false;
            ftrblg.dataGridView2.AllowUserToAddRows = false;

            SqlCommand komut22 = new SqlCommand();
            komut22.CommandText = "Select  Ad,Soyad from Musteri where Oda_no = " + alma + "";
            komut22.Connection = yeni;
            ftrblg.comboBox1.Items.Clear();
            ftrblg.comboBox1.Items.Add("Müşteri Seçiniz");
            SqlDataReader isimver;
            isimver = komut22.ExecuteReader();
            while (isimver.Read())
            {
                ftrblg.comboBox1.Items.Add(isimver["Ad"] + " " + isimver["Soyad"]);
            }
            ftrblg.comboBox1.SelectedIndex = 0;
            ftrblg.comboBox2.SelectedIndex = 0;

            yeni.Close();
            yeni.Open();

            string sorgu222 = "select sum(Odeme_Tutari) from Fatura where Oda_no=" + alma + " ";
            SqlDataAdapter adpp222 = new SqlDataAdapter(sorgu222, yeni);
            DataSet ds22 = new DataSet();
            adpp222.Fill(ds22);

            ftrblg.label26.Text = ds22.Tables[0].Rows[0][0].ToString() + " TL";

            if (!Baslangic.Instance.pnlcontainer.Controls.ContainsKey("faturabilgi"))
            {
                ftrblg.Dock = DockStyle.Fill;
                Baslangic.Instance.pnlcontainer.Controls.Add(ftrblg);
            }
            Baslangic.Instance.pnlcontainer.Controls["faturabilgi"].BringToFront();

            yeni.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (genel55.secim55 == "10")
            {
                MessageBox.Show("Arama Yapabilmek İçin Lütfen Bir Kriter Seçiniz");
                textBox1.Text = "";
            }
            else
            {
                yeni.Close();
                yeni.Open();
                string dgr = textBox1.Text.Replace(" ", "");
                SqlCommand komut = new SqlCommand();
                komut.CommandText = "select  m.Musteri_no as 'Müş No' , ad+ ' '+ soyad as 'Ad Soyad', m.Oda_no as 'Oda No' from Musteri as m join Hesap as h on m.Musteri_no=h.Musteri_no where " + genel55.secim55 + " Like '%" + dgr + "%'  order by m.oda_no DESC ";
                komut.Connection = yeni;
                SqlDataReader oku = komut.ExecuteReader();
                DataTable tablo = new DataTable();
                tablo.Load(oku); dataGridView1.DataSource = tablo;
                dataGridView1.AllowUserToAddRows = false;

                yeni.Close();
            }
        }
    }
}