using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications; // Notifications library
using Microsoft.QueryStringDotNET; // QueryString.NET
using Windows.Storage;
using static StegoPlusPlus.Controls.Process;

namespace StegoPlusPlus.Controls
{
    class ToastDialog
    {
        public static void Show(StorageFile file, string typeToast, string processOf1, string processOf2)
        {
            string title = String.Format("{0} was saved", processOf1);
            string content = String.Format("File {0} was saved successfully !", file.Name);
            string logo = "ms-appx:///Assets/MyLogo.png";
            string Visual;
            string Act;

            ToastNotificationManager.History.Clear();
            PassData(file, processOf1);

            if (typeToast == "Image Files")
            {
                string image = file.Path;
                Visual = $@"<visual><binding template='ToastGeneric'><text>{title}</text><text>{content}</text><text placement='attribution'>{processOf2}</text><image placement='inline' src='{image}'/><image placement='appLogoOverride' src='{logo}' hint-crop='rectangle'/></binding></visual>";
                Act = $@"<actions><action content='Open Image' imageUri='ms-appx:///Assets/Button/Picture.png' activationType='foreground' arguments='Open'/><action content='Dismiss' imageUri='ms-appx:///Assets/Button/Dismiss.png' activationType='background' arguments='dismiss'/></actions>";
            }
            else
            {
                Visual = $@"<visual><binding template='ToastGeneric'><text>{title}</text><text>{content}</text><text placement='attribution'>{processOf2}</text><image placement='appLogoOverride' src='{logo}' hint-crop='rectangle'/></binding></visual>";
                Act = $@"<actions><action content='Open File' imageUri='ms-appx:///Assets/Button/File.png' activationType='foreground' arguments='Open'/><action content='Dismiss' imageUri='ms-appx:///Assets/Button/Dismiss.png' activationType='background' arguments='dismiss'/></actions>";
            }
            string toastXmlString = $@"<toast activationType='background'>{Visual}{Act}</toast>";

            XmlDocument toastXml = new XmlDocument();
            toastXml.LoadXml(toastXmlString);

            var toast = new ToastNotification(toastXml);            
            var notification = ToastNotificationManager.CreateToastNotifier();
            notification.Show(toast);           
        }

        public static void PassData(StorageFile file, string type)
        {
            if (GetData.ToastData.ContainsKey(Data.Misc.ToastData) == true) GetData.ToastData.Remove(Data.Misc.ToastData);
            GetData.ToastData.Add(Data.Misc.ToastData, file);
        }

    }
}
