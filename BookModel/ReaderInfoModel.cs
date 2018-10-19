using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookModel
{
    public class ReaderInfoModel
    {
        public ReaderInfoModel()
        { }
        #region Model
        private string _ReaderID;
        private string _Name;
        private int _Sex = 1;
        private int _MaxCount;
        private int _IsShow;
        /// <summary>
        /// 读者ID
        /// </summary>
        public string ReaderID
        {
            set { _ReaderID = value; }
            get { return _ReaderID; }
        }
        /// <summary>
        /// 读者姓名
        /// </summary>
        public string Name
        {
            set { Name = value; }
            get { return Name; }
        }
        /// <summary>
        /// 读者性别
        /// </summary>
        public int Sex
        {
            set { Sex = value; }
            get { return Sex; }
        }
        /// <summary>
        /// 最大借阅量
        /// </summary>
        public int MaxCount
        {
            set { _MaxCount = value; }
            get { return _MaxCount; }
        }
        /// <summary>
        /// 是否显示
        /// </summary>
        public int IsShow
        {
            set { _IsShow = value; }
            get { return _IsShow; }
        }
        #endregion Model
    }
}
