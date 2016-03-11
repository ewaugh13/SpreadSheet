using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using System.Text.RegularExpressions;
using Dependencies;
using System.IO;
using System.Xml;
using System.Xml.Schema;


namespace SS
{

    // MODIFIED PARAGRAPHS 1-3 AND ADDED PARAGRAPH 4 FOR PS6
    /// <summary>
    /// An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of a regular expression (called IsValid below) and an infinite 
    /// number of named cells.
    /// 
    /// A string is a valid cell name if and only if (1) s consists of one or more letters, 
    /// followed by a non-zero digit, followed by zero or more digits AND (2) the C#
    /// expression IsValid.IsMatch(s.ToUpper()) is true.
    /// 
    /// For example, "A15", "a15", "XY32", and "BC7" are valid cell names, so long as they also
    /// are accepted by IsValid.  On the other hand, "Z", "X07", and "hello" are not valid cell 
    /// names, regardless of IsValid.
    /// 
    /// Any valid incoming cell name, whether passed as a parameter or embedded in a formula,
    /// must be normalized by converting all letters to upper case before it is used by this 
    /// this spreadsheet.  For example, the Formula "x3+a5" should be normalize to "X3+A5" before 
    /// use.  Similarly, all cell names and Formulas that are returned or written to a file must also
    /// be normalized.
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
        private Dictionary<string, Cell> theSpreadSheet { get; set; }

        private DependencyGraph DepGraph { get; set; }

        private Regex regexValidator = new Regex(@"^[a-zA-Z]+[1-9]\d*$");

        private Regex IsValid { get; set; }

        private bool changed;

        // ADDED FOR PS6
        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get
            {
                return changed;
            }

