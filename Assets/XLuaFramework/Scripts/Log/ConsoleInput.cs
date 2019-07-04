#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
using System;

namespace ConsoleWindows
{
	public class ConsoleInput
	{
		public event System.Action<string> OnInputText;
		public string inputString;

		public void ClearLine()
		{
            Console.CursorLeft = 0;
			Console.Write( new String( ' ', Console.BufferWidth ) );
			Console.CursorTop--;
			Console.CursorLeft = 0;
		}

		public void RedrawInputLine()
		{
			if ( inputString.Length == 0 ) return;

			if ( Console.CursorLeft > 0 )
				ClearLine();

			//System.Console.ForegroundColor = ConsoleColor.Green;
			System.Console.Write( inputString );
		}

		internal void OnBackspace()
		{
			if ( inputString.Length < 1 ) return;

			inputString = inputString.Substring( 0, inputString.Length - 1 );
			RedrawInputLine();
		}

		internal void OnEscape()
		{
			ClearLine();
			inputString = "";
		}

		internal void OnEnter()
		{
			ClearLine();
			//System.Console.ForegroundColor = ConsoleColor.Green;
			System.Console.WriteLine( "> " + inputString );

			var strtext = inputString;
			inputString = "";

			if ( OnInputText != null )
			{
				OnInputText( strtext );
			}
		}

		public void Update()
		{
			if ( !Console.KeyAvailable ) return;
			var key = Console.ReadKey();

			if ( key.Key == ConsoleKey.Enter )
			{
				OnEnter();
				return;
			}

			if ( key.Key == ConsoleKey.Backspace )
			{
				OnBackspace();
				return;
			}

			if ( key.Key == ConsoleKey.Escape )
			{
				OnEscape();
				return;
			}

			if ( key.KeyChar != '\u0000' )
			{
				inputString += key.KeyChar;
				RedrawInputLine();
				return;
			}
		}
	}
}
#endif