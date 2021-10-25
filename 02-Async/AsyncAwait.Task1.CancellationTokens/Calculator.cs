using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens
{
    static class Calculator
    {
        public static Task<long> CalculateAsync(int n, CancellationToken token)
        {
            return Task.Run(() =>
            {
                long sum = 0;
                for (int i = 0; i < n; i++)
                {
                    token.ThrowIfCancellationRequested();
                    // i + 1 is to allow 2147483647 (Max(Int32)) 
                    sum = sum + (i + 1);
                    Thread.Sleep(10);
                }
                return sum;
            }, token);
        }
    }
}
