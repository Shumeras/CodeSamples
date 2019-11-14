using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DialogSystem
{	
	[NodeTint("#329832")]
	public class SelectionDialogNode : SimpleDialogNode{

		[Output(dynamicPortList = true, connectionType = ConnectionType.Override)] 
		public List<string> selections;

		public override BaseDialogNode GetNext(int number = 0)
		{
			if(selections.Count < 1 || number >= selections.Count) return null;

			NodePort port = GetOutputPort("selections " + number);

			if (port == null || !port.IsConnected) return null;

			return  port.Connection.node as BaseDialogNode;
		}

	}

}