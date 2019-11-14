using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DialogSystem
{
	[NodeTint("#2A2A2A")]
    public class RandomizeDialogNode : BaseDialogNode
    {
		[Input] public Empty prev;
		[Output] public Empty next;

		public override BaseDialogNode GetNext(int number = 0)
		{
			var port = GetOutputPort("next");

			if(port == null || port.ConnectionCount < 1)
				return null;	

			return port.GetConnection(Random.Range(0, port.ConnectionCount)).node as BaseDialogNode;

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