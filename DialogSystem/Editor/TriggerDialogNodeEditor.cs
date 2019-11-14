using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;
using XNode;
using XNodeEditor;
using UnityEditor;

namespace DialogSystem
{      
    [CustomNodeEditor(typeof(TriggerDialogNode))]
    public class TriggerDialogNodeEditor : NodeEditor
    {
        
        protected static float textFieldSpace;

        public override void OnCreate()
        {
            base.OnCreate();
            textFieldSpace = EditorGUIUtility.labelWidth;
        }
        
        public override int GetWidth()
        {
            return base.GetWidth()+100;
        }

        public override void OnHeaderGUI() 
        {
            GUILayout.Label("TRIGGER", NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI() 
        {
            serializedObject.Update();

            GUILayout.BeginHorizontal();
            {
                NodeEditorGUILayout.PortField(new GUIContent("Prev"), target.GetInputPort("prev"), GUILayout.Width(30));
                NodeEditorGUILayout.PortField(new GUIContent("Next"), target.GetOutputPort("next"));

            }

            GUILayout.EndHorizontal();

           // EditorGUIUtility.labelWidth = 80;
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("trigger"));
         //   EditorGUIUtility.labelWidth = textFieldSpace;

            serializedObject.ApplyModifiedProperties();
        }
    }
}