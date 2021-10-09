using Db.Bot;
using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Utils.Scaffolds;
using NeteaseCloudMusicApi;
using Newtonsoft.Json.Linq;
using SharedLibrary.Helper;
using SharedLibrary.Module.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            {"成语接龙", "Chengyu"},
            {"猜歌", "GussSong"}
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
        public async Task GussSong(Members mem, Groups group, List<string> command, GroupMessageReceiver receiver)
        {
            var game = GamesStatus.Find(GamesStatus._.GameGroup == group.GrpId & GamesStatus._.GameType == "2" & GamesStatus._.GameStatus == "0");
            if (command.Count <= 1)
            {
                if (game == null)
                {
                    var msg =MusicMsgAsync(group,mem,true).Result;
                    await SendGroupMessage.sendAsync(receiver, msg);
                }
                else
                {
                    await SendGroupMessage.sendAsync(receiver, "已存在猜歌对局，请在结束对局后重开!");
                }
            }
            else
            {
                if (game != null)
                {
                    var m = StringHelper.GetSimilarityWith(game.GameParams, command[1]);
                    if ((int)(m*100) > 80)
                    {
                        await SendGroupMessage.sendAsync(receiver, $"居然猜对了！歌曲[{game.GameParams}]\n这么厉害吗!再来猜一首！");
                        var msg = MusicMsgAsync(group, mem, false).Result;
                        await SendGroupMessage.sendAsync(receiver, msg);
                    }
                    else
                    {
                        Console.WriteLine($"准确率：{(m*100).ToString("0.0")}%");
                    }
                }
                else
                {
                    await SendGroupMessage.sendAsync(receiver, "对局不存在!请发送[猜歌]开始对局！");
                }
            }
        }

        private static async Task<MessageBase[]> MusicMsgAsync(Groups group,Members mem,bool isNew)
        {
            var songUrl = "";
            var jumpUrl = "";
            var singerId = "";
            var songName = "";
            var songId = "";
            var imgUrl = "http://iomoi.top/guess.png";
            MessageBase[] msg = new MessageBase[2];
            var api = new CloudMusicApi();

getSong:
            var singerList = await api.RequestAsync(CloudMusicApiProviders.ArtistList, new Dictionary<string, object> { ["limit"] = "80", ["area"] = "7" });
            if (singerList != null)
            {
                JArray res = singerList.Value<JArray>("artists");
                int num = UtilHelper.getRandomNum(0, res.Count);

                singerId = res[num].Value<string>("id");
                Console.WriteLine($"歌手id={singerId}");
            }

            var singerTopSong = await api.RequestAsync(CloudMusicApiProviders.ArtistTopSong, new Dictionary<string, object> { ["id"] = $"{singerId}" });
            if (singerTopSong != null)
            {
                JArray res = singerTopSong.Value<JArray>("songs");
                int num = UtilHelper.getRandomNum(0, res.Count);
                songName = res[num].Value<string>("name");
                songId = res[num].Value<string>("id");

               
                Console.WriteLine($"歌曲id={songId}");
                Console.WriteLine($"未处理歌曲名={songName}");
                
                songName = SongNameHandle(songName);
                Console.WriteLine($"歌曲名={songName}");
            }

            var songAv = await api.RequestAsync(CloudMusicApiProviders.SongUrl, new Dictionary<string, object> { ["id"] = $"{songId}" });
            JArray songares = songAv.Value<JArray>("data");
            try
            {
                var url = songares[0].Value<string>("url");
                if(url == null)
                {
                    Console.WriteLine("无法播放");
                    goto getSong;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            songUrl = $"http://music.163.com/song/media/outer/url?id=" + songId + "&userid=32407630";
            jumpUrl = "http://iomoi.top/guess.png";


            msg = "".Append(new MusicShareMessage() { Type = Messages.MusicShare, MusicUrl = songUrl, PictureUrl = imgUrl, Title = $"猜歌大赛", JumpUrl = jumpUrl, Brief = $"[来猜歌啦]", Kind = "NeteaseCloudMusic", Summary = $"相似度达到80%通过" });

            if (isNew)
            {
                var nGame = new GamesStatus()
                {
                    GameGroup = group.GrpId,
                    GameParams = songName,
                    GameStarter = mem.MemQq,
                    GameType = "2",
                    GameStatus = "0",
                };
                nGame.Insert();
            }
            else
            {
                var game = GamesStatus.Find(GamesStatus._.GameGroup == group.GrpId & GamesStatus._.GameType == "2" & GamesStatus._.GameStatus == "0");
                if (game != null)
                {
                    game.GameParams = songName;
                    game.Update();
                }
            }
            return msg;
        }

        private static string SongNameHandle(string songName)
        {
            var song = "";
            song =  Regex.Replace(songName, "[(][^(]*[)]", string.Empty, RegexOptions.IgnoreCase);
            song = Regex.Replace(song, "[（][^（]*[）]", string.Empty, RegexOptions.IgnoreCase);

            return song.Trim();
        } 
    }

  
}
