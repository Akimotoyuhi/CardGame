using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵AI作る前にビヘイビアツリーを理解しようクラス
/// </summary>
public class TreeSample : MonoBehaviour
{
    //とりあえずやってみるクラス
    //必要なのはNode。各Nodeは待機中、実行中、成功、失敗の状態を持つ
    //ActionNode(処理の実行)、ConditionalNode(条件判定(戻り値が成否(bool)である))
    //SequenceNode(子を順番に実行し、途中で１つでも失敗したらそこで終了、全部実行出来たら成功判定を返す)
    //SelectorNode(子を順番に実行し、どれか一個でも成功したらそこで終了して成功判定を返す)

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
        //Sequence2に行く
        if (Sequence2_1(turn)) return true;
        if (Sequence2_2(turn)) return true;
        return false;
    }

    private bool Sequence2_1(int turn)
    {
        //体力少なけりゃログ出す　出来たらTrue
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
        Debug.Log("HPが低い時の行動");
        return true;
    }

    private bool Sequence2_2(int turn)
    {
        //それ以外の時
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
        Debug.Log("なんもしない");
        return true;
    }

    /// <summary>
    /// 子を順番に実行し、途中で１つでも失敗したらそこで終了、全部実行出来たら成功判定を返す
    /// </summary>
    private bool SequenceNode()
    {
        if (!ConditionalNode(m_turn)) return false;
        if (!ActionNode()) return false;
        return true;
    }

    /// <summary>
    /// 子を順番に実行し、どれか一個でも成功したらそこで終了して成功判定を返す
    /// </summary>
    private void SelectorNode()
    {

    }

    /// <summary>
    /// 処理の実行。成功したらTrue
    /// </summary>
    private bool ActionNode()
    {
        //本当は成功したらTrue
        Debug.Log("Case0");
        return true;
    }

    /// <summary>
    /// 条件。特定の条件に合ってたらTrue
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
