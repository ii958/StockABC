<%@ Control Language="c#" AutoEventWireup="false" Codebehind="HyperLinkAndLinkButton.ascx.cs" Inherits="AISRS.WebUI.Modules.HyperLinkAndLinkButton" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table cellspacing="0" cellpadding="0" border="0">
	<tr>
		<td>
			<div>
				<table cellspacing="0" cellpadding="0" border="0">
					<tr>
						<td valign="top" align="right" width="17" height="25" rowspan="4"><img alt="" height="25" hspace="0" src="<%=_applicationPath%>/images/button_left.gif" width="17" vspace="0"
									border="0" /></td>
						<td valign="top" align="left" bgcolor="#999966" height="1"><img alt="" height="1" src="<%=_applicationPath%>/images/button_top.gif" border="0" /></td>
						<td valign="top" align="left" width="17" height="25" rowspan="4"><img alt="" height="25" src="<%=_applicationPath%>/images/button_right.gif" width="17" border="0" /></td>
					</tr>
					<tr>
						<td class="OraBGAccentLight OraNav6Selected" valign="middle" align="left" height="22"
							nowrap="nowrap">
							<asp:HyperLink ID="HyperLinkAction" runat="server" >取消</asp:HyperLink>
						</td>
					</tr>
					<tr>
						<td valign="top" align="left" bgcolor="#666633" height="1"><img alt="" height="1" src="<%=_applicationPath%>/images/button_bottom1.gif" border="0" /></td>
					</tr>
					<tr>
						<td valign="top" align="left" bgcolor="#333300" height="1"><img alt="" height="1" src="<%=_applicationPath%>/images/button_bottom2.gif" border="0" /></td>
					</tr>
				</table>
			</div>
		</td>
		<td width="5"><img alt="" src="<%=_applicationPath%>/Images/s.gif" width="5" border="0" /></td>
		<td>
			<div>
				<table cellspacing="0" cellpadding="0" border="0">
					<tr>
						<td valign="top" align="right" width="17" height="25" rowspan="4"><img alt="" height="25" hspace="0" src="<%=_applicationPath%>/images/button_left.gif" width="17" vspace="0"
									border="0" /></td>
						<td valign="top" align="left" bgcolor="#999966" height="1"><img alt="" height="1" src="<%=_applicationPath%>/images/button_top.gif" border="0" /></td>
						<td valign="top" align="left" width="17" height="25" rowspan="4"><img alt="" height="25" src="<%=_applicationPath%>/images/button_right.gif" width="17" border="0" /></td>
					</tr>
					<tr>
						<td class="OraBGAccentLight OraNav6Selected" valign="middle" align="left" height="22"
							nowrap="nowrap">
							<asp:LinkButton ID="LinkButtonAction" runat="server">确定</asp:LinkButton>
						</td>
					</tr>
					<tr>
						<td valign="top" align="left" bgcolor="#666633" height="1"><img alt="" height="1" src="<%=_applicationPath%>/images/button_bottom1.gif" border="0" /></td>
					</tr>
					<tr>
						<td valign="top" align="left" bgcolor="#333300" height="1"><img alt="" height="1" src="<%=_applicationPath%>/images/button_bottom2.gif" border="0" /></td>
					</tr>
				</table>
			</div>
		</td>
	</tr>
</table>
