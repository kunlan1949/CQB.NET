using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SharedLibrary.Helper
{
    class XmlHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="brief">预览标签</param>
        /// <param name="summary">文本内容</param>
        /// <param name="style">0-10样式</param>
        /// <returns></returns>
        public static string msgXml(string title, string brief, string summary)
        {
            var xml = "<?xml version=\"1.0\" encoding=\"utf - 8\" standalone=\"yes\"?>";
            string dirPath = @$"{AppDomain.CurrentDomain.BaseDirectory}Res\Test.xml";
            if (!File.Exists(dirPath))
            {
                createXml(dirPath);
            }
           
            XDocument doc = XDocument.Load(dirPath);
            XElement rootS = doc.Root; //获取根元素
            rootS.Attribute("brief").Value = brief;
            rootS.Attribute("brief").Value = brief;
            rootS.Attribute("serviceID").Value = "1";
            //IEnumerable<XElement> item = doc.Descendants("item");
            XElement item = rootS.Elements("item").First();
            XElement item2 = rootS.Elements("item").Last();
            XElement titleE = item.Element("title");
            XElement summaryE = item.Element("summary");
            XElement pictureE = item.Elements("picture").First();
            XElement summary2E = item2.Elements("summary").First();
            pictureE.Attribute("cover").Value = "https://img0.baidu.com/it/u=3122136587,3938996930&fm=26&fmt=auto";
            //pictureE.Remove();
            //XElement sourceE = item.Element("source");
            //sourceE.Remove();
            titleE.Value = title;
            summaryE.Value = summary;
            summary2E.Value = "???????";
            xml += doc.ToString();
            Console.WriteLine(xml);
            xml = UtilHelper.cleanStringEmpty(xml);
            return xml;
        }

        private static void createXml(string path)
        {
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),//（版本,编码,独立属性）
                new XElement("msg",     //添加msg根节点       
                new XAttribute("serviceID", "1"),
                //new XAttribute("action", "app"),
                new XAttribute("actionData", "com.android.browser"),
                  //com.android.browser
                //new XAttribute("url", "https://www.baidu.com"),
                new XAttribute("sourceMsgId", $"0"),
                new XAttribute("flag", "3"),
                new XAttribute("adverSign", "0"),
                new XAttribute("multiMsgFlag", "0"),
                new XAttribute("templateID", "-1"),
                new XAttribute("brief", $"[Test]")
                    )); ;

            XElement rootS = doc.Root; //获取根元素

            XElement itemE = new XElement("item", new XAttribute("layout", "0"));//创建节点
           
            rootS.Add(itemE);//将节点添加到根节点
            
            XElement titleE = new XElement("title");
            titleE.Value = "test title";
            XElement summaryE = new XElement("summary");
            summaryE.Value = "test summary";
            XElement pictureE = new XElement("picture", new object[] {
                new XAttribute("cover", $"url"),
                new XAttribute("w", $"0"),
                new XAttribute("h", $"0"),
            });
           
            itemE.Add(titleE);
            itemE.Add(summaryE);
            itemE.Add(pictureE);
           
            XElement item2E = new XElement("item", new XAttribute("layout", "9"));//创建节点
            rootS.Add(item2E);//将节点添加到根节点
            XElement summary2E = new XElement("summary");
            summaryE.Value = "test summary2";
            item2E.Add(summary2E);

            XElement sourceE = new XElement("source", new object[] {
                    new XAttribute("name", $"from"),
                    new XAttribute("icon", $"https://qzs.qq.com/ac/qzone_v5/client/auth_icon.png"),
                    new XAttribute("action", $""),
                    new XAttribute("appid", $"0")
            });
            rootS.Add(sourceE);
            doc.Save(path);
        }
    }
}
