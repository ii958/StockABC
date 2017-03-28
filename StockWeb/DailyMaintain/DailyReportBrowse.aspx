<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DailyReportBrowse.aspx.cs" Inherits="AISRS.WebUI.DailyMaintain.DailyReportBrowse" %>
<%@ Register TagPrefix="uc1" TagName="BmBlafTable" Src="../Modules/BmBlafTable.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SubHeader" Src="../Modules/SubHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Navigation" Src="../Modules/Navigation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../Modules/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LinkButton" Src="../Modules/LinkButton.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="../Modules/Footer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html>
	<head>
		<title>股票系统--投资笔记</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="C#"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
		<link href="../CSS/blaf-cn.css" type="text/css" rel="stylesheet" />
        <script type="text/javascript" src="../javascript/jsfunction.js"></script>
		<script type="text/javascript" src="../Extjs/adapter/ext/ext-base.js"></script>
		<script type="text/javascript" src="../Extjs/ext-all.js"></script>
        <script language="javascript" type="text/javascript">
            function EditData(keyID) {
                //调整数据时显示、隐藏控件
                var remark = "_remark";

                //保存、调整按钮控件，点击调整后显示保存按钮控件
                var saveControlID = "BmBlafTable_" + keyID + "_Save";
                var saveContrl = document.getElementById(saveControlID);
                var editControlID = "BmBlafTable_" + keyID + "_Edit";
                var editControl = document.getElementById(editControlID);
                var cancelControlID = "BmBlafTable_" + keyID + "_Cancel";
                var cancelControl = document.getElementById(cancelControlID);
                if (saveContrl != null && editControl != null && cancelControl != null) {
                    saveContrl.style.display = "inline";
                    cancelControl.style.display = "inline";
                    editControl.style.display = "none";
                }
                //投资记录
                var controlID = "BmBlafTable_" + keyID + remark + "_TextBox";
                var textBoxControl = document.getElementById(controlID);
                if (textBoxControl != null) {
                    textBoxControl.readOnly = false;
                    textBoxControl.style.border = "1px solid #CCCCCC";
                    textBoxControl.style.backgroundColor = "";
                }
            }
            //取页面中控件值
            function getValue(keyID) {
                var remark = "_remark";

                var dataStr = ""; //录入的数据

                var controlID = "BmBlafTable_" + keyID + remark + "_TextBox";
                var textBoxControl = document.getElementById(controlID);

                if (textBoxControl != null && Trim(textBoxControl.value) != "") {
                    dataStr += Trim(textBoxControl.value);                                        
                }
                
                var hiddenSelectedValue = document.getElementById("<%= hiddenSelectedValue.ClientID%>")
                hiddenSelectedValue.value = dataStr;

                return true;
            }
            //保存修改后数据
            function SaveData(keyID) {
                if (getValue(keyID)) {
                    var hiddenSelectedValue = document.getElementById("<%= hiddenSelectedValue.ClientID%>");
                    document.getElementById("operaMessageTop").style.display = "block";
                    document.getElementById("operaMessageBottom").style.display = "block";
                    Ext.Ajax.request({
                        url: 'DailyReportBrowse.aspx',
                        success: SaveSuccess,
                        failure: ajaxRequestFailer,
                        params: {
                            oper: 'SaveDailyReportData',
                            keyID: keyID,
                            valuedata: hiddenSelectedValue.value
                        }
                    });
                    return true;
                }
            }
            //点击取消按钮时，回调取得原始数据恢复
            function CancelData(keyID) {
                document.getElementById("operaMessageTop").style.display = "block";
                document.getElementById("operaMessageBottom").style.display = "block";

                Ext.Ajax.request({
                    url: 'DailyReportBrowse.aspx',
                    success: CancelSuccess,
                    failure: ajaxRequestFailer,
                    params: {
                        oper: 'LoadDailyReportData',
                        keyID: keyID
                    }
                });
                return true;

            }
            //保存数据回调成功后处理函数
            function SaveSuccess(response, options) {
                var responseArray = Ext.util.JSON.decode(response.responseText);
                if (responseArray.IsSuccess == true) {
                    if (responseArray.savedKeyID != null) {
                        //RestoreData(responseArray.savedKeyID);
                        //保存数据成功后，调用取消按钮事件，根据主键ID重新取得项目百分比数据，在页面中格式化显示
                        CancelData(responseArray.savedKeyID)
                    }
                    document.getElementById("operaMessageTop").style.display = "none";
                    document.getElementById("operaMessageBottom").style.display = "none";
                    window.alert("保存成功！");
                }
                else {
                    window.alert(responseArray.Message);
                }
            }
            //回调失败处理函数
            function ajaxRequestFailer() {
                document.getElementById("operaMessageTop").style.display = "none";
                document.getElementById("operaMessageBottom").style.display = "none";
                window.alert("系统出现未知异常，请联系管理员！");
            }
            //保存数据回调成功后处理函数
            function CancelSuccess(response, options) {
                var responseArray = Ext.util.JSON.decode(response.responseText);
                if (responseArray.IsSuccess == true) {
                    if (responseArray.keyID != null && responseArray.projectData != null) {
                        CancelDataResult(responseArray.keyID, responseArray.projectData);
                    }
                    document.getElementById("operaMessageTop").style.display = "none";
                    document.getElementById("operaMessageBottom").style.display = "none";
                }
                else {
                    window.alert(responseArray.Message);
                }
            }
            //保存数据后，恢复数据显示
            function RestoreData(keyID) {
                //调整数据时显示、隐藏控件
                var remark = "_remark";

                //保存、调整按钮控件，点击保存按钮数据保存成功后，显示调整控件
                var saveControlID = "BmBlafTable_" + keyID + "_Save";
                var saveContrl = document.getElementById(saveControlID);
                var editControlID = "BmBlafTable_" + keyID + "_Edit";
                var editControl = document.getElementById(editControlID);
                var cancelControlID = "BmBlafTable_" + keyID + "_Cancel";
                var cancelControl = document.getElementById(cancelControlID);
                if (saveContrl != null && editControl != null && cancelControl != null) {
                    saveContrl.style.display = "none";
                    cancelControl.style.display = "none";
                    editControl.style.display = "inline";
                }

                var controlID = "BmBlafTable_" + keyID + remark + "_TextBox";
                var textBoxControl = document.getElementById(controlID);
                if (textBoxControl != null) {
                    textBoxControl.readOnly = true;
                    textBoxControl.style.border = "none";
                    textBoxControl.style.backgroundColor = "#F7F7E7";
                }                
            }
            //点击取消按钮后，恢复数据显示
            function CancelDataResult(keyID, originalDataArgs) {
                //调整数据时显示、隐藏控件
                var remark = "_remark";

                //保存、调整按钮控件，点击保存按钮数据保存成功后，显示调整控件
                var saveControlID = "BmBlafTable_" + keyID + "_Save";
                var saveContrl = document.getElementById(saveControlID);
                var editControlID = "BmBlafTable_" + keyID + "_Edit";
                var editControl = document.getElementById(editControlID);
                var cancelControlID = "BmBlafTable_" + keyID + "_Cancel";
                var cancelControl = document.getElementById(cancelControlID);
                if (saveContrl != null && editControl != null && cancelControl != null) {
                    saveContrl.style.display = "none";
                    cancelControl.style.display = "none";
                    editControl.style.display = "inline";
                }

                var controlID = "BmBlafTable_" + keyID + remark + "_TextBox";
                var textBoxControl = document.getElementById(controlID);
                if (textBoxControl != null) {
                    textBoxControl.readOnly = true;
                    if (originalDataArgs != "") {
                        textBoxControl.value = originalDataArgs;
                    }
                    else {
                        textBoxControl.value = "";
                    }
                    textBoxControl.style.border = "none";
                    textBoxControl.style.backgroundColor = "#F7F7E7";
                }                
            }
        </script>
    </head>
    <body>
		<form id="Form1" method="post" runat="server">
			<uc1:Navigation id="Navigation1" runat="server"></uc1:Navigation>
            <input id="hiddenSelectedValue" type="hidden" name="hiddenSelectedValue" runat="server" />
			<uc1:Header id="Header1" runat="server" title="投资笔记"></uc1:Header>
            <uc1:SubHeader id="SubHeader1" runat="server" title="2013股票投资Schedule"></uc1:SubHeader>
			<table id="table1" cellspacing="0" cellpadding="0" width="100%" border="0">
				<tr>
					<td style="WIDTH: 20px" valign="top"></td>
					<td valign="top" colspan="3">
						<table id="table3" style="height:0" cellspacing="0" cellpadding="0" width="100%" border="0">
							<tr>
								<td align="right">
									<div id="operaMessageTop" style="DISPLAY:none;FONT-SIZE:9pt;COLOR:red">
										<img alt="" src="<%= _urlRoot%>/Images/loading.gif" /> 操作中...
									</div>
								</td>
							</tr>
							<tr>
								<td class="OraDataText">
									<asp:Label id="labelMessage" runat="server"></asp:Label>
								</td>
							</tr>
							<tr>
								<td>
									<uc1:BmBlafTable id="BmBlafTable" runat="server" width="1500"></uc1:BmBlafTable>
								</td>
							</tr>
							<tr>
								<td align="right">
									<div id="operaMessageBottom" style="DISPLAY:none;FONT-SIZE:9pt;COLOR:red">
										<img alt="" src="<%= _urlRoot%>/Images/loading.gif" /> 操作中...
									</div>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
            <uc1:Header id="Header2" runat="server"	></uc1:Header>
			<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
			<asp:Label ID="labScript" Runat="server"></asp:Label>
		</form>
	</body>
</html>
