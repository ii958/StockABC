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
	///		当页面上的Table显示高度和宽度大于给定的范围时，
	///		Table可以冻结设置好的前几行和前几列列，其他部分自动出现滚动条，
	///		内容随滚动条滚动。
	/// </summary>
	public class BmBlafTable : System.Web.UI.UserControl
	{
		
		// bmTable
		protected System.Web.UI.WebControls.LinkButton linkButtonSort; //排序
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

		#region 私有变量
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
		/// 设置或获取表格显示的宽度
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
		/// 设置或获取表格显示的高度
		/// </summary>
		public string Height
		{
			get{return _tableFrameHeight;}
			set{_tableFrameHeight = value;}
		}

		/// <summary>
		/// 设置或获取左边冻结列的个数。取值范围为1到合并单元格前表的列数。
		/// 如果有冻结列和其后列单元格合并，控件自动拆分这些单元格。
		/// </summary>
		public int FreezeColumnCount
		{
			get{return _freezeColumnCount;}
			set{_freezeColumnCount = value;}
		}
	

		/// <summary>
		/// 表格标题有几行。必须和表的标题行数一致，即冻结行和其下各行的单元各不允许有
		/// 合并的情况，否则表格不能正常显示。
		/// </summary>
		public int TitleRowCount
		{
			get{return _titleRowCount;}
			set{_titleRowCount = value;}
		}

		#endregion
		
		#region 事件
		public delegate void HeadCellClickedHandler(Object sender,SortEventArgs e);
		public event HeadCellClickedHandler HeadCellClicked = null;
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			// 在此处放置用户代码以初始化页面			
			labelJs.Text = "";

			table.BorderWidth = this._borderWidth;
			table.BorderColor = this._borderColor;
			table.BorderStyle = this._borderStyle;
			table.CellSpacing = 0;
			
			SetScript();		
			
		}

		
		/// <summary>
		/// 在labelScript中，填写一组JavaScript语句。这组语句中包扩一个JavaScript函数，把被鼠标单击的TR元素用特殊颜色高亮显示。
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

			#region 初始化排序字段
			sb.Append("function " + this.ClientID + "_onHeadClick(columnName)\r\n");
			sb.Append("{\r\n");
			//sb.Append("document.all(\"" + hiddenSortColumnName.ClientID + "\").value = columnName;\r\n");
            sb.Append("document.getElementById(\"" + hiddenSortColumnName.ClientID + "\").value = columnName;\r\n");
			sb.Append("var sortHref = document.getElementById(\"" + linkButtonSort.ClientID + "\");\r\n");
			sb.Append("eval(sortHref.href);\r\n");
			sb.Append("}\r\n");
			#endregion

			#region 找同位列的函数
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
            //20120420 chencheng 项目升级到4.0后更改获取对象的方式
            //string tmp = "document.getElementById(\""+ hiddenHighlightRowID.ClientID + "\").value";
            //sb.Append(this.ClientID + "_highlightRow(document.getElementById(\"<%=" + tmp + ".ClientID%>\"))\r\n");
            //20120730 chencheng 注释该行
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
		/// 由给定的颜色返回16进制颜色字符串 如：#FFFFFF.
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		private string GetHexColorString(Color color)
		{
			return System.String.Format("#{0}{1}{2}",color.R.ToString("X2"),color.G.ToString("X2"),color.B.ToString("X2"));			
		}

		/// <summary>
		/// 移除所有行
		/// </summary>
		public void Clear()
		{
			table.Rows.Clear();
		}

		/// <summary>
		/// 在表中增加一个表头行，并返回该表头行
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
		/// 添加可排序的表头单元格，并返回该单元格
		/// </summary>
		/// <param name="headRow">表头行对象的引用</param>
		/// <param name="text">表头要显示的文本</param>
		/// <param name="columnName">列名，和DataTable的ColumnName对应</param>
		/// <param name="cellWidth">表中列宽</param>		
		/// <returns>返回添加的表头单元格</returns>
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

			
				// 加排序链接
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

				// 加上下箭头
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
		/// 添加表头单元格，并返回该单元格
		/// </summary>
		/// <param name="headRow">表头行对象的引用</param>
		/// <param name="text">表头要显示的文本</param>		
		/// <param name="cellWidth">表中列宽</param>		
		/// <returns>返回表头单元格</returns>
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
		/// 添加表头单元格，并返回该单元格
		/// </summary>
		/// <param name="headRow">表头行对象的引用</param>
		/// <param name="text">表头要显示的文本</param>		
		/// <returns>返回表头单元格</returns>
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
		/// 添加一个 Body 行，并返回该行
		/// </summary>
		/// <param name="rowID">行标识</param>
		/// <returns></returns>
		public TableRow AddBodyRow(string rowID)
		{
			TableRow bodyRow = AddBodyRow();			
			bodyRow.ID = rowID;			
			return bodyRow;
		}

		/// <summary>
		/// 添加一个 Body 行，并返回该行
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
		/// 添加一个单元格，并返回该单元格
		/// </summary>
		/// <param name="bodyRow">要添加单元格的行</param>
		/// <param name="text">添加的内容</param>
		/// <param name="horizontalAlign">水平位置</param>
		/// <returns></returns>
		public TableCell AddCell(TableRow tableRow,string text,HorizontalAlign horizontalAlign)
		{
			TableCell tableCell = AddCell(tableRow,text);			
			tableCell.HorizontalAlign = horizontalAlign;			
			return tableCell;			
		}
		
		/// <summary>
		/// 添加一个单元格，并返回该单元格
		/// </summary>
		/// <param name="bodyRow">要添加单元格的行</param>
		/// <param name="text">添加的内容</param>
		/// <param name="horizontalAlign">水平位置</param>
		/// <returns></returns>
		public TableCell AddCell(TableRow tableRow,string text,HorizontalAlign horizontalAlign,int cellWidth)
		{
			TableCell tableCell = AddCell(tableRow,text);			
			tableCell.HorizontalAlign = horizontalAlign;
			tableCell.Width = Unit.Pixel(cellWidth);
			return tableCell;			
		}

		/// <summary>
		/// 添加一个单元格，并返回该单元格
		/// </summary>
		/// <param name="bodyRow">要添加单元格的行</param>
		/// <param name="text">添加的内容</param>		
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
		/// 添加一个单元格，并返回该单元格
		/// </summary>
		/// <param name="bodyRow">要添加单元格的行</param>
		/// <param name="text">添加的内容</param>
		/// <param name="horizontalAlign">水平位置</param>
		/// <param name="borderStyle">单元格边框样式</param>
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
		/// 添加一个表尾行
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
		/// 响应排序事件
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
			this.linkButtonSort.Click += new System.EventHandler(this.linkButtonSort_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		/// <summary>
		/// 设置参数
		/// </summary>		
		/// <param name="tableWidthToShow">表格显示的宽度</param>
		/// <param name="tableHeightToShow">表格显示的高度</param>
		/// <param name="freezeColumnCount">左边冻结列的个数</param>
		/// <param name="titleRowCount">表格标题有几行</param>
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
			// TODO:  添加 BmScrollTable.OnPreRender 实现
			base.OnPreRender (e);

			if(!this._isEnableScroll)
			{
				table.Attributes.Add("width",this._tableFrameWidth);
				return;
			}

			// 为页面添加JavaScript函数
			InitializeJsFunctions();
			

			// 检查属性的有效性
			if(this.table == null)
				return;
			
			if(this._titleRowCount < 0)
				return;

			if(this._freezeColumnCount < 0)
				return;

			if(this._titleRowCount > this.table.Rows.Count)
				return;

			// 设置控件属性			
			table.Style.Add("TABLE-LAYOUT","fixed");
			SetControlsAttributes();
						
			// 把Table发到页面上
			this.table.ID = "tableMain";
			this.table.EnableViewState = false;			
					
			// 设置表格显示宽度和高度
			tableFrame.Width = this._tableFrameWidth.ToString();
			tableFrame.Height = this._tableFrameHeight.ToString();
			tableFrameTd.Width = this._tableFrameWidth.ToString();
			tableFrameTd.Height = this._tableFrameHeight.ToString();

			// 向页面添加直接执行的JavaScript语句
			AddJsCommand();			
		}

		/// <summary>
		/// 把JavaScript函数，写到labelJsFunctions中
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
		/// 设置各控件的属性
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
		/// 把直接执行的JavaScript语句添加到labelJs中
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
		/// 返回 JavaScript 函数 ScrollAll 的字符串
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
		/// 返回 JavaScript 函数 VerticalScrollMain 的字符串
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
		/// 返回 JavaScript 函数 CellMappings 的字符串
		/// </summary>
		private string AddJsfCellMappings()
		{
			/*
				// 定义一个保存表中每个单元格是否合并以及和Dom对象Cell的对应关系
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
		/// 返回 JavaScript 函数 MAISRSell 的字符串
		/// </summary>
		private string AddJsfMAISRSell()
		{
			/*
				// 返回一个保存没合并单元格前，从Table左上角开始，nRowCount行，nColCount列范围内单元格和合并后单元格的对应关系的数组。
				// 数组的每个元素是一个cellMappings对象，保存合并前单元格对应的Dom对象Cell（可理解为合并后的单元格）的位置。
				function mAISRSell(mapedTable,nRowCount,nColCount)
				{					
					
					//用一维数组保存二维数组数据				
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
										
							// 保存合并的单元格和DOM的CELL对象的对应关系
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
					//用一维数组保存二维数组数据				
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
										
							// 保存合并的单元格和DOM的CELL对象的对应关系
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
		/// 返回 JavaScript 函数 DivideLockedCell 的字符串
		/// </summary>
		private string AddJsfDivideLockedCell()
		{
			/*
				// 从原始单元格来看，如果锁定的单元格和没锁定的单元格合并，把影响锁定的合并的单元格拆开 
				// 因为表头应该和内容截然分开，不考虑垂直合并影响锁定的情况
				function divideLockedCell(lockedTable,nLockedRowCount,nLockedColCount)
				{
					// 获取原始单元格和合并后单元格的对应关系
					var nTotalColCount = 0;
					for(var j=0; j<lockedTable.rows[0].cells.length;j++)
					{
						nTotalColCount += lockedTable.rows[0].cells[j].colSpan;
					}
					
					if (nLockedColCount >= nTotalColCount)
						return;
						
					var aCellMapping = mAISRSell(lockedTable,nLockedRowCount,nTotalColCount);
					
					// 表中每行的原始单元格的第nLockedRowCount格和第nLockedRowCount + 1个格对应的是同一个合并后的单元格，
					// 则把此合并后的单元格水平拆分成两个单元格，重新计算对应关系。
					// 拆分后的两个单元格的宽度，从其下各锁定行中获得。如果无法从锁定行中获得，尝试从第一个非锁定行获得
					for(var i=0; i<nLockedRowCount; i++)
					{
						var leftCellMapping = aCellMapping[i*nTotalColCount + nLockedColCount - 1];
						var rightCellMapping = aCellMapping[i*nTotalColCount + nLockedColCount];
						if(leftCellMapping.rowIndex == rightCellMapping.rowIndex && leftCellMapping.colIndex == rightCellMapping.colIndex )
						{
							// 水平拆分单元格
							var dividedCell = lockedTable.rows[leftCellMapping.rowIndex].cells[leftCellMapping.colIndex];
							// 计算拆分后单元格的宽度
							// 向下各行依次查找不用拆分的单元格，以确定拆分后两个单元格的宽度。
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
								//尝试从第一个非锁定行获得计算宽度所需参照单元格
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
								// 计算拆分后左右单元格的宽度
								var rCellMapping = aCellMapping[iRowIndex*nTotalColCount + nLockedColCount];
								var lWidth = lockedTable.rows[rCellMapping.rowIndex].cells[rCellMapping.colIndex].offsetLeft - 
									dividedCell.offsetLeft - lockedTable.cellSpacing;
								var rWidth = dividedCell.offsetWidth - lWidth - lockedTable.cellSpacing;
																		
								// 计算拆分后左右单元格的colSpan
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
								
								// 水平拆分单元格
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
													
								// 调整对应关系
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
					// 获取原始单元格和合并后单元格的对应关系
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
					// 表中每行的原始单元格的第nLockedRowCount格和第nLockedRowCount + 1个格对应的是同一个合并后的单元格，
					// 则把此合并后的单元格水平拆分成两个单元格，重新计算对应关系。
					// 拆分后的两个单元格的宽度，从其下各锁定行中获得。如果无法从锁定行中获得，尝试从第一个非锁定行获得
					for(var i=0; i<nLockedRowCount; i++)
					{
						var leftCellMapping = aCellMapping[i*nTotalColCount + nLockedColCount - 1];
						var rightCellMapping = aCellMapping[i*nTotalColCount + nLockedColCount];
						if(leftCellMapping.rowIndex == rightCellMapping.rowIndex && leftCellMapping.colIndex == rightCellMapping.colIndex )
						{
							// 水平拆分单元格
							var dividedCell = lockedTable.rows[leftCellMapping.rowIndex].cells[leftCellMapping.colIndex];
							// 计算拆分后单元格的宽度
							// 向下各行依次查找不用拆分的单元格，以确定拆分后两个单元格的宽度。
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
								//尝试从第一个非锁定行获得计算宽度所需参照单元格
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
								// 计算拆分后左右单元格的宽度
								var rCellMapping = aCellMapping[iRowIndex*nTotalColCount + nLockedColCount];
								var lWidth = lockedTable.rows[rCellMapping.rowIndex].cells[rCellMapping.colIndex].offsetLeft - 
									dividedCell.offsetLeft - lockedTable.cellSpacing;
								var rWidth = dividedCell.offsetWidth - lWidth - lockedTable.cellSpacing;
																		
								// 计算拆分后左右单元格的colSpan
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
								
								// 水平拆分单元格
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
													
								// 调整对应关系
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
		/// 返回 JavaScript 函数 DeleteFirstNColumn 的字符串
		/// </summary>
		/// <returns></returns>
		private string AddJsfDeleteFirstNColumn()
		{
			/*
				// 删除Table的前几列。	
				function deleteFirstNColumn(deletedTable,nCount)
				{
					var DELETED_TAG = "*Del_Cell*";
					
					// 先标记要删除的Cell
					var aCellMapping = mAISRSell(deletedTable,deletedTable.rows.length,nCount);
					for(var i=0;i<deletedTable.rows.length;i++)
					{
						for(var j=0;j<nCount;j++)		
						{
							var cellMapping = aCellMapping[i*nCount + j];
							deletedTable.rows[cellMapping.rowIndex].cells[cellMapping.colIndex].innerText = DELETED_TAG;
						}
					}
							
					// 删除标记过的Cell
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
					
					// 先标记要删除的Cell
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
									
					// 删除标记过的Cell
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
		/// 返回 JavaScript 函数 DeleteAfterFirstNColumn 的字符串
		/// </summary>
		/// <returns></returns>
		private string AddJsfDeleteAfterFirstNColumn()
		{
			/*
				// 删除Table的前几列以后的列。	
				function deleteAfterFirstNColumn(deletedTable,nCount)
				{
					if(deletedTable.rows.length <= 0)
						return;
						
					var DELETED_TAG = "*Del_Cell*";
					
					// 先标记要删除的Cell
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
					
					// 删除标记过的Cell		
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
							
						// 先标记要删除的Cell
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
								
						// 删除标记过的Cell		
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
		/// 返回 JavaScript 函数 RecordCellSize 的字符串
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
		/// 返回 JavaScript 函数 MoveTitle 的字符串
		/// </summary>
		private string AddJsfMoveTitle()
		{
			/*
				// 把表头行移到不垂直移动的表中
				function moveTitle(nMovedRowCount)
				{
					// 检查参数有效性		
					if(nMovedRowCount < 1)
						return;
					
					var tableMain = divMain.getElementsByTagName("table");
					tableMain = tableMain[0];		
					if(tableMain.rows.length < nMovedRowCount)
						return;		
							
					// 同步分界两行的列宽度		
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
							
					// 移动Main的表头行
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
					// 检查参数有效性		
					if(nMovedRowCount < 1)
						return;
					");
			sb.Append(" var divMain = document.getElementById('" + divMain.ClientID + "');");			
			sb.Append(@"
					var tableMain = divMain.getElementsByTagName('table');
					tableMain = tableMain[0];		
					if(tableMain.rows.length < nMovedRowCount)
						return;		
							
					// 同步分界两行的列宽度		
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
							
					// 移动Main的表头行
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
		/// 返回 JavaScript 函数 MoveTitleFreeze 的字符串
		/// </summary>
		private string AddJsfMoveTitleFreeze()
		{
			/*
				// 把Title行的锁定列移到不水平移动的表中
				function moveTitleFreeze(nMovedColCount)
				{
					// 检查有效性
					if(nMovedColCount < 1)
						return;
					
					var tableTitle = divTitle.getElementsByTagName('TABLE');
					tableTitle = tableTitle[0];
					
					if(tableTitle.rows < 1)
						return;
									
					// 复制一个Table,添加到	divTitleFreeze 中
					var tableTitleFreeze = tableTitle.cloneNode(true);
					tableTitleFreeze.id += 'Freeze';				
					divTitleFreeze.appendChild(tableTitleFreeze);	
							
					// 删除该移走的列
					deleteAfterFirstNColumn(tableTitleFreeze,nMovedColCount);
					deleteFirstNColumn(tableTitle,nMovedColCount);		
							
				}
			*/
			StringBuilder sb = new StringBuilder();
			sb.Append("function " + this.ClientID + "_moveTitleFreeze(nMovedColCount)\r\n");
			sb.Append("{\r\n");	
			sb.Append(@"
					// 检查有效性
					if(nMovedColCount < 1)
						return;
					");
			sb.Append(" var divTitle = document.getElementById('" + divTitle.ClientID + "');");
			sb.Append(@"
					var tableTitle = divTitle.getElementsByTagName('TABLE');
					tableTitle = tableTitle[0];
					
					if(tableTitle.rows < 1)
						return;
									
					// 复制一个Table,添加到	divTitleFreeze 中
					var tableTitleFreeze = tableTitle.cloneNode(true);
					tableTitleFreeze.id += 'Freeze';
				");
			sb.Append(" var divTitleFreeze = document.getElementById('" + divTitleFreeze.ClientID + "');");
			sb.Append(@"	
					divTitleFreeze.appendChild(tableTitleFreeze);	
							
					// 删除该移走的列
				");
			sb.Append(this.ClientID + "_deleteAfterFirstNColumn(tableTitleFreeze,nMovedColCount);\r\n");
			sb.Append(this.ClientID + "_deleteFirstNColumn(tableTitle,nMovedColCount);\r\n");		
			sb.Append("}\r\n");
			return sb.ToString();
		}

		/// <summary>
		/// 返回 JavaScript 函数 MoveMainFreeze 的字符串
		/// </summary>
		private string AddJsfMoveMainFreeze()
		{
			/*
				// 把数据行的锁定列移到不水平移动的表中
				function moveMainFreeze(nMovedColCount)
				{
					// 检查有效性
					if(nMovedColCount < 1)
						return;
						
					var tableMain = divMain.getElementsByTagName('table');
					tableMain = tableMain[0];
					if(tableMain.rows < 1)
						return;
					
					// 复制一个Table,添加到	divMainFreeze 中
					var tableMainFreeze = tableMain.cloneNode(true);
					tableMainFreeze.id += 'Freeze';		
					divMainFreeze.appendChild(tableMainFreeze);
					
								
					// 删除该移走的列
					deleteAfterFirstNColumn(tableMainFreeze,nMovedColCount);
					deleteFirstNColumn(tableMain,nMovedColCount);		
				}
			*/
			StringBuilder sb = new StringBuilder();
			sb.Append("function " + this.ClientID + "_moveMainFreeze(nMovedColCount)\r\n");
			sb.Append("{\r\n");	
			sb.Append(@"
					// 检查有效性
					if(nMovedColCount < 1)
						return;				
				");
			sb.Append(" var divMain = document.getElementById('" + divMain.ClientID + "');");
			sb.Append(@"
					var tableMain = divMain.getElementsByTagName('table');
					tableMain = tableMain[0];
					if(tableMain.rows < 1)
						return;
					
					// 复制一个Table,添加到	divMainFreeze 中
					var tableMainFreeze = tableMain.cloneNode(true);
					tableMainFreeze.id += 'Freeze';	
				");
			sb.Append(" var divMainFreeze = document.getElementById('" + divMainFreeze.ClientID + "');");
			sb.Append(@"
					divMainFreeze.appendChild(tableMainFreeze);
					
								
					// 删除该移走的列
				");
			sb.Append(this.ClientID + "_deleteAfterFirstNColumn(tableMainFreeze,nMovedColCount);\r\n");
			sb.Append(this.ClientID + "_deleteFirstNColumn(tableMain,nMovedColCount);\r\n");		
			sb.Append("}\r\n");
			return sb.ToString();
		}

		
		/// <summary>
		/// 返回 JavaScript 函数 AdjustDivPosition 的字符串
		/// </summary>
		private string AddJsfAdjustDivPosition()
		{
			/*
				// 调整各div的位置
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
									
					// 计算锁定列宽度		
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
						
					// 计算锁定行高度				
					var lockedHeight = tableTitleFreeze.offsetHeight - borderWidth;
					
					if(lockedWidth + SCROLL_BAR_WIDTH > totalWidth)
						return;
					if(lockedHeight + SCROLL_BAR_HEIGHT > totalHeight )
						return;
					
					// 设置div的高度和宽度		
					divTitleFreeze.style.width = lockedWidth;
					divTitleFreeze.style.height = lockedHeight;
					divTitle.style.height = lockedHeight + SCROLL_BAR_HEIGHT;
					divMainFreeze.style.width = lockedWidth + SCROLL_BAR_WIDTH;
					
					if(tableMain.offsetWidth <= totalWidth - lockedWidth)
					{
						if(tableMain.offsetHeight <= totalHeight -  lockedHeight)
						{
							// 宽度够，高度够
							divTitle.style.width = tableMain.offsetWidth;
							divMain.style.width = tableMain.offsetWidth;
							divMainFreeze.style.height = tableMain.offsetHeight;		
							divMain.style.height = tableMain.offsetHeight + 2;
						}
						else
						{
							// 宽度够，高度不够
							if(tableMain.offsetWidth + SCROLL_BAR_WIDTH <= totalWidth - lockedWidth )
							{
								// 只出垂直滚动条
								divTitle.style.width = tableMain.offsetWidth + + SCROLL_BAR_WIDTH;
								divMain.style.width = tableMain.offsetWidth + SCROLL_BAR_WIDTH;
								divMainFreeze.style.height = totalHeight - lockedHeight;		
								divMain.style.height = totalHeight - lockedHeight;
							}
							else
							{
								// 出两个滚动条
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
							// 宽度不够，高度够
							if(tableMain.offsetHeight + SCROLL_BAR_HEIGHT <= totalHeight - lockedHeight )
							{
								// 只出水平滚动条
								divTitle.style.width = totalWidth - lockedWidth;
								divMain.style.width = totalWidth  - lockedWidth;
								divMainFreeze.style.height = tableMain.offsetHeight - SCROLL_BAR_HEIGHT;		
								divMain.style.height = tableMain.offsetHeight + 2;
							}
							else
							{
								// 出两个滚动条
								divTitle.style.width = totalWidth - lockedWidth - SCROLL_BAR_WIDTH;
								divMain.style.width = totalWidth  - lockedWidth;
								divMainFreeze.style.height = totalHeight - lockedHeight - SCROLL_BAR_HEIGHT;		
								divMain.style.height = totalHeight - lockedHeight;
							}
						}
						else
						{
							// 宽度不够，高度不够，出两个滚动条
							divTitle.style.width = totalWidth - lockedWidth - SCROLL_BAR_WIDTH;
							divMain.style.width = totalWidth  - lockedWidth;
							divMainFreeze.style.height = totalHeight - lockedHeight - SCROLL_BAR_HEIGHT;		
							divMain.style.height = totalHeight - lockedHeight;
						}						
						
					}						
					
					// 设置各层位置		
					divTitle.style.left = divTitleFreeze.style.left + lockedWidth;
					divTitle.style.top = lockedHeight * -1;
					divTitle.style.zIndex = divTitleFreeze.style.zIndex + 1;	
					
					divMainFreeze.style.top = (divTitle.offsetHeight) * -1;
					divMainFreeze.style.zIndex = divTitleFreeze.style.zIndex + 2;		
					
					divMain.style.top = (divTitle.offsetHeight + divMainFreeze.offsetHeight) * -1;
					divMain.style.left = divTitle.style.left;
					divMain.style.zIndex = divTitleFreeze.style.zIndex + 3;	
					
					// 设置各表的显示位置
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

					// 计算锁定列宽度	
					var lockedWidth = tableTitleFreeze.offsetWidth - borderWidth;
						
					// 计算锁定行高度				
					var lockedHeight = tableTitleFreeze.offsetHeight - borderWidth;
					
					if(lockedWidth + SCROLL_BAR_WIDTH > totalWidth)
						return;
					if(lockedHeight + SCROLL_BAR_HEIGHT > totalHeight )
						return;
					
					// 设置div的高度和宽度		
					divTitleFreeze.style.width = lockedWidth;
					divTitleFreeze.style.height = lockedHeight;
					divTitle.style.height = lockedHeight + SCROLL_BAR_HEIGHT;
					divMainFreeze.style.width = lockedWidth + SCROLL_BAR_WIDTH;
					
					if(tableMain.offsetWidth <= totalWidth - lockedWidth)
					{
						if(tableMain.offsetHeight <= totalHeight -  lockedHeight)
						{
							// 宽度够，高度够
							divTitle.style.width = tableMain.offsetWidth;
							divMain.style.width = tableMain.offsetWidth;
							divMainFreeze.style.height = tableMain.offsetHeight;		
							divMain.style.height = tableMain.offsetHeight;
						}
						else
						{
							// 宽度够，高度不够
							if(tableMain.offsetWidth + SCROLL_BAR_WIDTH <= totalWidth - lockedWidth )
							{
								// 只出垂直滚动条
								divTitle.style.width = tableMain.offsetWidth;
								divMain.style.width = tableMain.offsetWidth + SCROLL_BAR_WIDTH;
								divMainFreeze.style.height = totalHeight - lockedHeight;		
								divMain.style.height = totalHeight - lockedHeight;
							}
							else
							{
								// 出两个滚动条
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
							// 宽度不够，高度够
							if(tableMain.offsetHeight + SCROLL_BAR_HEIGHT <= totalHeight - lockedHeight )
							{
								// 只出水平滚动条
								divTitle.style.width = totalWidth - lockedWidth;
								divMain.style.width = totalWidth  - lockedWidth;
								divMainFreeze.style.height = tableMain.offsetHeight;		
								divMain.style.height = tableMain.offsetHeight + SCROLL_BAR_HEIGHT;
							}
							else
							{
								// 出两个滚动条
								divTitle.style.width = totalWidth - lockedWidth - SCROLL_BAR_WIDTH;
								divMain.style.width = totalWidth  - lockedWidth;
								divMainFreeze.style.height = totalHeight - lockedHeight - SCROLL_BAR_HEIGHT;		
								divMain.style.height = totalHeight - lockedHeight;
							}
						}
						else
						{
							// 宽度不够，高度不够，出两个滚动条
							divTitle.style.width = totalWidth - lockedWidth - SCROLL_BAR_WIDTH;
							divMain.style.width = totalWidth  - lockedWidth;
							divMainFreeze.style.height = totalHeight - lockedHeight - SCROLL_BAR_HEIGHT;		
							divMain.style.height = totalHeight - lockedHeight;
						}						
						
					}						
					
					// 设置各层位置		
					divTitle.style.left = divTitleFreeze.style.left + lockedWidth;
					divTitle.style.top = lockedHeight * -1;
					divTitle.style.zIndex = divTitleFreeze.style.zIndex + 1;	
					
					divMainFreeze.style.top = (divTitle.offsetHeight) * -1;
					divMainFreeze.style.zIndex = divTitleFreeze.style.zIndex + 2;		
					
					divMain.style.top = (divTitle.offsetHeight + divMainFreeze.offsetHeight) * -1;
					divMain.style.left = divTitle.style.left;
					divMain.style.zIndex = divTitleFreeze.style.zIndex + 3;	
					
					// 设置各表的显示位置
					var tableMainCellBorderWidth = 0;
				");
			sb.Append("if(tableMain.rows[0].cells[0].style.borderWidth != \"\")\r\n");
			sb.Append(@"
						tableMainCellBorderWidth = parseInt(tableMain.rows[0].cells[0].style.borderWidth);
					
					tableTitle.style.marginLeft = (borderWidth + cellPadding + tableMainCellBorderWidth) * -1;		
					tableMainFreeze.style.marginTop = (borderWidth + cellPadding + tableMainCellBorderWidth) * -1;
					
					tableMain.style.marginLeft = (borderWidth + cellPadding + tableMainCellBorderWidth) * -1;	
					tableMain.style.marginTop = (borderWidth + cellPadding + tableMainCellBorderWidth) * -1;

					// 调整各元素的位置和尺寸，以变滚动时显示边界					
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
		/// 返回 JavaScript 函数 LockTable 的字符串
		/// </summary>
		private string AddJsfLockTable()
		{
			
			/*			  
				// 锁定表的	nLockedRowCount 行，nLockedColCount列
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
		/// 返回 JavaScript 函数 onDocumentReadyStateChange 的字符串
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
		/// 返回 JavaScript 函数 onTableResize 的字符串
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
		/// 返回 JavaScript 函数 onPropertyChange 的字符串
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
	/// 用来传递按列排序所需参数
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
