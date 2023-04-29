using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class ConsoleStyle
    {
        public Point Position { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        public ConsoleDisplay Display { get; set; }

        public ConsoleStyle() {
            this.Position = null;
            this.ForegroundColor = ConsoleColor.White;
            this.BackgroundColor = Console.BackgroundColor;
            this.Display = ConsoleDisplay.Block;
        }

        public ConsoleStyle(ConsoleStyle style)
        {
            if (this.Position != null)
                this.Position = new Point(style.Position.X, style.Position.Y);
            else this.Position = null;
            this.ForegroundColor = style.ForegroundColor;
            this.BackgroundColor = style.BackgroundColor;
            this.Display = style.Display;
        }

        public virtual ConsoleStyle Clone()
        {
            return new ConsoleStyle(this);
        }
    }

    public class ConsoleInputStyle : ConsoleStyle
    {
        public string Placeholder { get; set; }
        public ConsoleColor PlaceholderForeground { get; set; }

        public ConsoleInputStyle() : base() {
            this.Placeholder = "";
            this.PlaceholderForeground = Console.ForegroundColor;
        }

        public ConsoleInputStyle(ConsoleInputStyle style) : base(style)
        {
            this.Placeholder = style.Placeholder;
            this.PlaceholderForeground = style.PlaceholderForeground;
        }

        public override ConsoleStyle Clone()
        {
            return new ConsoleInputStyle(this);
        }
    }

    public class ConsoleMenuStyle : ConsoleStyle
    {
        public char Mark { get; set; }
        public MarkType MarkType { get; set; }
        public ConsoleColor SelectedColor { get; set; }
        public ConsoleColor CaptionColor { get; set; }

        public ConsoleMenuStyle() : base()
        {
            this.Mark = ' ';
            this.MarkType = MarkType.Simple;
            this.SelectedColor = ConsoleColor.Green;
            this.CaptionColor = ConsoleColor.White;
        }

        public ConsoleMenuStyle(ConsoleMenuStyle style) : base(style)
        {
            this.Mark = style.Mark;
            this.MarkType = style.MarkType;
            this.SelectedColor = style.SelectedColor;
            this.CaptionColor = style.CaptionColor;
        }

        public override ConsoleStyle Clone()
        {
            return new ConsoleMenuStyle(this);
        }
    }

    public enum ConsoleDisplay
    {
        Block,
        Inline
    }

    public enum MarkType
    {
        Simple,
        Numeral
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
