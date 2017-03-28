<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockQueryBrowse.aspx.cs" Inherits="AISRS.WebUI.DataCenter.StockQueryBrowse" %>
<%@ Register TagPrefix="uc1" TagName="BmBlafTable" Src="../Modules/BmBlafTable.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SubHeader" Src="../Modules/SubHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Navigation" Src="../Modules/Navigation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../Modules/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LinkButton" Src="../Modules/LinkButton.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="../Modules/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DropDownListYearMonthPicker" Src="../Modules/DropDownListYearMonthPicker.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>股票系统--实时查询</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="C#"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<link href="../CSS/blaf-cn.css" type="text/css" rel="stylesheet" />
		<script type="text/javascript">
		    
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:Navigation id="Navigation1" runat="server"></uc1:Navigation>
			<uc1:Header id="Header1" runat="server" title="股价实时查询"></uc1:Header>
			<input type="hidden" id="HiddenStockID" runat="server" value="" />
			<table id="table1" cellpadding="1" cellspacing="1" border="0" width="100%">	
                <tr>
					<td width="12"></td>
					<td class="OraPromptText" width="85">证券市场&nbsp;</td>
					<td width="110"><asp:dropdownlist id="DropDownListCategory" Width="100" runat="server"></asp:dropdownlist></td>
					<td class="OraPromptText" width="100">股票代码&nbsp;</td>
					<td width="160"><asp:textbox id="txtStockCode" runat="server" Width="100" MaxLength="8"></asp:textbox></td>
					<td></td>
				</tr>		
                <tr>
					<td width="12"></td>
					<td class="OraPromptText" width="85">所属行业&nbsp;</td>
					<td width="110"><asp:dropdownlist id="DropDownListField" Width="100" runat="server"></asp:dropdownlist></td>
					<td class="OraPromptText" width="100">省份&nbsp;</td>
					<td width="160"><asp:dropdownlist id="DropDownListProvince" Width="100" runat="server"></asp:dropdownlist></td>
					<td></td>
				</tr>	
				<tr>
					<td></td>
					<td></td>
					<td><uc1:LinkButton id="LinkButtonQuery" runat="server" Text="查询"></uc1:LinkButton></td>
					<td></td>
                    <td></td>
                    <td></td>
				</tr>
			</table>
			<uc1:SubHeader id="SubHeader1" runat="server" title="股票列表"></uc1:SubHeader>
			<table id="table2" cellpadding="1" cellspacing="1" border="0" width="100%">
				<tr>
					<td width="30">&nbsp;</td>
					<td><uc1:BmBlafTable id="BmBlafTableStock" runat="server"></uc1:BmBlafTable></td>
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
