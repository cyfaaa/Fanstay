using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏主题
/// </summary>
public class BgTheme : MonoBehaviour
{

    private SpriteRenderer m_spriterender;
    private Managevars vars;

	void Awake()
	{
        vars = Managevars.GetManagevars();
        m_spriterender = gameObject.GetComponent<SpriteRenderer>();
        int ranValue = Random.Range(0, vars.bgThemeSpriteList.Count);
        m_spriterender.sprite = vars.bgThemeSpriteList[ranValue];

	}

	void Start()
    {
        
    }


    void Update()
    {
        
    }
}
