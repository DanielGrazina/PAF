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
		#########################
		#                       #
		#  Função guardar logs  #
		#                       #
		#########################
		*/
		private void GuardarLogs()
		{
			cmd2 = new MySqlCommand();
			cmd2.Connection = con;
			cmd2.CommandText = "Select lg_id from tbl_utilizadores where lg_nome = @user";
			cmd2.Parameters.AddWithValue("@user", txtUser.Text);
			int id = Convert.ToInt32(cmd2.ExecuteScalar());

			cmd2.CommandText = "Insert into tbl_logs_login values (@id, @user, @pass, @data)";
			cmd2.Parameters.AddWithValue("@id", id);
			cmd2.Parameters.AddWithValue("@pass", txtPass.Text);
			cmd2.Parameters.AddWithValue("@data", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			cmd2.ExecuteNonQuery();
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
			con.Open();
			try
			{
				cmd = new MySqlCommand
				{
					Connection = con,
					CommandText = "select * from tbl_utilizadores where lg_nome = @user and lg_password= @pass"
				};
				cmd.Parameters.AddWithValue("@user", txtUser.Text);
				cmd.Parameters.AddWithValue("@pass", txtPass.Text);

				//Guardar as tentativas e logins
				GuardarLogs();

				//Executar comando
				dr = cmd.ExecuteReader();

				if (dr.Read())
				{
					// Sucesso no login
					MessageBox.Show($"Bem vindo {txtUser.Text}!", "AutoCAR v1.0", MessageBoxButtons.OK, MessageBoxIcon.Information);
					this.Hide();
					Stand stand = new Stand();
					stand.UserNome_gl = txtUser.Text;
					stand.UserNivel_gl = dr["lg_nivel"].ToString();
					stand.ShowDialog();
					this.Close();
				}
				else
				{
					// Falha no login
					MessageBox.Show("User ou Pass não corretos!!!\nTente novamente.", "AutoCAR v1.0", MessageBoxButtons.OK, MessageBoxIcon.Error);
					txtUser.Clear();
					txtPass.Clear();
					txtUser.Focus();
				}
			}
			catch (Exception)
			{
				MessageBox.Show("Erro ao realizar o login!!!\nContacte o administrador.", "AutoCAR v1.0", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
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
