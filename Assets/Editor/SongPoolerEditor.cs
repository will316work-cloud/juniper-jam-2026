using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BootStrapper))]
public class SongPoolerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tests");
        BootStrapper bootStrapper = (BootStrapper)target;
        if (GUILayout.Button("Fade to next song"))
        {
            bootStrapper.GameContext.SongPooler.FadeToNextSong();
        }
    }
}