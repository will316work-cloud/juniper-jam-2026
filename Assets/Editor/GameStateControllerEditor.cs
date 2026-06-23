using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameStateController))]
public class GameStateControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GameStateController myScript = (GameStateController)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tests");
        
        if (GUILayout.Button("Trigger Game Over State"))
            myScript.ChangeState(StateType.GameOver);
        
    }
}