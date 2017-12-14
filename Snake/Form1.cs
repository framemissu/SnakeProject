using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();

            //ตั้งค่าเริ่มต้น
            new Settings();

            //ตั้งค่าความเร็วของเกมและเริ่มจับเวลา
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();

            //เริ่มเกมใหม่
            StartGame();
        }

        private void StartGame()
        {
            lblGameOver.Visible = false;

            //ตั้งค่าเริ่มต้น
            new Settings();

            //สร้างอ็อบเจ็กต์ผู้เล่นใหม่
            Snake.Clear();
            Circle head = new Circle {X = 10, Y = 5};
            Snake.Add(head);


            lblScore.Text = Settings.Score.ToString();
            GenerateFood();

        }

        //วางอาหารแบบสุ่ม
        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();
            food = new Circle {X = random.Next(0, maxXPos), Y = random.Next(0, maxYPos)};
        }


        private void UpdateScreen(object sender, EventArgs e)
        {
            //ตรวจสอบเกม
            if (Settings.GameOver)
            {
                //เช็คว่ากด Enter มั้ย
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                //กด ขึ้น ลง ซ้าย ขวา
                if (Input.KeyPressed(Keys.Right) && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPressed(Keys.Left) && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPressed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPressed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;

                //ให้มันขยับเคลื่นที่
                MovePlayer();
            }

            pbCanvas.Invalidate();

        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver)
            {
                //กำหนดรูปร่างต่างๆ
                //ตั้งสีของงู
                //วาดงู
                for (int i = 0; i < Snake.Count; i++)
                {
                    Brush snakeColour;
                    if (i == 0)
                        snakeColour = Brushes.Black;     //วาดหัวงู
                    else
                        snakeColour = Brushes.Green;    //วาดลำตัวของงู

                    //วาดงู
                    canvas.FillEllipse(snakeColour,
                        new Rectangle(Snake[i].X * Settings.Width,
                                      Snake[i].Y * Settings.Height,
                                      Settings.Width, Settings.Height));


                    //กำหนดอาหารของงู
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Settings.Width,
                             food.Y * Settings.Height, Settings.Width, Settings.Height));

                }
            }
            else
            {
                //กำหนดหลังจากเกมจบให้ขึ้นคะแนนแล้วคำสั่งตามที่เรากำหนด
                string gameOver = "Game over \nYour final score is: " + Settings.Score + "\nPress Enter to try again!";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }


        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                //กำหนดให้มันขยับหัวงู
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        //ขยับไปทางขวาในแกน X
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        //ขยับไปทางซ้ายในแกน X
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        //ขยับไปทางขวาในแกน Y
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        //ขยับไปทางซ้ายในแกน Y
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }


                    //รับตำแหน่งสูงสุด X และ Y
                    int maxXPos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;

                    //ตรวจจับการชนกับเส้นขอบของเกม
                    if (Snake[i].X < 0 || Snake[i].Y < 0
                        || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos)
                    {
                        Die();
                    }


                    //ตรวจจับการชนกับร่างกาย
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X &&
                           Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    //ตรวจจับการชนกับชิ้นอาหาร
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }

                }
                else
                {
                    //ใหร่างกายขยับ
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void Eat()
        {
            //Add circle ใส่ร่างกาย
            Circle circle = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(circle);

            //ให้มัน Update Score
            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();

            GenerateFood();
        }

        private void Die()
        {
            Settings.GameOver = true;
        }
    }
}
