namespace AISRS.WebUI.Modules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		LinkButton 的摘要说明。
	///		此Button点击后触发后端服务器事件
	/// </summary>
	public class LinkButton : System.Web.UI.UserControl
	{
		#region 事件
		public delegate void LinkButtonClickedHandler(Object sender,System.EventArgs e);
		public event LinkButtonClickedHandler LinkButtonClicked;
		#endregion

		protected System.Web.UI.WebControls.LinkButton LinkButtonAction;
		protected string _applicationPath = "";
		protected string _visible = "";

		private void Page_Load(object sender, System.EventArgs e)
		{
			_applicationPath = Context.Request.ApplicationPath;
			LinkButtonAction.Attributes["Class"] = "OraNav6Selected";
		}
		
		public string Text
		{
			set
			{
				this.LinkButtonAction.Text = value;
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

		private void LinkButtonAction_Click(object sender, System.EventArgs e)
		{
			if(LinkButtonClicked != null)
			{
				LinkButtonClicked(sender,e);
			}
		}

		public string JavascriptOnClick
		{
			set
			{
				this.LinkButtonAction.Attributes["onclick"] = value;
			}
		}

		public bool Hidden
		{
			set
			{
				if(value == true)
				{
					this._visible = "VISIBILITY: hidden";
				}
				else
				{
					this._visible = "VISIBILITY: visible";
				}
			}
		}

		public bool Enabled
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
	}
}
