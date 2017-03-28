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
	///		Footer 的摘要说明。
	/// </summary>
	public class Footer : System.Web.UI.UserControl
	{

		protected string _footer = "" ;
		protected string _applicationPath = "";

		private void Page_Load(object sender, System.EventArgs e)
		{
			string pageUrl = this.GetPageUrl();

			string userNavigation = this.GetUserNavigation();

			_footer = this.DrawFooter(pageUrl,userNavigation);


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


		/// <summary>
		/// 
		/// </summary>
		/// <param name="url"></param>
		/// <param name="userNavigation"></param>
		/// <returns></returns>
		private string DrawFooter(string url,string userNavigation)
		{

			StringBuilder stringBuilder = new StringBuilder();	
			stringBuilder.Append("<div  style=\"text-align:center;word-wrap:break-word;word-break:keep-all\">");

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
							break;
						}
					}//end foreach pageNode


				}//end foreach itemNode

				if(isExist)
				{
					stringBuilder.Append("&nbsp;&nbsp; <a href=\""+_applicationPath+categoryDefaultUrl+"\" class=\"FooterSelected\" ><font class=\"FooterSelected\"> "+categoryNode.Attributes["Title"].Value+"</font></a> &nbsp;&nbsp;");
				}
				else
				{
					stringBuilder.Append("&nbsp;&nbsp; <a href=\""+_applicationPath+categoryDefaultUrl+"\" class=\"FooterEnabled\"><font class=\"FooterEnabled\"> "+categoryNode.Attributes["Title"].Value+"</font></a> &nbsp;&nbsp;");
				}

				if(i != categoryList.Count-1)
				{
					stringBuilder.Append("|");
				}

			
			}//end foreach categoryNode

			stringBuilder.Append("</div>");
			return stringBuilder.ToString();

		}




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
