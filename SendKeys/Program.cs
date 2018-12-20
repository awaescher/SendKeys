using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace SendKeys
{
	static class Program
	{
		[DllImport("User32.dll")]
		static extern int SetForegroundWindow(IntPtr point);

		[DllImport("kernel32.dll")]
		static extern bool AttachConsole(int dwProcessId);
		private const int ATTACH_PARENT_PROCESS = -1;

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool FreeConsole();

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			int pid = -1;
			int wait = 0;
			string keysToSend = "";

			var validArguments = args?.Length == 2 || args?.Length == 3;

			if (validArguments)
			{
				for (int i = 0; i < args.Length; i++)
				{
					int pidIndex = args[i].IndexOf("pid:", StringComparison.OrdinalIgnoreCase);
					int waitIndex = args[i].IndexOf("wait:", StringComparison.OrdinalIgnoreCase);
					if (pidIndex > -1)
					{
						var pidString = args[i].Substring(pidIndex + "pid:".Length);
						int.TryParse(pidString, out pid);
					}
					else if (waitIndex > -1)
					{
						var waitString = args[i].Substring(waitIndex + "wait:".Length);
						int.TryParse(waitString, out wait);
					}
					else
					{
						keysToSend = args[i].Replace("'", "\"");
					}
				}
			}

			if (!validArguments)
			{
				WriteError("Invalid arguments. Please define a process id and the string value to send as keys." +
					"\n  Example:  SendKeys.exe -pid:4711 \"Keys to send{Enter}\"" +
					"\n  Optional: Add -wait:100 to add a delay of 100 milliseconds, for example.");

				return;
			}

			Process process = null ;
			try
			{
				process = Process.GetProcessById(pid);
			}
			catch (Exception ex)
			{
				WriteError(ex.ToString());
				return;
			}

			if (process.MainWindowHandle == IntPtr.Zero)
			{
				WriteError($"Process {process.ProcessName} ({process.Id}) has no main window handle.");
			}
			else
			{
				if (wait > 0)
					Thread.Sleep(wait);

				SetForegroundWindow(process.MainWindowHandle);
				System.Windows.Forms.SendKeys.SendWait(keysToSend);
			}
		}

		private static void WriteError(string message)
		{
			AttachConsole(ATTACH_PARENT_PROCESS);
			Console.WriteLine(message);
			FreeConsole();
		}
	}
}
