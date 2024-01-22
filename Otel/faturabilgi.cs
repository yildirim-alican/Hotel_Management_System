using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Otel
{
    public partial class faturabilgi : UserControl
    {
        public faturabilgi()
        {
            InitializeComponent();

            OdemeTipi.Items.Add("Nakit");
            OdemeTipi.Items.Add("Kredi");
            OdemeTipi.Items.Add("Havale");

        }

        private SqlConnection yeni = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTime bTarih = Convert.ToDateTime(dateTimePicker1.Value);
            DateTime kTarih = Convert.ToDateTime(dateTimePicker2.Value);

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
        }

        public static int yeri3;

        private musekle msk = new musekle();

        public void newWindow()
        {
            msk.FormClosed += nw_FormClosed;
            msk.ShowDialog();
        }

        private void nw_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as musekle;

            form.FormClosed -= nw_FormClosed;

            DialogResult result = form.DialogResult;

            yeni.Close();
            yeni.Open();

            SqlCommand komut90 = new SqlCommand();
            komut90.CommandText = "Select  Musteri_no as 'Müşteri No' , ad+' '+Soyad as 'Ad Soyad' from Musteri where Oda_no = " + label1.Text.Substring(4) + " ";
            komut90.Connection = yeni;
            SqlDataReader oku200 = komut90.ExecuteReader();
            DataTable tablo200 = new DataTable();
            tablo200.Load(oku200);
            dataGridView1.DataSource = tablo200;
            dataGridView1.AllowUserToAddRows = false;

            yeni.Close();
            yeni.Open();
            string sorgu = "Select * from Musteri where Oda_No = '" + label1.Text.Substring(4) + "'";
            SqlDataAdapter adp = new SqlDataAdapter(sorgu, yeni);
            DataSet ds = new DataSet();
            adp.Fill(ds);

            label21.Text = Convert.ToString(ds.Tables[0].Rows.Count);

            SqlCommand komut22 = new SqlCommand();
            komut22.CommandText = "Select  Ad,Soyad from Musteri where Oda_no = " + label1.Text.Substring(4) + "";
            komut22.Connection = yeni;
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Müşteri Seçiniz");
            SqlDataReader isimver;
            isimver = komut22.ExecuteReader();
            while (isimver.Read())
            {
                comboBox1.Items.Add(isimver["Ad"] + " " + isimver["Soyad"]);
            }
            comboBox1.SelectedIndex = 0;

            yeni.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            msk.label1.Text = label1.Text.Substring(4);
            msk.label2.Text = dateTimePicker1.Value.ToString("yyyy/MM/dd").Substring(0, 10).Replace(" ", "").Replace(".", "-");
            msk.label3.Text = dateTimePicker2.Value.ToString("yyyy/MM/dd").Substring(0, 10).Replace(" ", "").Replace(".", "-");
            msk.label4.Text = label21.Text;
            msk.label5.Text = label23.Text;

            newWindow();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();

            SqlCommand komut3 = new SqlCommand();
            komut3.CommandText = "UPDATE Musteri SET Oda_no = null where Musteri_no = '" + yeri3 + "'";
            komut3.Connection = yeni;
            komut3.ExecuteNonQuery();

            yeni.Close();
            yeni.Open();

            SqlCommand komut4 = new SqlCommand();
            komut4.CommandText = "delete from Hesap where  Musteri_no = '" + yeri3 + "'";
            komut4.Connection = yeni;
            komut4.ExecuteNonQuery();

            yeni.Close();
            yeni.Open();

            SqlCommand komut90 = new SqlCommand();
            komut90.CommandText = "Select  Musteri_no as 'Müşteri No' , ad+' '+Soyad as 'Ad Soyad' from Musteri where Oda_no = " + label1.Text.Substring(4) + " ";
            komut90.Connection = yeni;
            SqlDataReader oku200 = komut90.ExecuteReader();
            DataTable tablo200 = new DataTable();
            tablo200.Load(oku200);
            dataGridView1.DataSource = tablo200;
            dataGridView1.AllowUserToAddRows = false;

            yeni.Close();
            yeni.Open();
            string sorgu = "Select * from Musteri where Oda_No = '" + label1.Text.Substring(4) + "'";
            SqlDataAdapter adp = new SqlDataAdapter(sorgu, yeni);
            DataSet ds = new DataSet();
            adp.Fill(ds);

            label21.Text = Convert.ToString(ds.Tables[0].Rows.Count);

            yeni.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            yeri3 = Int32.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
        }

        public static string kac;

        private void button5_Click(object sender, EventArgs e)
        {
            yeni.Close();
            yeni.Open();

            string selectedPaymentType = OdemeTipi.SelectedItem.ToString();

            if (comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("Müşteri Seçimi Yapmanız Gerek");
            }
            else
            {
                if ((Convert.ToInt16(textBox1.Text)) <= (Convert.ToInt16(label19.Text.Replace("TL", ""))))
                {
                    string sorgu = "Select * from Musteri where Oda_No = '" + label1.Text.Substring(4) + "'";
                    SqlDataAdapter adp = new SqlDataAdapter(sorgu, yeni);
                    DataSet ds = new DataSet();
                    adp.Fill(ds);

                    kac = ds.Tables[0].Rows[comboBox1.SelectedIndex - 1][0].ToString();

                    SqlCommand komut2 = new SqlCommand();
                    komut2.CommandText = "insert into Fatura(Musteri_no,Oda_Giris,Oda_Cikis,Oda_no,Odeme_Tarihi,Odeme_Tutari,Odeme_Tipi) values(@bMusteri_no,@bOda_Giris,@bOda_Cikis,@bOda_no,@bOdeme_Tarihi,@bOdeme_Tutari,@bOdeme_Tipi)";
                    komut2.Connection = yeni;

                    SqlParameter bMusteri_no = new SqlParameter();
                    bMusteri_no.ParameterName = "@bMusteri_no";
                    bMusteri_no.SqlDbType = SqlDbType.Int;
                    bMusteri_no.Size = 50;
                    bMusteri_no.Value = Convert.ToInt32(kac);
                    komut2.Parameters.Add(bMusteri_no);

                    SqlParameter bOdeme_Tutari = new SqlParameter();
                    bOdeme_Tutari.ParameterName = "@bOdeme_Tutari";
                    bOdeme_Tutari.SqlDbType = SqlDbType.Int;
                    bOdeme_Tutari.Size = 50;
                    bOdeme_Tutari.Value = textBox1.Text;
                    komut2.Parameters.Add(bOdeme_Tutari);

                    SqlParameter bOda_no = new SqlParameter();
                    bOda_no.ParameterName = "@bOda_no";
                    bOda_no.SqlDbType = SqlDbType.Int;
                    bOda_no.Size = 50;
                    bOda_no.Value = Convert.ToInt32(label1.Text.Substring(4));
                    komut2.Parameters.Add(bOda_no);

                    SqlParameter bOda_Giris = new SqlParameter();
                    bOda_Giris.ParameterName = "@bOda_Giris";
                    bOda_Giris.SqlDbType = SqlDbType.Date;
                    bOda_Giris.Size = 50;
                    bOda_Giris.Value = dateTimePicker1.Value;
                    komut2.Parameters.Add(bOda_Giris);

                    SqlParameter bOda_Cikis = new SqlParameter();
                    bOda_Cikis.ParameterName = "@bOda_Cikis";
                    bOda_Cikis.SqlDbType = SqlDbType.Date;
                    bOda_Cikis.Size = 50;
                    bOda_Cikis.Value = dateTimePicker2.Value;
                    komut2.Parameters.Add(bOda_Cikis);

                    SqlParameter bOdeme_Tarihi = new SqlParameter();
                    bOdeme_Tarihi.ParameterName = "@bOdeme_Tarihi";
                    bOdeme_Tarihi.SqlDbType = SqlDbType.Date;
                    bOdeme_Tarihi.Size = 50;
                    bOdeme_Tarihi.Value = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    komut2.Parameters.Add(bOdeme_Tarihi);

                    SqlParameter bOdeme_Tipi = new SqlParameter();
                    bOdeme_Tipi.ParameterName = "@bOdeme_Tipi";
                    bOdeme_Tipi.SqlDbType = SqlDbType.VarChar;
                    bOdeme_Tipi.Size = 50;
                    bOdeme_Tipi.Value = selectedPaymentType;
                    komut2.Parameters.Add(bOdeme_Tipi);

                    komut2.ExecuteNonQuery();

                    textBox1.Clear();
                    MessageBox.Show("Ödeme Yapıldı");

                    SqlCommand komut50 = new SqlCommand();
                    komut50.CommandText = "Select  Musteri_no as 'Müş No' , Odeme_Tutari as 'Ödeme Tutarı (TL)', Odeme_Tarihi as 'Ödeme Tarihi', Odeme_Tipi as 'Odeme Tipi', Oda_Giris from Fatura where Oda_no = " + label1.Text.Substring(4) + "  ";
                    komut50.Connection = yeni;
                    SqlDataReader oku50 = komut50.ExecuteReader();
                    DataTable tablo50 = new DataTable();
                    tablo50.Load(oku50);
                    dataGridView2.DataSource = tablo50;
                    dataGridView2.Columns[4].Visible = false;
                    dataGridView2.AllowUserToAddRows = false;

                    string sorgu222 = "select sum(Odeme_Tutari) from Fatura where Oda_no=" + label1.Text.Substring(4) + " ";
                    SqlDataAdapter adpp222 = new SqlDataAdapter(sorgu222, yeni);
                    DataSet ds22 = new DataSet();
                    adpp222.Fill(ds22);

                    label26.Text = ds22.Tables[0].Rows[0][0].ToString() + " TL";

                    label19.Text = Convert.ToString((Convert.ToInt32(label17.Text.Replace("TL", ""))) - (Convert.ToInt32(ds22.Tables[0].Rows[0][0].ToString()))) + " TL";
                }
                else
                {
                    MessageBox.Show("Kalan Tutardan Fazla Bir Miktar Girdiniz");
                }

                if (label19.Text == "0 TL")
                {
                    checkBox1.Checked = true;
                    checkBox1.Text = "Ödeme Tamamlandı";
                }
                else
                {
                    checkBox1.Checked = false;
                    checkBox1.Text = "Ödeme Tamamlanmadı";
                }

                yeni.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                yeni.Close();
                yeni.Open();

                SqlCommand komut4 = new SqlCommand();
                komut4.CommandText = "delete from Hesap where  Oda_No = '" + label1.Text.Substring(4) + "'";
                komut4.Connection = yeni;
                komut4.ExecuteNonQuery();

                SqlCommand komut45 = new SqlCommand();
                komut45.CommandText = "delete from Fatura where  Oda_No = '" + label1.Text.Substring(4) + "'";
                komut45.Connection = yeni;
                komut45.ExecuteNonQuery();

                SqlCommand komut455 = new SqlCommand();
                komut455.CommandText = "delete from Ekstra where  Oda_No = '" + label1.Text.Substring(4) + "'";
                komut455.Connection = yeni;
                komut455.ExecuteNonQuery();

                SqlCommand komut3 = new SqlCommand();
                komut3.CommandText = "UPDATE Musteri SET Oda_no = null where Oda_No ='" + label1.Text.Substring(4) + "'";
                komut3.Connection = yeni;
                komut3.ExecuteNonQuery();
                MessageBox.Show("Oda Çıkışı Yapıldı");

                SqlCommand komut5 = new SqlCommand();
                komut5.CommandText = "UPDATE Odalar SET Doluluk = 0 where Oda_No = '" + label1.Text.Substring(4) + "'";
                komut5.Connection = yeni;

                komut5.ExecuteNonQuery();

                this.Hide();
                fatura ftr = new fatura();

                if (!Baslangic.Instance.pnlcontainer.Controls.ContainsKey("fatura"))
                {
                    ftr.Dock = DockStyle.Fill;
                    Baslangic.Instance.pnlcontainer.Controls.Add(ftr);
                }
                Baslangic.Instance.pnlcontainer.Controls["fatura"].BringToFront();
                Baslangic.Instance.pnlcontainer.Controls["fatura"].Show();

                yeni.Close();
            }
            else
            {
                MessageBox.Show("Çıkışı Gerçekleştirebilmek İçin Ödeme Tamamlanmalıdır.");
            }
        }

        private Ekstram eks = new Ekstram();

        public void new2Window()
        {
            eks.FormClosed += nw2_FormClosed;
            eks.ShowDialog();
        }

        private void nw2_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as Ekstram;

            form.FormClosed -= nw2_FormClosed;

            DialogResult result = form.DialogResult;

            MessageBox.Show("Iptal Edildi!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            eks.label1.Text = label1.Text;
            new2Window();
        }

        private Kayitiptal kyti = new Kayitiptal();

        public void new3Window()
        {
            kyti.FormClosed += nw3_FormClosed;
            kyti.ShowDialog();
        }

        private void nw3_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as Kayitiptal;

            form.FormClosed -= nw3_FormClosed;

            DialogResult result = form.DialogResult;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            new3Window();
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            // Declare a variable to keep track of vertical position
            int yOffset = 10;

            // Hotel Name
            e.Graphics.DrawString("DPU HOTEL", new Font("Arial", 17, FontStyle.Bold), Brushes.Black, new PointF(15, yOffset));
            yOffset += 30;

            // Invoice Title
            e.Graphics.DrawString("Odeme Faturasi", new Font("Arial", 13, FontStyle.Bold), Brushes.Black, new PointF(12, yOffset));
            yOffset += 40;

            e.Graphics.DrawString("Müşteri Bilgileri", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new PointF(12, yOffset));

            // Table Header
            yOffset += 30;

            // Customer Information
            e.Graphics.DrawString($"{comboBox1.SelectedItem}", new Font("Arial", 14), Brushes.Black, new PointF(10, yOffset));

            yOffset += 30;

            // Check-in and Check-out Dates
            e.Graphics.DrawString($"Giriş Tarihi: {dateTimePicker1.Value.ToString("yyyy/MM/dd")}", new Font("Arial", 12), Brushes.Black, new PointF(10, yOffset));
            yOffset += 20;
            e.Graphics.DrawString($"Çıkış Tarihi: {dateTimePicker2.Value.ToString("yyyy/MM/dd")}", new Font("Arial", 12), Brushes.Black, new PointF(10, yOffset));
            yOffset += 40;

            // Payment Details Table
            e.Graphics.DrawString("Ödeme Detayları", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new PointF(10, yOffset));
            yOffset += 20;

            // Table Header
            e.Graphics.DrawString("Tutar (TL)", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new PointF(10, yOffset));
            e.Graphics.DrawString("Ödeme Tarihi", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new PointF(150, yOffset));
            yOffset += 20;

            // Payment Details
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                decimal paymentAmount = Convert.ToDecimal(row.Cells["Ödeme Tutarı (TL)"].Value);
                DateTime paymentDate = Convert.ToDateTime(row.Cells["Ödeme Tarihi"].Value);

                e.Graphics.DrawString($"{paymentAmount} TL", new Font("Arial", 12), Brushes.Black, new PointF(10, yOffset));
                e.Graphics.DrawString(paymentDate.ToString("yyyy/MM/dd"), new Font("Arial", 12), Brushes.Black, new PointF(150, yOffset));

                yOffset += 20;
            }

            // Total Amount
            e.Graphics.DrawString($"Toplam Tutar: {label26.Text}", new Font("Arial", 14, FontStyle.Bold), Brushes.Black, new PointF(10, yOffset));
            yOffset += 30;

            // Fatura Tarihi
            e.Graphics.DrawString($"Fatura Tarihi: {DateTime.Now.ToString("yyyy/MM/dd")}", new Font("Arial", 12), Brushes.Black, new PointF(10, yOffset));
            yOffset += 20;


        }
        private void button9_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(PrintPage);
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                pd.Print();
            }
        }
    }
}