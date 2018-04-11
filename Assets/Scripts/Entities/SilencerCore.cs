using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class SilencerCore : Entity
    {
        protected override IEnumerator Die()
        {
            if (DeathClip != null)
            {
                AudioSource.PlayOneShot(DeathClip);
                Renderer.enabled = false;
                Collider2D.enabled = false;

                yield return new WaitForSeconds(DeathClip.length);
            }

            transform.parent.gameObject.SetActive(false);
        }
    }
}
