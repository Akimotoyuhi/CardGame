using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour
{
    [SerializeField] Image m_icon;
    [SerializeField] Image m_subIcon;
    private CustomModeDataBase m_database;

    public void Setup(CustomModeDataBase database)
    {
        m_database = database;
        m_icon.SetImage(database.IconColor, database.IconSprite);
        m_subIcon.SetImage(database.SubIconColor, database.SubIconSprite);
    }
}
