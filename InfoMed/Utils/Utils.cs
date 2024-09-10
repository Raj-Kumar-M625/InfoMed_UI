using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace InfoMed.Utils
{
    public static class Utils
    {
        public static string RenderRazorViewToString(Controller controller, string viewName, object model = null)
        {
            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                try
                {
                    IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                    ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

                    ViewContext viewContext = new ViewContext(
                        controller.ControllerContext,
                        viewResult.View,
                        controller.ViewData,
                        controller.TempData,
                        sw,
                        new HtmlHelperOptions()
                    );

                    viewResult.View.RenderAsync(viewContext);
                    return sw.GetStringBuilder().ToString();
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static string FormatDateRange(DateTime? startDate, DateTime? endDate)
        {
            string formattedStartDate = FormatDate(startDate);
            string formattedEndDate = FormatDate(endDate);

            string dayNames = $"{startDate:dddd} and {endDate:dddd}";

            if (startDate.Value.Date == endDate.Value.Date)
            {
                return $"{formattedStartDate}-{startDate:MMM-yyyy}";
            }

            return $"{formattedStartDate}-{startDate:MMM-yyyy} to {formattedEndDate}-{startDate:MMM-yyyy}";
        }

        public static string FormatDate(DateTime? date)
        {
            int day = date!.Value.Day;
            string ordinalSuffix = GetOrdinalSuffix(day);
            string formattedDate = $"{day}";
            return formattedDate;
        }

        public static string GetOrdinalSuffix(int day)
        {
            if (day % 100 >= 11 && day % 100 <= 13)
            {
                return "th";
            }

            switch (day % 10)
            {
                case 1: return "st";
                case 2: return "nd";
                case 3: return "rd";
                default: return "th";
            }
        }

        public static string GetSpecificDate(DateTime inputDate)
        {
            string formattedDate = inputDate.ToString("dd MMMM yyyy");
            return formattedDate;
        }

        public static string RemoveSpecialCharacters(string input)
        {
            string pattern = @"[^a-zA-Z]";
            string result = Regex.Replace(input, pattern, "");
            return result;
        }
    }
}
