using System.Collections;
using System.Collections.Generic;
using Corruption.Core.Framework;
using UnityEditor;
using UnityEngine;

namespace Corruption.Core.Editor
{
    [CustomPropertyDrawer(typeof(MinMaxRange))]
    public class MinMaxRangeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty minValue = property.FindPropertyRelative("m_min");
            SerializedProperty maxValue = property.FindPropertyRelative("m_max");
            
            var labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
            var minRect = new Rect(position.x + EditorGUIUtility.labelWidth - 30, position.y, (position.width - EditorGUIUtility.labelWidth) / 2, position.height);
            var maxRect = new Rect(position.x + (EditorGUIUtility.labelWidth - 20) + (position.width - EditorGUIUtility.labelWidth) / 2, position.y, (position.width - EditorGUIUtility.labelWidth) / 2, position.height);
            
            // Draw the label
            EditorGUI.LabelField(labelRect, label + ":");

            // Draw the fields
            EditorGUI.PropertyField(minRect, minValue, GUIContent.none);
            EditorGUI.PropertyField(maxRect, maxValue, GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
}
