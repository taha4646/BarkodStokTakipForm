using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace BarkodStokTakipForm
{
    public partial class MusteriEkleFrm : Form
    {
        public MusteriEkleFrm()
        {
            InitializeComponent();
        }

        //database bağlantı
        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BarkodStokFormDb;Integrated Security=True");




        private void MusteriEkleFrm_Load(object sender, EventArgs e)
        {

        }
        public int VarMi(string aranan)
        {
            int sonuc;
            baglanti.Open();

            string sorgu = "Select COUNT(tc) from musteri Where tc='" + aranan + "'";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);


            sonuc = Convert.ToInt32(komut.ExecuteScalar());

            baglanti.Close();
            return sonuc;

        }
        private void button1_Click(object sender, EventArgs e)// müşteri ekleme
        {
            if (VarMi(txtTc.Text) != 0)
            {
                MessageBox.Show("Bu TC Numara ile daha önce kayıt yapılmış");
            }
            else
            {
                baglanti.Open();
                if (txtTc.Text != "" & txtAdSoyad.Text != "" & txtTelefon.Text != "" & txtEmail.Text != "" & txtAdres.Text != "")
                {
                    SqlCommand komut = new SqlCommand("insert into musteri(tc,adsoyad,telefon,email,adres) values(@tc,@adsoyad,@telefon,@email,@adres) ", baglanti);
                    komut.Parameters.AddWithValue("@tc", txtTc.Text);
                    komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
                    komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
                    komut.Parameters.AddWithValue("@email", txtEmail.Text);
                    komut.Parameters.AddWithValue("@adres", txtAdres.Text);
                    komut.ExecuteNonQuery();
                   
                    MessageBox.Show("Kayıt İşlemi Yapıldı");

                    foreach (Control item in this.Controls)
                    {

                        if (item is TextBox)
                        {
                            item.Text = "";
                        }



                    }


                }

                else
                {
                    MessageBox.Show("eksik bilgi girdiniz.Boş bırakılan yerleri doldurunuz");
                }
                baglanti.Close();

            }

            
               

            




        }
    }
}
