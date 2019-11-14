using XNode;

namespace DialogSystem
{

    public abstract class BaseDialogNode : Node {

		public virtual string Text {get => "";}

		public virtual BaseDialogNode GetNext(int number = 0) {return null;}
		public virtual BaseDialogNode GetPrev() {return null;}

		///For base/simple dialog node types return Text value regardles of input;
		public override object GetValue(NodePort port)
		{
			return Text;
		}

	}
}