using Db.Bot;
using Mirai.Net.Data.Messages.Receivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Action.GroupMessage.Game
{
    class GameAction
    {
        public static void CommandParse(Members mem, Groups group, List<string> command,GroupMessageReceiver receiver)
        {
            Game game = new Game();
            //反射：寻找指定类中的对应函数名的函数
            var method = typeof(Game).GetMethod(DGame[command[0]]);
            //传递参数并调用此函数
            method.Invoke(game, new object[] { mem, group,command, receiver });
            
        }

        static Dictionary<string, string> DGame = new Dictionary<string, string>()
        {
            {"猜数字", "GussNum"},
            {"成语接龙", "Chengyu"}
        };
    }

    class Game
    {
        public void GussNum(Members mem, Groups group, List<string> command, GroupMessageReceiver receiver)
        {
           
        }

        public void Chengyu(Members mem, Groups group, List<string> command, GroupMessageReceiver receiver)
        {
           
        }
    }

  
}
