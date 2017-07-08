<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCPBConfig.ascx.cs" Inherits="YX.Web.Admin.Glo.UC.UCPBConfig" %>


<div id="divDataEditor">
    <dl class="de_field">
        <dt><span>*</span> 站点：</dt>
        <dd>
			<jk:MyTextBox runat="server" ID="txtApplicationType" MaxLength="25" ClientIDMode="Static" ></jk:MyTextBox>
             <asp:HiddenField ID="hidApplicationType" ClientIDMode="Static" runat="server" />
        </dd>
    </dl>
    <dl class="de_field">
        <dt><span>*</span> 屏蔽类别</dt>
        <dd>
			<asp:DropDownList runat="server" ID="drPbType" >
			</asp:DropDownList>
			<asp:RequiredFieldValidator ID="rfvdrPbType" runat="server" ControlToValidate="drPbType" Display="None" ErrorMessage="屏蔽类别 不能为空！"> 
			</asp:RequiredFieldValidator> 
         <asp:RegularExpressionValidator ID="revdrPbType" runat="server" ControlToValidate="drPbType" Display="None" ErrorMessage="屏蔽类别 必须介于0 - 255之间！" ValidationExpression="(^[0-9]$)|(^[1-9]\d$)|(^[1-2][0-5][0-5]$)"> 
         </asp:RegularExpressionValidator> 
        </dd>
    </dl>
    <dl class="de_field">
        <dt><span>*</span> 主键：</dt>
        <dd>
			<jk:MyTextBox runat="server" PlaceHolder="请填写对应的ID值" ID="txtKeyData" >
			</jk:MyTextBox>
			<asp:RequiredFieldValidator ID="rfvtxtKeyData" runat="server" ControlToValidate="txtKeyData" Display="None" ErrorMessage="主键 不能为空！"> 
			</asp:RequiredFieldValidator> 
         <asp:RegularExpressionValidator ID="revtxtKeyData" runat="server" ControlToValidate="txtKeyData" Display="None" ErrorMessage="主键 必须为整数！" ValidationExpression="^-?[1-9]\d*$"> 
         </asp:RegularExpressionValidator> 
        </dd>
    </dl>
    <ol>
        <jk:MyButton ID="btAccept" runat="server" Text="确定" />
        &nbsp;&nbsp;
    </ol>
</div>
