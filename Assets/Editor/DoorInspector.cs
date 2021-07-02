using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Door))]
public class DoorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Door door = (Door)target;
        if (GUILayout.Button("Unlock/Lock Door"))
        {
            door.ToggleLock();
        }
    }
}
