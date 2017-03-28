<%@ Register TagPrefix="uc1" TagName="DatePicker" Src="~/Modules/DatePicker.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Navigation" Src="~/Modules/Navigation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HyperLinkButton" Src="~/Modules/HyperLinkButton.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LinkButton" Src="~/Modules/LinkButton.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BmBlafTable" Src="~/Modules/BmBlafTable.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SubHeader" Src="~/Modules/SubHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="~/Modules/Header.ascx" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockImport.aspx.cs" Inherits="AISRS.WebUI.DataCenter.StockImport" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="../Modules/Footer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>股票系统--导数</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="C#"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<link href="../CSS/blaf-cn.css" type="text/css" rel="stylesheet" />
		<script type="text/javascript">
		    function SubmitClick() {
		        var Importdate = document.getElementById("<%= DatePickerImportDate.ClientID%>_textBoxDate");
		        if (Importdate.value == "" || Importdate.value.length == 0) {
		            Importdate.value == ""
		            alert("导数日期为必选项，必须输入!");
		            return false;
		        }
            }
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:Navigation id="Navigation1" runat="server"></uc1:Navigation>
			<uc1:Header id="Header1" runat="server" title="导数"></uc1:Header>
			<input type="hidden" id="HiddenStockID" runat="server" value="" />
			<table id="table1" cellpadding="1" cellspacing="1" border="0" width="100%">
				<tr>
					<td width="20">&nbsp;</td>
					<td class="OraPromptText" width="80">导数日期</td>
					<td><uc1:datepicker id="DatePickerImportDate" runat="server"></uc1:datepicker></td>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td></td>
                    <td></td>					
					<td><uc1:LinkButton id="LinkButtonImport" runat="server" Text="导数"></uc1:LinkButton></td>                   
					<td></td>
				</tr>
			</table>
			<uc1:SubHeader id="SubHeader1" runat="server" title="导数列表"></uc1:SubHeader>
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
