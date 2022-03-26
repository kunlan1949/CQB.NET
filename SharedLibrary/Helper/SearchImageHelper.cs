using Db.Bot;
using IqdbApi;
using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Utils.Scaffolds;
using SharedLibrary.Module.Message;
using SharedLibrary.Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharedLibrary.Model.MissionModel;

namespace SharedLibrary.Helper;
internal class SearchImageHelper
{
    public static async Task SearchImageWithOutTextAsync(Members mem, GroupMessageReceiver messageReceiver,int type)
    {

    }
        public static async Task SearchImageWithTextAsync(Members mem, GroupMessageReceiver messageReceiver,List<string> command)
    {
        var mission = MissionHelper.missionExist(mem.MissionId);
        if (mission == null)
        {
            if (!UtilHelper.ISTODAY(mem.ImgTime))
            {
                mem.SimageLimit = 5;
                mem.ImgTime = UtilHelper.GetTimeUnix().ToString();
                mem.Update();
            }
            if (mem.SimageLimit > 0)
            {
                //if (mem.SImg == 0)
                //{
                if (command[1] != null)
                {
                    if (command[1].Contains("模糊"))
                    {
                        MissionHelper.createMission(mem, messageReceiver, (int)MissionType.SIMAGE, 1);
                        memSetDB(mem, true);
                        //模糊搜索
                        await SearchObscureImageAsync(mem, messageReceiver, command[2]);

                    }
                    else
                    {
                        MissionHelper.createMission(mem, messageReceiver, (int)MissionType.SIMAGE, 0);
                        memSetDB(mem, true);
                        //普通搜索
                        await SearchDefaultImageAsync(mem, messageReceiver,command[2]);
                    }
                }
                else
                {
                    MissionHelper.createMission(mem, messageReceiver, (int)MissionType.SIMAGE, 0);
                    memSetDB(mem, true);
                    await SearchDefaultImageAsync(mem, messageReceiver, command[2]);
                }
                //}
                //else
                //{
                //    await SendGroupMessageModule.sendGroupAtAsync(messageReceiver, "已存在识图任务！请等待完成后再次使用！", false);
                //}
            }
            else
            {
                await SendGroupMessage.sendAtAsync(messageReceiver, "您今日的识图次数已经用完!", false); 
            }
        }
        else
        {
            var mType = GetType(mission.MType, mission.MTypeAux);
            await SendGroupMessage.sendAtAsync(messageReceiver, $"您存在一个未完成任务！任务种类：{mType}", true);
        }
    }

    private static async Task SearchDefaultImageAsync(Members mem, GroupMessageReceiver messageReceiver, string imgUrl)
    {
        Console.WriteLine("Default\n");
        await SendGroupMessage.sendAtAsync(messageReceiver, "识图需要时间，请耐心等待！", false);
        Console.WriteLine($"要查找的图片链接：【{imgUrl}】");
        MessageBase[] msg = { };

        await SearchImageParse.imgAsync(imgUrl).ContinueWith(async (e) =>
        {
            var imageInfoList = e.Result;
            if (imageInfoList != null)
            {
                var loca = imgLocationTrans(imageInfoList.First().ImageLocation);
                msg = "".Append("识图信息来源：【ASCII2D】\n")
                .Append(new ImageMessage() { Url = imageInfoList.First().PreImageUrl, Type = Messages.Image })
                .Append($"作者:{imageInfoList.First().AuthorName}\n")
                .Append($"作者链接:{imageInfoList.First().ImageAuthorUrl}\n")
                .Append($"作品名:{imageInfoList.First().ImageName}\n")
                .Append($"作品详情链接:{imageInfoList.First().DetailUrl}\n")
                .Append($"出处:{loca}\n")
                .Append($"作品有可能同时出现在Pixiv和Twitter上，\n本查询优先展示Twitter\n")
                .Append($"您的本日查询次数剩余 {mem.SimageLimit} 次\n次数会在每日凌晨更新!\n");
                await SendGroupMessage.sendAtAsync(messageReceiver, msg, false);
                MissionHelper.endMission(mem, mem.MissionId);
                //修改数据库
                memSetDB(mem, false);
            }
            else
            {
                await SendGroupMessage.sendAtAsync(messageReceiver, "图片未能找到对应结果，请使用模糊查询", false);
                MissionHelper.endMission(mem, mem.MissionId);
                //修改数据库
                memSetDB(mem, false);
            }
        });
    }

