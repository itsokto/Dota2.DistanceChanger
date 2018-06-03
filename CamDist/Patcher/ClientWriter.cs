using System.IO;
using CamDist.Patcher.Abstractions;

namespace CamDist.Patcher
{
    public class ClientWriter : IClientWriter
    {
        public void Write(string path, byte[] distance, long index)
        {
            using (var srteam = new FileStream(path, FileMode.Open, FileAccess.Write))
            {
                srteam.Position = index;
                srteam.Write(distance, 0, distance.Length);
            }
        }
    }
}
