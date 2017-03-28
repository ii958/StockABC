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
	/// NavigationAndPermission ��ժҪ˵����
	/// ������һ��Navigation���ԣ�����ΪXmlDocument��ֵΪNavigation.Xml ��Page�ڵ����Permission�ӽڵ���Xml�ĵ�	
	/// </summary>
	/*
	 *     <Category Title="��Cognos����">
	<Item Title="������Ŀ�ɱ���Ϣ">
	<Page Class="AISRS.WebUI.ImportFromCognos.ImportFromCognosBrowse" URL="/ImportFromCognos/ImportFromCognosBrowse.aspx"/>
	</Item>
	<Item Title="�����·�">
	<Page Class="AISRS.WebUI.ImportFromCognos.LockMonth" URL="/ImportFromCognos/LockMonth.aspx"/>
	</Item>
	</Category>	
	*/
	public class NavigationPermission
	{
		#region ����
		private const string _navigationXmlFileRelativePath = @"Modules\Navigation.xml";
		private static System.Web.Caching.CacheDependency _xmlDocumentDependencies;
		#endregion

		#region Property
		/// <summary>
		/// ����XmlDocument���浽Cache���������������
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
		/// ����Navigation.Xml ��Page�ڵ����Permission�ӽڵ���XmlDocument����
		/// Permission�ڵ���Ϣ�Ӹ�Page��Ӧ�����Permission�����еõ���
		/// ����Cache�������Permission�ӽڵ���XmlDocument����
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
					//��ȡNavigation.xml�ļ�						
					streamReader = new StreamReader(navigationXmlFilePath,Encoding.UTF8);
					string xmlDoc = streamReader.ReadToEnd();

					// ����һ��������ʽ��ԭXML�е�URL="****"�е�****���Сд
					string strPattern = "URL=\"(.)*\"";
					Regex regEx = new Regex(strPattern, RegexOptions.ECMAScript & RegexOptions.IgnoreCase);
					xmlDoc = regEx.Replace(xmlDoc, new MatchEvaluator(URLToLower));
			
					// ������XmlDocument���󣬲�װ��Navtigation Xml����
					permissionXml = new XmlDocument();
					permissionXml.LoadXml(xmlDoc);
					
					// ����Navtigation Xml���ݣ���ÿ��Page�ڵ��£����Permission�ڵ�
					XmlElement xmlRoot = permissionXml.DocumentElement;
					XmlNodeList categoryList = xmlRoot.ChildNodes;

					//�ҵ�Item��Ӧ����������ͨ�������ȡ������ԡ�
					//������ֵ�ӵ�Item���ӽڵ��С�
					foreach(XmlNode categoryNode in categoryList)
					{
						XmlNodeList itemList = categoryNode.ChildNodes;
						foreach(XmlNode itemNode in itemList)
						{
							//��ȡ�༰��Ȩ������
							XmlNodeList pageList = itemNode.ChildNodes;
							foreach(XmlNode pageNode in pageList)
							{
								string pageClass = pageNode.Attributes["Class"].Value;
								MemberInfo memberInfo = System.Type.GetType(pageClass);

								
								PermissionAttribute permissionAttribute = (PermissionAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(PermissionAttribute));
								if (permissionAttribute != null)
								{
									string[] permissions = permissionAttribute.Permissions;

									//��Ȩ�޼ӵ��ӽڵ�
									//<Permission>���Թ���.Ӧ����Ϣ.�鿴</Permission>	
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
					throw new Exception("����Permission Xmlʱ���� " + ex.Message);						
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
		
		#region ����
		/// <summary>
		/// ��URL="****"�е�****���Сд�ķ���
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
