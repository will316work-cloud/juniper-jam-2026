using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TaskManager))]
public class TaskManagerEditor : Editor
{
    bool _foldout;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        _foldout = EditorGUILayout.Foldout(_foldout, "Testing", true, EditorStyles.foldoutHeader);
        if (!_foldout) return;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tests");

        TaskManager myScript = (TaskManager)target;
        if (GUILayout.Button("On Task Complete"))
        {
            myScript.OnTaskSuccess();
        }

        GUI.enabled = true;
    }
}