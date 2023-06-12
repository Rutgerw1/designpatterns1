using NUnit.Framework;
using sudoku.Creation;
using sudoku.SolvingAlgorithm;
using System.Drawing;

namespace UnitTests
{
    public class SudokuTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestTryMoveUp()
        {
            var puzzleString = "700509001000000000150070063003904100000050000002106400390040076000000000600201004";
            var puzzle = new ClassicFactory().CreatePuzzle(puzzleString);

            puzzle.TryMove(sudoku.Game.Direction.Up);

            Assert.True(puzzle.Cursor.Equals(new Point(0, 8)));
        }

        [Test]
        public void TestTryMoveRight()
        {
            var puzzleString = "700509001000000000150070063003904100000050000002106400390040076000000000600201004";
            var puzzle = new ClassicFactory().CreatePuzzle(puzzleString);

            puzzle.TryMove(sudoku.Game.Direction.Right);

            Assert.True(puzzle.Cursor.Equals(new Point(1, 0)));
        }

        [Test]
        public void TestTryMoveDown()
        {
            var puzzleString = "700509001000000000150070063003904100000050000002106400390040076000000000600201004";
            var puzzle = new ClassicFactory().CreatePuzzle(puzzleString);

            puzzle.TryMove(sudoku.Game.Direction.Down);

            Assert.True(puzzle.Cursor.Equals(new Point(0, 1)));
        }

        [Test]
        public void TestTryMoveLeft()
        {
            var puzzleString = "700509001000000000150070063003904100000050000002106400390040076000000000600201004";
            var puzzle = new ClassicFactory().CreatePuzzle(puzzleString);

            puzzle.TryMove(sudoku.Game.Direction.Left);

            Assert.True(puzzle.Cursor.Equals(new Point(8, 0)));
        }
    }
}