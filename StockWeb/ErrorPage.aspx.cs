using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using AISRS.Common.Exception;

namespace AISRS.WebUI
{
	/// <summary>
	/// ErrorPage ��ժҪ˵����
	/// </summary>
	public class ErrorPage : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Label labelErrorMessage;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (this.Session["Session:ErrorMessage"] == null || this.Session["Session:ErrorMessage"].ToString().Trim() == string.Empty)
			{
				this.labelErrorMessage.Text = CommonException.CommonDefaultFriendlyMessage;
			}
			else
			{
				this.labelErrorMessage.Text = this.Session["Session:ErrorMessage"].ToString().Trim();

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
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭���޸�
		/// �˷��������ݡ�
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
