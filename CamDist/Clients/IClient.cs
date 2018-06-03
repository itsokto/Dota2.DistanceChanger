using System.Collections.Generic;

namespace CamDist.Clients
{
    public interface IClient
    {
        string Path { get; set; }
        string Distance { get; set; }
        IEnumerable<byte[]> Patterns { get; set; }
    }
}
