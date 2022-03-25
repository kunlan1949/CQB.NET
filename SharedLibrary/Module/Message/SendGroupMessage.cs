using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Sessions.Http.Managers;
using Mirai.Net.Utils.Scaffolds;
using SharedLibrary.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Module.Message
{
    public class SendGroupMessage
    {
        /// <summary>
        /// 推送指定群[字符串]
        /// </summary>
        /// <param name="group">群号</param>
        /// <param name="msg">消息</param>
        public static async void PostMessageAsync(string group, string msg)
        {
            TimeCounterHelper tcc = new TimeCounterHelper();
            tcc.Start();
            await MessageManager.SendGroupMessageAsync(group, "".Append(msg)).ContinueWith((e) => {
                tcc.Over();
                Console.WriteLine("发送耗时" + tcc.Span());
            }); ;
        }
        /// <summary>
        /// 推送指定群[消息链]
        /// </summary>
        /// <param name="group">群号</param>
        /// <param name="msg">消息</param>
        public static async void PostMessageAsync(string group, MessageBase[] msg)
        {
            TimeCounterHelper tcc = new TimeCounterHelper();
            tcc.Start();
            await MessageManager.SendGroupMessageAsync(group, "".Append(msg)).ContinueWith((e) => {
                tcc.Over();
                Console.WriteLine("发送耗时" + tcc.Span());
            }); ;
        }

        /// <summary>
        /// 发送普通消息，接收字符串格式
        /// </summary>
        /// <param name="receiver">群接收</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static async Task sendAsync(GroupMessageReceiver receiver, string msg)
        {
            TimeCounterHelper tcc = new TimeCounterHelper();
            tcc.Start();
            await receiver.SendGroupMessageAsync($"".Append(msg)).ContinueWith((e) => {
                tcc.Over();
                Console.WriteLine("发送耗时" + tcc.Span());
            });
        }
        /// <summary>
        /// 发送普通消息，接收消息链格式
        /// </summary>
        /// <param name="receiver">群接收</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>

        public static async Task sendAsync(GroupMessageReceiver receiver, MessageBase[] msg)
        {
            TimeCounterHelper tcc = new TimeCounterHelper();
            tcc.Start();
            await receiver.SendGroupMessageAsync(msg).ContinueWith((e) => {
                tcc.Over();
                Console.WriteLine("发送耗时" + tcc.Span());
            });
        }

        /// <summary>
        /// 发送@消息，接收字符串格式
        /// </summary>
        /// <param name="receiver">群接收</param>
        /// <param name="msg">消息</param>
        /// <param name="atMsgPosition">@的位置</param>
        /// <returns></returns>
        public static async Task sendAtAsync(GroupMessageReceiver receiver, string msg, bool atMsgPosition)
        {
            TimeCounterHelper tcc = new TimeCounterHelper();
            tcc.Start();
            if (atMsgPosition)
            {

                await receiver.SendGroupMessageAsync($"".Append(new AtMessage(receiver.Sender.Id)).Append(msg)).ContinueWith((e) => {
                    tcc.Over();
                    Console.WriteLine("发送耗时" + tcc.Span());
                });
            }
            else
            {
                await receiver.SendGroupMessageAsync($"".Append(msg).Append(new AtMessage(receiver.Sender.Id))).ContinueWith((e) => {
                    tcc.Over();
                    Console.WriteLine("发送耗时" + tcc.Span());
                });
            }

        }

        /// <summary>
        /// 发送@消息，接收消息链格式
        /// </summary>
        /// <param name="receiver">群接收</param>
        /// <param name="msg">消息链</param>
        /// <param name="atMsgPosition">@的位置</param>
        public static async Task sendAtAsync(GroupMessageReceiver receiver, MessageBase[] msg, bool atMsgPosition)
        {
            TimeCounterHelper tcc = new TimeCounterHelper();
            tcc.Start();
            if (atMsgPosition)
            {
                await receiver.SendGroupMessageAsync("".Append(new AtMessage(receiver.Sender.Id)).Append(msg)).ContinueWith((e) => {
                    tcc.Over();
                    Console.WriteLine("发送耗时" + tcc.Span());
                });
            }
            else
            {
                await receiver.SendGroupMessageAsync(msg.Append(new AtMessage(receiver.Sender.Id))).ContinueWith((e) => {
                    tcc.Over();
                    Console.WriteLine("发送耗时" + tcc.Span());
                });
            }
        }

        /// <summary>
        /// 发送XML标签
        /// </summary>
        /// <param name="receiver">群接收</param>
        /// <param name="msg">消息链</param>
        public static async Task sendXmlAsync(GroupMessageReceiver receiver, string msg)
        {
            TimeCounterHelper tcc = new TimeCounterHelper();
            tcc.Start();

            await receiver.SendGroupMessageAsync(new XmlMessage() { Xml = msg,Type = Messages.Xml }).ContinueWith((e) => {
                tcc.Over();
                Console.WriteLine("发送耗时" + tcc.Span());
            });
        }

        /// <summary>
        /// 发送Json标签
        /// </summary>
        /// <param name="receiver">群接收</param>
        /// <param name="msg">消息链</param>
        public static async Task sendAppAsync(GroupMessageReceiver receiver, string msg)
        {
            TimeCounterHelper tcc = new TimeCounterHelper();
            tcc.Start();

            await receiver.SendGroupMessageAsync(new AppMessage() {App = msg, Type = Messages.Json }).ContinueWith((e) => {
                tcc.Over();
                Console.WriteLine("发送耗时" + tcc.Span());
            });
        }
    }
}
