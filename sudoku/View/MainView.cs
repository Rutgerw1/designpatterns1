using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.View
{
    class MainView : IView
    {
        public void PrintMessage(string message, ConsoleColor color = ConsoleColor.White)
		{
            Console.WriteLine(message);
        }

        public void PrintFilePrompt()
		{
            PrintMessage("Please select a file");
		}

        public void PrintInvalidFile(string typesString)
		{
            string message = String.Format("Not a valid input file. File must end with {0}", typesString);
            PrintMessage(message);
            PrintMessage("Press any key to try again:");
            Console.ReadKey();
		}
    }
}
