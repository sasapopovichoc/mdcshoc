using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HourOfCodeMessengerViewer
{
	class Program
	{
		static void Main(string[] args)
		{
			MessengerViewer mv = new MessengerViewer();

			while (true)
			{
				string[] messages = mv.GetNewMessages();

				foreach (string message in messages)
				{
					Console.WriteLine(message);
				}
			}
		}
	}
}
