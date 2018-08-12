using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush
{
    public static class HtmlHelpers
    {

        public static string IsSelected(this IHtmlHelper html, string controller = null, string action = null, string cssClass = null)
        {
            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }

        public static string PageClass(this IHtmlHelper htmlHelper)
        {
            string currentAction = (string)htmlHelper.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

    }
}
