<%@ Page language="c#" Codebehind="DatePicker.aspx.cs" AutoEventWireup="false" Inherits="APC.WebUI.DatePicker.DataPicker" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Selecte Date</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../CSS/blaf-cn.css" type="text/css" rel="stylesheet">
		<style type="text/css">A:link { TEXT-DECORATION: none }
	A:active { TEXT-DECORATION: none }
	A:visited { TEXT-DECORATION: none }
		</style>
		<script language="javascript">
			function DatePickerSetDate(sender,selectdate)
			{
				var senderObj = window.opener.document.all[sender];
				if(senderObj)
				{
					senderObj.value = selectdate;
					senderObj.focus();
				}				
				window.close();
				
				
			}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" border="0" width="225" align="center">
				<tr>
					<td align="center">
						<asp:ImageButton id="ImagebuttonPrevMonth" runat="server" ImageAlign="AbsBottom" ImageUrl="../Images/datepicker_lgarrowleft_enabled.gif"></asp:ImageButton>
						<img src="../Images/s.gif" border="0" width="3">
						<asp:DropDownList id="DropDownListYear" runat="server" AutoPostBack="True"></asp:DropDownList>
						<asp:DropDownList id="DropDownListMonth" runat="server" AutoPostBack="True"></asp:DropDownList>
						<img src="../Images/s.gif" border="0" width="3">
						<asp:ImageButton id="ImageButtonNextMonth" runat="server" ImageAlign="AbsBottom" ImageUrl="../Images/datepicker_lgarrowright_enabled.gif"></asp:ImageButton>
					</td>
				</tr>
				<tr>
					<td>
						<asp:calendar id="CalendarDate" runat="server" BorderStyle="None" BorderWidth="1px" BorderColor="#FFCC66"
							Font-Names="Arial" Font-Size="X-Small" Height="200px" ForeColor="#336699" Width="225px" BackColor="#F7F7E7"
							ShowTitle="False">
							<TodayDayStyle Font-Bold="True" ForeColor="White" BackColor="#999966"></TodayDayStyle>
							<SelectorStyle BackColor="#FFCC66"></SelectorStyle>
							<NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC"></NextPrevStyle>
							<DayHeaderStyle Font-Size="X-Small" Font-Names="Arial" Font-Bold="True" Height="1px" ForeColor="#336699"
								CssClass="OraCalendarHeader" BackColor="#CCCC99"></DayHeaderStyle>
							<SelectedDayStyle Font-Bold="True" BackColor="#999966"></SelectedDayStyle>
							<TitleStyle Font-Size="X-Small" Font-Bold="True" ForeColor="#336699" BackColor="White"></TitleStyle>
							<OtherMonthDayStyle ForeColor="#999999"></OtherMonthDayStyle>
						</asp:calendar>
					</td>
				</tr>
				<tr>
					<td height="6">
					</td>
				</tr>
				<tr>
					<td align="right">
						<TABLE cellSpacing="0" cellPadding="0" border="0">
							<TR>
								<TD vAlign="top" align="right" width="12" height="18" rowSpan="4"><IMG height="18" hspace="0" src="../images/smallbutton_left.gif" width="12" vspace="0"
										border="0"></TD>
								<TD vAlign="top" align="left" bgColor="#999966" height="1"><IMG height="1" src="../Images/button_top.gif" border="0"></TD>
								<TD vAlign="top" align="left" width="12" height="18" rowSpan="4"><IMG height="18" src="../images/smallbutton_right.gif" width="12" border="0"></TD>
							</TR>
							<TR>
								<TD class="OraBGAccentLight OraNav6Selected" vAlign="middle" noWrap align="left" height="15"><A href="#" onclick="window.close();"><font class="OraNav6Selected">È¡Ïû</font></A>
								</TD>
							</TR>
							<TR>
								<TD vAlign="top" align="left" bgColor="#666633" height="1"><IMG height="1" src="../images/button_bottom1.gif" border="0"></TD>
							</TR>
							<TR>
								<TD vAlign="top" align="left" bgColor="#333300" height="1"><IMG height="1" src="../images/button_bottom2.gif" border="0"></TD>
							</TR>
						</TABLE>
					</td>
				</tr>
			</table>
			<asp:Label id="LabelScript" runat="server"></asp:Label>
		</form>
	</body>
</HTML>
