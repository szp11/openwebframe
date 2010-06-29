<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<MvcAppTest.Models.Log.CLog_WuQi>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="mark">
         <h3><%=Html.Encode("查询标记")%></h3>
         
        <% Html.BeginForm("SearchMarkInfo", "Log");
           {%>
        <p><%=Html.Encode("请输入所有者姓名：")%><%=Html.TextBox("owner")%><input id='mark' type="submit" 
                style="width: 86px" value="提交" /></p>        
        <% Html.EndForm();}%>
    </div>
        <div id="msg">
         <h3><%=Html.Encode("查询LOG信息")%></h3>
         
        <% Html.BeginForm("SearchMsgInfo", "Log");
           {%>
        <p><%=Html.Encode("请输入所有者姓名：")%><%=Html.TextBox("owner")%><input id='msg' type="submit" 
                style="width: 86px" value="提交" /></p>        
        <% Html.EndForm();
           }%>
    </div>

</asp:Content>

