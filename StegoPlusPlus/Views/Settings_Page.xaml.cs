using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StegoPlusPlus.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings_Page : Page
    {        
        public Settings_Page()
        {
            this.InitializeComponent();
            check_transition_effect_status();
            check_toggle_status();
            InitializingPage();
        }

        //Trigger Toggled
        private void Toggle_BG_Toggled(object sender, RoutedEventArgs e)
        {
            change_toggle();
        }

        //Function Check Effect Transition
        TransitionCollection collection = new TransitionCollection();
        NavigationThemeTransition theme = new NavigationThemeTransition();

        private void check_transition_effect_status()
        {
            if ((string)ApplicationData.Current.LocalSettings.Values["Effect_set"] == "Continuum")
            {
                var info = new ContinuumNavigationTransitionInfo();
                theme.DefaultNavigationTransitionInfo = info;
                collection.Add(theme);
                this.Transitions = collection;
                cb_effect.SelectedValue = "Effect 1";
            }

            else if ((string)ApplicationData.Current.LocalSettings.Values["Effect_set"] == "Common")
            {
                var info = new CommonNavigationTransitionInfo();
                theme.DefaultNavigationTransitionInfo = info;
                collection.Add(theme);
                this.Transitions = collection;
                cb_effect.SelectedValue = "Effect 2";
            }

            else if ((string)ApplicationData.Current.LocalSettings.Values["Effect_set"] == "Slide")
            {
                var info = new SlideNavigationTransitionInfo();
                theme.DefaultNavigationTransitionInfo = info;
                collection.Add(theme);
                this.Transitions = collection;
                cb_effect.SelectedValue = "Effect 3";
            }

            else
            {
                var info = new SuppressNavigationTransitionInfo();
                theme.DefaultNavigationTransitionInfo = info;
                collection.Add(theme);
                this.Transitions = collection;
                cb_effect.SelectedValue = "None";
            }

        }

        //Function Check Toggle From Default Theme
        private void check_toggle_status()
        {
            if ((string)ApplicationData.Current.LocalSettings.Values["BG_set"] == "Dark")
            {
                Toggle_BG.IsOn = true;
                this.RequestedTheme = ElementTheme.Dark;
            }
            else
            {
                Toggle_BG.IsOn = false;
                this.RequestedTheme = ElementTheme.Light;
            }
        }

        //Function Change Toggle Theme
        public void change_toggle()
        {
            if (Toggle_BG.IsOn == true)
            {
                ApplicationData.Current.LocalSettings.Values["BG_set"] = "Dark";
                this.RequestedTheme = ElementTheme.Dark;
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values["BG_set"] = "Light";
                this.RequestedTheme = ElementTheme.Light;
            }
        }

        //Function Change ComboBox Transition Effect
        private void cb_effect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_effect.SelectedValue.ToString() == "Effect 1")
            {
                ApplicationData.Current.LocalSettings.Values["Effect_set"] = "Continuum";
            }
            else if (cb_effect.SelectedValue.ToString() == "Effect 2")
            {
                ApplicationData.Current.LocalSettings.Values["Effect_set"] = "Common";
            }
            else if (cb_effect.SelectedValue.ToString() == "Effect 3")
            {
                ApplicationData.Current.LocalSettings.Values["Effect_set"] = "Slide";
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values["Effect_set"] = "Off";
            }
        }

        //Initial Text
        private void InitializingPage()
        {
            HeaderInfo.Text = HeaderPage.SettingsPage;
        }

    }
}
