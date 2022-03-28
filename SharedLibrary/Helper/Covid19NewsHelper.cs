using Db.Bot;
using Mirai.Net.Data.Messages.Receivers;
using Mirai.Net.Utils.Scaffolds;
using Newtonsoft.Json.Linq;
using RestSharp;
using SharedLibrary.Model.FuncModel;
using SharedLibrary.Module.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Helper
{
    internal class Covid19NewsHelper
    {
        public static async Task GetLastNewsAsync(Members mem, Groups group, List<string> command, GroupMessageReceiver receiver)
        {
            if (!string.IsNullOrEmpty(command[1]))
            {
                var citys = Citys.Find(Citys._.CityName == command[1]);
                if(citys != null)
                {
                    Console.WriteLine(citys.CityName);
                    var news = await CityNewsUpdateAsync(citys.CityName);
                    var message = "".Append($"【{citys.CityName}*疫情】\n")
                        .Append($"现有确诊：{news.presentNumber}例\n")
                        .Append($"本日新增：{news.sureNewNumber}例\n")
                        .Append($"无症状病例：{news.sureNewHidNumber}例\n")
                        .Append($"高风险地区数：{news.highRankNumber}处\n")
                        .Append($"中风险地区数：{news.midRankNumber}处\n")
                        .Append($"累计治愈：{news.cureNumber}例\n")
                        .Append($"累计确诊：{news.definiteNumber}例\n")
                        .Append($"[{news.newsTime}]\n");
                    await SendGroupMessage.sendAtAsync(receiver, message, false);
                }
                else
                {
                    await SendGroupMessage.sendAtAsync(receiver, "错误，无法找到您查询的相关城市信息！", false);
                }
            }
        }

        private static async Task<Covid19NewsModel> CityNewsUpdateAsync(string cityName)
        {
            var client = new RestClient($"https://m.sm.cn");

            var request = new RestRequest($"/api/rest?format=json&method=Huoshenshan.ncov2022&type=latest&news_type=ncp&city={cityName}", Method.Options);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:91.0) Gecko/20100101 Firefox/91.0");
            request.AddHeader("Content-Type", "application/json");
            request.Timeout = 5000;



            var response = await client.ExecuteAsync(request);
            JObject responseObj = JObject.Parse(response.Content.ToString());
            var data = "cityData";
            if (cityName=="北京" || cityName=="上海" || cityName=="重庆" || cityName== "天津")
            {
                data = "provinceData";
            }
            JObject dataObj = responseObj[$"{data}"].Value<JObject>();
            //数据统计时间
            string newsTime = responseObj["time"].Value<string>();
            //当前确诊人数
            string present = dataObj["present"].Value<string>();
            //累计确诊人数
            string sureCnt = dataObj["sure_cnt"].Value<string>();
            //累计死亡人数
            string dieCnt = dataObj["die_cnt"].Value<string>();
            //累计治愈人数
            string cureCnt = dataObj["cure_cnt"].Value<string>();
            //当日新增人数
            string sureNewCnt = dataObj["sure_new_cnt"].Value<string>();
            //无症状病例数
            string sureNewHid = dataObj["sure_new_hid"].Value<string>();

            JObject danager =  dataObj["danger"].Value<JObject>();
            //中风险区数量
            string midRankArea = danager["1"].Value<string>();
            //高风险区数量
            string highRankArea = danager["2"].Value<string>();

            var model = new Covid19NewsModel()
            {
                presentNumber = present,
                definiteNumber = sureCnt,
                dieNumber = dieCnt,
                cureNumber = cureCnt,
                sureNewNumber = sureNewCnt, 
                sureNewHidNumber = sureNewHid,
                midRankNumber = midRankArea,
                highRankNumber = highRankArea,
                newsTime = newsTime
            };
            return model;
        }
    }
}
