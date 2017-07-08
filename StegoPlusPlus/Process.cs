using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace StegoPlusPlus
{
    class Process
    {
        public class Theme
        {
            public static bool GetTheme(string getTheme)
            {
                bool value = (getTheme == "Light") ? true : false;
                return value;
            }
            public static void SetTheme(string value)
            {
                ApplicationData.Current.LocalSettings.Values["BG_set"] = value;
            }

        }
        public class Transition
        {
            public static NavigationThemeTransition GetTransition(string getTransition)
            {
                NavigationThemeTransition theme = new NavigationThemeTransition();

                if (getTransition == "Continuum")
                {
                    theme.DefaultNavigationTransitionInfo = new ContinuumNavigationTransitionInfo();
                    return theme;
                }

                else if (getTransition == "Common")
                {
                    theme.DefaultNavigationTransitionInfo = new CommonNavigationTransitionInfo();
                    return theme;
                }

                else if (getTransition == "Slide")
                {
                    theme.DefaultNavigationTransitionInfo = new SlideNavigationTransitionInfo();
                    return theme;
                }

                else
                {
                    theme.DefaultNavigationTransitionInfo = new SuppressNavigationTransitionInfo();
                    return theme;
                }
            }
            public static void SetTransition(string value)
            {
                ApplicationData.Current.LocalSettings.Values["Effect_set"] = value;
            }

        }

    }
}
