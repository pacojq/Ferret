/*
    Coroutine system based in Chevy Ray Johnston's work: 
    https://github.com/ChevyRay/Coroutines
 
    MIT License
    Copyright (c) 2017 Chevy Ray Johnston
    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:
    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.
    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;

namespace FerretEngine.Coroutines
{
    /// <summary>
    /// Handles all <see cref="Coroutine"/> running in the game.
    /// </summary>
    public static class FeCoroutines
    {
        public delegate IEnumerator Coroutine();

        private static List<IEnumerator> _running;
        private static List<float> _delays;


        internal static void Initialize()
        {
            _running = new List<IEnumerator>();
            _delays = new List<float>();
        }

        internal static void Dispose()
        {
            _running.Clear();
            _delays.Clear();
        }
        
        
        /// <summary>
        /// Update all running coroutines.
        /// </summary>
        /// <returns>True if any routines were updated.</returns>
        /// <param name="deltaTime">How many seconds have passed sinced the last update.</param>
        internal static void Update(float deltaTime)
        {
            for (int i = 0; i < _running.Count; i++)
            {
                if (_delays[i] > 0f)
                    _delays[i] -= deltaTime;
                else if (_running[i] == null || !MoveNext(_running[i], i))
                {
                    _running.RemoveAt(i);
                    _delays.RemoveAt(i--);
                }
            }
        }

        private static bool MoveNext(IEnumerator routine, int index)
        {
            if (routine.Current is IEnumerator)
            {
                if (MoveNext((IEnumerator)routine.Current, index))
                    return true;
                
                _delays[index] = 0f;
            }

            bool result = routine.MoveNext();

            if (routine.Current is float)
                _delays[index] = (float)routine.Current;
            
            return result;
        }
        
        
        
        
        
        /// <summary>
        /// Run a coroutine.
        /// </summary>
        /// <returns>A handle to the new coroutine.</returns>
        /// <param name="delay">How many seconds to delay before starting.</param>
        /// <param name="routine">The routine to run.</param>
        public static CoroutineHandle Run(float delay, Coroutine routine)
        {
            IEnumerator r = routine();
            _running.Add(r);
            _delays.Add(delay);
            return new CoroutineHandle(r);
        }
        
        
        
        /// <summary>
        /// Stop the specified routine.
        /// </summary>
        /// <returns>True if the routine was actually stopped.</returns>
        /// <param name="routine">The routine to stop.</param>
        public static bool Stop(CoroutineHandle routine)
        {
            return routine.Stop();
        }

        internal static bool Stop(IEnumerator routine)
        {
            int i = _running.IndexOf(routine);
            if (i < 0)
                return false;
            _running[i] = null;
            _delays[i] = 0f;
            return true;
        }
        
       
        
        
        /// <summary>
        /// Check if the routine is currently running.
        /// </summary>
        /// <returns>True if the routine is running.</returns>
        /// <param name="routine">The routine to check.</param>
        public static bool IsRunning(CoroutineHandle routine)
        {
            return routine.IsRunning;
        }
        
        internal static bool IsRunning(IEnumerator routine)
        {
            return _running.Contains(routine);
        }
        
        
    }
}