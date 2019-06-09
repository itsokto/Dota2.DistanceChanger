using System.ComponentModel;
using DynamicData.Binding;

namespace Dota2.DistanceChanger.Core.Models
{
    public class Settings : INotifyPropertyChanged
    {
        public bool Backup { get; set; }

        public ObservableCollectionExtended<Client> Clients { get; set; }

        public bool DarkMode { get; set; }

        public string Dota2FolderPath { get; set; }

        public ObservableCollectionExtended<byte[]> Patterns { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}