    private static async Task SearchObscureImageAsync(Members mem, GroupMessageReceiver messageReceiver,string imgUrl)
    {
        Console.WriteLine("Obscure\n");
        IIqdbClient api = new IqdbClient();

        await SendGroupMessage.sendAtAsync(messageReceiver, "模糊查询开始，识别需要时间，请耐心等待！", false);
        MessageBase[] msg = { };
        var results = await api.SearchUrl($"{imgUrl}");
        var url = "https://iqdb.org/" + results.Matches[0].PreviewUrl;
        Console.WriteLine(url);
        var loca = imgLocationTrans(results.Matches[0].Source.ToString());
        msg = "".Append("识图信息来源：【IQDB】\n")
        .Append(new ImageMessage() { Url = url, Type = Messages.Image })
        .Append($"作品详情链接:{results.Matches[0].Url}\n")
        .Append($"出处:{loca}\n")
        .Append($"准确度：{results.Matches[0].Similarity}%\n【大于90%匹配度高,具体以缩略图为准】\n")
        .Append($"您的本日查询次数剩余 {mem.SimageLimit} 次\n次数会在每日凌晨更新!\n");
        await SendGroupMessage.sendAtAsync(messageReceiver, msg, false);
        MissionHelper.endMission(mem, mem.MissionId);
        //修改数据库
        memSetDB(mem, false);

    }

    private static async Task exeDefaultAsync(Members mem, GroupMessageReceiver messageReceiver, string imageUrl)
    {
            
        //if (command != null)
        //{
        //    if (image.Length > 0)
        //    {

        //        await SendGroupMessageModule.sendGroupAtAsync(messageReceiver, "识图需要时间，请耐心等待！", false);
        //        Console.WriteLine($"要查找的图片链接：【{image[0].Url}】+【{image[0].Base64}】");
        //        MessageBase[] msg = { };

        //        await SearchImageAction.imgAsync(image[0].Url).ContinueWith(async (e) =>
        //        {
        //            var imageInfoList = e.Result;
        //            if (imageInfoList != null)
        //            {
        //                var loca = imgLocationTrans(imageInfoList.First().ImageLocation);
        //                msg = "".Append("识图信息来源：【ASCII2D】\n")
        //                .Append(new ImageMessage() { Url = imageInfoList.First().PreImageUrl, Type = Messages.Image })
        //                .Append($"作者:{imageInfoList.First().AuthorName}\n")
        //                .Append($"作者链接:{imageInfoList.First().ImageAuthorUrl}\n")
        //                .Append($"作品名:{imageInfoList.First().ImageName}\n")
        //                .Append($"作品详情链接:{imageInfoList.First().DetailUrl}\n")
        //                .Append($"出处:{loca}\n")
        //                .Append($"作品有可能同时出现在Pixiv和Twitter上，\n本查询优先展示Twitter\n")
        //                .Append($"您的本日查询次数剩余 {mem.SimageLimit} 次\n次数会在每日凌晨更新!\n");
        //                await SendGroupMessageModule.sendGroupAtAsync(messageReceiver, msg, false);
        //                MissionHelper.endMission(mem, mem.MissionId);
        //                //修改数据库
        //                memSetDB(mem, false);
        //            }
        //            else
        //            {
        //                await SendGroupMessageModule.sendGroupAtAsync(messageReceiver, "图片未能找到对应结果，请使用模糊查询", false);
        //                MissionHelper.endMission(mem, mem.MissionId);
        //                //修改数据库
        //                memSetDB(mem, false);
        //            }
        //        });
        //    }
        //    else
        //    {
        //        await SendGroupMessageModule.sendGroupAtAsync(messageReceiver, "请发送您想要查找的图片!", false);
        //    }

        //}

    }
    private static void memSetDB(Members members, bool onoff)
    {
        if (onoff)
        {

            var sCount = members.SimageLimit;
            var dCount = 0;
            dCount = sCount - 1;
            if (dCount == 0)
            {
                members.ImgTime = UtilHelper.GetTimeUnix().ToString();
            }
            members.SimageLimit = dCount;

            members.Update();
        }
        else
        {
            members.Update();
        }

    }
    private static string GetType(int missionType, int missionTypeAux)
    {
        var type = "";
        switch (missionType)
        {
            case 0:
                {
                    if (missionTypeAux == 0)
                    {
                        type = "识图";
                    }
                    else
                    {
                        type = "模糊识图";
                    }
                    break;
                }

            default: break;
        }
        return type;
    }

    private static string imgLocationTrans(string location)
    {
        string str = location.Replace("\"", "");
        string lo = "";
        if (str.Contains("pixiv"))
        {
            lo = $"{UtilHelper.ConvertFirstUpper(lo)}";
        }
        else if (str.Contains("twitter"))
        {
            lo = $"{UtilHelper.ConvertFirstUpper(lo)}";
        }
        else
        {
            lo = str;
        }

        return lo;
    }
}

