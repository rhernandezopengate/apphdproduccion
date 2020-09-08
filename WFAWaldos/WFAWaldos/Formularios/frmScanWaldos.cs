using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WFAWaldos.Controllers;
using WFAWaldos.ViewModels;

namespace WFAWaldos.Formularios
{
    public partial class frmScanWaldos : Form
    {        
        //Con Controladores
        List<DetalleCaja> listaDetalles;
        ctrlFolios ctrlFolios = new ctrlFolios();
        ctrlPickers ctrlPickers = new ctrlPickers();
        ctrlTienda ctrlTienda = new ctrlTienda();
        ctrlGuiasWaldos ctrlGuiasWaldos = new ctrlGuiasWaldos();
        ctrlDetalleFolio ctrlDetalles = new ctrlDetalleFolio();
        ctrlKits ctrlKits = new ctrlKits();

        public frmScanWaldos(string user)
        {
            InitializeComponent();
            this.lblUserUsar.Text = user;
        }

        private void txtCodigoCaja_KeyPress(object sender, KeyPressEventArgs e)
        {
            string caja = this.txtCodigoCaja.Text.Trim().ToUpper();
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (caja.Equals(string.Empty))
                {
                    Error("EL CAMPO CAJA NO PUEDE ESTAR VACIO", "ERROR EN LA CAJA");
                }
                else
                {
                    EscaneoFolio(caja);
                }

                e.Handled = true;
            }
        }

        public void EscaneoFolio(string _caja)
        {
            if (ctrlFolios.IsFolioExistente(_caja))
            {
                if (ctrlFolios.IsFolioCerrado(_caja)) 
                {
                    Error("ESTA CAJA ESTA CERRADA", "ERROR EN LA CAJA");
                    Limpiar();
                }
                else
                {
                    Picker();
                    int idcaja = ctrlFolios.IdByFolio(_caja);
                    this.lblTienda.Text = ctrlTienda.TiendaByFolio(idcaja);
                    this.lblGuia.Text = ctrlGuiasWaldos.GuiaByFolio(idcaja);
                    this.lblCaja.Text = _caja;
                    listaDetalles = ctrlDetalles.DetallesFolio(idcaja);
                    dgvDetalles.DataSource = null;
                    dgvDetalles.DataSource = listaDetalles;
                    this.txtSKU.Enabled = true;
                    this.txtSKU.Enabled = true;
                    this.txtSKU.Focus();
                    this.txtCodigoCaja.Enabled = false;
                }
            }
            else
            {
                Error("ESTA CAJA NO EXISTE", "ERROR EN LA CAJA");
                Limpiar();
            }           
        }

        public void Picker()
        {
            bool IsPicker = false;
            do
            {
                SystemSounds.Beep.Play();
                string picker = Microsoft.VisualBasic.Interaction.InputBox("INGRESAR CODIGO DE PICKER.");

                if (ctrlPickers.IsPickerExistente(picker))
                {
                    IsPicker = true;
                    SystemSounds.Beep.Play();
                    this.lblPickerUsar.Text = picker.ToUpper();
                }
            } while (IsPicker == false);
        }

        private void txtSKU_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                int cantidad = 0;
                string sku = this.txtSKU.Text.Trim().ToUpper();
                var skuperteneorden = listaDetalles.Where(x => x.sku.Equals(sku)).FirstOrDefault();

                if (skuperteneorden != null)
                {
                    cantidad = skuperteneorden.cantidadescaneada + 1;

                    if (cantidad <= skuperteneorden.cantidad)
                    {
                        skuperteneorden.cantidadescaneada = skuperteneorden.cantidadescaneada + 1;
                    }
                    else
                    {
                        Error("YA SE HA COMPLETADO EL ESCANEO DE ESTE SKU", "ERROR DE CANTIDADES");
                    }
                }
                else if (ctrlKits.IsKitValido(sku))
                {
                    if (ValidarProductosKits(sku))
                    {
                        if (ValidarCantidades(sku))
                        {
                            AgregarCantidadesKit(sku);
                        }
                        else
                        {
                            Error("LA CANTIDAD DE ARTICULOS DE ESTE KIT SUPERA A LA CANTIDAD SOLICITADA EN EL FOLIO", "ERROR DE KITS");
                        }
                    }
                    else
                    {
                        Error("LOS PRODUCTOS DE ESTE KIT NO APLICAN A ESTA CAJA", "ERROR DE KITS");
                    }
                }                
                else if (sku.Equals("6") || sku.Equals("12") || sku.Equals("24"))
                {
                    //Ingreso de SKUS por cajas completas escanedo directo de la caja fisica
                    string skucaja = Microsoft.VisualBasic.Interaction.InputBox("INGRESAR SKU DEL KIT");
                    string kit = sku + "-" + skucaja;

                    if (ctrlKits.IsKitValido(kit))
                    {
                        if (ValidarProductosKits(kit))
                        {
                            if (ValidarCantidades(kit))
                            {
                                AgregarCantidadesKit(kit);
                            }
                            else
                            {
                                Error("LA CANTIDAD DE ARTICULOS DE ESTE KIT SUPERA A LA CANTIDAD SOLICITADA EN EL FOLIO", "ERROR DE KITS");
                            }
                        }
                        else
                        {
                            Error("LOS PRODUCTOS DE ESTE KIT NO APLICAN A ESTA CAJA", "ERROR DE KITS");
                        }
                    }
                    else
                    {
                        Error("ESTE KIT NO EXISTE", "ERROR DE KITS");
                    }
                }
                else
                {
                    Error("ESTE SKU NO PERTENECE A LA ORDEN", "ERROR EN SKU");
                }

