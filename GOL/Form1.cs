﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOL
{
    public partial class Form1 : Form
    {
        //HUD number and color to render in paint
        int number = 100;
        Color numColor = Color.Red;

        //Random Seed
        int seed = 100; //dummy number

        // The universe array
        bool[,] universe = new bool[30, 30];
        bool[,] scratchPad = new bool[30, 30];
        bool[,] temp = new bool[30, 30];

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.DarkSlateGray;

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
                    int countNbr = CountNeighborsToroidal(x, y); //returns neighbor count

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

            // Update status strip generations
            toolStripStatusLabelGenerations.Text = "Generations = " + generations.ToString();
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
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            RectangleF rect = new RectangleF(0, 0, 100, 100);
            int neighbors = 8;

            e.Graphics.DrawString(neighbors.ToString(), graphicsPanel1.Font, Brushes.Black, rect, stringFormat);

            #region Transparent text HUD          
            Brush numBrush = new SolidBrush(numColor);
            e.Graphics.DrawString(number.ToString(), graphicsPanel1.Font, numBrush, new Point(0, ClientRectangle.Height  - 175));
            #endregion

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

                    // Fill the cell with a brush if alive
                    if (universe[x,y] == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                    }

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                }
            }
            #region Thicker grid lines    
            Pen thickPen = new Pen(gridColor, 2); //2 is thicker
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    RectangleF gridRect = RectangleF.Empty;
                    gridRect.X = x * cellWidth * 10;
                    gridRect.Y = y * cellHeight * 10;
                    gridRect.Width = cellWidth * 10;
                    gridRect.Height = cellHeight * 10;

                    // Outline the cell with a pen
                    e.Graphics.DrawRectangle(thickPen, gridRect.X, gridRect.Y, gridRect.Width, gridRect.Height);
                }
            }
            #endregion
            // Cleaning up pens and brushes
            numBrush.Dispose();
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
                universe[(int)x , (int)y] = !universe[(int)x , (int)y];

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }
        #region Finite
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
        #endregion

        #region Toroidal
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
                    if (yCheck < 0) yCheck = yLen -1;           // if yCheck is less than 0, then set to yLen - 1
                    if (xCheck >= xLen) xCheck = 0;             // if xCheck is greater than or equal to xLen, then set to 0
                    if (yCheck >= yLen) yCheck = 0;             // if yCheck is greater than or equal to yLen, then set to 0

                    if (universe[xCheck, yCheck] == true) count++;
                }
            }
            return count;
        }
        #endregion

        #region Click events
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*this.*/Close(); //calling close method
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            #region basic click
            timer.Enabled = true;
            #endregion
            //if (timer.Enabled == true)
            //    timer.Enabled = false;
            //else
            //    timer.Enabled = true;
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
            graphicsPanel1.Invalidate(); //call for click events
        }
        #endregion

        private void toroidalToolStripMenuItem_Click(object sender, EventArgs e) //come back to this later
        {
        }

        #region Modal Dialog Boxes

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = numColor;

            if (DialogResult.OK == dlg.ShowDialog()) //checking if the action is cancelled by the user
            {
                numColor = dlg.Color;
                graphicsPanel1.Invalidate();//repaint
            }
        }
        #endregion

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModalOptions dlg = new ModalOptions(); //instantiate

            dlg.Number = number;

            if (DialogResult.OK == dlg.ShowDialog()) //checking if the action is cancelled by the user after already clicking //Dialog.OK says "thats the accept button"
            {
                number = dlg.Number;
                graphicsPanel1.Invalidate();
            }
        }

        private void fromSeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SeedDialog dlg = new SeedDialog();

            dlg.Seed = seed;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                seed = dlg.Seed;
                graphicsPanel1.Invalidate();
            }
        }

        #region Background color option
        // Create the color picker dialog box
        private void backColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();

            dlg.Color = graphicsPanel1.BackColor;
            if(DialogResult.OK == dlg.ShowDialog())
            {
                graphicsPanel1.BackColor = dlg.Color;
            }
        }

        // Saving the color when closing the application
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.BackColor = graphicsPanel1.BackColor;
            Properties.Settings.Default.Save();
        }
        #endregion

        #region Resetting settings
     
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset(); //only to cached data
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;  
        }
        #endregion
    }
}