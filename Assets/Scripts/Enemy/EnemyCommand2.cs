using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyCommand2
{
    public enum ForkingNode { Selector, Sequence }
    public ForkingNode m_forkingNode;
    [SerializeReference, SubclassSelector]
    public List<ICommand2> m_command2s;

    public bool Command()
    {
        switch (m_forkingNode)
        {
            case ForkingNode.Selector:
                if (m_command2s[0].Execute())
                {
                    //•Ð•ûTrue‚¾‚Á‚½‚çTrue
                    return true;
                }
                return false;
            case ForkingNode.Sequence:
                if (m_command2s[0].Execute())
                {
                    //false‚ª‹A‚Á‚½Žž“_‚Åfalse‚ð•Ô‚·
                    return true;
                }
                return false;
            default:
                return false;
        }
    }
}
