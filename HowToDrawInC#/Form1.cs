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
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            //1x1 grid
            int[][] onebyone = new int[4][] ;
            onebyone[0] = new int[4] { 0, 0, 0, 10 };
            onebyone[1] = new int[4] { 0, 0, 10, 0 };
            onebyone[2] = new int[4] { 10, 0, 10, 10 };
            onebyone[3] = new int[4] { 0, 10, 10, 10 };
            for (int i = 0; i < onebyone.Length; i++)
            {
                DrawStraightLine(g, onebyone[i]);
            }
            //2x2
            int[][] twobytwo = new int[16][];
            twobytwo[0] = new int[4] { 0, 0, 0, 10 };
            twobytwo[1] = new int[4] { 0, 0, 10, 0 };
            twobytwo[2] = new int[4] { 10, 0, 10, 10 };
            twobytwo[3] = new int[4] { 0, 10, 10, 10 };

            twobytwo[4] = new int[4] { 10, 0, 10, 10 };
            twobytwo[5] = new int[4] { 10, 0, 20, 0 };
            twobytwo[6] = new int[4] { 20, 0, 20, 10 };
            twobytwo[7] = new int[4] { 10, 10, 20, 10 };

            twobytwo[8] = new int[4] { 0, 10, 0, 20 };
            twobytwo[9] = new int[4] { 0, 10, 10, 10 };
            twobytwo[10] = new int[4] { 10, 10, 10, 20 };
            twobytwo[11] = new int[4] { 0, 20, 10, 20 };

            twobytwo[12] = new int[4] { 10, 10, 10, 20 };
            twobytwo[13] = new int[4] { 10, 10, 20, 10 };
            twobytwo[14] = new int[4] { 20, 10, 20, 20 };
            twobytwo[15] = new int[4] { 10, 20, 20, 20 };
            for (int i = 0; i < twobytwo.Length; i++)
            {
                DrawStraightLine(g, twobytwo[i]);
            }
            //3x3

            //DrawCircleUsingLines(g);
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
