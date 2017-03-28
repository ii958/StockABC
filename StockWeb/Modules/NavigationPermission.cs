using System;
using System.Xml;
using System.IO;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using AISRS.Common.Framework;

namespace AISRS.WebUI.Modules
{
	/// <summary>
	/// 	
	/// NavigationAndPermission 的摘要说明。
	/// 类中有一个Navigation属性，类型为XmlDocument，值为Navigation.Xml 各Page节点加上Permission子节点后的Xml文档	
	/// </summary>
	/*
	 *     <Category Title="从Cognos导数">
	<Item Title="导入项目成本信息">
	<Page Class="AISRS.WebUI.ImportFromCognos.ImportFromCognosBrowse" URL="/ImportFromCognos/ImportFromCognosBrowse.aspx"/>
	</Item>
	<Item Title="锁定月份">
	<Page Class="AISRS.WebUI.ImportFromCognos.LockMonth" URL="/ImportFromCognos/LockMonth.aspx"/>
	</Item>
	</Category>	
	*/
	public class NavigationPermission
	{
		#region 变量
		private const string _navigationXmlFileRelativePath = @"Modules\Navigation.xml";
		private static System.Web.Caching.CacheDependency _xmlDocumentDependencies;
		#endregion

		#region Property
		/// <summary>
		/// 属性XmlDocument保存到Cache中所依赖项的数组
		/// </summary>
		public static System.Web.Caching.CacheDependency XmlDocumentCacheDependencies
		{
			get 
			{				
				if(_xmlDocumentDependencies == null)
				{
					string[] dependFiles = new string[]
					{
						Configuration.DiskRoot + "\\" + _navigationXmlFileRelativePath,
						Configuration.DiskRoot + @"\bin\AISRS_Demodll"
					};
					_xmlDocumentDependencies = new System.Web.Caching.CacheDependency(dependFiles);					
					
				}
				return _xmlDocumentDependencies;
			}
		}

		/// <summary>
		/// 返回Navigation.Xml 各Page节点加上Permission子节点后的XmlDocument对象，
		/// Permission节点信息从各Page对应的类的Permission属性中得到。
		/// 利用Cache缓存加上Permission子节点后的XmlDocument对象
		/// </summary>
		public static XmlDocument XmlDocument
		{
			get
			{	
				string navigationXmlFilePath = Configuration.DiskRoot + "\\" + _navigationXmlFileRelativePath;
				StreamReader streamReader = null;

				XmlDocument permissionXml;
				try
				{									
					//读取Navigation.xml文件						
					streamReader = new StreamReader(navigationXmlFilePath,Encoding.UTF8);
					string xmlDoc = streamReader.ReadToEnd();

					// 定义一个正则表达式将原XML中的URL="****"中的****变成小写
					string strPattern = "URL=\"(.)*\"";
					Regex regEx = new Regex(strPattern, RegexOptions.ECMAScript & RegexOptions.IgnoreCase);
					xmlDoc = regEx.Replace(xmlDoc, new MatchEvaluator(URLToLower));
			
					// 构造新XmlDocument对象，并装载Navtigation Xml数据
					permissionXml = new XmlDocument();
					permissionXml.LoadXml(xmlDoc);
					
					// 遍历Navtigation Xml数据，在每个Page节点下，添加Permission节点
					XmlElement xmlRoot = permissionXml.DocumentElement;
					XmlNodeList categoryList = xmlRoot.ChildNodes;

					//找到Item对应的类名，并通过反射获取类的属性。
					//将属性值加到Item的子节点中。
					foreach(XmlNode categoryNode in categoryList)
					{
						XmlNodeList itemList = categoryNode.ChildNodes;
						foreach(XmlNode itemNode in itemList)
						{
							//获取类及其权限属性
							XmlNodeList pageList = itemNode.ChildNodes;
							foreach(XmlNode pageNode in pageList)
							{
								string pageClass = pageNode.Attributes["Class"].Value;
								MemberInfo memberInfo = System.Type.GetType(pageClass);

								
								PermissionAttribute permissionAttribute = (PermissionAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(PermissionAttribute));
								if (permissionAttribute != null)
								{
									string[] permissions = permissionAttribute.Permissions;

									//将权限加到子节点
									//<Permission>测试管理.应用信息.查看</Permission>	
									foreach(string permission in permissions)
									{
										XmlElement permissionNode = permissionXml.CreateElement("Permission");	
										permissionNode.InnerText = permission;
										pageNode.AppendChild(permissionNode);
						
									}//end foreach
								}//end if
								
															
							}//end foreach pageNode                                															
						}//end foreach itemNode
					}//end foreach categoryNode							

				}
				catch(Exception ex)
				{
					permissionXml = null;
					throw new Exception("生成Permission Xml时出错： " + ex.Message);						
				}
				finally
				{							
					if(streamReader!=null)
					{
						streamReader.Close();
					}	
				}									
				return permissionXml;
			}			
		}
		#endregion
		
		#region 方法
		/// <summary>
		/// 将URL="****"中的****变成小写的方法
		/// </summary>
		/// <param name="match"></param>
		/// <returns></returns>
		private static string URLToLower(Match match)
		{
			return match.Value.ToLower();
		}
		#endregion

	}
}
