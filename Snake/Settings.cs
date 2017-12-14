namespace Snake
{
    public enum Direction 
    {
        //กำหนด ขึ้น ลง ซ้าย ขวา
        Up,
        Down,
        Left,
        Right
    };

    public class Settings
    {
        //กำหนดตัวแปร
        public static int Width { get; set; }
        public static int Height { get; set; }
        public static int Speed { get; set; }
        public static int Score { get; set; }
        public static int Points { get; set; }
        public static bool GameOver { get; set; }
        public static Direction direction { get; set; }

        public Settings()
        {
            //กำหนดค่าต่างๆ มี ความใหญ่กว้าง + ความเร็วเริ่มต้น
            Width = 16;
            Height = 16;
            Speed = 12;
            Score = 0;
            Points = 100;
            GameOver = false;
            direction = Direction.Down;
        }
    }


}
