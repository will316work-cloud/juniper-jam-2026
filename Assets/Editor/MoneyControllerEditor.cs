using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MoneyController))]
public class MoneyControllerEditor : Editor
{
    int _amountToGain = 0;
    int _amountToLose = 0;
    int _amountToSpend = 0;

    bool _foldout;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MoneyController moneyController = (MoneyController)target;

        _foldout = EditorGUILayout.Foldout(_foldout, "Testing", true, EditorStyles.foldoutHeader);
        if (!_foldout) return;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Testing", EditorStyles.boldLabel);

        GUI.enabled = Application.isPlaying;

        _amountToGain = EditorGUILayout.IntField("Amount to Gain", _amountToGain);
        if (GUILayout.Button("Gain Money"))
        {
            moneyController.GainMoney(_amountToGain);
        }

        _amountToLose = EditorGUILayout.IntField("Amount to Lose", _amountToLose);
        if (GUILayout.Button("Lose Money"))
        {
            moneyController.LoseMoney(_amountToLose);
        }

        _amountToSpend = EditorGUILayout.IntField("Amount to Spend", _amountToSpend);
        if (GUILayout.Button("Spend Money"))
        {
            moneyController.SpendMoney(_amountToSpend);
        }

        GUI.enabled = true;
    }
}