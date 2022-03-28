using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Model.FuncModel
{
    internal class TransParamModel
    {
        public string Salt { get; set; }
        public string Sign { get; set; }
        public string Ts { get; set; }
        public string Bv { get; set; }
    }
}
