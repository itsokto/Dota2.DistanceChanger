using System.Collections.Generic;

namespace CamDist.Clients
{
    public class Client64 : IClient
    {
        public string Path { get; set; }
        public string Distance { get; set; }
        public IEnumerable<byte[]> Patterns { get; set; }

        public Client64(string path)
        {
            Path = path;
            Patterns = new List<byte[]>
            {
                new byte[] //dota_camera_distance
                {
                    0x64, 0x6F, 0x74, 0x61, 0x5F, 0x63, 0x61, 0x6D, 0x65, 0x72, 0x61, 0x5F, 0x64, 0x69, 0x73, 0x74, 0x61, 0x6E, 0x63, 0x65, 0x00
                },
                new byte[] //dota_camera_pitch_max
                {
                    0x00, 0x64, 0x6F, 0x74, 0x61, 0x5F, 0x63, 0x61, 0x6D, 0x65, 0x72, 0x61, 0x5F, 0x70, 0x69, 0x74,
                    0x63, 0x68, 0x5F, 0x6D, 0x61, 0x78, 0x00
                }
            };
        }
    }
}
