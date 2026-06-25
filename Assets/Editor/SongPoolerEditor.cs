using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PoolManager))]
public class PoolManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tests");
        PoolManager myScript = (PoolManager)target;
        if (GUILayout.Button("Fade to next song"))
        {
            myScript.FadeInGameplayMusic();
        }
    }
}