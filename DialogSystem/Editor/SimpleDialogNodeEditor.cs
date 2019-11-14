using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace DialogSystem
{
    
    [CustomNodeEditor(typeof(SimpleDialogNode))]
    public class SimpleDialogNodeEditor : NodeEditor
    {

        protected static Color defaultNodeLabelColor;
        protected static GUIStyle textAreaStyle;
        protected static float textFieldSpace;

        public override int GetWidth()
        {
            return base.GetWidth()+50;
        }

        public override void OnCreate(){
            defaultNodeLabelColor = NodeEditorResources.styles.nodeHeader.normal.textColor;
            textFieldSpace = EditorGUIUtility.labelWidth;
            base.OnCreate();
        }

        public override void OnHeaderGUI() {
    
            NodeEditorResources.styles.nodeHeader.normal.textColor = (target as SimpleDialogNode).prefixColor;

            string label = (target as SimpleDialogNode).prefix;
            if(label == null || label.Length < 1) label = "NODE";

            GUILayout.Label(label, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));

            NodeEditorResources.styles.nodeHeader.normal.textColor = defaultNodeLabelColor;
        }

        public override void OnBodyGUI() {

            serializedObject.Update();

            GUILayout.BeginHorizontal();
            {
                NodeEditorGUILayout.PortField(new GUIContent("Prev"), target.GetInputPort("prev"), GUILayout.Width(30));

                DrawTextSupplements();
                
                NodeEditorGUILayout.PortField(new GUIContent("Next"), target.GetOutputPort("next"), GUILayout.Width(30));
            }
            GUILayout.EndHorizontal();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("text"), new GUIContent("Text"), false);

            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawTextSupplements()
        {

            GUILayout.BeginVertical();
            {       
                EditorGUIUtility.labelWidth = 80;
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("prefixColor"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("prefix"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("leftImage"));
                NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("rightImage"));   
                EditorGUIUtility.labelWidth = textFieldSpace;
            }
            GUILayout.EndVertical();
        }

    }

}