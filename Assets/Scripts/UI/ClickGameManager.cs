using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickGameManager : MonoBehaviour
{
    private AudioSource m_audiosource;
    private Managevars vars;

	void Awake()
	{
        m_audiosource = gameObject.GetComponent<AudioSource>();
        vars = Managevars.GetManagevars();
        EventCenter.AddListener(EventDefine.PlayClickAudio, PlayAudio);
        EventCenter.AddListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
	}

	void OnDestroy()
	{
        EventCenter.RemoveListener(EventDefine.PlayClickAudio, PlayAudio);
        EventCenter.RemoveListener<bool>(EventDefine.IsMusicOn, IsMusicOn);
	}

	void PlayAudio()
    {
        m_audiosource.PlayOneShot(vars.BtnClip);
    }

    private void IsMusicOn(bool ismusicon)
    {
        m_audiosource.mute = !ismusicon;
    }

	void Start()
    {
        
    }


    void Update()
    {
        
    }
}
