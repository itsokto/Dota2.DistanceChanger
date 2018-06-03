using System.IO;
using CamDist.Patcher.Abstractions;

namespace CamDist.Patcher
{
    public class ClientReader: IClientReader
    {
        public byte[] ReadBytes(string path, long index = 0, long count = 0)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                count = count == 0 ? stream.Length : count;

                byte[] buffer = new byte[count];
                stream.Position = index;
                stream.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }
    }
}
