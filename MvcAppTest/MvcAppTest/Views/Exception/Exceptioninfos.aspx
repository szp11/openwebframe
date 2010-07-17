<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CPagerQuery<CPagerInfo,IEnumerable<MvcAppTest.Models.Exception.CExceptionInfo_WuQi>>>" %>
<%@ Import  Namespace="MvcAppTest.Helper.HtmlHelpers.pagerhelper" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Exceptioninfos
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <% Html.BeginForm("ManagerException", "Exception");{%>
            <script src="../../Scripts/CheckBoxAll.js" type="text/javascript"></script>

    <h2>Exceptioninfos</h2>
    <div>
    <p>
    <input name="delete"  type="submit" style="width: 86px" value="�������" />
    </p>                
    </div>
    <div>
    <table>
        <tr>
            <th><input type='checkbox' name='all' onclick='check_all(this,"Guid")'/></th>
            <th>
                <%=Html.Encode("ȫ��")%>
            </th>
            <th>
                <%=Html.Encode("��Ϣ")%>
            </th>
            <th>
                <%=Html.Encode("����") %>
            </th>
            <th>
                <%=Html.Encode("��Դ") %>
            </th>
            <th>
                <%= Html.Encode("����ʱ��")%>
            </th>
        </tr>

    <% foreach (var item in Model.EntityList) { %>
    
        <tr>
            <td>
            <%=Html.CheckBox("Guid",new { value=item.i_id.ToString()})%>
            </td>
            <td>
                <%= Html.Encode(item.s_fullname) %>
            </td>
            <td>
                <%= Html.Encode(item.s_msg) %>
            </td>
            <td>
                <%= Html.Encode(item.s_basefullname) %>
            </td>
                        <td>
                <%= Html.Encode(item.s_trace) %>
            </td>

            <td>
                <%= Html.Encode(String.Format("{0:g}", item.d_time)) %>
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
