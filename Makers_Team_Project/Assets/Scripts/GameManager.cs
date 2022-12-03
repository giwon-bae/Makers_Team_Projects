using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float playTime;

    public GameObject[] Panels;
    public GameObject[] Abilities;
    public GameObject[] ShowPos;
    public GameObject[] Patterns;
    public Image[] HpImages;
    public Text[] timeTxt;
    public AudioClip[] audioClips;
    public RectTransform ExpBar;
    public RectTransform DistanceBar;
    public AudioSource audioSource;
    public Bullet bullet;

    private Queue<int> platformindices = new Queue<int>();
    private float platformWidth = 17.7f;
    private int[] abilityIndex = new int[3];
    private int randomPlatformIdx;
    private int mapCount = 2;
    private int summonBossTime = 24;

    [SerializeField] PlayerController playerController;
    [SerializeField] Animator playerAnimator;
    [SerializeField] GameObject Boss;
    [SerializeField] Image kickButtonImage;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            GameStart();
            audioSource.clip = audioClips[0];
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            CoolDownIcon();
            ShowBar();
            PlatformPositioning();

            if (mapCount > summonBossTime + 1)
            {
                Boss.SetActive(true);
            }

            playTime += Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            int min = (int)(playTime / 60);
            int sec = (int)(playTime % 60);
            timeTxt[0].text = string.Format("{0:00}", min) + ":" + string.Format("{0:00}", sec);
        }
    }

    public void UpdateHpIcon(int num)
    {
        for(int i=0; i< HpImages.Length; i++)
        {
            HpImages[i].color = new Color(0, 0, 0, 0);
        }

        for (int i = 0; i < num; i++)
        {
            HpImages[i].color = new Color(255, 0, 0, 1);
        }
    }

    public void GoToTitleScene()
    {
        SceneManager.LoadScene("Title");
    }

    public void GoToMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void GameStart()
    {
        Panels[0].SetActive(true);
        for(int i=1; i<Panels.Length; i++)
        {
            Panels[i].SetActive(false);
        }

        platformindices.Enqueue(0);
        SelectPlatform();
        platformindices.Enqueue(randomPlatformIdx);
        Patterns[randomPlatformIdx].SetActive(true);
        Patterns[randomPlatformIdx].transform.position = new Vector2(platformWidth, 0);
        bullet.damage = 2;

        Time.timeScale = 1;
    }

    public void GameOver()
    {
        Panels[2].SetActive(true);
        Time.timeScale = 0;
    }

    public void GameClear()
    {
        Panels[3].SetActive(true);
        //playerAnimator.SetBool("IsBossDead", true);
        Time.timeScale = 0;
        int min = (int)(playTime / 60);
        int sec = (int)(playTime % 60);
        timeTxt[1].text = "현재 기록 " + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", sec);

        float HighRecord = PlayerPrefs.GetFloat("High Record");
        if(HighRecord == default)
        {
            HighRecord = 60 * 99;
        }

        if(playTime < HighRecord)
        {
            timeTxt[2].text = "최고 기록 " + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", sec);
            PlayerPrefs.SetFloat("High Record", playTime);
        }
        else
        {
            int h_min = (int)(HighRecord / 60);
            int h_sec = (int)(HighRecord % 60);
            timeTxt[2].text = "최고 기록 " + string.Format("{0:00}", h_min) + ":" + string.Format("{0:00}", h_sec);
        }
        
    }

    public void GetAbility(int index) // 능력 획득
    {
        for (int i = 0; i < 3; i++)
        {
            Abilities[abilityIndex[i]].SetActive(false);
        }
        Panels[1].SetActive(false);

        switch (index)
        {
            case 0:
                bullet.damage += 1;
                break;
            case 1:
                playerController.shield.SetActive(true);
                break;
            case 2:
                playerController.fireDelay *= 0.8f;
                break;
            case 3:
                playerController.kickDelay *= 0.8f;
                break;
            case 4:
                playerController.invincibilityDuration += 1f;
                break;
            case 5:
                playerController.StopCoroutine("Gigantic");
                playerController.StartCoroutine("Gigantic");
                break;
        }

        if(index != 5)
        {
            playerController.StopCoroutine("Invincibility");
            playerController.StartCoroutine("Invincibility");
        }
        
        Time.timeScale = 1;
    }

    public void AbilityEnforce() // 능력 강화 함수 실행
    {
        Panels[1].SetActive(true);
        SelectAbility();
        ShowAbility();
    }

    private void SelectAbility() // 랜덤으로 3개의 능력강화 선택
    {
        bool[] checkIndex = new bool[Abilities.Length];

        for (int i=0; i<Abilities.Length; i++)
        {
            checkIndex[i] = false;
        }

        //예외
        if (playerController.shield.activeSelf)
        {
            checkIndex[1] = true;
        }
        if (bullet.damage == 5)
        {
            checkIndex[0] = true;
        }
        
        for(int i=0; i<3; i++)
        {
            int tmpIndex = Random.Range(0, Abilities.Length);
            while (checkIndex[tmpIndex])
            {
                tmpIndex = Random.Range(0, Abilities.Length);
            }
            abilityIndex[i] = tmpIndex;
            checkIndex[tmpIndex] = true;
        }
    }

    private void ShowAbility() // 선택된 능력강화 버튼 UI에 띄우기
    {
        for(int i=0; i<3; i++)
        {
            Abilities[abilityIndex[i]].SetActive(true);
            Abilities[abilityIndex[i]].transform.position = ShowPos[i].transform.position;
        }
    }

    private void ShowBar()
    {
        float expBarLength = (float)playerController.cur_exp / playerController.req_exp * 600f;
        if(expBarLength > 600f)
        {
            expBarLength = 600f;
        }
        ExpBar.sizeDelta = new Vector2(expBarLength, ExpBar.sizeDelta.y);
        DistanceBar.sizeDelta = new Vector2((float)(mapCount - 2) / summonBossTime * 600f, DistanceBar.sizeDelta.y);
    }

    private void CoolDownIcon()
    {
        kickButtonImage.fillAmount = (playerController.curKickCool / playerController.kickDelay);
    }

    private void SelectPlatform()
    {
        if (mapCount > summonBossTime)
        {
            randomPlatformIdx = Random.Range(1, 4);

            while (Patterns[randomPlatformIdx].activeSelf)
            {
                randomPlatformIdx = Random.Range(1, 4);
            }
            return;
        }
        randomPlatformIdx = Random.Range(4, Patterns.Length);

        while (Patterns[randomPlatformIdx].activeSelf)
        {
            randomPlatformIdx = Random.Range(4, Patterns.Length);
        }
    }

    private void PlatformPositioning()
    {
        if (Patterns[platformindices.Peek()].transform.position.x > -platformWidth)
        {
            return;
        }

        SelectPlatform();

        //Activation
        platformindices.Enqueue(randomPlatformIdx);
        Patterns[randomPlatformIdx].SetActive(true);
        Patterns[randomPlatformIdx].transform.position = new Vector2(platformWidth, 0);

        //Disabled
        for(int i=0; i<Patterns[platformindices.Peek()].transform.childCount; i++)
        {
            Patterns[platformindices.Peek()].transform.GetChild(i).gameObject.SetActive(true);
        }
        Patterns[platformindices.Dequeue()].SetActive(false);

        if(mapCount-2 < summonBossTime)
        {
            mapCount++;
        }
    }
}
