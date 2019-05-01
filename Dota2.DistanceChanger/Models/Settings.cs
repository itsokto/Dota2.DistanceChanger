using DynamicData.Binding;
using PropertyChanged;

namespace Dota2.DistanceChanger.Models
{
    [AddINotifyPropertyChangedInterface]
    public class Settings
    {
        public ObservableCollectionExtended<Client> Clients { get; set; }

        public string Dota2FolderPath { get; set; }

        public ObservableCollectionExtended<byte[]> Patterns { get; set; }
    }
}