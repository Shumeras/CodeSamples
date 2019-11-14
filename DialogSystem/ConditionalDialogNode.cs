using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XNode;

namespace DialogSystem
{
	[NodeTint("#D03939")]
	public class ConditionalDialogNode : BaseDialogNode {
        
		public override string Text => "";

		public CheckableEvent conditions;

		[Input]  public Empty prev;
		[Output(connectionType = ConnectionType.Override)] public Empty success;
		[Output(connectionType = ConnectionType.Override)] public Empty fail;

		public override BaseDialogNode GetNext(int number = 0)
		{
			NodePort port = GetOutputPort("success");
			conditions.Invoke();
			if(!conditions.Check())
			{
				port = GetOutputPort("fail");
			}

			return port.Connection.node as BaseDialogNode; 
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