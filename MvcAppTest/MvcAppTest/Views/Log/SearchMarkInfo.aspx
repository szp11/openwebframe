<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<MvcAppTest.Models.Log.CLogMarkInfo>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	SearchMarkInfo
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>SearchMarkInfo</h2>

    <table>
        <tr>
            <th></th>
            <th>
                i_Guid
            </th>
            <th>
                s_Owner
            </th>
            <th>
                s_Desc
            </th>
            <th>
                i_IsOpen
            </th>
            <th>
                d_CreateTime
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.ActionLink("Edit", "Edit", new { /* id=item.PrimaryKey */ }) %> |
                <%= Html.ActionLink("Details", "Details", new { /* id=item.PrimaryKey */ })%>
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
                <%= Html.Encode(item.i_IsOpen) %>
            </td>
            <td>
                <%= Html.Encode(String.Format("{0:g}", item.d_CreateTime)) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

