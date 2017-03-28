<%@ Control Language="c#" AutoEventWireup="false" Codebehind="Navigation.ascx.cs" Inherits="AISRS.WebUI.Modules.Navigation" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<script type="text/javascript" language="javascript">
	function ChangeGlobalButtion(obj)
	{
		if(obj)
		{
		
			var activeImg = obj+"_active.gif";
			var activeUrl = "<%=_applicationPath%>/Images/"+activeImg;
			
			var enabledImg = obj+"_enabled.gif";
			var enabledUrl = "<%=_applicationPath%>/Images/"+enabledImg;
			
			//定义一个帮助窗口，如果没打开就新开一个窗口，如果打开了就显示出来
			//打开一个帮助窗口
			//确定实际能达到的宽度
			var windowName = "在线帮助";
			var url = "<%=_applicationPath%>/Help/Default.aspx";
			if (obj == "qaButton")
			{
				url = "<%=_applicationPath%>/Help/AISRSQA/QA/AISRSqa.html";
				windowName = "常见问题";
			}
			var realWidth = 650;
			if(realWidth >= screen.width)
			{
				realWidth = screen.width - 20;
			}
			//确定实际能达到的高度
			var realHeight = 700;
			if(realHeight >= screen.height)
			{
				realHeight = screen.height - 80;
			}
			//确定实际的TOP
			var realTop = 0;
			realTop = (screen.height - realHeight)/2 ;
			//realTop = (screen.height - realHeight)/2 - 20;
			//确定实际的LEFT
			var realLeft = 0;
			realLeft = (screen.width - realWidth) - 5;
			//realLeft = (screen.width - realWidth)/2 - 5;
			
			var paraStr = "width="+ realWidth +",height="+ realHeight +",top="+ realTop +",left="+ realLeft +",Status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes";
			var openWindow = window.open(url,windowName,paraStr)
			
	
			if(document.all[obj].src.indexOf(activeUrl) > 0)
			{
				openWindow.focus();
			}
			else
			{
				if(document.all[obj].src.indexOf(enabledUrl) > 0)
				{
					document.all[obj].src = activeUrl;
				}
			}
		}
	}
</script>

<table cellspacing="0" cellpadding="0" border="0" width="100%">
	<tr>
		<td width="14"></td>
		<td>
			<table cellspacing="0" cellpadding="0" border="0">
				<tr>
					<td height="2"></td>
				</tr>
				<tr valign="bottom">
					<td width="340" height="75">
						<!-- LOGO --><img alt="" src="<%=_applicationPath%>/Images/logoApply.jpg" align="absBottom" border="0" height="70" width="148" />
					</td>
				</tr>
				<tr>
					<td bgcolor="#003366" height="1"></td>
				</tr>
			</table>
		</td>
		<td width="100%" valign="bottom">
			<table cellspacing="0" cellpadding="0" border="0" width="100%">
				<tr>
					<td bgcolor="#003366" height="1"></td>
				</tr>
			</table>
		</td>
		<td align="right">
			<table cellspacing="0" cellpadding="0" border="0" align="right">
				<tr>
					<td height="2"></td>
				</tr>
				<tr>
					<td height="55">
						<table cellspacing="0" cellpadding="0" border="0">
							<tr>
								<td colspan="6" height="4"></td>
							</tr>
							<tr valign="top" style="height:51px">
								<!-- Advertising Begin-->
								<td width="100%"></td>
								<!-- Advertising End-->
								<!-- Global Button Begin -->
								
								<td width="10"><img alt="" src="<%=_applicationPath%>/Images/s.gif" border="0" width="10" /></td>
								<td align="center">
									<img alt="" id="helpButton"  style="CURSOR: hand" onmousedown="ChangeGlobalButtion('helpButton');" src="<%=_applicationPath%>/Images/helpButton_enabled.gif" border="0" />
									<a href="#" onclick="javascript:ChangeGlobalButtion('helpButton');">Help </a>
								</td>
								<td width="10"><img alt="" src="<%=_applicationPath%>/Images/s.gif" border="0" width="10" /></td>
								<!-- Global Button End --></tr>
						</table>
					</td>
				</tr>
				<tr align="right">
					<td height="20">
						<!-- Tab Navigation Begin -->
						<%=_tabNavigation%>
						<!-- Tab Navigation End --></td>
				</tr>
			</table>
		</td>
		<td width="14"></td>
	</tr>
	<tr> <!--Horizontal Navigation Begin -->
		<td align="right" width="14" height="23"><img alt="" src="<%=_applicationPath%>/Images/Left.gif" width="3" border="0" /></td>
		<td background="<%=_applicationPath%>/Images/HorizontalNavigation.gif" colspan="3" height="23" valign="middle" >
			<%=_horizontalNavigation%>
		</td>
		<td align="left" width="14" height="23"><img alt="" src="<%=_applicationPath%>/Images/Right.gif" width="3" border="0" /></td>
		<!--Horizontal Navigation End --></tr>
</table>
