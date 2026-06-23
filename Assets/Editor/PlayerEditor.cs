using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerControl))]
public class PlayerControlEditor : Editor {
    public override void OnInspectorGUI() 
    {
        DrawDefaultInspector();
        PlayerControl myScript = (PlayerControl)target;  

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("TESTS", EditorStyles.boldLabel);
        if (GUILayout.Button("Teleport player to starting position")) myScript.TeleportPlayerToStartingPosition();  
    }
}