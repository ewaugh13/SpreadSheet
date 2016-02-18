using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using System.Text.RegularExpressions;
using Dependencies;

namespace SS
{

    /// <summary>
    /// An Spreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string is a cell name if and only if it consists of one or more letters, 
    /// followed by a non-zero digit, followed by zero or more digits.  Cell names
    /// are not case sensitive.
    /// 
    /// For example, "A15", "a15", "XY32", and "BC7" are cell names.  (Note that 
    /// "A15" and "a15" name the same cell.)  On the other hand, "Z", "X07", and 
    /// "hello" are not cell names."
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  
    /// In addition to a name, each cell has a contents and a value.  The distinction is
    /// important, and it is important that you understand the distinction and use
    /// the right term when writing code, writing comments, and asking questions.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In an empty spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError.
    /// The value of a Formula, of course, can depend on the values of variables.  The value 
    /// of a Formula variable is the value of the spreadsheet cell it names (if that cell's 
    /// value is a double) or is undefined (otherwise).  If a Formula depends on an undefined
    /// variable or on a division by zero, its value is a FormulaError.  Otherwise, its value
    /// is a double, as specified in Formula.Evaluate.
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        private HashSet<Cell> theSpreadSheet { get; set; }

        private DependencyGraph DepGraph { get; set; }

        /// <summary>
        /// Is the spreadsheet constructor
        /// </summary>
        public Spreadsheet()
        {
            theSpreadSheet = new HashSet<Cell>();
            DepGraph = new DependencyGraph();
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> CellsWithNames = new HashSet<string>();
            foreach (Cell element in theSpreadSheet)
            {
                if ((element.contents != null) && (element.contents as string != ""))
                {
                    CellsWithNames.Add(element.name);
                }
            }
            return CellsWithNames;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if ((name == null) || (!checkCellName(name)))
            {
                throw new InvalidNameException();
            }

            foreach (Cell element in theSpreadSheet)
            {
                if (element.name == name)
                {
                    return element.contents;
                }
            }
            throw new Exception();
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, double number)
        {
            HashSet<string> cellAndDependents = new HashSet<string>();

            if ((name == null) || (!checkCellName(name)))
            {
                throw new InvalidNameException();
            }

            cellAndDependents.Add(name);

            bool existingCell = false;

            foreach (Cell element in theSpreadSheet)
            {
                if (element.name == name)
                {
                    existingCell = true;
                    element.contents = number;
                    element.value = number;
                }
            }

            if (existingCell == false)
            {
                Cell newCell = new Cell(name, number);

                theSpreadSheet.Add(newCell);
            }


            foreach (string dependent in GetDirectDependents(name))
            {
                cellAndDependents.Add(dependent);
            }

            return cellAndDependents;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, string text)
        {
            HashSet<string> cellAndDependents = new HashSet<string>();

            if ((name == null) || (!checkCellName(name)))
            {
                throw new InvalidNameException();
            }

            cellAndDependents.Add(name);

            Cell newCell = new Cell(name, text);

            foreach (Cell element in theSpreadSheet)
            {
                if (text.Contains(element.name))
                {
                    DepGraph.AddDependency(name, element.name);
                }
            }

            foreach (string dependent in GetDirectDependents(name))
            {
                cellAndDependents.Add(dependent);
            }

            theSpreadSheet.Add(newCell);

            return cellAndDependents;
        }

        /// <summary>
        /// If formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            HashSet<string> cellAndDependents = new HashSet<string>();

            if ((name == null) || (!checkCellName(name)))
            {
                throw new InvalidNameException();
            }

            cellAndDependents.Add(name);

            Cell newCell = new Cell(name, formula);

            string formulaAsString = formula.ToString();

            foreach (Cell element in theSpreadSheet)
            {
                if (formulaAsString.Contains(element.name))
                {
                    DepGraph.AddDependency(name, element.name);
                }
            }

            foreach (string dependent in GetDirectDependents(name))
            {
                cellAndDependents.Add(dependent);
            }

            theSpreadSheet.Add(newCell);

            return cellAndDependents;

        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            if (name == null)
            {
                throw new ArgumentException();
            }

            if (!checkCellName(name))
            {
                throw new InvalidNameException();
            }

            foreach (string dependent in DepGraph.GetDependents(name))
            {
                yield return dependent;
            }
        }

        private bool checkCellName(string name)
        {
            bool isValid = true;

            Regex regex = new Regex(@"[a-z A-Z]+[1-9]+\d*");

            Match match = regex.Match(name);

            if (!match.Success)
            {
                isValid = false;
            }

            return isValid;
        }

        private double lookUp(string input)
        {
            foreach (Cell element in theSpreadSheet)
            {
                if (element.name.Equals(input))
                {
                    return element.value;
                }
            }
            throw new UndefinedVariableException(input);
        }


    }

    class Cell
    {
        public string name { get; private set; }
        public double value { get; set; }
        public object contents { get; set; }

        public Cell(string inputName)
        {
            name = inputName;
        }

        public Cell(string inputName, double inputValue)
        {
            name = inputName;
            contents = inputValue;
            value = inputValue;
        }

        public Cell(string inputName, string inputString)
        {
            name = inputName;
            contents = inputString;
        }

        public Cell(string inputName, Formula inputFormula)
        {
            name = inputName;
            contents = inputFormula;
        }
    }
}
