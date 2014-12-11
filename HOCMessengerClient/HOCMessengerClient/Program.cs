using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOCMessengerClient
{
	class Program
	{
		static void Main(string[] args)
		{
			HOCMessengerClient messenger = new HOCMessengerClient();
			bool quit = false;
			
			while (!quit)
			{
				Console.Write(String.Format("[{0}]>", messenger.GetUsername()));
				string command = Console.ReadLine();

				if (command.Equals("_quit"))
				{
					quit = true;
				}
				else
				{
					messenger.ProcessCommand(command);
				}
			}
		}
	}
}
