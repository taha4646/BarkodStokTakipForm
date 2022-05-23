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
    public partial class BarkodAnasayfafrm : Form
    {
        public BarkodAnasayfafrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BarkodGirisfrm barkodGirisfrm = new BarkodGirisfrm();
            barkodGirisfrm.ShowDialog();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BarkodCıkısfrm barkodCıkısfrm = new BarkodCıkısfrm();
            barkodCıkısfrm.ShowDialog();
        }
    }
}
