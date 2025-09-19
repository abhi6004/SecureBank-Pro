namespace SecureBank_Pro.Services
{
    public class Logs
    {
        public static async Task LogTransaction(string message)
        {
            try
            {
                string _transactionLogFile = Path.Combine(Directory.GetCurrentDirectory(), "Logs/transaction.txt");
                string log = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}{Environment.NewLine}";
                await File.AppendAllTextAsync(_transactionLogFile, log);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
