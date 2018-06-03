namespace CamDist.Patcher.Abstractions
{
    public interface IClientWriter
    {
        void Write(string path, byte[] distance, long index);
    }
}