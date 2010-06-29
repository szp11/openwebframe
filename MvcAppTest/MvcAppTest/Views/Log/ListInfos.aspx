<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CPagerQuery<CPagerInfo,IEnumerable<MvcAppTest.Models.Log.CLogMarkInfo>>>" %>
<%@ Import  Namespace="MvcAppTest.Helper.HtmlHelpers.pagerhelper" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ListMarkInfo
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        <% Html.BeginForm("ManagerMark", "Log");{%>
    <script src="../../Scripts/CheckBoxAll.js" type="text/javascript"></script>

    <h2>ListMarkInfo</h2>
    <div>
    <p>
    <input name="switch"  type="submit" style="width: 86px" value="�л�״̬" />
    </p>                
    </div>
    <div>
    <table>
        <tr>
            <th><input type='checkbox' name='all' onclick='check_all(this,"Guid")'/></th>
            <th>
                <%=Html.Encode("����")%>
            </th>
            <th>
                <%=Html.Encode("������")%>
            </th>
            <th>
                <%=Html.Encode("����") %>
            </th>
            <th>
                <%=Html.Encode("״̬") %>
            </th>
            <th>
                <%= Html.Encode("����ʱ��")%>
            </th>
        </tr>

    <% foreach (var item in Model.EntityList) { %>
    
        <tr>
            <td>
            <%=Html.CheckBox("Guid",new { value=item.i_Guid.ToString()})%>
            </td>
            <td>
                <%= Html.Encode(item.i_Guid) %>
            </td>
            <td>
                <%= Html.Encode(item.s_Owner) %>
            </td>
            <td>
                <%= Html.Encode(item.s_Desc) %>
            </td>
            <td>                
                <% if (0 == item.i_IsOpen)
                   { %><%= Html.Encode("�ر�") %> <%}
                   else
                   { %><%= Html.Encode("��") %><%} %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:g}", item.d_CreateTime)) %>
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
