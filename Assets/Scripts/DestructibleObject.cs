using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructible 
{
    void OnAttacked(GameObject attacker);
}

public class DestructibleObject : MonoBehaviour, IDestructible
{
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
    public void OnAttacked(GameObject attacker)
    {
        DestroyObject();
    }

    void DestroyObject()
    {
        src.PlayOneShot(destroySound);
        destroyParticles.Play();
        render.enabled = false;
        objCollider.enabled = false;
        Destroy(this.gameObject, 1);
    }
}
