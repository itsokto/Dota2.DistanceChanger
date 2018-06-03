using System;
using System.Collections.Generic;
using System.Windows;
using CamDist.Clients;
using CamDist.Patcher;
using CamDist.Patcher.Abstractions;
using Microsoft.Win32;
using NLog;

namespace CamDist
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly ILogger _logger;
        private string _dotaPath;
        private readonly List<IClient> _clients;
        private readonly IPatcher _patcher;

        public MainWindow()
        {
            InitializeComponent();
            _logger = Utils.InitLogger();

            _patcher = 
                new Patcher.Patcher(_logger, new ClientReader(), new ClientWriter(),
                                    new ClientDistanceFinder(), new BackupManager());


            _dotaPath = GetDotaInstallLocation();
            path_tbl.Text = _dotaPath;

            _clients = new List<IClient>
            {
                new Client32(_dotaPath + @"\game\dota\bin\win32\client.dll"),
                new Client64(_dotaPath + @"\game\dota\bin\win64\client.dll")
            };
        }

        public string GetDotaInstallLocation()
        {
            try
            {
                var key = Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 570");
                var value = key?.GetValue("InstallLocation");

                if (value == null)
                {
                    throw new InvalidOperationException(nameof(value));
                }

                _logger?.Info($"Dota path: {value}");

                return value.ToString();
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, $"An error occurred while {nameof(GetDotaInstallLocation)} was executing.");
                return default;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (backup_chkb.IsChecked == true)
                {
                    foreach (var client in _clients)
                    {
                        _patcher.CreateBackup(client.Path, client.Path + ".back", true);
                        var result = _patcher.SetCustomDistance(client.Path, value_tb.Text, client.Patterns);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.Error(ex);
            }
        }
    }
}