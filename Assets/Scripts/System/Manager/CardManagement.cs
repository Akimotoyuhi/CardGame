using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManagement : MonoBehaviour
{
    /// <summary>
    /// ���̃N���X�̎q�I�u�W�F�N�g��S�Ĕj������
    /// </summary>
    public void CardDelete()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
