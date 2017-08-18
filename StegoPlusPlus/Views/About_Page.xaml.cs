using StegoPlusPlus.Controls;
using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StegoPlusPlus.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class About_Page : Page
    {
        public About_Page()
        {
            InitializeComponent();
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            Init_Tips();
            Init_Page();
            Init_Theme();
            Init_Transition();
        }

        #region Initializing Property
        private void Init_Page()
        {
            Init_AppDetail();
            Init_AboutMe();
            HeaderInfo.Text = Data.Prop_Page.AboutPage;
        }
        private void Init_AppDetail()
        {
            lbl_Title_AppDetail.Text = Data.Prop_AppDetail.title;
            lbl_Version_AppDetail.Text = Data.Prop_AppDetail.version;
            lbl_Source_Code_AppDetail.Content = Data.Prop_AppDetail.source_code;
            lbl_Source_Code_AppDetail.Tag = Data.Prop_AppDetail.source_code_link;
            lbl_Bug_Support_AppDetail.Content = Data.Prop_AppDetail.bug_support;
            lbl_Bug_Support_AppDetail.Tag = Data.Prop_AppDetail.bug_support_link;
        }
        private void Init_AboutMe()
        {
            lbl_Title_AboutMe.Text = Data.Prop_AboutMe.title;
            lbl_Head1_AboutMe.Text = Data.Prop_AboutMe.head1;
            lbl_Head2_AboutMe.Text = Data.Prop_AboutMe.head2;
            lbl_Name_AboutMe.Text = Data.Prop_AboutMe.name;
            lbl_Birth_AboutMe.Text = Data.Prop_AboutMe.birth;
            lbl_Domicile_AboutMe.Text = Data.Prop_AboutMe.domicile;
            lbl_School_TK_AboutMe.Text = Data.Prop_AboutMe.school_tk;
            lbl_School_SD_AboutMe.Text = Data.Prop_AboutMe.school_sd;
            lbl_School_SMP_AboutMe.Text = Data.Prop_AboutMe.school_smp;
            lbl_School_SMK_AboutMe.Text = Data.Prop_AboutMe.school_smk;
            lbl_University_AboutMe.Text = Data.Prop_AboutMe.university;
        }
        #endregion
        #region Initializing Animation
        private void Init_Transition()
        {
            string value = (string)ApplicationData.Current.LocalSettings.Values["Effect_set"];
            Transitions = Process.Transition.GetTransition(value);
            Process.Transition.SetTransition(value);
        }
        private void Init_Theme()
        {
            string value = (string)ApplicationData.Current.LocalSettings.Values["BG_set"];
            var setTheme = Process.Theme.GetTheme(value) == true ? RequestedTheme = ElementTheme.Light : RequestedTheme = ElementTheme.Dark;
            Process.Theme.SetTheme(setTheme.ToString());
        }
        #endregion
        #region Initializing Tips
        private void Init_Tips()
        {
            string value = (string)ApplicationData.Current.LocalSettings.Values["Tips_set"];
            if (value == "True")
            {
                Tips_Prop.Visibility = Visibility.Visible;
                Tips_Prop2.Visibility = Visibility.Visible;
                Tips_Prop3.Margin = new Thickness(0, 0, 0, 0);
            }
            else
            {
                Tips_Prop.Visibility = Visibility.Collapsed;
                Tips_Prop2.Visibility = Visibility.Collapsed;
                Tips_Prop3.Margin = new Thickness(0, -5, 0, 0);
            }
        }
        #endregion       

        //Initial Click URL
        async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(((HyperlinkButton)sender).Tag.ToString()));
        }
    }
}
