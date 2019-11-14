using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace DialogSystem
{
    [CustomNodeEditor(typeof(RandomizeDialogNode))]
    public class RandomizeDialogNodeEditor : NodeEditor
    {
        private static GUIStyle headerStyle;
		
        public override void OnCreate()
        {
            base.OnCreate();
            if(headerStyle == null)
			{
				headerStyle = new GUIStyle(EditorStyles.label);
				headerStyle.alignment = TextAnchor.LowerCenter;
				headerStyle.normal.textColor = Color.white;
				headerStyle.fontStyle = FontStyle.Bold;
			}
        }

		public override int GetWidth()
		{
			return 100;
		}

		public override void OnHeaderGUI()
		{
			GUILayout.Space(5);
			GUILayout.Label("RANDOM", headerStyle);
		}

		public override void OnBodyGUI()
		{
			GUILayout.Space(5);
            
			    base.OnBodyGUI();
            
		}
    }
}
