using XNode;

namespace DialogSystem
{
	[NodeTint("#F06666")]
	public class RootDialogNode : BaseDialogNode {

		[Output(connectionType = ConnectionType.Override)] public Empty next;

        public override string Text => "";
	
		public override BaseDialogNode GetNext(int number = 0)
		{
			if(GetOutputPort("next").IsConnected)
				return  (GetOutputPort("next").Connection.node as BaseDialogNode);
			else 
				return null;
		}

	}

}