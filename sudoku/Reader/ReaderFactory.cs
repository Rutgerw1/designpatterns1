using sudoku.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sudoku.Reader
{
	class ReaderFactory
	{
		private Dictionary<string, Func<ISudokuReader>> _types;
		public Dictionary<string, Func<ISudokuReader>> Types { get => _types; set => _types = value; }

		public ReaderFactory()
		{
			Types = new Dictionary<string, Func<ISudokuReader>>()
			{
				{ "4x4", () => new ClassicReader() },
				{ "6x6", () => new ClassicReader() },
				{ "9x9", () => new ClassicReader() },
				{ "jigsaw", () => new JigsawReader() },
				{ "samurai", () => new SamuraiReader() }
			};
		}

		public ISudokuReader GetReader(string ext)
		{
			return Types[ext]();
		}
	}
}
