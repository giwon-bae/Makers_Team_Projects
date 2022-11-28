using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject[] Panels;
    public GameObject[] Abilities;
    public GameObject[] ShowPos;
    public GameObject[] Patterns;
    public Image[] HpImages;
    public Slider ExpBar;

    private int[] abilityIndex = new int[3];
    private Queue<int> platformindices = new Queue<int>();
    private float platformWidth = 17.7f;
    private int randomPlatformIdx;

    [SerializeField] PlayerController playerController;
    [SerializeField] Image kickButtonImage;

    void Awake()
    {
        platformindices.Enqueue(0);
        SelectPlatform();
        platformindices.Enqueue(randomPlatformIdx);
        Patterns[randomPlatformIdx].SetActive(true);
        Patterns[randomPlatformIdx].transform.position = new Vector2(platformWidth, 0);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            CoolDownIcon();
            ShowExpBar();
            PlatformPositioning();
        }
    }

    public void AbilityEnforce()
    {
        Panels[1].SetActive(true);
        SelectAbility();
        ShowAbility();
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

    public void GameStart()
    {
        Panels[2].SetActive(false);
        SceneManager.LoadScene("Main");
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        Panels[2].SetActive(true);
        Time.timeScale = 0;
        //check score
    }

    public void ShowLog(int index)
    {
        for (int i = 0; i < 3; i++)
        {
            Abilities[abilityIndex[i]].SetActive(false);
        }
        Panels[1].SetActive(false);
        Debug.Log(index);

        switch (index)
        {
            case 0:

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
                playerController.GiganticAbility();
                break;
        }

        Time.timeScale = 1;
    }

    private void SelectAbility() //랜덤으로 3개의 능력강화 선택
    {
        bool[] checkIndex = new bool[Abilities.Length];

        for (int i=0; i<Abilities.Length; i++)
        {
            checkIndex[i] = false;
        }

        if (playerController.shield.activeSelf)
        {
            checkIndex[1] = true;
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

    private void ShowAbility() //선택된 능력강화들 UI에 띄우기
    {
        for(int i=0; i<3; i++)
        {
            Abilities[abilityIndex[i]].SetActive(true);
            Abilities[abilityIndex[i]].transform.position = ShowPos[i].transform.position;
        }
    }

    private void ShowExpBar()
    {
        ExpBar.value = (float)playerController.cur_exp / playerController.req_exp;
    }

    private void CoolDownIcon()
    {
        kickButtonImage.fillAmount = (playerController.curKickCool / playerController.kickDelay);
    }

    private void SelectPlatform()
    {
        randomPlatformIdx = Random.Range(1, Patterns.Length);

        while (Patterns[randomPlatformIdx].activeSelf)
        {
            randomPlatformIdx = Random.Range(0, Patterns.Length);
        }
    }

    private void PlatformPositioning()
    {
        if (Patterns[platformindices.Peek()].transform.position.x > -platformWidth)
        {
            return;
        }
        //Debug.Log(platformindices.Peek());
        //Debug.Log(Patterns[platformindices.Peek()].transform.position.x);

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
    }
}
