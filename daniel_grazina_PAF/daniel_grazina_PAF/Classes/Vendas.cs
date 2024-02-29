using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daniel_grazina_PAF.Classes
{
	public class Vendas
	{
		private int vendasId;
		private int vendasModelo;
		private int vendasCliente;
		private int vendasVendedor;
		private int vendasPreco;
		private String vendasData;

		public Vendas() { }
		public Vendas(int vendasId, int vendasModelo, int vendasCliente, int vendasVendedor, int vendasPreco, string vendasData)
		{
			this.vendasId = vendasId;
			this.vendasModelo = vendasModelo;
			this.vendasCliente = vendasCliente;
			this.vendasVendedor = vendasVendedor;
			this.vendasPreco = vendasPreco;
			this.vendasData = vendasData;
		}

		public int GetVendasId() {  return vendasId; }
		public int GetVendasModelo() {  return vendasModelo; }
		public int GetVendasCliente() {  return vendasCliente; }
		public int GetVendasVendedor() {  return vendasVendedor; }
		public int GetVendasPreco() {  return vendasPreco; }
		public String GetVendasData() {  return vendasData; }

	}
}
