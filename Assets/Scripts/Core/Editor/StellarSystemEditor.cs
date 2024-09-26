using System;
using System.Collections;
using System.Collections.Generic;
using Corruption.Astro;
using Corruption.Astro.Bodies;
using UnityEditor;
using UnityEngine;

namespace Corruption.Core.Editor
{
    [CustomEditor(typeof(StarSystem))]
    public class StellarSystemEditor : UnityEditor.Editor
    {
        private SerializedProperty m_echoSourceDistance;
        private SerializedProperty m_echoSourceSize;
        private SerializedProperty m_echoSourceColor;
        
        private SerializedProperty m_systemName;
        private SerializedProperty m_starList;

        private GUIStyle m_headerStyle;
        private GUIStyle m_sectionStyle;
        private GUIStyle m_centeredStyle;
        
        private void OnEnable()
        {
            m_echoSourceDistance = serializedObject.FindProperty("m_sourceDistance");
            m_echoSourceSize = serializedObject.FindProperty("m_sourceSize");
            m_echoSourceColor = serializedObject.FindProperty("m_sourceColor");
            
            m_systemName = serializedObject.FindProperty("m_name");
            m_starList = serializedObject.FindProperty("m_stars");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SetupStyles();
            
            // -- Header -- 
            EditorGUILayout.LabelField("Stellar System", m_headerStyle);
            DrawSeparator(Color.gray);
            
            // -- Name --
            EditorGUILayout.LabelField("System Name", m_sectionStyle);
            EditorGUILayout.Space();
            m_systemName.stringValue = EditorGUILayout.TextField(m_systemName.stringValue);
            DrawSeparator(Color.gray);
            
            // -- Echo Source --
            DrawEchoSourceDetails();
            
            // -- Stars --
            EditorGUILayout.LabelField("Stars", m_sectionStyle);
            DrawSeparator(Color.gray);

            // Display each SystemBody element without the "Element X" label
            for (int i = 0; i < m_starList.arraySize; i++)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                SerializedProperty starList = m_starList.GetArrayElementAtIndex(i);
                SerializedProperty systemBody = starList.FindPropertyRelative("m_body");
                
                DrawArrayLabel("Star: " + (i + 1), systemBody);
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(systemBody, GUIContent.none);
                if (systemBody.objectReferenceValue != null)
                {
                    StellarBody stellarBody = (StellarBody)systemBody.objectReferenceValue;
                    if (stellarBody.Type != StellarBodyType.STAR)
                    {
                        EditorUtility.DisplayDialog("Invalid Type", "This Stellar Body isn't a Star", "OK");
                        systemBody.objectReferenceValue = null;
                    }
                }
                
                if (GUILayout.Button("Remove Star"))
                {
                    m_starList.arraySize--;
                    serializedObject.ApplyModifiedProperties();
                }
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Size ");
                    SerializedProperty bodySize = starList.FindPropertyRelative("m_bodyScale");
                    bodySize.floatValue = EditorGUILayout.FloatField(bodySize.floatValue);
                EditorGUILayout.EndHorizontal();
                
                SerializedProperty orbitingBodies = starList.FindPropertyRelative("m_orbitingBodies");
                if (GUILayout.Button("Add Body"))
                    AddToArray(orbitingBodies);
                
                if (orbitingBodies.arraySize > 0)
                {
                    EditorGUILayout.LabelField("Orbiting Bodies", m_sectionStyle);
                    DrawSeparator(Color.gray);
                    EditorGUI.indentLevel++;
                    
                    for (int j = 0; j < orbitingBodies.arraySize; j++)
                    {
                        SerializedProperty orbitingBody = orbitingBodies.GetArrayElementAtIndex(j).FindPropertyRelative("m_body");
                        SerializedProperty satelliteBodies = orbitingBodies.GetArrayElementAtIndex(j).FindPropertyRelative("m_orbitingBodies");
                        
                        DrawArrayLabel("Body: " + (j + 1), orbitingBody);
                        
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(orbitingBody, GUIContent.none);
                        
                        if (GUILayout.Button("Add Satellite"))
                            AddToArray(satelliteBodies);
                        
                        if (GUILayout.Button("Remove Body"))
                            RemoveFromArray(orbitingBodies);
                        
                        EditorGUILayout.EndHorizontal();

                        DrawSystemBodyDetails(orbitingBodies.GetArrayElementAtIndex(j), j);
                        
                        DrawSatellites(satelliteBodies);
                        DrawSeparator(Color.gray);
                    }

                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                }
                
                EditorGUILayout.EndVertical();
                
                DrawSeparator(Color.gray);
            }

