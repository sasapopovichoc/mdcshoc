using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace HOCMessengerClient
{
	class HOCMessengerClient
	{
		string connectionString = @"Server=tcp:kzkogjfm75.database.windows.net,1433;Database=hocm;User ID=sasapopo@kzkogjfm75;Password=Password1;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";
		
		protected const string LOGIN = "_login";
		protected const string LOGOUT = "_logout";
		protected const string ADDUSER = "_adduser";
		protected const string ABOUT = "_about";
		protected List<string> HELP = new List<string> {"_help", "/?", "-?"};

		protected int? userid = null;
		protected string username = null;

		public void ProcessCommand(string command)
		{
			if (command.Equals(LOGIN))
			{
				Login();
			}
			else if (command.Equals(LOGOUT))
			{
				Logout();
			}
			else if (command.Equals(ADDUSER))
			{
				AddUser();
			}
			else if (HELP.Contains(command))
			{
				ShowHelp();
			}
			else if (ABOUT.Equals(command))
			{
				ShowAbout();
			}
			else
			{
				SendMessage(command);
			}
		}

		public string GetUsername()
		{
			if (username == null)
			{
				return "?";
			}
			else
			{
				return username;
			}
		}

		private void Logout()
		{
			userid=null;
			Log(username+" has logged out."); // Put entry in eventLog table that user "Username" has logged out.
			username=null; // Set userid and username to null;
			
			
			
		}

		private void Login()
		{
		Console.WriteLine("Enter username:");
		string un = Console.ReadLine();
		Console.WriteLine("Enter password:");
		string pw = Console.ReadLine();
		
using (SqlConnection conn = new SqlConnection(connectionString))
{
	conn.Open();
	using (SqlCommand command = new SqlCommand())
	{
		command.Connection = conn;
		command.CommandText = String.Format("select username, userId from users where username = {0} and password = {1}",un, pw);
		using (SqlDataReader reader = command.ExecuteReader())
		{
			// reader.Read() will read one row from the result-set and return true.
			// If there are no more rows, function Read will return false.
			//
			if (reader.Read())
			{
				username = reader[0].ToString();
				userid = Convert.ToInt32(reader[1].ToString());
				Console.WriteLine("Logged in!");
			}
			else
			{
				Console.WriteLine("Error. Person with this username/password combination does not exist.");
			}
		}
	}
}
			// Ask for username and password.
			// Check if that combination of username and password exist in the database.
			// If yes, set variables userid and username to the right values.
			
		}

		private void AddUser()
		{
		Console.WriteLine("Insert desired username:");
		string un = Console.ReadLine();
		Console.WriteLine("Insert desired password:");
		string pw = Console.ReadLine();
using (SqlConnection conn = new SqlConnection(connectionString))
{
	conn.Open();
	using (SqlCommand command = new SqlCommand())
	{
		command.Connection = conn;
		command.CommandText = String.Format("INSERT INTO users(username,password) values('{1}', '{2}')", un, pw);
		
		// this will execute query against the server
		//
		try
		{
		command.ExecuteNonQuery();
		}
		catch
		{
		Console.WriteLine("Error!");
		}
		finally 
		{
			Console.WriteLine("Successful registration!");
			command.CommandText = String.Format("select username, userId from users where username = {0} and password = {1}",un, pw);
				using (SqlDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						username = reader[0].ToString();
						userid = Convert.ToInt32(reader[1].ToString());
						Console.WriteLine("Logged in!");
					}
					else
					{
						Console.WriteLine("Database error!");
					}
				}
		
		}
	}
}
			// Ask for new username and password.
			// Create new user by inserting row in the messenger database.
		
		}

		private void ShowHelp()
		{
			using (TextReader tr = new StreamReader("Help.txt"))
			{
				string helpContent = tr.ReadToEnd();
				Console.WriteLine(helpContent);
			}
			if (username != null) Log("User with username "+username+" asked for help.");
			else Log("User with username ? asked for help.");
			// Put in eventLog table info that user with username e.g. "John123" asked for help.
			// If username is unknown (null) put "?"
		}

		private void ShowAbout()
		{
			using (TextReader tr = new StreamReader("About.txt"))
			{
				string helpContent = tr.ReadToEnd();
				Console.WriteLine(helpContent);
			}
			// Show text from file About.txt
			if (username != null) Log("User with username "+username+" asked for help.");
			else Log("User with username ? asked for help.");
			// Put in eventLog table info that user with username e.g. "John123" executed About command.
			// If username is unknown (null) put "?"
		}
		
		private void Log(string message)
		{
			using (SqlConnection openCon = new SqlConnection(connectionString))
			{
				string insertMessageQuery = String.Format(
					"INSERT INTO eventLog(eventTime,eventDescription) VALUES values (getutcdate(), '{0}')",
					message);

				using (SqlCommand insertMessageCommand = new SqlCommand(insertMessageQuery))
				{
					insertMessageCommand.Connection = openCon;
					openCon.Open();

					insertMessageCommand.ExecuteNonQuery();
				}
			}
		}
		private void SendMessage(string message)
		{
			string toUsername = null;
			int? toUserId = null;

			Log(message);
			// Insert message into eventLog table.
			

			ExtractToUserFromMessage(ref message, out toUsername, out toUserId);

			using (SqlConnection openCon = new SqlConnection(connectionString))
			{
				string insertMessageQuery = String.Format(
					"INSERT INTO messages VALUES ({0}, {1}, '{2}', getutcdate())",
					userid == null ? "null" : userid.ToString(),
					toUserId == null ? "null" : toUserId.ToString(),
					message);

				using (SqlCommand insertMessageCommand = new SqlCommand(insertMessageQuery))
				{
					insertMessageCommand.Connection = openCon;
					openCon.Open();

					insertMessageCommand.ExecuteNonQuery();
				}
			}
		}

		private void ExtractToUserFromMessage(ref string message, out string toUsername, out int? toUserId)
		{
			toUsername = null;
			toUserId = null;
			string[] args = Regex.Split(message, ",,");
					if (args.Length>1) 
					{
					 
							using (SqlConnection conn = new SqlConnection(connectionString))
								{
									conn.Open();
									using (SqlCommand command = new SqlCommand())
									{
										command.Connection = conn;
										command.CommandText = String.Format("select userId from users where username = {0}",args[0]);
										using (SqlDataReader reader = command.ExecuteReader())
										{
											if (reader.Read())
											{
												toUserId = reader.GetInt32(0);
												toUsername = args[0];
											}
											else
											{
												Console.WriteLine("Error. Person with this username does not exist.");
											}
										}
									}
								}
					
					
					
					}
			// If message looks like this: "John123,, Do you have 5 minutes?"
			// that means that message text "Do you have 5 minutes?"
			// should be sent to user with username John123.
			// In that case find userId for John123 and set that value to: toUserId.
			// Also, set toUsername to: John123
			// If message does not contain sequence ",," that means to whom message should be send is not defined.
		}
	}
}
