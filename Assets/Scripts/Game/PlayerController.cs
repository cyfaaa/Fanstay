using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
/// <summary>
/// 玩家控制
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 是否向左移动，反之向右
    /// </summary>
    private bool isMoveLeft = false;
    /// <summary>
    /// 是否正在跳跃
    /// </summary>
    private bool isJumping = false;
    private Vector3 nextPlatformLeft, nextPlatformRight;
    private Managevars vars;
    private Rigidbody2D my_Body;
    private SpriteRenderer spriterender;
    private bool isMove = false;


    public Transform rayDown, rayLeft, rayRight;
    public LayerMask platformLayer, obstacleLayer;
    private AudioSource m_AudioSource;

	void Awake()
	{
        EventCenter.AddListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
        EventCenter.AddListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        vars = Managevars.GetManagevars();
        my_Body = GetComponent<Rigidbody2D>();
        spriterender = GetComponent<SpriteRenderer>();
        m_AudioSource = gameObject.GetComponent<AudioSource>();
	}

	void OnDestroy()
	{
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, ChangeSkin);
        EventCenter.RemoveListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
	}

    /// <summary>
    /// 更换皮肤的调用
    /// </summary>
    /// <param name="skinIndex"></param>
	private void ChangeSkin(int skinIndex)
    {
        spriterender.sprite = vars.characterSpriteList[skinIndex];
    }

	void Start()
    {
        ChangeSkin(GameManager.Instance.GetSkinSelected());
    }

    private bool IsPointerOverGameObject(Vector2 mousePosition)
    {
        //创建一个点击事件
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        //向点击位置发射一条射线，检测是否点击的UI
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults.Count > 0;
    }

    void Update()
    {

        Debug.DrawRay(rayDown.position, Vector3.down * 1,Color.red);
        Debug.DrawRay(rayRight.position, Vector3.right * 0.15f, Color.red);
        Debug.DrawRay(rayLeft.position, Vector3.left * 0.15f, Color.red);

        /*if (Application.platform == RuntimePlatform.Android||Application.platform == RuntimePlatform.IPhonePlayer)
        {
            int fingerID = Input.GetTouch(0).fingerId;
            if (EventSystem.current.IsPointerOverGameObject(fingerID)) return;
        }
        else
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
        }*/
        if (IsPointerOverGameObject(Input.mousePosition)) return;

        if (GameManager.Instance.IsGameStarted == false || GameManager.Instance.isGameOver == true || GameManager.Instance.IsPause )
            return;
        
        if(Input.GetMouseButtonDown(0)&&isJumping == false&&nextPlatformLeft!=Vector3.zero)
        {
            if(isMove==false)
            {
                EventCenter.Broadcast(EventDefine.PlayerMove);
                isMove = true;
            }
            m_AudioSource.PlayOneShot(vars.JumpClip);
            EventCenter.Broadcast(EventDefine.DecidePath); 
            isJumping = true;
            Vector3 mousePos = Input.mousePosition;
            //点击的是左边屏幕
            if(mousePos.x<=Screen.width / 2)
            {
                isMoveLeft = true;
            }
            //right
            else if(mousePos.x>Screen.width / 2)
            {
                isMoveLeft = false;
            }
            Jump();
        }
        //游戏结束了
        if(my_Body.velocity.y < 0&&isRayPlatform()==false&&GameManager.Instance.isGameOver==false)
        {
            m_AudioSource.PlayOneShot(vars.FallClip);
            spriterender.sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.isGameOver = true;
            StartCoroutine(DealyShowGameOver());

        }
        if(isJumping && isRayObstacle() && GameManager .Instance.isGameOver==false)
        {
            m_AudioSource.PlayOneShot(vars.HitClip);
            GameObject go = ObjectPool.Instance.GetDeathEffect();
            go.SetActive(true);
            go.transform.position = transform.position;
            GameManager.Instance.isGameOver =  true;
            spriterender.enabled = false;
            StartCoroutine(DealyShowGameOver());

        }
        if(transform.position.y - Camera.main.transform.position.y<-6f&&GameManager.Instance.isGameOver == false)
        {
            m_AudioSource.PlayOneShot(vars.FallClip);
            GameManager.Instance.isGameOver = true;
            StartCoroutine(DealyShowGameOver());
        }
    }

    private GameObject lastHitGo = null;

    /// <summary>
    /// 是否检测到平台
    /// </summary>
    /// <returns></returns>
    private bool isRayPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayDown.position, Vector2.down, 1f, platformLayer);
        if(hit.collider != null)
        {
            if (hit.collider.tag == "Platform")
            {
                if(lastHitGo != hit.collider.gameObject)
                {
                    if(lastHitGo == null)
                    {
                        lastHitGo = hit.collider.gameObject;
                        return true;
                    }
                    EventCenter.Broadcast(EventDefine.AddScore);
                    lastHitGo = hit.collider.gameObject;
                }
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 是否检测到障碍物
    /// </summary>
    /// <returns></returns>
    private bool isRayObstacle()
    {
        RaycastHit2D hit1 = Physics2D.Raycast(rayLeft.position, Vector2.left, 0.15f, obstacleLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(rayRight.position, Vector2.right, 0.15f, obstacleLayer);

        if(hit1.collider != null)
        {
            if(hit1.collider.tag == "Obstacle")
            {
                return true;
            }
        }

        if(hit2.collider != null)
        {
            if (hit2.collider.tag == "Obstacle")
            {
                return true;
            }
        }

        return false;
    }

    private void Jump()
    {
        if(isMoveLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            transform.DOMoveX(nextPlatformLeft.x, 0.2f);
            transform.DOMoveY(nextPlatformLeft.y + 0.8f, 0.15f);

        }
        else
        {
            transform.DOMoveX(nextPlatformRight.x, 0.2f);
            transform.DOMoveY(nextPlatformRight.y + 0.8f, 0.15f);
            transform.localScale = Vector3.one;
        }
    }

	void OnTriggerEnter2D(Collider2D collision)
	{
        if(collision.tag == "Platform")
        {
            isJumping = false;
            Vector3 currentPlatformPos = collision.gameObject.transform.position;
            nextPlatformLeft = new Vector3(currentPlatformPos.x - vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
            nextPlatformRight = new Vector3(currentPlatformPos.x + vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
        }
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
        if(collision.collider.tag == "PickUp")
        {
            m_AudioSource.PlayOneShot(vars.DiamondClip);
            EventCenter.Broadcast(EventDefine.AddDiamond);
            Debug.Log("ok1");
            collision.gameObject.SetActive(false);
            EventCenter.Broadcast(EventDefine.UpdateAllDiamond, 1);
            //GameManager.Instance.UpdateAllDiamond(1);
        }
	}

    IEnumerator DealyShowGameOver()
    {
        yield return new WaitForSeconds(1.5f);
        EventCenter.Broadcast(EventDefine.ShowGameOver);
    }

    private void IsMusicOn(bool ismusicon)
    {
        m_AudioSource.mute = !ismusicon;
    }
}
