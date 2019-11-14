using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;
using XNode;
using XNodeEditor;


namespace DialogSystem
{
    [CustomNodeEditor(typeof(InputDialogNode))]    
    public class InputDialogNodeEditor : SimpleDialogNodeEditor
    {

        public override int GetWidth()
        {
            return base.GetWidth() + 80;
        }

        public override void OnBodyGUI() {
            
            serializedObject.Update();

            GUILayout.BeginHorizontal();
            {
                NodeEditorGUILayout.PortField(new GUIContent("Prev"), target.GetInputPort("prev"), GUILayout.Width(30));
                 
                DrawTextSupplements();   
                
                GUILayout.BeginVertical(GUILayout.Width(55));
                {
                    NodeEditorGUILayout.PortField(new GUIContent("Success"), target.GetOutputPort("success"),GUILayout.Width(55));
                    NodeEditorGUILayout.PortField(new GUIContent("Fail"), target.GetOutputPort("fail"), GUILayout.Width(55));
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("text"), new GUIContent("Text"), false);
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("synonims"));

            GUILayout.BeginHorizontal();
            {
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("blacklist"), true, GUILayout.Width(100));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("whitelist"), true, GUILayout.Width(10));
            }
            GUILayout.EndHorizontal();
            
            // Apply property modifications
            serializedObject.ApplyModifiedProperties();
        }

    }

}