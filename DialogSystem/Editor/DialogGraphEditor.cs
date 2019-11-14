using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace DialogSystem
{
    
    [CustomNodeGraphEditor(typeof(Dialog))]
    public class DialogGraphEditor : NodeGraphEditor { 

        public override string GetNodeMenuName(System.Type type) {
			if (type.Namespace == "DialogSystem" && type != typeof(RootDialogNode))
				return base.GetNodeMenuName(type);
            else 
                return null;
        }

        public override void OnCreate()
        {
            Dialog dialog = (target as Dialog);
            if(dialog.RootNode == null)
            {
                
            }
        }

    }

}