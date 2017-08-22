using System;
using Windows.UI.Notifications;
using Windows.Storage;
using static StegoPlusPlus.Controls.Process;
using Microsoft.Toolkit.Uwp.Notifications;

namespace StegoPlusPlus.Controls
{
    class ToastDialog
    {
        public static void Show(StorageFile file, string typeToast, string processOf1, string processOf2)
        {
            string header = String.Format("{0} was saved", processOf1);
            string title = String.Format("File {0} was saved successfully !", file.Name);

            ToastNotificationManager.History.Clear();
            PassData(file, processOf1);

            var Content = new ToastContent()
            {
                ActivationType = ToastActivationType.Background,
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = header
                            },
                            new AdaptiveText()
                            {
                                Text = title
                            }
                        },
                        Attribution = new ToastGenericAttributionText()
                        {
                            Text = processOf2
                        },
                    }
                },
                Actions = new ToastActionsCustom()
                {
                    Buttons =
                    {
                        new ToastButton("Open File", "Open")
                        {
                            ActivationType = ToastActivationType.Foreground
                        },
                        new ToastButton("Dismiss", "Dismiss")
                        {
                            ActivationType = ToastActivationType.Background
                        }
                    }
                },
            };

            var toastNotif = new ToastNotification(Content.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }

        public static void PassData(StorageFile file, string type)
        {
            if (GetData.ToastData.ContainsKey(Data.Misc.ToastData) == true) GetData.ToastData.Remove(Data.Misc.ToastData);
            GetData.ToastData.Add(Data.Misc.ToastData, file);
        }

    }
}
