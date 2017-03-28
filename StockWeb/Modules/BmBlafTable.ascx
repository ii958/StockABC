<%@ Control Language="c#" AutoEventWireup="false" Codebehind="BmBlafTable.ascx.cs" Inherits="AISRS.WebUI.Modules.BmBlafTable" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:label id="labelJsFunctions" EnableViewState="False" runat="server"></asp:label><asp:linkbutton id="linkButtonSort" runat="server"></asp:linkbutton><input id="hiddenHighlightRowID" type="hidden" name="hiddenHighlightRowID" runat="server" />
<input id="hiddenSortColumnName" type="hidden" name="hiddenSortColumnName" runat="server" />
<input id="hiddenIsSortDesc" type="hidden" name="hiddenIsSortDesc" runat="server" />
<asp:label id="labelScript" EnableViewState="False" runat="server"></asp:label>
<table id="tableFrame" cellspacing="0" cellpadding="0" border="0" runat="server">
	<tbody>
		<tr>
			<td id="tableFrameTd" runat="server">
				<div id="divTitleFreeze" runat="server"></div>
				<div id="divTitle" runat="server"></div>
				<div id="divMainFreeze" runat="server"></div>
				<div id="divMain" runat="server"><asp:table id="table" EnableViewState="False" runat="server"></asp:table></div>
			</td>
		</tr>
	</tbody>
</table>
<asp:label id="labelJs" EnableViewState="False" runat="server"></asp:label>
