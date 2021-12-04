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
                    //�Е�True��������True
                    return true;
                }
                return false;
            case ForkingNode.Sequence:
                if (m_command2s[0].Execute())
                {
                    //false���A�������_��false��Ԃ�
                    return true;
                }
                return false;
            default:
                return false;
        }
    }
}
