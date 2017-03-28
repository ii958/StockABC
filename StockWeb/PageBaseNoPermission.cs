using System;
using System.Reflection;
using System.Text;
using System.IO;
using AISRS.BusinessFacade;
using AISRS.Common.Exception;
using AISRS.Common.Framework;
using System.Xml;
using Bluematrix.Web.Utilities;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Diagnostics;

namespace AISRS.WebUI
{
    /// <summary>
    /// PageBaseNoPermission是所有网页（aspx页面）的基类
    /// </summary>
    public class PageBaseNoPermission : CallbackPage
    {
        #region Fields
        // 在Session中，保存User对象的键 
        public const string KEY_LOGINUSER = "Session:LoginUser";

        // 利用Session传递ErrorMessage，在Session中保存ErrorMessage的键
        public const string KEY_ERRORMESSAGE = "Session:ErrorMessage";

        // 保存页面的权限
        private string[] _permissions;

        // 用来设置界面显示时焦点所在的控件
        private string _focusedControl;
        private const string SetFocusFunctionName = "__setFocus";
        private const string SetFocusScriptName = "__inputFocusHandler";

        #endregion

        #region 属性
        public AISRS.Common.Framework.User LoginUser
        {
            get { return this.Session[KEY_LOGINUSER] as AISRS.Common.Framework.User; }
        }
        #endregion

        #region 构造析构函数
        public PageBaseNoPermission()
        {
            // 设置错误页
            this.ErrorPage = Configuration.UrlRoot + Configuration.ErrorPagePath;

            // 获取页面权限
            //this.LoadPermission();

            // 初始化焦点控件ID
            _focusedControl = string.Empty;

            // 挂接FromLoad处理时间，在其中进行登录和权限验证
            this.Load += new EventHandler(this.Page_Load);
        }
        #endregion

        /// <summary>
        /// 检查用户是否登录和用户是否有权访问页面
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Load(object sender, EventArgs e)
        {

            // 检查是否系统正在升级，如果是，转到升级信息提示页
            if (Configuration.IsOnUpdating)
                this.Response.Redirect(Configuration.UrlRoot + "/" + Configuration.UpdateMessagePageUrl, true);

            //检查访问的域名是否和设置的rootUrl一致			
            //			string requestUrlRoot = "http://" + Context.Request.Url.Host;
            //			if(Context.Request.Url.Port.ToString() != "80")
            //				requestUrlRoot += ":"  + Context.Request.Url.Port.ToString();
            //			if(Context.Request.ApplicationPath != "/")
            //				requestUrlRoot += Context.Request.ApplicationPath;
            //			if(this.Page.Request.Url.AbsoluteUri.ToString() != requestUrlRoot +  Configuration.ErrorPagePath)
            //			{
            //				if(requestUrlRoot.ToLower() != Configuration.UrlRoot.ToLower())
            //				{
            //					this.Session[KEY_ERRORMESSAGE] = "应该用如下Url访问本站点：<br>" + Configuration.UrlRoot + "<br>"
            //						+ "而您用的是：<br>" + requestUrlRoot;
            //					this.Response.Redirect(requestUrlRoot + Configuration.ErrorPagePath);
            //				}
            //			}

            //获取当前登录用的NT帐号（去掉域名部分）
            //string ntAccount = this.GetNTAccount();

            //if (Configuration.SystemNtAccount.ToLower() != string.Empty)
            //{
            //    ntAccount = Configuration.SystemNtAccount.ToLower(); //测试时用
            //}

            //Common.Framework.Log.WriteInfo(ntAccount);

            //if (ntAccount != String.Empty)
            //{
            //    //获取用户所拥有的权限并保存到Session中
            //    //如果Session存在则直接读取Session中的权限信息
            //    if (this.LoginUser == null)
            //    {
            //        //登录
            //        AISRS.Common.Framework.User user;
            //        AccessControlSystem userSystem = new AccessControlSystem();

            //        userSystem.Login(ntAccount, out user);

            //        Common.Framework.Log.WriteInfo(user == null ? "null" : user.FullName);

            //        if (user != null)
            //        {
            //            this.Session[KEY_LOGINUSER] = user;
            //        }

            //        else
            //        {
            //            this.Session[KEY_ERRORMESSAGE] = PermissionException.Default;
            //            this.Response.Redirect(this.ErrorPage);
            //        }

            //    }

            //    //检验用户是否有权限访问此页面，如果没有转到错误页面中提示用户没有权限
            //    if (!IsPermitLoginUser())
            //    {
            //        this.Session[KEY_ERRORMESSAGE] = PermissionException.Default;
            //        this.Response.Redirect(this.ErrorPage);
            //    }

            //}
            //else
            //{
            //    //TODO:当无NT帐号时，显示相关QA的链接
            //    this.Session[KEY_ERRORMESSAGE] = "对不起，您没有NT帐号或者帐号无效，不能登录本系统!";
            //    this.Response.Redirect(this.ErrorPage);
            //    //				throw new CommonException();
            //}

        }


        /// <summary>
        /// 获取登录用户的NT帐号（去掉域名部分）
        /// </summary>
        /// <returns></returns>
        private string GetNTAccount()
        {
            string ntAccount = String.Empty;

            string[] userNameArray = this.User.Identity.Name.Split('\\');

            int length = userNameArray.Length;

            if (length > 0)
            {
                ntAccount = userNameArray[length - 1];
            }

            return ntAccount.ToLower();

        }

