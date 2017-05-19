using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kcfk28FinalProject
{
    class Cell : Border
    {

        Boolean rightWall = true;
        Boolean leftWall = true;
        Boolean topWall = true;
        Boolean bottomWall = true;
        Boolean visited = false;
        int row;
        int column;

        public Cell()
        {
            
        }

        public Cell(int row, int column)
        {
            this.row = row;
            this.column = column;
            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(1);
            Background = Brushes.WhiteSmoke;
        }

        //Keeps track of each wall and if removed, sets the Border thickness of that side to zero
        public Boolean RightWall
        {
            get
            {
                return rightWall;
            }
            set
            {
                rightWall = value;
                int x = 0;
                if(value == true)
                {
                    x = 1;
                }
                Thickness t = BorderThickness;
                t.Right = x;
                BorderThickness = t;
            }
        }
        public Boolean LeftWall
        {
            get
            {
                return leftWall;
            }
            set
            {
                leftWall = value;
                int x = 0;
                if (value == true)
                {
                    x = 1;
                }
                Thickness t = BorderThickness;
                t.Left = x;
                BorderThickness = t;
            }
        }

        public Boolean TopWall
        {
            get
            {
                return topWall;
            }
            set
            {
                topWall = value;
                int x = 0;
                if (value == true)
                {
                    x = 1;
                }
                Thickness t = BorderThickness;
                t.Top = x;
                BorderThickness = t;
            }
        }

        public Boolean BottomWall
        {
            get
            {
                return bottomWall;
            }
            set
            {
                bottomWall = value;
                int x = 0;
                if (value == true)
                {
                    x = 1;
                }
                Thickness t = BorderThickness;
                t.Bottom = x;
                BorderThickness = t;
            }
        }

        //Getters and setters for private properties
        public Boolean Visited
        {
            get
            {
                return visited;
            }
            set
            {
                visited = value;
            }
        }

        public int Row
        {
            get
            {
                return row;
            }
            set
            {
                row = value;
            }
        }

        public int Column
        {
            get
            {
                return column;
            }
            set
            {
                column = value;
            }
        }
    }
}
