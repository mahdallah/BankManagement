using BankManagement.MobileApp.Consts;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BankManagement.MobileApp;

public partial class SettingsPage : ContentPage
{
    public ObservableCollection<SmsSender> SmsSenders { get; set; }
    public ObservableCollection<SmsSender> FilteredSmsSenders { get; set; }
    public ICommand SaveCommand { get; set; }

    public SettingsPage()
    {
        InitializeComponent();

        SmsSenders = new ObservableCollection<SmsSender>();
        FilteredSmsSenders = new ObservableCollection<SmsSender>();
        SaveCommand = new Command(SaveSelectedSenders);

        BindingContext = this;

        // Populate FilteredSmsSenders initially
        SmsSenders.CollectionChanged += (s, e) => FilterSenders(string.Empty);
    }

    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        FilterSenders(e.NewTextValue);
    }

    private void FilterSenders(string filter)
    {
        FilteredSmsSenders.Clear();
        var filtered = SmsSenders.Where(sender => string.IsNullOrEmpty(filter) ||
                                                   sender.Sender.Contains(filter, StringComparison.OrdinalIgnoreCase));

        foreach (var sender in filtered)
        {
            FilteredSmsSenders.Add(sender);
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Recheck SMS permissions when the page appears
        var permissionStatus = await Permissions.CheckStatusAsync<Permissions.Sms>();
        if (permissionStatus == PermissionStatus.Granted && SmsSenders.Count == 0)
        {
            LoadSmsSenders();
        }
        else if (permissionStatus != PermissionStatus.Granted)
        {
            await PromptForPermission();
        }
    }

    // Method to initialize the page and handle permissions
    private async void InitializePage()
    {
        var permissionStatus = await CheckAndRequestSMSPermission();
        if (permissionStatus == PermissionStatus.Granted)
        {
            LoadSmsSenders();
        }
        else
        {
            await PromptForPermission();
        }
    }

    // Prompt user for permission and guide them to settings
    private async Task PromptForPermission()
    {
        bool goToSettings = await DisplayAlert(
            "Permission Required",
            "SMS permission is required. Please enable it in app settings.",
            "Go to Settings",
            "Cancel");

        if (goToSettings)
        {
            AppInfo.ShowSettingsUI();
        }
    }

    // Method to check and request SMS permissions
    public async Task<PermissionStatus> CheckAndRequestSMSPermission()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Sms>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.Sms>();
        }
        return status;
    }

    private async void LoadSmsSenders()
    {
        await Task.Run(() =>
        {
#if ANDROID
        string INBOX = "content://sms/inbox";
        var uri = Android.Net.Uri.Parse(INBOX)
            ?? throw new Exception("Uri is null");
        var projection = new[] { "DISTINCT address", "body", "date", "type" };
        var cursor = Android.App.Application.Context.ContentResolver.Query(uri, projection, null, null, null);

        if (cursor != null)
        {
            var messages = new List<SmsMessage>();
            try
            {
                while (cursor.MoveToNext())
                {
                    var address = cursor.GetString(cursor.GetColumnIndex("address"));
                    var body = cursor.GetString(cursor.GetColumnIndex("body"));
                    var date = cursor.GetLong(cursor.GetColumnIndex("date"));
                    var type = cursor.GetInt(cursor.GetColumnIndex("type"));

                    messages.Add(new SmsMessage
                    {
                        Address = address,
                        Body = body,
                        Date = DateTimeOffset.FromUnixTimeMilliseconds(date).DateTime,
                        Type = type == 1 ? "Received" : "Sent"
                    });
                }

                // Retrieve saved senders from Preferences
                var savedSendersJson = Preferences.Get("SelectedSmsSenders", string.Empty);
                var savedSenders = string.IsNullOrEmpty(savedSendersJson)
                    ? new List<SmsSender>()
                    : JsonConvert.DeserializeObject<List<SmsSender>>(savedSendersJson);

                // Populate SmsSenders with unique addresses and check if they are saved
                var uniqueSenders = messages.Select(m => m.Address).Distinct().ToList();

                // Update the UI on the main thread
                Device.BeginInvokeOnMainThread(() =>
                {
                    SmsSenders.Clear();
                    foreach (var sender in uniqueSenders)
                    {
                        // Mark sender as selected if it exists in saved senders
                        var isSelected = savedSenders.Any(s => s.Sender == sender);
                        SmsSenders.Add(new SmsSender { Sender = sender, IsSelected = isSelected });
                    }
                });
            }
            finally
            {
                cursor?.Close();
            }
        }
#endif
        });
    }

    private void SaveSelectedSenders()
    {
        var selectedSenders = SmsSenders.Where(sender => sender.IsSelected).ToList();

        if (selectedSenders.Count == 0)
        {
            DisplayAlert("No Selection", "Please select at least one sender.", "OK");
            return;
        }

        // Serialize the selected senders and save them to Preferences
        var serializedSenders = JsonConvert.SerializeObject(selectedSenders);
        Preferences.Set(PreferenceContsts.SelectedSmsSender, serializedSenders);

        DisplayAlert("Saved", $"Selected senders saved successfully.", "OK");
    }
}