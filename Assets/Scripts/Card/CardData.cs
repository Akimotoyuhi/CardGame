using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

/// <summary>
/// カードデータ<br/>
/// 報酬画面に出現する確率比重も管理している。
/// </summary>
[CreateAssetMenu(fileName = "Card Data")]
public class CardData : ScriptableObject
{
    [Serializable]
    public class RewardCardProbability
    {
        public string m_rabel;
        [SerializeField, Range(0, 100)] int m_rareProbability;
        [SerializeField, Range(0, 100)] int m_superRareProbability;
        public int RareProbability => m_rareProbability;
        public int SuperRareProbability => m_superRareProbability;
    }
    [SerializeField] List<RewardCardProbability> m_rewardCardProbability;
    [SerializeField] List<CardDataBase> m_cardData = new List<CardDataBase>();
    public CardInfomationData CardDatas(int id, int upgrade) => m_cardData[id].GetCardInfo(upgrade);
    public void Setup()
    {
        for (int i = 0; i < m_cardData.Count; i++)
        {
            m_cardData[i].Setup((CardID)i);
        }
    }
    /// <summary>
    /// ランダムで抽選したレア度の中からランダムで１つカードを抽選する
    /// </summary>
    /// <param name="upgradeNum">強化回数</param>
    /// <returns>カードデータ</returns>
    public CardInfomationData GetCardRarityRandom(int upgradeNum, EnemyType enemyType)
    {
        Rarity rarity;
        int r = UnityEngine.Random.Range(0, 100);
        if (r < m_rewardCardProbability[(int)enemyType].SuperRareProbability)
        {
            rarity = Rarity.SuperRare;
        }
        else if (r < m_rewardCardProbability[(int)enemyType].RareProbability)
        {
            rarity = Rarity.Rare;
        }
        else
        {
            rarity = Rarity.Common;
        }
        return GetCardRarityRandom(rarity, upgradeNum);
    }
    /// <summary>
    /// 指定したレア度のカードを１つ抽選する
    /// </summary>
    /// <param name="rarity">抽選するレア度</param>
    /// <param name="upgradeNum">強化回数</param>
    /// <returns>カードデータ</returns>
    public CardInfomationData GetCardRarityRandom(Rarity rarity, int upgradeNum)
    {
        var list = m_cardData.Where(card => card.GetCardInfo(upgradeNum).Rarity == rarity).ToList();
        int r = UnityEngine.Random.Range(0, list.Count);
        return list[r].GetCardInfo(upgradeNum);
    }
}
/// <summary>カードのデータベース<br/>カードの効果の部分</summary>
[Serializable]
public class CardDataBase
{
    public string m_rabel;
    [SerializeField] CardInfomationData m_cardInfo;
    [SerializeField] CardInfomationData m_cardUpgrade;
    public void Setup(CardID id)
    {
        m_cardInfo.CardId = id;
        m_cardUpgrade.CardId = id;
    }
    /// <summary>強化前後のカード情報<br/>引数は複数回のカード強化を想定してintにしている</summary>
    /// <param name="upgradeNum">強化前後。0が前、1が後</param>
    /// <returns>カード情報</returns>
    public CardInfomationData GetCardInfo(int upgradeNum)
    {
        switch (upgradeNum)
        {
            case 0:
                return m_cardInfo;
            case 1:
                return m_cardUpgrade;
            default:
                Debug.LogError("これ以上設定された強化データが存在しない");
                return null;
        }
    }
}
/// <summary>カードの使用条件/効果の発動条件</summary>
[Serializable]
public class CardConditional
{
    [SerializeReference, SubclassSelector] List<IConditional> m_cardConditional = new List<IConditional>();
    /// <summary>条件の評価</summary>
    /// <param name="evaluationParam">評価したいパラメーターの種類</param>
    /// <returns>カード使用可否</returns>
    public bool Evaluation(Player player, EnemyBase enemy, int deckChildCount, int handChildCount, int discardChildCount)
    {
        bool flag = true;
        foreach (var c in m_cardConditional)
        {
            int[] nums = c.Execute();
            switch ((CardConditionalType)nums[0])
            {
                case CardConditionalType.EvaluationParam:
                    if (!EvaluationParam(nums[1], (CardUsedConditional)nums[2], (CardConditionalEvaluationType)nums[3], (CardConditionalEvaluationParam)nums[4], player, enemy))
                    {
                        return false;
                    }
                    break;
                default:
                    Debug.LogError("例外エラー");
                    break;
            }
        }
        return flag;
    }
    /// <summary>条件がEvaluationParamだった時用</summary>
    private bool EvaluationParam(int evaNum, CardUsedConditional cardUsedConditional, CardConditionalEvaluationType evaluationType, CardConditionalEvaluationParam evaluationParam, Player player, EnemyBase enemy)
    {
        int num = default;
        switch (evaluationType)
        {
            case CardConditionalEvaluationType.Player:
                switch (evaluationParam)
                {
                    //case CardConditionalEvaluationParam.Power://攻撃力を取得するにはちょっと手順が多いので特別処理にすると思う
                    //    break;
                    case CardConditionalEvaluationParam.Block:
                        num = player.CurrentBlock;
                        break;
                    case CardConditionalEvaluationParam.Life:
                        num = player.CurrentLife;
                        break;
                    //case CardConditionalEvaluationParam.BuffDebuff://必要なパラメーターが違うので特別処理にすると思う
                    //    break;
                    default:
                        Debug.LogError("例外エラー");
                        return false;
                }
                break;
            case CardConditionalEvaluationType.Enemy:
                switch (evaluationParam)
                {
                    //case CardConditionalEvaluationParam.Power:
                    //    break;
                    case CardConditionalEvaluationParam.Block:
                        num = enemy.CurrentBlock;
                        break;
                    case CardConditionalEvaluationParam.Life:
                        num = enemy.CurrentLife;
                        break;
                    //case CardConditionalEvaluationParam.BuffDebuff:
                    //    break;
                    default:
                        Debug.LogError("例外エラー");
                        break;
                }
                break;
            default:
                Debug.LogError("例外エラー");
                break;
        }
        switch (cardUsedConditional)
        {
            case CardUsedConditional.High:
                if (evaNum <= num)
                    return true;
                break;
            case CardUsedConditional.Low:
                if (evaNum >= num)
                    return true;
                break;
            case CardUsedConditional.Even:
                if (num % 2 == 0)
                    return true;
                break;
            case CardUsedConditional.Odd:
                if (num % 2 != 0)
                    return true;
                break;
            default:
                Debug.LogError("例外エラー");
                break;
        }
        return false;
    }
}
/// <summary>カードデータの使用時の効果の部分</summary>
[Serializable]
public class CardInfomationData
{
    [SerializeField] string m_name;
    [SerializeField] string m_cost;
    [SerializeField] Sprite m_image;
    [TextArea(0, 5), Tooltip("変数に差し替えたい部分は{leg0}(数値の部分は配列番号)のように記述する事")]
    [SerializeField] string m_tooltip;
    [SerializeField] Rarity m_rarity;
    [SerializeField] List<CardExecuteCommand> m_commands;
    [SerializeField] CardConditional m_cardConditional;
    [SerializeField] UseTiming m_cardType;
    [SerializeField] bool m_isDiscarding = false;
    [SerializeField] bool m_ethereal = false;
    /// <summary>カードの名前</summary>
    public string Name => m_name;
    /// <summary>コスト</summary>
    public string Cost => m_cost; //コストは数字じゃない可能性があるのでとりあえずstringにしとく
    /// <summary>画像</summary>
    public Sprite Sprite => m_image;
    /// <summary>効果の説明文</summary>
    public string Tooltip => m_tooltip;
    /// <summary>カードID</summary>
    public CardID CardId { get; set; }
    /// <summary>レアリティ</summary>
    public Rarity Rarity => m_rarity;
    /// <summary>カードの効果とその条件</summary>
    public List<CardExecuteCommand> Commands => m_commands;
    public CardConditional CardConditional => m_cardConditional;
    /// <summary>使用する標的</summary>
    public UseTiming UseType => m_cardType;
    /// <summary>廃棄カード</summary>
    public bool IsDiscarding => m_isDiscarding;
    public bool Ethereal => m_ethereal;
}
[Serializable]
public class CardExecuteCommand
{
    [SerializeReference, SubclassSelector] ICommand m_command;
    [SerializeField] CardConditional m_conditional;
    /// <summary>
    /// カード使用時の効果<br/>{ 効果の種類(CommandParam), 効果発動時のパーティクル(ParticleID), 発動対象(UseType), 効果(int) }
    /// </summary>
    public int[] CardCommand => m_command.Execute();
    public CardConditional Conditional => m_conditional;
}
/// <summary>〇〇に対して何かしらをしたい時に使うインターフェース</summary>
public interface ICommand
{
    /// <summary>コマンドデータ</summary>
    /// <returns>{ 効果の種類(CommandParam), 効果発動時のパーティクル(ParticleID), 発動対象(UseType), 効果(int) }</returns>
    int[] Execute();
}
public class CardAttackCommand : ICommand
{
    [SerializeField, Tooltip("何ダメージを与えるか")] int m_power;
    [SerializeField, Tooltip("確定ダメージ(バフデバフを考慮しない)を与えるか")] bool m_isTrueDmg;
    [SerializeField, Tooltip("付与対象")] UseTiming m_useType;
    [SerializeField, Tooltip("使用時に表示するパーティクルのID")] ParticleID m_particleID;
    public int[] Execute()
    {
        int b = m_isTrueDmg ? 1 : 0;
        return new int[] { (int)CommandParam.Attack, (int)m_particleID, (int)m_useType, m_power, b };
    }
}
public class CardBlockCommnad : ICommand
{
    [SerializeField, Tooltip("何ブロックを得るか")] int m_block;
    [SerializeField, Tooltip("付与対象")] UseTiming m_useType;
    [SerializeField, Tooltip("使用時に表示するパーティクルのID")] ParticleID m_particleID;
    public int[] Execute() => new int[] { (int)CommandParam.Block, (int)m_particleID, (int)m_useType, m_block };
}
public class CardConditionCommand : ICommand
{
    [SerializeField, Tooltip("付与するバフデバフの設定")] ConditionSelection m_condition;
    [SerializeField, Tooltip("付与対象")] UseTiming m_useType;
    [SerializeField, Tooltip("使用時に表示するパーティクルのID")] ParticleID m_particleID;
    public int[] Execute() => new int[] { (int)CommandParam.Conditon, (int)m_particleID, (int)m_useType, (int)m_condition.GetCondition.GetConditionID(), m_condition.GetCondition.Turn };
}
public class AddCardCommand : ICommand
{
    [SerializeField, Tooltip("カードの追加枚数")] int m_addNum;
    [SerializeField, Tooltip("追加するカードのID")] CardID m_cardID;
    [SerializeField, Tooltip("カード追加先")] CardAddDestination m_cardAddDestination;
    [SerializeField, Tooltip("追加するカードを強化済みにする")] bool m_isUpgrade;
    [SerializeField, Tooltip("使用時に表示するパーティクルのID")] ParticleID m_particleID;
    public int[] Execute()
    {
        int i = m_isUpgrade ? 1 : 0;
        return new int[] { (int)CommandParam.AddCard, (int)m_particleID, (int)UseTiming.System, (int)m_cardID, m_addNum, (int)m_cardAddDestination, i };
    }
}

