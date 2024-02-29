using daniel_grazina_PAF.Properties;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace daniel_grazina_PAF
{
	public partial class Login : Form
	{
		//Variaveis para conecção à base de dados
		MySqlConnection con;
		MySqlCommand cmd;
		MySqlCommand cmd2;
		MySqlDataReader dr;
		public Login()
		{
			//Coneccao base de dados
			con = new MySqlConnection("Server=127.0.0.1;Database=AutoCar_Daniel_Grazina;Uid=root;Pwd=Qop2006a;");
			InitializeComponent();
		}


		/*
		####################################
		#                                  #
		# Botão para mostrar/esconder pass #
		#                                  #
		####################################
		*/
		private void BtnHideShow_Click(object sender, EventArgs e)
		{
			//Verificação para esconder ou mostrar pass
			if (txtPass.PasswordChar == '•')
			{
				txtPass.PasswordChar = '\0';
				btnHideShow.BackgroundImage = Resources.hide;
			}
			else
			{
				txtPass.PasswordChar = '•';
				btnHideShow.BackgroundImage = Resources.visible;
			}
		}


		/*
		###############
		#             #
		# Botão Login #
		#             #
		###############
		*/
		private void BtnLogin_Click(object sender, EventArgs e)
		{
			//Abrir ligação à base de dados
			con.Open();

			//Comando para verificar utilizador e password
			cmd = new MySqlCommand
			{
				Connection = con,
				CommandText = "select * from tbl_utilizadores where lg_nome = @user and lg_password= @pass"
			};
			cmd.Parameters.AddWithValue("@user", txtUser.Text);
			cmd.Parameters.AddWithValue("@pass", txtPass.Text);

			//Comando para guardar login ou tentativa de login na aplicação
			cmd2 = new MySqlCommand();
			cmd2.Connection = con;
			cmd2.CommandText = $"Insert into tbl_logs_login (logs_nome, logs_password, logs_data) values (@user, @pass, @data)";
			cmd2.Parameters.AddWithValue("@user", txtUser.Text);
			cmd2.Parameters.AddWithValue("@pass", txtPass.Text);
			cmd2.Parameters.AddWithValue("@data", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

			//Executar comandos
			cmd2.ExecuteNonQuery();
			dr = cmd.ExecuteReader();
			

			//Verificação do login
			if (dr.Read())
			{
				//Caixa de mensagem para confirmação do login
				MessageBox.Show($"Bem vindo {txtUser.Text}!", "AutoCAR v1.0", MessageBoxButtons.OK, MessageBoxIcon.Information);

				//Esconder esta página
				this.Hide();

				//Abrir a proxima página e enviar o nome e o nivel (admin ou user) para a proxima página
				Stand stand = new Stand(); 
				stand.UserNome_gl = dr["lg_nome"].ToString();
				stand.UserNivel_gl = dr["lg_nivel"].ToString();
				stand.ShowDialog();

				//Fechar esta página
				this.Close();
			
			}
			else
			{
				//Caixa de mensagem de erro no login
				MessageBox.Show("User ou Pass não corretos!!!\nTente novamente.", "AutoCAR v1.0", MessageBoxButtons.OK, MessageBoxIcon.Error);
				
				//Limpar caixas de texto e colocar cursor na caixa de texto do UserName
				txtUser.Clear();
				txtPass.Clear();
				txtUser.Focus();
			}

			//Fechar ligação
			con.Close();
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
		}
	}
}
