using System;
using System.Linq;

namespace AsyncBasics.Con._2_Returning_completed_tasks
{
    /// <summary>
    /// <b>Problem:</b> You need to implement a synchronous method with an asynchronous signature.
    /// This situation can arise if you are inheriting from an asynchronous interface or base class but wish to
    /// implement it synchronously.
    /// <para>
    /// This technique is particularly useful: 
    /// <list type="bullet">
    /// <item>when unit-testing asynchronous code</item>
    /// <item>when you need a simple stub or mock for an asynchronous interface</item>
    /// </list>
    /// </para>
    /// <para>
    /// <b>Solution:</b> Use <see cref="Task.FromResult{TResult}(TResult)"/> to create and return a 
    /// new <see cref="Task{TResult}"/> that is already completed with the specified value.
    /// </para>
    /// 
    /// <b>Discussion: </b>If you are implementing an asynchronous Interface with synchronous code, avoid any form
    /// of blocking. It's not natural for an asynchronous method to block and then return a completed task.
    /// If an asynchronous method blocks, it prevents the calling thread from starting other tasks, which interferes with
    /// concurrency and may even cause a deadlock.
    /// </summary>
    public class Examples : IMyAsyncInterface
    {
        public Task<int> GetValueAsync(int result = 13)
        {
            return Task.FromResult(result);
        }

        /// <summary>
        /// Helper method for synchronous task with a 'unsuccessful' result using <see cref="TaskCompletionSource"/>.
        /// Conceptually <see cref="Task.FromResult{TResult}(TResult)"/> 
        /// is just a shorthand for <seealso cref="TaskCompletionSource"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>A <seealso cref="Task{TResult}"/> with a  <see cref="NotImplementedException"/> as result.</returns>
        public static Task<T> NotImplementedAsync<T>()
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetException(new NotImplementedException());
            return tcs.Task;
        }

        /// <summary>
        /// If you regularly use <see cref="Task.FromResult{TResult}(TResult)"/> with the same value, consider caching
        /// the actual task to avoid creating extra instances that will have to be garbage-collected.
        /// </summary>
        private static readonly Task<int> zeroTask = Task.FromResult(0);
    }

    public interface IMyAsyncInterface
    {
        Task<int> GetValueAsync(int result);
    }
}
