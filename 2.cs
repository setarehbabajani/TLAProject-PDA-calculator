using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Q2
{
    public class Q2
    {
        public static double Calculator(string str)
        {
            Stack<double> Numbers = new Stack<double>();
            Stack<string> Operations = new Stack<string>();
            if(str.Contains(".."))
                return int.MaxValue;
            for (int i = 0; i < str.Length; i++)
            {
                char strI = str[i];
                string sstr = strI.ToString();
                if(strI == '-' && i != str.Length-1 && str[i+1] >= '0' && str[i+1] <= '9')//negative numbers
                {
                    while(i != str.Length-1 && ((str[i+1] >= '0' && str[i+1] <= '9') || str[i+1] == '.'))
                    {
                        sstr += str[i+1];
                        i++;
                    }
                    Numbers.Push(double.Parse(sstr));
                }
                else
                {
                    if(strI == ' ')
                    {
                        double x = 0;
                        if(i!= 0 && i != str.Length-1 && double.TryParse(str[i-1].ToString(),out x) && double.TryParse(str[i+1].ToString(),out x))
                            return int.MaxValue;
                        continue;
                    }
                    else if (strI == '(')
                        Operations.Push(sstr);
                    else if (strI == ')')
                    {
                        if(i!= 0 && str[i-1] == '(')
                            return int.MaxValue;
                        while (Operations.Peek() != "(")
                        {
                            string thisOp = Operations.Peek();
                            if(thisOp == "+" || thisOp == "-" || thisOp == "*" || thisOp == "/" || thisOp == "^")
                                Numbers.Push(WhichOp(Operations.Pop(), Numbers.Pop(), Numbers.Pop()));
                            else
                                Numbers.Push(WhichUniqeOp(Operations.Pop(), Numbers.Pop()));
                        }
                        Operations.Pop();
                    }
                    else if (strI == '+' || (strI == '-') || strI == '*' || strI == '/' || strI == '^')
                    {
                        if(i == 0 || (i > 0 && (str[i-1] == '+' || str[i-1] == '-' ||str[i-1] == '/' ||str[i-1] == '*' ||str[i-1] == '^' ||str[i-1] == '(')))
                            return int.MaxValue;
                        while(Operations.Count > 0 && ThereIsPriority(sstr, Operations.Peek()))
                            Numbers.Push(WhichOp(Operations.Pop(), Numbers.Pop(), Numbers.Pop()));
                        Operations.Push(sstr);
                    }
                    else if(strI == 't' || strI == 'c' || strI == 'a' || strI == 'e')
                    {
                        while(Operations.Count > 0 && ThereIsPriority(sstr, Operations.Peek()))
                            Numbers.Push(WhichOp(Operations.Pop(), Numbers.Pop(), Numbers.Pop()));
                        Operations.Push(sstr);
                        i += 2;
                    }
                    else if(strI == 'l')//l --> ln
                    {
                        while(Operations.Count > 0 && ThereIsPriority(sstr, Operations.Peek()))
                            Numbers.Push(WhichOp(Operations.Pop(), Numbers.Pop(), Numbers.Pop()));
                        Operations.Push(sstr);
                        i++;
                    }
                    else if (strI == 's')
                    {
                        if(i+1 != str.Length && str[i + 1] == 'i')
                        {
                            sstr = "si";
                            i += 2;
                        }
                        else
                            i += 3;
                        while(Operations.Count > 0 && ThereIsPriority(sstr, Operations.Peek()))
                            Numbers.Push(WhichOp(Operations.Pop(), Numbers.Pop(), Numbers.Pop()));
                        Operations.Push(sstr);
                    }
                    else//for numbers
                    {
                        // if (i >= 2 && (str[i-2] != '+' && str[i-2] != '-' && str[i-2] != '*' && str[i-2] != '/' && str[i-2] != '^' && str[i-2] != 'n' && str[i-2] != 's'&& str[i-2] != 'p' && str[i-2] != 't' && str[i-2] != ' '))
                        //     return double.MaxValue;
                        while(i != str.Length-1 && ((str[i+1] >= '0' && str[i+1] <= '9') || str[i+1] == '.'))
                        {
                            sstr += str[i+1];
                            i++;
                        }
                        Numbers.Push(double.Parse(sstr));
                    }
                }
            }
            while (Operations.Count > 0)
            {
                if(Numbers.Count >= 2)
                {
                    double num1 = WhichOp(Operations.Pop(), Numbers.Pop(), Numbers.Pop());
                    if(num1 == int.MaxValue)
                        return num1;
                    Numbers.Push(num1);
                }
                else
                {
                    double num2 = WhichUniqeOp(Operations.Pop(), Numbers.Pop());
                    if(num2 == int.MaxValue)
                        return num2;
                    Numbers.Push(num2);
                }
            }
            if(Numbers.Count > 1)
                return int.MaxValue;
            return Numbers.Pop();
        }
        public static bool ThereIsPriority(string op1, string op2)
        {
            if (op2 == "(" || op2 == ")")
                return false;
            if ((op1 == "*" || op1 == "/") && (op2 == "+" || op2 == "-"))
                return false;
            if ((op1 == "^") && (op2 == "+" || op2 == "-" || op2 == "*" || op2 == "/"))//op2 == "/"----
                return false;
            if ((op1 == "si" || op1 == "c" || op1 == "t" || op1 == "l" || op1 == "e" || op1 == "a" || op1 == "s") && (op2 == "+" || op2 == "-" || op2 == "*" || op2 == "^" || op2 == "/"))//op2 == "/"
                return false;
            else
                return true;
        }
        public static double WhichOp(string Op, double b, double a)
        {
            switch (Op)
            {
                case "+":
                return a + b;
                case "-":
                return a - b;
                case "*":
                return a * b;
                case "^":
                return Math.Pow(a,b);
                case "/":
                if (b == 0)
                    return int.MaxValue;
                return a / b;
            }
            return 0;
        }
        public static double WhichUniqeOp(string Op, double n)
        {
            switch (Op)
            {
                case "s"://sqrt
                if (n < 0)
                    return int.MaxValue;
                return Math.Sqrt(n);
                case "t"://tan
                return Math.Tan(n);
                case "c"://cos
                return Math.Cos(n);
                case "si"://sin
                return Math.Sin(n);
                case "a"://abs
                return Math.Abs(n);
                case "l"://ln
                if (n <= 0)
                    return int.MaxValue;
                return Math.Log(n);
                case "e"://exp
                return Math.Exp(n);
            }
            return 0;
        }
        public static bool Check_Brackets(string s)
        {
            int countres = 0;
            int newStackPos = 0;//for making new pos for stack when we see start and end of it
            bool IsBalanced = true;
            int length = s.Length;
            Stack stack = new Stack();
            for(int ch = 0; ch < length; ch++)
            {
                char currch = s[ch];
                if(stack.Count == 0)
                    newStackPos = countres;
                if(currch == '(')
                    stack.Push(s[ch]);
                else if(currch == ')')//ignoring if it's letter
                {
                    if(stack.Count == 0)
                    {
                        IsBalanced = false;
                        break;
                    }
                    var top = stack.Pop();
                    if((char)top == '('  && currch != ')')
                    {
                        IsBalanced = false;
                        break;
                    }
                }
                countres++;
            }
            if(IsBalanced && stack.Count == 0)
                return true;
            return false;
        }
        static void Main()
        {
            string input = Console.ReadLine();
            if(Check_Brackets(input))
            {
                double result = Calculator(input);
                if(result == int.MaxValue)
                    System.Console.WriteLine("INVALID");
                else
                {
                    if(result.ToString().Contains("."))
                    {
                        if((result * 10).ToString().Contains("."))
                        {
                            int idx = result.ToString().IndexOf(".");
                            System.Console.WriteLine(result.ToString().Substring(0,idx+3));
                        }
                        else
                            System.Console.WriteLine(result + "0");
                    }
                    else
                        System.Console.WriteLine(result.ToString() + ".00");
                }
            }
            else
                System.Console.WriteLine("INVALID");
        }
    }
}