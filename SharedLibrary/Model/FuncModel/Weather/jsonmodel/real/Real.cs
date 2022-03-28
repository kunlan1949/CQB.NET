using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weather.jsonmodel;
using Weather.jsonmodel.real;

namespace Weather.jsonmodel
{
    public class Real
    {
        /// <summary>
        /// 
        /// </summary>
        public Station station { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string publish_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Weather weather { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Wind wind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Warn warn { get; set; }
    }
}
