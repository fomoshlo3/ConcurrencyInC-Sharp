using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncBasics.Con._5_Waiting_for_any_task_to_complete
{
    /// <summary>
    /// <b>Problem:</b> You have several tasks and need to respond to just one of them comnpleting.
    /// The most common situation for this is when you have multiple independent attempts at an operation, with a
    /// first-one-takes-all kind of structure.
    /// <para>
    /// For example: Requesting stock quotes from multiple web services
    /// simultaneously, but only caring about the first one that responds.
    /// </para>
    /// </summary>
    public class Examples
    {
        /// <summary>
        /// <b>Solution:</b> Use the <see cref="Task.WhenAny(Task, Task)"/> method. This method takes a sequence
        /// of tasks and returns a task that completes when any of the tasks complete. The result of the returned task
        /// is the task that completed.
        /// <para>
        ///    <strong>Discussion:</strong> The task returned by <see cref="Task.WhenAny(Task, Task)"/> never completes
        ///    in a faulted or canceled state. It always results in the first <see langword="Task"/> to complete. If that
        ///    task completed with an <see cref="Exception"/>, it won't be propagated to the task returned by
        ///    <see cref="Task.WhenAny(Task, Task)"/>. For this reason you should usually await the task after it has completed.
        /// </para>
        /// <para>
        ///     When the first task completes, consider whther to cancel the remaining tasks. If the other tasks aren't 
        ///     cancelled but are also never awaited, they're abandoned. Abandoned tasks will run to completion, 
        ///     ignoring their results. Any exceptions from those tasks will also be ignored.
        /// </para>
        /// <para>
        ///     It is possible to use <see cref="Task.WhenAny(Task, Task)"/> to implement timeouts 
        ///     (e.g. using <see cref="Task.Delay(int)"/>), but it's not recommended. It's more natural to express timeouts
        ///     with cancellation, which has the added benefit of actually cancel the operation if they timeout.
        /// </para>
        /// <para>
        ///     Another anti-pattern for <see cref="Task.WhenAny(Task, Task)"/> is handling Tasks as they complete. 
        ///     At first it seems like a reasonable approach to keep a list of tasks and remove each task from the list
        ///     as it completes. The problem here is that it executes in <em>O(N^2) time</em> when an <em>O(N) algortihm</em>
        ///     exists.
        /// </para>
        /// </summary>
        /// <param name="urlA"></param>
        /// <param name="urlB"></param>
        /// <returns></returns>
        public static async Task<int> FirstRespondingUrlAsync(string urlA, string urlB)
        {
            using (var httpClient = new HttpClient())
            {
                // Start both downloads concurrently
                Task<byte[]> downloadTaskA = httpClient.GetByteArrayAsync(urlA);
                Task<byte[]> downloadTaskB = httpClient.GetByteArrayAsync(urlB);

                // Wait for either of the tasks to complete.
                Task<byte[]> completedTask = await Task.WhenAny(downloadTaskA, downloadTaskB);

                // Return the length of the data retrieved from that URL.
                byte[] data = await completedTask;
                return data.Length;
            }
        }
    }
}
