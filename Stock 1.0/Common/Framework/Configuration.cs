using System;
using System.Data;
using System.Collections;
using System.Diagnostics;
using System.Configuration;
using System.Xml;
using System.Collections.Specialized;
using System.Text;

using Microsoft.Win32;
using Bluematrix.DataProtection;

namespace AISRS.Common.Framework
{    
	/// <summary>
	///     从文件“Web.config”中读取配置信息，保存在名值集合“_settings”中。
	///     <remarks> 
	///			要在 global.asax 的事件响应函数 Application_OnStart 中，调用本类的 OnApplicationStart 方法。
	///			写法如下：
	///         <example>
	///             The global.asax file should be similar to the following code:
	///             <code>
	///                 <%@ Import Namespace="Framework"  %>
	///                 <script language="c#" runat="SERVER">
	///                     void Application_OnStart()
	///                     {
	///                         Configuration.OnApplicationStart(Context);
	///                     }
	///                 </script>
	///             </code>
	///         </example>
	///     </remarks>
	/// </summary>
	public class Configuration : IConfigurationSectionHandler
	{
		
		private const string COMPANY_NAME = "Blue Matrix";
		
		#region 保存配置信息变量

		// 集合变量，保存所有从配置文件中读到的配置信息
		private static NameValueCollection _settings;

		// 保存应用的位置信息
		private static string _diskRoot;
		private static string _urlRoot;

		// 保存链接字符串
		private static string _connectionString;

        //20120502 chencheng 考核期
        private static string _assessPeriod;
		#endregion
		
		#region Realization interface IConfigurationSectionHandler
    
		/// <summary>
		///     Called from OnApplicationStart to initialize settings from
		///     the Web.Config file(s). 
		///     <remarks>
		///         The app domain will restart if settings change, so there is 
		///         no reason to read these values more than once. This funtion
		///         uses the NameValueSectionHandler base class to generate a 
		///         hashtablefrom the XML, which is then used to store the current
		///         settings.  Because all settings are read here, we do not actually 
		///         store the generated hashtable object for later retrieval by
		///         Context.GetConfig. The application should use the accessor
		///         functions directly.
		///     </remarks>
		///     <param name="parent">An object created by processing a section 
		///         with this name in a Config.Web file in a parent directory.
		///     </param>
		///     <param name="configContext">The config's context.</param>
		///     <param name="section">The section to be read.</param>
		///     <retvalue>
		///		    <para>
		///             A ConfigOutput object: which we leave empty because all settings
		///             are stored at this point.
		///		    </para>
		///		    <para>
		///             null:  if there was an error.
		///		    </para>
		///	    </retvalue>
		/// </summary>
		public Object Create(Object parent, object configContext, XmlNode section)
		{
                  
			try
			{
				NameValueSectionHandler baseHandler = new NameValueSectionHandler();
				_settings = (NameValueCollection)baseHandler.Create(parent, configContext, section);
			}
			catch
			{
				_settings = null;
			}
			return null;
		}
		#endregion

		#region ReadSetting methods
    
		/// <summary>
		///     String version of ReadSetting.
		///     <remarks>
		///         Reads a setting from a hashtable and converts it to the correct
		///         type. One of these functions is provided for each type
		///         expected in the hash table. These are public so that other
		///         classes don't have to duplicate them to read settings from
		///         a hash table.
		///     </remarks>
		///     <param name="settings">The Hashtable to read from.</param>
		///     <param name="key">A key for the value in the Hashtable.</param>
		///     <param name="defaultValue">The default value if the item is not found.</param>
		///     <retvalue>
		///		    <para>value: from the hash table</para>
		///         <para>
		///             default: if the item is not in the table or cannot be case to the expected type.
		///		    </para>
		///	    </retvalue>
		/// </summary>
		public static string ReadSetting(NameValueCollection settings, String key, String defaultValue)
		{
			try
			{
				Object setting = settings[key];
            
				return (setting == null) ? defaultValue : (String)setting;
			}
			catch
			{
				return defaultValue;
			}
		}
    
