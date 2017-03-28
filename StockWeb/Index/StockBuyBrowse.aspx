<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockBuyBrowse.aspx.cs" Inherits="AISRS.WebUI.Index.StockBuyBrowse" %>
<%@ Register TagPrefix="uc1" TagName="BmBlafTable" Src="../Modules/BmBlafTable.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SubHeader" Src="../Modules/SubHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Navigation" Src="../Modules/Navigation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../Modules/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LinkButton" Src="../Modules/LinkButton.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="../Modules/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DatePicker" Src="~/Modules/DatePicker.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html>
	<head>
		<title>股票系统--股票买入信号</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="C#"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<link href="../CSS/blaf-cn.css" type="text/css" rel="stylesheet" />
    </head>
    <body>
		<form id="Form1" method="post" runat="server">
			<uc1:Navigation id="Navigation1" runat="server"></uc1:Navigation>
            <input id="hiddenSelectedValue" type="hidden" name="hiddenSelectedValue" runat="server" />
            <input type="hidden" id="hiddenExportUrl" runat="server" name="hiddenExportUrl" />
			<uc1:Header id="Header1" runat="server" title="股票买入信号"></uc1:Header>
            <table id="table1" cellpadding="1" cellspacing="1" border="0" width="100%">
                <tr>
					<td width="12">&nbsp;</td>
					<td class="OraPromptText" width="80">查询日期</td>
					<td width="140"><uc1:datepicker id="DatePickerFrom" runat="server" Width="100"></uc1:datepicker></td>
					<td class="OraPromptText" width="85">查询类型</td>
					<td width="160"><asp:dropdownlist id="DropDownListType" Width="140" runat="server"></asp:dropdownlist></td>
					<td></td>
				</tr>
				<tr>
					<td></td>
					<td></td>
					<td><uc1:LinkButton id="LinkButtonQuery" runat="server" Text="查询"></uc1:LinkButton></td>					
                    <td></td>
                    <td><uc1:LinkButton id="LinkButtonForecast" runat="server" Text="统计"></uc1:LinkButton></td>
                    <td></td>
                    
				</tr>
            </table>
            <uc1:SubHeader id="SubHeader1" runat="server" title="股票买入信号列表"></uc1:SubHeader>
			<table id="table2" cellpadding="1" cellspacing="1" border="0" width="100%">
				<tr>
					<td width="30">&nbsp;</td>
					<td><uc1:BmBlafTable id="BmBlafTable" runat="server"></uc1:BmBlafTable></td>
					<td></td>
				</tr>
				<tr style="height:1px"><td colspan="3">&nbsp;</td></tr>
			</table>
            <uc1:Header id="Header2" runat="server"	></uc1:Header>
			<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
			<asp:Label ID="labScript" Runat="server"></asp:Label>
		</form>
	</body>
</html>
