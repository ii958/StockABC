using System;

namespace AISRS.WebUI.Modules
{
	/// <summary>
	/// PagePicker ��ժҪ˵����
	/// </summary>
	public class PagePicker : System.Web.UI.UserControl
	{
	
		#region ����
		private const string KEY_PageSize = "Key:PageSize";
		private const string KEY_TotalRecordCount = "Key:TotalRecordCount";

		protected int _pageNumber = 1;
		protected int _pageSize = 10;
		protected int _totalRecordCount = -1;
		#endregion
		
		#region ����
		//
		public int TotalRecordCount
		{
			get { return _totalRecordCount; }
			set { _totalRecordCount = value;}
		}

		//
		public int PageSize
		{
			get { return _pageSize; }
			set { _pageSize = value;}
		}

		//
		public int PageNumber
		{
			get { return _pageNumber; }
			set { _pageNumber = value;}			
		}		

		//
		public int PageCount
		{
			get 
			{ 
				if(_totalRecordCount < 0)
					return -1;
				
				if(_pageSize <= 0)
					return -1;
			
				if(_totalRecordCount % _pageSize == 0)
					return _totalRecordCount / _pageSize;

				else
					return _totalRecordCount / _pageSize + 1;				
			}			
		}

		public int CurrentPageRecordCount
		{
			get
			{
				if(this.PageCount < 0)
					return -1;
				if(this.PageCount == 0)
					return 0;

				if(this.PageNumber < this.PageCount)
					return this._pageSize;
				else
				{
					if(_totalRecordCount % _pageSize == 0)
						return this._pageSize;
					else
						return _totalRecordCount % _pageSize;
				}
			}
		}		
		
		#endregion

		#region �¼�	
		public delegate void PageChangedHandler(Object sender,PageChangedArgs e);
		public event PageChangedHandler PageChanged = null;
		#endregion

		#region ����
		/// <summary>
		/// ���ֺ�source״̬һ��
		/// </summary>
		/// <param name="source">Դ����</param>
		public void SameWith(PagePicker source)
		{
			// ����
			this.TotalRecordCount = source.TotalRecordCount;
			this.PageSize = source.PageSize;
			this.PageNumber = source.PageNumber;
			
			// �¼�	
			this.PageChanged = source.PageChanged;
		}

	
		
		/// <summary>
		/// ����PageChanged�¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RaisePageChangedEvent()
		{
			if(this.PageChanged != null)
			{
				PageChanged(this, new PageChangedArgs(this._pageNumber,this._pageSize));
			}
		}
		#endregion

		#region ���ط���
		/// <summary>
		/// �Ѷ������Ա��浽ViewState��
		/// </summary>
		/// <returns></returns>	
		protected override object SaveViewState()
		{
			// TODO:  ��� PagePicker.SaveViewState ʵ��
			this.ViewState[KEY_PageSize] = this._pageSize;
			this.ViewState[KEY_TotalRecordCount] = this._totalRecordCount;

			return base.SaveViewState ();
		}
	
		/// <summary>
		/// ����ʱ����viewstate�лָ���������
		/// </summary>
		/// <param name="savedState"></param>
		protected override void LoadViewState(object savedState)
		{
			// TODO:  ��� PagePicker.LoadViewState ʵ��
			base.LoadViewState (savedState);

			object pageSize = this.ViewState[KEY_PageSize];
			if(pageSize != null)
				this._pageSize = (int)pageSize;

			object totalRecordCount = this.ViewState[KEY_TotalRecordCount];
			if(totalRecordCount != null)
				this._totalRecordCount = (int)totalRecordCount;
		}
		#endregion
	}	

	/// <summary>
	/// ��������ҳ�Ÿı��¼��Ĳ���
	/// </summary>
	public class PageChangedArgs:System.EventArgs
	{
		private int _pageNumber;
		private int _pageSize;

		public PageChangedArgs( int pageNumber,int pageSize)
		{
			this._pageNumber = pageNumber;
			this._pageSize = pageSize;
		}

		public int PageNumber
		{
			get{ return _pageNumber;}
		}
		
		public int PageSize
		{
			get{return _pageSize;}

		}
	}
}
