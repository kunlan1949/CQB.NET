using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Parse
{
    class FortuneParse
    {
        private static async Task<HtmlDocument> doc(string url)
        {
            var web = new HtmlWeb();
            var htmlDocument = await web.LoadFromWebAsync(url);
            return htmlDocument;
        }

        public static async Task<HtmlDocument> MainParseAsync()
        {
            var htmlDocument = await doc("https://www.d1xz.net/yunshi/");
            return htmlDocument;
        }

        public static async Task<string> luckResultAsync(string parse, HtmlDocument document)
        {
            var result = "";
            var urlNode = document.DocumentNode.SelectSingleNode(parse);
            var url = urlNode.Attributes["href"].Value;

            var resultDocument = await doc("https://www.d1xz.net" + url);
            var resultParse = "//*[@class='txt']/p";
            var resultNode = resultDocument.DocumentNode.SelectSingleNode(resultParse);
            result = resultNode.InnerText;


            return result;
        }



        public static string Parse(Sign sign)
        {
            string str = "";
            switch (sign)
            {
                case Sign.Aries: str = "//*[@class='xzys_left fl']/div[1]/div/div[2]/a"; break;
                case Sign.Taurus: str = "//*[@class='xzys_left fl']/div[2]/div/div[2]/a"; break;
                case Sign.Gemini: str = "//*[@class='xzys_left fl']/div[3]/div/div[2]/a"; break;
                case Sign.Cancer: str = "//*[@class='xzys_left fl']/div[4]/div/div[2]/a"; break;
                case Sign.Leo: str = "//*[@class='xzys_left fl']/div[5]/div/div[2]/a"; break;
                case Sign.Virgo: str = "//*[@class='xzys_left fl']/div[6]/div/div[2]/a"; break;
                case Sign.Libra: str = "//*[@class='xzys_left fl']/div[7]/div/div[2]/a"; break;
                case Sign.Scorpio: str = "//*[@class='xzys_left fl']/div[8]/div/div[2]/a"; break;
                case Sign.Sagittarius: str = "//*[@class='xzys_left fl']/div[9]/div/div[2]/a"; break;
                case Sign.Capricorn: str = "//*[@class='xzys_left fl']/div[10]/div/div[2]/a"; break;
                case Sign.Aquarius: str = "//*[@class='xzys_left fl']/div[11]/div/div[2]/a"; break;
                case Sign.Pisces: str = "//*[@class='xzys_left fl']/div[12]/div/div[2]/a"; break;
            }
            return str;
        }

        public enum Sign
        {
            /// <summary>
            /// 白羊座
            /// </summary>
            Aries,
            /// <summary>
            /// 金牛座
            /// </summary>
            Taurus,
            /// <summary>
            /// 双子座
            /// </summary>
            Gemini,
            /// <summary>
            /// 巨蟹座
            /// </summary>
            Cancer,
            /// <summary>
            /// 狮子座
            /// </summary>
            Leo,
            /// <summary>
            /// 处女座
            /// </summary>
            Virgo,
            /// <summary>
            /// 天秤座 
            /// </summary>
            Libra,
            /// <summary>
            /// 天蝎座
            /// </summary>
            Scorpio,
            /// <summary>
            /// 射手座
            /// </summary>
            Sagittarius,
            /// <summary>
            /// 魔羯座
            /// </summary>
            Capricorn,
            /// <summary>
            /// 水瓶座
            /// </summary>
            Aquarius,
            /// <summary>
            /// 双鱼座
            /// </summary>
            Pisces,
        }
        /// <summary>
        /// 白羊座
        /// </summary>
        public static int Aries = 1;
        /// <summary>
        /// 金牛座
        /// </summary>
        public static int Taurus = 2;
        /// <summary>
        /// 双子座
        /// </summary>
        public static int Gemini = 3;
        /// <summary>
        /// 巨蟹座
        /// </summary>
        public static int Cancer = 4;
        /// <summary>
        /// 狮子座
        /// </summary>
        public static int Leo = 5;
        /// <summary>
        /// 处女座
        /// </summary>
        public static int Virgo = 6;
        /// <summary>
        /// 天秤座 
        /// </summary>
        public static int Libra = 7;
        /// <summary>
        /// 天蝎座
        /// </summary>
        public static int Scorpio = 8;
        /// <summary>
        /// 射手座
        /// </summary>
        public static int Sagittarius = 9;
        /// <summary>
        /// 魔羯座
        /// </summary>
        public static int Capricorn = 10;
        /// <summary>
        /// 水瓶座
        /// </summary>
        public static int Aquarius = 11;
        /// <summary>
        /// 双鱼座
        /// </summary>
        public static int Pisces = 12;
    }
}

