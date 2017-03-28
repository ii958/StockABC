namespace AISRS.WebUI.Modules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		HyperLinkAndLinkButton ��ժҪ˵����
	/// </summary>
	public class HyperLinkAndLinkButton : System.Web.UI.UserControl
	{

		#region �¼�
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

		#region HyperLinkButton����
		/// <summary>
		/// HyperLinkButton��Text����
		/// Ĭ��Ϊ��ȡ����
		/// </summary>
		public string HyperLinkButtonText
		{
			set
			{
				this.HyperLinkAction.Text = value;
			}
		}
		
		/// <summary>
		/// HyperLinkButton��NavigateUrl
		/// </summary>
		public string HyperLinkButtonNavigateUrl
		{
			set
			{
				this.HyperLinkAction.NavigateUrl = value;
				
			}
		}

	
		/// <summary>
		/// HyperLinkButton�Ƿ�����
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
		/// HyperLinkButton�Ƿ���Ч
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

		#region LinkButton�������

		/// <summary>
		/// LinkButtonText
		/// Ĭ��Ϊ��ȷ����
		/// </summary>
		public string LinkButtonText
		{
			set
			{
				this.LinkButtonAction.Text = value;
			}
		}

		/// <summary>
		/// LinkButton�Ŀͻ���OnClick�¼�
		/// </summary>
		public string LinkButtonJavascriptOnClick
		{
			set
			{
				this.LinkButtonAction.Attributes["onclick"] = value;
			}
		}

		/// <summary>
		/// LinkButton�Ƿ�����
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
		/// LinkButton�Ƿ���Ч
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
		/// LinkButton��������OnClick�¼�
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

	
	}
}
