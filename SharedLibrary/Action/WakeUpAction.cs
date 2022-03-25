using SharedLibrary.Module.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Action
{
    public class WakeUpAction
    {
        private enum Time
        {
            Morning = 8,
            SNoon = 12,
            ENoon = 14,
            Afternoon = 18
        };

        public static void SayHello()
        {
           
            SendGroupMessage.PostMessageAsync("333424728", TimePickWords());
        }

        private static string TimePickWords()
        {
            DateTime dt = DateTime.Now;
            if (dt.Hour < (int)Time.Morning)
            {
                return "主人，起这么早啊！";
            }
            else if(dt.Hour >= (int)Time.Morning && dt.Hour < (int)Time.SNoon)
            {
                return "主人，早上好！";
            }
            else if (dt.Hour >= (int)Time.SNoon && dt.Hour < (int)Time.ENoon)
            {
                return "主人，中午了哦，有在好好吃午饭吗？";
            }
            else if (dt.Hour >= (int)Time.ENoon && dt.Hour < (int)Time.Afternoon)
            {
                return "主人，下午好呀！";
            }
            else if (dt.Hour >= (int)Time.Afternoon)
            {
                return "主人，天快黑了，记得不要熬夜哦！";
            }
            else
            {
                return "主人你好呀";
            }
        }
    }
}
