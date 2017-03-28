namespace AISRS.WebUI.Modules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		HyperLinkButton ��ժҪ˵����
	///		��Button�����ִֻ��ǰ��Javascript ��������������˷������¼�
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
