
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TransitionController))]
public class TransitionControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("TESTS");

        TransitionController myScript = (TransitionController)target;

        if (GUILayout.Button("Start Transition"))
            myScript.StartCoroutine(myScript.TransitionFadeIn(""));

        if(GUILayout.Button("Stop Transition"))
            myScript.StartCoroutine(myScript.TransitionFadeOut());
    }
}