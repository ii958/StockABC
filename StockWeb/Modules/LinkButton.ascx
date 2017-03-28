<%@ Control Language="c#" AutoEventWireup="false" Codebehind="LinkButton.ascx.cs" Inherits="AISRS.WebUI.Modules.LinkButton" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<div style="<%=_visible%>">
<table cellspacing="0" cellpadding="0" border="0">
	<tr>
		<td valign="top" align="right" width="17" height="25" rowspan="4"><img alt="" height="25" hspace="0" src="<%=_applicationPath%>/images/button_left.gif" width="17" vspace="0"
					border="0" /></td>
		<td valign="top" align="left" bgcolor="#999966" height="1"><img alt="" height="1" src="<%=_applicationPath%>/Images/button_top.gif" border="0" /></td>
		<td valign="top" align="left" width="17" height="25" rowspan="4"><img alt="" height="25" src="<%=_applicationPath%>/Images/button_right.gif" width="17" border="0" /></td>
	</tr>
	<tr>
		<td class="OraBGAccentLight OraNav6Selected" valign="middle" align="left" height="22"
			nowrap="nowrap">
			<asp:LinkButton ID="LinkButtonAction" runat="server"></asp:LinkButton>
		</td>
	</tr>
	<tr>
		<td valign="top" align="left" bgcolor="#666633" height="1"><img alt="" height="1" src="<%=_applicationPath%>/Images/button_bottom1.gif" border="0" /></td>
	</tr>
	<tr>
		<td valign="top" align="left" bgcolor="#333300" height="1"><img alt="" height="1" src="<%=_applicationPath%>/Images/button_bottom2.gif" border="0" /></td>
	</tr>
</table>
</div>