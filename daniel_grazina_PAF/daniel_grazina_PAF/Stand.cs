using daniel_grazina_PAF.Classes;
using daniel_grazina_PAF.Properties;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace daniel_grazina_PAF
{
	public partial class Stand : Form
	{
		//Variaveis para DataBase
		MySqlConnection con;
		MySqlCommand cmd;
		MySqlCommand cmd2;
		MySqlDataReader dr;

		//Variaveis para receber nome e nivel do utilizador "logado"
		public String UserNome_gl;
		public String UserNivel_gl;

		//Variveis para numero de registos na DataBase
		private int NumAutoDB;
		private int NumVendedorDB;
		private int NumClientDB;
		private int NumVendasDB;
		private int NumUtilizadoresDB;

		//Variaveis para guardar o objeto de cada tipo
		private int indexAuto = 0;
		private int indexVendedor = 0;
		private int indexCliente = 0;
		private int indexVendas = 0;
		private int indexUtilizadores = 0;

		//Variaveis para declarar objetos para cada class
		private Automovel[] auto;
		private Vendedor[] vendedor;
		private Cliente[] cliente;
		private Vendas[] vendas;
		private Utilizador[] utilizador;

		public Stand()
		{
			//Coneccao base de dados
			con = new MySqlConnection("Server=127.0.0.1;Database=AutoCar_Daniel_Grazina;Uid=root;Pwd=Qop2006a;");
			InitializeComponent();
		}


		/*
		#############
		#           #
		#  Funções  #
		#           #
		#############
		*/

		//Função para contagem de registos na DataBase
		private void GetCount(String nomeTabela)
		{
			cmd2 = new MySqlCommand
			{
				Connection = con,
				CommandText = "select count(*) from " + nomeTabela //Query para buscar o numero de registos numa tabela
			};

			//Deacordo com a tabela pedida guardar na variavel o numero de registos
			switch (nomeTabela)
			{
				case "tbl_automovel":
					NumAutoDB = Convert.ToInt32(cmd2.ExecuteScalar());
					break;
				case "tbl_vendedor":
					NumVendedorDB = Convert.ToInt32(cmd2.ExecuteScalar());
					break;
				case "tbl_cliente":
					NumClientDB = Convert.ToInt32(cmd2.ExecuteScalar());
					break;
				case "tbl_vendas":
					NumVendasDB = Convert.ToInt32(cmd2.ExecuteScalar());
					break;
				case "tbl_utilizadores":
					NumUtilizadoresDB = Convert.ToInt32(cmd2.ExecuteScalar());
					break;
			}
		}

		//Função para guardar os registos de uma tabela numa classe
		private void GuardarClass(String nomeTabela)
		{
			con.Open();
			cmd = new MySqlCommand
			{
				Connection = con,
				CommandText = "Select * from " + nomeTabela //Query para buscar os registos de uma tabela
			};

			//Variavel para guardar diferentes objetos
			int i = 0;

			//Verificação da tabela para guardar as imformações na classe correta
			switch (nomeTabela)
			{
				case "tbl_automovel":
					//Buscar a contagem de registos da tabela (esta função é executada sempre para caso seja adicionado algum registo novo)
					GetCount("tbl_automovel");
					//Mudar label com a contagem de automoveis
					lblNumeroAuto.Text = NumAutoDB.ToString();
					//Definir o tamanho do vetor de acordo com o numero de registos
					auto = new Automovel[NumAutoDB];
					dr = cmd.ExecuteReader();
					//sempre que lê um registo relaciona-lo com um objeto da classe
					while (dr.Read())
					{
						object imagemObj = dr["auto_imagem"];
						byte[] imagem = imagemObj != DBNull.Value ? (byte[])imagemObj : null;
						auto[i] = new Automovel(Convert.ToInt32(dr["auto_id"]), dr["auto_marca"].ToString(), dr["auto_modelo"].ToString(), Convert.ToInt32(dr["auto_cilindrada"]), Convert.ToInt32(dr["auto_potencia"]), dr["auto_combustivel"].ToString(), Convert.ToInt32(dr["auto_preco"]), Convert.ToBoolean(dr["auto_vendido"]), imagem);
						i++;
					}
					break;
				case "tbl_vendedor":
					GetCount("tbl_vendedor");
					vendedor = new Vendedor[NumVendedorDB];
					dr = cmd.ExecuteReader();
					while (dr.Read())
					{
						object imagemObj = dr["vendedor_imagem"];
						byte[] imagem = imagemObj != DBNull.Value ? (byte[])imagemObj : null;
						vendedor[i] = new Vendedor(Convert.ToInt32(dr["vendedor_id"]), dr["vendedor_nome"].ToString(), dr["vendedor_email"].ToString(), Convert.ToInt32(dr["vendedor_tlm"]), imagem);
						i++;
					}
					break;
				case "tbl_cliente":
					GetCount("tbl_cliente");
					cliente = new Cliente[NumClientDB];
					dr = cmd.ExecuteReader();
					while (dr.Read())
					{
						cliente[i] = new Cliente(Convert.ToInt32(dr["cliente_id"]), dr["cliente_nome"].ToString(), dr["cliente_email"].ToString(), Convert.ToInt32(dr["cliente_tlm"]));
						i++;
					}
					break;
				case "tbl_vendas":
					GetCount("tbl_vendas");
					vendas = new Vendas[NumVendasDB];
					dr = cmd.ExecuteReader();
					while (dr.Read())
					{
						vendas[i] = new Vendas(Convert.ToInt32(dr["venda_id"]), Convert.ToInt32(dr["venda_modelo"]), Convert.ToInt32(dr["venda_cliente"]), Convert.ToInt32(dr["venda_vendedor"]), Convert.ToInt32(dr["venda_preco"]), dr["venda_data"].ToString());
						i++;
					}
					break;
				case "tbl_utilizadores":
					GetCount("tbl_utilizadores");
					utilizador = new Utilizador[NumUtilizadoresDB];
					dr = cmd.ExecuteReader();
					while (dr.Read())
					{
						utilizador[i] = new Utilizador(Convert.ToInt32(dr["lg_id"]), dr["lg_nome"].ToString(), dr["lg_password"].ToString(), dr["lg_nivel"].ToString());
						i++;
					}
					break;
			}
			con.Close();
		}

		//Função para introduzir dados nas combobox do separador vendas
		public void EnviarDadosComboBox()
		{
			con.Open();

			//Query para recolher modelos registados na DataBase
			cmd = new MySqlCommand
			{
				Connection = con,
				CommandText = "Select auto_id, auto_modelo from tbl_automovel"
			};
			MySqlDataAdapter da = new MySqlDataAdapter(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);

			//Introduzir os modelos na combobox dos modelos
			cbModeloVendas.DataSource = dt;
			cbModeloVendas.DisplayMember = "auto_modelo";
			cbModeloVendas.ValueMember = "auto_id";

			//Query para recolher cliente registados na DataBase
			cmd.CommandText = "Select cliente_id, cliente_nome from tbl_cliente";
			da = new MySqlDataAdapter(cmd);
			dt = new DataTable();
			da.Fill(dt);
			//Introduzir os clientes na combobox dos modelos
			cbClienteVendas.DataSource = dt;
			cbClienteVendas.DisplayMember = "cliente_nome";
			cbClienteVendas.ValueMember = "cliente_id";

			//Query para recolher vendedores registados na DataBase
			cmd.CommandText = "Select vendedor_id, vendedor_nome from tbl_vendedor";
			da = new MySqlDataAdapter(cmd);
			dt = new DataTable();
			da.Fill(dt);
			//Introduzir os vendedores na combobox dos modelos
			cbVendedorVendas.DataSource = dt;
			cbVendedorVendas.DisplayMember = "vendedor_nome";
			cbVendedorVendas.ValueMember = "vendedor_id";
			con.Close();
		}

		//Função para abilitar ou desativar as informações caso o automovel tenha sido vendido
		private void DisableAble(bool truefalse)
		{
			txtMarcaPA.Enabled = truefalse;
			txtModeloPA.Enabled = truefalse;
			txtCilindradaPA.Enabled = truefalse;
			txtPotenciaPA.Enabled = truefalse;
			cbCombustivelPA.Enabled = truefalse;
			txtPrecoPA.Enabled = truefalse;
			btnCarregarImgPA.Enabled = truefalse;
			btnAtualizarPA.Enabled = truefalse;
		}

		/*
		#########################
		#                       #
		#  Mostrar informações  #
		#                       #
		#########################
		*/

		//Mostrar automovel
		private void ShowAuto(Automovel automovel)
		{
			//Buscar o automovel à class Automovel
			txtIDPA.Text = automovel.GetAutoId().ToString();
			txtMarcaPA.Text = automovel.GetAutoMarca().ToString();
			txtModeloPA.Text = automovel.GetAutoModelo().ToString();
			txtCilindradaPA.Text = automovel.GetAutoCilindrada().ToString();
			txtPotenciaPA.Text = automovel.GetAutoPotencia().ToString();
			cbCombustivelPA.SelectedItem = automovel.GetAutoCombustivel().ToString();
			txtPrecoPA.Text = automovel.GetAutoPreco().ToString();

			//Verificar se o automovel
			if (automovel.GetAutoVendido())
			{
				DisableAble(false);
				lblVendidoPA.Visible = true;
			}
			else
			{
				DisableAble(true);
				lblVendidoPA.Visible = false;
			}

			//Buscar a imagem do carro, caso não tenha usar imagem default
			if (automovel.GetAutoImagem() == null)
				pbAutoImagemPA.Image = Resources.stand_car_default;
			else
			{
				MemoryStream ms = new MemoryStream(automovel.GetAutoImagem());
				Image imagem = Image.FromStream(ms);
				pbAutoImagemPA.Image = imagem;
			}
		}

		//Mostrar vendedores
		private void ShowVendedor(Vendedor vendedor)
		{
			//Buscar os vendedores à class Vendedor
			txtIdVendedor.Text = vendedor.GetVendedorId().ToString();
			txtNomeVendedor.Text = vendedor.GetVendedorNome().ToString();
			txtEmailVendedor.Text = vendedor.GetVendedorEmail().ToString();
			txtTlmVendedor.Text = vendedor.GetVendedorTlm().ToString();
			//Buscar a imagem do vendedor, caso não tenha usar imagem default
			if (vendedor.GetVendedorImagem() == null)
				pbVendedorImagem.Image = Resources.stand_vendedor_default;
			else
			{
				MemoryStream ms = new MemoryStream(vendedor.GetVendedorImagem());
				Image imagem = Image.FromStream(ms);
				pbVendedorImagem.Image = imagem;
			}

			//Buscar à DataBase as vendas realizadas pelo vendedor
			con.Open();
			cmd = new MySqlCommand
			{
				Connection = con,
				CommandText = "SELECT tbl_vendas.venda_id, tbl_automovel.auto_modelo, tbl_cliente.cliente_nome, tbl_vendedor.vendedor_nome, tbl_vendas.venda_preco, tbl_vendas.venda_data " +
					  "FROM tbl_vendas " +
					  "INNER JOIN tbl_automovel ON tbl_vendas.venda_modelo = tbl_automovel.auto_id " +
					  "INNER JOIN tbl_cliente ON tbl_vendas.venda_cliente = tbl_cliente.cliente_id " +
					  "INNER JOIN tbl_vendedor ON tbl_vendas.venda_vendedor = tbl_vendedor.vendedor_id " +
					  "WHERE tbl_vendas.venda_vendedor = @idVendedor"
			};
			cmd.Parameters.AddWithValue("@idVendedor", vendedor.GetVendedorId());
			MySqlDataAdapter da = new MySqlDataAdapter
			{
				SelectCommand = cmd
			};

			DataTable dt = new DataTable();
			da.Fill(dt);
			//Verificar se encontrou alguma informação de vendas, caso não tenha encontrado limpar ou colocar a gridView vazia
			if (dt.Rows.Count > 0)
				dgvVendasVendedor.DataSource = dt;
			else
				dgvVendasVendedor.DataSource = null;
			con.Close();
		}

		//Mostrar clientes
		private void ShowCliente(Cliente cliente)
		{
			//Buscar os vendedores à class Cliente
			txtIdCliente.Text = cliente.GetClienteId().ToString();
			txtNomeCliente.Text = cliente.GetClienteNome().ToString();
			txtEmailCliente.Text = cliente.GetClienteEmail().ToString();
			txtTlmCliente.Text = cliente.GetClienteTlm().ToString();
		}

		//Mostrar vendas
		private void ShowVendas(Vendas vendas)
		{
			//Trocar o id do modelo, cliente e vendedor pelo nome
			String Modelo, Cliente, Vendedor;
			con.Open();
			cmd = new MySqlCommand
			{
				Connection = con,
				CommandText = "Select auto_modelo from tbl_automovel where auto_id = @idAutomovel"
			};
			cmd.Parameters.AddWithValue("@idAutomovel", vendas.GetVendasModelo());
			Modelo = cmd.ExecuteScalar().ToString();
			cmd.CommandText = "Select cliente_nome from tbl_cliente where cliente_id = @idCliente";
			cmd.Parameters.AddWithValue("@idCliente", vendas.GetVendasCliente());
			Cliente = cmd.ExecuteScalar().ToString();
			cmd.CommandText = "Select vendedor_nome from tbl_vendedor where vendedor_id = @idVendedor";
			cmd.Parameters.AddWithValue("@idVendedor", vendas.GetVendasVendedor());
			Vendedor = cmd.ExecuteScalar().ToString();
			con.Close();

			//Buscar as vendas à class Vendas e o modelo, cliente e vendedor das buscas feitas acima
			txtIdVendas.Text = vendas.GetVendasId().ToString();
			cbModeloVendas.Text = Modelo;
			cbClienteVendas.Text = Cliente;
			cbVendedorVendas.Text = Vendedor;
			txtPrecoVendas.Text = vendas.GetVendasPreco().ToString();
			dtpDataVendas.Text = vendas.GetVendasData().ToString();
		}

		//Mostrar utilizadores
		private void ShowUtilizador(Utilizador utilizador)
		{
			//Buscar utilizadores à classe Utilizador
			txtIdGestao.Text = utilizador.GetUserId().ToString();
			txtNomeGestao.Text = utilizador.GetUserName().ToString();
			txtPasswordGestao.Text = utilizador.GetUserPassword().ToString();
			cbNivelGestao.SelectedItem = utilizador.GetUserLevel().ToString();

			//Buscar lista de utilizadores à DataBase
			con.Open();
			cmd = new MySqlCommand
			{
				Connection = con,
				CommandText = "Select * from tbl_utilizadores"
			};
			MySqlDataAdapter da = new MySqlDataAdapter { SelectCommand = cmd };
			DataTable dt = new DataTable();
			da.Fill(dt);
			if (dt.Rows.Count > 0)
				dgvListagemUtilizadorGestao.DataSource = dt;
			else
				dgvListagemUtilizadorGestao.DataSource = "";

			//Buscar lista de acessos realizados, ou tentados, à DataBase
			cmd.CommandText = "Select * from tbl_logs_login";
			da = new MySqlDataAdapter { SelectCommand = cmd };
			dt = new DataTable();
			da.Fill(dt);
			if (dt.Rows.Count > 0)
				dgvLogsGestao.DataSource = dt;
			else
				dgvLogsGestao.DataSource = "";
			con.Close();
		}

		/*
		###################################
		#                                 #
		#  Funções avançar ou retroceder  #
		#                                 #
		###################################
		*/

		//Função para avançar
		private void NextRecord(ref int index, int numRecords, Action<int> showFunction)
		{
			//Verificar index para programa não mostrar algum registo que não existe
			if ((index + 1) > (numRecords - 1))
				MessageBox.Show("Ultimo Registo!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
			else
			{
				index++;
				showFunction(index);
			}
		}

		//Função para retroceder
		private void PreviousRecord(ref int index, Action<int> showFunction)
		{
			//Verificar index para programa não mostrar algum registo que não existe
			if ((index - 1) < 0)
				MessageBox.Show("Primeiro Registo!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
			else
			{
				index--;
				showFunction(index);
			}
		}

		/*
		###########################
		#                         #
		#  Função para consultas  #
		#                         #
		###########################
		*/

		private void Consultar(object objeto, int id, string mensagemErro)
		{
			//Variavel temporaria para atualizar index
			int i = 0;
			//Verificar objeto em que a consulta foi pedida
			switch (objeto)
			{
				case Automovel _:
					Automovel automovelConsultado = null;
					//Loop para procurar o id do automovel na lista de automoveis
					foreach (Automovel automovel in auto)
					{
						//Se o id pedido para pesquisar for encontrado guardar os dados do automovel na variavel automovelConsultado
						if (automovel.GetAutoId() == id)
						{
							automovelConsultado = automovel;
							break;
						}
						i++;
					}
					indexAuto = i;

					//Se for automovelConsultado não estiver vazio, mostrar o automovel. Se não mostrar mensagem de erro
					if (automovelConsultado != null)
						ShowAuto(automovelConsultado);
					else
						MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;

				case Vendedor _:
					Vendedor vendedorConsultado = null;
					foreach (Vendedor vendedorID in vendedor)
					{
						if (vendedorID.GetVendedorId() == id)
						{
							vendedorConsultado = vendedorID;
							break;
						}
						i++;
					}
					indexVendedor = i;

					if (vendedorConsultado != null)
						ShowVendedor(vendedorConsultado);
					else
						MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;

				case Cliente _:
					Cliente clienteConsultado = null;
					foreach (Cliente clienteID in cliente)
					{
						if (clienteID.GetClienteId() == id)
						{
							clienteConsultado = clienteID;
							break;
						}
						i++;
					}
					indexCliente = i;

					if (clienteConsultado != null)
						ShowCliente(clienteConsultado);
					else
						MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;

				case Vendas _:
					Vendas vendaConsultado = null;
					foreach (Vendas vendaID in vendas)
					{
						if (vendaID.GetVendasId() == id)
						{
							vendaConsultado = vendaID;
							break;
						}
						i++;
					}
					indexVendas = i;

					if (vendaConsultado != null)
						ShowVendas(vendaConsultado);
					else
						MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;

				case Utilizador _:
					Utilizador utilizadorConsultado = null;
					foreach (Utilizador utilizadorID in utilizador)
					{
						if (utilizadorID.GetUserId() == id)
						{
							utilizadorConsultado = utilizadorID;
							break;
						}
						i++;
					}
					indexUtilizadores = i;

					if (utilizadorConsultado != null)
						ShowUtilizador(utilizadorConsultado);
					else
						MessageBox.Show(mensagemErro, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
			}

		}

		/*
		#############################
		#                           #
		#  Função ao form carregar  #
		#                           #
		#############################
		*/

		//Função ao carregar form
		private void Stand_Load(object sender, EventArgs e)
		{
			//Verificar nivel do utilizador
			if (UserNivel_gl != "admin")
			{
				tcStand.TabPages.Remove(tpGestaoUtilizador);
				btnGravarVendedor.Enabled = false;
				btnAtualizarVendedor.Enabled = false;
				btnCarregarImgVendedor.Enabled = false;
			}

			//Chamar funções para guardar os automoveis na class Automovel e mostrar os automoveis
			GuardarClass("tbl_automovel");
			ShowAuto(auto[indexAuto]);

			//Mudar label user para nome do utilizador e label nivel para o nivel do utilizador
			lblUser.Text = UserNome_gl;
			lblNivel.Text = UserNivel_gl;
		}


		/*
		########################################################################
		#                                                                      #
		#  Função para mostrar os dados de acordo com o separador selecionado  #
		#                                                                      #
		########################################################################
		*/
		private void TcStand_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (tcStand.SelectedTab.Name)
			{
				case "tpParqueAuto":
					GuardarClass("tbl_automovel");
					ShowAuto(auto[indexAuto]);
					break;
				case "tpVendedores":
					GuardarClass("tbl_vendedor");
					ShowVendedor(vendedor[indexVendedor]);
					break;
				case "tpClientes":
					GuardarClass("tbl_cliente");
					ShowCliente(cliente[indexCliente]);
					break;
				case "tpVendas":
					EnviarDadosComboBox();
					GuardarClass("tbl_vendas");
					ShowVendas(vendas[indexVendas]);
					break;
				case "tpGestaoUtilizador":
					GuardarClass("tbl_utilizadores");
					ShowUtilizador(utilizador[indexUtilizadores]);
					break;
			}
		}

		/*
		###################################################################################
		#                                                                                 #
		#  Função para mudar preço no separador vendas quando é selecionado outro modelo  #
		#  e verificar se o modelo já foi vendido                                         #
		#                                                                                 #
		###################################################################################
		*/
		private void CbModeloVendas_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Verificar o tipo de extração de dados
			if (cbModeloVendas.SelectedItem is DataRowView drv)
			{
				int autoId = Convert.ToInt32(drv.Row["auto_id"]);

				try
				{
					//Verificar a abertura da coneccao à DataBase
					if (con.State == ConnectionState.Closed)
					{
						con.Open();

						// Procurar o preço correspondente ao modelo selecionado
						cmd = new MySqlCommand
						{
							Connection = con,
							CommandText = "SELECT auto_preco, auto_vendido FROM tbl_automovel WHERE auto_id = @autoId"
						};
						cmd.Parameters.AddWithValue("@autoId", autoId);
						dr = cmd.ExecuteReader();

						while (dr.Read())
						{
							int result = Convert.ToInt32(dr["auto_preco"]);
							bool vendido = (bool)dr["auto_vendido"];

							int preco = Convert.ToInt32(result);
							txtPrecoVendas.Text = preco.ToString();

							//Verificar se o carro já foi vendido
							if (vendido)
							{
								btnGravarVendas.Enabled = false;
								lblVendidoVendas.Visible = true;
							}
							else
							{
								btnGravarVendas.Enabled = true;
								lblVendidoVendas.Visible = false;
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Erro ao consultar o banco de dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				finally
				{
					con.Close();
				}
			}
		}

		/*
		##################
		#                #
		#  Botão Gravar  #
		#                #
		##################
		*/

		//Gravar novo automovel na Database
		private void BtnGravarAuto_Click(object sender, EventArgs e)
		{
			con.Open();
			cmd = new MySqlCommand { Connection = con };

			Image imagem = pbAutoImagemPA.Image;
			MemoryStream ms = new MemoryStream();
			imagem.Save(ms, imagem.RawFormat);
			byte[] bytes = ms.ToArray();

			cmd.CommandText = "insert into tbl_automovel (auto_marca, auto_modelo, auto_cilindrada, auto_potencia, auto_combustivel, auto_preco, auto_vendido, auto_imagem) values (@marca, @modelo, @cilindrada, @potencia, @combustivel,  @preco, 0, @imagem)";
			cmd.Parameters.AddWithValue("@marca", txtMarcaPA.Text);
			cmd.Parameters.AddWithValue("@modelo", txtModeloPA.Text);
			cmd.Parameters.AddWithValue("@cilindrada", Convert.ToInt32(txtCilindradaPA.Text));
			cmd.Parameters.AddWithValue("@potencia", Convert.ToInt32(txtPotenciaPA.Text));
			cmd.Parameters.AddWithValue("@combustivel", cbCombustivelPA.Text);
			cmd.Parameters.AddWithValue("@preco", Convert.ToInt32(txtPrecoPA.Text));
			cmd.Parameters.AddWithValue("@imagem", bytes);
			cmd.ExecuteNonQuery();
			con.Close();

			//Atualizar informações na classe
			GuardarClass("tbl_automovel");
			ShowAuto(auto[indexAuto]);
			MessageBox.Show("Carro inserido na BD!!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		//Gravar novo vendedor ou cliente
		private void BtnGravarVendedorCliente_Click(object sender, EventArgs e)
		{
			Button clickedButton = sender as Button;

			con.Open();
			cmd = new MySqlCommand { Connection = con };
			//Verificação do botão clicado (botão do vendedor ou cliente)
			switch (clickedButton.Name)
			{
				case "btnGravarVendedor":
					Image imagem = pbVendedorImagem.Image;
					MemoryStream ms = new MemoryStream();
					imagem.Save(ms, imagem.RawFormat);
					byte[] bytes = ms.ToArray();

					cmd.CommandText = "INSERT INTO tbl_vendedor (vendedor_nome, vendedor_email, vendedor_tlm, vendedor_imagem) VALUES (@nome, @email, @tlm, @imagem)";
					cmd.Parameters.AddWithValue("@nome", txtNomeVendedor.Text);
					cmd.Parameters.AddWithValue("@email", txtEmailVendedor.Text);
					cmd.Parameters.AddWithValue("@tlm", Convert.ToInt32(txtTlmVendedor.Text));
					cmd.Parameters.AddWithValue("@imagem", bytes);
					cmd.ExecuteNonQuery();
					con.Close();

					//Atualizar informações na classe
					GuardarClass("tbl_vendedor");
					ShowVendedor(vendedor[indexVendedor]);
					MessageBox.Show("Vendedor inserido na BD!!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;

				case "btnGravarCliente":
					cmd.CommandText = "INSERT INTO tbl_cliente (cliente_nome, cliente_email, cliente_tlm) VALUES (@nome, @email, @tlm)";
					cmd.Parameters.AddWithValue("@nome", txtNomeCliente.Text);
					cmd.Parameters.AddWithValue("@email", txtEmailCliente.Text);
					cmd.Parameters.AddWithValue("@tlm", Convert.ToInt32(txtTlmCliente.Text));
					cmd.ExecuteNonQuery();
					con.Close();

					//Atualizar informações na classe
					GuardarClass("tbl_cliente");
					ShowCliente(cliente[indexCliente]);
					MessageBox.Show("Cliente inserido na BD!!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
			}
			con.Close();
		}

		//Gravar nova venda
		private void BtnGravarVendas_Click(object sender, EventArgs e)
		{
			con.Open();
			cmd = new MySqlCommand
			{
				Connection = con,
				CommandText = "INSERT INTO tbl_vendas (venda_modelo, venda_cliente, venda_vendedor, venda_preco, venda_data) VALUES (@modelo, @cliente, @vendedor, @preco, @data)"
			};

			cmd.Parameters.AddWithValue("@modelo", Convert.ToInt32(cbModeloVendas.SelectedValue));
			cmd.Parameters.AddWithValue("@cliente", Convert.ToInt32(cbClienteVendas.SelectedValue));
			cmd.Parameters.AddWithValue("@vendedor", Convert.ToInt32(cbVendedorVendas.SelectedValue));
			cmd.Parameters.AddWithValue("@preco", Convert.ToInt32(txtPrecoVendas.Text));
			cmd.Parameters.AddWithValue("@data", dtpDataVendas.Text);

			cmd.ExecuteNonQuery();

			cmd.CommandText = "update tbl_automovel set auto_vendido = 1 where auto_id= @idModelo";
			cmd.Parameters.AddWithValue("@idModelo", Convert.ToInt32(cbModeloVendas.SelectedValue));
			cmd.ExecuteNonQuery();

			con.Close();
			//Atualizar informações na classe
			GuardarClass("tbl_vendas");
			ShowVendas(vendas[indexVendas]);
			MessageBox.Show("Venda registrada com sucesso!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

		}

		//Gravar novo utilizador
		private void BtnGravarUtilizador_Click(object sender, EventArgs e)
		{
			con.Open();
			cmd = new MySqlCommand
			{
				Connection = con,
				CommandText = "INSERT INTO tbl_utilizadores (lg_nome, lg_password, lg_nivel) VALUES (@nome, @pass, @nivel)"
			};
			cmd.Parameters.AddWithValue("@nome", txtNomeGestao.Text);
			cmd.Parameters.AddWithValue("@pass", txtPasswordGestao.Text);
			cmd.Parameters.AddWithValue("@nivel", cbNivelGestao.SelectedItem);
			cmd.ExecuteNonQuery();
			con.Close();
			//Atualizar informações na classe
			GuardarClass("tbl_utilizadores");
			ShowUtilizador(utilizador[indexUtilizadores]);
			MessageBox.Show("Utilizador registrado com sucesso!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		/*
		#####################
		#                   #
		#  Botão Atualizar  #
		#					#
		#####################
		*/

		//Atualizar automovel
		private void BtnAtualizarAuto_Click(object sender, EventArgs e)
		{
			con.Open();
			cmd = new MySqlCommand { Connection = con };

			Image imagem = pbAutoImagemPA.Image;
			MemoryStream ms = new MemoryStream();
			imagem.Save(ms, imagem.RawFormat);
			byte[] bytes = ms.ToArray();

			cmd.CommandText = "UPDATE tbl_automovel SET auto_marca=@Marca, auto_modelo=@Modelo, auto_cilindrada=@Cilindrada, auto_potencia=@Potencia, auto_combustivel=@Combustivel, auto_preco=@Preco, auto_imagem=@Imagem WHERE auto_id=@ID";

			cmd.Parameters.AddWithValue("@Marca", txtMarcaPA.Text);
			cmd.Parameters.AddWithValue("@Modelo", txtModeloPA.Text);
			cmd.Parameters.AddWithValue("@Cilindrada", Convert.ToInt32(txtCilindradaPA.Text));
			cmd.Parameters.AddWithValue("@Potencia", Convert.ToInt32(txtPotenciaPA.Text));
			cmd.Parameters.AddWithValue("@Combustivel", cbCombustivelPA.Text);
			cmd.Parameters.AddWithValue("@Preco", Convert.ToInt32(txtPrecoPA.Text));
			cmd.Parameters.AddWithValue("@Imagem", bytes);
			cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(txtIDPA.Text));

			cmd.ExecuteNonQuery();
			con.Close();

			//Atualizar class
			GuardarClass("tbl_automovel");
			MessageBox.Show("Informações atualizadas!!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		//Atualizar vendedor ou utilizador
		private void BtnAtualizarVendedorCliente_Click(object sender, EventArgs e)
		{
			Button clickedButton = sender as Button;
			con.Open();
			cmd = new MySqlCommand { Connection = con };
			//Verificação do botão clicado (botão do vendedor ou cliente)
			switch (clickedButton.Name)
			{
				case "btnAtualizarVendedor":
					Image imagem = pbVendedorImagem.Image;
					MemoryStream ms = new MemoryStream();
					imagem.Save(ms, imagem.RawFormat);
					byte[] bytes = ms.ToArray();

					cmd.CommandText = "UPDATE tbl_vendedor SET vendedor_nome=@Nome, vendedor_email=@Email, vendedor_tlm=@Telemovel, vendedor_imagem=@Imagem WHERE vendedor_id=@ID";

					cmd.Parameters.AddWithValue("@Nome", txtNomeVendedor.Text);
					cmd.Parameters.AddWithValue("@Email", txtEmailVendedor.Text);
					cmd.Parameters.AddWithValue("@Telemovel", Convert.ToInt32(txtTlmVendedor.Text));
					cmd.Parameters.AddWithValue("@Imagem", bytes);
					cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(txtIdVendedor.Text));

					cmd.ExecuteNonQuery();
					con.Close();
					//Atualizar class
					GuardarClass("tbl_vendedor");
					MessageBox.Show("Informações do vendedor atualizadas!!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;

				case "btnAtualizarCliente":
					cmd.CommandText = "UPDATE tbl_cliente SET cliente_nome=@Nome, cliente_email=@Email, cliente_tlm=@Telemovel WHERE vendedor_id=@ID";

					cmd.Parameters.AddWithValue("@Nome", txtNomeCliente.Text);
					cmd.Parameters.AddWithValue("@Email", txtEmailCliente.Text);
					cmd.Parameters.AddWithValue("@Telemovel", Convert.ToInt32(txtTlmCliente.Text));
					cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(txtIdCliente.Text));

					cmd.ExecuteNonQuery();
					con.Close();
					//Atualizar class
					GuardarClass("tbl_cliente");
					MessageBox.Show("Informações do cliente atualizadas!!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
					break;
			}

		}

		//Atualizar venda
		private void BtnAtualizarVendas_Click(object sender, EventArgs e)
		{
			con.Open();
			cmd = new MySqlCommand { Connection = con };

			//Caso o modelo seja atualizado (por engano de seleção), tornar modelo antigo disponivel para venda
			int modeloAnterior = 0;
			cmd.CommandText = "SELECT venda_modelo FROM tbl_vendas WHERE venda_id = @ID";
			cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(txtIdVendas.Text));
			var result = cmd.ExecuteScalar();
			if (result != null)
			{
				modeloAnterior = Convert.ToInt32(result);
			}
			if (modeloAnterior != 0)
			{
				cmd.CommandText = "UPDATE tbl_automovel SET auto_vendido = 0 WHERE auto_id = @ModeloAnterior";
				cmd.Parameters.Clear();
				cmd.Parameters.AddWithValue("@ModeloAnterior", modeloAnterior);
				cmd.ExecuteNonQuery();
			}


			cmd.CommandText = "UPDATE tbl_vendas SET venda_modelo=@Modelo, venda_cliente=@Cliente, venda_vendedor=@Vendedor, venda_preco=@Preco, venda_data=@Data WHERE venda_id=@ID";

			cmd.Parameters.AddWithValue("@Modelo", Convert.ToInt32(cbModeloVendas.SelectedValue));
			cmd.Parameters.AddWithValue("@Cliente", Convert.ToInt32(cbClienteVendas.SelectedValue));
			cmd.Parameters.AddWithValue("@Vendedor", Convert.ToInt32(cbVendedorVendas.SelectedValue));
			cmd.Parameters.AddWithValue("@Preco", Convert.ToInt32(txtPrecoVendas.Text));
			cmd.Parameters.AddWithValue("@Data", dtpDataVendas.Text);
			cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(txtIdVendas.Text));
			cmd.ExecuteNonQuery();

			//Tornar novo modelo como vendido
			cmd.CommandText = "UPDATE tbl_automovel SET auto_vendido = 1 WHERE auto_id = @Modelo";
			cmd.Parameters.Clear();
			cmd.Parameters.AddWithValue("@Modelo", Convert.ToInt32(cbModeloVendas.SelectedValue));
			cmd.ExecuteNonQuery();

			con.Close();
			//Atualizar class
			GuardarClass("tbl_vendas");
			ShowVendas(vendas[indexVendas]);
			MessageBox.Show("Informações atualizadas!!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		//Atualizar Utilizador
		private void btnAtualizarUtilizador_Click(object sender, EventArgs e)
		{
			con.Open();
			cmd = new MySqlCommand
			{
				Connection = con,
				CommandText = "UPDATE tbl_utilizadores SET lg_nome = @nome, lg_password = @pass, lg_nivel = @nivel where lg_id = @id"
			};
			cmd.Parameters.AddWithValue("@nome", txtNomeGestao.Text);
			cmd.Parameters.AddWithValue("@pass", txtPasswordGestao.Text);
			cmd.Parameters.AddWithValue("@nivel", cbNivelGestao.SelectedItem);
			cmd.Parameters.AddWithValue("@id", txtIdGestao.Text);
			cmd.ExecuteNonQuery();
			con.Close();
			GuardarClass("tbl_utilizadores");
			//Atualizar class
			ShowUtilizador(utilizador[indexUtilizadores]);
			MessageBox.Show("Informações atualizadas!!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		/*
		#####################
		#                   #
		#  Botão Consultar  #
		#					#
		#####################
		*/

		//Consulta automovel
		private void BtnConsultarAuto_Click(object sender, EventArgs e)
		{
			int autoIdConsultado = Convert.ToInt32(txtIDPA.Text);
			Consultar(new Automovel(), autoIdConsultado, "Automóvel com o ID especificado não encontrado!");
		}

		//Consulta vendedor
		private void BtnConsultarVendedor_Click(object sender, EventArgs e)
		{
			int vendedorIdConsultado = Convert.ToInt32(txtIdVendedor.Text);
			Consultar(new Vendedor(), vendedorIdConsultado, "Vendedor com o ID especificado não encontrado!");
		}

		//Consulta cliente
		private void BtnConsultarCliente_Click(object sender, EventArgs e)
		{
			int clienteIdConsultado = Convert.ToInt32(txtIdCliente.Text);
			Consultar(new Cliente(), clienteIdConsultado, "Cliente com o ID especificado não encontrado !");
		}

		//Consulta venda
		private void BtnConsultarVendas_Click(object sender, EventArgs e)
		{
			int vendaIdConsultado = Convert.ToInt32(txtIdVendas.Text);
			Consultar(new Vendas(), vendaIdConsultado, "Venda com o ID especificado não encontrada !");
		}

		//Consulta utilizador
		private void btnConsultaUtilizador_Click(object sender, EventArgs e)
		{
			int utilizadorIdConsultado = Convert.ToInt32(txtIdGestao.Text);
			Consultar(new Utilizador(), utilizadorIdConsultado, "Utilizador com o ID especificado não encontrada !");
		}

		/*
		##################################
		#                                #
		#  Botões avançar ou retroceder  #
		#                                #
		##################################
		*/

		//Botão avançar
		private void BtnProximo_Click(object sender, EventArgs e)
		{
			//Verificar em separador o botão foi clicado
			switch (tcStand.SelectedTab.Name)
			{
				case "tpParqueAuto":
					//Enviar como referencia o index, o numero de registos na DataBase e a função para mostrar as informações
					NextRecord(ref indexAuto, NumAutoDB, index => ShowAuto(auto[index]));
					break;
				case "tpVendedores":
					NextRecord(ref indexVendedor, NumVendedorDB, index => ShowVendedor(vendedor[index]));
					break;
				case "tpClientes":
					NextRecord(ref indexCliente, NumClientDB, index => ShowCliente(cliente[index]));
					break;
				case "tpVendas":
					NextRecord(ref indexVendas, NumVendasDB, index => ShowVendas(vendas[index]));
					break;
				case "tpGestaoUtilizador":
					NextRecord(ref indexUtilizadores, NumUtilizadoresDB, index => ShowUtilizador(utilizador[index]));
					break;
			}
		}

		//Botão retroceder
		private void BtnAnterior_Click(object sender, EventArgs e)
		{
			switch (tcStand.SelectedTab.Name)
			{
				case "tpParqueAuto":
					//Enviar como referencia o index e a função para mostrar as informações
					PreviousRecord(ref indexAuto, index => ShowAuto(auto[indexAuto]));
					break;
				case "tpVendedores":
					PreviousRecord(ref indexVendedor, index => ShowVendedor(vendedor[index]));
					break;
				case "tpClientes":
					PreviousRecord(ref indexCliente, index => ShowCliente(cliente[index]));
					break;
				case "tpVendas":
					PreviousRecord(ref indexVendas, index => ShowVendas(vendas[index]));
					break;
				case "tpGestaoUtilizador":
					PreviousRecord(ref indexUtilizadores, index => ShowUtilizador(utilizador[index]));
					break;
			}
		}

		/*
		#################################
		#                               #
		#  Botão para carregar imagem  #
		#                               #
		#################################
		*/

		private void BtnCarregarImg_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = "Arquivos de imagem|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Todos os arquivos|*.*",
				Title = "Selecionar Imagem"
			};
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string caminhoDoArquivo = openFileDialog.FileName;
				byte[] imagemBytes = File.ReadAllBytes(caminhoDoArquivo);
				MemoryStream ms = new MemoryStream(imagemBytes);

				//Verificar se a imagem é valida
				try
				{
					Image imagem = Image.FromStream(ms);
					
					//Verificar separador para mudar imagem na pictureBox
					switch (tcStand.SelectedTab.Name)
					{
						case "tpParqueAuto":
							pbAutoImagemPA.Image = imagem;
							break;
						case "tpVendedores":
							pbVendedorImagem.Image = imagem;
							break;
					}
				}
				catch (ArgumentException)
				{
					//Mensagem de erro caso a imagem seja invalida
					MessageBox.Show("O arquivo selecionado não é uma imagem válida.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}


		/*
		##############
		#            #
		# Botão Sair #
		#            #
		##############
		*/
		private void BtnSair_Click(object sender, EventArgs e)
		{
			//Fechar aplicação
			Application.Exit();


			//Alternativa para o botão. Em vez de sair dar logout da conta
			/*
			this.Hide();
			Login frm1 = new Login();
			frm1.ShowDialog();
			this.Close();
			*/
		}
	}
}	