using System;
using System.Drawing;
using System.Windows.Forms;


namespace visual
{

    class CellWalls
    {
        public bool Top;
        public bool Bottom;
        public bool Left;
        public bool Right;
        public bool visted;
        public int? distance;
    }


    public partial class visual : Form
    {
        public visual()
        {
            InitializeComponent();

            maze = new CellWalls[width,height];
            Random rnd = new Random();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    maze[x, y] = new CellWalls { Top = true, Bottom = true, Left = true, Right = true,visted =false };
                }
            }

            var walkX = 5;
            var walkY = 5;
            int numVisited = 0;
            while (numVisited < width*height)
            {
                var randomDirection = rnd.Next() % 4;

                if (randomDirection == 0)
                {
                    //UP
                    if (walkY > 0)
                    {
                        var newY = walkY - 1;
                        if (!maze[walkX, newY].visted)
                        {
                            maze[walkX, newY].visted = true;
                            maze[walkX, newY].Bottom = false;
                            maze[walkX, walkY].Top = false;
                            numVisited += 1;
                        }
                        walkY = newY;
                    }
                }

                if (randomDirection == 1)
                {
                    //Right
                    if (walkX < width - 1)
                    {
                        var newX = walkX + 1;
                        if (!maze[newX, walkY].visted)
                        {
                            maze[newX, walkY].visted = true;
                            maze[newX, walkY].Left = false;
                            maze[walkX, walkY].Right = false;
                            numVisited += 1;
                        }
                        walkX = newX;
                    }
                }



                if (randomDirection == 2)
                {
                    //DOWN
                    if (walkY < height-1)
                    {
                        var newY = walkY + 1;
                        if (!maze[walkX, newY].visted)
                        {
                            maze[walkX, newY].visted = true;
                            maze[walkX, newY].Top = false;
                            maze[walkX, walkY].Bottom = false;
                            numVisited += 1;
                        }
                        walkY = newY;
                    }
                }

                if (randomDirection == 3)
                {
                    //Left
                    if (walkX > 0)
                    {
                        var newX = walkX - 1;
                        if (!maze[newX, walkY].visted)
                        {
                            maze[newX, walkY].visted = true;
                            maze[newX, walkY].Right = false;
                            maze[walkX, walkY].Left = false;
                            numVisited += 1;
                        }
                        walkX = newX;
                    }
                }

            }

            maze[width/2, 0].distance = 0;
            while (UpdateDistances()) { }

        }

        bool UpdateDistances()
        {
            bool modified = false;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var c = maze[x, y];
                    if(!c.distance.HasValue)
                    {
                        bool[] hasWall = { c.Left, c.Right, c.Top, c.Bottom };
                        int[] dx = { -1, 1, 0, 0 };
                        int[] dy = { 0, 0, -1, 1 };
                        int maxDist = -1;
                        for(int d = 0; d < 4; d++)
                        {
                            if (!hasWall[d])
                            {

                                var neighbourX = x + dx[d];
                                var neighbourY = y + dy[d];
                                //if (neighbourX < 0 || neighbourX >= width)
                                //    continue;
                                //if (neighbourY < 0 || neighbourY >= height)
                                //    continue;
                                var n = maze[neighbourX, neighbourY];

                                if (n.distance.HasValue)
                                {
                                    if (n.distance.Value > maxDist)
                                    {
                                        maxDist = n.distance.Value;
                                    }
                                }
                            }
                        }
                        if(maxDist > -1)
                        {
                            c.distance = maxDist + 1;
                            modified = true;
                        }
                    }
                }
            }
            return modified;
        }

        private CellWalls[,] maze;
        private const int width = 55;
        private const int height = 50;
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var c = maze[x, y];
                    if (c.Left)
                    {
                        g.DrawLine(Pens.Green, x * 10 + 10, y * 10 + 10, x * 10 + 10, y * 10 + 19);
                    }
                    if (c.Top)
                    {
                        g.DrawLine(Pens.Green, x * 10 + 10, y * 10 + 10, x * 10 + 19, y * 10 + 10);
                    }
                    if (c.Right)
                    {
                        g.DrawLine(Pens.Green, x * 10 + 20, y * 10 + 10, x * 10 + 20, y * 10 + 19);
                    }
                    if(c.Bottom)
                    {
                        g.DrawLine(Pens.Green, x * 10 + 10, y * 10 + 20, x * 10 + 19, y * 10 + 20);
                    }

                    Brush b = new SolidBrush(Color.FromArgb(255- Math.Min(c.distance.Value,255) , 0, Math.Min(c.distance.Value, 255)));
                    g.FillRectangle(b, x * 10 + 11, y * 10 + 11, 8, 8);
                }
            }
        }

        /// <summary>
        /// Draws a single line on g
        /// </summary>
        /// <param name="g"></param>
        private static void DrawStraightLine(Graphics g, int[] c)
        {
            var colour = Pens.Green;
            var startPoint = new Point(c[0], c[1]);
            var endPoint = new Point(c[2], c[3]);

            g.DrawLine(colour, startPoint, endPoint);
        }

        /// <summary>
        /// Draws a circle on g using just lines.
        /// </summary>
        /// <param name="g"></param>
        private void DrawCircleUsingLines(Graphics g)
        {
            var theta = 0;  // angle that will be increased each loop
            var h = this.Height / 2;      // x coordinate of circle center
            var k = this.Width / 2;      // y coordinate of circle center
            var step = 15;  // amount to add to theta each time (degrees)
            var r = 100;
            var last = new Point(0, 0);
            while (theta <= 360)
            {
                var x = h + r * Math.Cos(ConvertToRadians(theta));
                var y = k + r * Math.Sin(ConvertToRadians(theta));
                var newPoint = new Point((int)x, (int)y);
                theta += step;

                if (last.X != 0)
                    g.DrawLine(Pens.Black, last, newPoint);
                last = newPoint;
            }
        }

        /// <summary>
        /// Convert degrees to radians
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
    }
}
