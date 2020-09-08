using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WFAWaldos.Controllers;
using WFAWaldos.Formularios;

namespace WFAWaldos
{
    public partial class frmMenu : Form
    {
        ctrlUsers ctrlUsuario = new ctrlUsers();
        public frmMenu(string auditor)
        {
            InitializeComponent();
            this.lblAuditor.Text = ctrlUsuario.UsuarioByEmail(auditor);
        }

        private void btnOrdenes_Click(object sender, EventArgs e)
        {
            var form = Application.OpenForms.OfType<frmScanOrdenes>().FirstOrDefault();
            frmScanOrdenes hijo = form ?? new frmScanOrdenes(this.lblAuditor.Text);
            AddFormInPanel(hijo);
        }

        private void btnWaldos_Click(object sender, EventArgs e)
        {
            var form = Application.OpenForms.OfType<frmScanWaldos>().FirstOrDefault();
            frmScanWaldos hijo = form ?? new frmScanWaldos(this.lblAuditor.Text);
            AddFormInPanel(hijo);
        }

        private void AddFormInPanel(Form fh)
        {
            if (this.panelContenedor.Controls.Count > 0)
                this.panelContenedor.Controls.RemoveAt(0);
            fh.TopLevel = false;
            //fh.FormBorderStyle = FormBorderStyle.None;
            fh.Dock = DockStyle.Fill;
            this.panelContenedor.Controls.Add(fh);
            this.panelContenedor.Tag = fh;
            fh.Show();
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {

        }
    }
}
