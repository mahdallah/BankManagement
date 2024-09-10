using BankManagement.MobileApp.SqlLiteServices;
using BankManagement.MobileApp.Models;
using BankManagement.MobileApp.Helpers;
namespace BankManagement.MobileApp
{
    public partial class MainPage : ContentPage
    {
        private SmsDatabase _smsDatabase;
        private TemplateParser _templateParser;

        public MainPage()
        {
            InitializeComponent();
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "smsmessages.db3");
            _smsDatabase = new SmsDatabase(dbPath);
            _templateParser = new TemplateParser();
        }

        public async Task<PermissionStatus> CheckAndRequestSMSPermission()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Sms>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.Sms>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }
            status = await Permissions.RequestAsync<Permissions.Sms>();
            return status;
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            var res = await CheckAndRequestSMSPermission();
            if (res.Equals(PermissionStatus.Granted))
            {

                #if ANDROID
                    string INBOX = "content://sms/inbox";
                    string[] reqCols = new string[] { "_id", "thread_id", "address", "person", "date", "body", "type" };
                    Android.Net.Uri uri = Android.Net.Uri.Parse(INBOX);
                    Android.Database.ICursor cursor = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.ContentResolver.Query(uri, reqCols, "address=?", new string[] { "alinmabank" }, null);

                    if (cursor.MoveToFirst())
                    {
                        do
                        {
                            string body = cursor.GetString(cursor.GetColumnIndex(reqCols[5]));
                            var parsedMessage = _templateParser.ParseSmsMessage(body);
                            await _smsDatabase.SaveMessageAsync(parsedMessage);
                        } while (cursor.MoveToNext());
                    }
                #endif

                var unknownTemplates = await _smsDatabase.GetUnknownTemplatesAsync();
                foreach (var template in unknownTemplates)
                {
                    Console.WriteLine($"Unknown template: {template.RawMessage}");
                }
            }
        }
    }
}
