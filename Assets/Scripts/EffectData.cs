using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EffectData 
{
    /// <summary>
    /// 效果類別
    /// 攻擊、防禦、補血
    /// </summary>
    public enum EffectCategory{
        Attack,
        Defense,
        Tonic
    }
    /// <summary>
    /// 效果類別
    /// </summary>
    public EffectCategory effectCategory;

    /// <summary>
    /// 效果數值
    /// </summary>
    public int effectValue;



}
