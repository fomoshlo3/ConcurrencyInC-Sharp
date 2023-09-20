namespace AsyncBasics.Con._4_Waiting_for_a_set_of_tasks_to_complete
{
    /// <summary>
    /// <b>Problem:</b> You have several tasks and need to wait for them all to complete.
    /// <b>Discussion:</b> If any of the tasks throws an exception, then <see cref="Task.WhenAll(Task[])"/> will fault its
    /// returned Task with that exception.
    /// </summary>
    public class Examples
    {

        /// <summary>
        /// <b>Solution:</b> The framework provides a <see cref="Task.WhenAll(Task[])"/> method for this purpose.
        /// This method takes several tasks and returns a task that completes when all of those tasks have completed.
        /// </summary>
        /// <returns></returns>
        public async Task SetOfTasksToComplete()
        {
            Task task1 = Task.Delay(TimeSpan.FromSeconds(1));
            Task task2 = Task.Delay(TimeSpan.FromSeconds(2));
            Task task3 = Task.Delay(TimeSpan.FromSeconds(1));

            await Task.WhenAll(task1, task2, task3);
        }


        /// <summary>
        /// <b>Detail:</b>Showcasing that if all tasks awaited by <see cref="Task.WhenAll(Task[])"/>
        /// , complete successfully and
        /// have the same result type it returns an array containing all tasks results.
        /// </summary>
        /// <returns></returns>
        public async Task<int[]> SetOfTasksWithSameResultType()
        {
            Task<int> task4 = Task.FromResult(3);
            Task<int> task5 = Task.FromResult(5);
            Task<int> task6 = Task.FromResult(7);

            return await Task.WhenAll(task4, task5, task6);
        }

        /// <summary>
        /// <b>Detail:</b> There is an overload of <see cref="Task.WhenAll(IEnumerable{Task})"/> that takes an
        /// <seealso cref="IEnumerable{T}"/> of tasks; However, wouldn't recommend usage.
        /// Whenever mixing asynchronous code with <em>LINQ</em>, the code is clearer when explicitly "reifying" the sequence
        /// (i.e. evaluating the sequence, creating a collection)
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        public static async Task<string> DownloadAllAsync(IEnumerable<string> urls)
        {
            using (var httpClient = new HttpClient())
            {
                //Define what to do for each URL
                var downloads = urls.Select(url => httpClient.GetStringAsync(url));
                    //Note that no tasks have actually started yet because the sequence is not evaluated.


                //Start all URLs downloading simultaneously.
                Task<string>[] downloadTasks = downloads.ToArray();
                // Now the tasks have all started

                //Asynchronously wait for all downloads to complete
                string[] htmlPages = await Task.WhenAll(downloadTasks);

                return string.Concat(htmlPages);
            }
        }

        static async Task ThrowNotImplementedException()
        {
            throw new NotImplementedException();
        }

        static async Task ThrowInvalidOperationException()
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// <b>Discussion:</b> If multiple tasks throw an exception, all of those exeptions are placed on the 
        /// <see langword="Task"/> returned by <seealso cref="Task.WhenAll(Task[])"/>. When that task is awaited,
        /// only one of them will be thrown.
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="NotImplementedException"/>
        public static async Task ObserveOneExceptionAsync()
        {
            var task1 = ThrowNotImplementedException();
            var task2 = ThrowInvalidOperationException();

            try
            {
                await Task.WhenAll(task1,task2);
            }
            catch (Exception ex)
            {
                
            }
        }

        /// <summary>
        /// <b>Discussion:</b> If you need each specific exception, you can examine the exception property
        /// on the <see langword="Task"/> returned by <see cref="Task.WhenAll(Task[])"/>.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AggregateException"/>
        public static async Task ObserveAllExceptionsAsync()
        {
            var task1 = ThrowInvalidOperationException();
            var task2 = ThrowNotImplementedException();

            Task allTasks = Task.WhenAll(task1,task2);
            try
            {
                await allTasks;
            }
            catch
            {
                AggregateException allExceptions = allTasks.Exception;
            }
        }
    }
}

