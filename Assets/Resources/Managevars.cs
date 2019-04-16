using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 变量赋值,存储
/// </summary>
//[CreateAssetMenu(menuName ="CreatManagerVarsContainers")]

public class Managevars : ScriptableObject{
    public static Managevars GetManagevars()
    {
        return Resources.Load<Managevars>("ManagerVarsContainer");
    }

    public List<Sprite> bgThemeSpriteList = new List<Sprite>();
    public List<Sprite> platformThemeSpriteList = new List<Sprite>();

    public List<Sprite> skinSpriteList = new List<Sprite>();
    public GameObject skinChooseItemPre;


    public GameObject NormalPlatform;
    public float nextXPos = 0.554f, nextYPos = 0.645f;
    public GameObject characterPrefab;

    public List<GameObject> commonPlatformGroup = new List<GameObject>();
    public List<GameObject> grassPlatformGroup = new List<GameObject>();
    public List<GameObject> winterPlatformGroup = new List<GameObject>();
    public List<Sprite> characterSpriteList = new List<Sprite>(); 
    public List<string> skinPrice = new List<string>();


    public GameObject diamond;
    public GameObject spikePlatformLeft;
    public GameObject spikePlatformRight;

    public List<string> SkinNameList = new List<string>();
     

    public GameObject DeathEffect;

    public AudioClip JumpClip, FallClip, HitClip, DiamondClip, BtnClip;
    public Sprite AudioClipOn, AudioClipOff;


}
