using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// 游戏结束
/// </summary>
public class GameOverPanel : MonoBehaviour
{

    public Text txt_Score;
    public Text txt_Best, diamond_count;
    public Image img_new;

    public Button Restart;
    public Button rank;
    public Button main;

    void Awake()
	{
        Restart.onClick.AddListener(OnRestartBtnClick);
        rank.onClick.AddListener(OnRankBtnClick);
        main.onClick.AddListener(OnMainBtnClick);
        EventCenter.AddListener(EventDefine.ShowGameOver,Show);

        gameObject.SetActive(false);
	}

	void OnDestroy()
	{
        EventCenter.RemoveListener(EventDefine.ShowGameOver, Show);
	}

	private void Show()
    {
        if(GameManager.Instance.GetGameScore()>GameManager.Instance.getBestScore())
        {
            img_new.gameObject.SetActive(true);
            txt_Best.text = "HIGHEST: " + GameManager.Instance.GetGameScore();
        }
        else{
            img_new.gameObject.SetActive(false);
            txt_Best.text = "HIGHEST: " + GameManager.Instance.getBestScore();
        }
        GameManager.Instance.SaveScore(GameManager.Instance.GetGameScore());
        txt_Score.text = GameManager.Instance.GetGameScore().ToString();
        diamond_count.text = "+"+GameManager.Instance.GetDiamondCount().ToString();
        //GameManager.Instance.UpdateAllDiamond(GameManager.Instance.GetGameScore());

        gameObject.SetActive(true);
    }

    /// <summary>
    /// 再来一局按钮点击
    /// </summary>
    private void OnRestartBtnClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.isRestartGame = true;
    }

    /// <summary>
    /// 排行榜按钮点击
    /// </summary>
    private void OnRankBtnClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }

    private void OnMainBtnClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.isRestartGame = false;
    }

	void Start()
    {
        
    }


    void Update()
    {
        
    }
}
