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
	///		Navigation ��ժҪ˵����
	/// </summary>
//	/// 	    <Item Title="�Զ�ִ�д�Cognos���������趨">
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
		/// Ҫ������ʾ��ˮƽ�����˵�Title
		/// </summary>
		public string HeightLightNavigation
		{
			set
			{
				_heightLightNavigation = value;
			}
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

		#region ��һ�������˵�
		/// <summary>
		/// ��������
		/// �������е�Category�ڵ㣬��һ����������
		/// ���ж������url�Ƿ��ڵ�ǰ��Category�£���������Category�����������ʾ
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
							activePosition = i+1;
							break;
						}
					}//end foreach pageNode

					//���url�ڴ�category�����category�����ItemΪ�����˵�
					if(isExist)
					{
						_categoryNode = categoryNode;

						break;
					}

				}//end foreach itemNode
					
				//��Tab

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

			//����һ��Tab������˵���β����10px�Ŀո�
			stringBuilder.Append("<td width=\"10\"><img src=\""+_applicationPath+"/Images/s.gif\" width=\"10\" border=\"0\" alt=\"\" /></td>");
			stringBuilder.Append("</tr>");

			//��������ɫ��ǳ��ɫ�ġ��ߡ���ColSpan
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

			//��һ���˵�������˵��м��һ������ɫ��ǳ��ɫ���ߡ�����ǰѡ�е�TabǰΪ����ɫ���ߣ���Ϊǳ��ɫ�ģ�
			stringBuilder.Append("<tr>");

			if(darkblueColSpan != 0)
			{
				stringBuilder.Append("<td bgColor=\"#003366\" colspan=\""+darkblueColSpan.ToString()+"\" height=\"1\"></td>");
			}

			if(blueColSpan != 0)
			{
				stringBuilder.Append("<td bgColor=\"#336699\" colspan=\""+blueColSpan.ToString()+"\" height=\"1\"></td>");
			}

			//���һ��tab������˵���β֮����һ������ɫ����
			stringBuilder.Append("<td bgcolor=\"#003366\" height=\"1\"></td>");

			stringBuilder.Append("</tr>");
			stringBuilder.Append("</table>");

			return stringBuilder.ToString();

		}


	
		/// <summary>
		/// ��Tab
		/// </summary>
		/// <param name="isActive">Ҫ����Tab�Ƿ���ѡ�е�Tab</param>
		/// <param name="position">Ҫ����Tab��λ��</param>
		/// <param name="activePosition">ѡ�е�Tab��λ��</param>
		/// <param name="isLast">�Ƿ������һ��Tab</param>
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
			
			//�����ǰ���ʵ�url�봫���url��ͬ���tab����ʽΪTabselected����ɫ�ġ�
			if(isActive)
			{
				//���Ϊ��һ��tab���tabΪ����ɫ�������״��
				if(position == 1)
				{
					stringBuilder.Append("<td><IMG src=\""+_applicationPath+"/Images/blue_left.gif\" align=\"absBottom\" border=\"0\"></td>");
				}//������ǵ�һ��tab��Ϊһ���ơ���ɫ��ͼ��
				else
				{
					stringBuilder.Append("<td><IMG src=\""+_applicationPath+"/Images/yellow_blue.gif\" align=\"absBottom\" border=\"0\"></td>");

				}

				stringBuilder.Append("<td background=\""+_applicationPath+"/Images/blue_background.gif\"  class=\"Tabselected\" align=\"center\" valing=\"center\" nowrap=\"true\" >");
				stringBuilder.Append("<A  href=\"");
				stringBuilder.Append(Context.Request.ApplicationPath+url);//�˵������ӵ�ַ
				stringBuilder.Append("\">");
				stringBuilder.Append("&nbsp;&nbsp;"+title+"&nbsp;&nbsp;");//�˵�������
				stringBuilder.Append("</A>");

				stringBuilder.Append("</td>");
				stringBuilder.Append("<td><IMG src=\""+_applicationPath+"/Images/blue_right.gif\" align=\"absBottom\" border=\"0\"></td>");


			}//�����ǰ���ʵ�url�봫���url����ͬ���tab����ʽΪTabEnabled����ɫ�ġ�
			else
			{
				//���Ϊ��һ��tab���tabΪ����ɫ�������״��
				if(position == 1)
				{
					stringBuilder.Append("<td><IMG src=\""+_applicationPath+"/Images/yellow_left.gif\" align=\"absBottom\" border=\"0\"></td>");
					//����ǵ�һ��tab���ɫ����Ϊyellow_background_01����һ��խ�����ɫ��
					stringBuilder.Append("<td background=\""+_applicationPath+"/Images/yellow_background_01.gif\" nowrap=\"true\" class=\"TabEnabled\" align=\"center\" valing=\"center\">");

				}//������ǵ�һ��tab��Ϊһ����ɫ�޼��ͼ��
				else
				{	
					//���Ҫ����Tab��Acitve��Tab����һ��ͼ����Ҫ����β�Ļ�ɫͼ�꣬��Ϊ��Acitve��Tabʱ�Ѿ�����һ����ɫ�Ľ�β��ͼ���ˡ�
					if(activePosition+1 != position && position != 0 )
					{
						stringBuilder.Append("</td>");
						if(position == 2)
						{
							//����һ��խ�����ɫ�Ľ�βͼ��
							stringBuilder.Append("<td><IMG src=\""+_applicationPath+"/Images/yellow_right_01.gif\" align=\"absBottom\" border=\"0\"></td>");
						}
						else
						{
							//����һ��������ɫ�Ľ�βͼ��
							stringBuilder.Append("<td><IMG src=\""+_applicationPath+"/Images/yellow_right.gif\" align=\"absBottom\" border=\"0\"></td>");
						}
					}

					stringBuilder.Append("<td><IMG src=\""+_applicationPath+"/Images/yellow_left_02.gif\" align=\"absBottom\" border=\"0\"></td>");
					//������ǵ�һ��tab���ɫ����Ϊyellow_background����һ��������ɫ��
					stringBuilder.Append("<td background=\""+_applicationPath+"/Images/yellow_background.gif\" nowrap=\"true\" class=\"TabEnabled\" align=\"center\" valing=\"center\">");
				}

				stringBuilder.Append("<A href=\"");
				stringBuilder.Append(Context.Request.ApplicationPath+url);
				stringBuilder.Append("\">");
				stringBuilder.Append("&nbsp;&nbsp;"+title+"&nbsp;&nbsp;");
				stringBuilder.Append("</A>");
				stringBuilder.Append("</td>");

				//��������һ��Tab�򻭽�β�Ļ�ɫͼ��
				if(isLast)
				{
					//��������һ��tab���������һ��tab˵��ֻ��һ��tab��������δѡ�е�tab���Ի���һ��խ�����ɫ�Ľ�βͼ��
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
	
		#region �����������˵�
		/// <summary>
		/// ������������
		/// ����categoryNode�µ�Item�ڵ㣬��������������
		/// ���ж������url�Ƿ��ڵ�ǰ��Item�ڵ��£�
		/// ������򽫴�Item�ĵ����������ʾ��
		/// ��Ϊһ��Item�¿����ж��Page�ڵ㣬��˽�Item�µĵ�һ��Page�ڵ��InnerText
		/// ��ΪΪ��ǰItem��Ӧ�ĵ������Url������˵�����ʱ��������Url��ҳ�档
		/// </summary>
		/// <param name="url"></param>
		/// <param name="categoryNode"></param>
		/// <returns></returns>
		private string DrawHorizontalNavigation(string url)
		{
			//���û�б�ѡ���category�򣬶����˵�������
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
				//�ж������url�Ƿ��ڴ�Item�£�
				//����¼��Item�µĵ�һ��Page��InnerText,��Ϊ��Item������Ķ�λ��url��
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
		/// �������˵���
		/// </summary>
		/// <param name="isFirst"></param>
		/// <param name="url"></param>
		/// <param name="title"></param>
		/// <returns></returns>
		private string DrawHorizontalMenu(bool isFirst,bool isExist,string url,string title)
		{
			StringBuilder stringBuilder = new StringBuilder();

			//������ǵ�һ��Menu��Menum��֮��ķָ���
			if(!isFirst)
			{
				stringBuilder.Append("<td ><img src=\""+_applicationPath+"/Images/toolbar_separator.gif\" align=\"absMiddle\" border=\"0\" alt=\"\" /></td>");
			}

			stringBuilder.Append("<td width=\"10\"></td>");

			
			if( _heightLightNavigation.Trim() != String.Empty)
			{
				//���Ҫ������ʾ��ˮƽ�����˵��봫��title��ͬ���Menu��ΪHorizontalNavigationSelected��ʽ�����壩
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
					//�����ǰ���ʵ�ҳ���url�봫���url��ͬ�򣬴�Menu��ΪHorizontalNavigationSelected��ʽ�����壩
					stringBuilder.Append("<td class=\"HorizontalNavigationSelected\" align=\"center\" >");

				}//�����ǰ���ʵ�ҳ���url�봫���url����ͬ�򣬴�Menu��ΪHorizontalNavigationEnabled��ʽ��һ�����壩
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
