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

namespace WFAWaldos
{
    public partial class frmScan : Form
    {
        waldosController ctrlWaldos = new waldosController();
        List<DetalleCaja> listaDetalles;
        public frmScan(string user)
        {
            InitializeComponent();
            this.lblUserUsar.Text = ctrlWaldos.UsuarioByEmail(user);
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
                    EscaneoCaja(caja);                    
                }

                e.Handled = true;
            }
        }

        private void txtSKU_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                int cantidad = 0;                
                string sku = this.txtSKU.Text.Trim().ToUpper();
                var skuexistente = listaDetalles.Where(x => x.sku.Equals(sku)).FirstOrDefault();

                if (skuexistente != null)
                {
                    cantidad = skuexistente.cantidadescaneada + 1;

                    if (cantidad <= skuexistente.cantidad)
                    {
                        skuexistente.cantidadescaneada = skuexistente.cantidadescaneada + 1;
                    }
                    else
                    {
                        Error("YA SE HA COMPLETADO EL ESCANEO DE ESTE SKU", "ERROR DE CANTIDADES");
                    }
                }
                else if(ctrlWaldos.IsKit(sku))
                {
                    //Validar que el kit sea aplicacable a la orden X skus
                    if (ValidarProductosKits(sku))
                    {
                        if (ValidarCantidades(sku))
                        {
                            AgregarCantidadesKit(sku);
                        }
                        else
                        {
                            Error("LA CANTIDAD DE ARTICULOS DE ESTE KIT SUPERA A LA CANTIDAD SOLICITADA", "ERROR DE KITS");
                        }
                    }
                    else
                    {
                        Error("NINGUN PRODUCTO DE ESTE KIT APLICA A ESTA CAJA", "ERROR DE KITS");
                    }
                    //Validar que el kit sea aplicacable a la orden X cantidades
                    //Validar que el kit sea aplicacable a la orden X cantidades escaneadas
                }
                else if (sku.Equals("6") || sku.Equals("12") || sku.Equals("24"))
                {
                    string skucaja = Microsoft.VisualBasic.Interaction.InputBox("INGRESAR SKU DEL KIT");
                    string kit = sku + "-" + skucaja;

                    if (ctrlWaldos.IsKit(kit)) 
                    {
                        if (ValidarProductosKits(kit))
                        {
                            if (ValidarCantidades(kit))
                            {
                                AgregarCantidadesKit(kit);
                            }
                            else
                            {
                                Error("LA CANTIDAD DE ARTICULOS DE ESTE KIT SUPERA A LA CANTIDAD SOLICITADA", "ERROR DE KITS");
                            }
                        }
                        else
                        {
                            Error("NINGUN PRODUCTO DE ESTE KIT APLICA A ESTA CAJA", "ERROR DE KITS");
                        }
                    }
                    else
                    {
                        Error("ESTE KIT NO EXISTE", "ERROR DE KITS");
                    }
                }
                else
                {
                    Error("ESTE SKU NO PERTENECE A LA ORDEN", "ERROR DE SKU");
                }

                int cantidadtotal = listaDetalles.Sum(x => x.cantidad);
                int cantidadescaneados = listaDetalles.Sum(x => x.cantidadescaneada);
                
                dgvDetalles.DataSource = null;
                dgvDetalles.DataSource = listaDetalles;

                if (cantidadtotal == cantidadescaneados)
                {                   
                    if (GuiaCorrecta())
                    {
                        ctrlWaldos.CerrarCaja(this.lblCaja.Text, this.lblUserUsar.Text, this.lblPickerUsar.Text);
                        Limpiar();
                    }                    
                }
                
                this.txtSKU.Text = "";
                this.txtSKU.Focus();

                e.Handled = true;
            }               
        }

        public bool ValidarProductosKits(string kit) 
        {
            foreach (var item in ctrlWaldos.DetalleKit(kit))
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
            foreach (var item in ctrlWaldos.DetalleKit(kit))
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
            foreach (var item in ctrlWaldos.DetalleKit(kit))
            {
                var skuexistente = listaDetalles.Where(x => x.sku.Equals(item.skus.codigobarras)).FirstOrDefault();
                int cantidadsolicitada = listaDetalles.Where(x => x.sku.Equals(item.skus.codigobarras)).FirstOrDefault().cantidad;
                int cantidadescaneada = listaDetalles.Where(x => x.sku.Equals(item.skus.codigobarras)).FirstOrDefault().cantidadescaneada;
                int cantidadkit = (int)item.Cantidad;
                
                skuexistente.cantidadescaneada = skuexistente.cantidadescaneada + cantidadkit;
            }            
        }

        public void EscaneoCaja(string caja) 
        {
            if (ctrlWaldos.ValidarCaja(caja))
            {
                if (ctrlWaldos.IsCajaCerrada(caja))
                {
                    Picker();
                    int idcaja = ctrlWaldos.IdByCaja(caja);
                    this.lblTienda.Text = ctrlWaldos.Tienda(idcaja);
                    this.lblGuia.Text = ctrlWaldos.Guia(idcaja);
                    this.lblCaja.Text = caja;                    
                    listaDetalles = ctrlWaldos.listaDetalle(idcaja);
                    dgvDetalles.DataSource = null;
                    dgvDetalles.DataSource = listaDetalles;
                    this.txtSKU.Enabled = true;
                    this.txtSKU.Focus();
                    this.txtCodigoCaja.Enabled = false;
                }
                else
                {
                    Error("ESTA CAJA ESTA CERRADA", "ERROR EN LA CAJA");
                    Limpiar();
                }                
            }
            else
            {
                Error("ESTA CAJA NO EXISTE", "ERROR EN LA CAJA");
                Limpiar();
            }
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

        public void Picker()
        {            
            bool IsPicker = false;
            do
            {
                SystemSounds.Beep.Play();
                string picker = Microsoft.VisualBasic.Interaction.InputBox("INGRESAR CODIGO DE PICKER.");

                if (ctrlWaldos.IsPicker(picker))
                {
                    IsPicker = true;
                    SystemSounds.Beep.Play();
                    this.lblPickerUsar.Text = picker.ToUpper();
                }
            } while (IsPicker == false);
        }

        public bool GuiaCorrecta()
        {            
            bool respuesta = false;
            do
            {
                SystemSounds.Beep.Play();
                string guia = Microsoft.VisualBasic.Interaction.InputBox("INGRESAR GUIA DE LA CAJA");
                if (ctrlWaldos.BusquedaGuia(this.txtCodigoCaja.Text, guia))
                {
                    SystemSounds.Beep.Play();
                    respuesta = true;
                }
                else
                {
                    SystemSounds.Asterisk.Play();
                }

            } while (respuesta == false);

            return respuesta;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
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

        private void frmScan_Load(object sender, EventArgs e)
        {
            dgvDetalles.AutoGenerateColumns = false;
        }
    }
}
