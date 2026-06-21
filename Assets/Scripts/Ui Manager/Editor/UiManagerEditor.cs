using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UiManager))]
public class UiManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tests");

        UiManager myScript = (UiManager)target;

        if (GUILayout.Button("Trigger ingame menu"))
        {
            if(myScript.InGameUiHandler.IsPanelActive())
                myScript.InGameUiHandler.SetPanelState(false);
            else 
                myScript.InGameUiHandler.SetPanelState(true);
        }

        if (GUILayout.Button("Trigger interaction indicator"))
        {
            if(myScript.InGameUiHandler.IndicatorState())
                myScript.InGameUiHandler.SetInteractionIndicatorstate(false);
            else 
                myScript.InGameUiHandler.SetInteractionIndicatorstate(true);
        }
    }   
}