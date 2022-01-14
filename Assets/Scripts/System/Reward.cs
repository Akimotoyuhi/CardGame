using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    /// <summary>��V��ʃp�l��</summary>
    [SerializeField] GameObject m_panel;
    /// <summary>�\������J�[�h�̃v���n�u</summary>
    [SerializeField] GameObject m_uiCard;
    /// <summary>�J�[�h�̐e�I�u�W�F�N�g(���C�A�E�g�O���[�v�p)</summary>
    [SerializeField] Transform m_cardsParent;

    /// <summary>
    /// ��V��ʕ\��
    /// </summary>
    public void RewardView(NewCardDataBase cardDataBase, int id)
    {
        m_panel.SetActive(true);
        Transform obj = Instantiate(m_uiCard).transform;
        obj.SetParent(m_cardsParent);
        obj.GetComponent<UICard>().Setup(cardDataBase, id, this);
    }

    public void OnClick(int id)
    {
        m_panel.SetActive(false);
    }
}