﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koutetunoisi : CardBase, IBuffCard
{
    void Start()
    {
        SetUp();
        m_tooltip.text = $"敏捷と筋力を5得る\n10ブロックを得る\nターン終了時に\n敏捷と筋力を5失い\n次のターンスタンする";
    }

    public int[] SetBlock()
    {
        OnCast();
        return SetBlock();
    }
}