            protected set
            {
                changed = value;
            }
        }

        /// <summary>
        /// Is empty the spreadsheet constructor
        /// </summary>
        public Spreadsheet()
        {
            theSpreadSheet = new Dictionary<string, Cell>();
            DepGraph = new DependencyGraph();
            Changed = false;
            IsValid = new Regex(@"");
        }

        /// <summary>
        /// Is the spreadsheet constructor with a regex parameter
        /// </summary>
        public Spreadsheet(Regex isValid)
        {
            theSpreadSheet = new Dictionary<string, Cell>();
            Changed = false;
            DepGraph = new DependencyGraph();
            IsValid = isValid;
        }

        /// <summary>
        /// Creates a spreadsheet from source textreader
        /// </summary>
        public Spreadsheet(TextReader source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }

            theSpreadSheet = new Dictionary<string, Cell>();
            DepGraph = new DependencyGraph();
            Changed = false;

            XmlSchemaSet schemaSet = new XmlSchemaSet();

            schemaSet.Add(null, "Spreadsheet.xsd");

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = schemaSet;
            settings.ValidationEventHandler += ValidationCallback;

            try
            {
                using (XmlReader reader = XmlReader.Create(source, settings))
                {
                    while (reader.Read())
                    {
                        switch (reader.Name)
                        {
                            case "spreadsheet":
                                if (reader.NodeType != XmlNodeType.EndElement)
                                {
                                    IsValid = new Regex(reader["IsValid"]);
                                }
                                break;
                            case "cell":
                                if (reader.NodeType != XmlNodeType.EndElement)
                                {
                                    try
                                    {
                                        if (theSpreadSheet.ContainsKey(reader["name"]))
                                        {
                                            throw new SpreadsheetReadException("Already contains this value");
                                        }
                                        SetContentsOfCell(reader["name"], reader["contents"]);
                                    }

                                    catch (CircularException)
                                    {
                                        throw new SpreadsheetReadException("Circular exception");
                                    }
                                    catch (InvalidNameException)
                                    {
                                        throw new SpreadsheetReadException("Invalid name error");
                                    }
                                    catch (FormulaFormatException)
                                    {
                                        throw new SpreadsheetReadException("Formula format error");
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (!(e is IOException))
                {
                    throw new SpreadsheetReadException("Could not be read");
                }
                else
                {
                    throw new IOException();
                }
            }

        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> CellsWithNames = new HashSet<string>();
            foreach (Cell element in theSpreadSheet.Values)
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

            name = name.ToUpper();

            if (theSpreadSheet.ContainsKey(name))
            {
                return theSpreadSheet[name].contents;
            }

            return "";
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
        protected override ISet<string> SetCellContents(string name, double number)
        {
            HashSet<string> cellAndDependents = new HashSet<string>();

            if ((name == null) || (!checkCellName(name)))
            {
                throw new InvalidNameException();
            }

            name = name.ToUpper();

            if (theSpreadSheet.ContainsKey(name))
            {
                theSpreadSheet[name].contents = number;
            }

            else
            {
                theSpreadSheet.Add(name, new Cell(name, number));
            }

            foreach (string cellName in GetCellsToRecalculate(name))
            {
                cellAndDependents.Add(cellName);
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
        protected override ISet<string> SetCellContents(string name, string text)
        {
            HashSet<string> cellAndDependents = new HashSet<string>();

            if ((name == null) || (!checkCellName(name)))
            {
                throw new InvalidNameException();
            }

            if (text == null)
            {
                throw new ArgumentNullException();
            }

            name = name.ToUpper();

            if (theSpreadSheet.ContainsKey(name))
            {
                theSpreadSheet[name].contents = text;
            }

            else
            {
                theSpreadSheet.Add(name, new Cell(name, text));
            }

            foreach (string cellName in GetCellsToRecalculate(name))
            {
                cellAndDependents.Add(cellName);
            }

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
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            HashSet<string> cellAndDependents = new HashSet<string>();

            if ((name == null) || (!checkCellName(name)))
            {
                throw new InvalidNameException();
            }

            if (formula.Equals(null))
            {
                throw new ArgumentNullException();
            }

            name = name.ToUpper();

            object oldValue = new object();

            if (theSpreadSheet.ContainsKey(name))
            {
                oldValue = theSpreadSheet[name].contents;

                theSpreadSheet[name].contents = formula;
                if (DepGraph.HasDependees(name))
                {
                    DepGraph.ReplaceDependees(name, formula.GetVariables());
                }
                else
                {
                    foreach (string variable in formula.GetVariables())
                    {
                        DepGraph.AddDependency(variable, name);
                    }
                }
            }

            else
            {
                theSpreadSheet.Add(name, new Cell(name, formula));

                foreach (string variable in formula.GetVariables())
                {
                    DepGraph.AddDependency(variable, name);
                }
            }

            try
            {
                foreach (string cellName in GetCellsToRecalculate(name))
                {
                    cellAndDependents.Add(cellName);
                }
            }

            catch (CircularException)
            {
                theSpreadSheet[name].contents = oldValue;
                throw new CircularException();
            }

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

            name = name.ToUpper();

            foreach (string dependent in DepGraph.GetDependents(name))
            {
                yield return dependent;
            }
        }

        // ADDED FOR PS6
        /// <summary>
        /// Writes the contents of this spreadsheet to dest using an XML format.
        /// The XML elements should be structured as follows:
        ///
        /// <spreadsheet IsValid="IsValid regex goes here">
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        /// </spreadsheet>
        ///
        /// The value of the isvalid attribute should be IsValid.ToString()
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.
        /// If the cell contains a string, the string (without surrounding double quotes) should be written as the contents.
        /// If the cell contains a double d, d.ToString() should be written as the contents.
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        ///
        /// If there are any problems writing to dest, the method should throw an IOException.
        /// </summary>
        public override void Save(TextWriter dest)
        {
            try
            {
                using (XmlWriter writer = XmlWriter.Create(dest))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("IsValid", IsValid.ToString());

                    List<string> cellsWithValues = new List<string>();

                    foreach (string cellElement in GetNamesOfAllNonemptyCells())
                    {
                        cellsWithValues.Add(cellElement);
                    }

                    foreach (string cellElement in cellsWithValues)
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteAttributeString("name", cellElement);
                        if (theSpreadSheet[cellElement].contents is string)
                        {
                            writer.WriteAttributeString("contents", (string)theSpreadSheet[cellElement].contents);
                        }

                        else if (theSpreadSheet[cellElement].contents is double)
                        {
                            writer.WriteAttributeString("contents", theSpreadSheet[cellElement].contents.ToString());
                        }

                        else if (theSpreadSheet[cellElement].contents is Formula)
                        {
                            writer.WriteAttributeString("contents", "=" + theSpreadSheet[cellElement].contents.ToString());
                        }

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
                Changed = false;
            }

            catch (Exception e)
            {
                if (!(e is IOException))
                {
                    throw new SpreadsheetReadException("Sheet could not be read");
                }
                else
                {
                    throw new IOException();
                }
            }
        }

        // ADDED FOR PS6
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            if ((name == null) || (!checkCellName(name)))
            {
                throw new InvalidNameException();
            }

            name = name.ToUpper();

            if (theSpreadSheet.ContainsKey(name))
            {
                double value;
                if (Double.TryParse(theSpreadSheet[name].contents.ToString(), out value))
                {
                    theSpreadSheet[name].value = value;
                    return value;
                }

                if (theSpreadSheet[name].contents.GetType().Name.Equals("Formula"))
                {
                    try
                    {
                        theSpreadSheet[name].value = new Formula(theSpreadSheet[name].contents.ToString()).Evaluate(lookUp);
                    }

                    catch (FormulaEvaluationException)
                    {
                        theSpreadSheet[name].value = new FormulaError("The formula contents and not defined in the spreadsheet");
                    }
                    return theSpreadSheet[name].value;
                }

                theSpreadSheet[name].value = theSpreadSheet[name].contents;
                return theSpreadSheet[name].contents;
            }

            else
            {
                return "";
            }
        }

        // ADDED FOR PS6
        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        ///
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor with s => s.ToUpper() as the normalizer and a validator that
        /// checks that s is a valid cell name as defined in the AbstractSpreadsheet
        /// class comment.  There are then three possibilities:
        ///
        ///   (1) If the remainder of content cannot be parsed into a Formula, a
        ///       Formulas.FormulaFormatException is thrown.
        ///
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///
        ///   (3) Otherwise, the contents of the named cell becomes f.
        ///
        /// Otherwise, the contents of the named cell becomes content.
        ///
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            ISet<string> cellAndDependents = new HashSet<string>();

            if ((name == null) || (!checkCellName(name)))
            {
                throw new InvalidNameException();
            }

            if (content == null)
            {
                throw new ArgumentNullException();
            }

            name = name.ToUpper();

            double value = 0;
            if (double.TryParse(content, out value))
            {
                cellAndDependents = SetCellContents(name, value);
            }

            else if (content.Length > 0 && content[0] == ('='))
            {
                content = content.Substring(1);
                cellAndDependents = SetCellContents(name, new Formula(content, s => s.ToUpper(), checkCellName));
            }

            else
            {
                cellAndDependents = SetCellContents(name, content);
            }

            Changed = true;

            GetCellValue(name);


            foreach (string cell in GetCellsToRecalculate(name))
            {
                if (name != cell)
                {
                    GetCellValue(cell);
                }
            }

            return cellAndDependents;

        }

        /// <summary>
        /// Checks if the cell validates the regex
        /// </summary>
        private bool checkCellName(string name)
        {
            bool IsValidBool = true;

            Match match = IsValid.Match(name);

            Match matchValidator = regexValidator.Match(name);

            if (!match.Success || !matchValidator.Success)
            {
                IsValidBool = false;
            }

            if (IsValidBool == true)
            {
                if (char.IsLetter(name[1]))
                {
                    IsValidBool = false;
                }

                double value;
                if (double.TryParse(name.Substring(1), out value))
                {
                    if (value > 99 || value < 1)
                    {
                        IsValidBool = false;
                    }
                }
            }

            return IsValidBool;
        }

        /// <summary>
        /// Looks up the value for the evauluator
        /// </summary>
        private double lookUp(string input)
        {
            if (theSpreadSheet.ContainsKey(input))
            {
                double value;
                if (Double.TryParse(theSpreadSheet[input].contents.ToString(), out value))
                {
                    return value;
                }

                if (Double.TryParse(theSpreadSheet[input].value.ToString(), out value))
                {
                    return value;
                }
            }
            throw new UndefinedVariableException(input);
        }

        // Display any validation errors.
        private static void ValidationCallback(object sender, ValidationEventArgs e)
        {
            throw new SpreadsheetReadException("Validation failed");
        }

    }

    /// <summary>
    /// Creates the cell elements that contains a name, value, and contents
    /// </summary>
    class Cell
    {
        public string name { get; private set; }
        public object value { get; set; }
        public object contents { get; set; }

        public Cell(string inputName, object inputObject)
        {
            name = inputName;
            contents = inputObject;
        }
    }
}
