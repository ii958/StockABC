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
	/// Log 的摘要说明。
	/// </summary>
	public class Log 
	{
		#region 私有变量定义
        
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


		#region 写日志方法

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

					// 记录日志
					DateTime dt = DateTime.Now;

					_logWriter.WriteLine("<logitem>");
					_logWriter.WriteLine("<time>" + GetDateString(dt) + " " + GetTimeString(dt) + "</time>");
					_logWriter.WriteLine("<type>" + logEntryType + "</type>");
					_logWriter.WriteLine("<info>\r\n");
					_logWriter.WriteLine(messageText);
					_logWriter.WriteLine("\r\n</info>");
					_logWriter.WriteLine("</logitem>" + "\r\n");
				
					// 当日志长度大于设置长度时，存成一个文件
					if(_logWriter.BaseStream.Length >= Configuration.EventLogFileMaxLength)
					{
						// 计算存档文件名称,算法是 日志文件名 + "_YYYY-MM-DD_HH-MM-SS" + 日至文件扩展名。如日志文件名为 "log.txt",存档文件名称为"log_2004-06-09_14-55-11.txt" 
						string logFileName = Configuration.EventLogFileName;
						FileInfo fileInfo = new FileInfo(logFileName);			
						string archiveFileName = logFileName.Substring(0,logFileName.Length - fileInfo.Extension.Length) + GetDateString(dt) + "_" + GetTimeString(dt).Replace(":","-") + fileInfo.Extension;
						
						_logWriter.Close();
					
						System.IO.File.Move(logFileName,archiveFileName);					
						_logWriter = new StreamWriter(logFileName,false);
					}

					// 刷新磁盘文件内容
					_logWriter.Flush();
				}					
				
			}
			catch(System.Exception ex)
			{
				string a = ex.Message;
			}//Ignore any exceptions.
		}

		// 返回表示当前日期的字符串，格式为 YYYY-MM-DD
		private static string GetDateString(DateTime date)
		{
			return date.Year.ToString() + "-" + date.Month.ToString().PadLeft(2,'0') + "-" + date.Day.ToString().PadLeft(2,'0');
		}	

		// 返回表示当前时间的字符串，格式为 HH:MM:SS
		private static string GetTimeString(DateTime time)
		{
			return time.Hour.ToString() + ":" + time.Minute.ToString().PadLeft(2,'0') + ":" + time.Second.ToString().PadLeft(2,'0');
		}
		#endregion

		#region 发送日志Email方法

		/// <summary>
		/// 发送日志Email
		/// </summary>
		private static void SendLogEmail()
		{
			try
			{
				//判断是否发送Email
				if(Configuration.EventLogSendEmailSwitch == "on")
				{
					//需要发送邮件
					
					//获得当前日期
					DateTime now = DateTime.Now;
					//获得扫描周期（小时）
					TimeSpan scanCycle = new TimeSpan(Configuration.EventLogScanCycle,0,0);
					//获得发送周期（分钟）
					TimeSpan sendCycle = new TimeSpan(0,Configuration.EventLogSendCycle,0);

					//取得Log文件所在的目录
					FileInfo defaultLogFile = new FileInfo(Configuration.EventLogFileName);
					DirectoryInfo logDir = defaultLogFile.Directory;

					//获得Log目录下的所有Log文件
					FileInfo[] logFiles = logDir.GetFiles();

					//记录最后一次记录日志的时间
					DateTime lastWriteLogTime = DateTime.Now;

					//判断是否带附件
					if(Configuration.EventLogAccessoriesSwitch == "on")
					{
						//需要带附件

						//定义一个StringBuilder，存放信件内容
						StringBuilder content = new StringBuilder();
						//定义一个bool型标志，表示是否至少有一个日志被修改过
						bool isExist = false;

						//组织信件内容（带附件的信件）

						//组织信件的开头部分内容（带附件的信件）
						content.Append("Hi Manager,<br>");
						content.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
						content.Append("系统在运行期间出现了系统故障，记录故障的日志文件已经作为附件发送给您，请您及时查看。<br>");
							
						//组织信件的主体部分（带附件的信件）
						content.Append("");

						//组织信件的结尾部分（带附件的信件）
						content.Append("<br>");

						//把Log目录下的所有需要发送的文件作为附件发送
						foreach(FileInfo file in logFiles)
						{
							//记录最后一次记录日志的时间
							if(lastWriteLogTime > file.LastWriteTime)
								lastWriteLogTime = file.LastWriteTime;
							
							//判断此文件的最后修改日期是否再上一次扫描之后
							if((now - file.LastWriteTime) > scanCycle)
							{
								//在上次扫描之后，此日志文件没有被修改过
							}
							else
							{
								//在上次扫描之后，此日志文件被修改

								//有日志被修改，设标志为True
								isExist = true;

								//将此文件作为附件发送出去
								if(Configuration.EventLogFileName != file.FullName)
									SendEmailWithAccessories(file.FullName,content.ToString());								
								else
								{
									// 当前正在使用的日志文件，由于被系统打开，无法作为附件发送。
									// 所以把当前的日志文件Copy到另一个文件中，以便发送。
									// 因为发送日志文件的周期会比较长，所以每次都复制到同一个文件名的文件中，以减少临时文件个数
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

								//等待发送信件（防止堵塞）
								System.Threading.Thread.Sleep(Configuration.EventLogSendCycle * 60 * 1000);
							}
						}

						//判断是否有日志文件被修改
						if(isExist)
						{
							//有日志文件被修改
						}
						else
						{
							//没有日志文件被修改

							//组织信件内容
							content = new StringBuilder();

							//组织信件的开头部分内容							
							content.Append("Hi Manager,<br>");
							content.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
							content.Append("系统在 " + GetDateString(lastWriteLogTime) + " " + GetTimeString(lastWriteLogTime) + " 后，没有记录新的日志。<br><br>");
							
							//组织信件的主体部分
							content.Append("");

							//组织信件的结尾部分
							content.Append("<br>");

							//发送信件
							SendEmailWithoutAccessories(content.ToString());
						}

					}
					else
					{
						//不需要带附件

						//定义一个StringBuilder，存放信件的主体内容
						System.Text.StringBuilder body = new StringBuilder();
						//定义一个bool型标志，表示是否至少有一个日志被修改过
						bool isExist = false;

						//按照Log目录下的所有需要发送的文件组织信件内容
						foreach(FileInfo file in logFiles)
						{
							if(lastWriteLogTime > file.LastWriteTime)
								lastWriteLogTime = file.LastWriteTime;

							if((now - file.LastWriteTime) > scanCycle)
							{
								//在上次扫描之后，此日志文件没有被修改
							}
							else
							{
								//在上次扫描之后，此日志文件被修改

								//有日志被修改，设标志为True
								isExist = true;

								//将此文件记录添加到信件内容
								string href = Configuration.UrlRoot + @"\Log\" + file.Name;	//生成文件链接
								body.Append("&nbsp:&nbsp:&nbsp:&nbsp:");
								body.Append("<a href=\"" + href + "\" target=\"_blank\">");
								body.Append(file.Name);
								body.Append("</a><br>");
							}
						}

						//定义一个StringBuilder，存放信件内容
						StringBuilder content = new StringBuilder();

						//判断是否有日志文件被修改
						if(isExist)
						{
							//有日志文件被修改

							//组织信件内容

							//组织信件的开头部分内容
							content.Append("Hi Manager,<br>");
							content.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
							content.Append("系统在运行期间出现了系统故障，已经记录日志，请您及时查看。<br>");
							content.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
							content.Append("以下系统日志记录了系统故障，点击查看：<br><br>");
							
							//组织信件的主体部分
							content.Append(body.ToString());

							//组织信件的结尾部分
							content.Append("<br>");
						}
						else
						{
							//没有日志文件被修改

							//组织信件内容

							//组织信件的开头部分内容							
							content.Append("Hi Manager,<br>");
							content.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
							content.Append("系统在 " + GetDateString(lastWriteLogTime) + " " + GetTimeString(lastWriteLogTime) + " 后，没有记录新的日志。<br><br>");
							
							//组织信件的主体部分
							content.Append("");

							//组织信件的结尾部分
							content.Append("<br>");
						}

						//发送信件
						SendEmailWithoutAccessories(content.ToString());
					}
				}
				else
				{
					//不需要发送Email
				}
			}
			catch(System.Exception ex)
			{
				string s = ex.Message;
			}
		}

		/// <summary>
		/// 发送带附件的Email
		/// </summary>
		/// <param name="accessories">附件文件的物理地址</param>
		private static void SendEmailWithAccessories(string accessories,string content)
		{
			System.Web.Mail.MailMessage message = new System.Web.Mail.MailMessage();
			System.Web.Mail.MailAttachment attachment = new System.Web.Mail.MailAttachment(accessories);	//定义一个附件
			message.Attachments.Add(attachment);
			message.BodyFormat = System.Web.Mail.MailFormat.Html;
			message.From = Configuration.EventLogAddresser;
			message.To = Configuration.EventLogAddressee;
			message.Cc = "";
			message.Bcc = "";
			message.Subject = "系统日志定期扫描报告";
			message.Body = content;
			System.Web.Mail.SmtpMail.Send(message);
		}

		/// <summary>
		/// 发送不带附件的Email
		/// </summary>
		/// <param name="content">Email的内容</param>
		private static void SendEmailWithoutAccessories(string content)
		{
			System.Web.Mail.MailMessage message = new System.Web.Mail.MailMessage();
			message.BodyFormat = System.Web.Mail.MailFormat.Html;
			message.From = Configuration.EventLogAddresser;
			message.To = Configuration.EventLogAddressee;
			message.Cc = "";
			message.Bcc = "";
			message.Subject = "系统日志定期扫描报告";
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
