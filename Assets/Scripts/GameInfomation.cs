using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームの常時表示しておく情報を管理するクラス
/// </summary>
public class GameInfomation : MonoBehaviour
{
    [SerializeField] Text m_playerName;
    [SerializeField] Text m_hp;
    [SerializeField] Text m_floor;

    void Start()
    {
        
    }
}
