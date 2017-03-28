namespace AISRS.WebUI.Modules
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	
	using System.Text;

	/// <summary>
	///		��ҳ���ϵ�Table��ʾ�߶ȺͿ�ȴ��ڸ����ķ�Χʱ��
	///		Table���Զ������úõ�ǰ���к�ǰ�����У����������Զ����ֹ�������
	///		�����������������
	/// </summary>
	public class BmBlafTable : System.Web.UI.UserControl
	{
		
		// bmTable
		protected System.Web.UI.WebControls.LinkButton linkButtonSort; //����
		protected System.Web.UI.WebControls.Label labelScript;
		protected System.Web.UI.WebControls.Table table;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hiddenHighlightRowID;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hiddenSortColumnName;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hiddenIsSortDesc;		

		// scroll table
		protected System.Web.UI.WebControls.Label labelJsFunctions;
		protected System.Web.UI.WebControls.Label labelJs;
		protected System.Web.UI.HtmlControls.HtmlTable tableFrame;
		protected System.Web.UI.HtmlControls.HtmlTableCell tableFrameTd;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divTitleFreeze;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divTitle;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divMainFreeze;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divMain;

		#region ˽�б���
		// bmTable
		private Unit _borderWidth = Unit.Pixel(0);
		private Unit _headRowHeight = 20;
		private Unit _bodyRowHeight = 18;
		private Unit _footRowHeight = 20;
		private Unit _cellBorderWidth = 1;
		
		private BorderStyle _borderStyle = BorderStyle.Solid;
		private BorderStyle _cellBorderStyle = BorderStyle.Solid;
		
		private FontUnit _headFontSize = FontUnit.Parse("9pt");
		private FontUnit _bodyFontSize = FontUnit.Parse("9pt");
		private FontUnit _footFontSize = FontUnit.Parse("9pt");

		private Color _borderColor = Color.FromArgb(0,0,0);
		private Color _cellBorderColor = Color.FromArgb(0,0,0);
		private Color _headRowForeColor = Color.FromArgb(0,0,0);
		private Color _headRowBackColor = Color.FromArgb(187,221,255);
		private Color _bodyRowForeColor = Color.FromArgb(0,0,0);
		private Color _bodyRowBackColor = Color.FromArgb(230,242,255);		
		private Color _bodyRowBackSwitchColor = Color.FromArgb(255,255,255);
		private Color _footRowForeColor = Color.FromArgb(0,0,0);
		private Color _footRowBackColor = Color.FromArgb(255,192,203);
		private Color _highlightRowForeColor = Color.FromArgb(0,0,0);
		private Color _highlightRowBackColor = Color.FromArgb(255,232,201);

		private string _headCellCssClass = "OraTableBorder0001";
		private string _bodyCellCssClass = "OraTableBorder0011";

		//Add By Zhangbo

		private string _sortableHeaderLinkClass = "OraTableSortableHeaderLink";
		private string _headerIconButtonClass = "OraTableColumnHeaderIconButton";
		private string _bodyRowClass ="OraTableCellText";
		private string _switchBodyRowClass="OraTableCellTextBand";
		private string _headerRowClass = "OraTableColumnHeader";
		//
		
		private string _sortImageUrl = "/BmFramework/images/up.gif";
		private string _sortDescImageUrl = "/BmFramework/images/down.gif";

		//		private bool _isSwitchBodyRowColor = true;
		//		private bool _isSwitchColorRow = false;
		//
		//		private bool _isSortClickedColumn = true;
		//		private bool _isHighlightClickedRow = true;		
		//	
		private bool _isSwitchBodyRowColor = false;
		private bool _isSwitchColorRow = false;

		private bool _isSortClickedColumn = true;
		private bool _isHighlightClickedRow = false;		

		// scroll table
		private bool _isEnableScroll = false;
		private string _tableFrameWidth;
		private string _tableFrameHeight;
		private int _freezeColumnCount = 0;					
		private int _titleRowCount = 0;

		#endregion

		#region Properties Declare
		public Unit BorderWidth
		{
			get { return _borderWidth; }
			set
			{ 
				_borderWidth = value;
				table.BorderWidth = value;
			}
		}

		public Unit HeadRowHeight
		{
			get { return _headRowHeight; }
			set { _headRowHeight = value;}
		}

		public Unit BodyRowHeight
		{
			get { return _bodyRowHeight; }
			set { _bodyRowHeight = value;}
		}

		public Unit FootRowHeight
		{
			get { return _footRowHeight; }
			set { _footRowHeight = value;}
		}

		public Unit CellBorderWidth
		{
			get { return _cellBorderWidth; }
			set { _cellBorderWidth = value;}
		}

		public BorderStyle BorderStyle
		{
			get { return _borderStyle; }
			set { _borderStyle = value;}
		}

		public BorderStyle CellBorderStyle 
		{
			get { return _cellBorderStyle; }
			set { _cellBorderStyle = value;}
		}

		public FontUnit HeadFontSize
		{
			get { return _headFontSize; }
			set { _headFontSize = value;}
		}

		public FontUnit BodyFontSize
		{
			get { return _bodyFontSize; }
			set { _bodyFontSize = value;}
		}

		public FontUnit FootFontSize
		{
			get { return _footFontSize; }
			set { _footFontSize = value;}
		}

		public Color BorderColor
		{
			get { return _borderColor; }
			set { _borderColor = value;}
		}

		public Color CellBorderColor
		{
			get { return _cellBorderColor; }
			set { _cellBorderColor = value;}
		}
	
		public Color HeadRowForeColor
		{
			get { return _headRowForeColor; }
			set { _headRowForeColor = value;}
		}

		public Color HeadRowBackColor
		{
			get { return _headRowBackColor; }
			set { _headRowBackColor = value;}
		}

		public Color BodyRowForeColor
		{
			get { return _bodyRowForeColor; }
			set { _bodyRowForeColor = value;}
		}

		public Color BodyRowBackColor
		{
			get { return _bodyRowBackColor; }
			set { _bodyRowBackColor = value;}
		}

		public Color BodyRowBackSwitchColor
		{
			get { return _bodyRowBackSwitchColor; }
			set { _bodyRowBackSwitchColor = value;}
		}

		public Color FootRowForeColor
		{
			get { return _footRowForeColor; }
			set { _footRowForeColor = value;}
		}

		public Color FootRowBackColor
		{
			get { return _footRowBackColor; }
			set { _footRowBackColor = value;}
		}

		public Color HighlightRowForeColor
		{
			get { return _highlightRowForeColor; }
			set { _highlightRowForeColor = value;}
		}

		public Color HighlightRowBackColor
		{
			get { return _highlightRowBackColor; }
			set { _highlightRowBackColor = value;}
		}

		public string HeadCellCssClass
		{
			get { return _headCellCssClass; }
			set { _headCellCssClass = value;}
		}

		public string BodyCellCssClass
		{
			get { return _bodyCellCssClass; }
			set { _bodyCellCssClass = value;}
		}


		//Add By Zhangbo
		public string SortableHeaderLinkClass
		{
			get{ return _sortableHeaderLinkClass ;}
			set{ _sortableHeaderLinkClass = value;}
		}

		public string HeaderIconButtonClass
		{
			get{ return _headerIconButtonClass;}
			set{ _headerIconButtonClass = value;}
		}

		public string BodyRowClass
		{
			get{ return _bodyRowClass;}
			set{ _bodyRowClass = value;}
		}

		public string SwitchBodyRowClass
		{
			get{ return _switchBodyRowClass;}
			set{ _switchBodyRowClass = value;}
		
		}
		//



		public string SortImageUrl
		{
			get { return _sortImageUrl; }
			set { _sortImageUrl = value;}
		}

		public string SortDescImageUrl
		{
			get { return _sortDescImageUrl; }
			set { _sortDescImageUrl = value;}
		}

		public bool IsSwitchBodyRowColor
		{
			get { return _isSwitchBodyRowColor; }
			set { _isSwitchBodyRowColor = value;}
		}

		public bool IsSortClickedColumn
		{
			get { return _isSortClickedColumn; }
			set { _isSortClickedColumn = value;}
		}

		public bool IsHighlightClickedRow
		{
			get { return _isHighlightClickedRow; }
			set { _isHighlightClickedRow = value;}
		}

		public bool IsSortDesc
		{
			get	{ return bool.Parse(hiddenIsSortDesc.Value); }
			set { hiddenIsSortDesc.Value = value.ToString(); }					
		}

		public string SortColumName
		{
			get { return this.hiddenSortColumnName.Value; }
			set { this.hiddenSortColumnName.Value = value;}
		}

		public int CellPadding
		{
			get { return table.CellPadding; }
			set { table.CellPadding = value;}
		}

		public int CellSpacing
		{
			get { return table.CellSpacing; }
			set { table.CellSpacing = value;}
		}

		// scroll table
		public bool IsEnableScroll
		{
			get { return this._isEnableScroll; }
			set { this._isEnableScroll = value;}
		}
					
		/// <summary>
		/// ���û��ȡ�����ʾ�Ŀ��
		/// </summary>
		public string Width
		{
			get{return _tableFrameWidth;}
			//			set{_tableFrameWidth = value;}

			set
			{
				_tableFrameWidth = value;
				this.tableFrame.Attributes["width"] = value;
			}
		}
		
		/// <summary>
		/// ���û��ȡ�����ʾ�ĸ߶�
		/// </summary>
		public string Height
		{
			get{return _tableFrameHeight;}
			set{_tableFrameHeight = value;}
		}

		/// <summary>
		/// ���û��ȡ��߶����еĸ�����ȡֵ��ΧΪ1���ϲ���Ԫ��ǰ���������
		/// ����ж����к�����е�Ԫ��ϲ����ؼ��Զ������Щ��Ԫ��
		/// </summary>
		public int FreezeColumnCount
		{
			get{return _freezeColumnCount;}
			set{_freezeColumnCount = value;}
		}
	

		/// <summary>
		/// �������м��С�����ͱ�ı�������һ�£��������к����¸��еĵ�Ԫ����������
		/// �ϲ����������������������ʾ��
		/// </summary>
		public int TitleRowCount
		{
			get{return _titleRowCount;}
			set{_titleRowCount = value;}
		}

		#endregion
		
		#region �¼�
		public delegate void HeadCellClickedHandler(Object sender,SortEventArgs e);
		public event HeadCellClickedHandler HeadCellClicked = null;
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			// �ڴ˴������û������Գ�ʼ��ҳ��			
			labelJs.Text = "";

			table.BorderWidth = this._borderWidth;
			table.BorderColor = this._borderColor;
			table.BorderStyle = this._borderStyle;
			table.CellSpacing = 0;
			
			SetScript();		
			
		}

		
		/// <summary>
		/// ��labelScript�У���дһ��JavaScript��䡣��������а���һ��JavaScript�������ѱ���굥����TRԪ����������ɫ������ʾ��
		/// </summary>
		private void SetScript()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("\r\n<SCRIPT Language=\"JavaScript\">\r\n");
			sb.Append("var " + this.ClientID + "_lastHighlightTR;");
			sb.Append("var " + this.ClientID + "_lastHighlightTRForeColor;");
			sb.Append("var " + this.ClientID + "_lastHighlightTRBackColor;");
			sb.Append("function " + this.ClientID + "_highlightRow(obj)\r\n");
			sb.Append("{\r\n");
			sb.Append("var highlightRowForeColor = \"" + GetHexColorString(this._highlightRowForeColor) + "\"\r\n" );
			sb.Append("var highlightRowBackColor = \"" + GetHexColorString(this._highlightRowBackColor) + "\"\r\n" );
			sb.Append("var lastHighlightTR = " + this.ClientID + "_lastHighlightTR;\r\n" );
			sb.Append("var lastHighlightTRForeColor = " + this.ClientID + "_lastHighlightTRForeColor;\r\n");
			sb.Append("var lastHighlightTRBackColor = " + this.ClientID + "_lastHighlightTRBackColor;\r\n" );
			sb.Append("var trid; ");
			sb.Append(@"
						if(obj && obj.tagName ==""TR"")
						{
							// 
							var anotherTr;
							var anotherLastHighlightTr;
				");
			sb.Append("anotherTr = " + this.ClientID + "_getAnotherTr(obj);\r\n");
			sb.Append(@"		
							if(lastHighlightTR)
				");
			sb.Append("anotherLastHighlightTr = " + this.ClientID + "_getAnotherTr(lastHighlightTR)\r\n");
			sb.Append(@"

							//
							if(lastHighlightTR)
							{
								if(lastHighlightTR == obj)
									return;

								lastHighlightTR.style.color = lastHighlightTRForeColor;
								lastHighlightTR.style.backgroundColor = lastHighlightTRBackColor;
								
								if(anotherLastHighlightTr)
								{
									anotherLastHighlightTr.style.color = lastHighlightTRForeColor;
									anotherLastHighlightTr.style.backgroundColor = lastHighlightTRBackColor;
								}
							}
							trid = obj.id;
							
							lastHighlightTR = obj;
							lastHighlightTRForeColor = obj.style.color;
							lastHighlightTRBackColor = obj.style.backgroundColor;
							
							obj.style.color = highlightRowForeColor;
							obj.style.backgroundColor = highlightRowBackColor;
							
							if(anotherTr)
							{
								anotherTr.style.color = highlightRowForeColor;
								anotherTr.style.backgroundColor = highlightRowBackColor;							
							}							
						}						 
					  ");
            sb.Append("if(trid) document.getElementById(\"" + hiddenHighlightRowID.ClientID + "\").value = trid;\r\n");
            //sb.Append("if(trid) document.all(\"" + hiddenHighlightRowID.ClientID + "\").value = trid;\r\n");
			sb.Append("if(lastHighlightTR) \r\n");
			sb.Append("{\r\n");
			sb.Append(this.ClientID +  "_lastHighlightTR = lastHighlightTR;\r\n");
			sb.Append(this.ClientID +  "_lastHighlightTRForeColor = lastHighlightTRForeColor;\r\n");
			sb.Append(this.ClientID +  "_lastHighlightTRBackColor = lastHighlightTRBackColor;\r\n");
			sb.Append("}\r\n");
			sb.Append("}\r\n");

			#region ��ʼ�������ֶ�
			sb.Append("function " + this.ClientID + "_onHeadClick(columnName)\r\n");
			sb.Append("{\r\n");
			//sb.Append("document.all(\"" + hiddenSortColumnName.ClientID + "\").value = columnName;\r\n");
            sb.Append("document.getElementById(\"" + hiddenSortColumnName.ClientID + "\").value = columnName;\r\n");
			sb.Append("var sortHref = document.getElementById(\"" + linkButtonSort.ClientID + "\");\r\n");
			sb.Append("eval(sortHref.href);\r\n");
			sb.Append("}\r\n");
			#endregion

			#region ��ͬλ�еĺ���
			sb.Append("function " + this.ClientID + "_getAnotherTr(obj)\r\n");
			sb.Append(@"					
					{
						var rowIndex = obj.rowIndex;
						var parent = obj.parentElement;
						while(parent.tagName != 'TABLE')
						{
							parent = parent.parentElement;
						}						
						var tableID = parent.id;						
						var anotherTableID;
						if(tableID.indexOf('Freeze') == -1)
							anotherTableID = tableID += 'Freeze';
						else
							anotherTableID = tableID.substring(0,tableID.length - 6);
						
						var anotherTable = document.getElementById(anotherTableID);
						var anotherObj;
						if(anotherTable != null)
							anotherObj = anotherTable.rows[rowIndex];
						return anotherObj;
					}
				");
			#endregion
            //20120420 chencheng ��Ŀ������4.0����Ļ�ȡ����ķ�ʽ
            //string tmp = "document.getElementById(\""+ hiddenHighlightRowID.ClientID + "\").value";
            //sb.Append(this.ClientID + "_highlightRow(document.getElementById(\"<%=" + tmp + ".ClientID%>\"))\r\n");
            //20120730 chencheng ע�͸���
            //sb.Append(this.ClientID + "_highlightRow(document.all(document.getElementById(\"" + hiddenHighlightRowID.ClientID + "\").value))\r\n");

			//Add By Zhangbo
			sb.Append("function "+ this.ClientID +"_OnMouseOverSortTable(obj,sortObj)\r\n");
			sb.Append("{\r\n"); 
			sb.Append("if(obj)\r\n");
			sb.Append("{\r\n");
			sb.Append("var sort_objID = \""+ this.ClientID +"_\"+sortObj;\r\n");
			sb.Append("var sort_obj = obj.getElementsByTagName(\"table\");\r\n");
			sb.Append("sort_obj = sort_obj[0];\r\n");
			
			sb.Append("if(sort_obj)\r\n");
			sb.Append("{\r\n");
			sb.Append("obj.className = \"OraTableColumnHeader OraSortTableBorder1111\";\r\n");
			sb.Append("sort_obj.className = \"OraTableSortableColumnHeader  OraSortBorder1111\";\r\n");
			sb.Append("}\r\n");
			sb.Append("}\r\n");
			sb.Append("}\r\n");
			
			sb.Append("function " + this.ClientID + "_OnMouseOutSortTable(obj,sortObj)");
			sb.Append("{\r\n");
			sb.Append("if(obj)\r\n");
			sb.Append("{\r\n");
			sb.Append("var sort_objID = \""+ this.ClientID +"_\"+sortObj;\r\n");
			sb.Append("var sort_obj = obj.getElementsByTagName(\"table\");\r\n");
			sb.Append("sort_obj = sort_obj[0];\r\n");

			sb.Append("if(sort_obj)\r\n");
			sb.Append("{\r\n");
			sb.Append("obj.className = \"OraTableColumnHeader OraTableBorder0001\";\r\n");
			sb.Append("sort_obj.className = \"OraTableSortableColumnHeader\";\r\n");
			sb.Append("}\r\n");
			sb.Append("}\r\n");
			sb.Append("}\r\n");


			sb.Append("</SCRIPT>\r\n");
			labelScript.Text = sb.ToString();				
		}

		/// <summary>
		/// �ɸ�������ɫ����16������ɫ�ַ��� �磺#FFFFFF.
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		private string GetHexColorString(Color color)
		{
			return System.String.Format("#{0}{1}{2}",color.R.ToString("X2"),color.G.ToString("X2"),color.B.ToString("X2"));			
		}

		/// <summary>
		/// �Ƴ�������
		/// </summary>
		public void Clear()
		{
			table.Rows.Clear();
		}

		/// <summary>
		/// �ڱ�������һ����ͷ�У������ظñ�ͷ��
		/// </summary>
		/// <returns></returns>
		public TableRow AddHeadRow()
		{
			TableRow headRow = new TableRow();
        
			if(_headerRowClass != String.Empty)
			{
				headRow.Attributes["Class"] = _headerRowClass;
			}

			table.Rows.Add(headRow);
			return headRow;
		}

		/// <summary>
		/// ��ӿ�����ı�ͷ��Ԫ�񣬲����ظõ�Ԫ��
		/// </summary>
		/// <param name="headRow">��ͷ�ж��������</param>
		/// <param name="text">��ͷҪ��ʾ���ı�</param>
		/// <param name="columnName">��������DataTable��ColumnName��Ӧ</param>
		/// <param name="cellWidth">�����п�</param>		
		/// <returns>������ӵı�ͷ��Ԫ��</returns>
		public TableCell AddHeadCell(
			TableRow headRow,
			string text,		
			string columnName,
			int cellWidth	
			)
		{
			TableCell tableCell = AddHeadCell(headRow,text,cellWidth);
			tableCell.Text = "";	

			if(this._isSortClickedColumn)
			{
				Table subtable = new Table();
				subtable.ID = "Sort_SubTable" + columnName.Replace("\'","\\\'");
				subtable.Attributes.Add("width","100%");
				subtable.Attributes.Add("height","100%");
		
				TableRow subtableRow = new TableRow();
				TableCell subtableCellLeft = new TableCell();
				TableCell subtableCellRight = new TableCell();
				subtableCellLeft.Attributes.Add("width","100%");
				subtableCellLeft.Attributes.Add("height","100%");
				subtableCellRight.Attributes.Add("width","100%");
				subtableCellRight.Attributes.Add("height","100%");

				//Add By Zhangbo
				if(_headerIconButtonClass != String.Empty)
				{
					subtableCellRight.Attributes["Class"] = _headerIconButtonClass;
				}
				
				subtableRow.Cells.Add(subtableCellLeft);
				subtableRow.Cells.Add(subtableCellRight);
				subtable.Rows.Add(subtableRow);
				tableCell.Controls.Add(subtable);
				
				text = "<font class='"+_sortableHeaderLinkClass+"'>" + text +"</font>";

			
				// ����������
				if(columnName != string.Empty)
				{
					tableCell.Attributes["onclick"] = "JavaScript:" + this.ClientID + "_onHeadClick(\'" + columnName.Replace("\'","\\\'") + "\');";
					tableCell.Attributes["onmouseover"] = "javascript:" + this.ClientID + "_OnMouseOverSortTable(this,'"+subtable.ID+"');";
					tableCell.Attributes["onmouseout"]  ="javascript:" +this.ClientID +"_OnMouseOutSortTable(this,'"+subtable.ID+"');";
					subtableCellLeft.Text = text;
				}
				else
				{
					subtableCellLeft.Text = text;	
				}

				// �����¼�ͷ
				if(_sortImageUrl != string.Empty)
				{
					string sortColumnName = hiddenSortColumnName.Value;
					string isSortDesc = hiddenIsSortDesc.Value;			
					if(columnName == sortColumnName && columnName != string.Empty)
					{

						if(isSortDesc == "True")
						{
							subtableCellRight.Text += "<img src=\"" + this._sortDescImageUrl + "\">";
						}
						else
						{
							subtableCellRight.Text += "<img src=\"" + this._sortImageUrl + "\">";
						}
					}
				}

			}
			else
			{
				tableCell.Text = text;	
			}
			
			return tableCell;
		}

		/// <summary>
		/// ��ӱ�ͷ��Ԫ�񣬲����ظõ�Ԫ��
		/// </summary>
		/// <param name="headRow">��ͷ�ж��������</param>
		/// <param name="text">��ͷҪ��ʾ���ı�</param>		
		/// <param name="cellWidth">�����п�</param>		
		/// <returns>���ر�ͷ��Ԫ��</returns>
		public TableCell AddHeadCell(
			TableRow headRow,
			string text,					
			int cellWidth	
			)
		{
			TableCell tableCell = new TableCell();			
			tableCell.Width = Unit.Pixel(cellWidth);
			//			tableCell.BorderWidth = this._cellBorderWidth;
			//			tableCell.BorderStyle = this._cellBorderStyle;
			//			tableCell.BorderColor = this._cellBorderColor;
			if(this._headCellCssClass != string.Empty)
				tableCell.CssClass = this._headCellCssClass;

			tableCell.Text = text;				
			headRow.Cells.Add(tableCell);
			return tableCell;
		}
		
		
		public TableRow AddEmptyRow()
		{
			if(this.table.Rows == null)
				return null;

			TableRow sRow = this.table.Rows[0];
			TableRow newRow = new TableRow();

			if(_bodyRowClass != String.Empty)
			{
				newRow.Attributes["Class"] = _bodyRowClass;
			}

			TableCell cell;

			for(int i=0;i<sRow.Cells.Count;i++)
			{
				cell = new TableCell();

				if(sRow.Cells[i].ColumnSpan > 0)
				{
					cell.ColumnSpan = sRow.Cells[i].ColumnSpan;
				}
				cell.Text = "&nbsp;";

				newRow.Cells.Add(cell);
			}

			this.table.Rows.Add(newRow);

			return newRow;

		}
		/// <summary>
		/// ��ӱ�ͷ��Ԫ�񣬲����ظõ�Ԫ��
		/// </summary>
		/// <param name="headRow">��ͷ�ж��������</param>
		/// <param name="text">��ͷҪ��ʾ���ı�</param>		
		/// <returns>���ر�ͷ��Ԫ��</returns>
		public TableCell AddHeadCell(
			TableRow headRow,
			string text)
		{
			TableCell tableCell = new TableCell();			
			if(this._headCellCssClass != string.Empty)
				tableCell.CssClass = this._headCellCssClass;

			tableCell.Text = text;				
			headRow.Cells.Add(tableCell);
			return tableCell;
		}


		/// <summary>
		/// ���һ�� Body �У������ظ���
		/// </summary>
		/// <param name="rowID">�б�ʶ</param>
		/// <returns></returns>
		public TableRow AddBodyRow(string rowID)
		{
			TableRow bodyRow = AddBodyRow();			
			bodyRow.ID = rowID;			
			return bodyRow;
		}

		/// <summary>
		/// ���һ�� Body �У������ظ���
		/// </summary>		
		/// <returns></returns>
		public TableRow AddBodyRow()
		{
			TableRow bodyRow = new TableRow();
			
			if(!this._isEnableScroll)
				bodyRow.Height = this._bodyRowHeight;			
			//			bodyRow.VerticalAlign = VerticalAlign.Middle;
			//			bodyRow.ForeColor = _bodyRowForeColor;
			//			bodyRow.BackColor = _bodyRowBackColor;
			//			bodyRow.Font.Size = _bodyFontSize;

			bodyRow.Attributes["Class"] = _bodyRowClass;
			
			if(this._isHighlightClickedRow)
				bodyRow.Attributes.Add("onclick","JavaScript:" + this.ClientID + "_highlightRow(this)");

			if(this._isSwitchBodyRowColor)
			{	
				if(this._isSwitchColorRow)
					bodyRow.Attributes["Class"] = _switchBodyRowClass;
				//					bodyRow.BackColor = this._bodyRowBackSwitchColor;
				_isSwitchColorRow = !_isSwitchColorRow;
			}
			
			table.Rows.Add(bodyRow);
			return bodyRow;
		}

		/// <summary>
		/// ���һ����Ԫ�񣬲����ظõ�Ԫ��
		/// </summary>
		/// <param name="bodyRow">Ҫ��ӵ�Ԫ�����</param>
		/// <param name="text">��ӵ�����</param>
		/// <param name="horizontalAlign">ˮƽλ��</param>
		/// <returns></returns>
		public TableCell AddCell(TableRow tableRow,string text,HorizontalAlign horizontalAlign)
		{
			TableCell tableCell = AddCell(tableRow,text);			
			tableCell.HorizontalAlign = horizontalAlign;			
			return tableCell;			
		}
		
		/// <summary>
		/// ���һ����Ԫ�񣬲����ظõ�Ԫ��
		/// </summary>
		/// <param name="bodyRow">Ҫ��ӵ�Ԫ�����</param>
		/// <param name="text">��ӵ�����</param>
		/// <param name="horizontalAlign">ˮƽλ��</param>
		/// <returns></returns>
		public TableCell AddCell(TableRow tableRow,string text,HorizontalAlign horizontalAlign,int cellWidth)
		{
			TableCell tableCell = AddCell(tableRow,text);			
			tableCell.HorizontalAlign = horizontalAlign;
			tableCell.Width = Unit.Pixel(cellWidth);
			return tableCell;			
		}

		/// <summary>
		/// ���һ����Ԫ�񣬲����ظõ�Ԫ��
		/// </summary>
		/// <param name="bodyRow">Ҫ��ӵ�Ԫ�����</param>
		/// <param name="text">��ӵ�����</param>		
		/// <returns></returns>
		public TableCell AddCell(TableRow tableRow,string text)
		{
			TableCell tableCell = new TableCell();	
			
			//			tableCell.BorderWidth = this._cellBorderWidth;
			//			tableCell.BorderStyle = this._cellBorderStyle;
			//			tableCell.BorderColor = this._cellBorderColor;
			if(this._bodyCellCssClass != string.Empty)
				tableCell.CssClass = this._bodyCellCssClass;
			
			tableCell.Text = text;
			//			tableCell.HorizontalAlign = HorizontalAlign.Left;	

			tableRow.Cells.Add(tableCell);
			return tableCell;
		}

		/// <summary>
		/// ���һ����Ԫ�񣬲����ظõ�Ԫ��
		/// </summary>
		/// <param name="bodyRow">Ҫ��ӵ�Ԫ�����</param>
		/// <param name="text">��ӵ�����</param>
		/// <param name="horizontalAlign">ˮƽλ��</param>
		/// <param name="borderStyle">��Ԫ��߿���ʽ</param>
		/// <returns></returns>
		public TableCell AddCell(
			TableRow tableRow,
			string text,
			HorizontalAlign horizontalAlign,
			int colspan,
			int rowspan,
			int height)
		{
			TableCell tableCell = this.AddCell(tableRow,text,horizontalAlign);
			tableCell.ColumnSpan = colspan;
			tableCell.RowSpan = rowspan;
			tableCell.Height = new Unit(height);

			return tableCell;
		}

		/// <summary>
		/// ���һ����β��
		/// </summary>
		/// <returns></returns>
		public TableRow AddFootRow()
		{
			TableRow footRow = new TableRow();
			
			footRow.Height = this._footRowHeight;			
			footRow.VerticalAlign = VerticalAlign.Middle;
			footRow.ForeColor = _footRowForeColor;
			footRow.BackColor = _footRowBackColor;	

			table.Rows.Add(footRow);
			return footRow;
		}

		/// <summary>
		/// ��Ӧ�����¼�
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void linkButtonSort_Click(object sender, System.EventArgs e)
		{
			if(hiddenIsSortDesc.Value == "True")
				hiddenIsSortDesc.Value = "False";
			else
				hiddenIsSortDesc.Value = "True";
						
			if(null != HeadCellClicked)
			{
				SortEventArgs arg = new SortEventArgs(hiddenSortColumnName.Value,bool.Parse(hiddenIsSortDesc.Value));
				HeadCellClicked(this,arg);
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
			this.linkButtonSort.Click += new System.EventHandler(this.linkButtonSort_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		/// <summary>
		/// ���ò���
		/// </summary>		
		/// <param name="tableWidthToShow">�����ʾ�Ŀ��</param>
		/// <param name="tableHeightToShow">�����ʾ�ĸ߶�</param>
		/// <param name="freezeColumnCount">��߶����еĸ���</param>
		/// <param name="titleRowCount">�������м���</param>
		public void Reset(int tableWidthToShow,
			int tableHeightToShow,int freezeColumnCount,int titleRowCount)
		{
			_tableFrameWidth = tableWidthToShow.ToString();
			_tableFrameHeight = tableHeightToShow.ToString();
			_freezeColumnCount = freezeColumnCount;
			_titleRowCount = titleRowCount;
		}
	
		protected override void OnPreRender(EventArgs e)
		{
			// TODO:  ��� BmScrollTable.OnPreRender ʵ��
			base.OnPreRender (e);

			if(!this._isEnableScroll)
			{
				table.Attributes.Add("width",this._tableFrameWidth);
				return;
			}

			// Ϊҳ�����JavaScript����
			InitializeJsFunctions();
			

			// ������Ե���Ч��
			if(this.table == null)
				return;
			
			if(this._titleRowCount < 0)
				return;

			if(this._freezeColumnCount < 0)
				return;

			if(this._titleRowCount > this.table.Rows.Count)
				return;

			// ���ÿؼ�����			
			table.Style.Add("TABLE-LAYOUT","fixed");
			SetControlsAttributes();
						
			// ��Table����ҳ����
			this.table.ID = "tableMain";
			this.table.EnableViewState = false;			
					
			// ���ñ����ʾ��Ⱥ͸߶�
			tableFrame.Width = this._tableFrameWidth.ToString();
			tableFrame.Height = this._tableFrameHeight.ToString();
			tableFrameTd.Width = this._tableFrameWidth.ToString();
			tableFrameTd.Height = this._tableFrameHeight.ToString();

			// ��ҳ�����ֱ��ִ�е�JavaScript���
			AddJsCommand();			
		}

		/// <summary>
		/// ��JavaScript������д��labelJsFunctions��
		/// </summary>
		private void InitializeJsFunctions()
		{
			labelJsFunctions.Text = "";

			StringBuilder sb = new StringBuilder();
			
			sb.Append("<script language=\"JavaScript\">\r\n");		
			
			sb.Append(AddJsfScrollAll());
			sb.Append("\r\n");
			sb.Append(AddJsfVerticalScrollMain());
			sb.Append("\r\n");
			sb.Append(AddJsfCellMappings());
			sb.Append("\r\n");
			sb.Append(AddJsfMAISRSell());
			sb.Append("\r\n");			
			sb.Append(AddJsfDivideLockedCell());
			sb.Append("\r\n");
			sb.Append(AddJsfDeleteFirstNColumn());
			sb.Append("\r\n");
			sb.Append(AddJsfDeleteAfterFirstNColumn());
			sb.Append("\r\n");
			sb.Append(AddJsfRecordCellSize());
			sb.Append("\r\n");
			sb.Append(AddJsfMoveTitle());
			sb.Append("\r\n");
			sb.Append(AddJsfMoveTitleFreeze());
			sb.Append("\r\n");
			sb.Append(AddJsfMoveMainFreeze());
			sb.Append("\r\n");
			sb.Append(AddJsfAdjustDivPosition());
			sb.Append("\r\n");			
			sb.Append(AddJsfLockTable());
			sb.Append("\r\n");
			sb.Append(AddJsfOnDocumentReadyStateChange());			
			sb.Append("\r\n");
			sb.Append(AddJsfOnTableResize());
			sb.Append("\r\n");
			sb.Append(AddJsfOnPropertyChange());
			sb.Append("\r\n");
			sb.Append("</script>\r\n");			

			labelScript.Text += sb.ToString();
		}

		/// <summary>
		/// ���ø��ؼ�������
		/// </summary>
		private void SetControlsAttributes()
		{
			// tableFrame
			this.tableFrame.Attributes["style"] = "TABLE-LAYOUT: fixed";

			// divTitleFreeze
			this.divTitleFreeze.Attributes["style"] = ""
				+ "OVERFLOW: scroll; "				
				+ "OVERFLOW-Y: hidden; "
				+ "OVERFLOW-X: hidden; "
				+ "BACKGROUND-COLOR: white; ";

			// divTitle
			this.divTitle.Attributes["style"] = ""
				+ "OVERFLOW: scroll; "
				+ "OVERFLOW-Y: hidden; "
				+ "OVERFLOW-X: hidden; "				
				+ "POSITION: relative; "
				+ "BACKGROUND-COLOR: white; ";
						
			// divMainFreeze
			this.divMainFreeze.Attributes["style"] = ""
				+ "VERFLOW: scroll; "
				+ "OVERFLOW-Y: auto; "
				+ "OVERFLOW-X: hidden; "
				+ "POSITION: relative; "
				+ "BACKGROUND-COLOR: white; ";

			// divMain
			this.divMain.Attributes["style"] = ""
				+ "OVERFLOW: auto; "
				+ "OVERFLOW-Y: auto; "
				+ "OVERFLOW-X: auto; "
				+ "POSITION: relative; "
				+ "BACKGROUND-COLOR: white; ";	
			this.tableFrameTd.VAlign="Top";

		}

		/// <summary>
		/// ��ֱ��ִ�е�JavaScript�����ӵ�labelJs��
		/// </summary>
		private void AddJsCommand()
		{
				
			StringBuilder sb = new StringBuilder();
			
			sb.Append("<script language=\"JavaScript\">\r\n");
			sb.Append("document.attachEvent(\"onreadystatechange\"," + this.ClientID + "_onDocumentReadyStateChange);\r\n");
			sb.Append("</script>\r\n");

			labelScript.Text += sb.ToString();
		}

		/// <summary>
		/// ���� JavaScript ���� ScrollAll ���ַ���
		/// </summary>
		private string AddJsfScrollAll()
		{
			/*
				function scrollAll()
				{					
					divTitle.scrollLeft = divMain.scrollLeft;
					divMainFreeze.scrollTop = divMain.scrollTop;
				}
			*/

			StringBuilder sb = new StringBuilder();
			sb.Append("function " + this.ClientID + "_scrollAll()\r\n");
			sb.Append("{\r\n");
			sb.Append(this.divTitle.ClientID + ".scrollLeft = " + this.divMain.ClientID + ".scrollLeft;\r\n");
			sb.Append(this.divMainFreeze.ClientID + ".scrollTop = " + this.divMain.ClientID + ".scrollTop;\r\n");
			sb.Append("}\r\n");
			return sb.ToString();
		}

		/// <summary>
		/// ���� JavaScript ���� VerticalScrollMain ���ַ���
		/// </summary>
		private string AddJsfVerticalScrollMain()
		{
			/*
				function verticalScrollMain()
				{		
					divMain.scrollTop = divMainFreeze.scrollTop;
				}
			*/
			StringBuilder sb = new StringBuilder();
			sb.Append("function " + this.ClientID + "_verticalScrollMain()\r\n");
			sb.Append("{\r\n");	
			sb.Append(this.divMain.ClientID + ".scrollTop = " + this.divMainFreeze.ClientID + ".scrollTop;\r\n");
			sb.Append("}\r\n");
			return sb.ToString();
		}

		/// <summary>
		/// ���� JavaScript ���� CellMappings ���ַ���
		/// </summary>
		private string AddJsfCellMappings()
		{
			/*
				// ����һ���������ÿ����Ԫ���Ƿ�ϲ��Լ���Dom����Cell�Ķ�Ӧ��ϵ
				function cellMappings(rowIndex,colIndex)
				{
					this.rowIndex = rowIndex;
					this.colIndex = colIndex;
				}
			*/
			StringBuilder sb = new StringBuilder();
			sb.Append("function " + this.ClientID + "_cellMappings(rowIndex,colIndex)\r\n");
			sb.Append("{\r\n");	
			sb.Append("this.rowIndex = rowIndex;\r\n");
			sb.Append("this.colIndex = colIndex;\r\n");
			sb.Append("}\r\n");
			return sb.ToString();
		}

		/// <summary>
		/// ���� JavaScript ���� MAISRSell ���ַ���
		/// </summary>
		private string AddJsfMAISRSell()
		{
			/*
				// ����һ������û�ϲ���Ԫ��ǰ����Table���Ͻǿ�ʼ��nRowCount�У�nColCount�з�Χ�ڵ�Ԫ��ͺϲ���Ԫ��Ķ�Ӧ��ϵ�����顣
				// �����ÿ��Ԫ����һ��cellMappings���󣬱���ϲ�ǰ��Ԫ���Ӧ��Dom����Cell�������Ϊ�ϲ���ĵ�Ԫ�񣩵�λ�á�
				function mAISRSell(mapedTable,nRowCount,nColCount)
				{					
					
					//��һά���鱣���ά��������				
					var aCellMapping = new Array(nRowCount * nColCount);
				
					var iColIndex;
					for(var iRowIndex=0; iRowIndex<nRowCount; iRowIndex++)
					{
						iColIndex = 0;
						for(var iCellIndex=0; iCellIndex<mapedTable.rows[iRowIndex].cells.length; iCellIndex++)
						{
							while(iColIndex < nColCount)
							{
								if(aCellMapping[iRowIndex*nColCount + iColIndex] != null)
									iColIndex++;
								else
									break;
							}
						
							if(iColIndex == nColCount)
								break;
						
							var currentCell = mapedTable.rows[iRowIndex].cells[iCellIndex];
										
							// ����ϲ��ĵ�Ԫ���DOM��CELL����Ķ�Ӧ��ϵ
							for(var i=iRowIndex; i<iRowIndex + currentCell.rowSpan; i++)
							{
								for(var j=iColIndex; j<iColIndex + currentCell.colSpan; j++)
								{
									aCellMapping[i*nColCount + j] = new cellMappings(iRowIndex,iCellIndex);
								}					
							}
										
							iColIndex += currentCell.colSpan;				
						}
					}
					return aCellMapping;
				}
			*/
			StringBuilder sb = new StringBuilder();
			sb.Append("function " + this.ClientID + "_mAISRSell(mapedTable,nRowCount,nColCount)\r\n");
			sb.Append("{\r\n");	
			sb.Append(@"
					//��һά���鱣���ά��������				
					var aCellMapping = new Array(nRowCount * nColCount);
				
					var iColIndex;
					for(var iRowIndex=0; iRowIndex<nRowCount; iRowIndex++)
					{
						iColIndex = 0;
						for(var iCellIndex=0; iCellIndex<mapedTable.rows[iRowIndex].cells.length; iCellIndex++)
						{
							while(iColIndex < nColCount)
							{
								if(aCellMapping[iRowIndex*nColCount + iColIndex] != null)
									iColIndex++;
								else
									break;
							}
						
							if(iColIndex == nColCount)
								break;
						
							var currentCell = mapedTable.rows[iRowIndex].cells[iCellIndex];
										
							// ����ϲ��ĵ�Ԫ���DOM��CELL����Ķ�Ӧ��ϵ
							for(var i=iRowIndex; i<iRowIndex + currentCell.rowSpan; i++)
							{
								for(var j=iColIndex; j<iColIndex + currentCell.colSpan; j++)
								{
				");
			sb.Append("aCellMapping[i*nColCount + j] = new " + this.ClientID + "_cellMappings(iRowIndex,iCellIndex);\r\n");
			sb.Append(@"
								}					
							}
										
							iColIndex += currentCell.colSpan;				
						}
					}
					return aCellMapping;
				");
			

			sb.Append("}\r\n");
			return sb.ToString();
		}

		/// <summary>
		/// ���� JavaScript ���� DivideLockedCell ���ַ���
		/// </summary>
		private string AddJsfDivideLockedCell()
		{
			/*
				// ��ԭʼ��Ԫ����������������ĵ�Ԫ���û�����ĵ�Ԫ��ϲ�����Ӱ�������ĺϲ��ĵ�Ԫ��� 
				// ��Ϊ��ͷӦ�ú����ݽ�Ȼ�ֿ��������Ǵ�ֱ�ϲ�Ӱ�����������
				function divideLockedCell(lockedTable,nLockedRowCount,nLockedColCount)
				{
					// ��ȡԭʼ��Ԫ��ͺϲ���Ԫ��Ķ�Ӧ��ϵ
					var nTotalColCount = 0;
					for(var j=0; j<lockedTable.rows[0].cells.length;j++)
					{
						nTotalColCount += lockedTable.rows[0].cells[j].colSpan;
					}
					
					if (nLockedColCount >= nTotalColCount)
						return;
						
					var aCellMapping = mAISRSell(lockedTable,nLockedRowCount,nTotalColCount);
					
					// ����ÿ�е�ԭʼ��Ԫ��ĵ�nLockedRowCount��͵�nLockedRowCount + 1�����Ӧ����ͬһ���ϲ���ĵ�Ԫ��
					// ��Ѵ˺ϲ���ĵ�Ԫ��ˮƽ��ֳ�������Ԫ�����¼����Ӧ��ϵ��
					// ��ֺ��������Ԫ��Ŀ�ȣ������¸��������л�á�����޷����������л�ã����Դӵ�һ���������л��
					for(var i=0; i<nLockedRowCount; i++)
					{
						var leftCellMapping = aCellMapping[i*nTotalColCount + nLockedColCount - 1];
						var rightCellMapping = aCellMapping[i*nTotalColCount + nLockedColCount];
						if(leftCellMapping.rowIndex == rightCellMapping.rowIndex && leftCellMapping.colIndex == rightCellMapping.colIndex )
						{
							// ˮƽ��ֵ�Ԫ��
							var dividedCell = lockedTable.rows[leftCellMapping.rowIndex].cells[leftCellMapping.colIndex];
							// �����ֺ�Ԫ��Ŀ��
							// ���¸������β��Ҳ��ò�ֵĵ�Ԫ����ȷ����ֺ�������Ԫ��Ŀ�ȡ�
							var bFound = false;
							var iRowIndex = i+1;
							while(iRowIndex < nLockedRowCount)
							{
								var lCellMapping = aCellMapping[iRowIndex*nTotalColCount + nLockedColCount - 1];
								var rCellMapping = aCellMapping[iRowIndex*nTotalColCount + nLockedColCount];
								if(lCellMapping.rowIndex != rCellMapping.rowIndex || lCellMapping.colIndex != rCellMapping.colIndex)
								{
									bFound = true;
									break;
								}
								iRowIndex++;
							}
							
							
							if(!bFound)
							{
								//���Դӵ�һ���������л�ü�����������յ�Ԫ��
								if(nLockedRowCount<lockedTable.rows.length)
								{
									var nCheckedColCount = 0;
									for(var j=0; j<lockedTable.rows[nLockedRowCount].cells.length; j++)
									{
										nCheckedColCount += lockedTable.rows[nLockedRowCount].cells[j].colSpan;
										if(nCheckedColCount >= nLockedColCount)
											break;								
									}
									if(nCheckedColCount == nLockedColCount)
									{
										bFound =true;
										iRowIndex = nLockedRowCount;							
									}
								}
							}
							
							if(bFound)
							{
								// �����ֺ����ҵ�Ԫ��Ŀ��
								var rCellMapping = aCellMapping[iRowIndex*nTotalColCount + nLockedColCount];
								var lWidth = lockedTable.rows[rCellMapping.rowIndex].cells[rCellMapping.colIndex].offsetLeft - 
									dividedCell.offsetLeft - lockedTable.cellSpacing;
								var rWidth = dividedCell.offsetWidth - lWidth - lockedTable.cellSpacing;
																		
								// �����ֺ����ҵ�Ԫ���colSpan
								var iColIndex = nLockedColCount + 1;
								while(iColIndex < nTotalColCount)
								{
									var cellMapping = aCellMapping[i*nTotalColCount + j];
									if(cellMapping.rowIndex != leftCellMapping.rowIndex || cellMapping.colIndex != leftCellMapping.colIndex)
									{
										break;
									}
									iColIndex++;						
								}
								var rCellColSpan = iColIndex - nLockedColCount;
								var lCellColSapn = dividedCell.colSpan - rCellColSpan;
								
								// ˮƽ��ֵ�Ԫ��
								var clonedCell = dividedCell.cloneNode(true);
								
								dividedCell.style.width = lWidth;
								clonedCell.style.width = rWidth;
								
								dividedCell.colSpan = lCellColSapn;
								clonedCell.colSpan = rCellColSpan;
								
								clonedCell.innerHTML = "&nbsp;";
								
								if(leftCellMapping.colIndex < lockedTable.rows[leftCellMapping.rowIndex].cells.length -1)
								{
									var cellRefer = lockedTable.rows[leftCellMapping.rowIndex].cells[leftCellMapping.colIndex+1];
									lockedTable.rows[leftCellMapping.rowIndex].insertBefore(clonedCell,cellRefer);
								}
								else
								{	
									lockedTable.rows[leftCellMapping.rowIndex].insertBefore(clonedCell);
								}
													
								// ������Ӧ��ϵ
								aCellMapping = mAISRSell(lockedTable,nLockedRowCount,nTotalColCount);
							}				
						}
					}		
				}
			*/
			StringBuilder sb = new StringBuilder();
			sb.Append("function " + this.ClientID + "_divideLockedCell(lockedTable,nLockedRowCount,nLockedColCount)\r\n");
			sb.Append("{\r\n");	
			sb.Append(@"
					// ��ȡԭʼ��Ԫ��ͺϲ���Ԫ��Ķ�Ӧ��ϵ
					var nTotalColCount = 0;
					for(var j=0; j<lockedTable.rows[0].cells.length;j++)
					{
						nTotalColCount += lockedTable.rows[0].cells[j].colSpan;
					}
					
					if (nLockedColCount >= nTotalColCount)
						return;					
				");
			sb.Append("var aCellMapping = " + this.ClientID + "_mAISRSell(lockedTable,nLockedRowCount,nTotalColCount);\r\n");
			sb.Append(@"
					// ����ÿ�е�ԭʼ��Ԫ��ĵ�nLockedRowCount��͵�nLockedRowCount + 1�����Ӧ����ͬһ���ϲ���ĵ�Ԫ��
					// ��Ѵ˺ϲ���ĵ�Ԫ��ˮƽ��ֳ�������Ԫ�����¼����Ӧ��ϵ��
					// ��ֺ��������Ԫ��Ŀ�ȣ������¸��������л�á�����޷����������л�ã����Դӵ�һ���������л��
					for(var i=0; i<nLockedRowCount; i++)
					{
						var leftCellMapping = aCellMapping[i*nTotalColCount + nLockedColCount - 1];
						var rightCellMapping = aCellMapping[i*nTotalColCount + nLockedColCount];
						if(leftCellMapping.rowIndex == rightCellMapping.rowIndex && leftCellMapping.colIndex == rightCellMapping.colIndex )
						{
							// ˮƽ��ֵ�Ԫ��
							var dividedCell = lockedTable.rows[leftCellMapping.rowIndex].cells[leftCellMapping.colIndex];
							// �����ֺ�Ԫ��Ŀ��
							// ���¸������β��Ҳ��ò�ֵĵ�Ԫ����ȷ����ֺ�������Ԫ��Ŀ�ȡ�
							var bFound = false;
							var iRowIndex = i+1;
							while(iRowIndex < nLockedRowCount)
							{
								var lCellMapping = aCellMapping[iRowIndex*nTotalColCount + nLockedColCount - 1];
								var rCellMapping = aCellMapping[iRowIndex*nTotalColCount + nLockedColCount];
								if(lCellMapping.rowIndex != rCellMapping.rowIndex || lCellMapping.colIndex != rCellMapping.colIndex)
								{
									bFound = true;
									break;
								}
								iRowIndex++;
							}
							
							
							if(!bFound)
							{
								//���Դӵ�һ���������л�ü�����������յ�Ԫ��
								if(nLockedRowCount<lockedTable.rows.length)
								{
									var nCheckedColCount = 0;
									for(var j=0; j<lockedTable.rows[nLockedRowCount].cells.length; j++)
									{
										nCheckedColCount += lockedTable.rows[nLockedRowCount].cells[j].colSpan;
										if(nCheckedColCount >= nLockedColCount)
											break;								
									}
									if(nCheckedColCount == nLockedColCount)
									{
										bFound =true;
										iRowIndex = nLockedRowCount;							
									}
								}
							}
							
							if(bFound)
							{
								// �����ֺ����ҵ�Ԫ��Ŀ��
								var rCellMapping = aCellMapping[iRowIndex*nTotalColCount + nLockedColCount];
								var lWidth = lockedTable.rows[rCellMapping.rowIndex].cells[rCellMapping.colIndex].offsetLeft - 
									dividedCell.offsetLeft - lockedTable.cellSpacing;
								var rWidth = dividedCell.offsetWidth - lWidth - lockedTable.cellSpacing;
																		
								// �����ֺ����ҵ�Ԫ���colSpan
								var iColIndex = nLockedColCount + 1;
								while(iColIndex < nTotalColCount)
								{
									var cellMapping = aCellMapping[i*nTotalColCount + j];
									if(cellMapping.rowIndex != leftCellMapping.rowIndex || cellMapping.colIndex != leftCellMapping.colIndex)
									{
										break;
									}
									iColIndex++;						
								}
								var rCellColSpan = iColIndex - nLockedColCount;
								var lCellColSapn = dividedCell.colSpan - rCellColSpan;
								
								// ˮƽ��ֵ�Ԫ��
								var clonedCell = dividedCell.cloneNode(true);
								
								dividedCell.style.width = lWidth;
								clonedCell.style.width = rWidth;
								
								dividedCell.colSpan = lCellColSapn;
								clonedCell.colSpan = rCellColSpan;
				");
			sb.Append("clonedCell.innerHTML = \"&nbsp;\"\r\n");
			sb.Append(@"
								
								if(leftCellMapping.colIndex < lockedTable.rows[leftCellMapping.rowIndex].cells.length -1)
								{
									var cellRefer = lockedTable.rows[leftCellMapping.rowIndex].cells[leftCellMapping.colIndex+1];
									lockedTable.rows[leftCellMapping.rowIndex].insertBefore(clonedCell,cellRefer);
								}
								else
								{	
									lockedTable.rows[leftCellMapping.rowIndex].insertBefore(clonedCell);
								}
													
								// ������Ӧ��ϵ
				");
			sb.Append("aCellMapping = " + this.ClientID + "_mAISRSell(lockedTable,nLockedRowCount,nTotalColCount);\r\n");
			sb.Append(@"
							}				
						}
					}		
					
				");
			sb.Append("}\r\n");
			return sb.ToString();
		}

		/// <summary>
		/// ���� JavaScript ���� DeleteFirstNColumn ���ַ���
		/// </summary>
		/// <returns></returns>
		private string AddJsfDeleteFirstNColumn()
		{
			/*
				// ɾ��Table��ǰ���С�	
				function deleteFirstNColumn(deletedTable,nCount)
				{
					var DELETED_TAG = "*Del_Cell*";
					
					// �ȱ��Ҫɾ����Cell
					var aCellMapping = mAISRSell(deletedTable,deletedTable.rows.length,nCount);
					for(var i=0;i<deletedTable.rows.length;i++)
					{
						for(var j=0;j<nCount;j++)		
						{
							var cellMapping = aCellMapping[i*nCount + j];
							deletedTable.rows[cellMapping.rowIndex].cells[cellMapping.colIndex].innerText = DELETED_TAG;
						}
					}
							
					// ɾ����ǹ���Cell
					for(var i=0;i<deletedTable.rows.length;i++)
					{			
						while(deletedTable.rows[i].cells.length > 0)
						{
							if(deletedTable.rows[i].cells[0].innerText == DELETED_TAG)
								deletedTable.rows[i].cells[0].removeNode();				
							else
								break;
						}			
					}
				} 
			*/
			StringBuilder sb = new StringBuilder();
			sb.Append("function " + this.ClientID + "_deleteFirstNColumn(deletedTable,nCount)\r\n");
			sb.Append("{\r\n");	
			sb.Append(@"
					var DELETED_TAG = '*Del_Cell*';
					
					// �ȱ��Ҫɾ����Cell
				");
			sb.Append("var aCellMapping = " + this.ClientID + "_mAISRSell(deletedTable,deletedTable.rows.length,nCount);\r\n");
			sb.Append(@"
					for(var i=0;i<deletedTable.rows.length;i++)
					{
						for(var j=0;j<nCount;j++)		
						{
							var cellMapping = aCellMapping[i*nCount + j];
							deletedTable.rows[cellMapping.rowIndex].cells[cellMapping.colIndex].innerText = DELETED_TAG;
						}
					}
									
					// ɾ����ǹ���Cell
					for(var i=0;i<deletedTable.rows.length;i++)
					{			
						while(deletedTable.rows[i].cells.length > 0)
						{
							if(deletedTable.rows[i].cells[0].innerText == DELETED_TAG)
								deletedTable.rows[i].cells[0].removeNode();				
							else
								break;
						}			
					}
				}");
			return sb.ToString();
		}

		/// <summary>
		/// ���� JavaScript ���� DeleteAfterFirstNColumn ���ַ���
		/// </summary>
		/// <returns></returns>
		private string AddJsfDeleteAfterFirstNColumn()
		{
			/*
				// ɾ��Table��ǰ�����Ժ���С�	
				function deleteAfterFirstNColumn(deletedTable,nCount)
				{
					if(deletedTable.rows.length <= 0)
						return;
						
					var DELETED_TAG = "*Del_Cell*";
					
					// �ȱ��Ҫɾ����Cell
					var totalColCount = 0;
					for(var i=0;i<deletedTable.rows[0].cells.length;i++)
					{
						totalColCount += deletedTable.rows[0].cells[i].colSpan;
					}
							
					var aCellMapping = mAISRSell(deletedTable,deletedTable.rows.length,totalColCount);
					for(var i=0;i<deletedTable.rows.length;i++)
					{
						for(var j=nCount;j<totalColCount;j++)
						{
							var cellMapping = aCellMapping[i*totalColCount + j];				
							deletedTable.rows[cellMapping.rowIndex].cells[cellMapping.colIndex].innerText = DELETED_TAG;
						}
					}
					
					// ɾ����ǹ���Cell		
					for(var i=0;i<deletedTable.rows.length;i++)
					{			
						while(deletedTable.rows[i].cells.length > 0)
						{
							if(deletedTable.rows[i].cells[deletedTable.rows[i].cells.length - 1].innerText == DELETED_TAG)				
								deletedTable.rows[i].cells[deletedTable.rows[i].cells.length - 1].removeNode();
							else
								break;
						}			
					}			
				} 
			*/
			StringBuilder sb = new StringBuilder();
			sb.Append("function " + this.ClientID + "_deleteAfterFirstNColumn(deletedTable,nCount)\r\n");
			sb.Append("{\r\n");	
			sb.Append(@"
					
						if(deletedTable.rows.length <= 0)
							return;
							
						var DELETED_TAG = '*Del_Cell*';
							
						// �ȱ��Ҫɾ����Cell
						var totalColCount = 0;
						for(var i=0;i<deletedTable.rows[0].cells.length;i++)
						{
							totalColCount += deletedTable.rows[0].cells[i].colSpan;
						}
					");
			sb.Append("var aCellMapping = " + this.ClientID + "_mAISRSell(deletedTable,deletedTable.rows.length,totalColCount);\r\n");
			sb.Append(@"											
						for(var i=0;i<deletedTable.rows.length;i++)
						{
							for(var j=nCount;j<totalColCount;j++)
							{
								var cellMapping = aCellMapping[i*totalColCount + j];				
								deletedTable.rows[cellMapping.rowIndex].cells[cellMapping.colIndex].innerText = DELETED_TAG;
							}
						}
								
						// ɾ����ǹ���Cell		
						for(var i=0;i<deletedTable.rows.length;i++)
						{			
							while(deletedTable.rows[i].cells.length > 0)
							{
								if(deletedTable.rows[i].cells[deletedTable.rows[i].cells.length - 1].innerText == DELETED_TAG)				
									deletedTable.rows[i].cells[deletedTable.rows[i].cells.length - 1].removeNode();
								else
									break;
							}			
						}			
					} 
				");
			return sb.ToString();
		}

		/// <summary>
		/// ���� JavaScript ���� RecordCellSize ���ַ���
		/// </summary>
		/// <returns></returns>
		private string AddJsfRecordCellSize()
		{
			/*
				//
				function recordCellSize(tableMain,nLockedRowCount)
				{
								
					for(var i=0; i< nLockedRowCount; i++)
					{
						tableMain.rows[i].style.height = tableMain.rows[i].offsetHeight;
						for(var j=0;j< tableMain.rows[i].cells.length;j++)
						{
							var cell = tableMain.rows[i].cells[j];
							
							cell.style.width = cell.offsetWidth;
							cell.style.height = cell.offsetHeight;
						}
					}
					for(var j=0;j< tableMain.rows[nLockedRowCount].cells.length;j++)
					{
						var cell = tableMain.rows[nLockedRowCount].cells[j];			
						cell.style.width = cell.offsetWidth;			
					}
					
					
					for(var i=nLockedRowCount; i<tableMain.rows.length; i++)
					{
						tableMain.rows[i].style.height = tableMain.rows[i].offsetHeight;			
					}
					
				}
			*/
			StringBuilder sb = new StringBuilder();
			sb.Append("function " + this.ClientID + "_recordCellSize(tableMain,nLockedRowCount)\r\n");
			sb.Append(@"
					{																					
						for(var i=0; i< nLockedRowCount; i++)
						{
							tableMain.rows[i].style.height = tableMain.rows[i].offsetHeight;
							for(var j=0;j< tableMain.rows[i].cells.length;j++)
							{
								var cell = tableMain.rows[i].cells[j];
								
								cell.style.width = cell.offsetWidth;
								cell.style.height = cell.offsetHeight;
							}
						}
						for(var j=0;j< tableMain.rows[nLockedRowCount].cells.length;j++)
						{
							var cell = tableMain.rows[nLockedRowCount].cells[j];			
							cell.style.width = cell.offsetWidth;			
						}
						
						
						for(var i=nLockedRowCount; i<tableMain.rows.length; i++)
						{
							tableMain.rows[i].style.height = tableMain.rows[i].offsetHeight;			
						}
					}
				");			
			return sb.ToString();
		}

		/// <summary>
		/// ���� JavaScript ���� MoveTitle ���ַ���
		/// </summary>
		private string AddJsfMoveTitle()
		{
			/*
				// �ѱ�ͷ���Ƶ�����ֱ�ƶ��ı���
				function moveTitle(nMovedRowCount)
				{
					// ��������Ч��		
					if(nMovedRowCount < 1)
						return;
					
					var tableMain = divMain.getElementsByTagName("table");
					tableMain = tableMain[0];		
					if(tableMain.rows.length < nMovedRowCount)
						return;		
							
					// ͬ���ֽ����е��п��		
					var totalColCount = 0;
					for(var i=0;i<tableMain.rows[0].cells.length;i++)
					{
						totalColCount += tableMain.rows[0].cells[i].colSpan;			
					}
					
					if(tableMain.rows[nMovedRowCount].cells.length == totalColCount)	
					{		
						var aCellMapping = mAISRSell(tableMain,nMovedRowCount,totalColCount);
						for(var j=0;j<totalColCount;j++)
						{
							var cellMapping = aCellMapping[(nMovedRowCount - 1) * totalColCount + j];
							var maxWidth = Math.max(tableMain.rows[cellMapping.rowIndex].cells[cellMapping.colIndex].offsetWidth, 
								tableMain.rows[nMovedRowCount].cells[j].offsetWidth);
							tableMain.rows[cellMapping.rowIndex].cells[cellMapping.colIndex].width = "";
							tableMain.rows[nMovedRowCount].cells[j].width = "";
							tableMain.rows[cellMapping.rowIndex].cells[cellMapping.colIndex].style.width = maxWidth;
							tableMain.rows[nMovedRowCount].cells[j].style.width = maxWidth;
						}
					}
							
					var tableTitle = tableMain.cloneNode();
					tableTitle.id += "Title";
					divTitle.appendChild(tableTitle);
					var tBody = document.createElement('TBODY');				
					tableTitle.appendChild(tBody);
							
					// �ƶ�Main�ı�ͷ��
					for(var i=0;i<nMovedRowCount;i++)
					{	
						var moveRow = tableMain.rows[0].cloneNode(true);
						tBody.appendChild(moveRow);
						
						while(tableMain.rows[0].cells.length > 0)
						{
							tableMain.rows[0].cells[0].removeNode();
						}
						tableMain.rows[0].removeNode();
					}			
				}
			*/
			StringBuilder sb = new StringBuilder();
			sb.Append("function " + this.ClientID + "_moveTitle(nMovedRowCount)\r\n");
			sb.Append("{\r\n");	
			sb.Append(@"
					// ��������Ч��		
					if(nMovedRowCount < 1)
						return;
					");
			sb.Append(" var divMain = document.getElementById('" + divMain.ClientID + "');");			
			sb.Append(@"
					var tableMain = divMain.getElementsByTagName('table');
					tableMain = tableMain[0];		
					if(tableMain.rows.length < nMovedRowCount)
						return;		
							
					// ͬ���ֽ����е��п��		
					var totalColCount = 0;
					for(var i=0;i<tableMain.rows[0].cells.length;i++)
					{
						totalColCount += tableMain.rows[0].cells[i].colSpan;			
					}
					
					if(tableMain.rows[nMovedRowCount].cells.length == totalColCount)	
					{		
				");
			sb.Append("var aCellMapping = " + this.ClientID + "_mAISRSell(tableMain,nMovedRowCount,totalColCount);\r\n");;
			sb.Append(@"
					for(var j=0;j<totalColCount;j++)
						{
							var cellMapping = aCellMapping[(nMovedRowCount - 1) * totalColCount + j];
							var maxWidth = Math.max(tableMain.rows[cellMapping.rowIndex].cells[cellMapping.colIndex].offsetWidth, 
								tableMain.rows[nMovedRowCount].cells[j].offsetWidth);
							tableMain.rows[cellMapping.rowIndex].cells[cellMapping.colIndex].width = '';
							tableMain.rows[nMovedRowCount].cells[j].width = '';
							tableMain.rows[cellMapping.rowIndex].cells[cellMapping.colIndex].style.width = maxWidth;
							tableMain.rows[nMovedRowCount].cells[j].style.width = maxWidth;
						}
					}
							
					var tableTitle = tableMain.cloneNode();
					tableTitle.id += 'Title';
				");
			sb.Append("var divTitle = document.getElementById('" + divTitle.ClientID + "');\r\n");
			sb.Append(@"
					divTitle.appendChild(tableTitle);
					var tBody = document.createElement('TBODY');				
					tableTitle.appendChild(tBody);
							
					// �ƶ�Main�ı�ͷ��
					for(var i=0;i<nMovedRowCount;i++)
					{	
						var moveRow = tableMain.rows[0].cloneNode(true);
						tBody.appendChild(moveRow);
						
						while(tableMain.rows[0].cells.length > 0)
						{
							tableMain.rows[0].cells[0].removeNode();
						}
						tableMain.rows[0].removeNode();
					}			
					
				}");
			return sb.ToString();
		}

		/// <summary>
		/// ���� JavaScript ���� MoveTitleFreeze ���ַ���
		/// </summary>
		private string AddJsfMoveTitleFreeze()
		{
			/*
				// ��Title�е��������Ƶ���ˮƽ�ƶ��ı���
				function moveTitleFreeze(nMovedColCount)
				{
					// �����Ч��
					if(nMovedColCount < 1)
						return;
					
					var tableTitle = divTitle.getElementsByTagName('TABLE');
					tableTitle = tableTitle[0];
					
					if(tableTitle.rows < 1)
						return;
									
					// ����һ��Table,��ӵ�	divTitleFreeze ��
					var tableTitleFreeze = tableTitle.cloneNode(true);
					tableTitleFreeze.id += 'Freeze';				
					divTitleFreeze.appendChild(tableTitleFreeze);	
							
					// ɾ�������ߵ���
					deleteAfterFirstNColumn(tableTitleFreeze,nMovedColCount);
					deleteFirstNColumn(tableTitle,nMovedColCount);		
							
				}
			*/
			StringBuilder sb = new StringBuilder();
			sb.Append("function " + this.ClientID + "_moveTitleFreeze(nMovedColCount)\r\n");
			sb.Append("{\r\n");	
			sb.Append(@"
					// �����Ч��
					if(nMovedColCount < 1)
						return;
					");
			sb.Append(" var divTitle = document.getElementById('" + divTitle.ClientID + "');");
			sb.Append(@"
					var tableTitle = divTitle.getElementsByTagName('TABLE');
					tableTitle = tableTitle[0];
					
					if(tableTitle.rows < 1)
						return;
									
					// ����һ��Table,��ӵ�	divTitleFreeze ��
					var tableTitleFreeze = tableTitle.cloneNode(true);
					tableTitleFreeze.id += 'Freeze';
				");
			sb.Append(" var divTitleFreeze = document.getElementById('" + divTitleFreeze.ClientID + "');");
			sb.Append(@"	
					divTitleFreeze.appendChild(tableTitleFreeze);	
							
					// ɾ�������ߵ���
				");
			sb.Append(this.ClientID + "_deleteAfterFirstNColumn(tableTitleFreeze,nMovedColCount);\r\n");
			sb.Append(this.ClientID + "_deleteFirstNColumn(tableTitle,nMovedColCount);\r\n");		
			sb.Append("}\r\n");
			return sb.ToString();
		}

		/// <summary>
		/// ���� JavaScript ���� MoveMainFreeze ���ַ���
		/// </summary>
		private string AddJsfMoveMainFreeze()
		{
			/*
				// �������е��������Ƶ���ˮƽ�ƶ��ı���
				function moveMainFreeze(nMovedColCount)
				{
					// �����Ч��
					if(nMovedColCount < 1)
						return;
						
					var tableMain = divMain.getElementsByTagName('table');
					tableMain = tableMain[0];
					if(tableMain.rows < 1)
						return;
					
					// ����һ��Table,��ӵ�	divMainFreeze ��
					var tableMainFreeze = tableMain.cloneNode(true);
					tableMainFreeze.id += 'Freeze';		
					divMainFreeze.appendChild(tableMainFreeze);
					
								
					// ɾ�������ߵ���
					deleteAfterFirstNColumn(tableMainFreeze,nMovedColCount);
					deleteFirstNColumn(tableMain,nMovedColCount);		
				}
			*/
			StringBuilder sb = new StringBuilder();
			sb.Append("function " + this.ClientID + "_moveMainFreeze(nMovedColCount)\r\n");
			sb.Append("{\r\n");	
			sb.Append(@"
					// �����Ч��
					if(nMovedColCount < 1)
						return;				
				");
			sb.Append(" var divMain = document.getElementById('" + divMain.ClientID + "');");
			sb.Append(@"
					var tableMain = divMain.getElementsByTagName('table');
					tableMain = tableMain[0];
					if(tableMain.rows < 1)
						return;
					
					// ����һ��Table,��ӵ�	divMainFreeze ��
					var tableMainFreeze = tableMain.cloneNode(true);
					tableMainFreeze.id += 'Freeze';	
				");
			sb.Append(" var divMainFreeze = document.getElementById('" + divMainFreeze.ClientID + "');");
			sb.Append(@"
					divMainFreeze.appendChild(tableMainFreeze);
					
								
					// ɾ�������ߵ���
				");
			sb.Append(this.ClientID + "_deleteAfterFirstNColumn(tableMainFreeze,nMovedColCount);\r\n");
			sb.Append(this.ClientID + "_deleteFirstNColumn(tableMain,nMovedColCount);\r\n");		
			sb.Append("}\r\n");
			return sb.ToString();
		}

		
		/// <summary>
		/// ���� JavaScript ���� AdjustDivPosition ���ַ���
		/// </summary>
		private string AddJsfAdjustDivPosition()
		{
			/*
				// ������div��λ��
				function adjustDivPosition(nLockedRowCount,nLockedColCount)
				{
					var SCROLL_BAR_WIDTH = 16;
					var SCROLL_BAR_HEIGHT = 16;		
					
					var totalWidth = parseInt(tableFrame.width);
					var totalHeight = parseInt(tableFrame.height);
						
					var divTitleFreeze = document.getElementById("divTitleFreeze");
					var divTitle = document.getElementById("divTitle");
					var divMainFreeze = document.getElementById("divMainFreeze");
					var divMain = document.getElementById("divMain");
					
					var tableTitleFreeze = divTitleFreeze.getElementsByTagName("TABLE");
					tableTitleFreeze = tableTitleFreeze[0];
					var tableTitle = divTitle.getElementsByTagName("TABLE");
					tableTitle = tableTitle[0];		
					var tableMainFreeze = divMainFreeze.getElementsByTagName("TABLE");
					tableMainFreeze = tableMainFreeze[0];
					var tableMain = divMain.getElementsByTagName("TABLE");
					tableMain = tableMain[0];
									
					// ���������п��		
					var borderWidth = 0;
					var cellPadding = 0;
					
					if(tableMain.border != "")
						borderWidth = parseInt(tableMain.border);
					if(tableMain.style.borderWidth != "")
						borderWidth = parseInt(tableMain.style.borderWidth);
						
					if(tableMain.cellPadding != "")
						cellPadding = parseInt(tableMain.cellPadding);
					if(tableMain.style.cellPadding != "")	
						cellPadding = parseInt(tableMain.cellPadding);
					
					var lockedWidth = tableTitleFreeze.offsetWidth - borderWidth;
						
					// ���������и߶�				
					var lockedHeight = tableTitleFreeze.offsetHeight - borderWidth;
					
					if(lockedWidth + SCROLL_BAR_WIDTH > totalWidth)
						return;
					if(lockedHeight + SCROLL_BAR_HEIGHT > totalHeight )
						return;
					
					// ����div�ĸ߶ȺͿ��		
					divTitleFreeze.style.width = lockedWidth;
					divTitleFreeze.style.height = lockedHeight;
					divTitle.style.height = lockedHeight + SCROLL_BAR_HEIGHT;
					divMainFreeze.style.width = lockedWidth + SCROLL_BAR_WIDTH;
					
					if(tableMain.offsetWidth <= totalWidth - lockedWidth)
					{
						if(tableMain.offsetHeight <= totalHeight -  lockedHeight)
						{
							// ��ȹ����߶ȹ�
							divTitle.style.width = tableMain.offsetWidth;
							divMain.style.width = tableMain.offsetWidth;
							divMainFreeze.style.height = tableMain.offsetHeight;		
							divMain.style.height = tableMain.offsetHeight + 2;
						}
						else
						{
							// ��ȹ����߶Ȳ���
							if(tableMain.offsetWidth + SCROLL_BAR_WIDTH <= totalWidth - lockedWidth )
							{
								// ֻ����ֱ������
								divTitle.style.width = tableMain.offsetWidth + + SCROLL_BAR_WIDTH;
								divMain.style.width = tableMain.offsetWidth + SCROLL_BAR_WIDTH;
								divMainFreeze.style.height = totalHeight - lockedHeight;		
								divMain.style.height = totalHeight - lockedHeight;
							}
							else
							{
								// ������������
								divTitle.style.width = totalWidth - lockedWidth - SCROLL_BAR_WIDTH;
								divMain.style.width = totalWidth  - lockedWidth;
								divMainFreeze.style.height = totalHeight - lockedHeight - SCROLL_BAR_HEIGHT;		
								divMain.style.height = totalHeight - lockedHeight;
							}
						}
						
					}
					else
					{
						if(tableMain.offsetHeight <= totalHeight - lockedHeight)
						{
							// ��Ȳ������߶ȹ�
							if(tableMain.offsetHeight + SCROLL_BAR_HEIGHT <= totalHeight - lockedHeight )
							{
								// ֻ��ˮƽ������
								divTitle.style.width = totalWidth - lockedWidth;
								divMain.style.width = totalWidth  - lockedWidth;
								divMainFreeze.style.height = tableMain.offsetHeight - SCROLL_BAR_HEIGHT;		
								divMain.style.height = tableMain.offsetHeight + 2;
							}
							else
							{
								// ������������
								divTitle.style.width = totalWidth - lockedWidth - SCROLL_BAR_WIDTH;
								divMain.style.width = totalWidth  - lockedWidth;
								divMainFreeze.style.height = totalHeight - lockedHeight - SCROLL_BAR_HEIGHT;		
								divMain.style.height = totalHeight - lockedHeight;
							}
						}
						else
						{
							// ��Ȳ������߶Ȳ�����������������
							divTitle.style.width = totalWidth - lockedWidth - SCROLL_BAR_WIDTH;
							divMain.style.width = totalWidth  - lockedWidth;
							divMainFreeze.style.height = totalHeight - lockedHeight - SCROLL_BAR_HEIGHT;		
							divMain.style.height = totalHeight - lockedHeight;
						}						
						
					}						
					
					// ���ø���λ��		
					divTitle.style.left = divTitleFreeze.style.left + lockedWidth;
					divTitle.style.top = lockedHeight * -1;
					divTitle.style.zIndex = divTitleFreeze.style.zIndex + 1;	
					
					divMainFreeze.style.top = (divTitle.offsetHeight) * -1;
					divMainFreeze.style.zIndex = divTitleFreeze.style.zIndex + 2;		
					
					divMain.style.top = (divTitle.offsetHeight + divMainFreeze.offsetHeight) * -1;
					divMain.style.left = divTitle.style.left;
					divMain.style.zIndex = divTitleFreeze.style.zIndex + 3;	
					
					// ���ø������ʾλ��
					tableTitle.style.marginLeft = (borderWidth + cellPadding) * -1;		
					tableMainFreeze.style.marginTop = (borderWidth + cellPadding) * -1;
					
					tableMain.style.marginLeft = (borderWidth + cellPadding) * -1;	
					tableMain.style.marginTop = (borderWidth + cellPadding) * -1;
							
				}
			*/
			StringBuilder sb = new StringBuilder();
			sb.Append("function " + this.ClientID + "_adjustDivPosition(nLockedRowCount,nLockedColCount)\r\n");
			sb.Append("{\r\n");	
			sb.Append(@"
					var SCROLL_BAR_WIDTH = 16;
					var SCROLL_BAR_HEIGHT = 16;		
				");
		
			
			sb.Append("	var totalWidth = parseInt(" + this.tableFrame.ClientID + ".width);\r\n");
			
			sb.Append("	var totalHeight = parseInt(" + this.tableFrame.ClientID + ".height);\r\n");

			sb.Append("var divTitleFreeze = " + this.divTitleFreeze.ClientID + ";\r\n");
			
			sb.Append("var divTitle = " + this.divTitle.ClientID + ";\r\n");
			
			sb.Append("var divMainFreeze = " + this.divMainFreeze.ClientID + ";\r\n");
			
			sb.Append("var divMain = " + this.divMain.ClientID + ";\r\n");
			
			sb.Append("var tableTitleFreeze = divTitleFreeze.getElementsByTagName(\"TABLE\");\r\n");
			
			sb.Append("tableTitleFreeze = tableTitleFreeze[0];\r\n");
			
			sb.Append("var tableTitle = divTitle.getElementsByTagName(\"TABLE\");\r\n");
			
			sb.Append("tableTitle = tableTitle[0];\r\n");
			
			sb.Append("var tableMainFreeze = divMainFreeze.getElementsByTagName(\"TABLE\");\r\n");
			
			sb.Append("tableMainFreeze = tableMainFreeze[0];\r\n");
			

			sb.Append("var tableMain = divMain.getElementsByTagName(\"TABLE\");\r\n");
			
			sb.Append("tableMain = tableMain[0];\r\n");

			sb.Append(@"				
											
					var borderWidth = 0;
					var cellPadding = 0;
				");
			sb.Append("if(tableMain.border != \"\")\r\n");
			sb.Append("borderWidth = parseInt(tableMain.border);\r\n");
			sb.Append("if(tableMain.style.borderWidth != \"\")\r\n");
			sb.Append("borderWidth = parseInt(tableMain.style.borderWidth);\r\n");
						
			sb.Append("if(tableMain.cellPadding != \"\")\r\n");
			sb.Append("cellPadding = parseInt(tableMain.cellPadding);\r\n");
			sb.Append("if(tableMain.style.padding != \"\")\r\n");	
			sb.Append(@"
					cellPadding = parseInt(tableMain.style.padding);

					// ���������п��	
					var lockedWidth = tableTitleFreeze.offsetWidth - borderWidth;
						
					// ���������и߶�				
					var lockedHeight = tableTitleFreeze.offsetHeight - borderWidth;
					
					if(lockedWidth + SCROLL_BAR_WIDTH > totalWidth)
						return;
					if(lockedHeight + SCROLL_BAR_HEIGHT > totalHeight )
						return;
					
					// ����div�ĸ߶ȺͿ��		
					divTitleFreeze.style.width = lockedWidth;
					divTitleFreeze.style.height = lockedHeight;
					divTitle.style.height = lockedHeight + SCROLL_BAR_HEIGHT;
					divMainFreeze.style.width = lockedWidth + SCROLL_BAR_WIDTH;
					
					if(tableMain.offsetWidth <= totalWidth - lockedWidth)
					{
						if(tableMain.offsetHeight <= totalHeight -  lockedHeight)
						{
							// ��ȹ����߶ȹ�
							divTitle.style.width = tableMain.offsetWidth;
							divMain.style.width = tableMain.offsetWidth;
							divMainFreeze.style.height = tableMain.offsetHeight;		
							divMain.style.height = tableMain.offsetHeight;
						}
						else
						{
							// ��ȹ����߶Ȳ���
							if(tableMain.offsetWidth + SCROLL_BAR_WIDTH <= totalWidth - lockedWidth )
							{
								// ֻ����ֱ������
								divTitle.style.width = tableMain.offsetWidth;
								divMain.style.width = tableMain.offsetWidth + SCROLL_BAR_WIDTH;
								divMainFreeze.style.height = totalHeight - lockedHeight;		
								divMain.style.height = totalHeight - lockedHeight;
							}
							else
							{
								// ������������
								divTitle.style.width = totalWidth - lockedWidth - SCROLL_BAR_WIDTH;
								divMain.style.width = totalWidth  - lockedWidth;
								divMainFreeze.style.height = totalHeight - lockedHeight - SCROLL_BAR_HEIGHT;		
								divMain.style.height = totalHeight - lockedHeight;
							}
						}
						
					}
					else
					{
						if(tableMain.offsetHeight <= totalHeight - lockedHeight)
						{
							// ��Ȳ������߶ȹ�
							if(tableMain.offsetHeight + SCROLL_BAR_HEIGHT <= totalHeight - lockedHeight )
							{
								// ֻ��ˮƽ������
								divTitle.style.width = totalWidth - lockedWidth;
								divMain.style.width = totalWidth  - lockedWidth;
								divMainFreeze.style.height = tableMain.offsetHeight;		
								divMain.style.height = tableMain.offsetHeight + SCROLL_BAR_HEIGHT;
							}
							else
							{
								// ������������
								divTitle.style.width = totalWidth - lockedWidth - SCROLL_BAR_WIDTH;
								divMain.style.width = totalWidth  - lockedWidth;
								divMainFreeze.style.height = totalHeight - lockedHeight - SCROLL_BAR_HEIGHT;		
								divMain.style.height = totalHeight - lockedHeight;
							}
						}
						else
						{
							// ��Ȳ������߶Ȳ�����������������
							divTitle.style.width = totalWidth - lockedWidth - SCROLL_BAR_WIDTH;
							divMain.style.width = totalWidth  - lockedWidth;
							divMainFreeze.style.height = totalHeight - lockedHeight - SCROLL_BAR_HEIGHT;		
							divMain.style.height = totalHeight - lockedHeight;
						}						
						
					}						
					
					// ���ø���λ��		
					divTitle.style.left = divTitleFreeze.style.left + lockedWidth;
					divTitle.style.top = lockedHeight * -1;
					divTitle.style.zIndex = divTitleFreeze.style.zIndex + 1;	
					
					divMainFreeze.style.top = (divTitle.offsetHeight) * -1;
					divMainFreeze.style.zIndex = divTitleFreeze.style.zIndex + 2;		
					
					divMain.style.top = (divTitle.offsetHeight + divMainFreeze.offsetHeight) * -1;
					divMain.style.left = divTitle.style.left;
					divMain.style.zIndex = divTitleFreeze.style.zIndex + 3;	
					
					// ���ø������ʾλ��
					var tableMainCellBorderWidth = 0;
				");
			sb.Append("if(tableMain.rows[0].cells[0].style.borderWidth != \"\")\r\n");
			sb.Append(@"
						tableMainCellBorderWidth = parseInt(tableMain.rows[0].cells[0].style.borderWidth);
					
					tableTitle.style.marginLeft = (borderWidth + cellPadding + tableMainCellBorderWidth) * -1;		
					tableMainFreeze.style.marginTop = (borderWidth + cellPadding + tableMainCellBorderWidth) * -1;
					
					tableMain.style.marginLeft = (borderWidth + cellPadding + tableMainCellBorderWidth) * -1;	
					tableMain.style.marginTop = (borderWidth + cellPadding + tableMainCellBorderWidth) * -1;

					// ������Ԫ�ص�λ�úͳߴ磬�Ա����ʱ��ʾ�߽�					
					/*
					divTitleFreeze.style.width = parseInt(divTitleFreeze.style.width) + 1;
					divTitleFreeze.style.height = parseInt(divTitleFreeze.style.height) + 1;
					divTitle.style.height = parseInt(divTitle.style.height) + 1;
					divMainFreeze.style.width = parseInt(divMainFreeze.style.width) + 1;
					
					divTitle.style.top = parseInt(divTitle.style.top) - 1;
					divMainFreeze.style.top = parseInt(divMainFreeze.style.top) - 1;
					divMain.style.top = parseInt(divMain.style.top) - 1;
					*/
					divTitle.style.left = parseInt(divTitle.style.left);
					divMain.style.left = parseInt(divMain.style.left);					

					
					tableTitle.style.marginLeft = parseInt(tableTitle.style.marginLeft); 
					tableMainFreeze.style.marginTop = parseInt(tableMainFreeze.style.marginTop);
					tableMain.style.marginLeft = parseInt(tableMain.style.marginLeft);
					tableMain.style.marginTop = parseInt(tableMain.style.marginTop);
				");
			sb.Append("}\r\n");
			return sb.ToString();
		}

		/// <summary>
		/// ���� JavaScript ���� LockTable ���ַ���
		/// </summary>
		private string AddJsfLockTable()
		{
			
			/*			  
				// �������	nLockedRowCount �У�nLockedColCount��
				function lockTable(nLockedRowCount,nLockedColCount)
				{								
					var divMain = document.getElementById("divMain");
					var tableMain = divMain.getElementsByTagName("TABLE");
					tableMain = tableMain[0];
					
					divideLockedCell(tableMain,nLockedRowCount,nLockedColCount);
					
					
					recordCellSize(tableMain,nLockedRowCount);
					tableMain.style.tableLayout = "fixed";
						
					moveTitle(nLockedRowCount);
					moveTitleFreeze(nLockedColCount);
					moveMainFreeze(nLockedColCount);
					moveMain();		
							
					adjustDivPosition(nLockedRowCount,nLockedColCount);		
					divMainFreeze.attachEvent("onscroll",verticalScrollMain);
					divMain.attachEvent("onscroll",scrollAll);
					
				}
			*/

			StringBuilder sb = new StringBuilder();

			sb.Append("function " + this.ClientID + "_lockTable(nLockedRowCount,nLockedColCount)\r\n");
			sb.Append("{\r\n");
			sb.Append("var divMain = document.getElementById('" + divMain.ClientID + "');\r\n");
			sb.Append("var tableMain = divMain.getElementsByTagName('TABLE');\r\n");
			sb.Append("tableMain = tableMain[0];\r\n");
			sb.Append(this.ClientID + "_divideLockedCell(tableMain,nLockedRowCount,nLockedColCount);\r\n");					
			sb.Append(this.ClientID + "_recordCellSize(tableMain,nLockedRowCount);\r\n");
			sb.Append("tableMain.style.tableLayout = 'fixed';\r\n");					
			sb.Append(this.ClientID + "_moveTitle(nLockedRowCount);\r\n");
			sb.Append(this.ClientID + "_moveTitleFreeze(nLockedColCount);\r\n");
			sb.Append(this.ClientID + "_moveMainFreeze(nLockedColCount);\r\n");				
			sb.Append(this.ClientID + "_adjustDivPosition(nLockedRowCount,nLockedColCount);\r\n");
			
			sb.Append(this.divMainFreeze.ClientID + ".attachEvent(\"onscroll\"," + this.ClientID + "_verticalScrollMain);\r\n");
			sb.Append(this.divMain.ClientID + ".attachEvent(\"onscroll\"," + this.ClientID + "_scrollAll);\r\n");
			sb.Append("}\r\n");				
			
			return sb.ToString();
		}

		/// <summary>
		/// ���� JavaScript ���� onDocumentReadyStateChange ���ַ���
		/// </summary>
		/// <returns></returns>
		private string AddJsfOnDocumentReadyStateChange()
		{
			/*
				function onDocumentReadyStateChange()
				{
					if(document.readyState == "complete")
					{						
						lockTable(3,3);
					}
				}  
			*/
			StringBuilder sb = new StringBuilder();
			sb.Append("function " + this.ClientID + "_onDocumentReadyStateChange()\r\n");
			sb.Append("{\r\n");			
			sb.Append("if(document.readyState == \"complete\")\r\n");
			sb.Append("{\r\n");
			sb.Append(this.ClientID + "_lockTable(" + this._titleRowCount.ToString() + ", " +  this._freezeColumnCount.ToString() + ");\r\n");
			sb.Append("}\r\n");
			sb.Append("}\r\n");  				
			return sb.ToString();
		}

		/// <summary>
		/// ���� JavaScript ���� onTableResize ���ַ���
		/// </summary>
		/// <returns></returns>
		private string AddJsfOnTableResize()
		{
			/*
				function onTableResize()
				{
					adjustDivPosition(nLockedRowCount,nLockedColCount);
				}  
			*/
			StringBuilder sb = new StringBuilder();
			sb.Append("function " + this.ClientID + "_onTableResize()\r\n");
			sb.Append("{\r\n");			
			sb.Append("var nLockedRowCount = " + this._titleRowCount.ToString() + ";\r\n");
			sb.Append("var nLockedColCount = " + this._freezeColumnCount.ToString() + ";\r\n");
			sb.Append(this.ClientID + "_adjustDivPosition(nLockedRowCount, nLockedColCount);\r\n");
			sb.Append("}\r\n");  				
			return sb.ToString();
		}

		/// <summary>
		/// ���� JavaScript ���� onPropertyChange ���ַ���
		/// </summary>
		/// <returns></returns>
		private string AddJsfOnPropertyChange()
		{
			/*
				function onPropertyChange(row)
				{
					var anotherRow = getAnotherTr(row);
					if(anotherRow != null)
						anotherRow.style.height = row.style.height;
				}  
			*/
			StringBuilder sb = new StringBuilder();
			sb.Append("function " + this.ClientID + "_onPropertyChange(row)\r\n");
			sb.Append("{\r\n");			
			sb.Append("var anotherRow = " + this.ClientID + "_getAnotherTr(row);\r\n");
			sb.Append(@"
					if(anotherRow != null)
						anotherRow.style.height = row.style.height;
				");
			sb.Append("}\r\n");  				
			return sb.ToString();
		}		
	}

	/// <summary>
	/// �������ݰ��������������
	/// </summary>
	public class SortEventArgs : System.EventArgs
	{
		private string _fieldName;
		private bool   _isSortDesc;

		public SortEventArgs(string fieldName, bool isSortDesc)
		{
			_fieldName = fieldName;
			_isSortDesc = isSortDesc;
		}

		public string FieldName
		{
			get { return _fieldName; }
		}

		public bool IsSortDesc
		{
			get { return _isSortDesc;}
		}
	}
}
