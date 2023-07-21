using Microsoft.AspNetCore.Components;

namespace BlazorLearning.WebClient.Events
{

    [EventHandler("oncustomevent", typeof(CustomEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
    [EventHandler("oncustomdblclick", typeof(CustomDblClickEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
    public static partial class EventHandlers
    {
    }

    public class CustomEventArgs : EventArgs
    {
        public string ElementHtml { get; set; }
        public string Author { get; set; }
    }

    public class CustomDblClickEventArgs : EventArgs
    {
        public string ElementHtml { get; set; }
        public string Author { get; set; }
    }
}
