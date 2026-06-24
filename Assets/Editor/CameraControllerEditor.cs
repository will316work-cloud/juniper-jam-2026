using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor
{
    private EffectType _selectedAction = EffectType.WorldHpLoss;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("TESTS");

        _selectedAction = (EffectType)EditorGUILayout.EnumPopup("Action Type", _selectedAction);

        CameraController myScript = (CameraController)target;
        if (GUILayout.Button("Shake Current Camera"))
            myScript.ShakeCamera(_selectedAction);
        if(GUILayout.Button("Switch to Game camera"))
            myScript.SwitchToGameCamera(CameraType.Gameplay);
        if(GUILayout.Button("Switch to Menu camera"))
            myScript.SwitchToGameCamera(CameraType.Menu);
        if(GUILayout.Button("Apply Chromatic Aberration"))
            myScript.ApplyChromaticAberration();
        if(GUILayout.Button("Apply Bloom"))
            myScript.ApplyBloom(_selectedAction);
        if(GUILayout.Button("Disable Depth of Field"))
            myScript.SetIsDepthOfFieldEnabled(false);
        if(GUILayout.Button("Enable Depth of Field"))
            myScript.SetIsDepthOfFieldEnabled(true);
        
    }
}