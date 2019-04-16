using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 
/// <summary>
/// 商店面板
/// </summary>
public class ShopPanel : MonoBehaviour
{
    private Managevars vars;
    private Transform parent;
    private Text txt_Name;
    private Text txt_diamond;
    private Button btn_Back;

    private Button btn_select;
    private Button btn_buy;
    private int currentIndex;

	void Awake()
	{
        EventCenter.AddListener(EventDefine.ShowShopPanel, Show);

        parent = transform.Find("ScrollRect/Parent");
        txt_Name = transform.Find("txt_Name").GetComponent<Text>();
        btn_Back = transform.Find("btn_back").GetComponent<Button>();
        btn_Back.onClick.AddListener(OnBackButtonClick);
        btn_select = transform.Find("btn_select").GetComponent<Button>();
        btn_select.onClick.AddListener(OnSelectBtnClick);
        btn_buy = transform.Find("btn_buy").GetComponent<Button>();
        btn_buy.onClick.AddListener(OnBuyButtonClick);
        txt_diamond = transform.Find("Diamond/Text").GetComponent<Text>();
        Debug.Log("jiazai wancheng");
        vars = Managevars.GetManagevars();
        //Init();

	}

	void OnDestroy()
	{
        EventCenter.RemoveListener(EventDefine.ShowShopPanel, Show);
	}

	private void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnBuyButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        int price = int.Parse(btn_buy.GetComponentInChildren<Text>().text);
        if(price > GameManager.Instance.GetAllDiamond())
        {
            EventCenter.Broadcast(EventDefine.Hint, "Diamond!");
            Debug.Log("mei qian");
            return;
        }
        GameManager.Instance.UpdateAllDiamond(-price);
        GameManager.Instance.SetSkinUnlocked(currentIndex);
        parent.GetChild(currentIndex).GetChild(0).GetComponent<Image>().color = Color.white;

    }

    private void Init()
    {
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2((vars.skinSpriteList.Count + 2) * 160, 302);
        for (int i = 0; i < vars.skinSpriteList.Count;i++)
        {
            GameObject go = Instantiate(vars.skinChooseItemPre, parent);
            if(GameManager.Instance.GetSkinUnLocked(i) == false)
            {
                go.GetComponentInChildren<Image>().color = Color.gray;
            }
            else
            {
                go.GetComponentInChildren<Image>().color = Color.white;
            }
            go.GetComponentInChildren<Image>().sprite = vars.skinSpriteList[i];
            go.transform.localPosition = new Vector3(80+(i + 1) * 120, 0, 0);
        }
        //打开页面直接定位到选中的皮肤
        parent.transform.localPosition = new Vector3(GameManager.Instance.GetSkinSelected() * -160, 0);
    }

	void Start()
    {
        Init();
        gameObject.SetActive(false);
    }

   
    void Update()
    {
        currentIndex = (int)Mathf.Round(parent.transform.localPosition.x / -120.0f);
        if(Input.GetMouseButton(0))
        {
            parent.transform.DOLocalMoveX(currentIndex * -120, 0.2f);
            //parent.transform.localPosition = new Vector3(currentIndex * -120, 0);
        }
        SetItemSize(currentIndex);
        RefreshUI(currentIndex);
    }

    private void SetItemSize(int index)
    {
        for (int i = 0; i < parent.childCount;i++)
        {
            if(index == i)
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(160, 160);
            }
            else
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);
            }
        }
    }

    private void RefreshUI(int selectIndex)
    {
        txt_Name.text = vars.SkinNameList[selectIndex];
        txt_diamond.text = GameManager.Instance.GetAllDiamond().ToString();

        //未解锁
        Debug.Log("diyibu"+GameManager.Instance.GetAllDiamond().ToString());
        if(GameManager.Instance.GetSkinUnLocked(selectIndex)==false)
        {
            btn_select.gameObject.SetActive(false);
            btn_buy.gameObject.SetActive(true);
            btn_buy.GetComponentInChildren<Text>().text = vars.skinPrice[selectIndex].ToString();
        }
        else
        {
            btn_select.gameObject.SetActive(true);
            btn_buy.gameObject.SetActive(false);
        }

    }

    private void OnBackButtonClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        EventCenter.Broadcast(EventDefine.ShowMainPanel);
        gameObject.SetActive(false);
    }

    private void OnSelectBtnClick()
    {
        EventCenter.Broadcast(EventDefine.PlayClickAudio);
        EventCenter.Broadcast(EventDefine.ChangeSkin, currentIndex);
        GameManager.Instance.SetSelectedSkin(currentIndex);
        EventCenter.Broadcast(EventDefine.ShowMainPanel);
        gameObject.SetActive(false);
    }
}
