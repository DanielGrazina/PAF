using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daniel_grazina_PAF.Classes
{
	public class Cliente
	{
		private int clienteId;
		private String clienteNome;
		private String clienteEmail;
		private int clienteTlm;

		public Cliente() { }

		public Cliente(int clienteId, string clienteNome, string clienteEmail, int clienteTlm)
		{
			this.clienteId = clienteId;
			this.clienteNome = clienteNome;
			this.clienteEmail = clienteEmail;
			this.clienteTlm = clienteTlm;
		}

		public int GetClienteId() {  return clienteId; }
		public String GetClienteNome() {  return clienteNome; }
		public String GetClienteEmail() {  return clienteEmail; }
		public int GetClienteTlm() {  return clienteTlm; }

	}
}
