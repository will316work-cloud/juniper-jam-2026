using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("TESTS");

        CameraController myScript = (CameraController)target;
        if (GUILayout.Button("Shake Current Camera"))
            myScript.ShakeCamera();
        if(GUILayout.Button("Switch to Game camera"))
            myScript.SwitchToGameCamera(CameraType.Gameplay);
        if(GUILayout.Button("Switch to Menu camera"))
            myScript.SwitchToGameCamera(CameraType.Menu);
        
    }
}