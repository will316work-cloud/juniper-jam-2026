using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CoworkerManager))]
public class CoworkerManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("TESTS");

        CoworkerManager myScript = (CoworkerManager)target;

        if (GUILayout.Button("Move random coworker"))
            myScript.MoveRandomWorker();
        if(GUILayout.Button("Stop moving coworkers"))
            myScript.StopCoworkerMovement();
        if(GUILayout.Button("Resume moving coworkers"))
            myScript.ContinueMovingCoworkersMovement();
        if(GUILayout.Button("Reset system"))
            myScript.ResetSystem();
        if(GUILayout.Button("Teleport coworkers to original place"))
            myScript.TeleportCoworkersToOriginalPlace();
        if(GUILayout.Button("Trigger coworker mover"))
            myScript.SetCoworkerMoverState(!myScript.IsCoworkerMoverActive);

    }   
}