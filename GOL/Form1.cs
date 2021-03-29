using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOL
{
    public partial class Form1 : Form
    {
        //HUD Color
        Color hudColor = Color.Red;   

        //Fixed Seed
        int seed = 1011110;

        // The universe array
        bool[,] universe = new bool[30, 30];
        bool[,] scratchPad = new bool[30, 30];
        bool[,] temp = new bool[30, 30];

        int countNbr = 0;
        string boundaryType = " Toroidal ";

        // View menu bools
        bool isToroidal = true;
        bool isHUDVisible = true;
        bool viewNbr = true;

        // Drawing colors
        Color gridColor = Color.Indigo;
        Color cellColor = Color.Purple;

        // Grid bool
        bool isGridVisible = true;

        // The Timer class
        Timer timer = new Timer();

        // Generation count
        int generations = 0;

        public Form1()
        {
            InitializeComponent();

            // Setup the timer
            timer.Interval = 30; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running

            //Read Settings
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;  //namespace, Class, property, added settings
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {            
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    scratchPad[x, y] = false;

                    if (isToroidal)
                        countNbr = CountNeighborsToroidal(x, y);
                    else
                        countNbr = CountNeighborsFinite(x, y);
            
                    //apply the rules(determine whether current cell lives or dies)
                    if (universe[x, y] == true) //if cell is on
                    {
                        if (countNbr < 2) scratchPad[x, y] = false; //A
                        if (countNbr > 3) scratchPad[x, y] = false; //B
                        if (countNbr == 2 || countNbr == 3) scratchPad[x, y] = true; //C
                    }
                    else if (universe[x, y] == false) //if cell is off
                    {
                        if (countNbr == 3) scratchPad[x, y] = true; //D
                    }
                }
            }
            //copy everything from scratchPad to universe
            temp = universe;
            universe = scratchPad;
            scratchPad = temp;

            // Increment generation count
            generations++;

            // Update status strip
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
            toolStripStatusLabelLivingCells.Text = "Living Cells = " + CountLivingCells().ToString();
            toolStripStatusLabelTimerInterval.Text = "Timer Interval (ms) = " + timer.Interval.ToString();

            graphicsPanel1.Invalidate();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
            graphicsPanel1.Invalidate();
        }

        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / (float)universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / (float)universe.GetLength(1);

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);

            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    RectangleF cellRect = RectangleF.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    #region Display neighbor count in cell
                    // So I can use both universe types
                    if (isToroidal) countNbr = CountNeighborsToroidal(x, y);
                    else countNbr = CountNeighborsFinite(x, y);

                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    #endregion

                    // Fill the cell with a brush if alive
                    if (universe[x, y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);

                    // Drawing neighbor count
                    if (viewNbr)
                    {
                        if (countNbr != 0) //leaving out 0 because clutter
                        {
                            if (universe[x,y] == true && countNbr == 2 || countNbr == 3)                                                      //If cell will live
                                e.Graphics.DrawString(countNbr.ToString(), graphicsPanel1.Font, Brushes.LightGreen, cellRect, stringFormat);  //Font is green
                            else                                                                                                              //Otherwise,
                                e.Graphics.DrawString(countNbr.ToString(), graphicsPanel1.Font, Brushes.Red, cellRect, stringFormat);         //Font is red
                        }
                    }
                }
            }

            #region Thicker grid lines    
            Pen thickPen = new Pen(gridColor, 2); //2 is thicker
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    RectangleF gridRect = RectangleF.Empty;     //Every 10 cells
                    gridRect.X = x * cellWidth * 10;
                    gridRect.Y = y * cellHeight * 10;
                    gridRect.Width = cellWidth * 10;
                    gridRect.Height = cellHeight * 10;

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(thickPen, gridRect.X, gridRect.Y, gridRect.Width, gridRect.Height);
                }
            }
            #endregion

            #region HUD           
            if (isHUDVisible)
            {
                //draw HUD
                Brush hudBrush = new SolidBrush(hudColor);
                Font hudFont = new Font("Arial", 14f);

                string hudGen = "Generations = " + generations.ToString();
                string hudLivingCells = "Living Cells = " + CountLivingCells().ToString();
                string hudTimer = "Timer Interval (ms) = " + timer.Interval.ToString();
                string hudBoundary = "Boundary Type = " + boundaryType.ToString();

                e.Graphics.DrawString(hudGen.ToString(), hudFont, hudBrush, new Point(0, ClientRectangle.Height - 100));
                e.Graphics.DrawString(hudLivingCells.ToString(), hudFont, hudBrush, new Point(0, ClientRectangle.Height - 125));
                e.Graphics.DrawString(hudTimer.ToString(), hudFont, hudBrush, new Point(0, ClientRectangle.Height - 150));
                e.Graphics.DrawString(hudBoundary.ToString(), hudFont, hudBrush, new Point(0, ClientRectangle.Height - 175));
            }
            #endregion

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
            thickPen.Dispose();          
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                float cellWidth = (float)graphicsPanel1.ClientSize.Width / (float)universe.GetLength(0);
                float cellHeight = (float)graphicsPanel1.ClientSize.Height / (float)universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                float x = (float)e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                float y = (float)e.Y / cellHeight;

                // Toggle the cell's state
                universe[(int)x, (int)y] = !universe[(int)x, (int)y];

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        private int CountLivingCells()
        {
            int count = 0;
            for (int y = 0; y < universe.GetLength(1); y++)     //Iterate y
            {
                for (int x = 0; x < universe.GetLength(0); x++) //Iterate x
                    if (universe[x, y] == true) count++;
            }
            return count;
        }

        private void ResizeUniverse(int newWidth, int newHeight)
        {
            universe = new bool[newWidth, newHeight]; //just call new
            scratchPad = new bool[newWidth, newHeight];

            graphicsPanel1.Invalidate();
        }

        #region Count Neighbors

        private int CountNeighborsFinite(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;

                    if (xOffset == 0 && yOffset == 0) continue; // if xOffset and yOffset are both equal to 0, then continue
                    if (xCheck < 0) continue;                   // if xCheck is less than 0, then continue
                    if (yCheck < 0) continue;                   // if yCheck is less than 0, then continue
                    if (xCheck >= xLen) continue;               // if xCheck is greater than or equal to xLen, then continue
                    if (yCheck >= yLen) continue;               // if yCheck is greater than or equal to yLen, then continue

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }

        private int CountNeighborsToroidal(int x, int y)
        {
            int count = 0;
            int xLen = universe.GetLength(0);
            int yLen = universe.GetLength(1);
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;

                    if (xOffset == 0 && yOffset == 0) continue; // if xOffset and yOffset are both equal to 0, then continue                          
                    if (xCheck < 0) xCheck = xLen - 1;          // if xCheck is less than 0, then set to xLen - 1
                    if (yCheck < 0) yCheck = yLen - 1;          // if yCheck is less than 0, then set to yLen - 1
                    if (xCheck >= xLen) xCheck = 0;             // if xCheck is greater than or equal to xLen, then set to 0
                    if (yCheck >= yLen) yCheck = 0;             // if yCheck is greater than or equal to yLen, then set to 0

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }
        #endregion

        #region Basic click events
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*this.*/Close(); //calling close method
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = true; 
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
        }

        private void nextToolStripMenuItem_Click(object sender, EventArgs e) //call nextgen once
        {
            NextGeneration();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) //File New
        {
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    universe[x, y] = false; //setting universe back to default (empty)
                }
            }
            for (int y = 0; y < scratchPad.GetLength(1); y++)   // clearing scratchPad too
            {
                for (int x = 0; x < scratchPad.GetLength(0); x++)
                {
                    scratchPad[x, y] = false;
                }
            }
            generations = 0;
            isToroidal = true;

            graphicsPanel1.Invalidate(); //call for click events
        }
        #endregion

        #region Options DLG

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e) //opens Modal Dialog box
        {
            ModalOptions dlg = new ModalOptions(); //instantiate

            //default values for numeric up/down
            dlg.UniverseHeight = 30;
            dlg.UniverseWidth = 30;

            if (DialogResult.OK == dlg.ShowDialog()) //checking if the action is cancelled by the user
            {
                ResizeUniverse(dlg.UniverseWidth, dlg.UniverseHeight);              
            }
            graphicsPanel1.Invalidate();
        }

        private void speedToolStripMenuItem_Click(object sender, EventArgs e) //Timer interval dialog box
        {
            TimerDLG dlg = new TimerDLG();
            dlg.TimerInterval = timer.Interval; //sets to same value as current interval

            if (DialogResult.OK == dlg.ShowDialog()) 
            {
                timer.Interval = dlg.TimerInterval; //updates interval with user input        
            }
            graphicsPanel1.Invalidate();
        }

        #endregion

        #region Color DLG

        //Background color
        private void backColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog(); //Create the color picker dialog box

            dlg.Color = graphicsPanel1.BackColor;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;
            }
            graphicsPanel1.Invalidate();
        }

        //Cell color
        private void cellColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = graphicsPanel1.BackColor;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                cellColor = dlg.Color;
            }
            graphicsPanel1.Invalidate();
        }

        //Grid Color
        private void gridColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = graphicsPanel1.BackColor;
            if (DialogResult.OK == dlg.ShowDialog())
            {
                gridColor = dlg.Color;
            }
            graphicsPanel1.Invalidate();
        }

        // Saving the color when closing the application
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.BackColor = graphicsPanel1.BackColor;
            Properties.Settings.Default.CellColor = cellColor;
            Properties.Settings.Default.GridColor = gridColor;
            Properties.Settings.Default.Save();
        }
        #endregion

        #region Settings

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset(); //only to cached data
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;
            timer.Interval = Properties.Settings.Default.Timer;
            isToroidal = Properties.Settings.Default.DefaultBoundary;
            generations = Properties.Settings.Default.Generations;
            countNbr = Properties.Settings.Default.CountNeighbor;
            universe = new bool[Properties.Settings.Default.UniverseSize, Properties.Settings.Default.UniverseSize];
            scratchPad = new bool[Properties.Settings.Default.UniverseSize, Properties.Settings.Default.UniverseSize]; //forgetting the scratchPad made it crash

            graphicsPanel1.Invalidate();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;
            timer.Interval = Properties.Settings.Default.Timer;
            isToroidal = Properties.Settings.Default.DefaultBoundary;
            generations = Properties.Settings.Default.Generations;
            countNbr = Properties.Settings.Default.CountNeighbor;
            universe = new bool[Properties.Settings.Default.UniverseSize, Properties.Settings.Default.UniverseSize];
            scratchPad = new bool[Properties.Settings.Default.UniverseSize, Properties.Settings.Default.UniverseSize];

            graphicsPanel1.Invalidate();
        }
        #endregion      

        #region Run to Generation
        private void toToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Run_To_Dialog dlg = new Run_To_Dialog();         
            dlg.PickGeneration = generations;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                timer.Enabled = true;
                if (generations == dlg.PickGeneration) timer.Enabled = false;
            }
            graphicsPanel1.Invalidate();
        }
        #endregion

        #region Save/Load

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";


            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                // Write any comments you want to include first.
                // Prefix all comment strings with an exclamation point.
                // Use WriteLine to write the strings to the file. 
                // It appends a CRLF for you.
                writer.WriteLine("!Cell pattern.");

                // Iterate through the universe one row at a time.
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Create a string to represent the current row.
                    String currentRow = string.Empty;

                    // Iterate through the current row one cell at a time. (can use stringbuilder)
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        // If the universe[x,y] is alive then append 'O' (capital O) to the row string.
                        if (universe[x, y] == true) currentRow += 'O';

                        // Else if the universe[x,y] is dead then append '.' (period) to the row string.
                        else if (universe[x, y] == false) currentRow += '.';
                    }

                    // Once the current row has been read through and the 
                    // string constructed then write it to the file using WriteLine.
                    writer.WriteLine(currentRow);
                }
                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;

                int yPos = 0; //to make up for no yPos for-loop

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {                   
                    string row = reader.ReadLine();                      // Read one row at a time.

                    if (row[0] == '!') continue;                         // If the row begins with '!' then it is a comment and should be ignored. (continue)
                    if (row[0] != '!') maxHeight++;                    // If the row is not a comment then it is a row of cells. Increment the maxHeight variable for each row read.
                    
                    maxWidth = row.Length;                               // Get the length of the current row string and adjust the maxWidth variable if necessary.             
                }

                // Resize the current universe and scratchPad (call new) to the width and height of the file calculated above.
                universe = new bool [maxWidth , maxHeight];
                scratchPad = new bool [maxWidth, maxHeight];

                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);

                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {                   
                    string row = reader.ReadLine();                                 // Read one row at a time.

                    if (row[0] == '!') continue;                                    // If the row begins with '!' then it is a comment and should be ignored. (continue)        //changed from row.Substring(0,row.length)              
                    if (row[0] != '!')                                              // If the row is not a comment then it is a row of cells and needs to be iterated through.
                    {
                        for (int xPos = 0; xPos < row.Length; xPos++) // iterate
                        {
                           if (row[xPos] == 'O') universe[xPos, yPos] = true;       // If row[xPos] is a 'O' (capital O) then set the corresponding cell in the universe to alive.
                           if (row[xPos] == '.') universe[xPos, yPos] = false;      // If row[xPos] is a '.' (period) then set the corresponding cell in the universe to dead.
                        }
                        yPos++;
                    }                
                }              
                reader.Close(); // Close the file.
                graphicsPanel1.Invalidate();
            }
        }
        #endregion

        #region Random universe

        #region Methods for seed and time

        private void InitialRandomUniverse()
        {
            //from time
            Random tRand = new Random(); //automatically

            // Iterate y
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate x
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    int randNeighbors = tRand.Next(0, 2);

                    if (randNeighbors == 0) universe[x, y] = true;
                    else universe[x, y] = false;
                }
            }
            graphicsPanel1.Invalidate();
        }

        private void FixedSeedRandomUniverse()
        {
            //from seed
            Random sRand = new Random(seed); //from seed

            // Iterate y
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate x
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    int randNeighbors = sRand.Next(0, 2);

                    if (randNeighbors == 0) universe[x, y] = true;
                    else universe[x, y] = false;
                }
            }
            graphicsPanel1.Invalidate();
        }
        #endregion

        //From Seed (user choice)
        private void fromSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SeedDialog dlg = new SeedDialog();

            dlg.Seed = seed;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                seed = dlg.Seed;
                FixedSeedRandomUniverse();
                CountLivingCells();
                graphicsPanel1.Invalidate();
            }
        }     

        //Current seed
        private void fromCurrentSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FixedSeedRandomUniverse();
            CountLivingCells();
            graphicsPanel1.Invalidate();
        } 
        
        //Time
        private void fromTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitialRandomUniverse();
            CountLivingCells();
            graphicsPanel1.Invalidate();
        }
        #endregion

        #region View Menu

        #region Switching between toroidal and finite

        private void toroidalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isToroidal = true;
            boundaryType = " Toroidal ";
            graphicsPanel1.Invalidate();
        }

        private void finiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isToroidal = false;
            boundaryType = " Finite ";
            graphicsPanel1.Invalidate();
        }
        #endregion

        //View grid
        //I want to figure out a way to do this by turning off/clearing the grid pen instead
        private void gridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isGridVisible)                        //if grid is on
            {
                isGridVisible = false;                //user click will turn the grid off
                gridColor = graphicsPanel1.BackColor; //set color to background color
            }
            else if (!isGridVisible)                  //otherwise, if grid is off 
            {
                isGridVisible = true;                 //user turns grid on
                gridColor = Color.Black;              //set color to black
            }
            graphicsPanel1.Invalidate();
        }

        //View neighbor count
        private void neighborCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (viewNbr) viewNbr = false;
            else if (!viewNbr) viewNbr = true;
            graphicsPanel1.Invalidate();
        }

        //View HUD
        private void hUDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isHUDVisible) isHUDVisible = false;
            else if (!isHUDVisible) isHUDVisible = true;
            graphicsPanel1.Invalidate();
        }
        #endregion
    }
}