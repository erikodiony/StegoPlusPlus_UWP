﻿using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using StegoPlusPlus.Views.Popup;
using static StegoPlusPlus.Controls.Data.Prop_Popup;

namespace StegoPlusPlus.Controls
{
    class PopupDialog
    {
        public static async Task Show(string status, string title, string msg, string ico)
        {
            Notification cbox = new Notification()
            {
                Title = String.Format("{0} | {1}", status, title),
                PrimaryButtonText = Data.Prop_Button.OK,
                Detail = msg,
                Icon = ico
            };
            await cbox.ShowAsync();
        }
        public static async Task<bool> ShowConfirm(string status, string title, string msg, string ico)
        {
            Notification cbox = new Notification()
            {
                Title = String.Format("{0} | {1}", status, title),
                PrimaryButtonText = Data.Prop_Button.OK,
                SecondaryButtonText = Data.Prop_Button.Cancel,
                Detail = msg,
                Icon = ico
            };
            bool value = (await cbox.ShowAsync() == ContentDialogResult.Primary) ? true : false;
            return value;
        }
        public class Loading
        {
            Progress pg = new Progress();
            public async void Show(bool type, string msg, string detail)
            {
                pg.Message = msg;
                pg.Detail = detail;
                if (type == true) await pg.ShowAsync(); else pg.Hide();
            }
        }
        public class Message
        {
            public async void Show()
            {
                Secret_Message sm = new Secret_Message()
                {
                    Title = String.Format("{0} | {1}", Title.Status.Result, "Secret Text/Message")
                };                
                await sm.ShowAsync();
            }
        }
        public class Info
        {
            public async void Show()
            {
                Secret_Passwd sp = new Secret_Passwd()
                {
                    Title = String.Format("{0} | {1}", Title.Status.Result, "Check Stego Info")
                };
                await sp.ShowAsync();
            }
        }
    }
}
