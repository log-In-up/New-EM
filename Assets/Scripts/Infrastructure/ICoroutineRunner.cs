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
    }
}