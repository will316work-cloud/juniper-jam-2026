

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldHealthMeter))]
public class WorldHealthMeterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("TESTS");

        WorldHealthMeter myScript = (WorldHealthMeter)target;

        if (GUILayout.Button("Gain health"))
            myScript.GainHealth(10f,true);

        if (GUILayout.Button("Lose health"))
            myScript.LoseHealth(10f,true);

        if(GUILayout.Button("Trigger Timer"))
            myScript.SetTimerState(!myScript.IsSystemActive);
    }

}