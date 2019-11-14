using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XNode;

namespace DialogSystem
{

	[NodeTint("#5555FF")]
    public class TriggerDialogNode : BaseDialogNode
    {
        public override string Text => "";

		[Input] public Empty prev;
		[Output(connectionType = ConnectionType.Override)] public Empty next;

		public UnityEvent trigger;

        public override BaseDialogNode GetNext(int number = 0)
        {   
            trigger.Invoke();
            return GetOutputPort("next").Connection.node as BaseDialogNode;
        }
        
    }

}