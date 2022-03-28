using ImageMagick;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Helper
{

    internal class PCRRankHelper
    {
        static List<string> files = new List<string> { "rank_1.png" };//,"rank_2.png","rank_3.png"};
        public static async Task GetNewsAsync()
        {
            foreach (string fileName in files)
            {
                var m = ImageToBase64(fileName);
                FileStream fs1 = new FileStream($@"{AppDomain.CurrentDomain.BaseDirectory}test.txt", FileMode.Create, FileAccess.Write);//创建写入文件                //设置文件属性为隐藏
                StreamWriter sw = new StreamWriter(fs1);
                sw.WriteLine(m);//开始写入值
                sw.Close();
                fs1.Close();
            }

        }
        private static string ImageToBase64(string fileName)
        {
            try
            {
                Assembly asm = Assembly.GetExecutingAssembly(); //读取嵌入式资源

                var stream = asm.GetManifestResourceStream($"SharedLibrary.Res.PCR.{fileName}");
               
                //Image img = Image.FromStream(stream);
                //img.Save($"{AppDomain.CurrentDomain.BaseDirectory+@$"\sss.png"}");
                //Bitmap bmp = new Bitmap(stream);
                //MemoryStream ms = new MemoryStream();
                //ms.Position = 0;
                string base64String = "";
                //var suffix = fileFullName.Substring(fileFullName.LastIndexOf('.') + 1,
                //    fileFullName.Length - fileFullName.LastIndexOf('.') - 1).ToLower();
                //var suffixName = suffix == "png"
                //    ? ImageFormat.Png
                //    : suffix == "jpg" || suffix == "jpeg"
                //        ? ImageFormat.Jpeg
                //        : suffix == "bmp"
                //            ? ImageFormat.Bmp
                //            : suffix == "gif"
                //                ? ImageFormat.Gif
                //                : ImageFormat.Jpeg;
                
                //bmp.Save(ms, ImageFormat.Png);
                //byte[] arr = new byte[ms.Length];
                //ms.Position = 0;
                //ms.Read(arr, 0, (int)ms.Length);
                //ms.Close();
                MagickReadSettings settings = new MagickReadSettings()
                {
                    ColorSpace = ColorSpace.sRGB,
                    Format = MagickFormat.Png,
                    UseMonochrome = false
                };
               
                using (MagickImage image = new MagickImage())
                {
                    image.SetProfile(ColorProfile.SRGB);
                    try
                    {
                        image.Read(stream,settings);
                        base64String = image.ToBase64();
                    }catch (Exception ex)
                    {
                        base64String = ex.Message;
                    }
                }
                return base64String;
                    //return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
