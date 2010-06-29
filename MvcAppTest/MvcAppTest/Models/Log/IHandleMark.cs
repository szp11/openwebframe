using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcAppTest.Models.Log
{
    public interface IHandleMark_WuQi
    {
        /// <summary>
        /// 放置信息
        /// </summary>
        /// <param name="smsg">消息内容，要求唯一</param>
        /// <param name="stype">消息类型，目前约定两类分别用字符串“debug”“run”表示</param>
        /// <param name="sowner">消息所有者</param>
        void TriggerLogMsg(string smsg,string stype, string sowner);
        int SynchronousAllRecord();
        List<CLogMark_WuQi> GetAllRecords(int adapter,string markowner);
        void ChangeMarkState(int id);
        int DeleteMarks(List<uint> ids);
    }
}
