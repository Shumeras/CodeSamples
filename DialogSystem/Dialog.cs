using System;
using UnityEditor;
using UnityEngine;
using XNode;
using DialogSystem;

namespace DialogSystem
{
    [Serializable] 
	public class Empty{}
}

[CreateAssetMenu(fileName = "New Dialog", menuName = "CUSTOM/Dialog")]
public class Dialog : NodeGraph
{
    private BaseDialogNode rootNode;
    public BaseDialogNode RootNode {
        get
        {
            if(rootNode == null)
            {
                rootNode = nodes.Find((n) => {return (n is RootDialogNode);}) as RootDialogNode;
                if(rootNode == null)
                {
                    rootNode = AddNode<RootDialogNode>();
                    rootNode.name = "RootNode";
                    AssetDatabase.AddObjectToAsset(rootNode as RootDialogNode, this);
                    AssetDatabase.Refresh();
                }
                
            }
            return rootNode;
        }   
    }

}
