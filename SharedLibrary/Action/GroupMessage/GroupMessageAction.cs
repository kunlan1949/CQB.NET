﻿using Db.Bot;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Utils.Scaffolds;
using SharedLibrary.Action.GroupMessage.Func;
using SharedLibrary.Action.GroupMessage.Game;
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
        public static bool GroupCommandParse(Members mem, Groups group, GroupMessageReceiver receiver)
        {
            TimeCounterHelper tcc = new();
            tcc.Start();
            bool isParseTrue;
            //Console.WriteLine(receiver.MessageChain.First());

            //receiver.MessageChain.Where(x => x is PlainMessage).Cast<PlainMessage>().ToArray()
            //等效于receiver.MessageChain.WhereAndCast<PlainMessage>()

            //Image消息
            var imageMsg = receiver.MessageChain.OfType<ImageMessage>();
            //Plain消息
            var plainMsg = receiver.MessageChain.OfType<PlainMessage>();

            foreach (var message in plainMsg)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"[{tcc.Span().TotalMilliseconds*100:0}ms][{message.Type}]: ");
                Console.ResetColor();
                Console.WriteLine($"{message.Text}");
                PlainMsgParse(mem, group, message, receiver);
                isParseTrue = true;
            }

            if (imageMsg != null && !plainMsg.Any())
            {
                foreach (var image in imageMsg)
                {
                    tcc.Over();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($"{tcc.Span().TotalMilliseconds * 100:0}ms][{image.Type}]: ");
                    Console.ResetColor();
                    Console.WriteLine($"{image.Url}");
                }
                isParseTrue = true;
            }
            else
            {
                return false;
            }

            return isParseTrue;
        }

        private static void PlainMsgParse(Members mem, Groups group, PlainMessage message, GroupMessageReceiver receiver)
        {
            var command = CommandSplit(message.Text);
            if (GroupMsg.DGroupMsg.ContainsKey(command[0]))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"[CMD][{GroupMsg.DGroupMsg[command[0]]}]: ");
                Console.ResetColor();
                Console.WriteLine($"{UtilHelper.ListToString(command)}");
                if (GroupMsg.DGroupMsg[command[0]] == GroupMsg.MsgType.Game)
                {
                    GameAction.CommandParse(mem, group, command, receiver);
                }
                else if (GroupMsg.DGroupMsg[command[0]] == GroupMsg.MsgType.Func)
                {
                    FuncAction.CommandParse(mem, group, command, receiver);
                }
            }
            else
            {
                //关键词不存在或输入错误

            }

        }

        private static List<string> CommandSplit(string command)
        {
            List<string> cList = new();
            var list = command.Split(" ").ToList();
            if (list.Count > 0 && list != null)
            {
                if (list.Count >= 1)
                {
                    foreach (var l in list)
                    {
                        cList.Add(l);
                    }
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
