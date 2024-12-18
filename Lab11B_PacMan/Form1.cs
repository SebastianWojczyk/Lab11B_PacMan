using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab11B_PacMan
{
    public partial class Form1 : Form
    {
        private enum FieldType { Empty, Wall, Dot }
        private FieldType[,] Board;
        private Point PacMan;
        private Keys Direction;
        private int Score = 0;

        private Timer timer;

        private Graphics graphics;
        private const int fieldSize = 40;

        public Form1()
        {
            InitializeComponent();

            ReadMap(Properties.Resources.Mapa);

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            pictureBoxBoard.SizeMode = PictureBoxSizeMode.AutoSize;

            pictureBoxBoard.Image = new Bitmap(Board.GetLength(1) * fieldSize,
                                               Board.GetLength(0) * fieldSize);
            graphics = Graphics.FromImage(pictureBoxBoard.Image);


            timer = new Timer();
            timer.Interval = 250;
            timer.Tick += Timer_Tick;
            timer.Start();

            DrawBoard();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Point newLocation = Point.Empty;
            switch (Direction)
            {
                case Keys.Right:
                    newLocation = new Point(PacMan.X + 1, PacMan.Y);
                    break;
                case Keys.Left:
                    newLocation = new Point(PacMan.X - 1, PacMan.Y);
                    break;
                case Keys.Down:
                    newLocation = new Point(PacMan.X, PacMan.Y + 1);
                    break;
                case Keys.Up:
                    newLocation = new Point(PacMan.X, PacMan.Y - 1);
                    break;
            }
            if (Board[newLocation.Y, newLocation.X] != FieldType.Wall)
            {
                if (Board[newLocation.Y, newLocation.X] == FieldType.Dot)
                {
                    Board[newLocation.Y, newLocation.X] = FieldType.Empty;
                    Score++;
                }
                PacMan = newLocation;
            }
            DrawBoard();
        }

        private void ReadMap(string map)
        {
            map = map.Replace("\r", "");
            string[] lines = map.Split('\n');
            Board = new FieldType[lines.Length, lines[0].Length];

            for (int row = 0; row < lines.Length; row++)
            {
                for (int col = 0; col < lines[row].Length; col++)
                {
                    switch (lines[row][col])
                    {
                        case 'P':
                            Board[row, col] = FieldType.Empty;
                            PacMan = new Point(col, row);
                            Direction = Keys.Right;
                            break;
                        case '#':
                            Board[row, col] = FieldType.Wall;
                            break;
                        case ' ':
                            Board[row, col] = FieldType.Dot;
                            break;
                        case 'G':
                            Board[row, col] = FieldType.Dot;
                            break;
                    }
                }
            }

        }

        private void DrawBoard()
        {
            graphics.Clear(Color.LightSalmon);
            for (int row = 0; row < Board.GetLength(0); row++)
            {
                for (int col = 0; col < Board.GetLength(1); col++)
                {
                    switch (Board[row, col])
                    {
                        case FieldType.Wall:
                            graphics.DrawImage(Properties.Resources.Wall,
                                               col * fieldSize,
                                               row * fieldSize);
                            break;
                        case FieldType.Dot:
                            graphics.DrawImage(Properties.Resources.Dot,
                                               col * fieldSize,
                                               row * fieldSize);
                            break;
                        case FieldType.Empty:
                            graphics.DrawImage(Properties.Resources.Empty,
                                               col * fieldSize,
                                               row * fieldSize);
                            break;

                    }
                }
            }
            graphics.DrawImage(Properties.Resources.PacMan,
                               PacMan.X * fieldSize,
                               PacMan.Y * fieldSize);

            graphics.DrawString(Score.ToString(),
                                new Font("Arial", fieldSize / 2),
                                new SolidBrush(Color.White),
                                0,
                                0);

            pictureBoxBoard.Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left ||
                e.KeyCode == Keys.Right ||
                e.KeyCode == Keys.Down ||
                e.KeyCode == Keys.Up)
            {
                Direction = e.KeyCode;
            }


        }
    }
}
