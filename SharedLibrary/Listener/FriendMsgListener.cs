using Mirai.Net.Data.Messages;
using Mirai.Net.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Listener
{
    public class FriendMsgListener : ICommandModule
    {
        public bool? IsEnable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Execute(MessageReceiverBase receiver, MessageBase executeMessage)
        {
            throw new NotImplementedException();
        }
    }
}
