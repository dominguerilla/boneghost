using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public enum MOVEMENT_TYPE
    {
        BACK_AND_FORTH
    }

    public Vector3 moveSpeed;
    public float maxDistance = 10f;

    Vector3 _startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;
        StartCoroutine(MoveBackAndForth(_startingPosition, moveSpeed, maxDistance));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveBackAndForth(Vector3 startPosition, Vector3 moveSpeed, float maxDistance)
    {
        int flip = 1;

        while (true)
        {
            transform.Translate(Time.deltaTime * moveSpeed.x * flip, Time.deltaTime * moveSpeed.y * flip, Time.deltaTime * moveSpeed.z * flip);
            if (Vector3.Distance(startPosition, transform.position) > maxDistance)
            {
                flip *= -1;
            }
            yield return null;
        }
    }
}
