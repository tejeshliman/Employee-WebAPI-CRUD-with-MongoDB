using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Company.Services.EmployeePortal.Website.Models
{
    public enum MessageSeverity
    {
        None,
        Info,
        Success,
        Warning,
        Danger
    }
    public interface INotifier
    {
        IList<Message> Messages { get; }
        void AddMessage(MessageSeverity severity, string text, params object[] format);
    }

    public class Notifier : INotifier
    {
        public IList<Message> Messages { get; private set; }

        public Notifier()
        {
            Messages = new List<Message>();
        }

        public void AddMessage(MessageSeverity severity, string text, params object[] format)
        {
            Messages.Add(new Message { Severity = severity, Text = string.Format(text, format) });
        }
    }
    public class Message
    {
        public MessageSeverity Severity { get; set; }

        public string Text { get; set; }

        public string Generate()
        {
            var isDismissable = Severity != MessageSeverity.Danger;
            if (Severity == MessageSeverity.None) Severity = MessageSeverity.Info;
            var sb = new StringBuilder();
            var divTag = new TagBuilder("div");
            divTag.AddCssClass("alert");
            divTag.AddCssClass("alert-" + Severity.ToString().ToLower());


            var spanTag = new TagBuilder("span");
            spanTag.MergeAttribute("id", "MessageContent");

            if (isDismissable)
            {
                divTag.AddCssClass("alert-dismissable");
            }

            sb.Append(divTag.ToString(TagRenderMode.StartTag));

            if (isDismissable)
            {
                var buttonTag = new TagBuilder("button");
                buttonTag.MergeAttribute("class", "close");
                buttonTag.MergeAttribute("data-dismiss", "alert");
                buttonTag.MergeAttribute("aria-hidden", "true");
                buttonTag.InnerHtml = "×";
                sb.Append(buttonTag.ToString(TagRenderMode.Normal));
            }

            sb.Append(spanTag.ToString(TagRenderMode.StartTag));
            sb.Append(Text);
            sb.Append(spanTag.ToString(TagRenderMode.EndTag));
            sb.Append(divTag.ToString(TagRenderMode.EndTag));

            return sb.ToString();
        }
    }


    public static class Constants
    {
        public const string TempDataKey = "Messages";
    }

    public class NotifierFilterAttribute : ActionFilterAttribute
    {
        public INotifier Notifier { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var messages = Notifier.Messages;
            if (messages.Any())
            {
                filterContext.Controller.TempData[Constants.TempDataKey] = messages;
            }
        }
    }

    public static class NotifierExtensions
    {
        public static void Error(this INotifier notifier, string text, params object[] format)
        {
            notifier.AddMessage(MessageSeverity.Danger, text, format);
        }

        public static void Info(this INotifier notifier, string text, params object[] format)
        {
            notifier.AddMessage(MessageSeverity.Info, text, format);
        }

        public static void Success(this INotifier notifier, string text, params object[] format)
        {
            notifier.AddMessage(MessageSeverity.Success, text, format);
        }

        public static void Warning(this INotifier notifier, string text, params object[] format)
        {
            notifier.AddMessage(MessageSeverity.Warning, text, format);
        }

        public static MvcHtmlString DisplayMessages(this ViewContext context)
        {
            if (!context.Controller.TempData.ContainsKey(Constants.TempDataKey))
            {
                return null;
            }

            var messages = (IEnumerable<Message>)context.Controller.TempData[Constants.TempDataKey];
            var builder = new StringBuilder();
            foreach (var message in messages)
            {
                builder.AppendLine(message.Generate());
            }

            return builder.ToHtmlString();
        }

        private static MvcHtmlString ToHtmlString(this StringBuilder input)
        {
            return MvcHtmlString.Create(input.ToString());
        }
    }

}