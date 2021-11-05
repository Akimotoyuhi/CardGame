using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// GameManagerより上のManager。GameManager神
/// </summary>
public class GodGameManager
{
    
    static GodGameManager instance = new GodGameManager(); // privateなクラス変数
    static public GodGameManager getInstance() { return instance; }// インスタンスを返す関数
    private int test = 0;
    private GodGameManager() { } //privateなコンストラクタ
    public void set(int p)
    {
        instance.test = p;
    }
    public int get()
    {
        return instance.test;
    }
}