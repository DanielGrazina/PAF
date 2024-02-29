using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace daniel_grazina_PAF.Classes
{
	public class Utilizador
	{
		private int userId;
		private String userName;
		private String userPassword;
		private String userLevel;

		public Utilizador() { }
		public Utilizador(int userId, string userName, string userPassword, string userLevel)
		{
			this.userId = userId;
			this.userName = userName;
			this.userPassword = userPassword;
			this.userLevel = userLevel;
		}
		public int GetUserId() { return userId; }
		public String GetUserName() { return userName; }
		public String GetUserPassword() { return userPassword;}
		public String GetUserLevel() { return userLevel;}
	}
}
