using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncBasics.Con._3_Reporting_progress
{
    /// <summary>
    /// <b>Problem:</b> You need to respond to progress while an asynchronous operation is executing.
    /// <para>
    /// <b>Discussion:</b> By convention the <see cref="IProgress{T}"/> parameter may be <seealso langword="null"/>
    /// if the caller does not need progress reports, so be sure to check for this in your async method.
    /// </para>
    /// <para>
    /// Bear in mind that the <seealso cref="IProgress{T}.Report(T)"/> method may be asynchronous.
    /// This means <see cref="MyMethodAsync(IProgress{double})"/> may continue executing before the progress is actually
    /// reported. For this reason, it's best to define <see langword="T"/> as an <em>immutable</em> type or
    /// at least a <em>value</em> type. If <see langword="T"/> is a <em>mutable</em> reference type, then you'll have
    /// to create a separate copy yourself each time you call <seealso cref="IProgress{T}.Report(T)"/>.
    /// </para>
    /// <para>
    /// <seealso cref="Progress{T}"/> will capture the current context when it is constructed and will invoke its
    /// callback within that context. So if you construct the <seealso cref="Progress{T}"/> on the UI thread,
    /// then you can update the UI from its callback, even if the asynchronous method is invoking <see langword="Report"/>
    /// from a background thread.
    /// </para>
    /// When a method supports progress reporting, it should also make a best effort to support <em>cancellation</em>.
    /// </summary>
    public class Examples
    {
        /// <summary>
        /// <b>Solution:</b> Use the provided <see cref="IProgress{T}"/> and <seealso cref="Progress{T}"/> types.
        /// Your <see langword="async"/> method should take an <seealso cref="IProgress{T}"/> argument.
        /// <seealso langword="T"/> is whatever type of progress you need to report.
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static async Task MyMethodAsync(IProgress<double>? progress = null)
        {
            double percentComplete = 0;
            while (percentComplete != 100)
            {
                await Task.Delay(100);
                ///...
                if(progress != null) progress.Report(percentComplete);
            }
        }

        /// <summary>
        /// Callback method to use, for instance, in an UI to show a progress bar. 
        /// </summary>
        /// <returns></returns>
        public static async Task CallMyMethodAsync()
        {
            var progress = new Progress<double>();
            progress.ProgressChanged += (sender, args) =>
            {
                ///...
            };
            await MyMethodAsync(progress);
        }
    }
}
