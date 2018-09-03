using Acr.UserDialogs;
using DentalAssistantXF.Models;
using System.Text.RegularExpressions;

namespace DentalAssistantXF.Utils
{
    public static class HelperFunctions
    {
        public static void ShowToastMessage(ToastMessageType type, string message)
        {
            var icon = string.Empty;
            System.Drawing.Color color = System.Drawing.Color.FromArgb(0, 188, 212);

            switch (type)
            {
                case ToastMessageType.Success:
                    icon = "ic_check_white_24dp.png";
                    break;
                case ToastMessageType.Error:
                    icon = "ic_error.png";
                    color = System.Drawing.Color.Red;
                    break;
            }

            var toastConfig = new ToastConfig(message);
            toastConfig.SetDuration(2000);
            toastConfig.SetBackgroundColor(color);
            toastConfig.SetIcon(icon);
            UserDialogs.Instance.Toast(toastConfig);
        }

        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }
    }
}
