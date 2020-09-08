using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WFAWaldos
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usuario = "gsaynes@opengatelogistics.com";//this.txtUsuario.Text.Trim().ToLower();
            string password = "Gsaynes1*";//this.txtPassword.Text;
            if (VerifyUserNamePassword(usuario, password))
            {
                frmMenu frmMenu = new frmMenu(usuario);
                frmMenu.Show();
                this.Hide();
            }            
        }

        public bool VerifyUserNamePassword(string userName, string password)
        {
            
            frmLogin frmLogin = new frmLogin();
            var usermanager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext()));

            if (usermanager.Find(userName, password) != null)
            {                         
                return true;
            }
            else
            {
                MessageBox.Show("USUARIO Y/O CONTRASEÑA INCORRECTOS");
                return false;
            }            
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
