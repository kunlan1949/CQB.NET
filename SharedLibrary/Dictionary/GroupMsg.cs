using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Dictionary
{
    //public class GroupMsgType 
    //{
    //   public string msg; 
    //   public string type; 
    //}
    public class GroupMsg
    {
        public enum MsgType
        {
            Null = -1,
            Game = 0,
            Func = 1,
            Ctrl = 2,
            Off = 2,
        };
        public static Dictionary<string, MsgType> DGroupMsg = new Dictionary<string, MsgType>()
        {
            {"",  MsgType.Null},
            {"猜数字", MsgType.Game},
            {"成语接龙", MsgType.Game},
            {"猜歌", MsgType.Game},



            {"翻译", MsgType.Func},
            {"查游戏", MsgType.Func},
            {"运势", MsgType.Func},
            {"点歌", MsgType.Func},
            {"天气", MsgType.Func},
            {"笑话", MsgType.Func},
            {"抽签", MsgType.Func},
            {"识图", MsgType.Func},
            {"疫情", MsgType.Func},

            {"解签", MsgType.Ctrl},
        };

    }


}
