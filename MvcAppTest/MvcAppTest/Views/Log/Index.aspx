<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<MvcAppTest.Models.Log.CLog_WuQi>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="mark">
         <h3><%=Html.Encode("��ѯ���")%></h3>
         
        <% Html.BeginForm("SearchMarkInfo", "Log");
           {%>
        <p><%=Html.Encode("������������������")%><%=Html.TextBox("owner")%><input id='mark' type="submit" 
                style="width: 86px" value="�ύ" /></p>        
        <% Html.EndForm();}%>
    </div>
        <div id="msg">
         <h3><%=Html.Encode("��ѯLOG��Ϣ")%></h3>
         
        <% Html.BeginForm("SearchMsgInfo", "Log");
           {%>
        <p><%=Html.Encode("������������������")%><%=Html.TextBox("owner")%><input id='msg' type="submit" 
                style="width: 86px" value="�ύ" /></p>        
        <% Html.EndForm();
           }%>
    </div>

</asp:Content>

