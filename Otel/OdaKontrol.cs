using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Otel
{
    public partial class OdaKontrol : Form
    {
        private SqlConnection baglanti = new SqlConnection("Data Source=" + veribaglanma.baglantiyeri + " ; Initial Catalog=" + veribaglanma.veritabanı + "; Integrated Security = True");
        private SqlCommand komut = new SqlCommand();

        public OdaKontrol()
        {
            InitializeComponent();
        }

        public void kayit_al()
        {
            listBox1.Items.Clear();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT * FROM Odalar", baglanti);
            SqlDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                listBox1.Items.Add(oku["Oda_No"]);
            }
            baglanti.Close();
        }

        private void OdaKontrol_Load(object sender, EventArgs e)
        {
            kayit_al();
            panel3.Location = new Point(panel1.Width + 1, panel2.Height + 1);
            panel3.Size = new Size((this.Width - panel1.Width), (this.Height - panel2.Height - 50));
            listBox1.Height = this.Height - listBox1.Location.Y - 80;
            yataklar();

            BtnDirty = new Button();
            BtnDirty.Text = "Kirli";
            BtnDirty.Location = new Point(20, this.Height - 60);
            BtnDirty.Click += BtnDirty_Click;
            this.Controls.Add(BtnDirty);

            BtnClean = new Button();
            BtnClean.Text = "Temiz";
            BtnClean.Location = new Point(140, this.Height - 60);
            BtnClean.Click += BtnClean_Click;
            this.Controls.Add(BtnClean);

        }

        private void BtnDirty_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                int selectedRoom = Convert.ToInt32(listBox1.SelectedItem);

                // Check if the room is empty (Doluluk == 0) before marking it dirty
                int doluluk = 0;

                baglanti.Open();
                SqlCommand dirtyCommand = new SqlCommand();
                dirtyCommand.Connection = baglanti;

                dirtyCommand.CommandText = "SELECT Doluluk FROM Odalar WHERE Oda_No = @roomNo";
                dirtyCommand.Parameters.AddWithValue("@roomNo", selectedRoom);

                SqlDataReader dolulukReader = dirtyCommand.ExecuteReader();
                if (dolulukReader.Read())
                {
                    doluluk = Convert.ToInt32(dolulukReader["Doluluk"]);
                }
                dolulukReader.Close();
                baglanti.Close();

                if (doluluk == 0)
                {
                    // Update DirtyRoom to 1 (dirty) in the database
                    baglanti.Open();
                    SqlCommand updateCommand = new SqlCommand();
                    updateCommand.Connection = baglanti;

                    updateCommand.CommandText = "UPDATE Odalar SET DirtyRoom = 1 WHERE Oda_No = @roomNo";
                    updateCommand.Parameters.AddWithValue("@roomNo", selectedRoom);

                    updateCommand.ExecuteNonQuery();
                    baglanti.Close();

                    // Refresh room status
                    yataklar();
                }
                else
                {
                    MessageBox.Show("Odada kalan musteri var!");
                }
            }
            else
            {
                MessageBox.Show("Lutfen bir oda seciniz.");
            }
        }


        private void BtnClean_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                int selectedRoom = Convert.ToInt32(listBox1.SelectedItem);

                // Check if the room is empty (Doluluk == 0) before marking it clean
                int doluluk = 0;

                baglanti.Open();
                SqlCommand cleanCommand = new SqlCommand();
                cleanCommand.Connection = baglanti;

                cleanCommand.CommandText = "SELECT Doluluk FROM Odalar WHERE Oda_No = @roomNo";
                cleanCommand.Parameters.AddWithValue("@roomNo", selectedRoom);

                SqlDataReader dolulukReader = cleanCommand.ExecuteReader();
                if (dolulukReader.Read())
                {
                    doluluk = Convert.ToInt32(dolulukReader["Doluluk"]);
                }
                dolulukReader.Close();
                baglanti.Close();

                if (doluluk == 0)
                {
                    // Update DirtyRoom to 0 (clean) in the database
                    baglanti.Open();
                    SqlCommand updateCommand = new SqlCommand();
                    updateCommand.Connection = baglanti;

                    updateCommand.CommandText = "UPDATE Odalar SET DirtyRoom = 0 WHERE Oda_No = @roomNo";
                    updateCommand.Parameters.AddWithValue("@roomNo", selectedRoom);

                    updateCommand.ExecuteNonQuery();
                    baglanti.Close();

                    // Refresh room status
                    yataklar();
                }
                else
                {
                    MessageBox.Show("Odada kalan musteri var.");
                }
            }
            else
            {
                MessageBox.Show("Lutfen bir oda seciniz.");
            }
        }

        public void yataklar()
        {
            panel3.Controls.Clear();
            int x = 20;
            int y = 10;
            Panel pnlyatak;

            foreach (int item in listBox1.Items)
            {
                if (panel1.Width + x + 120 > (panel3.Location.X + panel3.Width))
                {
                    x = 20;
                    y += 120;
                }

                int doluluk = 0; // Assuming Doluluk is of type int in the database
                int dirtyRoom = 0; // Assuming DirtyRoom is of type bit in the database

                // Retrieve Doluluk and DirtyRoom values from the database
                baglanti.Open();
                komut.Connection = baglanti;
                komut.CommandText = "SELECT Doluluk, DirtyRoom FROM Odalar WHERE Oda_No='" + item + "'";
                SqlDataReader roomReader = komut.ExecuteReader();
                if (roomReader.Read())
                {
                    if (roomReader["Doluluk"] != DBNull.Value)
                    {
                        doluluk = Convert.ToInt32(roomReader["Doluluk"]);
                    }

                    if (roomReader["DirtyRoom"] != DBNull.Value)
                    {
                        dirtyRoom = Convert.ToInt32(roomReader["DirtyRoom"]);
                    }
                }
                roomReader.Close();
                baglanti.Close();

                pnlyatak = new Panel();
                pnlyatak.Size = new Size(100, 110);
                pnlyatak.Location = new Point(x, y);
                pnlyatak.BackgroundImageLayout = ImageLayout.Stretch;
                pnlyatak.Name = item.ToString(); // Convert item to string for the Name

                Label odaism = new Label();
                odaism.Text = item.ToString();
                odaism.Size = new Size(pnlyatak.Width, 50);
                odaism.Location = new Point(x, ((y + pnlyatak.Height) - odaism.Height));
                odaism.TextAlign = ContentAlignment.MiddleCenter;

                // Set background image or color based on Doluluk and DirtyRoom values
                if (doluluk == 1)
                {
                    pnlyatak.BackgroundImage = Resource1.kirmizi_yatak; // Room with customer
                }
                else if (dirtyRoom == 1)
                {
                    pnlyatak.BackgroundImage = Resource1.mavi_yatak; // Dirty room
                }
                else
                {
                    pnlyatak.BackgroundImage = Resource1.yesil_yatak; // Empty and clean room
                }

                panel3.Controls.Add(pnlyatak);
                pnlyatak.BringToFront();
                panel3.Controls.Add(odaism);
                odaism.BringToFront();
                panel3.SendToBack();
                x += 120;
                pnlyatak.Click += Pnlyatak_Click;
            }
        }




        private void Pnlyatak_Click(object sender, EventArgs e)
        {
            Panel panelClick = (Panel)sender;
            baglanti.Open();
            komut.Connection = baglanti;
            komut.CommandText = "SELECT Musteri.Ad, Musteri.Soyad, Musteri.Musteri_no, Musteri.Telefon_no, Musteri.E_posta, Musteri.Cinsiyet, Musteri.Oda_no, Hesap.Giris_Tarihi, Hesap.Cikis_Tarihi FROM Musteri INNER JOIN Hesap ON Musteri.Musteri_no = Hesap.Musteri_no WHERE Musteri.Oda_no='" + panelClick.Name + "'";
            SqlDataReader oku = komut.ExecuteReader();

            string kisiler = "";

            while (oku.Read())
            {
                kisiler += "\n Ad : " + oku["Ad"] + "\n Soyad : " + oku["Soyad"] + "\n Mus No : " + oku["Musteri_no"] + "\n Telefon : " + oku["Telefon_no"] + "\n Mail : " + oku["E_posta"] + "\n Cinsiyet : " + oku["Cinsiyet"] + "\n Oda : " + oku["Oda_no"] + "\n Giriş Tarihi: " + oku["Giris_Tarihi"] + "\n Çıkış Tarihi: " + oku["Cikis_Tarihi"] + "\n-----------------------------------------";
            }

            oku.Close();
            baglanti.Close();

            if (kisiler != "")
            {
                MessageBox.Show(kisiler);
            }

        }
    }
}
