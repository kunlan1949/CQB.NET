﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Helper
{
    class FortuneHelper
    {
        private static Dictionary<string, int> Sign = new Dictionary<string, int>
        {
            {"白羊", 0},
            {"金牛", 1},
            {"双子", 2},
            {"巨蟹", 3},
            {"狮子", 4},
            {"处女", 5},
            {"天秤", 6},
            {"天蝎", 7},
            {"射手", 8},
            {"魔羯", 9},
            {"水瓶", 10},
            {"双鱼", 11},
        };
        public static int SignDic(string target)
        {
            var value = 0;
            if (Sign.ContainsKey(target))
            {
                value = Sign[target];
            }
            else
            {
                value = -1;
            }
            return value;
        }
    }
}
