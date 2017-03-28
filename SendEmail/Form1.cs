using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AISRS.Common.Function;

namespace SendEmail
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //smtp.163.com
                //smtp.gmail.com
                //smtp.qq.com
                string senderServerIp = "smtp.qq.com";
                //string senderServerIp = "smtp.sina.com";
                string toMailAddress = "476707501@qq.com";
                string fromMailAddress = "476707501@qq.com";
                string subjectInfo = "Test sending e_mail";
                string bodyInfo = "Hello chencheng, This is my first testing e_mail";
                string mailPort = "25";
                //string attachPath = "E:\\123123.txt; E:\\haha.pdf";

                EmailUtil email = new EmailUtil(senderServerIp, toMailAddress, fromMailAddress, subjectInfo, bodyInfo, mailPort, false, false);
                //email.AddAttachments(attachPath);
                email.Send();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
