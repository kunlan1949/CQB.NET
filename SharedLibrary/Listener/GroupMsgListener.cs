using Db.Bot;
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
                var m = Members.Find(Members._.MemGroup == receiver.Sender.Group.Id & Members._.MemQq==receiver.Sender.Id);
                var g = Groups.Find(Groups._.GrpId == receiver.Sender.Group.Id);
                if (g!= null)
                {
                    if(m != null)
                    {
                        var p = GroupMessageAction.GroupCommandParseAsync(m, g, receiver);
                        if (!p.Result)
                        {
                            //不符合指令
                        }
                    }
                    else
                    {
                        //不存在成员记录(此处需要成员注册)
                    }

                }
                else
                {
                    //不存在群或成员记录(此处需要群注册)

                }
            }
        }
    }
}
