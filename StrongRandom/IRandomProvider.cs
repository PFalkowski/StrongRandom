namespace Extensions.Standard.RandomExtensions
{
    public interface IRandomProvider
    {
        void GetBytes(byte[] input);
    }
}
