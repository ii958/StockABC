<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IndexAnalyseBrowse.aspx.cs" Inherits="AISRS.WebUI.Index.IndexAnalyseBrowse" %>
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
		<title>股票系统--技术指标分析</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="C#"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<link href="../CSS/blaf-cn.css" type="text/css" rel="stylesheet" />
        <script type="text/javascript">
            function ExportExcel() {
		        var market = document.getElementById("<%= DropDownListCategory.ClientID%>");
		        var field = document.getElementById("<%= DropDownListField.ClientID%>");
		        var index = document.getElementById("<%= DropDownListIndex.ClientID%>");
		        var province = document.getElementById("<%= DropDownListProvince.ClientID%>");
		        var stockCode = document.getElementById("<%= txtStockCode.ClientID%>");
		        var stockName = document.getElementById("<%= txtStockName.ClientID%>");
		        var dateFrom = document.getElementById("<%= DatePickerFrom.ClientID%>_textBoxDate");

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
		        if (index != null) {
		            if (index.value.length > 0) {
		                url += "&index=" + encodeURIComponent(index.value);
		            }
		        }
		        if (province != null) {
		            if (province.value.length > 0) {
		                url += "&province=" + encodeURIComponent(province.value);
		            }
		        }
		        url += "&datefrom=" + encodeURIComponent(dateFrom.value)
					+ "&MarketName=" + encodeURIComponent(market.options[market.selectedIndex].text)
					+ "&FieldName=" + encodeURIComponent(field.options[field.selectedIndex].text)
                    + "&IndexName=" + encodeURIComponent(index.options[index.selectedIndex].text)
					+ "&ProvinceName=" + encodeURIComponent(province.options[province.selectedIndex].text);

		        window.open(url);

		        return false;
		    }
        </script>
    </head>
    <body>
		<form id="Form1" method="post" runat="server">
			<uc1:Navigation id="Navigation1" runat="server"></uc1:Navigation>
            <input id="hiddenSelectedValue" type="hidden" name="hiddenSelectedValue" runat="server" />
            <input type="hidden" id="hiddenExportUrl" runat="server" name="hiddenExportUrl" />
			<uc1:Header id="Header1" runat="server" title="技术指标分析"></uc1:Header>
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
					<td class="OraPromptText" width="100">技术指标&nbsp;</td>
					<td width="160"><asp:dropdownlist id="DropDownListIndex" Width="100" runat="server"></asp:dropdownlist></td>
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
					<td class="OraPromptText" width="80">查询日期</td>
					<td width="140"><uc1:datepicker id="DatePickerFrom" runat="server" Width="100"></uc1:datepicker></td>
					<td></td>
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
            <uc1:SubHeader id="SubHeader1" runat="server" title="指标分析列表"></uc1:SubHeader>
            <asp:Label ID="labPrompt" Runat="server"></asp:Label>
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