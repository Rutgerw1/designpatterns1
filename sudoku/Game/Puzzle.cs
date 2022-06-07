using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku.Game
{
    public class Puzzle
    {
        private Row[] _rows;
		public Row[] Rows { get => _rows; }

		private Column[] _columns;
		public Column[] Columns { get => _columns; }

		private Region[] _regions;
		public Region[] Regions { get => _regions; }

        private (int X, int Y) _location = (0, 0); 
        public (int X, int Y) Location { get => _location; }


		public Puzzle(Row[] rows, Column[] columns, Region[] regions)
		{
			_rows = rows;
			_columns = columns;
			_regions = regions;
		}

		public void TryMove(Direction direction)
        {
			int nextX = _location.X;
			int nextY = _location.Y;

			switch (direction)
			{
				case Direction.Up:
					nextY = _location.Y - 1;
					break;
				case Direction.Right:
					nextX = _location.X + 1;
					break;
				case Direction.Down:
					nextY = _location.Y + 1;
					break;
				case Direction.Left:
					nextX = _location.X - 1;
					break;
			}
			if (!(nextX == _columns.Length || nextX == -1 || nextY == _rows.Length || nextY == -1))
			{
				_location.X = nextX;
				_location.Y = nextY;
			}
		}

        public void ChangeCellValue(int number)
        {
			if (_rows[_location.Y].Cells[_location.X].Number == number)
            {
				_rows[_location.Y].Cells[_location.X].Number = 0;
			}
			else
            {
				_rows[_location.Y].Cells[_location.X].Number = number;
            }
        }
    }
}
