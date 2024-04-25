using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Infrastructure
{
    /// <summary>
    /// Interaction with Unity Coroutines.
    /// </summary>
    public interface ICoroutineRunner
    {
        /// <summary>
        /// Starts a Coroutine.
        /// </summary>
        /// <param name="coroutine">The Coroutine to be executed.</param>
        /// <returns>Current Coroutine.</returns>
        Coroutine StartCoroutine(IEnumerator coroutine);

        /// <summary>
        /// Stops the coroutine named <paramref name="methodName"/>.
        /// </summary>
        /// <param name="methodName">Name of coroutine.</param>
        void StopCoroutine(string methodName);

        /// <summary>
        /// Stops an instance of a coroutine.
        /// </summary>
        /// <param name="coroutine">Current Coroutine.</param>
        void StopCoroutine(IEnumerator coroutine);

        /// <summary>
        /// Stops an instance of a coroutine.
        /// </summary>
        /// <param name="coroutine">Current Coroutine.</param>
        void StopCoroutine(Coroutine coroutine);
    }
}