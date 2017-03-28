using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Web.Mail;

namespace AISRS.Common.Framework
{
	/// <summary>
	/// Log ��ժҪ˵����
	/// </summary>
	public class Log 
	{
		#region ˽�б�������
        
		//EventLog variables used for the trace level if the event log is specified.
		private static TraceLevel _eventLogTraceLevel;

		private static StreamWriter _logWriter;
		private static System.Timers.Timer _timerLogSendEmail;

		#endregion

		static Log()
		{
			//
			// Read the current settings from the configuration file to determine
			//   whether we need trace file support, event logging, or both.
			//
        
			//Get the class object in order to take the initialization lock
			Type myType = typeof(Log);
        
			//Protect thread locks with Try/Catch to guarantee that we let go of the lock.
			try
			{
				_timerLogSendEmail = new System.Timers.Timer();
				_timerLogSendEmail.Elapsed += new System.Timers.ElapsedEventHandler(timerLogSendEmail_Elapsed);
				_timerLogSendEmail.Interval = 1000 * 60 * 10;
				_timerLogSendEmail.AutoReset = true;
				_timerLogSendEmail.Enabled = true;

				//See if anyone else is using the lock, grab it if they//re not
				if (!Monitor.TryEnter(myType))
				{
					//Just wait until the other thread finishes processing, then leave if
					//  the lock was already in use.
					Monitor.Enter(myType);
					return;
				}
        
				_eventLogTraceLevel = Configuration.EventLogTraceLevel;
				_logWriter = new StreamWriter(Configuration.EventLogFileName, true);
			}
			catch{}
			finally
			{
				//Remove the lock from the class object
				Monitor.Exit(myType);
			}
		}


		#region д��־����

		/// <summary>
		///     Write at the Error level to the event log and/or tracing file.
		///     <param name="message">The text to write to the log file or event log.</param>
		/// </summary>
		public static void WriteError(string message)
		{
			//Defer to the helper function to log the message.
			WriteLog(TraceLevel.Error, message);
		}
        
		/// <summary>
		///     Write at the Warning level to the event log and/or tracing file.
		///     <param name="message">The text to write to the log file or event log.</param>
		/// </summary>
		public static void WriteWarning(string message)
		{
			//Defer to the helper function to log the message.
			WriteLog(TraceLevel.Warning, message);
		}
        
		/// <summary>
		///     Write at the Info level to the event log and/or tracing file.
		///     <param name="message">The text to write to the log file or event log.</param>
		/// </summary>
		public static void WriteInfo(string message)
		{
			//Defer to the helper function to log the message.
			WriteLog(TraceLevel.Info, message);
		}
        
		/// <summary>
		///     Write at the Verbose level to the event log and/or tracing file.
		///     <param name="message">The text to write to the log file or event log.</param>
		/// </summary>
		public static void WriteTrace(string message)
		{
			//Defer to the helper function to log the message.
			WriteLog(TraceLevel.Verbose, message);
		}

