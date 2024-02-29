using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daniel_grazina_PAF.Classes
{
	public class Vendedor {

		private int vendedorId;
		private String vendedorNome;
		private String vendedorEmail;
		private int vendedorTlm;
		private Byte[] vendedorImagem;

		public Vendedor() { }

		public Vendedor(int vendedorId, string vendedorNome, string vendedorEmail, int vendedorTlm, byte[] vendedorImagem)
		{
			this.vendedorId = vendedorId;
			this.vendedorNome = vendedorNome;
			this.vendedorEmail = vendedorEmail;
			this.vendedorTlm = vendedorTlm;
			this.vendedorImagem = vendedorImagem;
		}

		public int GetVendedorId() {  return vendedorId; }
		public String GetVendedorNome() {  return vendedorNome; }
		public String GetVendedorEmail() {  return vendedorEmail; }
		public int GetVendedorTlm() {  return vendedorTlm; }
		public Byte[] GetVendedorImagem() {  return vendedorImagem; }
	}
}
