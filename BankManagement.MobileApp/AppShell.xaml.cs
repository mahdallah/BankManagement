﻿namespace BankManagement.MobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for navigation
            Routing.RegisterRoute("SettingsPage", typeof(SettingsPage));
        }
    }
}
