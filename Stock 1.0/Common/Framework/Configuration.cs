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
	///     ���ļ���Web.config���ж�ȡ������Ϣ����������ֵ���ϡ�_settings���С�
	///     <remarks> 
	///			Ҫ�� global.asax ���¼���Ӧ���� Application_OnStart �У����ñ���� OnApplicationStart ������
	///			д�����£�
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
		
		#region ����������Ϣ����

		// ���ϱ������������д������ļ��ж�����������Ϣ
		private static NameValueCollection _settings;

		// ����Ӧ�õ�λ����Ϣ
		private static string _diskRoot;
		private static string _urlRoot;

		// ���������ַ���
		private static string _connectionString;

        //20120502 chencheng ������
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
		/// ����һ����������Ƽ�ֵ�ļ���
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="key">��ֵ�а����Ĺؼ���</param>
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
		/// WebӦ�õ���ַ
		/// </summary>
		public static string UrlRoot
		{
			get
			{
				return _urlRoot;
			}
		}

		/// <value>
		/// Ӧ�õĸ��ڷ�����Ӳ���ϵ������ַ
		/// </value>
		public static string DiskRoot
		{
			get
			{
				return _diskRoot;
			}
		}

		/// <value>
		///	���ݿ����Ӵ�
		/// </value>
		public static string DataAccessConnectionString
		{
			get
			{
				if(_connectionString != null && _connectionString != string.Empty)
					return _connectionString;
				else
					// ��Ԫ����ʱ�����������ļ���ֱ�Ӹ����Ӵ���ֵ
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
		/// ������ʾҳ��λ��
		/// </summary>
		public static string ErrorPagePath
		{
			get{ return "/ErrorPage.aspx";}
		}

		/// <summary>
		/// ��ʾϵͳ�Ƿ���������
		/// </summary>
		public static bool IsOnUpdating
		{
			get { return ReadSetting(_settings,"Framework.SystemState.OnUpdating",false); }
		}

		// ϵͳ������ʾ��Ϣ��ҳ��λ��
		public static string UpdateMessagePageUrl
		{
			get { return ReadSetting(_settings,"Framework.Common.UpdateMessagePage","");}
		}

		// ϵͳ�������ݣ������ٴη������������ʼ�����ز���
		public static DataTable AwokeParameters
		{
			get {return ReadSettingList(_settings,"AvokeParameter");}
		}
		/// <summary>
		/// ϵͳ����
		/// </summary>
		public static string SystemEmailAddress
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.SystemEmailAddress","shiqi_yao@bluematrix.com.cn");
			}
		}	
		
		/// <summary>
		/// ϵͳ����ԱEmail
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
		/// �Ƿ�BCC/CC��ϵͳ����
		/// </summary>
		public static string BCC_CC_ToSystemEmail
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.BCC_CC_ToSystemEmail","0");
			}
		}
		
		/// <summary>
		/// ��ϵ����Ϣ
		/// </summary>
		public static string LinkmanInfo
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.LinkmanInfo","");
			}
		}


		/// <summary>
		/// ϵͳ��ʽ��������
		/// </summary>
		public static DateTime SystemAppliedDate
		{
			get
			{
				return Convert.ToDateTime(ReadSetting(_settings,"Framework.SystemSetting.AppliedDate","2005-11-10"));
			}
		}

		/// <summary>
		/// ϵͳȨ�������Ƿ������˾��Ĭ��Ϊ0������������˾��ֻ��֤SBU
		/// </summary>
		public static int SystemPermissionIncludingCompany
		{
			get
			{
				return Convert.ToInt32(ReadSetting(_settings,"Framework.SystemSetting.SystemPermissionIncludingCompany","0"));
			}
		}

		/// <summary>
		/// Cognos�ṩ����ҳ���URL��ַ
		/// </summary>
		public static string SystemCognosImportUrl
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.CognosImportUrl","");
			}
		}

		/// <summary>
		/// �ж������·��Ƿ������Ŀ���ȱ���
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
		/// 20101111 chaidanlei Cognos�Զ����������趨������ע������ƻ�URL��ַ
		/// </summary>
		public static string SystemCognosImportScheduleTaskUrl
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.CognosImportScheduleTaskUrl","");
			}
		}
		
		/// <summary>
		/// �Զ���������COGNOSȡ�ɱ���EXE�ļ�����λ��
		/// </summary>
		public static string SystemCognosPrjPercentFilePath
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.CognosPrjPercentFilePath","");
			}
		}
		
		/// <summary>
		/// Job ����Ŀ¼λ��
		/// </summary>
		public static string SystemCognosPrjPercentJobFilePath
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.CognosPrjPercentJobFilePath","");
			}
		}
		
		/// <summary>
		/// ִ�е�������Ա
		/// </summary>
		public static string SystemCognosUser
		{
			get
			{
				return ReadSetting(_settings,"Framework.SystemSetting.User","");
			}
		}
		
		/// <summary>
		/// ִ�е�������Ա����
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
		/// 20120331 ����������ĿActual����Budget������Ŀ�Ľ������
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
		/// Log �ʼ������˵�ַ
		/// </summary>
		internal static String EventLogAddresser
		{
			get
			{
				return ReadSetting(_settings,"Framework.EventLog.Addresser","EventLogDefault@bluematrix.com.cn");
			}
		}
		/// <summary>
		/// Log �ʼ��ռ��˵�ַ
		/// </summary>
		internal static String EventLogAddressee
		{
			get
			{
				return ReadSetting(_settings,"Framework.EventLog.Addressee","Application@bluematrix.com.cn");
			}
		}
		/// <summary>
		/// Log �ʼ��Ƿ����������
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
		/// ��ȡ���ã��ַ������ķ���
		/// </summary>
		/// <param name="key">��Ҫ�������õļ�ֵ</param>
		/// <returns></returns>
		public static string GetConfiguration(string key)
		{
			return ReadSetting(_settings,key,"");
		}

		/// <summary>
		/// ���������Framework.DataAccess.ConnectionString�ԡ�registry:����ͷ�����ʾ���Ӵ�������ע����С�
		/// ������Ӵ�������ע����У����������ж������Ӵ���ע����е�λ�ã��������ý��ܺ󣬱��浽����_connectionString�С�
		/// ������Ӵ�û������ע����У�ֱ�Ӹ�ֵ������_connectionString��
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
				// ���Ӵ�û������ע�����
				_connectionString = connectionString;
			}
			else
			{
				// ���Ӵ�������ע�����
				string name = connectionString.Substring(promptLength);

				if(name.Trim() == string.Empty)
					return;
			
				SaveUpdateConnectionStringUrl(name);
				// ��ȡע����м��ܵ����Ӵ�
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

				// ����
				if(encryptedText != null)
				{
					byte[] plainText = (new DataProtector(DataProtector.Store.USE_MACHINE_STORE)).Decrypt(encryptedText,null);
					_connectionString = Encoding.Default.GetString(plainText);
				}
			}
		}

		/// <summary>
		/// �Ѹ������Ӵ���ҳ���URLд��ע�����
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
