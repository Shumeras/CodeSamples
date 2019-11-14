using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;
using XNode;
using XNodeEditor;
using UnityEditor;

namespace DialogSystem
{      
    [CustomNodeEditor(typeof(ConditionalDialogNode))]
    public class ConditionalDialogNodeEditor : NodeEditor
    {
        protected static float textFieldSpace;

        public override void OnCreate()
        {
            base.OnCreate();
            textFieldSpace = EditorGUIUtility.labelWidth;
        }

        public override int GetWidth()
        {
            return base.GetWidth() +50;
        }
        public override void OnHeaderGUI() {

            GUILayout.Label("CONDITION", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI() 
        {
            serializedObject.Update();

            GUILayout.BeginHorizontal();
            {
                NodeEditorGUILayout.PortField(new GUIContent("Prev"), target.GetInputPort("prev"), GUILayout.Width(30));
                GUILayout.BeginVertical();
                {
                    NodeEditorGUILayout.PortField(new GUIContent("Success"), target.GetOutputPort("success"));
                    NodeEditorGUILayout.PortField(new GUIContent("Fail"), target.GetOutputPort("fail"));
                }
                GUILayout.EndVertical();              
            }
            GUILayout.EndHorizontal();

            //EditorGUIUtility.labelWidth = 80;
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("conditions"), true, GUILayout.Width(200));
            //EditorGUIUtility.labelWidth = textFieldSpace;
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}