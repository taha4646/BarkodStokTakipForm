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
    public partial class SatisListelefrm : Form
    {
        public SatisListelefrm()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BarkodStokFormDb;Integrated Security=True");
        DataSet ds = new DataSet();
        private void satislistele()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select *from satis", baglanti);
            adtr.Fill(ds, "satis");
            dataGridView1.DataSource = ds.Tables["satis"];
            

            baglanti.Close();


        }
        private void SatisListelefrm_Load(object sender, EventArgs e)
        {
            satislistele();
        }
    }
}
