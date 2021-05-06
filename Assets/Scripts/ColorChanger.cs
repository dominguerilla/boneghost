using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity), typeof(MeshRenderer))]
public class ColorChanger : MonoBehaviour
{
    public Material[] materials;

    int _currentIndex = -1;

    MeshRenderer meshRender;

    Entity entity;
    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity>();
        meshRender = GetComponent<MeshRenderer>();

        ChangeColor(null);
        entity.onInteract.AddListener(ChangeColor);
    }

    void ChangeColor(GameObject caller)
    {
        _currentIndex = (_currentIndex+1) % materials.Length;
        meshRender.material = materials[_currentIndex];
    }
}
