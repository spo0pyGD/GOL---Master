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
            timer.Interval = 100; // milliseconds
            timer.Tick += Timer_Tick;
            timer.Enabled = false; // start timer running
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
                    int countNbr = CountNeighborsToroidal(x, y); //returns neighbor count

                    #region cleaner rules      
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
                    #endregion

                    #region commented rules             
                    //Rule A
                    //if (universe[x, y] == true && countNbr < 2)     //die if less than 2 neighbors  
                    //{
                    //    //universe[x, y] = false;
                    //    scratchPad[x, y] = false;
                    //}

                    ////Rule B
                    //if (universe[x, y] == true && countNbr > 3)     //die if more than 3 neighbors
                    //{
                    //    //universe[x, y] = false;
                    //    scratchPad[x, y] = false;
                    //}

                    ////Rule C
                    //if (universe[x, y] == true && countNbr == 2 || countNbr == 3)      //stay the same if 2-3 neighbors     
                    //{
                    //    //universe[x, y] = true;
                    //    scratchPad[x, y] = true;
                    //}

                    ////Rule D
                    //if (universe[x, y] == false && countNbr == 3) //cells reborn
                    //{
                    //    //universe[x, y] = true;
                    //    scratchPad[x, y] = true;
                    //}
                    #endregion
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
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

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
                    Rectangle cellRect = Rectangle.Empty;
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

            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }

        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                int cellWidth = graphicsPanel1.ClientSize.Width / universe.GetLength(0);
                int cellHeight = graphicsPanel1.ClientSize.Height / universe.GetLength(1);

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = e.X / cellWidth;
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = e.Y / cellHeight;

                // Toggle the cell's state
                universe[x , y] = !universe[x , y];

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
                    else if (xCheck < 0) continue;                   // if xCheck is less than 0, then continue
                    else if (yCheck < 0) continue;                   // if yCheck is less than 0, then continue
                    else if (xCheck >= xLen) continue;               // if xCheck is greater than or equal to xLen, then continue
                    else if (yCheck >= yLen) continue;               // if yCheck is greater than or equal to yLen, then continue

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
            graphicsPanel1.Invalidate(); //call for click events
        }  
        #endregion
    }
}