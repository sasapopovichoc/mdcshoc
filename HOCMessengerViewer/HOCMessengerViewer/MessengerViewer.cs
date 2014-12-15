using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HourOfCodeMessengerViewer
{
	class MessengerViewer
	{
		public DateTime Time = DateTime.Parse("2000-1-1");
		public string getNewMessagesQueryTemplate =
@"SELECT 
	m.messagetext, 
	m.messagetime, 
	u.username
FROM 
	messages m 
	LEFT JOIN 
	users u 
	ON u.userid = m.fromuserid
WHERE 
	messagetime > '{0}'";

		string connectionString = @"Server=tcp:kzkogjfm75.database.windows.net,1433;Database=hocm;User ID=sasapopo@kzkogjfm75;Password=Password1;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

		internal string[] GetNewMessages()
		{
			List<string> messages = new List<string>();

			using (SqlConnection openCon = new SqlConnection(connectionString))
			{
				string getNewMessagesQuery = String.Format(getNewMessagesQueryTemplate, Time.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));

				using (SqlCommand getNewMessagesCommand = new SqlCommand(getNewMessagesQuery))
				{
					getNewMessagesCommand.Connection = openCon;
					openCon.Open();

					using (SqlDataReader reader = getNewMessagesCommand.ExecuteReader())
					{
						while (reader.Read())
						{
							messages.Add(string.Format("[{0}{1}] : {2}", GetMessageAuthor(reader), GetMessageTime(reader), GetMessageText(reader)));
							DateTime dateTime = reader.GetDateTime(1);
							if (dateTime > Time)
							{
								Time = dateTime;
							}
						}
					}
				}
			}

			return messages.ToArray();
		}

		protected string GetMessageText(SqlDataReader reader)
		{
			return reader.GetString(0);
		}

		protected string GetMessageAuthor(SqlDataReader reader)
		{
			// Method should extract username which sent this message from reader (row of the table).
			// If username is null, show "U?".
			//
			// TODO: Actual implementation missing here.

			return "-";
		}

		protected string GetMessageTime(SqlDataReader reader)
		{
			// Method should extract message time from reader (row of the table).
			// If time is null, show "T?".
			//
			// TODO: Actual implementation missing here.

			return "-";
		}
	}
}
