using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DayTimeController))]
public class DayTimeControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("TESTS");

        DayTimeController myScript = (DayTimeController)target;
        if (GUILayout.Button("Trigger panel state"))
        {
            myScript.SetPanelState(!myScript.IsPanelActive);
        }
        if (GUILayout.Button("Trigger timer state"))
        {
            if (myScript.IsTimerOn)
                myScript.SetIsTimerOn(false);
            else
                myScript.SetIsTimerOn(true);
        }
        if (GUILayout.Button("Move to next day"))
        {
            myScript.GoNextDay();
        }

    }   
}