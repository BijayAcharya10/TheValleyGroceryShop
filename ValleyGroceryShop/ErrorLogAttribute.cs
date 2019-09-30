using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ValleyGroceryShop
{
    public class ErrorLogAttribute:HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            LogError(filterContext);
            base.OnException(filterContext);
        }

        public void LogError(ExceptionContext filterContext)
        {
            // You could use any logging approach here

            StringBuilder builder = new StringBuilder();
            builder
                .AppendLine("----------")
                .AppendLine(DateTime.Now.ToString())
                .AppendFormat("Source:\t{0}", filterContext.Exception.Source)
                .AppendLine()
                .AppendFormat("Target:\t{0}", filterContext.Exception.TargetSite)
                .AppendLine()
                .AppendFormat("Type:\t{0}", filterContext.Exception.GetType().Name)
                .AppendLine()
                .AppendFormat("Message:\t{0}", filterContext.Exception.Message)
                .AppendLine()
                .AppendFormat("Stack:\t{0}", filterContext.Exception.StackTrace)
                .AppendLine();

            string filePath = filterContext.HttpContext.Server.MapPath("~/App_Data/Log.txt");

            using (StreamWriter writer = File.AppendText(filePath))
            {
                writer.Write(builder.ToString());
                writer.Flush();
            }
        }

    }
}