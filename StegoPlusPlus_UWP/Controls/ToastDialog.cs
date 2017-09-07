using System;
using Windows.UI.Notifications;
using Windows.Storage;
using static StegoPlusPlus.Controls.Process;
using Microsoft.Toolkit.Uwp.Notifications;

namespace StegoPlusPlus.Controls
{
    class ToastDialog
    {
        #region Notify Result
        public static void ShowData(StorageFile file, string typeToast, string processOf1, string processOf2)
        {
            string header = String.Format("{0} was saved", processOf1);
            string title = String.Format("File {0} was saved successfully !", file.Name);

            ToastNotificationManager.History.RemoveGroup(Data.Misc.ToastGroupA);
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

            var toastNotify = new ToastNotification(Content.GetXml())
            {
                Group = Data.Misc.ToastGroupA
            };

            if (Process.Notify.GetStatus("Result") == true) ToastNotificationManager.CreateToastNotifier().Show(toastNotify); else ToastNotificationManager.History.RemoveGroup(Data.Misc.ToastGroupA);

        }
        #endregion
        #region Notify Data Passing
        public static void PassData(StorageFile file, string type)
        {
            if (GetData.ToastData.ContainsKey(Data.Misc.ToastData) == true) GetData.ToastData.Remove(Data.Misc.ToastData);
            GetData.ToastData.Add(Data.Misc.ToastData, file);
        }
        #endregion
        #region Notify Timer
        public static void Notify(string value, string type)
        {
            string header = String.Format("{0} data was successfully", type);
            string title = String.Format("{0} was complete on {1}ms !", type, value);

            //FIX Ukuran TAG yang kena limit di versi 10240 - 14393
            if (type == Data.Misc.T_ConvertBinary) ToastNotificationManager.History.Remove("Converting", Data.Misc.ToastGroupB);
            //END FIX

            ToastNotificationManager.History.Remove(type, Data.Misc.ToastGroupB);

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
                            Text = type
                        },
                    }
                }
            };

            //FIX Ukuran TAG yang kena limit di versi 10240 - 14393
            if (type == Data.Misc.T_ConvertBinary) type = "Converting";
            //END FIX

            var toastNotify = new ToastNotification(Content.GetXml())
            {
                Group = Data.Misc.ToastGroupB,
                Tag = type
            };

            if (Process.Notify.GetStatus("Timer") == true) ToastNotificationManager.CreateToastNotifier().Show(toastNotify); else ToastNotificationManager.History.RemoveGroup(Data.Misc.ToastGroupB);

        }
        #endregion
    }
}
