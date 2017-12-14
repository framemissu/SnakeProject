using System.Collections;
using System.Windows.Forms;

namespace Snake
{
    internal class Input
    {
        //หาปุ่ม key ที่ใช้ได้
        private static Hashtable keyTable = new Hashtable();

        //ดำเนินการตรวจสอบเพื่อดูว่ามีการกดปุ่มใดปุ่มหนึ่งหรือไม่
        public static bool KeyPressed(Keys key)
        {
            if (keyTable[key] == null)
            {
                return false;
            }

            return (bool) keyTable[key];
        }

        //ตรวจสอบว่ามีการกดปุ่มแป้นพิมพ์หรือไม่
        public static void ChangeState(Keys key, bool state)
        {
            keyTable[key] = state;
        }
    }
}
