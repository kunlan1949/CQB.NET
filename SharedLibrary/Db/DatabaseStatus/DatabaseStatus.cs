using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace Db.Bot
{
    /// <summary></summary>
    [Serializable]
    [DataObject]
    [BindTable("database_status", Description = "", ConnName = "BotDb", DbType = DatabaseType.None)]
    public partial class DatabaseStatus
    {
        #region 属性
        private String _Status;
        /// <summary></summary>
        [DisplayName("Status")]
        [DataObjectField(false, false, false, 255)]
        [BindColumn("status", "", "varchar(255)")]
        public String Status { get => _Status; set { if (OnPropertyChanging("Status", value)) { _Status = value; OnPropertyChanged("Status"); } } }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public override Object this[String name]
        {
            get
            {
                switch (name)
                {
                    case "Status": return _Status;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case "Status": _Status = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得DatabaseStatus字段信息的快捷方式</summary>
        public partial class _
        {
            /// <summary></summary>
            public static readonly Field Status = FindByName("Status");

            static Field FindByName(String name) => Meta.Table.FindByName(name);
        }

        /// <summary>取得DatabaseStatus字段名称的快捷方式</summary>
        public partial class __
        {
            /// <summary></summary>
            public const String Status = "Status";
        }
        #endregion
    }
}