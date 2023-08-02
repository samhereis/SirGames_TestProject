using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Helpers
{
    public static class AsyncHelper
    {
        public static async Task Delay()
        {
            await Task.Yield();
        }

        public static async Task Delay(float delay)
        {
            int duration = (int)Mathf.Max(delay * 1000, 0);

            await Task.Delay(duration);
        }

        public static async Task Delay(float delay, CancellationToken cancellationToken)
        {
            int duration = (int)Mathf.Max(delay * 1000, 0);

            await Task.Delay(duration, cancellationToken);
        }

        public static async Task Delay(int delay)
        {
            await Task.Delay(delay);
        }

        public static async Task Delay(int delay, CancellationToken cancellationToken)
        {
            await Task.Delay(delay, cancellationToken);
        }
    }
}