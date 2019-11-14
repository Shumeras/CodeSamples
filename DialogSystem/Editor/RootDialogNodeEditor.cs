using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace DialogSystem
{
	[CustomNodeEditor(typeof(RootDialogNode))]
	public class RootDialogNodeEditor : NodeEditor {

		private static GUIStyle headerStyle;

		public override void AddContextMenuItems(GenericMenu menu)
		{
			if (Selection.objects.Length == 1 && Selection.activeObject is XNode.Node) {
                XNode.Node node = Selection.activeObject as XNode.Node;
                menu.AddItem(new GUIContent("Move To Top"), false, () => NodeEditorWindow.current.MoveNodeToTop(node));
            }
		}

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
			GUILayout.Label("ROOT", headerStyle);
		}

		public override void OnBodyGUI()
		{
			GUILayout.Space(5);
			base.OnBodyGUI();
		}

	}
}