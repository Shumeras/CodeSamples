using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DialogSystem
{
	[NodeTint("#CCAA11")]
	public class InputDialogNode : SimpleDialogNode  {

		[Output(connectionType = ConnectionType.Override)] public Empty success;
		[Output(connectionType = ConnectionType.Override)] public Empty fail;

		public bool synonims = false;

		public List<string> whitelist;
		public List<string> blacklist;

		public string LastInput { get; private set; }


		public BaseDialogNode GetNext(string value)
		{
			if(value == null) return null;

			LastInput = value;
			value = value.ToLower();
			
			foreach(var word in blacklist)
			{
				if(value.Contains(word.ToLower()))
					return GetOutputPort("fail").Connection.node as BaseDialogNode;
			}
			
			if(synonims)
			{
				foreach(var word in whitelist)
					if(value.Contains(word.ToLower())) 
						return GetOutputPort("success").Connection.node as BaseDialogNode;
			}
			else
			{
				foreach(var word in whitelist)
					if(!value.Contains(word.ToLower()))
						return GetOutputPort("fail").Connection.node as BaseDialogNode;
			}

			return GetOutputPort("success").Connection.node as BaseDialogNode;
		}

		public override BaseDialogNode GetNext(int number = 0)
		{
			Debug.LogWarning("GetNext(int) should not be called for nodes of type Input. Use GetNext(string)");
			return GetNext(LastInput ?? "");
		}
		
	}
}