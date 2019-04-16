using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 游戏面板
/// </summary>
public class GamePanel : MonoBehaviour
{

    private Button btn_Pause;
    private Button btn_Restart;


    private Text Diamond_txt;
    private Text txt_Score;


	void Awake()
	{
        EventCenter.AddListener(EventDefine.ShowGamePanel, Show);

        EventCenter.AddListener<int>(EventDefine.UpdateScore, UpdateScore);

        EventCenter.AddListener<int>(EventDefine.UpdateDiamond, UpdateDiamond);
        Init();
	}

    private void Init()
    {
        
        btn_Pause = transform.Find("btn_Pause").GetComponent<Button>();
        btn_Pause.onClick.AddListener(OnPauseButtonClick);
        btn_Restart = transform.Find("btn_Restart").GetComponent<Button>();
        btn_Restart.onClick.AddListener(OnRestartButtonClick);

        Diamond_txt = transform.Find("Diamond/Text").GetComponent<Text>();
        txt_Score = transform.Find("txt_Score").GetComponent<Text>();

        gameObject.SetActive(false);
        btn_Restart.gameObject.SetActive(false);

    }

    void OnDestroy()
	{
        EventCenter.RemoveListener(EventDefine.ShowGamePanel, Show);
        EventCenter.RemoveListener<int>(EventDefine.UpdateScore, UpdateScore);
        EventCenter.RemoveListener <int>(EventDefine.UpdateDiamond, UpdateDiamond);
	}

	void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 更新成绩显示
    /// </summary>
    /// <param name="score"></param>
    private void UpdateScore(int score)
    {
        txt_Score.text = score.ToString();
    }

    private void UpdateDiamond(int diamond)
    {
        Diamond_txt.text = diamond.ToString();
    }

    private void OnPauseButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        btn_Pause.gameObject.SetActive(false);
        btn_Restart.gameObject.SetActive(true);
        //游戏暂停
        Time.timeScale = 0;
        GameManager.Instance.IsPause = true;
    }

    private void OnRestartButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        btn_Pause.gameObject.SetActive(true);
        btn_Restart.gameObject.SetActive(false);
        //继续游戏
        Time.timeScale = 1;
        GameManager.Instance.IsPause = false;
    }
}
