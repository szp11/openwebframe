<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CPagerQuery<CPagerInfo,IEnumerable<MvcAppTest.Models.Log.CLogMsgInfo>>>" %>
<%@ Import  Namespace="MvcAppTest.Helper.HtmlHelpers.pagerhelper" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ListMsgInfos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


        <% Html.BeginForm("ManagerMsg", "Log");{%>
    <script src="../../Scripts/CheckBoxAll.js" type="text/javascript"></script>

    <h2>ListMsgInfo</h2>
    <div>
    <p>
    <input type="submit" style="width: 86px" value="删除"  name="delete" /> </p>                
    </div>
    <div>
    <table>
        <tr>
            <th><input type='checkbox' name='all' onclick='check_all(this,"Guid")'/></th>
            <th>
                <%=Html.Encode("索引")%>
            </th>
            <th>
                <%=Html.Encode("所属标记")%>
            </th>
            <th>
                <%=Html.Encode("描述") %>
            </th>
            <th>
                <%=Html.Encode("类型") %>
            </th>

            <th>
                <%=Html.Encode("所有者")%>
            </th>
            <th>
                <%= Html.Encode("创建时间")%>
            </th>
        </tr>

    <% foreach (var item in Model.EntityList) { %>
    
        <tr>
            <td>
            <%=Html.CheckBox("Guid", new { value = item.ui_id.ToString() })%>
            </td>
            <td>
                <%= Html.Encode(item.ui_id)%>
            </td>
            <td>
                <%= Html.Encode(item.str_mark) %>
            </td>
            <td>
                <%= Html.Encode(item.str_logmsg) %>
            </td>
            <td>
                <%= Html.Encode(item.str_logtype) %>
            </td>
            <td>
                <%= Html.Encode(item.str_logowner) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:g}", item.d_logtime)) %>
            </td>
        </tr>
    
    <% } %>

    </table>
    </div>
    <div>
        <p>
       <%=Html.Pager("pager",Model.Pager.CurrentPageIndex,Model.Pager.PageSize,Model.Pager.RecordCount,PageMode.Numeric) %>
    </p>

    </div>

        <% Html.EndForm();}%>

</asp:Content>
