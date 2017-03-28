<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockHistoryBrowse.aspx.cs" Inherits="AISRS.WebUI.DataCenter.StockHistoryBrowse" %>
<%@ Register TagPrefix="uc1" TagName="BmBlafTable" Src="../Modules/BmBlafTable.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SubHeader" Src="../Modules/SubHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Navigation" Src="../Modules/Navigation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../Modules/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LinkButton" Src="../Modules/LinkButton.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="../Modules/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DatePicker" Src="~/Modules/DatePicker.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DropDownListYearMonthPicker" Src="../Modules/DropDownListYearMonthPicker.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>股票系统--历史数据查询</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="C#"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<link href="../CSS/blaf-cn.css" type="text/css" rel="stylesheet" />
		<script type="text/javascript">
		    function ExportExcel() {
		        var market = document.getElementById("<%= DropDownListCategory.ClientID%>");
		        var field = document.getElementById("<%= DropDownListField.ClientID%>");
		        var chart = document.getElementById("<%= DropDownListChart.ClientID%>");
		        var province = document.getElementById("<%= DropDownListProvince.ClientID%>");
		        var stockCode = document.getElementById("<%= txtStockCode.ClientID%>");
		        var stockName = document.getElementById("<%= txtStockName.ClientID%>");
		        var dateFrom = document.getElementById("<%= DatePickerFrom.ClientID%>_textBoxDate");
		        var dateTo = document.getElementById("<%= DatePickerTo.ClientID%>_textBoxDate");

		        var hiddenExportUrl = document.getElementById("<%= hiddenExportUrl.ClientID%>");

		        var url = hiddenExportUrl.value + "?stockcode=" + encodeURIComponent(stockCode.value)
                + "&stockName=" + encodeURIComponent(stockName.value)
				+ "&market=";
		        if (market.length > 0 && market.value.length > 0) {
		            url += encodeURIComponent(market.value);
		        }
		        if (field != null) {
		            if (field.value.length > 0) {
		                url += "&field=" + encodeURIComponent(field.value);
		            }
		        }
		        if (chart != null) {
		            if (chart.value.length > 0) {
		                url += "&chart=" + encodeURIComponent(chart.value);
		            }
		        }
		        if (province != null) {
		            if (province.value.length > 0) {
		                url += "&province=" + encodeURIComponent(province.value);
		            }
		        }
		        url += "&datefrom=" + encodeURIComponent(dateFrom.value)
					+ "&dateto=" + encodeURIComponent(dateTo.value)
					+ "&MarketName=" + encodeURIComponent(market.options[market.selectedIndex].text)
					+ "&FieldName=" + encodeURIComponent(field.options[field.selectedIndex].text)
                    + "&ChartName=" + encodeURIComponent(chart.options[chart.selectedIndex].text)
					+ "&ProvinceName=" + encodeURIComponent(province.options[province.selectedIndex].text);

		        window.open(url);

		        return false;
		    }
		</script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:Navigation id="Navigation1" runat="server"></uc1:Navigation>
			<uc1:Header id="Header1" runat="server" title="历史数据查询"></uc1:Header>
            <input type="hidden" id="hiddenExportUrl" runat="server" name="hiddenExportUrl" />
			<table id="table1" cellpadding="1" cellspacing="1" border="0" width="100%">
                <tr>
					<td width="12"></td>
					<td class="OraPromptText" width="85">证券市场&nbsp;</td>
					<td width="140"><asp:dropdownlist id="DropDownListCategory" Width="100" runat="server"></asp:dropdownlist></td>
					<td class="OraPromptText" width="100">股票代码&nbsp;</td>
					<td width="160"><asp:textbox id="txtStockCode" runat="server" Width="100" MaxLength="8"></asp:textbox></td>
					<td></td>
				</tr>
                <tr>
					<td width="12"></td>
					<td class="OraPromptText" width="85">股票名称&nbsp;</td>
					<td width="140"><asp:textbox id="txtStockName" runat="server" Width="100" MaxLength="8"></asp:textbox></td>
					<td class="OraPromptText" width="100">线形图&nbsp;</td>
					<td width="160"><asp:dropdownlist id="DropDownListChart" Width="100" runat="server"></asp:dropdownlist></td>
					<td></td>
				</tr>
                <tr>
					<td width="12"></td>
					<td class="OraPromptText" width="85">所属行业&nbsp;</td>
					<td width="140"><asp:dropdownlist id="DropDownListField" Width="100" runat="server"></asp:dropdownlist></td>
					<td class="OraPromptText" width="100">省份&nbsp;</td>
					<td width="160"><asp:dropdownlist id="DropDownListProvince" Width="100" runat="server"></asp:dropdownlist></td>
					<td></td>
				</tr>
                <tr>
					<td width="12">&nbsp;</td>
					<td class="OraPromptText" width="80">开始日期</td>
					<td width="140"><uc1:datepicker id="DatePickerFrom" runat="server" Width="100"></uc1:datepicker></td>
					<td class="OraPromptText" width="100">结束日期&nbsp;</td>
					<td width="160"><uc1:datepicker id="DatePickerTo" runat="server" Width="100"></uc1:datepicker></td>
					<td></td>
				</tr>
                <tr>
					<td></td>
					<td class="OraPromptText">按增长率排序</td>
					<td colspan="2" width="160"><asp:CheckBoxList ID="CheckBoxOrder" Width="160" Runat="server" RepeatColumns="6" Font-Size="10">
							<asp:ListItem Text="降序" Value="desc" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="升序" Value="asc"></asp:ListItem>
						</asp:CheckBoxList></td>
                    <td></td>
                    <td></td>
				</tr>
				<tr>
					<td></td>
					<td></td>
					<td><uc1:LinkButton id="LinkButtonQuery" runat="server" Text="查询"></uc1:LinkButton></td>					
                    <td></td>
                    <td><uc1:LinkButton id="LinkButtonExport" runat="server" Text="导出EXCEL"></uc1:LinkButton></td>
                    <td></td>
                    
				</tr>
			</table>
            <uc1:SubHeader id="SubHeader1" runat="server" title="股票历史记录列表"></uc1:SubHeader>
			<table id="table2" cellpadding="1" cellspacing="1" border="0" width="100%">
				<tr>
					<td width="30">&nbsp;</td>
					<td><uc1:BmBlafTable id="BmBlafTableHistory" runat="server"></uc1:BmBlafTable></td>
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
