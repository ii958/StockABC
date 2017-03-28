namespace AISRS.WebUI.Modules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		HyperLinkButton 的摘要说明。
	///		此Button点击后只执行前端Javascript 函数，不触发后端服务器事件
	/// </summary>
	public class HyperLinkButton : System.Web.UI.UserControl
	{
		protected string _applicationPath = "";
		protected System.Web.UI.WebControls.HyperLink HyperLinkAction;
		protected string _visible = "";

		private void Page_Load(object sender, System.EventArgs e)
		{
			_applicationPath = Context.Request.ApplicationPath;
			HyperLinkAction.Attributes["Class"] = "OraNav6Selected";
		}
		
		public string Text
		{
			set
			{
				this.HyperLinkAction.Text = value;
			}
		}
		
		public string NavigateUrl
		{
			set
			{
				this.HyperLinkAction.NavigateUrl = value;
				
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
					this.HyperLinkAction.Enabled = true;
				}
				else
				{
					this.HyperLinkAction.Enabled = false;
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
