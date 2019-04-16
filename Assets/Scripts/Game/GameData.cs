using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 序列化数据
/// </summary>
[System.Serializable]

public class GameData  
{
    //是否再来一次游戏
    public static bool isRestartGame = false;

    private bool isFirstGame;
    private bool isMusicOn;
    private int[] bestScoreArr;

    private int selectSkin;
    private bool[] skinUnlock;
    private int DiamondCount;

    public void SetIsFirstGame(bool isfirstgame)
    {
        this.isFirstGame = isfirstgame;
    }

    public void SetIsMusicOn(bool isMusicOn)
    {
        this.isMusicOn = isMusicOn;
    }

    public void SetBestScoreArr(int[] BestScoreArr)
    {
        this.bestScoreArr = BestScoreArr;
    }

    public void SetSelectSkin(int selectSkin)
    {
        this.selectSkin = selectSkin;
    }

    public void SetSkinUnlock(bool[] skinUnlock)
    {
        this.skinUnlock = skinUnlock;
    }

    public void SetDiamondCount(int DiamondCount)
    {
        this.DiamondCount = DiamondCount;
    }

    public bool GetIsFirstGame()
    {
        return isFirstGame;
    }

    public bool GetIsMusicOn()
    {
        return isMusicOn;
    }

    public int[] GetBestScoreArr()
    {
        return bestScoreArr;
    }

    public int GetSelectSkin()
    {
        return selectSkin;
    }

    public bool[] GetSkinUnlock()
    {
        return skinUnlock;
    }

    public int GetDiamondCount()
    {
        return DiamondCount;
    }



}