public class DrawCardCommand : ICommand
{
    [SerializeField, Tooltip("カードのドロー(or捨てる)枚数")] int m_drawNum;
    [SerializeField, Tooltip("trueならドローする、falseなら捨てる")] bool m_isDraw;
    [SerializeField, Tooltip("使用時に表示するパーティクルのID")] ParticleID m_particleID;
    public int[] Execute()
    {
        int i = m_isDraw ? 1 : 0;
        return new int[] { (int)CommandParam.DrawCard, (int)m_particleID, (int)UseTiming.System, i, m_drawNum };
    }
}
public interface IConditional
{
    /// <summary>評価に必要な値をint配列にして渡す</summary>
    /// <returns>
    /// EvaluationParam   { CardConditionalType, life, CardUsedConditional, CardConditionalEvaluationType, CardConditionalEvaluationParam }<br/>
    /// EnemyPlan        { CardConditionalType }<br/>
    /// TurnEndWhereCard { CardConditionalType }
    /// </returns>
    int[] Execute();
}
public class EvaluationParam : IConditional
{
    [SerializeField] CardConditionalEvaluationParam m_evaluationParam;
    [SerializeField] CardConditionalEvaluationType m_evaluationType;
    [SerializeField] CardUsedConditional m_cardConditional;
    [SerializeField] int m_life;

