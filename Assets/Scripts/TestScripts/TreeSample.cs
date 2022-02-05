using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �GAI���O�Ƀr�w�C�r�A�c���[�𗝉����悤�N���X
/// </summary>
public class TreeSample : MonoBehaviour
{
    //�Ƃ肠��������Ă݂�N���X
    //�K�v�Ȃ̂�Node�B�eNode�͑ҋ@���A���s���A�����A���s�̏�Ԃ�����
    //ActionNode(�����̎��s)�AConditionalNode(��������(�߂�l������(bool)�ł���))
    //SequenceNode(�q�����ԂɎ��s���A�r���łP�ł����s�����炻���ŏI���A�S�����s�o�����琬�������Ԃ�)
    //SelectorNode(�q�����ԂɎ��s���A�ǂꂩ��ł����������炻���ŏI�����Đ��������Ԃ�)

    int m_turn = default;
    [SerializeField] int m_maxlife = 100;
    [SerializeField] int m_life = default;

    void Start()
    {
        m_life = m_maxlife;
    }

    public void OnClick(int turn)
    {
        Selector1_1(turn);
    }

    private bool Selector1_1(int turn)
    {
        //Sequence2�ɍs��
        if (Sequence2_1(turn)) return true;
        if (Sequence2_2(turn)) return true;
        return false;
    }

    private bool Sequence2_1(int turn)
    {
        //�̗͏��Ȃ���Ⴭ�O�o���@�o������True
        if (Conditional3_1(turn) && Action3_2(turn)) return true;
        return false;
    }

    private bool Conditional3_1(int turn)
    {
        if (m_life < 50) return true;
        else return false;
    }

    private bool Action3_2(int turn)
    {
        Debug.Log("HP���Ⴂ���̍s��");
        return true;
    }

    private bool Sequence2_2(int turn)
    {
        //����ȊO�̎�
        if (Action3_3(turn) == Action3_4(turn)) return true;
        return false;
    }

    private bool Action3_3(int turn)
    {
        Debug.Log($"turn:{turn}");
        return true;
    }

    private bool Action3_4(int turn)
    {
        Debug.Log("�Ȃ�����Ȃ�");
        return true;
    }

    /// <summary>
    /// �q�����ԂɎ��s���A�r���łP�ł����s�����炻���ŏI���A�S�����s�o�����琬�������Ԃ�
    /// </summary>
    private bool SequenceNode()
    {
        if (!ConditionalNode(m_turn)) return false;
        if (!ActionNode()) return false;
        return true;
    }

    /// <summary>
    /// �q�����ԂɎ��s���A�ǂꂩ��ł����������炻���ŏI�����Đ��������Ԃ�
    /// </summary>
    private void SelectorNode()
    {

    }

    /// <summary>
    /// �����̎��s�B����������True
    /// </summary>
    private bool ActionNode()
    {
        //�{���͐���������True
        Debug.Log("Case0");
        return true;
    }

    /// <summary>
    /// �����B����̏����ɍ����Ă���True
    /// </summary>
    private bool ConditionalNode(int num)
    {
        switch (num)
        {
            case 0:
                return true;
            default:
                return false;
        }
    }
}
