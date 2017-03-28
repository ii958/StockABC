namespace AISRS.WebUI.Modules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	using AISRS.Common.Framework;

	/// <summary>
	///		BlafPagePicker 的摘要说明。
	/// </summary>
	public class BlafPagePicker : PagePicker
	{
		protected System.Web.UI.WebControls.Label labelPrePage;
		protected System.Web.UI.WebControls.DropDownList dropDownListPageIndex;
		protected System.Web.UI.WebControls.Label labelNextPage;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hiddenPageNumber;
		protected System.Web.UI.WebControls.LinkButton linkButtonPageChange;

		private const string KEY_EnabledPrePageImage  = "KEY:EnabledPrePageImage";
		private const string KEY_DisabledPrePageImage = "KEY:DisabledPrePageImage";
		private const string KEY_EnabledNextPageImage = "KEY:EnabledNextPageImage";
		private const string KEY_DisabledNextPageImage = "KEY:DisabledNextPageImage";
		private const string KEY_PreNextLabelCss = "KEY:PreNextLabelCss";


		private string _enabledPrePageImage   = string.Empty; // Configuration.UrlRoot + "/Images/prevnext_smlarrowleft_enabled.gif";
		private string _disabledPrePageImage  = string.Empty; //Configuration.UrlRoot + "/Images/prevnext_smlarrowleft_disabled.gif";
		private string _enabledNextPageImage  = string.Empty; // Configuration.UrlRoot + "/Images/prevnext_smlarrowright_enabled.gif";
		private string _disabledNextPageImage = string.Empty; //Configuration.UrlRoot + "/Images/prevnext_smlarrowright_disabled.gif";

		private string _preNextLabelCss = "OraVLinkText";
		private	string _space = "&nbsp;&nbsp;";
		
		#region 属性

		public string EnabledPrePageImage
		{
			get
			{
				return _enabledPrePageImage;
			}
			set
			{
				_enabledPrePageImage = value;
			}
		}

		public string DisabledPrePageImage
		{
			get
			{
				return _disabledPrePageImage;
			}
			set
			{
				_disabledPrePageImage = value;
			}
		}

		public string EnabledNextPageImage
		{
			get
			{
				return _enabledNextPageImage;
			}
			set
			{
				_enabledNextPageImage = value;
			}
		}

		public string DisabledNextPageImage
		{
			get
			{
				return _disabledNextPageImage;
			}
			set
			{
				_disabledNextPageImage = value;
			}
		}

		public string PreNextLabelCss
		{
			get
			{
				return _preNextLabelCss;
			}
			set
			{
				_preNextLabelCss = value;
			}
		}

		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			string requestUrlRoot = "http://" + Context.Request.Url.Host;
			if(Context.Request.Url.Port.ToString() != "80")
				requestUrlRoot += ":"  + Context.Request.Url.Port.ToString();
			if(Context.Request.ApplicationPath != "/")
				requestUrlRoot += Context.Request.ApplicationPath;

			_enabledPrePageImage   = requestUrlRoot + "/Images/prevnext_smlarrowleft_enabled.gif";
			_disabledPrePageImage  = requestUrlRoot + "/Images/prevnext_smlarrowleft_disabled.gif";
			_enabledNextPageImage  = requestUrlRoot + "/Images/prevnext_smlarrowright_enabled.gif";
			_disabledNextPageImage = requestUrlRoot + "/Images/prevnext_smlarrowright_disabled.gif";


			this.SetDropDownListPageIndexDefaultValue();
			this.SetLabelNextPageDefaultValue();
			this.SetLabelPrePageDefaultValue();

			this.dropDownListPageIndex.Attributes["onchange"] = "javascript:BlafDropDownListSetPageNumber(" 
				+ "this" + ","
				+ "'" + this.hiddenPageNumber.ClientID + "',"
				+ "'" + this.linkButtonPageChange.ClientID + "')";
		}

		#region 设置控件初始值

		/// <summary>
		/// 设置上页标签默认内容
		/// </summary>
		private void SetLabelPrePageDefaultValue()
		{
			string imagePre = "<img border=\"0\" align=\"absbottom\" src=\""+ _disabledPrePageImage +"\">";
			this.labelPrePage.Text = imagePre + _space + "<font class='"+_preNextLabelCss+"'>" + "Previous" + "</font>";
		}

		/// <summary>
		/// 设置下页标签默认内容
		/// </summary>
		private void SetLabelNextPageDefaultValue()
		{
			string imageNex = "<img border=\"0\"  align=\"absbottom\" src=\""+ _disabledNextPageImage +"\">";
			this.labelNextPage.Text = "<font class='"+_preNextLabelCss+"'>" + "Next" + "</font>" + _space + imageNex ;
		}

		/// <summary>
		/// 设置分页下拉框默认值
		/// </summary>
		private void SetDropDownListPageIndexDefaultValue()
		{
			this.dropDownListPageIndex.Items.Clear();

			dropDownListPageIndex.Items.Add(new ListItem("0---0 of 0" ,"0"));
		}

		#endregion
			

		/// <summary>
		/// 刷新分页控件
		/// </summary>
		private void Refresh()
		{
			if(this.TotalRecordCount <= 0)
			{
				return;
			}
			
			// 检查数据有效性							
			if(this.PageNumber <= 0 || this.PageNumber > this.PageCount)
				return;

			if(this.PageSize <= 0)
				return;

		

			//设置上页按钮
			if(this.PageNumber <=1)
			{
				this.SetLabelPrePageDefaultValue();
			}
			else
			{
				int pageNumber = this.PageNumber -1;
				string imagePre = "<img border=\"0\"  align=\"absbottom\" src=\""+ _enabledPrePageImage +"\" />";
				string text =   imagePre + _space + "<font class='"+_preNextLabelCss+"'>" + "Previous "	+ this.PageSize.ToString() +"</font>" ;

				this.labelPrePage.Text = "<a "
					+"href=\"javascript:BlafpagePickerSetPageNumber(" 
					+ pageNumber.ToString () + ","
					+ "'" + this.hiddenPageNumber.ClientID + "',"
					+ "'" + this.linkButtonPageChange.ClientID + "')\">" 
					+ text	
					+ "</a>";
			}

			//设置下页按钮
			if(this.PageNumber >= this.PageCount)
			{
				this.SetLabelNextPageDefaultValue();
			}
			else
			{
				string imageNex = "<img border=\"0\"  align=\"absbottom\" src=\""+ _enabledNextPageImage +"\" />";
				int pageNumber = this.PageNumber + 1;
				string text = "<font class='"+_preNextLabelCss+"'>" + "Next " + GetNextPageRecordCount().ToString() + "</font>" + _space + imageNex ;

				this.labelNextPage.Text = "<a "
					+"href=\"javascript:BlafpagePickerSetPageNumber(" 
					+ pageNumber.ToString() + ","
					+ "'" + this.hiddenPageNumber.ClientID + "',"
					+ "'" + this.linkButtonPageChange.ClientID + "')\">" 
					+ text
					+ "</a>";
			}

			//设置选择记录范围下拉框
			this.InitDropDownListPageIndex();

			//记录当前第几页
			this.hiddenPageNumber.Value = this.PageNumber.ToString();

			// 注册客户端脚本
			string script = @"
				<script>
					function BlafpagePickerSetPageNumber(pageNumber,hiddenPageNumberClientID,linkButtonPageChangeClientID)
					{
						var hiddenPageNumber = document.getElementById(hiddenPageNumberClientID);
						hiddenPageNumber.value = pageNumber.toString();
						var linkButtonPageChange = document.getElementById(linkButtonPageChangeClientID);
						eval(linkButtonPageChange.href);
					}

					function BlafDropDownListSetPageNumber(dropDownObj,hiddenPageNumberClientID,linkButtonPageChangeClientID)
					{
						var hiddenPageNumber = document.getElementById(hiddenPageNumberClientID);
						hiddenPageNumber.value = dropDownObj.value;
						var linkButtonPageChange = document.getElementById(linkButtonPageChangeClientID);
						eval(linkButtonPageChange.href);
					}
				</script>";

			if(!this.Page.IsClientScriptBlockRegistered("BlafPagePickerScript"))
				this.Page.RegisterClientScriptBlock("BlafPagePickerScript",script);


		}

		/// <summary>
		/// 获取下页的记录数
		/// </summary>
		/// <returns></returns>
		private int GetNextPageRecordCount()
		{
			if(this.PageCount < 0)
				return -1;
			if(this.PageCount == 0)
				return 0;

			if(this.PageNumber >= this.PageCount)
			{
				return 0;
			}
			else
			{
				if(this.PageNumber == this.PageCount -1)
				{
					if(TotalRecordCount % this.PageSize == 0)
						return this.PageSize;
					else
						return TotalRecordCount % this.PageSize;
				}
				else
				{
					return this.PageSize;
				}
			}
		}		

		/// <summary>
		/// 设置选择记录范围下拉框
		/// </summary>
		private void InitDropDownListPageIndex()
		{
			this.dropDownListPageIndex.Items.Clear();

			if(TotalRecordCount == 0)
			{
				this.SetDropDownListPageIndexDefaultValue();
				return;
			}

			
			for(int i=0;i<PageCount;i++)
			{
				int beginRecorde = PageSize * i+1;

				int endrecorde = PageSize * (i+1);

				if(endrecorde > TotalRecordCount)
				{
					endrecorde = TotalRecordCount;
				}

				string recordeRange = beginRecorde.ToString() + "---" + endrecorde.ToString() + " of " + TotalRecordCount.ToString();

				dropDownListPageIndex.Items.Add(new ListItem(recordeRange ,Convert.ToString(i+1)));
				
			}

			//根据当前第几页定位分页下拉框
			if(this.PageNumber <= this.dropDownListPageIndex.Items.Count)
			{
				this.dropDownListPageIndex.SelectedIndex = 
					this.dropDownListPageIndex.Items.IndexOf(
					this.dropDownListPageIndex.Items.FindByValue(this.PageNumber.ToString()));
			}

		}

		/// <summary>
		/// 重载OnPreRender，栓新页码显示
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			// TODO:  添加 NumberSeriesPagePicker.OnPreRender 实现			
			base.OnPreRender (e);
			Refresh();
		}

		/// <summary>
		/// 响应linkButtonPageChange点击事件，引发PageNumberChanged事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void linkButtonPageChange_Click(object sender, System.EventArgs e)
		{
			int pageNumber = 0;			
			try
			{
				pageNumber = int.Parse(hiddenPageNumber.Value);				
				this.PageNumber = pageNumber;				
			}
			catch
			{
				throw new Exception("非法访问，请用正常方式使用本系统！");
			}
			
			base.RaisePageChangedEvent();
		}

		/// <summary>
		/// 把对象属性保存到ViewState中
		/// </summary>
		/// <returns></returns>	
		protected override object SaveViewState()
		{
			this.ViewState[KEY_DisabledNextPageImage] = this._disabledNextPageImage;
			this.ViewState[KEY_DisabledPrePageImage] = this._disabledPrePageImage;
			this.ViewState[KEY_EnabledNextPageImage] = this._enabledNextPageImage;
			this.ViewState[KEY_EnabledPrePageImage] = this._enabledPrePageImage;
			this.ViewState[KEY_PreNextLabelCss] = this._preNextLabelCss;
			return base.SaveViewState ();
		}

		
		/// <summary>
		/// 加载时，从viewstate中恢复对象属性
		/// </summary>
		/// <param name="savedState"></param>
		protected override void LoadViewState(object savedState)
		{
			base.LoadViewState (savedState);

			object disabledNextPageImage = this.ViewState[KEY_DisabledNextPageImage] ;
			if(disabledNextPageImage != null)
				this._disabledNextPageImage = (String)disabledNextPageImage;

			object disabledPrePageImage = this.ViewState[KEY_DisabledPrePageImage] ;
			if(disabledPrePageImage != null)
				this._disabledPrePageImage = (String)disabledPrePageImage;

			object enabledNextPageImage = this.ViewState[KEY_EnabledNextPageImage] ;
			if(enabledNextPageImage != null)
				this._enabledNextPageImage = (String)enabledNextPageImage;

			object enabledPrePageImage = this.ViewState[KEY_EnabledPrePageImage] ;
			if(enabledPrePageImage !=null)
				this._enabledPrePageImage = (String)enabledPrePageImage;

			object preNextLabelCss = this.ViewState[KEY_PreNextLabelCss] ;
			if(preNextLabelCss !=null)
				this._preNextLabelCss = (String)preNextLabelCss;
			
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
			this.linkButtonPageChange.Click += new System.EventHandler(this.linkButtonPageChange_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	
	}
}
