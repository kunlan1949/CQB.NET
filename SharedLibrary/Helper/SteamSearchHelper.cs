﻿using HtmlAgilityPack;
using Newtonsoft.Json;
using RestSharp;
using SharedLibrary.Action;
using SharedLibrary.Http;
using SharedLibrary.Model.FuncModel;
using SharedLibrary.Module.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Helper
{
    internal class SteamSearchHelper
    {
        public static async Task<SteamInfoModel> GetValueAsync(string keyword)
        {
            var info = new SteamInfoModel();
            ///
            /// 当前数量
            ///http://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1?appid=
            ///

            ///
            ///https://store.steampowered.com/search/?snr=1_4_4__12&term=
            ///
            var mainUrl = "";
            var steamid = "";
            TimeConsumingCounter tcc = new TimeConsumingCounter();

            if (RegHelper.IsUint(keyword))
            {
                mainUrl = "https://store.steampowered.com/app/" + $"{keyword}";
                Console.WriteLine("通过id查询");
                steamid = keyword;
                info = await getParamsAsync(mainUrl, steamid, tcc);
            }
            else
            {
                tcc.Start();
                var restResponse = searchRestAsync(keyword);
                if(restResponse.Result != null)
                {
                    var mainDoc = doch(restResponse.Result.Content);
                    tcc.Over();
                    Console.WriteLine("html文档映射实体操作耗时" + tcc.Span());
                    tcc.Start();
                    var mainParse = "//*[@id='search_resultsRows']/a[1]";
                    var mainNode = mainDoc.DocumentNode.SelectSingleNode(mainParse);


                    if (mainNode != null)
                    {
                        mainUrl = mainNode.Attributes["href"].Value;
                        steamid = mainNode.Attributes["data-ds-itemkey"].Value.Replace("App_", "");
                    }
                    else
                    {
                        return null;
                    }
                    info = await getParamsAsync(mainUrl, steamid, tcc);
                    return info;
                }
                else
                {
                    return null;
                }
               
            }




            // Console.WriteLine(response.Content);
            return info;
        }

        private static async Task<SteamInfoModel> getParamsAsync(string mainUrl, string steamid, TimeConsumingCounter tcc)
        {
            var info = new SteamInfoModel();
            if (mainUrl != "")
            {
                var gamePage = await httpGetExecAsync(mainUrl);
                //http://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1?appid=1579440

                var gameDoc = doch(gamePage.Content);
                var gameParse = "//*[@class='page_content_ctn']";
                var gameNode = gameDoc.DocumentNode.SelectSingleNode(gameParse);

                //图片链接
                var imageParse = "//*[@class='game_header_image_full']";
                var imageNode = gameDoc.DocumentNode.SelectSingleNode(imageParse);
                var imageUrl = imageNode.Attributes["src"].Value;

                //发行日期
                //var genDateParse = "//*[@id='genresAndManufacturer']//br[3]/following-sibling::*";
                //var genDateNode = gameDoc.DocumentNode.SelectSingleNode(genDateParse);
                //var genDate = genDateNode.InnerText;

                //内容简介
                var descParse = "//*[@class='game_description_snippet']";
                var descNode = gameDoc.DocumentNode.SelectSingleNode(descParse);

                var desc = "无";
                if (descNode != null)
                {
                    desc = descNode.InnerText;
                }

                //名字
                var nameParse = "//*[@id='appHubAppName']";
                var nameNode = gameDoc.DocumentNode.SelectSingleNode(nameParse);
                var name = nameNode.InnerText;


                //暂无评价
                var nevaCountParse = "//*[@class='user_reviews_summary_row']/div[2]";
                var nevaCountNode = gameDoc.DocumentNode.SelectSingleNode(nevaCountParse);
                var neva = nevaCountNode.InnerText;


                var evaCount = "无";
                var evaStatus = "无";

                if (HtmlParseHelper.NOHTMLSPACE(neva).Contains("无用户评测"))
                {
                    evaCount = "无用户评测";
                    evaStatus = "无用户评测";
                }
                else
                {
                    //总评过少
                    var levaCountParse = "//*[@class='game_review_summary not_enough_reviews']";
                    var levaCountNode = gameDoc.DocumentNode.SelectSingleNode(levaCountParse);


                    if (levaCountNode == null)
                    {
                        //总评数
                        var evaCountParse = "//*[@class='user_reviews_summary_row'][2]/div/span[2]";
                        var evaCountNode = gameDoc.DocumentNode.SelectSingleNode(evaCountParse);
                        if (evaCountNode != null)
                        {
                            evaCount = evaCountNode.InnerText.Replace("(", "").Replace(")", "");

                            //总评状态
                            var evaStatusParse = "//*[@class='user_reviews_summary_row'][2]/div/span[1]";
                            var evaStatusNode = gameDoc.DocumentNode.SelectSingleNode(evaStatusParse);
                            evaStatus = evaStatusNode.InnerText;
                        }
                        else
                        {
                            evaCountParse = "//*[@class='user_reviews_summary_row'][1]/div/span[2]";
                            evaCountNode = gameDoc.DocumentNode.SelectSingleNode(evaCountParse);
                            evaCount = evaCountNode.InnerText.Replace("(", "").Replace(")", "");

                            //总评状态
                            var evaStatusParse = "//*[@class='user_reviews_summary_row'][1]/div/span[1]";
                            var evaStatusNode = gameDoc.DocumentNode.SelectSingleNode(evaStatusParse);
                            evaStatus = evaStatusNode.InnerText;
                        }


                    }
                    else
                    {
                        evaCount = levaCountNode.InnerText;
                        evaStatus = "需要更多用户评测来生成整体评价";
                    }
                }

                //暂无价格（未发售）
                var nPriceParse = "//*[@class='game_area_purchase_game_wrapper']";
                var nPriceNode = gameDoc.DocumentNode.SelectSingleNode(nPriceParse);


                //正常价格
                var priceParse = "//*[@class='game_area_purchase_game_wrapper']//*[@class='game_purchase_action_bg']/div[1]";

                //折扣力度
                var dcPercentParse = "//*[@class='game_area_purchase_game_wrapper']//*[@class='game_purchase_action_bg']//*[@class='discount_pct']";
                //打折前价格
                var bdcPriceParse = "//*[@class='game_area_purchase_game_wrapper']//*[@class='game_purchase_action_bg']//*[@class='discount_prices']/div[1]";
                //打折后价格
                var dcPriceParse = "//*[@class='game_area_purchase_game_wrapper']//*[@class='game_purchase_action_bg']//*[@class='discount_prices']/div[2]";

                var dcPercentNode = gameDoc.DocumentNode.SelectSingleNode(dcPercentParse);

                var disCountPercent = "";
                var bdcPrice = "";
                var dcPrice = "";
                var price = "";
                var isDisCount = false;

                if (nPriceNode != null)
                {
                    if (dcPercentNode != null)
                    {
                        var bdcPriceNode = gameDoc.DocumentNode.SelectSingleNode(bdcPriceParse);
                        var dcPriceNode = gameDoc.DocumentNode.SelectSingleNode(dcPriceParse);
                        disCountPercent = dcPercentNode.InnerText;
                        bdcPrice = bdcPriceNode.InnerText;
                        dcPrice = dcPriceNode.InnerText;
                        isDisCount = true;
                    }
                    else
                    {
                        var priceNode = gameDoc.DocumentNode.SelectSingleNode(priceParse);
                        price = priceNode.InnerText;
                    }
                }
                else
                {
                    price = "暂未发售";
                }
                var online = "";
                if (nPriceNode != null)
                {
                    string onlineRe = HttpApi.HttpGet("http://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1?appid=" + $"{steamid}");
                    OnlineModel om = JsonConvert.DeserializeObject<OnlineModel>(onlineRe);
                    online = om.response.player_count.ToString();
                }
                else
                {
                    online = "无";
                }




                if (isDisCount)
                {
                    info = new SteamInfoModel()
                    {
                        GameName = name,
                        GameDesc = HtmlParseHelper.NOHTMLSPACE(desc),
                        GameEvaCount = HtmlParseHelper.NOHTMLSPACE(evaCount),
                        GameBdcPrice = HtmlParseHelper.NOHTMLSPACE(bdcPrice),
                        GameDcPercent = HtmlParseHelper.NOHTMLSPACE(disCountPercent),
                        GameDcPrice = HtmlParseHelper.NOHTMLSPACE(dcPrice),
                        GameEvaStatus = HtmlParseHelper.NOHTMLSPACE(evaStatus),
                        GameUrl = mainUrl,
                        GameImgUrl = imageUrl,
                        GameOnline = online,
                        GameId = steamid
                    };
                }
                else
                {
                    info = new SteamInfoModel()
                    {
                        GameName = name,
                        GameDesc = HtmlParseHelper.NOHTMLSPACE(desc),
                        GameEvaCount = HtmlParseHelper.NOHTMLSPACE(evaCount),
                        GameEvaStatus = HtmlParseHelper.NOHTMLSPACE(evaStatus),
                        GamePrice = HtmlParseHelper.NOHTMLSPACE(price),
                        GameUrl = mainUrl,
                        GameImgUrl = imageUrl,
                        GameOnline = online,
                        GameId = steamid
                    };
                }
                tcc.Over();
                Console.WriteLine("解析操作耗时" + tcc.Span());

            }
            return info;
        }


        private static async Task<RestResponse> searchRestAsync(string keyword)
        {
            string url = "https://store.steampowered.com/search/";
            string domain = new Uri(url).Host;
            TimeConsumingCounter tcc = new TimeConsumingCounter();
            tcc.Start();

            var client = new RestClient(url);

            var request = new RestRequest();
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:91.0) Gecko/20100101 Firefox/91.0");
            request.Timeout = 10000;
            request.AddHeader("Cache-Control", "no-cache");
            request.AddQueryParameter("snr", "1_4_4__12");
            request.AddQueryParameter("term", $"{keyword}");
            client.AddCookie("steamCountry", "CN%d4ea61d77497f82a0d74ba7f9e9f58bf", "/", domain);
            client.AddCookie("timezoneOffset", "28800", "/", domain);
            var response = await client.ExecuteAsync(request);

            tcc.Over();
            Console.WriteLine("第一次请求操作耗时" + tcc.Span());
            if (response.Content ==null)
            {
                return null;
            }
            return response;
        }


        private static async Task<RestResponse> httpGetExecAsync(string url)
        {
            string domain = new Uri(url).Host;
            var client = new RestClient(url);

            var request = new RestRequest();
            request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:91.0) Gecko/20100101 Firefox/91.0");
            request.Timeout = 10000;
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
            //年龄验证
            client.AddCookie("birthtime", "880905601","/", domain);
            //中国时区
            client.AddCookie("timezoneOffset", "28800", "/", domain);
            //使用语言、所在地区
            client.AddCookie("steamCountry", "CN%d4ea61d77497f82a0d74ba7f9e9f58bf", "/", domain);
            var response = await client.ExecuteAsync(request);
            return response;
        }

        private static async Task<HtmlDocument> doc(string url)
        {
            var web = new HtmlWeb();
            var htmlDocument = await web.LoadFromWebAsync(url);
            return htmlDocument;
        }

        private static HtmlDocument doch(string html)
        {
            var htmlDoc = new HtmlDocument();

            htmlDoc.LoadHtml(html);
            return htmlDoc;
        }


    }
}
