using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace BarkodStokTakipForm
{
    public partial class BarkodGirisfrm : Form
    {
        public BarkodGirisfrm()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BarkodStokFormDb;Integrated Security=True");
        DataSet ds = new DataSet();
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        public int VarMi(string aranan)
        {
            int sonuc;
            baglanti.Open();

            string sorgu = "Select COUNT(barkodno) from urun Where barkodno='" + aranan + "'";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);


            sonuc = Convert.ToInt32(komut.ExecuteScalar());

            baglanti.Close();
            return sonuc;

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BarkodGirisfrm_Load(object sender, EventArgs e)
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in filterInfoCollection)
            {
                cboCamera.Items.Add(device.Name);
                cboCamera.SelectedIndex = 0;

            }
            UrunListele();
            kategorigetir();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cboCamera.SelectedIndex].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            videoCaptureDevice.Start();
        }

        private void VideoCaptureDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            BarcodeReader reader = new BarcodeReader();
            var result = reader.Decode(bitmap);
            if (result != null)
            {

                txtBarkod.Invoke(new MethodInvoker(delegate ()
                {
                    txtBarkod.Text = result.ToString();
                }));

            }
            pictureBox.Image = bitmap;
        }

        private void BarkodGirisfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoCaptureDevice != null)
            {
                if (videoCaptureDevice.IsRunning)
                    videoCaptureDevice.Stop();
            }
        }

        private void txtBarkod_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo = new DataTable();
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from urun where barkodno like '%" + txtBarkod.Text + "%'", baglanti);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
        private void UrunListele()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from urun", baglanti);
            adtr.Fill(ds, "urun");
            dataGridView1.DataSource = ds.Tables["urun"];
            baglanti.Close();
        }
        public void kategorigetir()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from kategoribilgileri", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboKategori.Items.Add(read["kategori"].ToString());
            }
            baglanti.Close();
        }
       
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)//varolana ekle butonu
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update urun set miktar=miktar+'" + Miktaritxt.Text + "' where barkodno='" + txtBarkod.Text + "' ", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            foreach (Control item in Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";

                }
                if (item is ComboBox)
                {
                    item.Text = "";

                }
            }
        }
        bool durum;
        private void barkodengelle()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from urun", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkodNo.Text == read["barkodno"].ToString() || txtBarkodNo.Text == "")
                {
                    durum = false;
                }
            }
            baglanti.Close();


        }
        private void btnYeniEkle_Click(object sender, EventArgs e)
        {
            barkodengelle();
            if (durum == true)
            {

                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into urun(barkodno,kategori,marka,urunadi,miktar,alisfiyati,satisfiyati,tarih) values(@barkodno,@kategori,@marka,@urunadi,@miktar,@alisfiyati,@satisfiyati,@tarih)", baglanti);
                komut.Parameters.AddWithValue("@barkodno", txtBarkodNo.Text);
                komut.Parameters.AddWithValue("@kategori", comboKategori.Text);
                komut.Parameters.AddWithValue("@marka", comboMarka.Text);
                komut.Parameters.AddWithValue("@urunadi", txtUrunadi.Text);
                komut.Parameters.AddWithValue("@miktar", int.Parse(txtMiktari.Text));
                komut.Parameters.AddWithValue("@alisfiyati", double.Parse(txtAlisFiyati.Text));
                komut.Parameters.AddWithValue("@satisfiyati", double.Parse(txtSatisFiyati.Text));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                baglanti.Close();
                comboMarka.Items.Clear();
                MessageBox.Show("Ürün eklendi");

            }
            else
            {
                MessageBox.Show("aynı nolu barkod kaydı vardır");
            }
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
                if (item is ComboBox)
                {
                    item.Text = "";
                }

            }

        }

        private void txtBarkodNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboKategori_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            comboMarka.Items.Clear();
            comboMarka.Text = "";
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from markabilgileri where kategori='" + comboKategori.SelectedItem + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboMarka.Items.Add(read["marka"].ToString());
            }
            baglanti.Close();
        }

        private void BtnTemizle1_Click(object sender, EventArgs e)
        {
            txtBarkod.Text = "";
            Miktaritxt.Text = "1";
        }

        private void btnTemizle2_Click(object sender, EventArgs e)
        {
            txtBarkodNo.Text = "";
            comboKategori.Text = "";
            comboMarka.Text = "";
           txtUrunadi.Text = "";
            txtMiktari.Text = "";
            txtAlisFiyati.Text = "";
            txtSatisFiyati.Text = "";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Kategorifrm kategorifrm = new Kategorifrm();
            kategorifrm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Markafrm markafrm = new Markafrm();
            markafrm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UrunListelefrm urunListelefrm = new UrunListelefrm();
            urunListelefrm.ShowDialog();
        }
    }
}