		/// <summary>
		///     Determine where a string needs to be written based on the
		///     configuration settings and the error level.
		///     <param name="level">The severity of the information to be logged.</param>
		///     <param name="messageText">The string to be logged.</param>
		/// </summary>
		private static void WriteLog(TraceLevel level, string messageText)
		{
			
			//
			// Write the message to the system event log. We only write the message
			//   if the configuration settings say it is severe enough to warrant
			//   an entry in the event log.
			//
			if(level > _eventLogTraceLevel)
				return;

			//
			// Map the trace level to the corresponding event log attribute.
			//   Note that EventLogEntryType = 2 ^ (level - 1), but it is generally not
			//   considered good style to apply arithmetic operations to enum values.
			//					EventLogEntryType LogEntryType;
			string logEntryType;
			switch (level)
			{
				case TraceLevel.Error:
					logEntryType = "Error";
					break;
				case TraceLevel.Warning:
					logEntryType = "Warning";
					break;
				case TraceLevel.Info:
					logEntryType = "Information";
					break;
				case TraceLevel.Verbose:
					logEntryType = "Verbose";
					break;
				default:
					logEntryType = "Verbose";
					break;
			}

			//
			// Be very careful by putting a Try/Catch around the entire routine.
			//   We should never throw an exception while logging.
			//
			try
			{				
				if (_logWriter == null)
				{
					_logWriter = new StreamWriter(Configuration.EventLogFileName,true);
				}

				lock(_logWriter)
				{

					// ��¼��־
					DateTime dt = DateTime.Now;

					_logWriter.WriteLine("<logitem>");
					_logWriter.WriteLine("<time>" + GetDateString(dt) + " " + GetTimeString(dt) + "</time>");
					_logWriter.WriteLine("<type>" + logEntryType + "</type>");
					_logWriter.WriteLine("<info>\r\n");
					_logWriter.WriteLine(messageText);
					_logWriter.WriteLine("\r\n</info>");
					_logWriter.WriteLine("</logitem>" + "\r\n");
				
					// ����־���ȴ������ó���ʱ�����һ���ļ�
					if(_logWriter.BaseStream.Length >= Configuration.EventLogFileMaxLength)
					{
						// ����浵�ļ�����,�㷨�� ��־�ļ��� + "_YYYY-MM-DD_HH-MM-SS" + �����ļ���չ��������־�ļ���Ϊ "log.txt",�浵�ļ�����Ϊ"log_2004-06-09_14-55-11.txt" 
						string logFileName = Configuration.EventLogFileName;
						FileInfo fileInfo = new FileInfo(logFileName);			
						string archiveFileName = logFileName.Substring(0,logFileName.Length - fileInfo.Extension.Length) + GetDateString(dt) + "_" + GetTimeString(dt).Replace(":","-") + fileInfo.Extension;
						
						_logWriter.Close();
					
						System.IO.File.Move(logFileName,archiveFileName);					
						_logWriter = new StreamWriter(logFileName,false);
					}

					// ˢ�´����ļ�����
					_logWriter.Flush();
				}					
				
			}
			catch(System.Exception ex)
			{
				string a = ex.Message;
			}//Ignore any exceptions.
		}

		// ���ر�ʾ��ǰ���ڵ��ַ�������ʽΪ YYYY-MM-DD
		private static string GetDateString(DateTime date)
		{
			return date.Year.ToString() + "-" + date.Month.ToString().PadLeft(2,'0') + "-" + date.Day.ToString().PadLeft(2,'0');
		}	

		// ���ر�ʾ��ǰʱ����ַ�������ʽΪ HH:MM:SS
		private static string GetTimeString(DateTime time)
		{
			return time.Hour.ToString() + ":" + time.Minute.ToString().PadLeft(2,'0') + ":" + time.Second.ToString().PadLeft(2,'0');
		}
		#endregion

		#region ������־Email����

