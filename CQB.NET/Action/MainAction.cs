using Mirai.Net.Data.Events;
using Mirai.Net.Data.Events.Concretes.Request;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Sessions;
using Mirai.Net.Utils.Scaffolds;
using SharedLibrary.Helper;
using SharedLibrary.Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Listener;
using SharedLibrary.Action;

namespace CQB.NET.Action
{
    public class MainAction
    {
        //创建变量
        static MiraiBot bot;
        static ConfigModel cm;

        public static async Task StartAsync()
        {
            cm = ConfigHelper.GetInfo();
            bot = new MiraiBot()
            {
                Address = cm.Address,
                QQ = cm.Number,
                VerifyKey = cm.VerifyKey
            };
            Console.WriteLine($"Number={cm.Number}");
            Console.WriteLine($"VerifyKey={cm.VerifyKey}");
            await bot.LaunchAsync().ContinueWith((e) =>
            {
                if (e.IsCompleted)
                {
                    MessageListener();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("消息监听已启动！");
                    WakeUpAction.SayHello();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("错误，连接失败");
                }
            });
        }


        /// <summary>
        /// 消息处理
        /// </summary>
        /// <returns></returns>
        private static void MessageListener()
        {
            var groupMsg = new GroupMsgListener();
            var friendMsg = new FriendMsgListener();
            
            ///好友消息订阅
            bot.MessageReceived
               .OfType<FriendMessageReceiver>()
               .Subscribe(x =>
               {
                   friendMsg.Execute(x, x.MessageChain.First());
               });

            ///群聊消息订阅
            bot.MessageReceived
                .OfType<GroupMessageReceiver>()
                .Subscribe(x =>
                {
                    groupMsg.Execute(x, x.MessageChain.First());
                });

        }
        /// <summary>
        /// 事件处理
        /// </summary>
        /// <returns></returns>
        //private static void BehaviorListener()
        //{
        //    ///加好友请求
        //    bot.EventReceived.Where(x => x.Type == Events.NewFriendRequested)
        //    .Cast<NewFriendRequestedEvent>().Subscribe(x =>
        //    {

        //    });

        //    ///被邀请入群申请
        //    bot.EventReceived.Where(x => x.Type == Events.NewInvitationRequested)
        //    .Cast<NewInvitationRequestedEvent>().Subscribe(x =>
        //    {

        //    });
        //}
    }
}
