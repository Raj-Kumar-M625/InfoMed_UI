using Newtonsoft.Json;

namespace InfoMed.Services.Implementation
{
    public static class PopUpService
    {
        public static string Notify(string title, string text, string icon, bool showCancel = false)
        {
            var message = new
            {
                Title = title,
                Text = text,
                Icon = icon,
                ShowCancelButton = showCancel
            };
            return JsonConvert.SerializeObject(message);
        }
    }
}
