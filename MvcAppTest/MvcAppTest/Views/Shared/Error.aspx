<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="errorTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Error
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2 style="color:Red">
        <%=ViewData["msg"] %>
    </h2>
    <p>
        <input id="Button1" type="button" value="返回上一级" onclick="javascript:history.go(-1);" /></p>
</asp:Content>
