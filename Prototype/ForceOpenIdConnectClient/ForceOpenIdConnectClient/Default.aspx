<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ForceOpenIdConnectClient.Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>ASP.NET webapplication with Keycloak authentication</h1>
    </div>
    
    <div class="row">
        <button runat="server" onServerClick="CreateUser_Click">Create a user</button>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Claims</h2>
            <table class="table col-md-4">
                <tr>
                    <th><b>Type</b></th>
                    <th><b>Value</b></th>
                </tr>
                <% foreach (var claim in Claims) { %>
                    <tr>
                        <td><%= claim.Type %></td>
                        <td><%= claim.Value %></td>
                    </tr>
                <% } %>
            </table>
        </div>
    </div>

</asp:Content>
