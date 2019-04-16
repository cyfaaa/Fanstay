using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 平台生成
/// </summary>

public enum PlatformGroupType{
    Grass,
    Winter,
}
public class PlatformSpawner : MonoBehaviour
{
    public Vector3 startSpawnPos;

    /// <summary>
    /// 里程碑数
    /// </summary>
    public int mileStoneCount = 10;
    public float fallTime;
    public float minfalltime;
    public float multiple;
    private Managevars vars;
    private Vector3 platformSpawnPosition;

    /// <summary>
    /// 是否超左边生成，反之朝右
    /// </summary>
    private bool isLeftSpawn = false;

    /// <summary>
    /// 选择的平台图
    /// </summary>
    private Sprite selectPlatformSprite;

    /// <summary>
    /// 生成平台数量
    /// </summary>
    private int spawnPlatformCount;

    private PlatformGroupType groupType;

    /// <summary>
    /// 钉子组合平台是否生成在左边，反之右边
    /// </summary>
    private bool spikeSpawnLeft = false;

    /// <summary>
    /// 钉子方向平台的位置
    /// </summary>
    private Vector3 spikeDirPlarformPos;

    /// <summary>
    /// 生成钉子平台之后需要在钉子方向生成的平台数量
    /// </summary>
    private int afterSpawnSpikeCount;
    private bool isSpawnSpike = false ;

	void Awake()
	{
        EventCenter.AddListener(EventDefine.DecidePath, DecidePath);
        vars = Managevars.GetManagevars();
	}

	void OnDestroy()
	{
        EventCenter.RemoveListener(EventDefine.DecidePath, DecidePath);
	}

	void Start()
    {
        RandomPlatformTheme();
        platformSpawnPosition = startSpawnPos;
        for (int i = 0; i < 5; i++)
        {
            //每一回合生成5个平台
            spawnPlatformCount = 5;
            DecidePath();
        }

        //生成人物
        GameObject go = Instantiate(vars.characterPrefab);
        go.transform.position = new Vector3(0, -1.8f, 0); 
    }

    /// <summary>
    /// 随机平台主题
    /// </summary>
    private void RandomPlatformTheme()
    {
        int ran = Random.Range(0, vars.platformThemeSpriteList.Count);
        selectPlatformSprite = vars.platformThemeSpriteList[ran];

        if(ran == 2 )
        {
            groupType = PlatformGroupType.Winter;
        }
        else
        {
            groupType = PlatformGroupType.Grass;
        }

    }

	void Update()
    {
        if (GameManager.Instance.IsGameStarted && GameManager.Instance.isGameOver == false)
        {
            UpdateFallTime();
        }

    }

    /// <summary>
    /// 更新平台掉落时间
    /// </summary>
	private void UpdateFallTime()
    {
        if(GameManager.Instance.GetGameScore() > mileStoneCount)
        {
            mileStoneCount *= 2;
            fallTime *= multiple;
            if(fallTime<minfalltime)
            {
                fallTime = minfalltime;
            }
        }
    }

    /// <summary>
    /// 确定路径
    /// </summary>
    private void DecidePath()
    {
        if(isSpawnSpike)
        {
            AfterSpawnSpike();
            return;
        }
        if (spawnPlatformCount > 0)
        {
            spawnPlatformCount--;
            SpawnPlatform();
        }
        else
        {
            //反转生成方向
            isLeftSpawn = !isLeftSpawn;
            spawnPlatformCount = Random.Range(1, 4);
            SpawnPlatform();
        }
    }

    /// <summary>
    /// 生成平台
    /// </summary>
    private void SpawnPlatform()
    {
        int ranObstacle = Random.Range(0, 2);
        //生成单个平台
        if(spawnPlatformCount >= 1)
        {
            SpawnNormalPlatform(ranObstacle);
        }

        //生成组合平台
        else if(spawnPlatformCount == 0)
        {
            int ran = Random.Range(0, 3);
            //生成通用组合平台
            if(ran == 0)
            {
                SpawnCommonPlatformGroup(ranObstacle);
            }
            //生成主题组合平台
            else if(ran == 1)
            {
                switch(groupType)
                {
                    case PlatformGroupType.Grass:
                        SpawnGrassPlatformGroup(ranObstacle);
                        break;
                    case PlatformGroupType.Winter:
                        SpawnWinterPlatformGroup(ranObstacle);
                        break;
                    default:
                        break;
                }
            }
            //生成钉子组合平台
            else
            {
                int value = -1;
                if(isLeftSpawn)
                {
                    value = 0;// 生成右边方向得钉子
                }
                else
                {
                    value = 1;// 生成左边方向得钉子
                }
                SpawnSpikePlatformGroup(value);

                isSpawnSpike = true;
                afterSpawnSpikeCount = 4;

                if(spikeSpawnLeft)//钉子在左边
                {
                    spikeDirPlarformPos = new Vector3(platformSpawnPosition.x - 1.65f, platformSpawnPosition.y + vars.nextYPos, 0);
                }
                else
                {
                    spikeDirPlarformPos = new Vector3(platformSpawnPosition.x + 1.65f, platformSpawnPosition.y + vars.nextYPos, 0);
                }

            }
        }

        int ranSpawnDiamond = Random.Range(0, 10);
        if(ranSpawnDiamond == 6&&GameManager.Instance.PlayerIsMove==true)
        {
            GameObject Diamond = ObjectPool.Instance.GetDiamond();
            Diamond.transform.position = new Vector3(platformSpawnPosition.x, platformSpawnPosition.y + 0.5f, 0);
            Diamond.SetActive(true);
        }
        if(isLeftSpawn) //向左生成
        {
            platformSpawnPosition = platformSpawnPosition + new Vector3(-vars.nextXPos, vars.nextYPos, 0);
        }
        else
        {
            platformSpawnPosition = platformSpawnPosition + new Vector3(vars.nextXPos, vars.nextYPos, 0);
        }

    }

