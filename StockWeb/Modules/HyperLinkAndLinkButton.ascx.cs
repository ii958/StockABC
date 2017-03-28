namespace AISRS.WebUI.Modules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		HyperLinkAndLinkButton 的摘要说明。
	/// </summary>
	public class HyperLinkAndLinkButton : System.Web.UI.UserControl
	{

		#region 事件
		public delegate void LinkButtonClickedHandler(Object sender,System.EventArgs e);
		public event LinkButtonClickedHandler LinkButtonClicked;
		#endregion

		protected System.Web.UI.WebControls.HyperLink HyperLinkAction;
		protected System.Web.UI.WebControls.LinkButton LinkButtonAction;


		protected string _applicationPath = "";
		protected string _hyperLinkVisible = "";
		protected string _linkButtonVisible = "";


		private void Page_Load(object sender, System.EventArgs e)
		{
			_applicationPath = Context.Request.ApplicationPath;
			HyperLinkAction.Attributes["Class"] = "OraNav6Selected";
			LinkButtonAction.Attributes["Class"] = "OraNav6Selected";

		}

		#region HyperLinkButton属性
		/// <summary>
		/// HyperLinkButton的Text属性
		/// 默认为“取消”
		/// </summary>
		public string HyperLinkButtonText
		{
			set
			{
				this.HyperLinkAction.Text = value;
			}
		}
		
		/// <summary>
		/// HyperLinkButton的NavigateUrl
		/// </summary>
		public string HyperLinkButtonNavigateUrl
		{
			set
			{
				this.HyperLinkAction.NavigateUrl = value;
				
			}
		}

	
		/// <summary>
		/// HyperLinkButton是否隐藏
		/// </summary>
		public bool HyperLinkButtonHidden
		{
			set
			{
				if(value == true)
				{
					this._hyperLinkVisible = "VISIBILITY: hidden";
				}
				else
				{
					this._hyperLinkVisible = "VISIBILITY: visible";
				}
			}
		}
		
		/// <summary>
		/// HyperLinkButton是否有效
		/// </summary>
		public bool HyperLinkButtonEnabled
		{
			set
			{
				if(value == true)
				{
					this.HyperLinkAction.Enabled = true;
				}
				else
				{
					this.HyperLinkAction.Enabled = false;
				}
			}
		}

		#endregion

		#region LinkButton相关属性

		/// <summary>
		/// LinkButtonText
		/// 默认为“确定”
		/// </summary>
		public string LinkButtonText
		{
			set
			{
				this.LinkButtonAction.Text = value;
			}
		}

		/// <summary>
		/// LinkButton的客户端OnClick事件
		/// </summary>
		public string LinkButtonJavascriptOnClick
		{
			set
			{
				this.LinkButtonAction.Attributes["onclick"] = value;
			}
		}

		/// <summary>
		/// LinkButton是否隐藏
		/// </summary>
		public bool LinkButtonHidden
		{
			set
			{
				if(value == true)
				{
					this._linkButtonVisible = "VISIBILITY: hidden";
				}
				else
				{
					this._linkButtonVisible = "VISIBILITY: visible";
				}
			}
		}

		/// <summary>
		/// LinkButton是否有效
		/// </summary>
		public bool LinkButtonEnabled
		{
			set
			{
				if(value == true)
				{
					this.LinkButtonAction.Enabled = true;
				}
				else
				{
					this.LinkButtonAction.Enabled = false;
				}
			}
		}
		#endregion

		/// <summary>
		/// LinkButton服务器端OnClick事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LinkButtonAction_Click(object sender, System.EventArgs e)
		{
			if(LinkButtonClicked != null)
			{
				LinkButtonClicked(sender,e);
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
			this.LinkButtonAction.Click += new System.EventHandler(this.LinkButtonAction_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

	
	}
}