    public int[] Execute()
    {
        return new int[] { (int)CardConditionalType.EvaluationParam, m_life, (int)m_cardConditional, (int)m_evaluationType, (int)m_evaluationParam };
    }
}
public class EnemyPlan : IConditional
{


    public int[] Execute()
    {
        return new int[] { (int)CardConditionalType.EnemyPlan };
    }
}
public class TurnEndWhereCard : IConditional
{
    public int[] Execute()
    {
        return new int[] { (int)CardConditionalType.TurnEndWhereCard };
    }
}
#region Enums
/// <summary>カードのレア度</summary>
public enum CardID
{
    /// <summary>斬撃</summary>
    Slashing, //スターター
    /// <summary>防御</summary>
    Defense, //スターター
    /// <summary>連撃</summary>
    Burst,
    /// <summary>虚脱の光</summary>
    WeaknessLight,
    /// <summary>やたらめったら</summary>
    YataraMettara,
    /// <summary>筋力増加</summary>
    StrengthUp,
    /// <summary>ガラスのナイフ</summary>
    GlassKnife,
    /// <summary>「破片」</summary>
    Shard,
    /// <summary>鉄壁</summary>
    IronWall,
    /// <summary>俊足</summary>
    SwiftHorse,
    /// <summary>奇妙な刻印</summary>
    StrangeEngraving,
    /// <summary>解かれし力</summary>
    ReleaseOfSealed,
    /// <summary>「終末」</summary>
    Apocalypse,
    /// <summary>リフレクト</summary>
    Reflect,
    /// <summary>無限の剣</summary>
    UnlimitedSword,
    /// <summary>深呼吸</summary>
    DeepBreath,
    /// <summary>ブーメラン</summary>
    Boomerang,
    /// <summary>シールドバッシュ</summary>
    ShieldBash,
    /// <summary>マジックシールド</summary>
    MagicShield,
    /// <summary>万全</summary>
    Perfection,
    /// <summary>決死の突撃</summary>
    DeadlyAssault,
    /// <summary>アローレイン</summary>
    ArrowRain,
    /// <summary>火傷</summary>
    Burn,
    /// <summary>瓦礫</summary>
    Rubble,
    /// <summary>緊急防御</summary>
    EmergencyBlock,
    /// <summary>タックル</summary>
    Tackle,
    /// <summary>テスト</summary>
    Test,
}
/// <summary>カードのレア度</summary>
public enum Rarity
{
    Common,
    Rare,
    SuperRare,
    Special,
    Curse,
    BadEffect,
}
/// <summary>カードの効果発動タイミングと標的</summary>
public enum UseTiming
{
    None = -1,
    ToPlayer,
    ToEnemy,
    ToAllEnemies,
    ToRandomEnemy,
    System,
    TurnEnd,
}
/// <summary>カードの使用時の効果が何なのか</summary>
public enum CommandParam
{
    Attack,
    Block,
    Conditon,
    AddCard,
    DrawCard,
}
/// <summary>カード追加系カードを使用した際のカードの追加先</summary>
public enum CardAddDestination
{
    ToDeck,
    ToHand,
    ToDiscard,
}
/// <summary>カード使用条件の種類</summary>
public enum CardConditionalType
{
    EvaluationParam,
    EnemyPlan,
    TurnEndWhereCard,
}
/// <summary>カード使用条件にて評価したい対象</summary>
public enum CardConditionalEvaluationType
{
    Player,
    Enemy,
}
/// <summary>カード使用条件にて評価したいパラメーター</summary>
public enum CardConditionalEvaluationParam
{
    //Power,
    Block,
    Life,
    //BuffDebuff,
}
/// <summary>CardConditionalTypeがEvaluationParamだった場合の使用条件</summary>
public enum CardUsedConditional
{
    High,
    Low,
    Even,//偶数
    Odd,//奇数
}
#endregion