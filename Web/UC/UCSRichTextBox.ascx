<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCSRichTextBox.ascx.cs" Inherits="YX.Web.UC.UCSRichTextBox" %>
<script type="text/plain" id="<%= this.ClientID %>_myEditor" style="width:<%= this.Width %>px; height:<%= this.Height %>px;">
</script>
<asp:HiddenField ID="hidMyEditor" runat="server" />
<asp:HiddenField ID="hidWidthHeight" runat="server" />
