using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace DialogSystem
{
    [CustomNodeEditor(typeof(SelectionDialogNode))]
    public class SelectionDialogNodeEditor : SimpleDialogNodeEditor
    {

        public override void OnBodyGUI() {
            
            serializedObject.Update();

            GUILayout.BeginHorizontal();
            {
                NodeEditorGUILayout.PortField(new GUIContent("Prev"), target.GetInputPort("prev"), GUILayout.Width(30));
                 
                DrawTextSupplements();   

            }
            GUILayout.EndHorizontal();
            
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("text"), new GUIContent("Text"), false);

            NodeEditorGUILayout.DynamicPortList("selections", typeof(string), serializedObject, NodePort.IO.Output, Node.ConnectionType.Override);

            // Apply property modifications
            serializedObject.ApplyModifiedProperties();
        }
    }

}