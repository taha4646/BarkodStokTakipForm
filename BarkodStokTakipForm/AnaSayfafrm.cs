using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    }
}
