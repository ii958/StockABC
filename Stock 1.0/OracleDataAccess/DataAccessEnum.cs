using System;

namespace AISRS.DataAccess
{
	/// <summary>
	///  枚举删除操作返回结果
	/// </summary>
	public enum DeleteResult
	{
		Success = 0,
		Fail = 1,
		Refered = 2		
	}

	/// <summary>
	/// 枚举插入操作返回结果
	/// </summary>
	public enum InsertResult
	{
		Success = 0,
		Fail = 1,
		ObeyUniqueConstraint = 2
	}

	/// <summary>
	/// 枚举Update操作返回结果
	/// </summary>
	public enum UpdateResult
	{
		Success = 0,
		Fail = 1,
		ObeyUniqueConstraint = 2,
		RecordLocked = 3
	}
}
