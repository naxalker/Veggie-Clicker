using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(DailyReward))]
public class DailyRewardPD : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        EditorGUI.LabelField(position, property.displayName);

        EditorGUILayout.PropertyField(property.FindPropertyRelative("RewardType"));
        EditorGUILayout.PropertyField(property.FindPropertyRelative("Amount"));
        EditorGUILayout.PropertyField(property.FindPropertyRelative("Icon"));

        if (property.FindPropertyRelative("RewardType").enumValueIndex == (int)DailyRewardType.Upgrade)
            EditorGUILayout.PropertyField(property.FindPropertyRelative("UpgradeIndex"));
    }
}

#endif