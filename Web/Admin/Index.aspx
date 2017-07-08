<%@ Page Title="首页" Language="C#" MasterPageFile="~/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="YX.Web.Admin.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cpHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpBody" runat="server">
    <iframe id="fraMain" frameborder="0" scrolling="no" src="" style="width: 100%; height: 500px;" marginheight="0" name="fraMain" onload="resetMainframeHeight('fraMain');">
    </iframe>
</asp:Content>
