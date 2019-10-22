using System.Collections;
using System.Collections.Generic;

namespace FerretEngine.Coroutines
{
    public class CoroutineHandle
    {
        /// <summary>
        /// True if the enumerator is currently running.
        /// </summary>
        public bool IsRunning => _enumerator != null && FeCoroutines.IsRunning(_enumerator);


        private readonly IEnumerator _enumerator;
        
        internal CoroutineHandle(IEnumerator enumerator)
        {
            _enumerator = enumerator;
        }
        
        
        /// <summary>
        /// Stop this coroutine if it is running.
        /// </summary>
        /// <returns>True if the coroutine was stopped.</returns>
        public bool Stop()
        {
            return IsRunning && FeCoroutines.Stop(_enumerator);
        }
        
        /// <summary>
        /// A routine to wait until this coroutine has finished running.
        /// </summary>
        /// <returns>The wait enumerator.</returns>
        public IEnumerator Wait()
        {
            if (_enumerator != null)
                while (FeCoroutines.IsRunning(_enumerator))
                    yield return null;
        }
    }
}