		/// <summary>
		///     Boolean version of ReadSetting.
		///     <remarks>
		///         Reads a setting from a hashtable and converts it to the correct
		///         type. One of these functions is provided for each type
		///         expected in the hash table. These are public so that other
		///         classes don't have to duplicate them to read settings from
		///         a hash table.
		///     </remarks>
		///     <param name="settings">The Hashtable to read from.</param>
		///     <param name="key">A key for the value in the Hashtable.</param>
		///     <param name="defaultValue">The default value if the item is not found.</param>
		///     <retvalue>
		///		    <para>value: from the hash table</para>
		///         <para>
		///             default: if the item is not in the table or cannot be case to the expected type.
		///		    </para>
		///	    </retvalue>
		/// </summary>
		public static bool ReadSetting(NameValueCollection settings, String key, bool defaultValue)
		{
			try
			{
				Object setting = settings[key];
            
				return (setting == null) ? defaultValue : Convert.ToBoolean((String)setting);
			}
			catch
			{
				return defaultValue;
			}
		}
    
		/// <summary>
		///     int version of ReadSetting.
		///     <remarks>
		///         Reads a setting from a hashtable and converts it to the correct
		///         type. One of these functions is provided for each type
		///         expected in the hash table. These are public so that other
		///         classes don't have to duplicate them to read settings from
		///         a hash table.
		///     </remarks>
		///     <param name="settings">The Hashtable to read from.</param>
		///     <param name="key">A key for the value in the Hashtable.</param>
		///     <param name="defaultValue">The default value if the item is not found.</param>
		///     <retvalue>
		///		    <para>value: from the hash table</para>
		///         <para>
		///             default: if the item is not in the table or cannot be case to the expected type.
		///		    </para>
		///	    </retvalue>
		/// </summary>
		public static int ReadSetting(NameValueCollection settings, String key, int defaultValue)
		{
			try
			{
				Object setting = settings[key];
            
				return (setting == null) ? defaultValue : Convert.ToInt32((String)setting);
			}
			catch
			{
				return defaultValue;
			}
		}


    
		/// <summary>
		///     TraceLevel version of ReadSetting.
		///     <remarks>
		///         Reads a setting from a hashtable and converts it to the correct
		///         type. One of these functions is provided for each type
		///         expected in the hash table. These are public so that other
		///         classes don't have to duplicate them to read settings from
		///         a hash table.
		///     </remarks>
		///     <param name="settings">The Hashtable to read from.</param>
		///     <param name="key">A key for the value in the Hashtable.</param>
		///     <param name="defaultValue">The default value if the item is not found.</param>
		///     <retvalue>
		///		    <para>value: from the hash table</para>
		///         <para>
		///             default: if the item is not in the table or cannot be case to the expected type.
		///		    </para>
		///	    </retvalue>
		/// </summary>
		public static TraceLevel ReadSetting(NameValueCollection settings, String key, TraceLevel defaultValue)
		{
			try
			{
				Object setting = settings[key];
            
				return (setting == null) ? defaultValue : (TraceLevel)Convert.ToInt32((String)setting);
			}
			catch
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// 返回一组参数的名称及值的集合
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="key">键值中包含的关键字</param>
		/// <returns></returns>
		public static DataTable ReadSettingList(NameValueCollection settings, String key)
		{
			try
			{
				int settingCount = settings.Count;
				DataTable settingList  = new DataTable();
				settingList.Columns.Add("ParaName",typeof(System.String));
				settingList.Columns.Add("ParaValue",typeof(System.String));
				for (int i=0;i<settingCount;i++)
				{
					string settingname = (string)settings.AllKeys[i];
					string paramValue = string.Empty;
					string paramName = string.Empty;
					int strIndex = settingname.IndexOf(key);
					if (strIndex >= 0)
					{
						paramName = settingname.Substring(settingname.LastIndexOf(".")+1,settingname.Length - settingname.LastIndexOf(".")-1);
						if (paramName.Length > 0)
						{
							paramValue = ReadSetting(settings,settingname,"0");
							DataRow paraRow = settingList.NewRow();
							paraRow["ParaName"] = paramName;
							paraRow["ParaValue"] = paramValue;
							settingList.Rows.Add(paraRow);
						}
						
					}
				}
            
				return settingList;
			}
			catch
			{
				return new DataTable();
			}
		}

		#endregion
    
		/// <summary>
		///     Function to be called by Application_OnStart as described in the
		///     class description. Initializes the application root.
		///     <param name="diskRoot">The path of the running application.</param>
		/// </summary>
		public static void OnApplicationStart(string diskRoot)
		{
			_connectionString = string.Empty;
			_diskRoot = diskRoot;

            _assessPeriod = string.Empty;

			System.Configuration.ConfigurationSettings.GetConfig("Configuration");

			_urlRoot = ReadSetting(_settings,"Framework.Common.UrlRoot","");

			if(_urlRoot == string.Empty)
				Log.WriteError("UrlRoot Configuration Error.");

            _assessPeriod = GetConfiguration("Framework.SystemSetting.AssessPeriod");
			
            UpdateConnectionString();
		}

		public const string MouseBusy ="wait";
		public const string MouseIdle = "auto";
		#region Properties
    
		#region Public properties

		/// <summary>
		/// Web应用的网址
		/// </summary>
		public static string UrlRoot
		{
			get
			{
				return _urlRoot;
			}
		}

		/// <value>
		/// 应用的根在服务器硬盘上的物理地址
		/// </value>
		public static string DiskRoot
		{
			get
			{
				return _diskRoot;
			}
		}

		/// <value>
		///	数据库连接串
		/// </value>
		public static string DataAccessConnectionString
		{
			get
			{
				if(_connectionString != null && _connectionString != string.Empty)
					return _connectionString;
				else
					// 单元测试时，不读配置文件，直接给连接串赋值
                    return "Data Source=DESKTOP-EN4Q4FU;Initial Catalog=AIStock;User ID=sa;Password=qaz123$%";
			}
		}

        public static string AssessmentPeriod
        {
            get
            {
                if (_assessPeriod != null && _assessPeriod != string.Empty)
                    return _assessPeriod;
                else
                    return "4|6|12|24|36";
            }
        }

		/// <summary>
		/// 错误提示页面位置
		/// </summary>
		public static string ErrorPagePath
		{
			get{ return "/ErrorPage.aspx";}
		}

		/// <summary>
		/// 表示系统是否正在升级
		/// </summary>
		public static bool IsOnUpdating
		{
			get { return ReadSetting(_settings,"Framework.SystemState.OnUpdating",false); }
		}

		// 系统升级提示信息的页面位置
		public static string UpdateMessagePageUrl
		{
			get { return ReadSetting(_settings,"Framework.Common.UpdateMessagePage","");}
		}

		// 系统配置数据，控制再次发送提醒审批邮件的相关参数
		public static DataTable AwokeParameters
		{
			get {return ReadSettingList(_settings,"AvokeParameter");}
		}
		/// <summary>
		/// 系统信箱
		/// </summary>
		public static string SystemEmailAddress
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.SystemEmailAddress","shiqi_yao@bluematrix.com.cn");
			}
		}	
		
