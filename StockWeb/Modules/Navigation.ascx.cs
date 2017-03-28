namespace AISRS.WebUI.Modules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Xml;
	using System.Text;

	using Common.Exception;

	/// <summary>
	///		Navigation 的摘要说明。
	/// </summary>
//	/// 	    <Item Title="自动执行从Cognos导数参数设定">
//	<Page Class="AISRS.WebUI.CognosImport.ImportParameterQuery" URL="/CognosImport/ImportParameterQuery.aspx"/>
//	<Page Class="AISRS.WebUI.CognosImport.ImportParameterInput" URL="/CognosImport/ImportParameterInput.aspx"/>
//	</Item>
	public class Navigation : System.Web.UI.UserControl
	{

		private XmlNode _categoryNode = null;

		protected string _tabNavigation = "" ;

		protected string _horizontalNavigation = "" ;

		protected string _applicationPath = "";

		private string _heightLightNavigation = String.Empty;

		private void Page_Load(object sender, System.EventArgs e)
		{
			string pageUrl = this.GetPageUrl();

			string userNavigation = this.GetUserNavigation();

			_tabNavigation = this.DrawTabNavigation(pageUrl,userNavigation);

			_horizontalNavigation = this.DrawHorizontalNavigation(pageUrl);

		}

		/// <summary>
		/// 要高亮显示的水平导航菜单Title
		/// </summary>
		public string HeightLightNavigation
		{
			set
			{
				_heightLightNavigation = value;
			}
		}

		/// <summary>
		/// 获取所在页面的Url
		/// </summary>
		/// <returns></returns>
		private string GetPageUrl()
		{
			string virtualPath = Context.Request.FilePath;
			string virtualDirectory = Context.Request.ApplicationPath;
			_applicationPath = Context.Request.ApplicationPath;

			#region 数据有效性检查

			if (virtualPath == null || virtualPath.Trim() == string.Empty)
				throw new ValidationException("virtualPath为空！");

			if (virtualDirectory == null || virtualDirectory.Trim() == string.Empty)
				throw new ValidationException("找不到virtualDirectory！");

			if (virtualPath.ToLower().IndexOf(virtualDirectory.ToLower()) != 0)
				throw new ValidationException("virtualPath的格式不对！");

            //if (this.Page == null)
            //    throw new ValidationException("页面为空！");

			#endregion

			string url = virtualPath.Substring(virtualDirectory.Length);

		

			return url.ToLower();

		}

		/// <summary>
		///计算当前用户的导航树，并将其保存在cache里，下次直接从Cache中取
		///用户导航树是整个系统导航树中用户有权限的一部分
		/// </summary>
		/// <returns>生成的xml串格式如下
		///<UserNavigation>
		///<Category  Title="title">
		///<Item Title="title">
		///<Page>pageUrl</Page>
		///</Item>
		///<Item Title="title">
		///<Page>pageUrl</Page>
		///</Item>
		///</Category>
		///</UserNavigation> 
		///</returns>
		private string GetUserNavigation()
		{
			string KEY_PERMISSIONXML = "Key_NavigationPermissionXml";
			string userNavigatoin = "<UserNavigation>";

			//遍历页面权限XmlDocument找到用户有权访问的页面的xml字符串
			XmlDocument xmlDocument = this.Cache.Get(KEY_PERMISSIONXML) as XmlDocument;
			if(xmlDocument == null)
			{
				xmlDocument = NavigationPermission.XmlDocument;
				this.Cache.Insert(KEY_PERMISSIONXML,xmlDocument,NavigationPermission.XmlDocumentCacheDependencies);
			}

			XmlNode rootNode = xmlDocument.DocumentElement;
　　　　　　
			XmlNodeList categoryList = rootNode.ChildNodes;

			foreach(XmlNode categoryNode in categoryList)
			{
				XmlNodeList itemList = categoryNode.ChildNodes;

				string itemXml = String.Empty;

				foreach(XmlNode itemNode in itemList)
				{
					XmlNodeList pageList = itemNode.ChildNodes;

					string pageXml = String.Empty;

					#region  判断是否对page有操作的权限，如果有操作权限则生成此Page节点
					foreach(XmlNode pageNode in pageList)
					{
						bool havePagePermisson = false;
						XmlNodeList permissionList = pageNode.ChildNodes;
						if(permissionList.Count==0)
						{
							havePagePermisson = true;
						}
						else
						{
							foreach(XmlNode permissionNode in permissionList)
							{
								string permission = permissionNode.InnerText;
								if(this.Page.LoginUser.IsHavePermission(permission))
								{
									havePagePermisson = true;
									break;
								}
							}
						}
						//如果有访问页面权限则生成相应的Page节点
						if(havePagePermisson)
						{
							pageXml += "<Page> "+ pageNode.Attributes["url"].Value +"</Page>\r\n";
						}

					}//end foreach pageNode

					#endregion

					//如果Page节点的内容不为空则说明用户对此Item下的某个Page有访问权限，生成此Item节点
					if(pageXml != String.Empty)
					{
						itemXml += "<Item Title=\""+ itemNode.Attributes["Title"].Value + "\">" + pageXml +"</Item>\r\n";
					}

				}

				//如果Item节点内容不为空则说明用户对此Category下的某个Page有访问权限，生成此Category节点
				if(itemXml != String.Empty)
				{
					userNavigatoin +="<Category Title=\""+ categoryNode.Attributes["Title"].Value +"\">" + itemXml + "</Category>\r\n";			
				}
			}
			userNavigatoin += "</UserNavigation>";
			return userNavigatoin;
		}

		#region 画一级导航菜单
		/// <summary>
		/// 画导航树
		/// 遍历所有的Category节点，画一级导航栏，
		/// 并判断输入的url是否在当前的Category下，如果在则此Category导航项高亮显示
		/// </summary>
		/// <param name="url"></param>
		/// <param name="userNavigation"></param>
		private string DrawTabNavigation(string url,string userNavigation)
		{
			int activePosition = 0;

			StringBuilder stringBuilder = new StringBuilder();	
			stringBuilder.Append("<table cellSpacing=\"0\" cellPadding=\"0\" border=\"0\">");
			stringBuilder.Append("<tr>");
			stringBuilder.Append("<td width=\"100%\"></td>");

			XmlDocument xmlDocument = new XmlDocument();

			xmlDocument.LoadXml(userNavigation);
　　　　　　
			XmlNode rootNode = xmlDocument.DocumentElement;
　　　　　　
			XmlNodeList categoryList = rootNode.ChildNodes;

			for(int i=0;i<categoryList.Count;i++)
			{
				XmlNode categoryNode = categoryList[i];	

				XmlNodeList itemList = categoryNode.ChildNodes;

				bool isExist = false;

				string categoryDefaultUrl = String.Empty;

				//遍历此Category下的所有Page节点判断输入的Url是否在此Category
				//如果在则画此Category样式为“Selected”,并记录此Category下的
				//第一个Page的url做为此Category的url，当点击此Category时导航到这个url的页面
				foreach(XmlNode itemNode in itemList)
				{
					XmlNodeList pageList = itemNode.ChildNodes;
					foreach(XmlNode pageNode in pageList)
					{
						if(categoryDefaultUrl == String.Empty)
						{
							categoryDefaultUrl = pageNode.InnerText.Trim();
						}
						//判断输入的Url是否在此Category的Page节点下
						if(pageNode.InnerText.ToLower().Trim() == url.ToLower().Trim())
						{
							isExist = true;
							activePosition = i+1;
							break;
						}
					}//end foreach pageNode

					//如果url在此category下则此category下面的Item为二级菜单
					if(isExist)
					{
						_categoryNode = categoryNode;

						break;
					}

				}//end foreach itemNode
					
				//画Tab

				bool isLast = i == categoryList.Count -1?true:false;
				string tab = this.DrawTab(
					isExist,
					i+1,
					activePosition,
					isLast,
					categoryDefaultUrl,
					categoryNode.Attributes["Title"].Value);

				stringBuilder.Append(tab);
			
			}//end foreach categoryNode

			//最后的一个Tab与二级菜单的尾部有10px的空格
			stringBuilder.Append("<td width=\"10\"><img src=\""+_applicationPath+"/Images/s.gif\" width=\"10\" border=\"0\" alt=\"\" /></td>");
			stringBuilder.Append("</tr>");

			//计算深蓝色和浅蓝色的“线”的ColSpan
			int darkblueColSpan = 0;

			int blueColSpan = 0;

			if(activePosition == 0 && categoryList.Count != 0)
			{
				darkblueColSpan = categoryList.Count *3 +1 ;
			}
			else if(activePosition == 1)
			{
				darkblueColSpan = 1;

				blueColSpan = categoryList.Count * 3;
			
			}
			else if(activePosition > 1 &&categoryList.Count != 0)
			{
				darkblueColSpan = (activePosition-1) *3;

				blueColSpan = (categoryList.Count - activePosition+1) * 3;
			}

			//画一级菜单与二级菜单中间的一条深蓝色、浅蓝色的线。（当前选中的Tab前为深蓝色的线，后为浅蓝色的）
			stringBuilder.Append("<tr>");

			if(darkblueColSpan != 0)
			{
				stringBuilder.Append("<td bgColor=\"#003366\" colspan=\""+darkblueColSpan.ToString()+"\" height=\"1\"></td>");
			}

			if(blueColSpan != 0)
			{
				stringBuilder.Append("<td bgColor=\"#336699\" colspan=\""+blueColSpan.ToString()+"\" height=\"1\"></td>");
			}

			//最后一个tab与二级菜单结尾之间有一个深蓝色的线
			stringBuilder.Append("<td bgcolor=\"#003366\" height=\"1\"></td>");

			stringBuilder.Append("</tr>");
			stringBuilder.Append("</table>");

			return stringBuilder.ToString();

		}


	
		/// <summary>
		/// 画Tab
		/// </summary>
		/// <param name="isActive">要画的Tab是否是选中的Tab</param>
		/// <param name="position">要画的Tab的位置</param>
		/// <param name="activePosition">选中的Tab的位置</param>
		/// <param name="isLast">是否是最后一个Tab</param>
		/// <param name="url"></param>
		/// <param name="title"></param>
		/// <returns></returns>
		private string DrawTab(
			bool isActive,
			int position,
			int activePosition,
			bool isLast,
			string url,
			string title)
		{
			StringBuilder stringBuilder = new StringBuilder();	
			
			//如果当前访问的url与传入的url相同则此tab的样式为Tabselected，蓝色的。
			if(isActive)
			{
				//如果为第一个tab则此tab为“蓝色带尖的形状”
				if(position == 1)
				{
					stringBuilder.Append("<td><IMG src=\""+_applicationPath+"/Images/blue_left.gif\" align=\"absBottom\" border=\"0\"></td>");
				}//如果不是第一个tab则为一个黄、蓝色的图标
				else
				{
					stringBuilder.Append("<td><IMG src=\""+_applicationPath+"/Images/yellow_blue.gif\" align=\"absBottom\" border=\"0\"></td>");

				}

				stringBuilder.Append("<td background=\""+_applicationPath+"/Images/blue_background.gif\"  class=\"Tabselected\" align=\"center\" valing=\"center\" nowrap=\"true\" >");
				stringBuilder.Append("<A  href=\"");
				stringBuilder.Append(Context.Request.ApplicationPath+url);//菜单项链接地址
				stringBuilder.Append("\">");
				stringBuilder.Append("&nbsp;&nbsp;"+title+"&nbsp;&nbsp;");//菜单项名称
				stringBuilder.Append("</A>");

				stringBuilder.Append("</td>");
				stringBuilder.Append("<td><IMG src=\""+_applicationPath+"/Images/blue_right.gif\" align=\"absBottom\" border=\"0\"></td>");


			}//如果当前访问的url与传入的url不相同则此tab的样式为TabEnabled，黄色的。
			else
			{
				//如果为第一个tab则此tab为“黄色带尖的形状”
				if(position == 1)
				{
					stringBuilder.Append("<td><IMG src=\""+_applicationPath+"/Images/yellow_left.gif\" align=\"absBottom\" border=\"0\"></td>");
					//如果是第一个tab则黄色背景为yellow_background_01（有一条窄的深黄色）
					stringBuilder.Append("<td background=\""+_applicationPath+"/Images/yellow_background_01.gif\" nowrap=\"true\" class=\"TabEnabled\" align=\"center\" valing=\"center\">");

				}//如果不是第一个tab则为一个黄色无尖的图标
				else
				{	
					//如果要画的Tab是Acitve的Tab的下一个图标则不要画结尾的黄色图标，因为画Acitve的Tab时已经画了一个蓝色的结尾的图标了。
					if(activePosition+1 != position && position != 0 )
					{
						stringBuilder.Append("</td>");
						if(position == 2)
						{
							//画有一条窄的深黄色的结尾图标
							stringBuilder.Append("<td><IMG src=\""+_applicationPath+"/Images/yellow_right_01.gif\" align=\"absBottom\" border=\"0\"></td>");
						}
						else
						{
							//画有一条宽的深黄色的结尾图标
							stringBuilder.Append("<td><IMG src=\""+_applicationPath+"/Images/yellow_right.gif\" align=\"absBottom\" border=\"0\"></td>");
						}
					}

					stringBuilder.Append("<td><IMG src=\""+_applicationPath+"/Images/yellow_left_02.gif\" align=\"absBottom\" border=\"0\"></td>");
					//如果不是第一个tab则黄色背景为yellow_background（有一条宽的深黄色）
					stringBuilder.Append("<td background=\""+_applicationPath+"/Images/yellow_background.gif\" nowrap=\"true\" class=\"TabEnabled\" align=\"center\" valing=\"center\">");
				}

				stringBuilder.Append("<A href=\"");
				stringBuilder.Append(Context.Request.ApplicationPath+url);
				stringBuilder.Append("\">");
				stringBuilder.Append("&nbsp;&nbsp;"+title+"&nbsp;&nbsp;");
				stringBuilder.Append("</A>");
				stringBuilder.Append("</td>");

				//如果是最后一个Tab则画结尾的黄色图标
				if(isLast)
				{
					//如果是最后一个tab且又是最后一个tab说明只有一个tab，而且是未选中的tab所以画有一条窄的深黄色的结尾图标
					if(position == 1)
					{
						stringBuilder.Append("<td><IMG src=\""+_applicationPath+"/Images/yellow_right_01.gif\" align=\"absBottom\" border=\"0\"></td>");
					}
					else
					{
						stringBuilder.Append("<td><IMG src=\""+_applicationPath+"/Images/yellow_right.gif\" align=\"absBottom\" border=\"0\"></td>");
					}
				}
			}

			return stringBuilder.ToString();

		}

		#endregion
	
		#region 画二级导航菜单
		/// <summary>
		/// 画二级导航栏
		/// 遍历categoryNode下的Item节点，画二级导航栏，
		/// 并判断输入的url是否在当前的Item节点下，
		/// 如果在则将此Item的导航项高亮显示。
		/// 因为一个Item下可以有多个Page节点，因此将Item下的第一个Page节点的InnerText
		/// 作为为当前Item对应的导航项的Url，当点此导航项时导航到此Url的页面。
		/// </summary>
		/// <param name="url"></param>
		/// <param name="categoryNode"></param>
		/// <returns></returns>
		private string DrawHorizontalNavigation(string url)
		{
			//如果没有被选择的category则，二级菜单无内容
			if(_categoryNode == null)
			{
				return String.Empty;
			}

			StringBuilder stringBuilder = new StringBuilder();

			stringBuilder.Append("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\">");
			stringBuilder.Append("<tr>");

			XmlNodeList itemList = _categoryNode.ChildNodes;

			for(int i=0;i<itemList.Count;i++)
			{
				bool isExist = false;

				XmlNode itemNode = itemList[i];

				string itemDefaultUrl = String.Empty;

				XmlNodeList pageList = itemNode.ChildNodes;
				//判断输入的url是否在此Item下，
				//并记录此Item下的第一个Page的InnerText,作为此Item导航项的定位的url。
				foreach(XmlNode pageNode in pageList)
				{
					if(itemDefaultUrl == String.Empty)
					{
						itemDefaultUrl = pageNode.InnerText.Trim();
					}

					if(pageNode.InnerText.ToLower().Trim() == url.ToLower().Trim())
					{
						isExist = true;

						break;
					}
				
				}//end foreach pageNode

				bool isFirst = i == 0?true:false;

				string horizontalMenu = this.DrawHorizontalMenu(isFirst,isExist,itemDefaultUrl,itemNode.Attributes["Title"].Value);

				stringBuilder.Append(horizontalMenu);
							
			}//end foreach itemNode

			stringBuilder.Append("</tr></table>");
		
			return stringBuilder.ToString();

		}

		/// <summary>
		/// 画二级菜单项
		/// </summary>
		/// <param name="isFirst"></param>
		/// <param name="url"></param>
		/// <param name="title"></param>
		/// <returns></returns>
		private string DrawHorizontalMenu(bool isFirst,bool isExist,string url,string title)
		{
			StringBuilder stringBuilder = new StringBuilder();

			//如果不是第一个Menu则画Menum项之间的分割线
			if(!isFirst)
			{
				stringBuilder.Append("<td ><img src=\""+_applicationPath+"/Images/toolbar_separator.gif\" align=\"absMiddle\" border=\"0\" alt=\"\" /></td>");
			}

			stringBuilder.Append("<td width=\"10\"></td>");

			
			if( _heightLightNavigation.Trim() != String.Empty)
			{
				//如果要高亮显示的水平导航菜单与传入title相同则此Menu项为HorizontalNavigationSelected样式（粗体）
				if(_heightLightNavigation.ToLower().Trim() == title.ToLower().Trim())
				{
					stringBuilder.Append("<td class=\"HorizontalNavigationSelected\" align=\"center\" >");
				}
				else
				{
					stringBuilder.Append("<td  class=\"HorizontalNavigationEnabled\" align=\"center\" >");
				}
			}
			else
			{
				if(isExist)
				{
					//如果当前访问的页面的url与传入的url相同则，此Menu项为HorizontalNavigationSelected样式（粗体）
					stringBuilder.Append("<td class=\"HorizontalNavigationSelected\" align=\"center\" >");

				}//如果当前访问的页面的url与传入的url不相同则，此Menu项为HorizontalNavigationEnabled样式（一般字体）
				else
				{
					stringBuilder.Append("<td  class=\"HorizontalNavigationEnabled\" align=\"center\" >");
				}
			}

			stringBuilder.Append("<A href=\"");
			stringBuilder.Append(Context.Request.ApplicationPath+url);
			stringBuilder.Append("\">");
			stringBuilder.Append(title);	
			stringBuilder.Append("</A>");
			stringBuilder.Append("</td>");
			stringBuilder.Append("<td width=\"10\"></td>");

			return stringBuilder.ToString();
		}

		#endregion


		/// <summary>
		/// 重写Page属性
		/// </summary>
        public new PageBaseNoPermission Page
		{
			get
			{
				try
				{
                    PageBaseNoPermission page = (PageBaseNoPermission)base.Page;

					return page;
				}
				catch
				{
					return null;
				}
			}
		}


		#region Web 窗体设计器生成的代码
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的。
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		设计器支持所需的方法 - 不要使用代码编辑器
		///		修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
