using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 平台初始化
/// </summary>
public class PlatformScript : MonoBehaviour
{
    public SpriteRenderer[] spriteRenderers;
    public GameObject obstacle;
    private bool startTimer;
    private float fallTime;
    private Rigidbody2D my_Body;

	void Awake()
	{
        my_Body = GetComponent<Rigidbody2D>();
	}

	public void Init(Sprite sprite, float fallTime, int obstacleDir //障碍物方向)
	{
        my_Body.bodyType = RigidbodyType2D.Static;
        this.fallTime = fallTime;
        startTimer = true;
        for (int i = 0; i < spriteRenderers.Length;i++)
        {
            spriteRenderers[i].sprite = sprite;  
        }
        if(obstacleDir == 0) //朝右边
        {
            if(obstacle != null)
            {
                Vector3 temp = obstacle.transform.localPosition;
                obstacle.transform.localPosition = new Vector3(-temp.x, temp.y, temp.z);
            }
        }
	}

	void Start()
    {
        
    }


    void Update()
    {
        if(GameManager.Instance.IsGameStarted == false|| GameManager.Instance.PlayerIsMove == false)
        {
            return;
        }
        if(startTimer)
        {
            fallTime -= Time.deltaTime;
            if(fallTime<0) //倒计时结束
            {
                //掉落
                startTimer = false;
                if(my_Body.bodyType != RigidbodyType2D.Dynamic)
                {
                    my_Body.bodyType = RigidbodyType2D.Dynamic;
                    StartCoroutine(DealyHide()); 
                }
            }
        }
        if(transform.position.y - Camera.main.transform.position.y < -6)
        {
            StartCoroutine(DealyHide()); 
        }
    }

    private IEnumerator DealyHide()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
