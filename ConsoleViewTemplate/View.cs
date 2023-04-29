using ConsoleUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//●○

namespace ConsoleUI
{
    public static class View
    {

        static View()
        {
            Console.OutputEncoding = Encoding.Unicode;
        }

        #region Elements
        public static void Label(string text, ConsoleStyle style = null)
        {
            ConsoleStyle _style;
            if (style == null) _style = new ConsoleStyle();
            else _style = style.Clone();
            PrepareStyle(_style);

            text = FormatText(text, _style);
            Console.Write(text);
        }

        public static string Input(ConsoleInputStyle style = null)
        {
            ConsoleInputStyle _style;
            if (style == null) _style = new ConsoleInputStyle();
            else _style = style.Clone() as ConsoleInputStyle;
            PrepareStyle(_style);

            string text = "";
            
            while (true)
            {
                var ch = Console.ReadKey();
                
                switch (ch.Key)
                {
                    case ConsoleKey.Backspace:
                        if (text.Length == 0)
                        {
                            Console.CursorLeft++;
                            break;
                        }
                        text = new string(text.ToCharArray(), 0, text.Length - 1);
                        Console.Write(' ');
                        Console.CursorLeft--;
                        if (text.Length == 0) ShowPlaceholder(_style);
                        break;
                    case ConsoleKey.Enter:
                        Console.WriteLine();
                        return text;
                    default:
                        if (ch.KeyChar == '\0')
                        {
                            Console.CursorLeft--;
                            if (_style.Placeholder.Length != 0 && text.Length == 0)
                            {
                                ShowPlaceholder(_style);
                            }

                            break;
                        }
                        if (text.Length == 0)
                        {
                            HidePlaceholder(_style);
                        }
                        text += ch.KeyChar;
                        break;
                }
            }
        }

        public static int Menu(string[] options, string caption = null, ConsoleMenuStyle style = null)
        {
            ConsoleMenuStyle _style;
            if (style == null) _style = new ConsoleMenuStyle();
            else _style = style.Clone() as ConsoleMenuStyle;
            PrepareStyle(_style);

            int index = 0;
            while (true)
            {
                Console.SetCursorPosition(_style.Position.X, _style.Position.Y);
                Console.ForegroundColor = _style.CaptionColor;
                Console.WriteLine(caption);
                for(int i = 0; i < options.Length; i++)
                {
                    if (i == index)
                        Console.ForegroundColor = _style.SelectedColor;
                    Console.CursorLeft = _style.Position.X;
                    if(_style.MarkType == MarkType.Simple)
                        Console.Write(_style.Mark + " ");
                    else Console.Write((char)(_style.Mark + i) + " ");
                    Console.WriteLine(options[i]);
                    Console.ForegroundColor = _style.ForegroundColor;
                }
                var ch = Console.ReadKey();
                switch (ch.Key)
                {
                    case ConsoleKey.UpArrow:
                        index = Math.Max(0, index - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        index = Math.Min(index + 1, options.Length - 1);
                        break;
                    case ConsoleKey.Enter:
                        return index;
                }
            }
        }

        public static bool Checker(string text, ConsoleStyle style = null, bool active = false)
        {
            ConsoleStyle _style;
            if (style == null) _style = new ConsoleStyle();
            else _style = style.Clone();
            PrepareStyle(_style);

            text = FormatText(text, _style);
            bool isActive = active;
            while (true)
            {
                Console.SetCursorPosition(_style.Position.X, _style.Position.Y);
                Console.Write((isActive ? "●" : "○") + ' ');
                Console.Write(text);
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Spacebar:
                        isActive = !isActive;
                        break;
                    case ConsoleKey.Enter:
                        return isActive;
                }
            }
        }
        #endregion

        private static void HidePlaceholder(ConsoleInputStyle style)
        {
            Console.Write(new string(' ', style.Placeholder.Length));
            Console.SetCursorPosition(style.Position.X + 1, style.Position.Y);
        }

        private static void ShowPlaceholder(ConsoleInputStyle style)
        {
            Console.ForegroundColor = style.PlaceholderForeground;
            Console.Write(style.Placeholder);
            Console.SetCursorPosition(style.Position.X, style.Position.Y);
            Console.ForegroundColor = style.ForegroundColor;
        }

        private static void PrepareStyle(ConsoleStyle style)
        {
            Console.CursorVisible = false;

            if(style.Position == null)
                style.Position = new Point(Console.CursorLeft, Console.CursorTop);

            if (style is ConsoleInputStyle)
            {
                Console.CursorVisible = true;
                ConsoleInputStyle inputStyle = (ConsoleInputStyle)style;
                ShowPlaceholder(inputStyle);
            }

            Console.SetCursorPosition(style.Position.X, style.Position.Y);
            Console.ForegroundColor = style.ForegroundColor;
            Console.BackgroundColor = style.BackgroundColor;
        }

        private static string FormatText(string text, ConsoleStyle style)
        {
            if (style == null) style = new ConsoleStyle();
            if (style.Display == ConsoleDisplay.Block)
                text += "\n";

            return text;
        }
    }
}