    /// <summary>
    /// 生成普通平台（单个）
    /// </summary>
    private void SpawnNormalPlatform(int ranObstacle)
    {
        GameObject go = ObjectPool.Instance.GetNormalPlatform();
        //GameObject go = Instantiate(vars.NormalPlatform, transform);
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, ranObstacle);
        go.SetActive(true);

    }

    /// <summary>
    /// 生成通用组合平台
    /// </summary>
    private void SpawnCommonPlatformGroup(int ranObstacle)
    {
        GameObject go = ObjectPool.Instance.GetCommonPlatformGroup();
        /*int ran = Random.Range(0, vars.commonPlatformGroup.Count);
        GameObject go = Instantiate(vars.commonPlatformGroup[ran], transform);*/
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, ranObstacle);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成草地组合平台
    /// </summary>
    private void SpawnGrassPlatformGroup(int ranObstacle)
    {
        /*int ran = Random.Range(0, vars.grassPlatformGroup.Count);
        GameObject go = Instantiate(vars.grassPlatformGroup[ran], transform);*/
        GameObject go = ObjectPool.Instance.GetGrassPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, ranObstacle);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成冬季组合平台
    /// </summary>
    private void SpawnWinterPlatformGroup(int ranObstacle)
    {
        GameObject go = ObjectPool.Instance.GetWinterPlatformGroup();
        /*int ran = Random.Range(0, vars.winterPlatformGroup.Count);
        GameObject go = Instantiate(vars.winterPlatformGroup[ran], transform);*/
        go.transform.position = platformSpawnPosition;
        go.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, ranObstacle);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成钉子组合平台
    /// </summary>
    /// <param name="dir"></param>
    private void SpawnSpikePlatformGroup(int dir)
    {
        GameObject temp = null;
        if(dir == 0)
        {
            spikeSpawnLeft = false;
            //temp = Instantiate(vars.spikePlatformRight, transform);
            temp = ObjectPool.Instance.GetRightSpikePlatform();
        }
        else
        {
            spikeSpawnLeft = true;
            //temp = Instantiate(vars.spikePlatformLeft, transform);
            temp = ObjectPool.Instance.GetLeftSpikePlatform();
        }
        temp.transform.position = platformSpawnPosition;
        temp .GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, dir);
        temp.SetActive(true);
    }

    /// <summary>
    /// 生成钉子平台之后需要生成的平台
    /// 包括钉子方向，也包括原来的方向
    /// </summary>
    private void AfterSpawnSpike( )
    {
        if(afterSpawnSpikeCount > 0)
        {
            afterSpawnSpikeCount--;
            for (int i = 0; i < 2; i++)
            {
                //GameObject temp = Instantiate(vars.NormalPlatform, transform);
                GameObject temp = ObjectPool.Instance.GetNormalPlatform();
                if(i == 0)//生成原来方向的平台
                {
                    temp.transform.position = platformSpawnPosition;
                    //如果钉子在左边，原先路径就是右边
                    if(spikeSpawnLeft)
                    {
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, 0);
                    }
                }
                else//生成钉子方向的平台
                {
                    temp.transform.position = spikeDirPlarformPos;
                    if(spikeSpawnLeft)
                    {
                        spikeDirPlarformPos = new Vector3(spikeDirPlarformPos.x - vars.nextXPos, spikeDirPlarformPos.y + vars.nextYPos, 0);
                    }
                    else
                    {
                        spikeDirPlarformPos = new Vector3(spikeDirPlarformPos.x + vars.nextXPos, spikeDirPlarformPos.y + vars.nextYPos, 0);
                    }
                }
                temp.GetComponent<PlatformScript>().Init(selectPlatformSprite, fallTime, 1);
                temp.SetActive(true);
            }
        }
        else
        {
            isSpawnSpike = false;
            DecidePath();
        }
    }
}
