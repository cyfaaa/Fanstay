using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 碰撞处理
/// </summary>
public class Hint : MonoBehaviour
{
    private Image img_Bg;
    private Text txt;
    void Start()
    {
        img_Bg = gameObject.GetComponent<Image>();
        txt = GetComponentInChildren<Text>();
        img_Bg.color = new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0);
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);

        EventCenter.AddListener<string>(EventDefine.Hint, Show);
    }

	void OnDestroy()
	{
        EventCenter.RemoveListener<string>(EventDefine.Hint, Show);
	}

	private void Show(string text)
    {
        StopCoroutine("Dealy");
        transform.localPosition = new Vector3(0, -70, 0);
        transform.DOLocalMoveY(0, 0.3f).OnComplete(()=>{
            StartCoroutine("Dealy");
        });
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0.4f), 0.1f);
        txt.DOColor(new Color(txt.color.r, txt.color.g, txt.color.b, 1), 0.1f);
    }

    private IEnumerator Dealy()
    {
        yield return new WaitForSeconds(1f);
        transform.DOLocalMoveY(70, 0.3f);
        img_Bg.DOColor(new Color(img_Bg.color.r, img_Bg.color.g, img_Bg.color.b, 0), 0.1f);
        txt.DOColor(new Color(txt.color.r, txt.color.g, txt.color.b, 0), 0.1f);
    }

    void Update()
    {
        
    }
}
