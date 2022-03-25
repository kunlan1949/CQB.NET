﻿using Db.Bot;
using HtmlAgilityPack;
using Mirai.Net.Data.Messages;
using Mirai.Net.Data.Messages.Concretes;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Utils.Scaffolds;
using SharedLibrary.Helper;
using SharedLibrary.Model.FuncModel;
using SharedLibrary.Module.Message;
using SharedLibrary.Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Action.GroupMessage.Func
{
    class FuncAction
    {
        public static void CommandParse(Members mem, Groups group, List<string> command, GroupMessageReceiver receiver)
        {
            Func func = new Func();
            //反射：寻找指定类中的对应函数名的函数
            var method = typeof(Func).GetMethod(DFunc[command[0]]);
            //传递参数并调用此函数
            method.Invoke(func, new object[] { mem,group,command, receiver });

        }

        static Dictionary<string, string> DFunc = new Dictionary<string, string>()
        {
            {"翻译", "Trans"},
            {"查游戏", "SGame"},
            {"免费游戏", "SFreeGame"},
            {"运势", "Fortune"},
            {"点歌", "PlaySong"},
            {"天气", "Weather"}, 
            {"笑话", "Joke"},
            {"抽签", "Sortilege"},
            {"识图", "SearchImage"}
        };
    }

    class Func
    {
        public async Task Trans(Members mem, Groups group, List<string> command, GroupMessageReceiver receiver)
        {

        }
        public async Task SGame(Members mem, Groups group, List<string> command, GroupMessageReceiver receiver)
        {
            if (command[1] != null && command[1] != "")
            {
                await SendGroupMessage.sendAsync(receiver, "查询需要10S左右，请稍等");
                var value = new SteamInfoModel();
                await SteamSearchHelper.GetValueAsync(command[1]).ContinueWith(async (e) => {
                    value = e.Result;
                    if(value != null)
                    {
                        MessageBase[] commonMsg = { };

                        MessageBase[] priceMsg = { };

                        if (!value.GamePrice.Contains("-1"))
                        {
                            priceMsg = "".Append(
                            $"游戏价格：{value.GamePrice}\n");
                        }
                        else
                        {
                            priceMsg = $"【该游戏{value.GameDcPercent}折扣中!】\n".Append(
                            $"游戏原价：{value.GameBdcPrice}\n" +
                            $"游戏现价：{value.GameDcPrice}\n");
                        }


                        commonMsg = "".Append(new ImageMessage() { Url = value.GameImgUrl, Type = Messages.Image })
                        .Append(
                            $"游戏ID：{value.GameId}\n" +
                            $"游戏名：{value.GameName}\n" +
                            $"链接：：{value.GameUrl}\n" +
                            $"在线人数：{value.GameOnline}\n" +
                            $"评价等级：[{value.GameEvaStatus}]\n" +
                            $"评价人数：[{value.GameEvaCount}]\n" +
                            $"游戏简介：{value.GameDesc}\n").Append(priceMsg);


                       await SendGroupMessage.sendAtAsync(receiver, commonMsg, true);
                    }
                    else
                    {
                        await SendGroupMessage.sendAtAsync(receiver, "查询失败！", true);
                    }
                  
                });

            }
            else
            {

            }
        }
        public async Task SFreeGame(Members mem, Groups group, List<string> command, GroupMessageReceiver receiver)
        {

        }
        public async Task Fortune(Members mem, Groups group, List<string> command, GroupMessageReceiver receiver)
        {
            var d = FortuneHelper.SignDic(command[1]);
            if (d > 0)
            {
                var c = Constellation.Find(Constellation._.Sign == command[1]);
                if (c != null)
                {
                    if (UtilHelper.ISTODAY(c.UpdateTime))
                    {
                        Console.WriteLine("存在，数据库获取");
                        await SendGroupMessage.sendAtAsync(receiver, c.LuckResult,true);
                    }
                    else
                    {
                        Console.WriteLine("过时，网页重新获取");
                        var m = FortuneHelper.SignDic(command[1]);

                        var result = "";
                        var web = new HtmlWeb();
                        var htmlDocument = FortuneParse.MainParseAsync().Result;
                        var parse = FortuneParse.Parse((FortuneParse.Sign)m);
                        result = FortuneParse.luckResultAsync(parse, htmlDocument).Result;
                        await SendGroupMessage.sendAtAsync(receiver, result, true);
                        c.LuckResult = result;
                        c.UpdateTime = UtilHelper.GetUTCTimeUnix().ToString();
                        c.Update();
                    }
                }
                else
                {
                    Console.WriteLine("不存在，网页获取");
                    var m = FortuneHelper.SignDic(command[1]);

                    var result = "";
                    var web = new HtmlWeb();
                    var htmlDocument = FortuneParse.MainParseAsync().Result;
                    var parse = FortuneParse.Parse((FortuneParse.Sign)m);
                    result = FortuneParse.luckResultAsync(parse, htmlDocument).Result;
                    await SendGroupMessage.sendAtAsync(receiver, result, true);
                    var nc = new Constellation();
                    nc.Sign = command[1];
                    nc.LuckResult = result;
                    nc.UpdateTime = UtilHelper.GetUTCTimeUnix().ToString();
                    nc.Insert();
                }
            }
            else
            {
                await SendGroupMessage.sendAsync(receiver, "不存在的星座，请输入正确的星座名！");
            }
        }
        public async Task PlaySong(Members mem, Groups group, List<string> command, GroupMessageReceiver receiver)
        {

        }
        public async Task Weather(Members mem, Groups group, List<string> command, GroupMessageReceiver receiver)
        {
            MessageBase[] msg;
            msg = "".Append(new ImageMessage() {Base64=ImageHelper.makePic(),Type=Messages.Image});
            await SendGroupMessage.sendAsync(receiver, msg);
        }
        public async Task Joke(Members mem, Groups group, List<string> command, GroupMessageReceiver receiver)
        {
            string msg = "<?xml version='1.0' encoding='UTF-8' standalone='yes' ?><msg serviceID=\"146\" templateID=\"1\" action=\"web\" brief=\"[分享] 精神小伙说精神丨学习雷 锋好榜样\" sourceMsgId=\"0\" url=\"https://xw.qq.com/cmsid/20210928A08CR900?f=newdc\" flag=\"0\" adverSign=\"0\" multiMsgFlag=\"0\"><item layout=\"2\" advertiser_id=\"0\" aid=\"0\"><picture cover=\"https://mat1.gtimg.com/www/mobi/image/logo/tencent_logo.png\" w=\"0\" h=\"0\" /><title>精神小伙说精神丨学习雷锋好榜样</title><summary>精神小伙说精神丨学习雷锋好榜样,△精神小伙说精神丨学 习雷锋好榜样“学习雷锋好榜样，忠于革命忠于党，爱憎分明不忘本，立场坚…</summary></item><source name=\"QQ浏览器\" icon=\"https://url.cn/PWkhNu\" action=\" \" appid=\"-1\" /></msg>";
            await SendGroupMessage.sendXmlAsync(receiver, msg);
        }
        public async Task Sortilege(Members mem, Groups group, List<string> command, GroupMessageReceiver receiver)
        {
            string msg = XmlHelper.msgXml("牛啊","牛啊牛啊","nnnnn");
            await SendGroupMessage.sendXmlAsync(receiver, msg);
        }
        public async Task SearchImage(Members mem, Groups group, List<string> command, GroupMessageReceiver receiver)
        {
            
            //await SendGroupMessage.sendXmlAsync(receiver, msg);
        }
        
    }
}