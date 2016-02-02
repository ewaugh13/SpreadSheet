// Skeleton written by Joe Zachary for CS 3500, January 2015
// Revised by Joe Zachary, January 2016
// JLZ Repaired pair of mistakes, January 23, 2016

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public class Formula
    {
        private List<string> Tokens { get; set; }

        private List<string> typesOfTokens { get; set; }

        /// <summary>
        ///         /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// 
        /// Constructs the formula based on the string inputed in the parameter called formula. It creates a list using strings 
        /// called Tokens using the GetTokens method, which splits the string into each individual part. It then checks to make sure
        /// the token count is greater than 0. Following that is uses the method testTokens that goes through each token making sure
        /// they are off the right type while also adding a string into typesOfTokens, where the string is the type of token in the 
        /// congruent index of tokens. It then checks to make sure the amount of both parenthesis types are the same. Lastly using
        /// the testOrder method it makes sure the formula given is in a order that is acceptable.
        /// </summary>
        public Formula(String formula)
        {
            int closingParenthesis;
            int openingParenthesis;
            typesOfTokens = new List<string>();

            Tokens = new List<string>();
            foreach (string token in GetTokens(formula)) //Goes through each token yield returned and adds to Tokens
            {
                Tokens.Add(token);
            }

            if (Tokens.Count < 1)
            {
                throw new FormulaFormatException("Formula can't be constructed with no tokens");
            }

            testTokens(out closingParenthesis, out openingParenthesis);

            if (closingParenthesis != openingParenthesis)
            {
                throw new FormulaFormatException("Formula can't be constructed with uneven parenthesis");
            }

            testOrder();

        }

        /// <summary>
        /// Goes through each token determing if it's of the correct type. It is either looking for a operator which
        /// would be +, -, *, or /. It then checks to see if it's either of the parenthesis. It then checks to see if 
        /// it's a number by using try parse. Lastly it looks at the first character of the tokken to see if it's a variable
        /// and as long as it's 1 letter followed by 0 or more letters and then followed by digits then it is a variable. For
        /// each type that is found it adds a string for each type into the typesOfTokens at congruent indexs of the list
        /// of tokens called Tokens. To check if it's any of these 5 types a bool called isValid is set to false and will
        /// only be set to true if it's one of these 5 types.
        /// </summary>
        private void testTokens(out int closingParenthesis, out int openingParenthesis)
        {
            string[] operators = { "*", "+", "-", "/" };

            closingParenthesis = 0;
            openingParenthesis = 0;
            typesOfTokens = new List<string>();

            bool isValid;
            for (int i = 0; i < Tokens.Count; i++)
            {
                isValid = false;
                string currentToken = Tokens[i];
                char[] currentTokenCharArray = currentToken.ToCharArray();

                if (operators.Contains(currentToken)) //Checks if operators contains the token
                {
                    isValid = true;
                    typesOfTokens.Add("operator");
                }

                if (currentToken.Equals("("))
                {
                    isValid = true;
                    typesOfTokens.Add("opening parenthesis");
                    openingParenthesis++;
                }
                else if (currentToken.Equals(")"))
                {
                    isValid = true;
                    typesOfTokens.Add("closing parenthesis");
                    closingParenthesis++;
                }

                Double value;
                if (double.TryParse(currentToken, out value)) //Does TryParse to see if it's a number
                {
                    if (value > 0)
                    {
                        isValid = true;
                        typesOfTokens.Add("number");
                    }
                }

                if (Char.IsLetter(currentTokenCharArray[0])) //Checks if the first character is a letter
                {
                    isValid = true;
                    typesOfTokens.Add("variable");
                    bool previousIsNum = false;
                    for (int k = 1; k < currentTokenCharArray.Length; k++)
                    {
                        if (previousIsNum == false)
                        {
                            if (!(Char.IsLetter(currentTokenCharArray[k]))) 
                            {
                                if (Char.IsDigit(currentTokenCharArray[k])) //If it's not a letter it then checks for a number and if it is then it knows that the next char can't be a letter
                                {
                                    previousIsNum = true;
                                }
                                else
                                {
                                    isValid = false;
                                }
                            }
                        }
                        else
                        {
                            if (!(Char.IsDigit(currentTokenCharArray[k])))
                            {
                                isValid = false;
                            }
                        }
                    }
                }

                if (isValid == false) //If the token is none of these types then it will throw an exception
                {
                    throw new FormulaFormatException("Formula can't be constructed with this input");
                }
            }

        }

        /// <summary>
        /// This method uses the info put into typesOfTokens. For each parenthesis of both types it will add to the count of that certain type
        /// to make sure there aren't more closing followed after opening. If it's looking at the first element it checks to see if it's a number,
        /// variable, or opening parenthesis. If it's the last element it checks if it's a number, variable, or closing parenthesis. It then compares
        /// the current type to the previous and if the previous type is a opening parenthesis or operator it needs to be followed by a number, variable, or
        /// a opening parenthesis. If the previous token is of the types number, variable or closing parenthesis then it needs to be followed by an operator
        /// or a closing parenthesis. If any of this isn't correct it will throw an exception.
        /// </summary>
        private void testOrder()
        {
            int currentOpeningParenthesis = 0;
            int currentClosingParenthesis = 0;

            string previousToken = "";


            for (int i = 0; i < Tokens.Count; i++)
            {
                if (typesOfTokens[i].Equals("opening parenthesis"))
                {
                    currentOpeningParenthesis++;
                }

                if (typesOfTokens[i].Equals("closing parenthesis"))
                {
                    currentClosingParenthesis++;
                }

                if (currentClosingParenthesis > currentOpeningParenthesis)
                {
                    throw new FormulaFormatException("Formula can't be constructed because the number of closing so far is greater than the opening");
                }

                if (i == 0)
                {
                    if (!(typesOfTokens[0].Equals("number") || typesOfTokens[0].Equals("variable") || typesOfTokens[0].Equals("opening parenthesis")))
                    {
                        throw new FormulaFormatException("Formula can't be constructed with first element not being a number, variable, or open parenthesis");
                    }
                }

                if (i == Tokens.Count - 1)
                {
                    if (!(typesOfTokens[i].Equals("number") || typesOfTokens[i].Equals("variable") || typesOfTokens[i].Equals("closing parenthesis")))
                    {
                        throw new FormulaFormatException("Formula can't be constructed with first element not being a number, variable, or closing parenthesis");
                    }
                }


                if (previousToken.Equals("opening parenthesis") || previousToken.Equals("operator"))
                {
                    if (!(typesOfTokens[i].Equals("number") || typesOfTokens[i].Equals("variable") || typesOfTokens[i].Equals("opening parenthesis")))
                    {
                        throw new FormulaFormatException("Formula can't be constructed because you have to follow up a open parenthesis or operator with a number, variable, or open parenthesis");
                    }
                }

                if (previousToken.Equals("number") || previousToken.Equals("variable") || previousToken.Equals("closing parenthesis"))
                {
                    if (!(typesOfTokens[i].Equals("operator") || typesOfTokens[i].Equals("closing parenthesis")))
                    {
                        throw new FormulaFormatException("Formula can't be constructed because you have to follow up a close parenthesis, number or variable with a operator or close parenthesis");
                    }
                }

                previousToken = typesOfTokens[i];

            }
        }

        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            Stack<Tuple<string, string>> operators = new Stack<Tuple<string, string>>();
            Stack<double> valueStack = new Stack<double>();
            Tuple<string, string> currentTuple;
            Tuple<string, string> topTuple;


            for (int i = 0; i < Tokens.Count; i++)
            {
                currentTuple = new Tuple<string, string>(Tokens[i], typesOfTokens[i]);

                if (operators.Count > 0 || valueStack.Count > 0) // Checks to see if there are any elements in either stacks
                {

                    if (currentTuple.Item2.Equals("number")) //If the currentTuple contains a number it adds it to the valueStack and checks the operators.
                        //If the peek of the operators is a multiply or divide it will then take the other value in valueStack and either multiply or divide 
                        //and then restore that into the valueStack.
                    {

                        double value;
                        double.TryParse(currentTuple.Item1, out value);

                        if (operators.Count > 0)
                        {
                            topTuple = operators.Peek();

                            if (topTuple.Item1.Equals("*") || topTuple.Item1.Equals("/"))
                            {
                                double topValue = valueStack.Pop();

                                if (topTuple.Item1.Equals("*"))
                                {
                                    topValue = topValue * value;
                                }
                                else
                                {
                                    if (value == 0)
                                    {
                                        throw new FormulaEvaluationException("Can't divide a number by 0");
                                    }
                                    else
                                    {
                                        topValue = topValue / value;
                                    }

                                }

                                valueStack.Push(topValue);
                                operators.Pop();

                            }
                            else
                            {
                                valueStack.Push(value);
                            }
                        }
                        else
                        {
                            valueStack.Push(value);
                        }

                    }

                    else if (currentTuple.Item1.Equals("+") || currentTuple.Item1.Equals("-")) //Checks to see if the currentTuple is a plus or minus.
                        //It then sees if the operators peek is another plus or minus and if it is it will calculate the value using the other number
                        //in the valueStack and then restore it. If not it will just add the tuple to the operators.
                    {
                        if (operators.Count > 0)
                        {
                            topTuple = operators.Peek();
                            if (topTuple.Item1.Equals("+") || topTuple.Item1.Equals("-"))
                            {
                                double topValue = valueStack.Pop();
                                double nextValue = valueStack.Pop();

                                if (topTuple.Item1.Equals("+"))
                                {
                                    topValue = nextValue + topValue;
                                }

                                else
                                {
                                    topValue = nextValue - topValue;
                                }

                                valueStack.Push(topValue);
                                operators.Pop();
                            }
                        }

                        operators.Push(currentTuple);

                    }

                    else if (currentTuple.Item1.Equals("*") || currentTuple.Item1.Equals("/")) // Adds multiply or divide to the operators
                    {
                        operators.Push(currentTuple);
                    }

                    else if (currentTuple.Item2.Equals("opening parenthesis")) // Adds opening parenthesis to operators
                    {
                        operators.Push(currentTuple);
                    }

                    else if (currentTuple.Item2.Equals("closing parenthesis")) // Will check if operators peek if plus or minus and it so
                        //it will calculate the value of the 2 numbers in the valueStack and restore it. It will then pop the opening parenthesis.
                        //if the new peek of the operators is a multiply or divide it will repeate the process but will multiply or divide the 
                        //values in the valueStack
                    {
                        if (operators.Count > 1)
                        {
                            topTuple = operators.Peek();
                            if (topTuple.Item1.Equals("+") || topTuple.Item1.Equals("-"))
                            {
                                double topValue = valueStack.Pop();
                                double nextValue = valueStack.Pop();

                                if (topTuple.Item1.Equals("+"))
                                {
                                    topValue = nextValue + topValue;
                                }

                                else
                                {
                                    topValue = nextValue - topValue;
                                }

                                valueStack.Push(topValue);
                                operators.Pop();
                            }

                        }
                        operators.Pop();
                        if (operators.Count > 0)
                        {
                            topTuple = operators.Peek();
                            if (topTuple.Item1.Equals("*") || topTuple.Item1.Equals("/"))
                            {
                                double topValue = valueStack.Pop();
                                double nextValue = valueStack.Pop();

                                if (topTuple.Item1.Equals("*"))
                                {
                                    topValue = nextValue * topValue;
                                }
                                else
                                {
                                    if (topValue == 0)
                                    {
                                        throw new FormulaEvaluationException("Can't divide a number by 0");
                                    }
                                    else
                                    {
                                        topValue = nextValue / topValue;
                                    }
                                }
                                valueStack.Push(topValue);
                                operators.Pop();

                            }
                        }

                    }

                    else if (currentTuple.Item2.Equals("variable")) //If the tuple is a variable it will try looking up and if it's not declared it
                        //will throw an exception. It then does a similar process to what number checks for and will multiply or divide if it meets
                        //what it needs to do so and store the value in the valueStack. If not it stores the value gotten from the variable in the
                        //value stack.
                    {
                        double value;
                        try
                        {
                            value = lookup(currentTuple.Item1);
                        }
                        catch (UndefinedVariableException)
                        {
                            throw new FormulaEvaluationException("Variables are not defined");
                        }

                        if (operators.Count > 0)
                        {
                            topTuple = operators.Peek();

                            if (topTuple.Item1.Equals("*") || topTuple.Item1.Equals("/"))
                            {
                                double topValue = valueStack.Pop();

                                if (topTuple.Item1.Equals("*"))
                                {
                                    topValue = topValue * value;
                                }
                                else
                                {
                                    if (value == 0)
                                    {
                                        throw new FormulaEvaluationException("Can't divide a number by 0");
                                    }
                                    else
                                    {
                                        topValue = topValue / value;
                                    }

                                }

                                valueStack.Push(topValue);
                                operators.Pop();
                                
                            }
                            else
                            {
                                valueStack.Push(value);
                            }
                        }
                        else
                        {
                            valueStack.Push(value);
                        }
                    }
                }



                else //This is for if there aren't any elements in either stack. If it's number or variable it adds to the valueStack(as long as
                //the variable is declared). If not it adds the element to the operator stack.
                {
                    if (currentTuple.Item2.Equals("number"))
                    {
                        double value;
                        double.TryParse(currentTuple.Item1, out value);
                        valueStack.Push(value);
                    }
                    else
                    {
                        if (currentTuple.Item2.Equals("variable"))
                        {
                            double value;
                            try
                            {
                                value = lookup(currentTuple.Item1);
                            }
                            catch(UndefinedVariableException)
                            {
                                throw new FormulaEvaluationException("Variables are not defined");
                            }
                            valueStack.Push(value);
                        }
                        else
                        {
                            operators.Push(currentTuple);
                        }
                    }
                }
            }


            if (valueStack.Count > 1) //After going through all the tokens if the valueStack has more than one element
                //it will either subtract or add them together and  restore the value.
            {
                topTuple = operators.Peek();
                if (topTuple.Item1.Equals("+") || topTuple.Item1.Equals("-"))
                {
                    double topValue = valueStack.Pop();
                    double nextValue = valueStack.Pop();

                    if (topTuple.Item1.Equals("+"))
                    {
                        topValue = nextValue + topValue;
                    }

                    else
                    {
                        topValue = nextValue - topValue;
                    }

                    valueStack.Push(topValue);
                }
            }

            return valueStack.Pop(); //Returns the final value from the valueStack
        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of a letter followed by
        /// zero or more digits and/or letters, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\("; // (
            String rpPattern = @"\)"; // )
            String opPattern = @"[\+\-*/]"; // +, \, -, *
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                       lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }

    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>
    public delegate double Lookup(string s);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {

        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
            Console.WriteLine(message);
        }
    }
}
