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

namespace BarkodStokTakipForm
{
    public partial class AnaSayfafrm : Form
    {
        public AnaSayfafrm()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (txtTc.Text=="")
            {
                txtAdSoyad.Text = "";
                txtTelefon.Text = "";
            }
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from musteri where tc like '"+ txtTc.Text + "'",baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtAdSoyad.Text = read["adsoyad"].ToString();
                txtTelefon.Text = read["telefon"].ToString();
            }
            baglanti.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MusteriEkleFrm musteriEkleFrm = new MusteriEkleFrm();
            musteriEkleFrm.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MüsteriListelefrm müsteriListelefrm = new MüsteriListelefrm();
            müsteriListelefrm.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            UrunEklefrm urunEklefrm = new UrunEklefrm();
            urunEklefrm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Kategorifrm kategorifrm = new Kategorifrm();
            kategorifrm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Markafrm markafrm = new Markafrm();
            markafrm.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UrunListelefrm urunListelefrm = new UrunListelefrm();
            urunListelefrm.ShowDialog();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BarkodStokFormDb;Integrated Security=True");
        DataSet ds = new DataSet();
        private void sepetlistele()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select *from sepet",baglanti);
            adtr.Fill(ds, "sepet");
            dataGridView1.DataSource = ds.Tables["sepet"];
            dataGridView1.Columns[0].Visible= false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;

            baglanti.Close();


        }

        private void AnaSayfafrm_Load(object sender, EventArgs e)
        {
            sepetlistele();
            hesapla();
        }

        private void txtBarkodNo_TextChanged(object sender, EventArgs e)
        {
            temizle();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from urun where barkodno like '" + txtBarkodNo.Text + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtUrunAdi.Text = read["urunadi"].ToString();
                txtSatisFiyati.Text = read["satisfiyati"].ToString();
            }
            baglanti.Close();
        }

        private void temizle()
        {
            if (txtBarkodNo.Text == "")
            {
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != txtMiktar)
                        {
                            item.Text = "";
                        }
                    }



                }


            }
        }
        bool durum;
        private void barkodkontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from sepet",baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkodNo.Text==read["barkodno"].ToString())
                {
                    durum = false;

                }


            }

            baglanti.Close();
        
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            barkodkontrol();
            if (durum==true)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into sepet(tc,adsoyad,telefon,barkodno,urunadi,miktar,satisfiyati,toplamfiyati,tarih) values(@tc,@adsoyad,@telefon,@barkodno,@urunadi,@miktar,@satisfiyati,@toplamfiyati,@tarih) ", baglanti);
                komut.Parameters.AddWithValue("@tc", txtTc.Text);
                komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
                komut.Parameters.AddWithValue("@barkodno", txtBarkodNo.Text);
                komut.Parameters.AddWithValue("@urunadi", txtUrunAdi.Text);
                komut.Parameters.AddWithValue("@miktar", int.Parse(txtMiktar.Text));
                komut.Parameters.AddWithValue("@satisfiyati", double.Parse(txtSatisFiyati.Text));
                komut.Parameters.AddWithValue("@toplamfiyati", double.Parse(txtToplamFiyat.Text));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
            else
            {
                baglanti.Open();
                SqlCommand komut2 = new SqlCommand("update sepet set miktar=miktar+'"+int.Parse(txtMiktar.Text)+ "' where barkodno='" + txtBarkodNo.Text + "'", baglanti);
                komut2.ExecuteNonQuery();
                SqlCommand komut3 = new SqlCommand("update sepet set toplamfiyati=miktar*satisfiyati where barkodno='"+txtBarkodNo.Text+"'" , baglanti);
                komut3.ExecuteNonQuery();
                baglanti.Close();

            }
           
            txtMiktar.Text = "1";
            ds.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    if (item != txtMiktar)
                    {
                        item.Text = "";
                    }
                }



            }
            

        }

        private void txtMiktar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyat.Text = (double.Parse(txtMiktar.Text) * double.Parse(txtSatisFiyati.Text)).ToString();
            }
            catch (Exception)
            {

                ;
            }
        }

        private void txtSatisFiyati_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyat.Text = (double.Parse(txtMiktar.Text) * double.Parse(txtSatisFiyati.Text)).ToString();
            }
            catch (Exception)
            {

                ;
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from sepet where barkodno='"+dataGridView1.CurrentRow.Cells["barkodno"].Value.ToString()+"'",baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
          
            MessageBox.Show("ürün sepetten çıkarıldı");
            ds.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
        }

        private void btnSatisIptal_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from sepet", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
           
            MessageBox.Show("Sepetteki Ürünler çıkarıldı");
            ds.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SatisListelefrm satisListelefrm = new SatisListelefrm();
            satisListelefrm.ShowDialog();
        }

        private void hesapla()
        {

            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("select sum(toplamfiyati) from sepet", baglanti);
                lblgeneltoplam.Text = komut.ExecuteScalar() + "TL";
                baglanti.Close();


            }
            catch (Exception)
            {

               ;
            }
           

        }

        private void btnSatisYap_Click(object sender, EventArgs e)
        {
            
            for (int i = 0; i <dataGridView1.Rows.Count-1; i++)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into satis(tc,adsoyad,telefon,barkodno,urunadi,miktar,satisfiyati,toplamfiyati,tarih) values(@tc,@adsoyad,@telefon,@barkodno,@urunadi,@miktar,@satisfiyati,@toplamfiyati,@tarih) ", baglanti);
                komut.Parameters.AddWithValue("@tc", txtTc.Text);
                komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
                komut.Parameters.AddWithValue("@barkodno", dataGridView1.Rows[i].Cells["barkodno"].Value.ToString());
                komut.Parameters.AddWithValue("@urunadi", dataGridView1.Rows[i].Cells["urunadi"].Value.ToString());
                komut.Parameters.AddWithValue("@miktar", int.Parse(dataGridView1.Rows[i].Cells["miktar"].Value.ToString()));
                komut.Parameters.AddWithValue("@satisfiyati", double.Parse(dataGridView1.Rows[i].Cells["satisfiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@toplamfiyati", double.Parse(dataGridView1.Rows[i].Cells["toplamfiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                
                SqlCommand komut2 = new SqlCommand("update urun set miktar=miktar-'" + int.Parse(dataGridView1.Rows[i].Cells["miktar"].Value.ToString()) + "' where barkodno='" + dataGridView1.Rows[i].Cells["barkodno"].Value.ToString() + "' ", baglanti);
                komut2.ExecuteNonQuery();
                baglanti.Close();

            }
            baglanti.Open();
            SqlCommand komut3 = new SqlCommand("delete from sepet", baglanti);
            komut3.ExecuteNonQuery();
            baglanti.Close();
            ds.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
        }
    }
}