		/// <summary>
		/// ������־Email
		/// </summary>
		private static void SendLogEmail()
		{
			try
			{
				//�ж��Ƿ���Email
				if(Configuration.EventLogSendEmailSwitch == "on")
				{
					//��Ҫ�����ʼ�
					
					//��õ�ǰ����
					DateTime now = DateTime.Now;
					//���ɨ�����ڣ�Сʱ��
					TimeSpan scanCycle = new TimeSpan(Configuration.EventLogScanCycle,0,0);
					//��÷������ڣ����ӣ�
					TimeSpan sendCycle = new TimeSpan(0,Configuration.EventLogSendCycle,0);

					//ȡ��Log�ļ����ڵ�Ŀ¼
					FileInfo defaultLogFile = new FileInfo(Configuration.EventLogFileName);
					DirectoryInfo logDir = defaultLogFile.Directory;

					//���LogĿ¼�µ�����Log�ļ�
					FileInfo[] logFiles = logDir.GetFiles();

					//��¼���һ�μ�¼��־��ʱ��
					DateTime lastWriteLogTime = DateTime.Now;

					//�ж��Ƿ������
					if(Configuration.EventLogAccessoriesSwitch == "on")
					{
						//��Ҫ������

						//����һ��StringBuilder������ż�����
						StringBuilder content = new StringBuilder();
						//����һ��bool�ͱ�־����ʾ�Ƿ�������һ����־���޸Ĺ�
						bool isExist = false;

						//��֯�ż����ݣ����������ż���

						//��֯�ż��Ŀ�ͷ�������ݣ����������ż���
						content.Append("Hi Manager,<br>");
						content.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
						content.Append("ϵͳ�������ڼ������ϵͳ���ϣ���¼���ϵ���־�ļ��Ѿ���Ϊ�������͸�����������ʱ�鿴��<br>");
							
						//��֯�ż������岿�֣����������ż���
						content.Append("");

						//��֯�ż��Ľ�β���֣����������ż���
						content.Append("<br>");

						//��LogĿ¼�µ�������Ҫ���͵��ļ���Ϊ��������
						foreach(FileInfo file in logFiles)
						{
							//��¼���һ�μ�¼��־��ʱ��
							if(lastWriteLogTime > file.LastWriteTime)
								lastWriteLogTime = file.LastWriteTime;
							
							//�жϴ��ļ�������޸������Ƿ�����һ��ɨ��֮��
							if((now - file.LastWriteTime) > scanCycle)
							{
								//���ϴ�ɨ��֮�󣬴���־�ļ�û�б��޸Ĺ�
							}
							else
							{
								//���ϴ�ɨ��֮�󣬴���־�ļ����޸�

								//����־���޸ģ����־ΪTrue
								isExist = true;

								//�����ļ���Ϊ�������ͳ�ȥ
								if(Configuration.EventLogFileName != file.FullName)
									SendEmailWithAccessories(file.FullName,content.ToString());								
								else
								{
									// ��ǰ����ʹ�õ���־�ļ������ڱ�ϵͳ�򿪣��޷���Ϊ�������͡�
									// ���԰ѵ�ǰ����־�ļ�Copy����һ���ļ��У��Ա㷢�͡�
									// ��Ϊ������־�ļ������ڻ�Ƚϳ�������ÿ�ζ����Ƶ�ͬһ���ļ������ļ��У��Լ�����ʱ�ļ�����
									// 
									string logFileName = file.FullName;
									string tempLogFileName = logFileName.Substring(0,logFileName.Length - file.Extension.Length) + "_Current" + file.Extension;
									try
									{
										file.CopyTo(tempLogFileName,true);
										SendEmailWithAccessories(tempLogFileName,content.ToString());	
									}
									catch(System.Exception ex)
									{
										Log.WriteError("When copy current log to temperary file: " + ex.Message);
									}
                                    
								}

								//�ȴ������ż�����ֹ������
								System.Threading.Thread.Sleep(Configuration.EventLogSendCycle * 60 * 1000);
							}
						}

						//�ж��Ƿ�����־�ļ����޸�
						if(isExist)
						{
							//����־�ļ����޸�
						}
						else
						{
							//û����־�ļ����޸�

							//��֯�ż�����
							content = new StringBuilder();

							//��֯�ż��Ŀ�ͷ��������							
							content.Append("Hi Manager,<br>");
							content.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
							content.Append("ϵͳ�� " + GetDateString(lastWriteLogTime) + " " + GetTimeString(lastWriteLogTime) + " ��û�м�¼�µ���־��<br><br>");
							
							//��֯�ż������岿��
							content.Append("");

							//��֯�ż��Ľ�β����
							content.Append("<br>");

							//�����ż�
							SendEmailWithoutAccessories(content.ToString());
						}

					}
					else
					{
						//����Ҫ������

						//����һ��StringBuilder������ż�����������
						System.Text.StringBuilder body = new StringBuilder();
						//����һ��bool�ͱ�־����ʾ�Ƿ�������һ����־���޸Ĺ�
						bool isExist = false;

						//����LogĿ¼�µ�������Ҫ���͵��ļ���֯�ż�����
						foreach(FileInfo file in logFiles)
						{
							if(lastWriteLogTime > file.LastWriteTime)
								lastWriteLogTime = file.LastWriteTime;

							if((now - file.LastWriteTime) > scanCycle)
							{
								//���ϴ�ɨ��֮�󣬴���־�ļ�û�б��޸�
							}
							else
							{
								//���ϴ�ɨ��֮�󣬴���־�ļ����޸�

								//����־���޸ģ����־ΪTrue
								isExist = true;

								//�����ļ���¼��ӵ��ż�����
								string href = Configuration.UrlRoot + @"\Log\" + file.Name;	//�����ļ�����
								body.Append("&nbsp:&nbsp:&nbsp:&nbsp:");
								body.Append("<a href=\"" + href + "\" target=\"_blank\">");
								body.Append(file.Name);
								body.Append("</a><br>");
							}
						}

						//����һ��StringBuilder������ż�����
						StringBuilder content = new StringBuilder();

						//�ж��Ƿ�����־�ļ����޸�
						if(isExist)
						{
							//����־�ļ����޸�

							//��֯�ż�����

							//��֯�ż��Ŀ�ͷ��������
							content.Append("Hi Manager,<br>");
							content.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
							content.Append("ϵͳ�������ڼ������ϵͳ���ϣ��Ѿ���¼��־��������ʱ�鿴��<br>");
							content.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
							content.Append("����ϵͳ��־��¼��ϵͳ���ϣ�����鿴��<br><br>");
							
							//��֯�ż������岿��
							content.Append(body.ToString());

							//��֯�ż��Ľ�β����
							content.Append("<br>");
						}
						else
						{
							//û����־�ļ����޸�

							//��֯�ż�����

							//��֯�ż��Ŀ�ͷ��������							
							content.Append("Hi Manager,<br>");
							content.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
							content.Append("ϵͳ�� " + GetDateString(lastWriteLogTime) + " " + GetTimeString(lastWriteLogTime) + " ��û�м�¼�µ���־��<br><br>");
							
							//��֯�ż������岿��
							content.Append("");

							//��֯�ż��Ľ�β����
							content.Append("<br>");
						}

						//�����ż�
						SendEmailWithoutAccessories(content.ToString());
					}
				}
				else
				{
					//����Ҫ����Email
				}
			}
			catch(System.Exception ex)
			{
				string s = ex.Message;
			}
		}

