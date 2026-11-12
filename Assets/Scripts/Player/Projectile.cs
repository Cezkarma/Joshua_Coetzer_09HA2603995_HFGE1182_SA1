using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("HIT FEEDBACK")]
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private string hitSound = "ArrowHit";

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy") || col.gameObject.CompareTag("NPC"))
        {
            PlayHitFeedback(col);
        }

        switch (col.gameObject.tag)
        {
            case  "Player":
                col.gameObject.GetComponent<ShootHandler>().AddAmmo(1);
                Destroy(gameObject);
                break;
            case  "Enemy":
                col.gameObject.GetComponent<HealthHandler>().DamageHandler("Arrow", damage);
                Destroy(gameObject);
                break;
            case  "NPC":
                col.gameObject.GetComponent<HealthHandler>().DamageHandler("Arrow", damage);
                Destroy(gameObject);
                break;
            case "Crate":
                col.gameObject.GetComponent<HealthHandler>().DamageHandler("Arrow", damage);
                Destroy(gameObject);
                break;
            default:

                break;
        }
    }

    private void PlayHitFeedback(Collision col)
    {
        if (hitEffect != null)
        {
            ContactPoint contact = col.GetContact(0);
            Instantiate(hitEffect, contact.point, Quaternion.LookRotation(contact.normal));
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.Play(hitSound);
        }
    }
}
