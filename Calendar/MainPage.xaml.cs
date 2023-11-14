namespace Calendar
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            await CheckAndRequestCalendarPermission();
        }

        public async Task<bool> CheckAndRequestCalendarPermission()
        {
            try
            {
                var statusWrite = await Permissions.CheckStatusAsync<Permissions.CalendarWrite>();
                var statusRead = await Permissions.CheckStatusAsync<Permissions.CalendarRead>();

                if (statusWrite != PermissionStatus.Granted && statusRead != PermissionStatus.Granted)
                {
                    statusWrite = await Permissions.RequestAsync<Permissions.CalendarWrite>();
                    statusRead = await Permissions.RequestAsync<Permissions.CalendarRead>();

                    if (statusWrite == PermissionStatus.Granted && statusRead == PermissionStatus.Granted)
                    {
                        return true;
                    }
                    else
                    {
                        if (DeviceInfo.Platform == DevicePlatform.iOS)
                        {
                            // Prompt the user to turn on in settings
                            // On iOS once a permission has been denied it may not be requested again from the application
                            await App.Current.MainPage.DisplayAlert("Calendar permission has been denied", "You can turn access on from your iPhone Settings->Privacy->Calendars.", "OK");
                        }
                        return false;
                    }
                }

                if (statusWrite == PermissionStatus.Granted && statusRead == PermissionStatus.Granted)
                {
                    return true;
                }
                else if (statusWrite == PermissionStatus.Unknown && statusRead == PermissionStatus.Unknown)
                {
                    return false;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}