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
            myScript.FadeInGameplayMusic();
        if (GUILayout.Button("Fade to Main Menu song"))
            myScript.FadeInMenuMusic();
        if (GUILayout.Button("Fade to Game Over song"))
            myScript.FadeInGameOverMusic();
        if (GUILayout.Button("Fade to Day Change song"))
            myScript.FadeInDayChangeMusic();
        

    }
}