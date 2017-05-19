using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kcfk28FinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int MAXROWLENGTH;
        int SLEEPTIME;
        Random r = new Random();
        List<Cell> unvisited;
        Stack<Cell> stack;
        Cell currentCell;
        List<Tuple<Cell, String>> unvisitedNeighbors = null;
        Cell chosenCell;
        Thread mazeThread = null;
        String BIAS = "";
        Calculations calc;
        Boolean changingBias;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void goButton_Click(object sender, RoutedEventArgs e)
        {
            //Remove any error message
            messageBlock.Text = "";

            //Check for invalid row length
            bool result = Int32.TryParse(numOfCellsBox.Text, out MAXROWLENGTH);
            if (!result || MAXROWLENGTH <= 2 || MAXROWLENGTH > 100)
            {
                messageBlock.Text = "Invalid input in Cell Input Box. Enter a number between 3 and 100.";
                numOfCellsBox.Text = "";
                return;
            }
            
            //Check for invalid speed 
            int speed;
            result = Int32.TryParse(speedBox.Text, out speed);
            if (!result || speed < 1 || speed > 100)
            {
                messageBlock.Text = "Invalid input in Speed Input Box. Enter a number between 1 and 100.";
                speedBox.Text = "";
                return;
            }

            //Check for bias magnitude
            int biasMag;
            result = Int32.TryParse(biasMagBox.Text, out biasMag);
            if (biasMagBox.Text == "")
            {
                biasMag = 1;
            }
            else if (!result || biasMag < 1 || biasMag > 10)
            {
                messageBlock.Text = "Invalid input for Bias Magnitude Box. Enter a number between 1 and 10.";
                biasMagBox.Text = "";
                return;
            }
            

            //Get bias selection. If no selection, it won't use a bias
            BIAS = biasComboBox.Text;
            if (BIAS == "Square")
            {
                changingBias = true;
            }
            else
            {
                changingBias = false;
            }
            
            //if (mazeThread != null)
            //{
            //    cancelButton.Content = "Cancel";
            //    mazeThread.Resume();
            //    mazeThread.Abort();
            //    mazeThread = null;
            //}

            //Calculate thread sleeptime, prep variables
            SLEEPTIME = 210 - (speed * 2);
            goButton.IsEnabled = false;
            abortButton.IsEnabled = true;

            uGrid.Children.Clear();
            unvisited = new List<Cell>();
            stack = new Stack<Cell>();
            currentCell = null;
            chosenCell = null;
            calc = new Calculations((biasMag + 1) * 2);

            //Generate cells, add to unvisited list and uGrid 
            for (int i = 0; i < MAXROWLENGTH; i++)
            {
                for (int j = 0; j < MAXROWLENGTH; j++)
                {
                    Cell c = new Cell(i, j);
                    c.Name = "row" + i.ToString() + "column" + j.ToString();
                    uGrid.Children.Add(c);
                    unvisited.Add(c);
                }
            }

            //Create entrance and beginning point for generation
            currentCell = unvisited.Find(x => x.Name == "row0column1");
            currentCell.Background = Brushes.CornflowerBlue;
            currentCell.Visited = true;
            currentCell.TopWall = false;
            unvisited.Remove(currentCell);

            //Create exit
            unvisited.Find(x => x.Row == MAXROWLENGTH - 1 && x.Column == MAXROWLENGTH - 2).BottomWall = false;

            //Start the maze generation thread
            mazeThread = new Thread(GenerateMaze);
            mazeThread.Start();
        }

        void GenerateMaze()
        {
            //While there are cells that aren't part of the maze
            while (unvisited.Count != 0)
            {
                //Check for which neighbors are unvisited
                unvisitedNeighbors = calc.CheckNeighbors(currentCell, unvisited);
                //If there are unvisited neighbors
                if (unvisitedNeighbors != null)
                {
                    //Randomly choose one
                    if(changingBias == true)
                    {
                        BIAS = calc.BiasChange(currentCell, MAXROWLENGTH);
                    }
                    chosenCell = calc.RandomPick(unvisitedNeighbors, BIAS, r);

                    //Push current cell onto the stack
                    stack.Push(currentCell);

                    //Remove the wall between the current and chosen cell
                    Dispatcher.Invoke(new Action(() => { calc.RemoveWalls(currentCell, chosenCell); }));
                    Dispatcher.Invoke(new Action(() => { currentCell.Background = Brushes.WhiteSmoke; }));

                    //Make the chosen cell the new current cell, and mark it as visited
                    currentCell = chosenCell;
                    Dispatcher.Invoke(new Action(() => { currentCell.Background = Brushes.CornflowerBlue; }));
                    currentCell.Visited = true;
                    unvisited.Remove(currentCell);
                }
                //Otherwise, if there are cells on the stack, pop one into the currentCell to backtrack
                else if (stack.Count != 0)
                {
                    Dispatcher.Invoke(new Action(() => { currentCell.Background = Brushes.WhiteSmoke; }));
                    currentCell = stack.Pop();
                    Dispatcher.Invoke(new Action(() => { currentCell.Background = Brushes.CornflowerBlue; }));
                }
                //Advance at the specified speed
                Thread.Sleep(SLEEPTIME);
            }
            //When the maze is generated, fix buttons
            Dispatcher.Invoke(new Action(() => { goButton.IsEnabled = true; abortButton.IsEnabled = false; currentCell.Background = Brushes.WhiteSmoke; }));
        }
        
        //Abort button, formerly cancel button but the pausing and resuming of
        //Threads cause Visual Studio to sieze up and prevent new builds
        private void abortButtonClick(object sender, RoutedEventArgs e)
        {
            mazeThread.Abort();
            goButton.IsEnabled = true;
            abortButton.IsEnabled = false;
            //if(cancelButton.Content.ToString() == "Cancel")
            //{
            //    //Suspend the thread and allow users to restart generation or resume
            //    mazeThread.Suspend();
            //    goButton.IsEnabled = true;
            //    cancelButton.Content = "Resume";
            //}
            //else
            //{
            //    //If the button says resume, Resume the thread
            //    mazeThread.Resume();
            //    goButton.IsEnabled = false;
            //    cancelButton.Content = "Cancel";
            //}
        }

        private void aboutButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This Application generates random mazes. It was developed by Kevin Free " +
                "for IT 4400 at the University of Missouri.\n\nThe algorithm employed is a modified " +
                "depth first search algorithm, using a list of all cells and a stack to create the maze. " +
                "For more information, view the source code.\n\nTo generate a maze, type a value for " +
                "number of rows between 3 and 100 and a value for speed % between 1 and 100. " +
                "Speed % affects how quickly the maze generates. Select " +
                "a passage bias* from the drop down box. Then hit Go! If you abort a maze generation, " +
                "you can hit Go! again to start a new one, but you cannot resume the maze it was making. " +
                "The entrance is at the top left and exit at the bottom right of the maze. " +
                "Enjoy!\n\n*Passage bias affects the algorithm's choices by making the biased direction " +
                "more likely to be chosen for the next cell. Optional input for the bias magnitude field " +
                "must be an integer from 1 to 10. This will increase the likelihood of that path being " +
                "chosen when the algorithm makes decisions from 75% to 95% likelihood.");
        }
    }
}
