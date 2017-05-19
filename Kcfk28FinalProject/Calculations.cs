using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kcfk28FinalProject
{
    class Calculations
    {
        int biasMagnitude; //Minimum = 4

        public Calculations(int biasMagnitude)
        {
            this.biasMagnitude = biasMagnitude;
        }

        //Function to remove walls between cells
        public void RemoveWalls(Cell a, Cell b)
        {
            //Figure out which direction the cells are to each other and remove the correct edge of each cell
            if (a.Row == b.Row)
            {
                if (a.Column < b.Column)
                {
                    a.RightWall = false;
                    b.LeftWall = false;
                }
                else
                {
                    a.LeftWall = false;
                    b.RightWall = false;
                }
            }
            else
            {
                if (a.Row < b.Row)
                {
                    a.BottomWall = false;
                    b.TopWall = false;
                }
                else
                {
                    a.TopWall = false;
                    b.BottomWall = false;
                }
            }
        }

        //Function to check for unvisited neighbors
        //Returns a list of cell and direction pairs
        //This is so RandomPick knows which direction each cell is relative to the current cell
        public List<Tuple<Cell, String>> CheckNeighbors(Cell _currentCell, List<Cell> unvisited)
        {
            List<Tuple<Cell, String>> _unvisitedNeighbors = new List<Tuple<Cell, String>>();

            //Cell above current cell
            Cell _neighbor = unvisited.Find(c => c.Row == _currentCell.Row - 1 && c.Column == _currentCell.Column);
            if (_neighbor != null)
            {
                _unvisitedNeighbors.Add(new Tuple<Cell, String>(_neighbor, "vertical"));
            }

            //Cell left of current cell
            _neighbor = unvisited.Find(c => c.Row == _currentCell.Row && c.Column == _currentCell.Column - 1);
            if (_neighbor != null)
            {
                _unvisitedNeighbors.Add(new Tuple<Cell, String>(_neighbor, "horizontal"));
            }

            //Cell below current cell
            _neighbor = unvisited.Find(c => c.Row == _currentCell.Row + 1 && c.Column == _currentCell.Column);
            if (_neighbor != null)
            {
                _unvisitedNeighbors.Add(new Tuple<Cell, String>(_neighbor, "vertical"));
            }

            //Cell right of current cell
            _neighbor = unvisited.Find(c => c.Row == _currentCell.Row && c.Column == _currentCell.Column + 1);
            if (_neighbor != null)
            {
                _unvisitedNeighbors.Add(new Tuple<Cell, String>(_neighbor, "horizontal"));
            }

            //If there's at least one unvisited cell, return the list
            if (_unvisitedNeighbors.Count != 0)
            {
                return _unvisitedNeighbors;
            }
            return null;
        }

        //Function to make a cell selection based on the given Bias
        //Implements Random to give the selected bias 75% chance of being selected
        public Cell RandomPick(List<Tuple<Cell, String>> list, String BIAS, Random r)
        {
            int i;

            //If there's only one option, take it
            if (list.Count == 1)
            {
                return list[0].Item1;
            }
            
            //Horizontal bias
            if (BIAS == "Horizontal")
            {
                //If there's two choices
                if (list.Count == 2)
                {
                    //If they are both vertical or both horizontal, randomly pick one
                    if (list[0].Item2 == list[1].Item2)
                    {
                        i = r.Next(2);
                        return list[i].Item1;
                    }
                    else
                    {
                        //Otherwise, get a random number between 0 and 3. If it's 3 go against
                        //the bias, otherwise go with the bias
                        i = r.Next(biasMagnitude);
                        if (i != 3)
                        {
                            return list.Find(x => x.Item2 == "horizontal").Item1;
                        }
                        return list.Find(x => x.Item2 == "vertical").Item1;
                    }
                }
                //Similar to the above selection, only adding a random pick between whichever
                //direction has two options
                if (list.Count == 3)
                {
                    if (list.FindAll(x => x.Item2 == "horizontal").Count == 2)
                    {
                        i = r.Next(biasMagnitude);
                        if (i != 3)
                        {
                            i = r.Next(2);
                            return list.FindAll(x => x.Item2 == "horizontal")[i].Item1;
                        }
                        return list.Find(x => x.Item2 == "vertical").Item1;
                    }
                    else
                    {
                        i = r.Next(biasMagnitude);
                        if (i != 3)
                        {
                            return list.Find(x => x.Item2 == "horizontal").Item1;
                        }
                        i = r.Next(2);
                        return list.FindAll(x => x.Item2 == "vertical")[i].Item1;
                    }
                }
            }
            //Same as Horizontal, just switched directions
            if (BIAS == "Vertical")
            {
                if (list.Count == 2)
                {
                    if (list[0].Item2 == list[1].Item2)
                    {
                        i = r.Next(2);
                        return list[i].Item1;
                    }
                    else
                    {
                        i = r.Next(biasMagnitude);
                        if (i != 3)
                        {
                            return list.Find(x => x.Item2 == "vertical").Item1;
                        }
                        return list.Find(x => x.Item2 == "horizontal").Item1;
                    }
                }
                if (list.Count == 3)
                {
                    if (list.FindAll(x => x.Item2 == "vertical").Count == 2)
                    {
                        i = r.Next(biasMagnitude);
                        if (i != 3)
                        {
                            i = r.Next(2);
                            return list.FindAll(x => x.Item2 == "vertical")[i].Item1;
                        }
                        return list.Find(x => x.Item2 == "horizontal").Item1;
                    }
                    else
                    {
                        i = r.Next(biasMagnitude);
                        if (i != 3)
                        {
                            return list.Find(x => x.Item2 == "vertical").Item1;
                        }
                        i = r.Next(2);
                        return list.FindAll(x => x.Item2 == "horizontal")[i].Item1;
                    }
                }
            }

            //If there is no bias, make a random selection
            i = r.Next(list.Count);
            return list[i].Item1;
        }

        //Calculate bias change
        public string BiasChange(Cell cell, int max)
        {
            //Y = X
            //Y = -X + max
            int fst = cell.Row;
            int snd = max - cell.Row;

            if (fst > cell.Column && cell.Column <= snd)
            {
                return "Vertical";
            }
            else if (fst <= cell.Column && cell.Column > snd)
            {
                return "Vertical";
            }
            return "Horizontal";

            //if (cell.Row < max / 2 && cell.Column <= max / 2)
            //{
            //    return "Vertical";
            //}
            //else if (cell.Row >= max / 2 && cell.Column > max / 2)
            //{
            //    return "Vertical";
            //}
            //return "Horizontal";
        }
    }
}