                int cantidadtotal = listaDetalles.Sum(x => x.cantidad);
                int cantidadescaneados = listaDetalles.Sum(x => x.cantidadescaneada);

                dgvDetalles.DataSource = null;
                dgvDetalles.DataSource = listaDetalles;

                if (cantidadtotal == cantidadescaneados)
                {                    
                    GuiaCorrecta();
                    Limpiar();
                }

                this.txtSKU.Text = "";
                this.txtSKU.Focus();

                e.Handled = true;
            }
        }

        public void GuiaCorrecta()
        {
            bool respuesta = false;
            do
            {
                SystemSounds.Beep.Play();
                string guia = Microsoft.VisualBasic.Interaction.InputBox("INGRESAR GUIA DE LA CAJA");

                if (ctrlGuiasWaldos.IsGuiaExistente(guia))
                {
                    if (ctrlGuiasWaldos.IsGuiaPerteneceFolio(this.txtCodigoCaja.Text, guia))
                    {
                        SystemSounds.Beep.Play();
                        respuesta = true;
                        //Cerrar Orden
                        ctrlFolios.CerrarFolio(this.lblCaja.Text, this.lblUserUsar.Text, this.lblPickerUsar.Text);                        
                    }
                    else
                    {
                        SystemSounds.Asterisk.Play();
                        Error("ESTA GUIA NO PERTENECE A LA ORDEN", "ERROR EN LA GUIA");
                    }
                }
                else
                {
                    SystemSounds.Asterisk.Play();
                    Error("ESTA GUIA NO EXISTE", "ERROR EN LA GUIA");
                }
            } while (respuesta == false);            
        }

        public void Error(string mensaje, string encabezado)
        {
            string respuesta = "";

            do
            {
                SystemSounds.Asterisk.Play();
                respuesta = Microsoft.VisualBasic.Interaction.InputBox("ERROR: " + mensaje + "\n\n COLOQUE OK PARA CONTINUAR", encabezado);
            } while (respuesta.Trim().ToUpper() != "OK");
        }

        public void Limpiar()
        {
            this.lblTienda.Text = "0000";
            this.lblCaja.Text = "00000";
            this.lblGuia.Text = "0000000000";
            this.lblPickerUsar.Text = "PICKER ACTUAL";
            this.txtSKU.Text = "";
            this.txtSKU.Enabled = false;
            this.txtCodigoCaja.Enabled = true;
            this.txtCodigoCaja.Text = "";
            this.txtCodigoCaja.Focus();
            listaDetalles = null;
            dgvDetalles.DataSource = null;
            dgvDetalles.DataSource = listaDetalles;
        }
               
        public bool ValidarProductosKits(string kit)
        {
            foreach (var item in ctrlKits.DetalleKit(kit))
            {
                var skuexistente = listaDetalles.Where(x => x.sku.Equals(item.skus.codigobarras)).FirstOrDefault();

                if (skuexistente == null)
                {
                    return false;
                }
            }
            return true;
        }

        public bool ValidarCantidades(string kit)
        {
            foreach (var item in ctrlKits.DetalleKit(kit))
            {
                var skuexistente = listaDetalles.Where(x => x.sku.Equals(item.skus.codigobarras)).FirstOrDefault();
                int cantidadsolicitada = listaDetalles.Where(x => x.sku.Equals(item.skus.codigobarras)).FirstOrDefault().cantidad;
                int cantidadescaneada = listaDetalles.Where(x => x.sku.Equals(item.skus.codigobarras)).FirstOrDefault().cantidadescaneada;
                int cantidadkit = (int)item.Cantidad;
                int total = cantidadsolicitada - (cantidadescaneada + cantidadkit);

                if (total < 0)
                {
                    return false;
                }
            }

            return true;
        }

        public void AgregarCantidadesKit(string kit)
        {
            foreach (var item in ctrlKits.DetalleKit(kit))
            {
                var skuexistente = listaDetalles.Where(x => x.sku.Equals(item.skus.codigobarras)).FirstOrDefault();
                int cantidadsolicitada = listaDetalles.Where(x => x.sku.Equals(item.skus.codigobarras)).FirstOrDefault().cantidad;
                int cantidadescaneada = listaDetalles.Where(x => x.sku.Equals(item.skus.codigobarras)).FirstOrDefault().cantidadescaneada;
                int cantidadkit = (int)item.Cantidad;

                skuexistente.cantidadescaneada = skuexistente.cantidadescaneada + cantidadkit;
            }
        }       

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
        
        private void frmScanWaldos_Load(object sender, EventArgs e)
        {
            dgvDetalles.AutoGenerateColumns = false;
        }        
    }
}