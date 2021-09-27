using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Modules;
using SharedLibrary.Action.GroupMessage;
using System;

namespace SharedLibrary.Listener
{
    public class GroupMsgListener : ICommandModule
    {
        public bool? IsEnable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Execute(MessageReceiverBase @base, MessageBase executeMessage)
        {
            if (@base is GroupMessageReceiver receiver)
            {

                var p = GroupMessageAction.GroupCommandParse(receiver);
                if (!p)
                {
                    //不符合指令
                }
            }
        }
    }
}
