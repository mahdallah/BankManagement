using BankManagement.MobileApp.Consts;
using Newtonsoft.Json;

namespace BankManagement.MobileApp
{
    public partial class MainPage : ContentPage
    {
        private List<SmsMessage> allSmsMessages = new();
        private int itemsToLoad = 50;
        private int itemsLoaded = 0;

        public MainPage()
        {
            InitializeComponent();
            LoadSmsMessages();

            // Enable lazy loading
            SmsListView.ItemAppearing += SmsListView_ItemAppearing;
        }

        private void LoadSmsMessages()
        {
            var selectedSenders = GetSelectedSenders();
            if (selectedSenders == null || selectedSenders.Count == 0)
            {
                DisplayAlert("No Senders", "No selected senders found in preferences.", "OK");
                return;
            }

            allSmsMessages = GetSmsFromSelectedSenders(selectedSenders);

            // Populate the filter dropdown
            SenderFilterPicker.ItemsSource = selectedSenders.Prepend("All").ToList();
            SenderFilterPicker.SelectedIndex = 0;

            // Display SMS messages with paging
            DisplayMessages();
        }

        private List<string> GetSelectedSenders()
        {
            var senders = Preferences.Get(PreferenceContsts.SelectedSmsSender, "");
            return !string.IsNullOrEmpty(senders)
                ? JsonConvert.DeserializeObject<List<SmsSender>>(senders).Select(s => s.Sender.Trim()).ToList()
                : new List<string>();
        }

        private List<SmsMessage> GetSmsFromSelectedSenders(List<string> selectedSenders)
        {
            var smsList = new List<SmsMessage>();
            var uniqueMessages = new HashSet<int>(); // Prevent duplicates

#if ANDROID
            string INBOX = "content://sms/inbox";
            var reqCols = new string[]
            {
        "_id", "thread_id", "address", "person", "date", "date_sent", "body",
        "type", "read", "status", "service_center", "locked", "error_code"
            };

            Android.Net.Uri uri = Android.Net.Uri.Parse(INBOX);
            Android.Database.ICursor cursor = null;

            try
            {
                cursor = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.ContentResolver
                    .Query(uri, reqCols, null, null, "date DESC LIMIT 100"); // Fetch only the latest 100 messages

                if (cursor != null && cursor.MoveToFirst())
                {
                    int idIndex = cursor.GetColumnIndex("_id");
                    int threadIdIndex = cursor.GetColumnIndex("thread_id");
                    int senderIndex = cursor.GetColumnIndex("address");
                    int contactIndex = cursor.GetColumnIndex("person");
                    int dateIndex = cursor.GetColumnIndex("date");
                    int dateSentIndex = cursor.GetColumnIndex("date_sent");
                    int bodyIndex = cursor.GetColumnIndex("body");
                    int typeIndex = cursor.GetColumnIndex("type");
                    int readIndex = cursor.GetColumnIndex("read");
                    int statusIndex = cursor.GetColumnIndex("status");
                    int serviceCenterIndex = cursor.GetColumnIndex("service_center");
                    int lockedIndex = cursor.GetColumnIndex("locked");
                    int errorCodeIndex = cursor.GetColumnIndex("error_code");

                    do
                    {
                        int messageId = cursor.GetInt(idIndex);
                        if (uniqueMessages.Contains(messageId)) continue; // Skip duplicates

                        string sender = cursor.GetString(senderIndex);
                        if (selectedSenders.Contains(sender))
                        {
                            smsList.Add(new SmsMessage
                            {
                                MessageId = messageId,
                                ThreadId = cursor.GetInt(threadIdIndex),
                                Sender = sender,
                                ContactId = contactIndex != -1 ? cursor.GetString(contactIndex) : "Unknown",
                                DateReceived = DateTimeOffset.FromUnixTimeMilliseconds(cursor.GetLong(dateIndex)).LocalDateTime,
                                DateSent = dateSentIndex != -1 ? DateTimeOffset.FromUnixTimeMilliseconds(cursor.GetLong(dateSentIndex)).LocalDateTime : DateTime.MinValue,
                                Message = cursor.GetString(bodyIndex),
                                MessageType = cursor.GetInt(typeIndex) == 1 ? "Inbox" : "Sent",
                                ReadStatus = cursor.GetInt(readIndex) == 1 ? "Read" : "Unread",
                                Status = statusIndex != -1 ? cursor.GetInt(statusIndex).ToString() : "Unknown",
                                ServiceCenter = serviceCenterIndex != -1 ? cursor.GetString(serviceCenterIndex) : "N/A",
                                IsLocked = lockedIndex != -1 && cursor.GetInt(lockedIndex) == 1,
                                ErrorCode = errorCodeIndex != -1 ? cursor.GetInt(errorCodeIndex) : 0
                            });

                            uniqueMessages.Add(messageId);
                        }
                    } while (cursor.MoveToNext());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading SMS: {ex.Message}");
            }
            finally
            {
                cursor?.Close();
                cursor?.Dispose();
            }
#endif
            return smsList;
        }

        private void DisplayMessages()
        {
            var selectedSender = SenderFilterPicker.SelectedItem?.ToString();

            var filteredMessages = selectedSender == "All"
                ? allSmsMessages.Take(itemsToLoad).ToList() // Load only the first batch
                : allSmsMessages.Where(m => m.Sender == selectedSender).Take(itemsToLoad).ToList();

            SmsListView.ItemsSource = filteredMessages;
            itemsLoaded = filteredMessages.Count;
        }

        private void OnFilterChanged(object sender, EventArgs e)
        {
            itemsLoaded = 0; // Reset loaded items count
            DisplayMessages();
        }

        private void SmsListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (e.Item == allSmsMessages.LastOrDefault() && itemsLoaded < allSmsMessages.Count)
            {
                itemsLoaded += itemsToLoad;
                SmsListView.ItemsSource = allSmsMessages.Take(itemsLoaded).ToList();
            }
        }
    }

}
