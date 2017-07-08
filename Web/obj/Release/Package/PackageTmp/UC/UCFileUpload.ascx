<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCFileUpload.ascx.cs"
    Inherits="YX.Web.UC.UCFileUpload" ClientIDMode="AutoID" %>
<div id='<%= this.ClientID %>' class="myUploader_9568536">
    <p class="myUploader_9568536_icon" id="divUpload" runat="server">上传文件</p>
    <ul id="divUploadFiles" runat="server" class="myUploader_9568536_Contaier">
    </ul>
    <asp:HiddenField ID="hidUploadFiles" runat="server" />
    <asp:HiddenField ID="hidMaXNumberUpload" runat="server" />
    <asp:HiddenField ID="hidUploadPath" runat="server" />
    <asp:HiddenField ID="hidDymicFolderName" runat="server" />
    <asp:HiddenField ID="hidModified" runat="server" />
    <asp:HiddenField ID="hidThumbSize" runat="server" />
    <asp:HiddenField ID="hidCheckedFile" runat="server" />
    <asp:HiddenField ID="hidIsMakeWatermark" runat="server" />
    <asp:HiddenField ID="hidIsUploadToImageServer" runat="server" />
    <asp:HiddenField ID="hidIsShowInsertButton" runat="server" />
    <asp:HiddenField ID="hidTargetEditor" runat="server" />
    <asp:HiddenField ID="hidScaleImageSize" runat="server" />
    <asp:HiddenField ID="hidIsShowCheckBox" runat="server" />
    <asp:HiddenField ID="hidCheckedFiles" runat="server" />
</div>