<%@ Page language="c#" Codebehind="ErrorPage.aspx.cs" AutoEventWireup="false" Inherits="AISRS.WebUI.ErrorPage" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Modules/Footer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>ErrorPage</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</head>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="GridLayout">
		<form id="FormErrorPage" method="post" runat="server">
			<table height="70%" width="80%" border="0">
				<tr>
					<td align="center" valign="middle">
						<table style="BORDER-RIGHT: deepskyblue 1px solid; BORDER-TOP: deepskyblue 1px solid; BORDER-LEFT: deepskyblue 1px solid; BORDER-BOTTOM: deepskyblue 1px solid"
							cellpadding="100">
							<tr align="center" valign="middle">
								<td>
									<table border="0">
										<tr>
											<td align="right">
												<img src="/AISRS/Images/warning.gif" border="0" />
											</td>
											<td align="left"><asp:label id="labelErrorMessage" runat="server" ForeColor="#ff0000" Font-Size="15px"></asp:label>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</html>