        /// <summary>
        /// 检查用户是否有访问页面的权限
        /// </summary>
        private bool IsPermitLoginUser()
        {
            //给未登录的用户Guest的角色


            // 如果页面没有限定权限，任何用户都可以访问
            if (this._permissions == null || this._permissions.Length == 0)
                return true;

            // 用户还没注册时，不能访问受权限控制的页面
            if (LoginUser == null)
                return false;

            bool isDefault = false;
            bool isHasPermission = false;

            string defaultPermission = "项目进度维护及查询.项目进度报告.读取";


            isHasPermission = LoginUser.IsHaveAnyoneOfPermissions(this._permissions);

            foreach (string permission in _permissions)
            {
                if (permission == defaultPermission)
                {
                    isDefault = true;
                    break;
                }
            }

            if (isHasPermission == false && isDefault == true)
            {
                string firstUrl = GetFirstCanAccessUrl(LoginUser);
                if (firstUrl != string.Empty)
                {
                    this.Response.Redirect(Configuration.UrlRoot + firstUrl);
                }
            }

            return isHasPermission;
        }


        /// <summary>
        /// 获得用户能够访问的第一个页面的url
        /// </summary>
        /// <param name="user">登录用户</param>
        /// <returns></returns>
        private string GetFirstCanAccessUrl(AISRS.Common.Framework.User user)
        {
            //获取导航栏及权限的XmlDocument。

            string KEY_PERMISSIONXML = "Key_NavigationPermissionXml";
            XmlDocument xmlDocument = this.Cache.Get(KEY_PERMISSIONXML) as XmlDocument;
            if (xmlDocument == null)
            {
                xmlDocument = AISRS.WebUI.Modules.NavigationPermission.XmlDocument;
                this.Cache.Insert(KEY_PERMISSIONXML, xmlDocument, AISRS.WebUI.Modules.NavigationPermission.XmlDocumentCacheDependencies);
            }

            XmlNode rootNode = xmlDocument.DocumentElement;

            XmlNodeList categoryList = rootNode.ChildNodes;

            foreach (XmlNode categoryNode in categoryList)
            {
                XmlNodeList itemList = categoryNode.ChildNodes;

                foreach (XmlNode itemNode in itemList)
                {

                    XmlNodeList pageList = itemNode.ChildNodes;

                    #region
                    //判断是否对page有操作的权限
                    //如果有权限生成导航项

                    foreach (XmlNode pageNode in pageList)
                    {

                        XmlNodeList permissionList = pageNode.ChildNodes;

                        if (permissionList.Count == 0)
                        {
                            return pageNode.Attributes["url"].Value;
                        }
                        else
                        {
                            foreach (XmlNode permissionNode in permissionList)
                            {
                                string permission = permissionNode.InnerText;

                                if (user.PermissionTable.ContainsKey(permission)
                                    || user.PermissionTable.ContainsValue(permission))
                                {
                                    return pageNode.Attributes["url"].Value;
                                }

                            }//end  permissionNode
                        }

                    }//end pageNode
                    #endregion

                }//end itemNode


            }//end categoryNode

            return String.Empty;
        }


        /// <summary>
        /// 取页面的所有访问权限
        /// </summary>
        private void LoadPermission()
        {
            //通过返射获取此页面的权限属性			
            MemberInfo memberInfo = this.GetType();
            PermissionAttribute permissionAttribute = (PermissionAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(PermissionAttribute));
            if (permissionAttribute != null && permissionAttribute.Permissions.Length != 0)
            {
                this._permissions = permissionAttribute.Permissions;
            }
            else
            {
                this._permissions = null;
            }
        }

        /// <summary>
        /// override onError method catch all the exceptions
        /// </summary>
        /// <param name="e"></param>
        protected override void OnError(EventArgs e)
        {
            Exception exception = Server.GetLastError();
            ExceptionHandler.WriteLogWithPageInformation(exception, this);
            this.Session[KEY_ERRORMESSAGE] = ExceptionHandler.GetFriendlyMessage(exception);

            //todo: 
            base.OnError(e);

#if(!DEBUG)
			this.Response.Redirect(this.ErrorPage);
#endif
        }

        /// <summary>
        /// Render 前必要的处理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            // TODO:  添加 PageBase.OnPreRender 实现
            AddSetFocusScript();
            base.OnPreRender(e);
        }

        /// <summary>
        /// 添加控制默认焦点的Javascript
        /// </summary>
        private void AddSetFocusScript()
        {
            if (_focusedControl == "")
                return;

            // Add the script to declare the function
            // (Only one form in ASP.NET pages)
            StringBuilder sb = new StringBuilder("");
            sb.Append("<script language=javascript>");
            sb.Append("function ");
            sb.Append(SetFocusFunctionName);
            sb.Append("(ctl) {");
            sb.Append("  if (document.forms[0][ctl] != null)");
            sb.Append("  {document.forms[0][ctl].focus();}");
            sb.Append("}");

            // Add the script to call the function
            sb.Append(SetFocusFunctionName);
            sb.Append("('");
            sb.Append(_focusedControl);
            sb.Append("');<");
            sb.Append("/");   // break like this to avoid misunderstandings...
            sb.Append("script>");

            // Register the script (names are CASE-SENSITIVE)
            if (!IsStartupScriptRegistered(SetFocusScriptName))
                RegisterStartupScript(SetFocusScriptName, sb.ToString());
        }

        /// <summary>
        /// 设置打开页面默认的焦点
        /// </summary>
        /// <param name="ctlId"></param>
        public void SetFocus(string ctlId)
        {
            _focusedControl = ctlId;
        }
    }
}
