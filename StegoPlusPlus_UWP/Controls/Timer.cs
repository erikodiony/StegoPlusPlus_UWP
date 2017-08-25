using System;
using System.Diagnostics;
using static StegoPlusPlus.Controls.Process;

namespace StegoPlusPlus.Controls
{
    public class Timer
    {
        Stopwatch sw = new Stopwatch();
        public void Run(bool stat, string type)
        {
            if (stat == true && type == String.Empty)
            {
                sw.Start();
            }
            else
            {
                sw.Stop();
                ToastDialog.Notify(sw.ElapsedMilliseconds.ToString(), type);
            }
        }
    }
}
