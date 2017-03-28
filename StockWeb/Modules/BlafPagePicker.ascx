<%@ Control Language="c#" AutoEventWireup="false" Codebehind="BlafPagePicker.ascx.cs" Inherits="AISRS.WebUI.Modules.BlafPagePicker" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table cellspacing="0" cellpadding="0" border="0">
	<tr>
		<td><asp:label id="labelPrePage" runat="server">Previous</asp:label></td>
		<td width="12"></td>
		<td><asp:dropdownlist id="dropDownListPageIndex" runat="server"></asp:dropdownlist></td>
		<td width="12"></td>
		<td><asp:label id="labelNextPage" runat="server">Next</asp:label></td>
	</tr>
</table>
<asp:linkbutton id="linkButtonPageChange" runat="server"></asp:linkbutton>
<input id="hiddenPageNumber" type="hidden" runat="server" />
