using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class SilencerCore : Entity
    {
        private Silencer _silencer;

        protected override void Awake()
        {
            _silencer = transform.parent.GetComponent<Silencer>();

            base.Awake();
        }

        protected override IEnumerator Die()
        {
            if (DeathClip != null)
            {
                AudioSource.PlayOneShot(DeathClip);
                Renderer.enabled = false;
                Collider2D.enabled = false;

                yield return new WaitForSeconds(DeathClip.length);
            }

            _silencer.Kill();
        }
    }
}
