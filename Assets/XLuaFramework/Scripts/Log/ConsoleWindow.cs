using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;

namespace ConsoleWindows
{
	public class ConsoleWindow
	{
		TextWriter oldOutput;

		public void Initialize()
		{
			if ( !AttachConsole( 0x0ffffffff ) )
			{
				AllocConsole();
			}

			oldOutput = Console.Out;

			try
			{
				IntPtr stdHandle = GetStdHandle( STD_OUTPUT_HANDLE );
				Microsoft.Win32.SafeHandles.SafeFileHandle safeFileHandle = new Microsoft.Win32.SafeHandles.SafeFileHandle( stdHandle, true );
                FileStream fileStream = new FileStream(safeFileHandle, FileAccess.Write);
				System.Text.Encoding encoding = System.Text.Encoding.Default;
				StreamWriter standardOutput = new StreamWriter( fileStream, encoding );
				standardOutput.AutoFlush = true;
				Console.SetOut( standardOutput );
			}
			catch ( System.Exception e )
			{
				Debug.Log( "Couldn't redirect output: " + e.Message );
			}
		}

		public void Shutdown()
		{
			Console.SetOut( oldOutput );
			FreeConsole();
		}

		public void SetTitle( string strName )
		{
			SetConsoleTitle( strName );
		}

		private const int STD_OUTPUT_HANDLE = -11;

		[DllImport( "kernel32.dll", SetLastError = true )]
		static extern bool AttachConsole( uint dwProcessId );

		[DllImport( "kernel32.dll", SetLastError = true )]
		static extern bool AllocConsole();

		[DllImport( "kernel32.dll", SetLastError = true )]
		static extern bool FreeConsole();

		[DllImport( "kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall )]
		private static extern IntPtr GetStdHandle( int nStdHandle );

		[DllImport( "kernel32.dll" )]
		static extern bool SetConsoleTitle( string lpConsoleTitle );
	}
}