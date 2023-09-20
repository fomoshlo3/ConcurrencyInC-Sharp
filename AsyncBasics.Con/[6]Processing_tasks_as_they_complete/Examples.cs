using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncBasics.Con._6_Processing_tasks_as_they_complete
{
    /// <summary>
    /// <b>Problem:</b> You have a collection of tasks to await, and you want to do some processing on each task
    /// after it completes without waiting for the other tasks of the collection.
    /// </summary>
    public static class Examples
    {
        public static async Task<int> DelayAndReturnAsync(int val)
        {
            await Task.Delay(TimeSpan.FromSeconds(val));
            return val;
        }

        public static async Task AwaitAndProcessAsync(Task<int> task)
        {
            var result = await task;
            Trace.WriteLine(result);
        }

        /// <summary>
        /// <para>
        /// <b>Solution:</b> Introducing the higher-level async method <see cref="AwaitAndProcessAsync(Task{int})"/>
        /// that handles awaiting the task and processing its result.
        /// </para>
        /// <example>
        /// Alternatively, comment out <see cref="AwaitAndProcessAsync(Task{int})"/> and change <see cref="ProcessTasksAsync"/>
        /// to this :
        ///     <code>
        ///         var processingTasks = tasks.Select( async t => 
        ///         {
        ///             var result = await t;
        ///             Trace.WriteLine(result);
        ///         }).ToArray();
        ///     </code>
        /// </example>
        /// This is the cleanest and most portable way to solve this problem, it changes the task processing from one-at-a-time
        /// to a concurrent approach, if it's not acceptable for your situation, consider using <see langword="Locks"/>
        /// or use the extension method linked:
        /// <list type="bullet">
        ///   <item>
        ///     <see href="https://codeblog.jonskeet.uk/2012/01/16/eduasync-part-19-ordering-by-completion-ahead-of-time/">
        ///         Jon Skeet's coding blog</see>
        ///   </item>
        ///     <item></item>
        /// </list>
        /// </summary>
        /// <returns></returns>
        public static async Task ProcessTasksAsync()
        {
            Task<int> taskA = DelayAndReturnAsync(2);
            Task<int> taskB = DelayAndReturnAsync(3);
            Task<int> taskC = DelayAndReturnAsync(1);

            var tasks = new[] { taskA, taskB, taskC };

            var processingTasks = (from t in tasks
                                   select AwaitAndProcessAsync(t)).ToArray();

            await Task.WhenAll(processingTasks);
        }
    }
}