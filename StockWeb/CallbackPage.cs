using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Bluematrix.Web.Utilities
{
	// This interface must be implemented by any control or page that needs postback.
	public interface ICallbackEventHandler
	{

		string RaiseClientCallbackEvent(string eventArgument);

	}

	/// <summary>
	/// CallbackPage 的摘要说明。
	/// </summary>
	public class CallbackPage : System.Web.UI.Page
	{
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			// __CALLBACKID will be in the Request dictionary with script callback.
			if (Request["__CALLBACKID"] != null) 
			{
				_isCallback = true;
				handleCallback();
			}
		}

		private void handleCallback() 
		{
			// Get the UniqueID of the control.
			// I have not tested this with controls put in INamingContainer.
			// It may not work properly.
			Control _control;
			string controlUniqueID = Request["__CALLBACKID"];

			if (controlUniqueID == "__PAGE") 
			{
				_control = this;
			} 
			else 
			{
				_control = FindControl(controlUniqueID);
			}

			// Get the reference to the interface.
			ICallbackEventHandler callbackHandler = _control as ICallbackEventHandler;
			if (callbackHandler != null) 
			{
				Response.Clear();
				string result;

				try 
				{
					// Fire the callback handler method.
					result = callbackHandler.RaiseClientCallbackEvent(Request["____CALLBACKPARAM"]);

					// 's' for success.This will be further processed at client side.
					Response.Write('s');
				} 
				catch (Exception ex) 
				{
					// 'e' for Exception.
					Response.Write('e');
					result = ex.Message;
				}

				// Write back to client.
				Response.Write(result);

				// Avoid any caching at client side.
				Response.Cache.SetExpires(DateTime.Now);

				Response.End();
			}
		}


		// Is client script callback?
		private bool _isCallback = false;

		public bool IsCallback 
		{
			get {return _isCallback;}
		}

		// JavaScript file contains the client callback functions
		private const string CALLBACKSCRIPT_KEY   = "Callback.js";

		// Get the client side callback JavaScript function name.
		// Then we can attach the function to any client side events.
		public string GetCallbackEventReference(
			Control _control, // Target control that handles callback at server side.
			string argument, // String argument.
			string clientCallback, // Client side callback function to process the result.
			// Usually this function operates DOM.
			string context, // Optional context.
			string clientErrorCallback, // Optional client side error handler.
			bool useAsync // Sync or Async ?
			) 
		{
			string target;

			if (_control is ICallbackEventHandler) 
			{
				if (_control is Page) 
				{
					target = "__PAGE"; // Page needs special handling.
				} 
				else 
				{
					target = _control.UniqueID;
				}
			} 
			else 
			{
				throw new ArgumentException("The control must implement ICallbackEventHandler interface.");
			}

			if ((clientCallback == null) || (clientCallback == string.Empty))
			{
				throw new ArgumentException("The clientCallback argument cannot be null or empty");
			}

			if (((Request != null)) && Request.Browser.JavaScript)
			{
				// Register the callback initialization scripts.
				if (!IsClientScriptBlockRegistered("CallbackInitializationScript")) 
				{
					RegisterClientScriptBlock("CallbackInitializationScript",

						String.Format("<script language=JavaScript src=\"{0}/javascript/{1}\" type=\"text/javascript\"></script>", Request.ApplicationPath, CALLBACKSCRIPT_KEY));
				}

				if (!IsStartupScriptRegistered("PageCallbackScript"))
				{
					RegisterStartupScript("PageCallbackScript", "<script language=JavaScript>var pageUrl='" + Request.Url.PathAndQuery + "';\r\n    WebForm_InitCallback();</script>");
				}
			} 
			else 
			{
				throw new NotSupportedException("Browser does not support JavaScript?");
			}

			if (argument == null)
			{
				argument = "null";
			}
			else if (argument.Length == 0)
			{
				argument = "\"\"";
			}

			if (context == null)
			{
				context = "null";
			}
			else if (context.Length == 0)
			{
				context = "\"\"";
			}

			// Constructor the client side callback function reference.
			string[] textArray1 = new string[13] { "WebForm_DoCallback('", target, "',", argument, ",", clientCallback, ",", context, ",", (clientErrorCallback == null) ? "null" : clientErrorCallback, ",", useAsync ? "true" : "false", ")" } ;

			return string.Concat(textArray1);
		}

	}
}
