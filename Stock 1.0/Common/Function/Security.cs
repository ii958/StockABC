using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace AISRS.Common.Function
{
	/// <summary>
	/// Security ��ժҪ˵����
	/// </summary>
	public class Security
	{
		public static string _encryptKey = "AbPkiZhe";
		/// <summary> 
		/// �����ַ��� 
		/// ע��:��Կ����Ϊ��λ 
		/// </summary> 
		/// <param name="strText">�ַ���</param> 
		/// <param name="_encryptKey">���ؼ��ܺ���ַ���</param> 
		public static string DesEncrypt(string inputString) 
		{ 
			byte[] byKey = null; 
			byte[] IV = { 0x12, 0x37, 0x16, 0x08, 0x90, 0xAB, 0xCD, 0xEF }; 
			try 
			{ 
				byKey = System.Text.Encoding.UTF8.GetBytes(_encryptKey.Substring(0, 8)); 
				DESCryptoServiceProvider des = new DESCryptoServiceProvider(); 
				byte[] inputByteArray = Encoding.UTF8.GetBytes(inputString); 
				MemoryStream ms = new MemoryStream(); 
				CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write); 
				cs.Write(inputByteArray, 0, inputByteArray.Length); 
				cs.FlushFinalBlock(); 
				return Convert.ToBase64String(ms.ToArray()); 
			} 
			catch (System.Exception error) 
			{ 
				return null; 
			} 
		} 

		/// <summary> 
		/// �����ַ��� 
		/// </summary> 
		/// <param name="this.inputString">�����ܵ��ַ���</param> 
		/// <param name="decryptKey">���ؽ��ܺ���ַ���</param> 
		public static string DesDecrypt(string inputString) 
		{ 
			byte[] byKey = null; 
			byte[] IV = { 0x12, 0x37, 0x16, 0x08, 0x90, 0xAB, 0xCD, 0xEF }; 
			byte[] inputByteArray = new Byte[inputString.Length]; 
			try 
			{ 
				byKey = System.Text.Encoding.UTF8.GetBytes(_encryptKey.Substring(0, 8)); 
				DESCryptoServiceProvider des = new DESCryptoServiceProvider(); 
				inputByteArray = Convert.FromBase64String(inputString); 
				MemoryStream ms = new MemoryStream(); 
				CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write); 
				cs.Write(inputByteArray, 0, inputByteArray.Length); 
				cs.FlushFinalBlock(); 
				System.Text.Encoding encoding = new System.Text.UTF8Encoding(); 
				return encoding.GetString(ms.ToArray()); 
			} 
			catch (System.Exception error) 
			{ 
				return null; 
			} 
		} 
	}
}
