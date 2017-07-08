using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using StegoPlusPlus.Views.Popup;
using static StegoPlusPlus.Data;

namespace StegoPlusPlus
{
    class PopupDialog
    {
        public static async void Show(string status, string title, string msg, string ico)
        {
            Button_Single cbox = new Button_Single()
            {
                Title = String.Format("{0} | {1}", status, title),
                PrimaryButtonText = Prop_Button.OK,
                Detail = msg,
                Icon = ico
            };
            ContentDialogResult result = await cbox.ShowAsync();
        }
    }
}
