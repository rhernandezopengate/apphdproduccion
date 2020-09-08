using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFAWaldos.Entities;

namespace WFAWaldos.Controllers
{
    public class ctrlUsers
    {
		DB_A3F19C_producccionEntities db = new DB_A3F19C_producccionEntities();

		public string UsuarioByEmail(string email)
		{
			try
			{
				string auditor = "";
				string nombre = db.empleados.Where(x => x.Email.Equals(email)).FirstOrDefault().Nombres;
				string ap = db.empleados.Where(x => x.Email.Equals(email)).FirstOrDefault().ApellidoPaterno;
				auditor = nombre + " " + ap;
				return auditor.ToUpper();
			}
			catch (Exception)
			{
				return "";
			}
		}
	}
}
