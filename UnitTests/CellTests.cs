using NUnit.Framework;
using sudoku.Creation;
using System.Drawing;
using sudoku.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class CellTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestConstructor()
        {
            var cell = new Cell(new Point(1, 3), 2, 9);
            Assert.AreEqual(cell.Position, new Point(1, 3));
            Assert.AreEqual(2, cell.Value);
            Assert.AreEqual(9, cell.RegionNumber);
            Assert.IsEmpty(cell.Notes);
        }

        [Test]
        public void TestIsValid()
        {
            var cell = new Cell(new Point(1, 3), 2, 9);
            var result = cell.IsValid(12);
            Assert.IsTrue(result);
        }

        [Test]
        public void TestIsNotValid()
        {
            var cell = new Cell(new Point(1, 3), 2, 9);
            var result = cell.IsValid(1);
            Assert.IsFalse(result);
        }

        [Test]
        public void TestToggleNote()
        {
            var cell = new Cell(new Point(1, 3), 2, 9);
            var valueToChange = 4;
            Assert.IsEmpty(cell.Notes);
            cell.ToggleNoteAtPosition(valueToChange, cell.Position);
            Assert.AreEqual(cell.Notes.First(), valueToChange);
            cell.ToggleNoteAtPosition(valueToChange, cell.Position);
            Assert.IsEmpty(cell.Notes);
        }

        [Test]
        public void TestChangeValue()
        {
            var value = 2;
            var newValue = 3;
            var cell = new Cell(new Point(1, 3), value, 9);
            Assert.AreEqual(cell.Value, value);
            cell.ChangeValueAtPosition(newValue, cell.Position);
            Assert.AreEqual(cell.Value, newValue);
        }

        [Test]
        public void TestCellAtPosition()
        {
            var cell = new Cell(new Point(1, 3), 2, 9);
            var testPoint = new Point(1, 3);
            var result = cell.CellAtPosition(testPoint);
            Assert.That(result, Is.EqualTo(cell));
        }

        [Test]
        public void TestCellNotAtPosition()
        {
            var cell = new Cell(new Point(1, 3), 2, 9);
            var testPoint = new Point(1, 4);
            var result = cell.CellAtPosition(testPoint);
            Assert.Null(result);
        }

        [Test]
        public void TestCellContains()
        {
            var cell = new Cell(new Point(1, 3), 2, 9);
            var result = cell.Contains(new Point(1, 3));
            Assert.True(result);
        }

        [Test]
        public void TestCellNotContains()
        {
            var cell = new Cell(new Point(1, 3), 2, 9);
            var result = cell.Contains(new Point(1, 4));
            Assert.False(result);
        }

        [Test]
        public void TestToString()
        {
            var cell = new Cell(new Point(1, 3), 2, 9);
            Assert.AreEqual("2", cell.ToString());
        }

        [Test]
        public void TestOffset()
        {
            var cell = new Cell(new Point(1, 3), 2, 9);
            cell.Offset(new Point(3,2));
            Assert.AreEqual(new Point(4,5), cell.Position);
        }


    }
}