		/// <summary>
		/// ���ʹ�������Email
		/// </summary>
		/// <param name="accessories">�����ļ��������ַ</param>
		private static void SendEmailWithAccessories(string accessories,string content)
		{
			System.Web.Mail.MailMessage message = new System.Web.Mail.MailMessage();
			System.Web.Mail.MailAttachment attachment = new System.Web.Mail.MailAttachment(accessories);	//����һ������
			message.Attachments.Add(attachment);
			message.BodyFormat = System.Web.Mail.MailFormat.Html;
			message.From = Configuration.EventLogAddresser;
			message.To = Configuration.EventLogAddressee;
			message.Cc = "";
			message.Bcc = "";
			message.Subject = "ϵͳ��־����ɨ�豨��";
			message.Body = content;
			System.Web.Mail.SmtpMail.Send(message);
		}

		/// <summary>
		/// ���Ͳ���������Email
		/// </summary>
		/// <param name="content">Email������</param>
		private static void SendEmailWithoutAccessories(string content)
		{
			System.Web.Mail.MailMessage message = new System.Web.Mail.MailMessage();
			message.BodyFormat = System.Web.Mail.MailFormat.Html;
			message.From = Configuration.EventLogAddresser;
			message.To = Configuration.EventLogAddressee;
			message.Cc = "";
			message.Bcc = "";
			message.Subject = "ϵͳ��־����ɨ�豨��";
			message.Body = content;
			System.Web.Mail.SmtpMail.Send(message);
		}

		#endregion		

		private static void timerLogSendEmail_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			try
			{
				DateTime nowTime = DateTime.Now;
				DateTime beginTime = new DateTime(nowTime.Year,nowTime.Month,nowTime.Day,Configuration.EventLogEmailSendBeginTime,0,0,0);
				DateTime endTime = new DateTime(nowTime.Year,nowTime.Month,nowTime.Day,Configuration.EventLogEmailSendEndTime,0,0,0);

				if(Configuration.EventLogEmailSendBeginTime >= Configuration.EventLogEmailSendEndTime)
					endTime =  endTime.AddDays(1);

				if(nowTime > beginTime && nowTime < endTime)
				{
					_timerLogSendEmail.Interval = Configuration.EventLogScanCycle * 3600 * 1000;
					Thread t = new Thread(new ThreadStart(SendLogEmail));
					t.Start();					
				}
				else
				{
					if(_timerLogSendEmail.Interval != 1000 * 60 * 10)
						_timerLogSendEmail.Interval = 1000 * 60 * 10;
				}
			}
			catch{}
		}
	}
}
