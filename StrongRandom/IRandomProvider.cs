namespace Extensions.Standard.Randomization
{
    public interface IRandomProvider
    {
        void GetBytes(byte[] input);
    }
}
