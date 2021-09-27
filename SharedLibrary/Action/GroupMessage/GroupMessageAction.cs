using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Utils.Scaffolds;
using SharedLibrary.Dictionary;
using SharedLibrary.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Action.GroupMessage
{
    public class GroupMessageAction
    {
        public static bool GroupCommandParse(GroupMessageReceiver receiver)
        {
            TimeCounterHelper tcc = new TimeCounterHelper();
            tcc.Start();
            bool isParseTrue = false;
            //Console.WriteLine(receiver.MessageChain.First());

            //receiver.MessageChain.Where(x => x is PlainMessage).Cast<PlainMessage>().ToArray()
            //等效于eceiver.MessageChain.WhereAndCast<PlainMessage>()

            //Image消息
            var imageMsg = receiver.MessageChain.WhereAndCast<ImageMessage>();
            //Plain消息
            var plainMsg = receiver.MessageChain.WhereAndCast<PlainMessage>();

            foreach (var message in plainMsg)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"[{tcc.Span().TotalMilliseconds.ToString("0.00")}ms][{message.Type}]: ");
                Console.WriteLine($"{message.Text}");
                Console.ResetColor();
                plainMsgParse(message);
                isParseTrue = true;
            }

            if (imageMsg != null && plainMsg.Length<=0)
            {
                foreach (var image in imageMsg)
                {
                    tcc.Over();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"{tcc.Span().TotalMilliseconds.ToString("0.00")}ms][{image.Type}]: ");
                    Console.WriteLine($"{image.Url}");
                    Console.ResetColor();
                }
                isParseTrue = true;
            }
            else
            {
                return false;
            }

            return isParseTrue;
        }

        private static void plainMsgParse(PlainMessage message)
        {
            commandSplit(message.Text);
            Console.WriteLine(GroupMsg.DGroupMsg["猜数字"]);
        }

        private static List<string> commandSplit(string command)
        {
            List<string> cList= new();
            var list = command.Split(" ").ToList();
            if(list.Count>0 && list != null)
            {
                if (list.Count >= 1)
                {

                }
                else
                {
                    cList.Add(list[0]);
                }
            }
            else
            {
                return null;
            }
           
            return cList;
        }
    }
}
