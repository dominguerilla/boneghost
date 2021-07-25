using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float flightSpeed = 3f;
    public float damage = 1.0f;
    public int maxNumberOfEntitiesDamaged = 1;

    public IEnumerator Launch(Vector3 origin, Quaternion orientation, float lifetime)
    {
        transform.position = origin;
        transform.rotation = orientation;
        this.gameObject.SetActive(true);

        float currentLifetime = 0f;
        while (currentLifetime < lifetime)
        {
            currentLifetime += Time.deltaTime;
            transform.Translate(Vector3.forward * flightSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
        this.gameObject.SetActive(false);
    }
}
