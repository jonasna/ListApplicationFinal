namespace ListApplicationFinal.Toast
{
    public interface IToastProvider
    {
        void LongMessage(string message);
        void ShortMessage(string message);
    }
}