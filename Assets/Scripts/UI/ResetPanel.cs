using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
/// <summary>
/// 数据重置
/// </summary>
public class ResetPanel : MonoBehaviour
{
    private Button btn_no;
    private Button btn_yes;
    private Image img_Bg;
    private GameObject dialog;

    void Awake()
	{
        EventCenter.AddListener(EventDefine.ShowResetPanel, Show);
        img_Bg = transform.Find("Bg").GetComponent<Image>();

        btn_no = transform.Find("dialog/no").GetComponent<Button>();
        btn_no.onClick.AddListener(OnNoBtnClick);

        btn_yes = transform.Find("dialog/yes").GetComponent<Button>();
        btn_yes.onClick.AddListener(OnYesBtnClick);

        dialog = transform.Find("dialog").gameObject;

        img_Bg.color = new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0);
        dialog.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);

	}

	void OnDestroy()
	{
        EventCenter.RemoveListener(EventDefine.ShowResetPanel, Show);
	}

	private void Show()
    {
        gameObject.SetActive(true);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0.3f),0.3f);
        dialog.transform.DOScale(Vector3.one, 0.3f); 
    }

    private void OnYesBtnClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        GameManager.Instance.ResetData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    private void OnNoBtnClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0.3f),0.3f);
        dialog.transform.DOScale(Vector3.zero, 0.3f).OnComplete(()=>{
            gameObject.SetActive(false);
        });
    }

}
