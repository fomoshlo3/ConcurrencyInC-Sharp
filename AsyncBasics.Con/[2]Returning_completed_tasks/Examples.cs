using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// </summary>
    public class Examples
    {

    }

    public interface IMyAsyncInterface
    {
        Task<int> GetValueAsync();
    }
}
