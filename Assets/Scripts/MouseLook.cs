using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls FPS style mouselook.
/// Should be attached to the Camera component of a child of the character's point of view.
/// https://github.com/jiankaiwang/FirstPersonController
/// </summary>
public class MouseLook : MonoBehaviour
{

    public float sensitivity = 5f;
    public float smoothing = 2f;

    public GameObject character;

    private Vector2 mouseLook;
    private Vector2 smoothV;

    // Start is called before the first frame update
    void Start()
    {
        character = this.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));

        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f/smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f/smoothing);
        mouseLook += smoothV;

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
    }
}
