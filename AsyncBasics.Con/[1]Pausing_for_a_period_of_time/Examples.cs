namespace AsyncBasics.Con._1_Pausing_for_a_period_of_time
{
    /// <summary>
    /// <b>Problem:</b>
    /// You need to (asynchronously) wait for a period of time. This can be useful when <em>unit-testing</em>, <em>implementing 
    /// retry delays</em> or <em>simple time-outs</em>.
    /// <para>
    /// <b>Solution:</b>the Task type has a static method: <see cref="Task.Delay(TimeSpan))"/>.
    /// </para>
    /// <para>
    /// <b>
    /// Discussion:
    /// </b>
    /// <see cref="Task.Delay(TimeSpan))"/> is a fine option for unit-testing asynchronous code or for implementing retry logic.
    /// <em>However</em>, if one needs to implement a timeout, a <seealso cref="CancellationToken"/> is usually a better choice.
    /// </para>
    /// 
    /// </summary>
    public static class Examples
    {
        /// <summary>
        /// Defines a task that completes asynchronously, for use with unit-testing.
        /// When faking an asynchronous operation, it's important to test at least synchronous success
        /// and asynchronous success as well as asynchronous failure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="delay"></param>
        /// <returns>A <see cref="Task"/> for the asynchronous success case.</returns>
        public static async Task<T> DelayResult<T>(T result, TimeSpan delay)
        {
            await Task.Delay(delay);
            return result;
        }

        /// <summary>
        /// Simple implementation of an <em>exponential backoff</em>, that is, a retry strategy,
        /// where you increase the delays between retries. Exponential backoff is a best practice when
        /// when working with web-services to ensure the server does not get flooded with retries.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>A response body as a <see langword="string"/> if the Task is completed successfully.</returns>
        public static async Task<string> DownloadStringWithRetries(string uri)
        {
            using (var client = new HttpClient())
            {
                //Retry after 1, then 2 and 4 seconds
                var nextDelay = TimeSpan.FromSeconds(1);
                // Note: If one can point out why prefix increment.
                for (int i = 0; i != 3; ++i)
                {
                    try
                    {
                        return await client.GetStringAsync(uri);
                    }
                    catch
                    {
                    }

                    await Task.Delay(nextDelay);
                    nextDelay += nextDelay;
                }

                // Try one last time, allowing the error to propagate ([DE]= verbreiten).
                return await client.GetStringAsync(uri);
            }
        }

        /// <summary>
        /// Simple timeout using <see cref="Task.Delay(int)"/>.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns><see langword="null"/> if the service does not respond within three seconds</returns>
        public static async Task<string?> DownloadStringWithTimeout(string uri)
        {
            using (var client = new HttpClient())
            {
                var downloadTask = client.GetStringAsync(uri);
                var timeoutTask = Task.Delay(300);

                var completedTask = await Task.WhenAny(downloadTask, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    return null;
                }

                return await downloadTask;
            }
        }
    }
}

