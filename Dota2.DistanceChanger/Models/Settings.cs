using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dota2.DistanceChanger.Models
{
    public class Settings
    {
        public IList<Client> Clients { get; set; }

        public string Dota2FolderPath { get; set; }

        public IList<byte[]> Patterns { get; set; }
    }
}