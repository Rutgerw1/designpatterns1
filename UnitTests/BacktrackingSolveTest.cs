using NUnit.Framework;
using sudoku.Creation;
using sudoku.SolvingAlgorithm;

namespace UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void TestSolvingAlgorithm()
        {
            var puzzleString = "700509001000000000150070063003904100000050000002106400390040076000000000600201004";
            var puzzle = new ClassicFactory().CreatePuzzle(puzzleString);
            var solver = new BacktrackAlgorithm();

            solver.Solve(puzzle);

            Assert.True(puzzle.FirstEmptyCell() == null);
            Assert.True(puzzle.IsValid());
        }
    }
}