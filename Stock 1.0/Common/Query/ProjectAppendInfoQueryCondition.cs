using System;

namespace AISRS.Common.Query
{
	/// <summary>
	/// ProjectAppendInfoQueryCondition ��ժҪ˵����
	/// </summary>
	public class ProjectAppendInfoQueryCondition: QueryCondition
	{
		/// <summary>
		/// ��Ŀ5λ����
		/// </summary>
		public string ProjectCodeShort
		{
			get { return this.GetCondition("ProjectCodeShort",""); }
			set { this.SetCondition("ProjectCodeShort",value); } 
		}
		/// <summary>
		/// ��Ŀ������Ϣ���ʹ���;0:�������;1:����ת��;2:������Ŀ
		/// </summary>
		public int AppendInfoID
		{
			get { return this.GetCondition("AppendInfoID",0); }
			set { this.SetCondition("AppendInfoID",value); } 
		}
		
		/// <summary>
		/// ��Ŀ������Ϣ״̬;0:��;1��
		/// </summary>
		public int AppendInfoStatus
		{
			get { return this.GetCondition("AppendInfoStatus",0);}
			set { this.SetCondition("AppendInfoStatus",value);}
		}

		/// <summary>
		/// ��ʼʱ��
		/// </summary>
		public string BeginTime
		{
			get { return this.GetCondition("BeginTime","");}
			set { this.SetCondition("BeginTime",value);}
		}

		/// <summary>
		/// ����ʱ��
		/// </summary>
		public string EndTime
		{
			get { return this.GetCondition("EndTime","");}
			set { this.SetCondition("EndTime",value);}
		}
	}
}
