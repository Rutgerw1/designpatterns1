using NUnit.Framework;
using sudoku.Creation;
using sudoku.SolvingAlgorithm;
using sudoku.View;
using System.Collections.Generic;
using System.Drawing;

namespace UnitTests
{
    public class CreationTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestClassicFactory9x9()
        {
            var puzzleString = "700509001000000000150070063003904100000050000002106400390040076000000000600201004";
            var puzzle = new ClassicFactory().CreatePuzzle(puzzleString);

            Assert.AreEqual(puzzle.Components.Count, 27);
            Assert.AreEqual(puzzle.Size, 9);
        }

        [Test]
        public void TestClassicFactory6x6()
        {
            var puzzleString = "003010560320054203206450012045040100";
            var puzzle = new ClassicFactory().CreatePuzzle(puzzleString);

            Assert.AreEqual(puzzle.Components.Count, 18);
            Assert.AreEqual(puzzle.Size, 6);
        }

        [Test]
        public void TestClassicFactory4x4()
        {
            var puzzleString = "0340400210030210";
            var puzzle = new ClassicFactory().CreatePuzzle(puzzleString);

            Assert.AreEqual(puzzle.Components.Count, 12);
            Assert.AreEqual(puzzle.Size, 4);
        }

        [Test]
        public void TestSamuraiFactory()
        {
            var puzzleString = "800000700003050206700300095000091840000007002000062000000000000609080000002903000\r\n149000000000091000000060000007120008000000340405008067000000000000007020000050003\r\n000000000000008000000004000010600005030070080800005010000900000000800000000000000\r\n900060000030400000000000000390800407065000000200037600000080000000190000000000914\r\n000402800000080902000000000000610000400800000098750000670008001901060700002000009";
            var puzzle = new SamuraiFactory().CreatePuzzle(puzzleString);

            Assert.AreEqual(puzzle.Components.Count, 5);
            Assert.AreEqual(puzzle.Size, 21);
        }

        [Test]
        public void TestJigsawFactory()
        {
            var puzzleString = "SumoCueV1=0J0=8J0=0J0=0J1=0J2=0J2=0J2=5J2=0J2=8J3=0J0=0J1=0J1=0J2=7J2=0J4=0J4=5J4=0J3=0J0=0J1=0J1=9J2=0J2=0J5=0J6=0J4=0J3=7J0=0J1=1J1=6J5=9J5=0J5=0J6=0J4=0J3=0J0=4J1=3J1=0J5=1J7=8J7=0J6=0J4=0J3=0J0=0J5=8J5=7J5=6J7=0J7=3J6=0J4=0J3=0J0=0J5=0J8=5J8=0J7=0J7=0J6=0J4=3J3=0J3=0J3=6J8=0J8=0J7=0J7=0J6=2J4=0J8=9J8=0J8=0J8=0J8=0J7=0J6=8J6=0J6\r\n149000000000091000000060000007120008000000340405008067000000000000007020000050003\r\n000000000000008000000004000010600005030070080800005010000900000000800000000000000\r\n900060000030400000000000000390800407065000000200037600000080000000190000000000914\r\n000402800000080902000000000000610000400800000098750000670008001901060700002000009";
            var factory = new SudokuFactory("jigsaw");
            var puzzle = factory.CreatePuzzle(puzzleString);

            Assert.AreEqual(puzzle.Components.Count, 27);
            Assert.AreEqual(puzzle.Size, 9);
        }

        [Test]
        public void TestInvalidExtension()
        {
            var puzzleString = "invalid";
            
            var factory = new SudokuFactory(puzzleString);

            Assert.Throws<KeyNotFoundException>(() => factory.CreatePuzzle(puzzleString));
        }
    }
}
