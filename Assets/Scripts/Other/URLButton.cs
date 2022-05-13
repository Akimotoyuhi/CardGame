using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLButton : MonoBehaviour
{
    [SerializeField] string m_url;
    public void OnClick()
    {
        Application.OpenURL(m_url);
    }
}
