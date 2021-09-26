using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newCardBase : MonoBehaviour
{
    [Serializable]
    class EnemyCommandSet
    {
        [SerializeReference, SubclassSelector]
        public ICommand Command;                // コマンド
        public string m_name = "name";
        public int Probability = 1;             // 実行確率
        public float CastTime = 0;              // 行動までの時間
        public float CoolTime = 0;              // 行動後のインターバル
        public int NextIndex = -1;              // 次に必ずそのIndexの行動をする。-1で抽選に戻る
    }

    [SerializeField] List<EnemyCommandSet> m_enemyCommandSets = new List<EnemyCommandSet>();
}
