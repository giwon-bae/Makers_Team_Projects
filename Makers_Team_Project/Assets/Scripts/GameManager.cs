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
    public Image[] HpImages;

    private int[] abilityIndex = new int[3];

    [SerializeField] PlayerController playerController;
    [SerializeField] Image kickButtonImage;

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            CoolDownIcon();
        }
    }

    public void AbilityEnforce()
    {
        Panels[0].SetActive(true);
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
        SceneManager.LoadScene("Main");
    }

    public void GameOver()
    {
        //show gameover UI (message, restart button etc..)
        //timescale = 0
        //check score
    }

    public void ShowLog(int index)
    {
        for (int i = 0; i < 3; i++)
        {
            Abilities[abilityIndex[i]].SetActive(false);
        }
        Panels[0].SetActive(false);
        Debug.Log(index);

        switch (index)
        {
            case 0:

                break;
            case 1:
                playerController.shield.SetActive(true);
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

    private void CoolDownIcon()
    {
        kickButtonImage.fillAmount = (playerController.curKickCool / playerController.kickDelay);
    }
}
