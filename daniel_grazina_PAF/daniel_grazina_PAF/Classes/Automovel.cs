using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daniel_grazina_PAF
{
	public class Automovel
	{
		private int autoId;
		private string autoMarca;
		private string autoModelo;
		private int autoCilindrada;
		private int autoPotencia;
		private string autoCombustivel;
		private int autoPreco;
		private bool autoVendido;
		private byte[] autoImagem;

		public Automovel() { }

		public Automovel(int autoId, string autoMarca, string autoModelo, int autoCilindrada, int autoPotencia, string autoCombustivel, int autoPreco, bool autoVendido, byte[] autoImagem)
		{
			this.autoId = autoId;
			this.autoMarca = autoMarca;
			this.autoModelo = autoModelo;
			this.autoCilindrada = autoCilindrada;
			this.autoPotencia = autoPotencia;
			this.autoCombustivel = autoCombustivel;
			this.autoPreco = autoPreco;
			this.autoVendido = autoVendido;
			this.autoImagem = autoImagem;
		}

		public int GetAutoId() { return autoId; }
		public string GetAutoMarca() {  return autoMarca; }
		public string GetAutoModelo() { return autoModelo; }
		public int GetAutoCilindrada() {  return autoCilindrada; }
		public int GetAutoPotencia() {  return autoPotencia; }
		public string GetAutoCombustivel() {  return autoCombustivel; }
		public int GetAutoPreco() {  return autoPreco; }
		public bool GetAutoVendido() { return autoVendido; }
		public byte[] GetAutoImagem() {  return autoImagem; }

	}
}
