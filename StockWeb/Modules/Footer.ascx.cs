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
	///		Footer ��ժҪ˵����
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
		/// ��ȡ����ҳ���Url
		/// </summary>
		/// <returns></returns>
		private string GetPageUrl()
		{
			string virtualPath = Context.Request.FilePath;
			string virtualDirectory = Context.Request.ApplicationPath;
			_applicationPath = Context.Request.ApplicationPath;

			#region ������Ч�Լ��

			if (virtualPath == null || virtualPath.Trim() == string.Empty)
				throw new ValidationException("virtualPathΪ�գ�");

			if (virtualDirectory == null || virtualDirectory.Trim() == string.Empty)
				throw new ValidationException("�Ҳ���virtualDirectory��");

			if (virtualPath.ToLower().IndexOf(virtualDirectory.ToLower()) != 0)
				throw new ValidationException("virtualPath�ĸ�ʽ���ԣ�");

            //if (this.Page == null)
            //    throw new ValidationException("ҳ��Ϊ�գ�");

			#endregion

			string url = virtualPath.Substring(virtualDirectory.Length);

		

			return url.ToLower();

		}

		/// <summary>
		///���㵱ǰ�û��ĵ������������䱣����cache��´�ֱ�Ӵ�Cache��ȡ
		///�û�������������ϵͳ���������û���Ȩ�޵�һ����
		/// </summary>
		/// <returns>���ɵ�xml����ʽ����
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

			//����ҳ��Ȩ��XmlDocument�ҵ��û���Ȩ���ʵ�ҳ���xml�ַ���
			XmlDocument xmlDocument = this.Cache.Get(KEY_PERMISSIONXML) as XmlDocument;
			if(xmlDocument == null)
			{
				xmlDocument = NavigationPermission.XmlDocument;
				this.Cache.Insert(KEY_PERMISSIONXML,xmlDocument,NavigationPermission.XmlDocumentCacheDependencies);
			}

			XmlNode rootNode = xmlDocument.DocumentElement;
������������
			XmlNodeList categoryList = rootNode.ChildNodes;

			foreach(XmlNode categoryNode in categoryList)
			{
				XmlNodeList itemList = categoryNode.ChildNodes;

				string itemXml = String.Empty;

				foreach(XmlNode itemNode in itemList)
				{
					XmlNodeList pageList = itemNode.ChildNodes;

					string pageXml = String.Empty;

					#region  �ж��Ƿ��page�в�����Ȩ�ޣ�����в���Ȩ�������ɴ�Page�ڵ�
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
						//����з���ҳ��Ȩ����������Ӧ��Page�ڵ�
						if(havePagePermisson)
						{
							pageXml += "<Page> "+ pageNode.Attributes["url"].Value +"</Page>\r\n";
						}

					}//end foreach pageNode

					#endregion

					//���Page�ڵ�����ݲ�Ϊ����˵���û��Դ�Item�µ�ĳ��Page�з���Ȩ�ޣ����ɴ�Item�ڵ�
					if(pageXml != String.Empty)
					{
						itemXml += "<Item Title=\""+ itemNode.Attributes["Title"].Value + "\">" + pageXml +"</Item>\r\n";
					}

				}

				//���Item�ڵ����ݲ�Ϊ����˵���û��Դ�Category�µ�ĳ��Page�з���Ȩ�ޣ����ɴ�Category�ڵ�
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
������������
			XmlNode rootNode = xmlDocument.DocumentElement;
������������
			XmlNodeList categoryList = rootNode.ChildNodes;

			for(int i=0;i<categoryList.Count;i++)
			{
				XmlNode categoryNode = categoryList[i];	

				XmlNodeList itemList = categoryNode.ChildNodes;

				bool isExist = false;

				string categoryDefaultUrl = String.Empty;

				//������Category�µ�����Page�ڵ��ж������Url�Ƿ��ڴ�Category
				//������򻭴�Category��ʽΪ��Selected��,����¼��Category�µ�
				//��һ��Page��url��Ϊ��Category��url���������Categoryʱ���������url��ҳ��
				foreach(XmlNode itemNode in itemList)
				{
					XmlNodeList pageList = itemNode.ChildNodes;
					foreach(XmlNode pageNode in pageList)
					{
						if(categoryDefaultUrl == String.Empty)
						{
							categoryDefaultUrl = pageNode.InnerText.Trim();
						}
						//�ж������Url�Ƿ��ڴ�Category��Page�ڵ���
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
		/// ��дPage����
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

		#region Web ������������ɵĴ���
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: �õ����� ASP.NET Web ���������������ġ�
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		�����֧������ķ��� - ��Ҫʹ�ô���༭��
		///		�޸Ĵ˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
