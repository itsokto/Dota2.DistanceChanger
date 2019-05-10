namespace Dota.Patcher.Core.Models
{
    public class SearchResult<T>
    {
        public int Offset { get; set; }
        public T Value { get; set; }
    }
}