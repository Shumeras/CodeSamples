using System;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace DialogSystem
{
    public class SimpleDialogNode : BaseDialogNode{

		public Color prefixColor = Color.white;
		public string prefix; 
		[TextArea] [SerializeField] private string text = "";

		public Sprite leftImage, rightImage;

		[Input] public Empty prev;
		[Output(connectionType = ConnectionType.Override)] private Empty next;

        public override string Text => text;

		public override BaseDialogNode GetNext(int number = 0)
		{
			if(GetOutputPort("next").IsConnected)
				return  (GetOutputPort("next").Connection.node as BaseDialogNode);
			else 
				return null;
		}

		public override BaseDialogNode GetPrev()
		{
			if(GetInputPort("prev").IsConnected)
				return  (GetInputPort("prev").Connection.node as BaseDialogNode);
			else 
				return null;
		}

	}

}