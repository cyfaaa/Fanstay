using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 
/// <summary>
/// 等级面板
/// </summary>
public class RankPanel : MonoBehaviour
{

    private Button btn_back;

    public Text[] txt_Scores;

    private GameObject ScoreList;
	void Awake()
	{
        EventCenter.AddListener(EventDefine.ShowRankPanel, Show);
        btn_back = transform.Find("btn_Back").GetComponent<Button>();
        btn_back.onClick.AddListener(OnBackBtnClick);
        ScoreList = transform.Find("ScoreList").gameObject;

        Color bg_color = btn_back.GetComponent<Image>().color;
        btn_back.GetComponent<Image>().color = new Color(bg_color.r, bg_color.g, bg_color.b, 0);
        ScoreList.transform.localScale = Vector3.zero;

        gameObject.SetActive(false);

	}

	void OnDestroy()
	{
        EventCenter.RemoveListener(EventDefine.ShowRankPanel, Show);
	}

	private void OnBackBtnClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        btn_back.GetComponent<Image>().DOColor(new Color
                                               (btn_back.GetComponent<Image>().color.r,
                                                btn_back.GetComponent<Image>().color.g,
                                                btn_back.GetComponent<Image>().color.b, 0f), 0.3f);
        ScoreList.transform.DOScale(Vector3.zero, 0.3f).OnComplete(()=>
        {
            gameObject.SetActive(false);
        });
        Debug.Log("gundan");
    }

    private void Show()
    {
        gameObject.SetActive(true);
        btn_back.GetComponent<Image>().DOColor(new Color
                                               (btn_back.GetComponent<Image>().color.r,
                                                btn_back.GetComponent<Image>().color.g,
                                                btn_back.GetComponent<Image>().color.b, 0.3f), 0.3f);
        ScoreList.transform.DOScale(Vector3.one, 0.3f);

        int[] arr = GameManager.Instance.GetScoreArr();
        for (int i = 0; i < arr.Length;i++)
        {
            txt_Scores[i].text = arr[i].ToString();
        }
    }

	void Start()
    {
        
    }


    void Update()
    {
        
    }
}
