using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Helper
{
    class ImageHelper
    {
        /// <summary>
        /// 生成海报
        /// </summary>
        public static string makePic()
        {
            string base64 = "";
           
            string imgPath = AppDomain.CurrentDomain.BaseDirectory + @"Image/gzh.jpg";  //要插入的二维码图片路径 绝对路径
            Bitmap bitmapPic = new Bitmap(350, 630);// 新建一个 400*600的Bitmap 位图 
            Graphics g = Graphics.FromImage(bitmapPic);// 根据新建的 Bitmap 位图，创建画布
            g.Clear(Color.White);// 使用白色重置画布

            //Image headPic = Image.FromFile(imgHeadPath);//抬头
            //背景图
            //Image ewmPic = Image.FromFile(imgPath);//二维码

            //在背景图片上绘制指定的二维码图片 和抬头图片
            //g.DrawImage(headPic, 0, 0, 400, 200);
            //g.DrawImage(ewmPic, 260, 210, 130, 130);

            //在背景照片上添加文字  
            PointF drawPoint = new PointF(20F, 20.0F);
            AddFont(g, drawPoint, "┏....⭐...宠☆物...⭐....┓", Color.DarkGreen);
            drawPoint = new PointF(20F, 360.0F);
            AddFont(g, drawPoint, "┗....⭐...市☆场...⭐....┛", Color.DarkGreen);
            for (int i = 0; i < 10; i++)
            {
                var y = 20 + 30 * (i + 1);
                drawPoint = new PointF(20F,y);
                AddFont(g, drawPoint, "│Test Test Test Test Tes│", Color.DarkGreen);
            }
            g.FillRectangle(Brushes.Silver, 0, 570, 400, 60);//底部的矩形填充 填充由一对坐标，一个宽度和一个高度指定的矩形的内部
            drawPoint = new PointF(10F, 574F);
            AddFont(g, drawPoint, "Tips:请照顾好自己的宠物哦^_^", "楷体",14,Color.White);
            drawPoint = new PointF(10F, 604F);
            AddFont(g, drawPoint, "【宠物死亡可购买复活券】", "楷体", 14, Color.White);
            using (MemoryStream ms = new MemoryStream())
            {
                //将背景图存到内存流(ms)
                bitmapPic.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                base64 = Convert.ToBase64String(ms.ToArray());
            }
            return base64;
        }
        /// <summary>
        /// 在图片上添加文字
        /// </summary>
        /// <param name="g"><目标Graphics对象/param>
        /// <param name="drawPoint"></param>
        /// <param name="data">准备添加的字符串</param>
        private static void AddFont(Graphics grap, PointF drawPoint, string data,Color color)
        {
            SolidBrush mybrush = new SolidBrush(color);    //设置默认画笔颜色
            Font myfont = new Font("黑体", 16, FontStyle.Bold);   //设置默认字体格式   
            grap.DrawString(data, myfont, mybrush, drawPoint);  //图片上添加文字;
        }

        /// <summary>
        /// 在图片上添加文字
        /// </summary>
        /// <param name="g"><目标Graphics对象/param>
        /// <param name="drawPoint"></param>
        /// <param name="data">准备添加的字符串</param>
        private static void AddFont(Graphics grap, PointF drawPoint, string data, string font,int fontSize, Color color)
        {
            
            SolidBrush mybrush = new SolidBrush(color);    //设置默认画笔颜色
            Font myfont = new Font(font, fontSize, FontStyle.Bold);   //设置默认字体格式   
            grap.DrawString(data, myfont, mybrush, drawPoint);  //图片上添加文字;
        }

    }
}
