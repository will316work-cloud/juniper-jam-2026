using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UiManager))]
public class UiManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("TESTS");

        UiManager myScript = (UiManager)target;

        EditorGUILayout.LabelField("In Game Menu Panel");
        if (GUILayout.Button("Trigger ingame Panel"))
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

        EditorGUILayout.LabelField("Game Over Panel");
        if (GUILayout.Button("Trigger Game Over Panel"))
        {
            if(myScript.GameOverUiHandler.IsPanelActive())
                myScript.GameOverUiHandler.SetPanelState(false);
            else 
                myScript.GameOverUiHandler.SetPanelState(true);
        }

        EditorGUILayout.LabelField("In Game Menu Panel");
        if (GUILayout.Button("Trigger ingame Panel"))
        {
            if(myScript.IngameMenuHandler.IsPanelActive())
                myScript.IngameMenuHandler.SetPanelState(false);
            else 
                myScript.IngameMenuHandler.SetPanelState(true);
        }
    }   
}