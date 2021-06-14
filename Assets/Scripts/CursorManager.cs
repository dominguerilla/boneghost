using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorImage;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if(cursorImage){
            Vector2 hotspot = new Vector2(cursorImage.width / 2, cursorImage.height / 2);
            Cursor.SetCursor(cursorImage, hotspot, CursorMode.Auto);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("escape")){
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
