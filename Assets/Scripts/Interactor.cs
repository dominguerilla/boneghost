using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles interacting with whateever's under the mouse cursor.
/// </summary>
public class Interactor : MonoBehaviour
{
    public float maxInteractionDistance = 2f;
    public float maxDropDistance = 2f;

    [SerializeField] Arm[] arms;
    [SerializeField] Camera cam;
    Vector3[] originalArmPositions;
    InventoryComponent inventory;

    Vector3 surfaceUnderCursor;

    private void Awake()
    {
        inventory = GetComponent<InventoryComponent>();
        originalArmPositions = new Vector3[arms.Length];
        for (int i = 0; i < arms.Length; i++)
        {
            originalArmPositions[i] = arms[i].transform.localPosition;
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 10, 10), "");
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseInput();
    }

    private void FixedUpdate()
    {
        surfaceUnderCursor = GetSurfaceUnderCursor();
    }

    void GetMouseInput()
    {
        Vector3 direction = cam.transform.forward;
        if (arms.Length <= 0) return;
        if (Input.GetMouseButtonDown(0))
        {
            arms[0].transform.position += direction;
            if (Input.GetKey(KeyCode.E))
            {
                arms[0].Drop(surfaceUnderCursor);
            }
            else
            {
                bool gotItem = UseArm(arms[0], Vector3.zero);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            arms[0].DropIfTemporary(surfaceUnderCursor);
            arms[0].transform.localPosition = originalArmPositions[0];
        }

        if (Input.GetMouseButtonDown(1))
        {
            arms[1].transform.position += direction;
            if (Input.GetKey(KeyCode.E))
            {
                arms[1].Drop(surfaceUnderCursor);
            }
            else
            {
                Vector3 flippedEulerAngles = new Vector3(0f, -90f, 0f);
                bool gotItem = UseArm(arms[1], flippedEulerAngles);
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            arms[1].DropIfTemporary(surfaceUnderCursor);
            arms[1].transform.localPosition = originalArmPositions[1];
        }
    }

    bool UseArm(Arm arm, Vector3 eulerAngleOffset)
    {
        if (arm.IsHoldingItem())
        {
            arm.UseItem();
            return false;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxInteractionDistance))
        {
            Transform objectHit = hit.transform;

            ItemComponent item = objectHit.GetComponent<ItemComponent>();
            if (item)
            {
                arm.Hold(item, eulerAngleOffset);
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if(Application.isPlaying)  Debug.DrawLine(cam.transform.position, surfaceUnderCursor, Color.red);
    }

    Vector3 GetSurfaceUnderCursor()
    {
        RaycastHit hit;
        Ray cameraForward = GetCameraForward();
        if (Physics.Raycast(cameraForward, out hit, maxDropDistance))
        {
            return hit.point;
        }
        return GetCameraForwardVector();
    }

    Ray GetCameraForward() {
        return cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, maxDropDistance));
    }

    Vector3 GetCameraForwardVector() {
        return cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, maxDropDistance));
    }
}
