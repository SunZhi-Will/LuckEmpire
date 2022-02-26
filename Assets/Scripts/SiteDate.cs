using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SiteRole", menuName = "Unity Royale/Site Data")]
/// <summary>
/// 整體紀錄檔案
/// </summary>
public class SiteDate : ScriptableObject
{
    /// <summary>
    /// 初始玩家檔案
    /// </summary>
    public RoleData playRoleDate;
    /// <summary>
    /// 初始敵人檔案
    /// </summary>
    public RoleData[] enemyRoleDate;

    /// <summary>
    /// 動態玩家檔案
    /// </summary>
    private RoleData m_playRoleDate;
    /// <summary>
    /// 動態敵人檔案
    /// </summary>
    private RoleData m_enemyRoleDate;
    
    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialization(){
        m_playRoleDate = ScriptableObject.CreateInstance<RoleData>();
        SetSiteDate(m_playRoleDate, playRoleDate);
    }

    public RoleData GetRoleData(){
        return m_playRoleDate;
    }
    public RoleData GetEnemyRoleDate(int num){
        return enemyRoleDate[num].RoleDataCopy();
    }
    /// <summary>
    /// 複製類別
    /// </summary>
    /// <param name="dst">覆蓋類別</param>
    /// <param name="src">複製類別</param>
    public void SetSiteDate(object dst, object src){
        var srcT = src.GetType();
        var dstT= dst.GetType();
        foreach(var f in srcT.GetFields())
        {
            var dstF = dstT.GetField(f.Name);
            if (dstF == null || dstF.IsLiteral)
                continue;
            dstF.SetValue(dst, f.GetValue(src));
        }

        foreach (var f in srcT.GetProperties())
        {
            var dstF = dstT.GetProperty(f.Name);
            if (dstF == null)
                continue;
            
            dstF.SetValue(dst, f.GetValue(src, null), null);
        }
    }
    
}
