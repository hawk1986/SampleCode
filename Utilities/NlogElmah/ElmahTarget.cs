using NLog;
using NLog.Config;
using NLog.Targets;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Utilities.NlogElmah
{
    [Target("Elmah")]
    public sealed class ElmahTarget : TargetWithLayout
    {
        static ElmahTarget()
        {
            ConfigurationItemFactory.Default.Targets
                    .RegisterDefinition("Elmah", typeof(ElmahTarget));
        }

        public string ConnectionStringName { get; set; }

        /// <summary>
        /// 將 NLog 寫入 Elmah 的 DB(SQLite) 
        /// </summary>
        /// <param name="logEvent"></param>
        protected override void Write(LogEventInfo logEvent)
        {
            var connectionString =
                ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                var application = System.Web.Hosting.HostingEnvironment.ApplicationID;
                var time = logEvent.TimeStamp.ToUniversalTime();
                var eventData = from p in logEvent.GetType().GetProperties()
                                where p.PropertyType.IsValueType || p.PropertyType.IsEnum
                                select new XElement("item",
                                                    new XAttribute("name", p.Name),
                                                    new XElement("value",
                                                        new XAttribute("string",
                                                            p.GetValue(logEvent, null).ToString())));
                var variables = from k in HttpContext.Current.Request.ServerVariables.AllKeys
                                let v = HttpContext.Current.Request.ServerVariables[k]
                                select new XElement("item",
                                                    new XAttribute("name", k),
                                                    new XElement("value",
                                                                    new XAttribute("string", v)));
                var xdoc = new XElement("error",
                                        new XAttribute("host", System.Environment.MachineName),
                                        new XAttribute("type", logEvent.Exception == null ?
                                                                "" : logEvent.Exception.ToString()),
                                        new XAttribute("message", logEvent.FormattedMessage),
                                        new XAttribute("source", logEvent.LoggerName),
                                        new XAttribute("detail", logEvent.StackTrace == null ?
                                                                "" : logEvent.StackTrace.ToString()),
                                        new XAttribute("time", time.ToString("s") + "Z"),
                                        new XElement("eventData", eventData),
                                        new XElement("serverVariables", variables));
                var user = string.Empty;
                if (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    user = HttpContext.Current.User.Identity.Name;
                }

                var SQL = @"insert into [error](Application,Host,Type,Source,Message,User,AllXml,StatusCode,TimeUtc) values(@Application,@Host,@Type,@Source,@Message,@User,@AllXml,@StatusCode,@TimeUtc)";
                var cmd = new SQLiteCommand(SQL, connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@Application", DbType.String).Value = application;
                cmd.Parameters.Add("@Host", DbType.String).Value = System.Environment.MachineName;
                cmd.Parameters.Add("@Type", DbType.String).Value = "NLog-" + logEvent.Level.Name;
                cmd.Parameters.Add("@Source", DbType.String).Value = logEvent.LoggerName;
                cmd.Parameters.Add("@Message", DbType.String).Value = logEvent.FormattedMessage;
                cmd.Parameters.Add("@User", DbType.String).Value = user;
                cmd.Parameters.Add("@AllXml", DbType.String).Value = xdoc.ToString();
                cmd.Parameters.Add("@StatusCode", DbType.Int32).Value = 0;
                cmd.Parameters.Add("@TimeUtc", DbType.DateTime).Value = time;

                cmd.ExecuteNonQuery();
            }
        }
    }
}
