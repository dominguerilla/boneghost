using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageEvent : UnityEvent<Projectile>
{

}

public class Damageable : MonoBehaviour
{
    public DamageEvent onProjectileHit = new DamageEvent();

    [SerializeField] AudioClip destroySound;
    [SerializeField] ParticleSystem destroyParticles;
    AudioSource src;
    Renderer render;
    Collider objCollider;

    void Start()
    {
        src = GetComponent<AudioSource>();
        render = GetComponent<MeshRenderer>();
        objCollider = GetComponent<Collider>();
    }

    void DestroyObject()
    {
        src.PlayOneShot(destroySound);
        destroyParticles.Play();
        render.enabled = false;
        objCollider.enabled = false;
        Destroy(this.gameObject, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile)
        {
            onProjectileHit.Invoke(projectile);
        }
    }
}
