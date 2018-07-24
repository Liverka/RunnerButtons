using System;
using System.Windows.Forms;

namespace RunnerButtons
{
    /// <summary>
    /// Представляет из себя обычную кнопку, но с функцией сравнения
    /// </summary>
    class ButtonCompare : Button, IComparable
    {
        public int CompareTo(object obj)
        {
            ButtonCompare temp = (ButtonCompare)obj;
            if (this.Location.X > temp.Location.X)
                return -1;
            if (this.Location.X < temp.Location.X)
                return 1;
            return 0;
        }
    }
}
