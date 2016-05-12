using System;
using System.Drawing;
using System.Windows.Forms;


namespace Circle
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            maze = new bool[width,height];
            Random rnd = new Random();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    maze[x, y] = (rnd.Next() % 2) == 0;
                }
            }

        }
        private bool[,] maze;
        private const int width = 50;
        private const int height = 50;
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (maze[x, y])
                    {
                        //horizontal
                        if (x < width - 1)
                            g.DrawLine(Pens.Green, x * 10 + 10, y * 10 + 10, x * 10 + 19, y * 10 + 10);
                    }
                    else
                    {
                        if(y<height-1)
                            g.DrawLine(Pens.Green, x * 10 + 10, y * 10 + 10, x * 10 + 10, y * 10 + 19);
                    }
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
