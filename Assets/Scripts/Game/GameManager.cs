using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
/// <summary>
/// 游戏控制
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 属性安全
    /// </summary>
    public static GameManager Instance;
    private GameData Data;
    private Managevars vars;

    // 游戏是否开始
    public bool IsGameStarted
    {
        get;
        set;
    }

    public bool IsPause
    {
        get;
        set;
    }

    // 游戏是否结束
    public bool isGameOver
    {
        get;
        set;
    }

    // 玩家是否开始移动
    public bool PlayerIsMove
    {
        get;
        set;
    }

    // 游戏成绩
    private int GameScore;
    private int gameDiamond;


    private bool isFirstGame;
    private bool isMusicOn;
    private int[] bestScoreArr;

    private int selectSkin;
    private bool[] skinunlock;
    private int DiamondCount;

	void Awake()
	{
        vars = Managevars.GetManagevars();
        Instance = this;
        EventCenter.AddListener(EventDefine.AddScore, AddGameScore);
        EventCenter.AddListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.AddListener(EventDefine.AddDiamond, AddGameDiamond);
        EventCenter.AddListener<int>(EventDefine.UpdateAllDiamond, UpdateAllDiamond);


        if(GameData.isRestartGame)
        {
            IsGameStarted = true;
        }

        InitGameData();
	}


   void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.AddScore, AddGameScore);
        EventCenter.RemoveListener(EventDefine.PlayerMove, PlayerMove);
        EventCenter.RemoveListener(EventDefine.AddDiamond, AddGameDiamond);
        EventCenter.RemoveListener<int>(EventDefine.UpdateAllDiamond, UpdateAllDiamond);
    }

    /// <summary>
    /// 保存成绩
    /// </summary>
    /// <param name="score"></param>
    public void SaveScore(int score)
    {
        List<int> list = bestScoreArr.ToList();
        //从大到小排序list
        list.Sort((x, y) => (-x.CompareTo(y)));
        bestScoreArr = list.ToArray();


        int index = -1;
        for (int i = 0; i < bestScoreArr.Length; i++)
        {
            if (score > bestScoreArr[i])
            {
                index = i;
            }
        }
        if (index == -1) return;

        for (int i = bestScoreArr.Length - 1; i > index; i--)
        {
            bestScoreArr[i] = bestScoreArr[i - 1];
        }
        bestScoreArr[index] = score;
        Save();
    }

    /// <summary>
    /// 获取最高分
    /// </summary>
    /// <returns></returns>
    public int getBestScore()
    {
        return bestScoreArr.Max();
    }

    /// <summary>
    /// 增加游戏成绩
    /// </summary>
    private void AddGameScore()
    {
        if(IsGameStarted == false||isGameOver||IsPause)
        {
            return;
        }
        GameScore++;
        EventCenter.Broadcast(EventDefine.UpdateScore, GameScore);
    }

    /// <summary>
    /// 玩家移动会调用到此方法
    /// </summary>
    private void PlayerMove()
    {
        PlayerIsMove = true;
    }
                               
    public int GetGameScore()
    {
        return GameScore;
    }

    /// <summary>
    /// 更新游戏钻石数量
    /// </summary>
    private void AddGameDiamond()
    {
        gameDiamond++;
        Debug.Log("ADD:"+gameDiamond);
        //刷新文件数据
        EventCenter.Broadcast(EventDefine.UpdateDiamond, gameDiamond);
        Debug.Log("guangbo zengjia");
    }

    public int GetDiamondCount()
    {
        return gameDiamond;
    }

    /// <summary>
    /// 获取当前皮肤是否解锁
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool GetSkinUnLocked(int index)
    {
        return skinunlock[index];
    }

    /// <summary>
    /// 设置当前皮肤解锁
    /// </summary>
    /// <param name="index"></param>
    public void SetSkinUnlocked(int index)
    {
        skinunlock[index] = true;
        Save();
    }

    /// <summary>
    /// 数据初始化
    /// </summary>
    private void InitGameData()
    {
        Read();
        if(Data!=null)
        {
            isFirstGame = Data.GetIsFirstGame();
        }
        else
        {
            isFirstGame = true;
        }

        if(isFirstGame)
        {
            //初始化数据
            isFirstGame = false;
            isMusicOn = true;
            bestScoreArr = new int[3];
            selectSkin = 0;
            skinunlock = new bool[vars.skinSpriteList.Count];
            skinunlock[0] = true;
            DiamondCount = 10;

            Data = new GameData();
            Save();
        }

        else
        {
            isMusicOn = Data.GetIsMusicOn();
            bestScoreArr = Data.GetBestScoreArr();
            selectSkin = Data.GetSelectSkin();
            skinunlock = Data.GetSkinUnlock();
            DiamondCount = Data.GetDiamondCount();
        }
        Debug.Log("zuanshi:" + DiamondCount);
    }

    /// <summary>
    /// 玩家数据存储
    /// </summary>
    private void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Create(Application.persistentDataPath + "/GameData.data")) 
            {
                Data.SetBestScoreArr(bestScoreArr);
                Data.SetDiamondCount(DiamondCount);
                Data.SetIsFirstGame(isFirstGame);
                Data.SetIsMusicOn(isMusicOn);
                Data.SetSelectSkin(selectSkin);
                Data.SetSkinUnlock(skinunlock);

                bf.Serialize(fs, Data);
            }
            Debug.Log("save ok");
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }

    private void Read()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath + "/GameData.data", FileMode.Open)) 
            {
                Data =  (GameData)bf.Deserialize(fs);
                Debug.Log(Data.GetDiamondCount());
                Debug.Log(Data.GetIsFirstGame());
                Debug.Log(Data.GetSelectSkin());

            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public int GetAllDiamond()
    {
        Debug.Log("get" + DiamondCount);
        return DiamondCount;
    }

    public void UpdateAllDiamond(int value)
    {
        DiamondCount = DiamondCount + value;
        Save();
    }

    public void SetSelectedSkin(int index)
    {
        selectSkin = index;
        Save(); 
    }

    public void SetIsMusicOn(bool value)
    {
        isMusicOn = value;
        Save();
    }

    public bool GetIsMusicOn()
    {
        return isMusicOn;
    }

    public int GetSkinSelected()
    {
        return selectSkin;
    }

    public void ResetData()
    {
        isFirstGame = false;
        isMusicOn = true;
        bestScoreArr = new int[3];
        selectSkin = 0;
        skinunlock = new bool[vars.skinSpriteList.Count];
        skinunlock[0] = true;
        DiamondCount = 10;
        Save(); 
    }

    /// <summary>
    /// 获得最高分数组
    /// </summary>
    /// <returns></returns>
    public int[] GetScoreArr()
    {
        List<int> list = bestScoreArr.ToList();
        //从大到小排序list
        list.Sort((x, y) => (-x.CompareTo(y)));
        bestScoreArr = list.ToArray();

        return bestScoreArr;
    }
}
