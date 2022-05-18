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
    public partial class MüsteriListelefrm : Form
    {
        public MüsteriListelefrm()
        {
            InitializeComponent();
        }
        //DATABASE BAĞLANTI
        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BarkodStokFormDb;Integrated Security=True");
        DataSet ds = new DataSet();


        private void MüsteriListelefrm_Load(object sender, EventArgs e)
        {
            baglanti.Open();

            Kayit_Göster();

        }

        private void Kayit_Göster()
        {
            SqlDataAdapter adtr = new SqlDataAdapter("select *from musteri", baglanti);
            adtr.Fill(ds, "musteri");
            dataGridView1.DataSource = ds.Tables["musteri"];
            baglanti.Close();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtTc.Text = dataGridView1.CurrentRow.Cells["tc"].Value.ToString();
            txtAdSoyad.Text = dataGridView1.CurrentRow.Cells["adsoyad"].Value.ToString();
            txtTelefon.Text = dataGridView1.CurrentRow.Cells["telefon"].Value.ToString();
            txtEmail.Text = dataGridView1.CurrentRow.Cells["email"].Value.ToString();
            txtAdres.Text = dataGridView1.CurrentRow.Cells["adres"].Value.ToString();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update musteri set adsoyad=@adsoyad,telefon=@telefon,email=@email,adres=@adres where tc=@tc ",baglanti);
            komut.Parameters.AddWithValue("@tc", txtTc.Text);
            komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
            komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
            komut.Parameters.AddWithValue("@email", txtEmail.Text);
            komut.Parameters.AddWithValue("@adres", txtAdres.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            ds.Tables["musteri"].Clear();
            Kayit_Göster();
            MessageBox.Show("Güncelleme İşlemi Yapıldı");

            foreach (Control item in this.Controls)
            {

                if (item is TextBox)
                {
                    item.Text = "";
                }



            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            DialogResult msj;
            msj = MessageBox.Show("silmek istediğinizden eminmisiniz !!!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (msj == DialogResult.Yes)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("delete from musteri where tc='" + dataGridView1.CurrentRow.Cells["tc"].Value.ToString() + "'", baglanti);
                komut.ExecuteNonQuery();
                baglanti.Close();
                ds.Tables["musteri"].Clear();
                Kayit_Göster();
                MessageBox.Show("Kayıt silindi");


            }
           
        }

        private void txtTcara_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo = new DataTable();
            baglanti.Open();
            SqlDataAdapter adtr= new SqlDataAdapter("select * from musteri where tc like '%"+txtTcara.Text+"%'",baglanti);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();


        }
    }
}