            if (GUILayout.Button("Add Star"))
            {
                m_starList.arraySize++;
                serializedObject.ApplyModifiedProperties();
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawEchoSourceDetails()
        {
            EditorGUILayout.LabelField("Echo Source Details", m_sectionStyle);
            DrawSeparator(Color.gray);
            
            GUILayout.BeginHorizontal();
            m_echoSourceDistance.intValue = EditorGUILayout.IntField(m_echoSourceDistance.intValue);
            EditorGUILayout.LabelField("Distance (LY)");
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            m_echoSourceSize.floatValue = EditorGUILayout.FloatField(m_echoSourceSize.floatValue);
            EditorGUILayout.LabelField("Size");
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            m_echoSourceColor.colorValue = EditorGUILayout.ColorField(m_echoSourceColor.colorValue);
            EditorGUILayout.LabelField("Hologram Color");
            GUILayout.EndHorizontal();
            
            DrawSeparator(Color.gray);
            EditorGUILayout.Space();
        }

        private void DrawSystemBodyDetails(SerializedProperty body, int index)
        {
            SerializedProperty distanceFromPrimary = body.FindPropertyRelative("m_distanceFromPrimary");
            SerializedProperty bodySize = body.FindPropertyRelative("m_bodyScale");

            DrawSystemBodyOrbitParameters(body);
            
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Distance From Primary");
                if (distanceFromPrimary.floatValue == 0) { distanceFromPrimary.floatValue = index + 1; }
                distanceFromPrimary.floatValue = EditorGUILayout.FloatField(distanceFromPrimary.floatValue);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Size");
                bodySize.floatValue = EditorGUILayout.FloatField(bodySize.floatValue);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawSystemBodyOrbitParameters(SerializedProperty body)
        {
            SerializedProperty orbitParameters = body.FindPropertyRelative("m_orbitParameters");
            //SerializedProperty orbitSpeed = orbitParameters.FindPropertyRelative("m_orbitSpeed");
            //SerializedProperty radiusCurve = orbitParameters.FindPropertyRelative("m_radiusCurve");
            //SerializedProperty angleCurve = orbitParameters.FindPropertyRelative("m_angleCurve");

            EditorGUILayout.PropertyField(orbitParameters);
        }

        private void DrawSatellites(SerializedProperty property)
        {
            if (property.isArray && property.arraySize > 0)
            {
                EditorGUILayout.BeginVertical();
                //EditorGUILayout.LabelField("Satellites", m_sectionStyle);
                EditorGUILayout.Space();
                EditorGUI.indentLevel++;

                for (int i = 0; i < property.arraySize; i++)
                {
                    SerializedProperty orbitingBody = property.GetArrayElementAtIndex(i).FindPropertyRelative("m_body");
                    
                    DrawArrayLabel("Satellite: " + (i + 1), orbitingBody);
                    
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(orbitingBody, GUIContent.none);
                        
                    if (GUILayout.Button("Remove Satellite"))
                        RemoveFromArray(property);
                        
                    EditorGUILayout.EndHorizontal();
                }
                
                EditorGUILayout.EndVertical();
            }
        }

        private void AddToArray(SerializedProperty property)
        {
            property.arraySize++;
            SerializedProperty newProperty = property.GetArrayElementAtIndex(property.arraySize - 1).FindPropertyRelative("m_body");
            newProperty.objectReferenceValue = null;
            
            serializedObject.ApplyModifiedProperties();
        }

        private void RemoveFromArray(SerializedProperty property)
        {
            property.arraySize--;
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawArrayLabel(string initialString, SerializedProperty property)
        {
            string labelName = initialString;
            if (property.objectReferenceValue != null)
            {
                labelName = property.objectReferenceValue.name.Replace("_", " ");
            }
            
            EditorGUILayout.LabelField(labelName, m_sectionStyle);
        }
        
        private void DrawSeparator(Color color)
        {
            EditorGUILayout.Space();
            
            GUIStyle separatorStyle = new GUIStyle(GUI.skin.box);
            separatorStyle.normal.background = EditorGUIUtility.whiteTexture;
            separatorStyle.margin = new RectOffset(0, 0, 4, 4);
            EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 1), color);
            
            EditorGUILayout.Space();
        }

        private void SetupStyles()
        {
            m_headerStyle = new GUIStyle(EditorStyles.boldLabel);
            m_headerStyle.alignment = TextAnchor.MiddleCenter;
            m_headerStyle.fontSize = 20;
            
            m_sectionStyle = new GUIStyle(EditorStyles.boldLabel);
            m_sectionStyle.alignment = TextAnchor.MiddleCenter;
            m_sectionStyle.fontSize = 14;
            
            m_centeredStyle = new GUIStyle(GUI.skin.box);
            m_centeredStyle.alignment = TextAnchor.MiddleCenter;
        }
    }
}
