using System;

namespace AISRS.DataAccess
{
	/// <summary>
	///  ö��ɾ���������ؽ��
	/// </summary>
	public enum DeleteResult
	{
		Success = 0,
		Fail = 1,
		Refered = 2		
	}

	/// <summary>
	/// ö�ٲ���������ؽ��
	/// </summary>
	public enum InsertResult
	{
		Success = 0,
		Fail = 1,
		ObeyUniqueConstraint = 2
	}

	/// <summary>
	/// ö��Update�������ؽ��
	/// </summary>
	public enum UpdateResult
	{
		Success = 0,
		Fail = 1,
		ObeyUniqueConstraint = 2,
		RecordLocked = 3
	}
}
