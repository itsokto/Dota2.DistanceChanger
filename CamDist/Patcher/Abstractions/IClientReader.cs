namespace CamDist.Patcher.Abstractions
{
    public interface IClientReader
    {
        byte[] ReadBytes(string path, long index = 0, long count = 0);
    }
}