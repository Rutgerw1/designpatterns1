using NUnit.Framework;
using sudoku.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class CompositeTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestAddComponents()
        {
            var composite = new Composite();
            var cellComponent = new Cell(new Point(3, 2), 2, 1);
            var puzzleComponent = new Puzzle(3, 3);
            
            composite.AddComponent(cellComponent);
            composite.AddComponent(puzzleComponent);

            Assert.True(composite.Components.Contains(cellComponent));
            Assert.True(composite.Components.Contains(puzzleComponent));
        }

        [Test]
        public void TestSubComponentsIsValidTrue()
        {
            var cell1 = new Cell(new Point(1, 2), 1, 1);
            var cell2 = new Cell(new Point(2, 2), 2, 1);
            var puzzle = new Puzzle(2,2);
            var composite = new Composite();
            composite.AddComponent(cell1);
            composite.AddComponent(cell2);
            puzzle.AddComponent(composite);

            Assert.True(puzzle.IsValid());
        }

        [Test]
        public void TestSubComponentsIsValidFalse()
        {
            var cell1 = new Cell(new Point(1, 2), 2, 1);
            var cell2 = new Cell(new Point(2, 2), 2, 1);
            var puzzle = new Puzzle(2, 2);
            var composite = new Composite();
            composite.AddComponent(cell1);
            composite.AddComponent(cell2);
            puzzle.AddComponent(composite);

            Assert.False(composite.IsValid(2));
        }

        [Test]
        public void TestContains()
        {
            var composite = new Composite();
            var cellComponent = new Cell(new Point(3, 2), 2, 1);

            composite.AddComponent(cellComponent);

            Assert.True(composite.Contains(cellComponent.Position));
        }

        [Test]
        public void TestDoesNotContain()
        {
            var composite = new Composite();
            var cellComponent = new Cell(new Point(3, 2), 2, 1);

            Assert.False(composite.Contains(cellComponent.Position));
        }

        [Test]
        public void TestIsValidTrue()
        {
            var cell1 = new Cell(new Point(1, 2), 1, 1);
            var cell2 = new Cell(new Point(2, 2), 2, 1);
            var composite = new Composite();
            composite.AddComponent(cell1);
            composite.AddComponent(cell2);

            Assert.True(composite.IsValid(2));
        }

        [Test]
        public void TestIsValidFalse()
        {
            var cell1 = new Cell(new Point(1, 2), 2, 1);
            var cell2 = new Cell(new Point(2, 2), 2, 1);
            var composite = new Composite();
            composite.AddComponent(cell1);
            composite.AddComponent(cell2);

            Assert.False(composite.IsValid(2));
        }

        [Test]
        public void TestChangeValue()
        {
            var composite = new Composite();
            var cell1 = new Cell(new Point(1, 2), 1, 1);
            var pointToCheck = new Point(1, 2);
            composite.AddComponent(cell1);

            composite.ChangeValueAtPosition(3, pointToCheck);

            Assert.True(composite.CellAtPosition(pointToCheck).Value == 3);
        }

        [Test]
        public void TestToggleNote()
        {
            var composite = new Composite();
            var cell1 = new Cell(new Point(1, 2), 1, 1);
            var pointToCheck = new Point(1, 2);
            composite.AddComponent(cell1);

            composite.ToggleNoteAtPosition(3, pointToCheck);

            Assert.True(composite.CellAtPosition(pointToCheck).Notes.Contains(3));
        }

        [Test]
        public void CellAtPositionTrue()
        {
            var composite = new Composite();
            var cell1 = new Cell(new Point(1, 2), 1, 1);
            composite.AddComponent(cell1);
            Assert.AreEqual(composite.CellAtPosition(cell1.Position), cell1);
        }

        [Test]
        public void CellAtPositionFalse()
        {
            var composite = new Composite();
            Assert.Null(composite.CellAtPosition(new Point(1, 1)));
        }
    }
}