		/// <summary>
		/// 系统管理员Email
		/// </summary>
		public static string SystemAdminEmailAddress
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.SystemAdminEmailAddress","shiqi_yao@bluematrix.com.cn");
			}
		}
		
		public static string SystemNtAccount
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.SystemNtAccount","wangyz2");
			}
		}
		
		/// <summary>
		/// 是否BCC/CC给系统邮箱
		/// </summary>
		public static string BCC_CC_ToSystemEmail
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.BCC_CC_ToSystemEmail","0");
			}
		}
		
		/// <summary>
		/// 联系人信息
		/// </summary>
		public static string LinkmanInfo
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.LinkmanInfo","");
			}
		}


		/// <summary>
		/// 系统正式上线日期
		/// </summary>
		public static DateTime SystemAppliedDate
		{
			get
			{
				return Convert.ToDateTime(ReadSetting(_settings,"Framework.SystemSetting.AppliedDate","2005-11-10"));
			}
		}

		/// <summary>
		/// 系统权限设置是否包括公司，默认为0，即不包括公司，只验证SBU
		/// </summary>
		public static int SystemPermissionIncludingCompany
		{
			get
			{
				return Convert.ToInt32(ReadSetting(_settings,"Framework.SystemSetting.SystemPermissionIncludingCompany","0"));
			}
		}

		/// <summary>
		/// Cognos提供导数页面的URL地址
		/// </summary>
		public static string SystemCognosImportUrl
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.CognosImportUrl","");
			}
		}

		/// <summary>
		/// 判断锁定月份是否控制项目进度报告
		/// </summary>
		public static bool SystemLockMonthControlProcessReport
		{
			get
			{
				int isControl = Convert.ToInt32(ReadSetting(_settings,"Framework.SystemSetting.IsLockMonthControlProcessReport",""));
				if (isControl == 0)
				{
					return false;
				}
				else
				{
					return true;
				}
		    }
		}
		
		/// <summary>
		/// 
		/// </summary>
		public static string ExcelBatchUrl
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.ExcelBatchUrl","");
			}
		}
		/// <summary>
		/// 20101111 chaidanlei Cognos自动导数参数设定创建、注销任务计划URL地址
		/// </summary>
		public static string SystemCognosImportScheduleTaskUrl
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.CognosImportScheduleTaskUrl","");
			}
		}
		
		/// <summary>
		/// 自动导数调用COGNOS取成本的EXE文件所在位置
		/// </summary>
		public static string SystemCognosPrjPercentFilePath
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.CognosPrjPercentFilePath","");
			}
		}
		
		/// <summary>
		/// Job 所在目录位置
		/// </summary>
		public static string SystemCognosPrjPercentJobFilePath
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.CognosPrjPercentJobFilePath","");
			}
		}
		
		/// <summary>
		/// 执行导数的人员
		/// </summary>
		public static string SystemCognosUser
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.User","");
			}
		}
		
		/// <summary>
		/// 执行导数的人员密码
		/// </summary>
		public static string SystemCognosPwd
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.Pwd","");
			}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public static string SystemCognosDomain
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.Domain","AILK");
			}
		}

		/// <summary>
		/// 20120331 增加配置项目Actual大于Budget计算项目的金额限制
		/// </summary>
		public static decimal ActualGreaterBudgetValue
		{
			get
			{
				return decimal.Parse(ReadSetting(_settings,"Framework.SystemSetting.ActualGreaterBudgetValue","100"));
			}
		}
		#endregion
    
		#region Internal properties
		

		/// <value>
		///     Property EventLogEnabled is used to get whether writing to the event log is support, defaults to True.
		///     <remarks>Returns true if writing to the event log is enabled, false otherwise</remarks>
		/// </value>
		internal static bool EventLogEnabled
		{
			get
			{
				return ReadSetting(_settings,"Framework.EventLog.Enabled",true);
			}
		}
		/// <value>
		///     Property EventLogMachineName is used to get the machine name to log the event to, defaults to an
		///     empty string, indicating the current machine.  A machine name 
		///     (without \\), may be empty.
		/// </value>
		internal static string EventLogFileName
		{
			get
			{
				return _diskRoot + "\\" + ReadSetting(_settings,"Framework.EventLog.FileName","Log.txt");									  
			}
		}

		/// <value>
		///     Property EventLogTraceLevel is used to get the highest logging level that should be written to the event log,
		///     defaults to TraceLevel.Error.
		/// </value>
		internal static TraceLevel EventLogTraceLevel
		{
			get
			{
				return ReadSetting(_settings,"Framework.EventLog.LogLevel",TraceLevel.Error);
			}
		}

		/// <value>
		///		Log File Max Length
		/// </value>
		internal static int EventLogFileMaxLength
		{
			get
			{
				return ReadSetting(_settings,"Framework.EventLog.LogFileMaxLength",1000000);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		internal static String EventLogSendEmailSwitch
		{
			get
			{
				return ReadSetting(_settings,"Framework.EventLog.SendEmailSwitch","off");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		internal static int EventLogScanCycle
		{
			get
			{
				return ReadSetting(_settings,"Framework.EventLog.ScanCycle",24);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		internal static int EventLogSendCycle
		{
			get
			{
				return ReadSetting(_settings,"Framework.EventLog.SendCycle",1);
			}
		}
		/// <summary>
		/// Log 邮件发件人地址
		/// </summary>
		internal static String EventLogAddresser
		{
			get
			{
				return ReadSetting(_settings,"Framework.EventLog.Addresser","EventLogDefault@bluematrix.com.cn");
			}
		}
		/// <summary>
		/// Log 邮件收件人地址
		/// </summary>
		internal static String EventLogAddressee
		{
			get
			{
				return ReadSetting(_settings,"Framework.EventLog.Addressee","Application@bluematrix.com.cn");
			}
		}
		/// <summary>
		/// Log 邮件是否带附件开关
		/// </summary>
		internal static String EventLogAccessoriesSwitch
		{
			get
			{
				return ReadSetting(_settings,"Framework.EventLog.AccessoriesSwitch","on");
			}
		}
	
		/// <summary>
		/// 
		/// </summary>
		internal static int EventLogEmailSendBeginTime
		{
			get
			{
				return ReadSetting(_settings,"Framework.EventLog.EmailSendBeginTime",23);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		internal static int EventLogEmailSendEndTime
		{
			get
			{
				return ReadSetting(_settings,"Framework.EventLog.EmailSendEndTime",1);
			}
		}

		#endregion

		#endregion


		/// <summary>
		/// 获取配置（字符串）的方法
		/// </summary>
		/// <param name="key">需要读的配置的键值</param>
		/// <returns></returns>
		public static string GetConfiguration(string key)
		{
			return ReadSetting(_settings,key,"");
		}

		/// <summary>
		/// 如果配置项Framework.DataAccess.ConnectionString以“registry:”开头，则表示链接串保存在注册表中。
		/// 如果连接串保存在注册表中，从配置项中读出连接串在注册表中的位置，读出配置解密后，保存到变量_connectionString中。
		/// 如果连接串没保存在注册表中，直接赋值给变量_connectionString。
		/// </summary>
		public static void UpdateConnectionString()
		{
			string prompt = "registry:";
			string connectionString = GetConfiguration("Framework.DataAccess.ConnectionString");

			if(connectionString.Trim() == string.Empty)
				return;
			
			int promptLength = prompt.Length;
			if(connectionString.Substring(0,promptLength).ToLower() != prompt)
			{
				// 连接串没保存在注册表中
				_connectionString = connectionString;
			}
			else
			{
				// 连接串保存在注册表中
				string name = connectionString.Substring(promptLength);

				if(name.Trim() == string.Empty)
					return;
			
				SaveUpdateConnectionStringUrl(name);
				// 读取注册表中加密的链接串
				byte[] encryptedText = null;
			
				try
				{
					RegistryKey keyLocalMachine = Registry.LocalMachine;

					RegistryKey keySoftware = keyLocalMachine.OpenSubKey("Software");
					if(keySoftware==null)
					{
						keyLocalMachine.Close();
						return;
					}

					RegistryKey keyCompany = keySoftware.OpenSubKey(COMPANY_NAME);
					if(keyCompany == null)
					{
						keySoftware.Close();
						keyLocalMachine.Close();
						return;
					}

					RegistryKey keyConnectionStrings = keyCompany.OpenSubKey("Connection Strings");
					if(keyConnectionStrings == null)
					{
						keyCompany.Close();
						keySoftware.Close();
						keyLocalMachine.Close();
						return;
					}

					encryptedText = (byte[])(keyConnectionStrings.GetValue(name));
					
					keyConnectionStrings.Close();
					keyCompany.Close();
					keySoftware.Close();
					keyLocalMachine.Close();

				}
				catch(System.Exception ex)
				{
					Log.WriteError("When get connection string from registry: " + ex.Message);
				}				

				// 解密
				if(encryptedText != null)
				{
					byte[] plainText = (new DataProtector(DataProtector.Store.USE_MACHINE_STORE)).Decrypt(encryptedText,null);
					_connectionString = Encoding.Default.GetString(plainText);
				}
			}
		}

		/// <summary>
		/// 把更新连接串的页面的URL写到注册表中
		/// </summary>
		/// <param name="name"></param>
		public static void SaveUpdateConnectionStringUrl(string name)
		{
			try
			{
				RegistryKey keyLocalMachine = Registry.LocalMachine;

				RegistryKey keySoftware = keyLocalMachine.OpenSubKey("Software",true);
				if(keySoftware==null)
				{
					keySoftware = keyLocalMachine.CreateSubKey("Software");
				}

				RegistryKey keyCompany = keySoftware.OpenSubKey(COMPANY_NAME,true);
				if(keyCompany == null)
				{
					keyCompany = keySoftware.CreateSubKey(COMPANY_NAME);
				}

				RegistryKey keyUpdateConnectionStringUrls = keyCompany.OpenSubKey("Update Connection String Urls",true);
				if(keyUpdateConnectionStringUrls == null)
				{
					keyUpdateConnectionStringUrls = keyCompany.CreateSubKey("Update Connection String Urls");
				}

				keyUpdateConnectionStringUrls.SetValue(name,_urlRoot + "/UpdateConnectionString.aspx");
				
				keyUpdateConnectionStringUrls.Close();
				keyCompany.Close();
				keySoftware.Close();
				keyLocalMachine.Close(); 

			}
			catch(System.Exception ex)
			{
				Log.WriteError("When save update connection string to registry: " + ex.Message);
			}
		}
		
	}
}
