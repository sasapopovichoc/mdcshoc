﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			// Set userid and username to null;
			//
			// TODO: Missing implementation.

			// Put entry in eventLog table that user 'Username' has logged out.
			//
			// TODO: Missing implementation.
		}

		private void Login()
		{
			// Ask for username and password.
			// Check if that combination of username and password exist in the database.
			// If yes, set variables userid and username to the right values.
			//
			// TODO: Missing implementation.
		}

		private void AddUser()
		{
			// Ask for new username and password.
			// Create new user by inserting row in the messenger database.
			//
			// TODO: Missing implementation.
		}

		private void ShowHelp()
		{
			using (TextReader tr = new StreamReader("Help.txt"))
			{
				string helpContent = tr.ReadToEnd();
				Console.WriteLine(helpContent);
			}

			// Put in eventLog table info that user with username e.g. 'John123' asked for help.
			// If username is unknown (null) put "?"
			//
			// TODO: Missing implementation.
		}

		private void ShowAbout()
		{
			// Show text from file About.txt
			//
            using(TextReader About = new StreamReader("About.txt")){
            Console.WriteLine(About.ReadToEnd());
            }
			// Put in eventLog table info that user with username e.g. 'John123' executed About command.
			// If username is unknown (null) put "?"
			//
            Log("User" + username!=null?username:null + "executed About command");
		}

		private void SendMessage(string message)
		{
			string toUsername = null;
			int? toUserId = null;

			// Insert message into eventLog table.
			//
			// TODO: Missing implementation.

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

        private void Log(string message)
        {
            using (SqlConnection openCon = new SqlConnection(connectionString))
            {
                string insertMessageQuery = String.Format(
                    "INSERT INTO eventLog(eventTime, eventDescription) VALUES (getutcdate(), '{0}')", message);

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

			// If message looks like this: "John123,, Do you have 5 minutes?"
			// that means that message text "Do you have 5 minutes?"
			// should be sent to user with username John123.
			// In that case find userId for John123 and set that value to: toUserId.
			// Also, set toUsername to: John123
			// If message does not contain sequence ",," that means to whom message should be send is not defined.
			//
			// TODO: Missing implementation.
		}
	}
}
