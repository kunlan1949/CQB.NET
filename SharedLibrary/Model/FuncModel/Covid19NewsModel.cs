using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Model.FuncModel
{
    internal class Covid19NewsModel
    {
        /// <summary>
        /// 当前确诊人数
        /// </summary>
        public string presentNumber { get; set; }
        /// <summary>
        /// 累计确诊人数
        /// </summary>
        public string definiteNumber { get; set; }
        /// <summary>
        /// 累计死亡人数
        /// </summary>
        public string dieNumber { get; set; }
        /// <summary>
        /// 累计治愈人数
        /// </summary>
        public string cureNumber { get; set; }
        /// <summary>
        /// 当日新增人数
        /// </summary>
        public string sureNewNumber { get; set; }
        /// <summary>
        /// 当日新增无症状病例数
        /// </summary>
        public string sureNewHidNumber { get; set; }
        /// <summary>
        /// 中风险区数量
        /// </summary>
        public string midRankNumber { get; set; }
        /// <summary>
        /// 高风险区数量
        /// </summary>
        public string highRankNumber { get; set; }
        /// <summary>
        /// 统计截止时间
        /// </summary>
        public string newsTime { get; set; }
    }
}
