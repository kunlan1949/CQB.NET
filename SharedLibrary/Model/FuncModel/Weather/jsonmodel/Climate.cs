﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.jsonmodel.climate;

namespace Weather.jsonmodel
{
    public class Climate
    {
        /// <summary>
        /// 1981年-2010年
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<MonthItem> month { get; set; }
    }
}
