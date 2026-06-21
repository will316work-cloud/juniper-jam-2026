using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TaskManager))]
public class TaskManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tests");

        TaskManager myScript = (TaskManager)target;
        if (GUILayout.Button("Start task system"))
        {
            myScript.RestartTaskSystem();
        }

        GUI.enabled = true;
    }
}