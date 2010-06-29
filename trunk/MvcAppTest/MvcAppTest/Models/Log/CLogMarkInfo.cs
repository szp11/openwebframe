using System;
using System.Data;
using System.Configuration;
using System.Linq;


namespace MvcAppTest.Models.Log
{
    public class CLogMarkInfo
    {        
        public uint i_Guid{get;set;}
        public string s_Owner { get; set; }
        public string s_Desc { get; set; }
        public int i_IsOpen {get;set;}
        public DateTime d_CreateTime{get;set;}

    }
}
