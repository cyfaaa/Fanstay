using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 主控面板
/// </summary>
public class MainPanel : MonoBehaviour
{

    private Button btnStart;
    private Button btnRank;
    private Button btnShop;
    private Button btnSound;
    private Managevars vars;
    private Button btnReset;


	void Awake()
	{
        vars = Managevars.GetManagevars();
        Init();
        EventCenter.AddListener(EventDefine.ShowMainPanel, Show);

        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);
	}

	void OnDestroy()
	{
        EventCenter.RemoveListener(EventDefine.ShowMainPanel, Show);
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
	}

	private void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 皮肤更换，这里更换UI皮肤图片
    /// </summary>
    /// <param name="skinIndex"></param>
    private void ChangeSkin(int SkinIndex)
    {
        btnShop.transform.GetChild(0).GetComponent<Image>().sprite =
                   vars.skinSpriteList[SkinIndex];
    }

	private void Init()
	{
        btnStart = transform.Find("BtnStart").GetComponent<Button>();
        btnStart.onClick.AddListener(OnStartButtonClick);
        btnRank = transform.Find("Buttons/BtnRank").GetComponent<Button>();
        btnRank.onClick.AddListener(OnRankButtonClick);
        btnShop = transform.Find("Buttons/BtnShop").GetComponent<Button>();
        btnShop.onClick.AddListener(OnShopButtonClick);
        btnSound = transform.Find("Buttons/BtnSound").GetComponent<Button>();
        btnSound.onClick.AddListener(OnSoundButtonClick);
        btnReset = transform.Find("Buttons/BtnReset").GetComponent<Button>();
        btnReset.onClick.AddListener(OnResetButtonClick);
	}

    private void OnStartButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        GameManager.Instance.IsGameStarted = true;
        EventCenter.Broadcast(EventDefine.ShowGamePanel);
        gameObject.SetActive(false);
    }

    private void OnRankButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }

    private void OnShopButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        EventCenter.Broadcast(EventDefine.ShowShopPanel);
        gameObject.SetActive(false);
    }

    private void OnSoundButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        GameManager.Instance.SetIsMusicOn(!GameManager.Instance.GetIsMusicOn());
        Sound();
    }

    private void Sound()
    {
        if (GameManager.Instance.GetIsMusicOn())
        {
            btnSound.transform.GetChild(0).GetComponent<Image>().sprite = vars.AudioClipOn;
        }
        else
        {
            btnSound.transform.GetChild(0).GetComponent<Image>().sprite = vars.AudioClipOff;
        }
        EventCenter.Broadcast(EventDefine.IsMusicOn, GameManager.Instance.GetIsMusicOn());
    }

	void Start()
    {
        if (GameData.isRestartGame)
        {
            EventCenter.Broadcast(EventDefine.ShowGamePanel);
            gameObject.SetActive(false);
        }

        Sound();
        ChangeSkin(GameManager.Instance.GetSkinSelected());
    }

    private void OnResetButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        EventCenter.Broadcast(EventDefine.ShowResetPanel);
    }


    void Update()
    {
        
    }
}
