namespace AISRS.WebUI.Modules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		LinkButton ��ժҪ˵����
	///		��Button����󴥷���˷������¼�
	/// </summary>
	public class LinkButton : System.Web.UI.UserControl
	{
		#region �¼�
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
