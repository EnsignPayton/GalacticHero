using System;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Wrapper for MonoBehaviour
    /// </summary>
    public abstract class Script : MonoBehaviour, IDisposable
    {
        // Only adding the messages I plan on using
        #region MonoBehaviour Messages

        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
        /// </summary>
        protected virtual void Start()
        {
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        protected virtual void Update()
        {
        }

        /// <summary>
        /// OnBecameInvisible is called when the renderer is no longer visible by any camera.
        /// </summary>
        protected virtual void OnBecameInvisible()
        {
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
        }

        /// <summary>
        /// Sent when an incoming collider makes contact with this object's collider (2D physics only).
        /// </summary>
        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
        }

        /// <summary>
        /// Sent when another object enters a trigger collider attached to this object (2D physics only).
        /// </summary>
        protected virtual void OnTriggerEnter2D(Collider2D triggerCollider)
        {
        }

        /// <summary>
        /// Sent when another object leaves a trigger collider attached to this object (2D physics only).
        /// </summary>
        protected virtual void OnTriggerExit2D(Collider2D triggerCollider)
        {
        }

        #endregion

        #region IDisposable

        protected bool IsDisposed { get; set; }

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing, float delay = 0.0f)
        {
            if (IsDisposed) return;

            IsDisposed = true;
            if (disposing)
            {
                Destroy(gameObject, delay);
            }
        }

        #endregion
    }
}
