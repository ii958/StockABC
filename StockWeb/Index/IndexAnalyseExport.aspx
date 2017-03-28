<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndexAnalyseExport.aspx.cs" Inherits="AISRS.WebUI.Index.IndexAnalyseExport" contentType="application/vnd.ms-excel" %>
<%@ Register TagPrefix="uc1" TagName="BmBlafTable" Src="../Modules/BmBlafTable.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>股票系统--导出技术指标</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		<meta content="text/html; charset=gb2312" name="vs_targetSchema" http-equiv="Content-Type"/>
		<style type="text/css">
		    td,th {font-size: 78%;}		
		</style>
	</head>
	<body style="margin-left:0; margin-top:0;">
		<form id="Form1" method="post" runat="server">
			<table width="400" cellpadding="0" cellspacing="0" border="0">
				<tr bgcolor="maroon" style="width:100%" >
					<td colspan="3" >
						<b><asp:Label ID="LabelTitle" Runat="server" Width="200"  ForeColor="White"></asp:Label></b>
					</td>
				</tr>
				<tr><td width="100" height="1">&nbsp;</td><td width="100">&nbsp;</td><td></td></tr>
			</table>
			<uc1:BmBlafTable id="BmBlafTable" runat="server"></uc1:BmBlafTable>
		</form>
	</body>
</html>
