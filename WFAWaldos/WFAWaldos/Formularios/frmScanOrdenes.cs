using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFAWaldos.Formularios
{
    public partial class frmScanOrdenes : Form
    {
        public frmScanOrdenes(string auditor)
        {
            InitializeComponent();
            this.lblAuditorUsar.Text = auditor;
        }

        private void frmScanOrdenes_Load(object sender, EventArgs e)
        {

        }
    }
}
