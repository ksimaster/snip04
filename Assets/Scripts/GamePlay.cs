using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GamePlay : MonoBehaviour
{
    public static GamePlay Instance { get; private set; }

    public Level_Manager level_Manager;

    public int Reward;
    public int Bonus;
    public int Total;
    public int score = 0;
    public Text Score_Text;

    public Text Level_Reward;
    public Text Bonus_Reward;
    public Text Total_Reward;

    public GameObject WinPanel;
    public GameObject PauseMenu;

    public GameObject MiniMap;
    public GameObject Player;
    //public MonoBehaviour Controls;
    public static bool IsPC = true;

    private void Awake()
    {
        /*
        if (Instance != null)
        {
            // ToDo: understand
            Destroy(gameObject);
            return;
        }
        */
        if (IsPC)
        {
            Player.GetComponent<FPSInputController>().enabled = true;
            PauseMenu.SetActive(false);
            GameObject.FindGameObjectWithTag("PauseButton").SetActive(false);
            GameObject.FindGameObjectWithTag("InfoButton").SetActive(false);
            GameObject.FindGameObjectWithTag("CTKCanvas").SetActive(false);
        }
        else
        {
            Player.GetComponent<FPSInputControllerMobile>().enabled = true;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            try
            {
                PauseMenu.SetActive(true);
                PauseMenu.GetComponent<DOTweenAnimation>().DORestart();
            }
            catch { }

            UnlockMouse();
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        level_Manager = FindObjectOfType<Level_Manager>();
        //GameManager.Instance.RemainingTime = 30f;
    }

    public void UpdateScore()
    {
        //kill score...
        score += 1;
        Score_Text.text = score.ToString();
        //coins addition...
        Reward = Random.Range(200, 500);
        Bonus = Reward + Random.Range(100, 200);
        Total = Reward + Bonus;
        //Reward Display...
        Level_Reward.text = Reward.ToString();
        Bonus_Reward.text = Bonus.ToString();
        Total_Reward.text = Total.ToString();
        //Coins Update...
        GameManager.Instance.Coins += Total;
        if (GameManager.Instance.Unlocked_Level == GameManager.Instance.Selected_Level && GameManager.Instance.Unlocked_Level < 14)
        {
            GameManager.Instance.Unlocked_Level++;
        }
        GameManager.Instance.SaveUserData();

        if (score >= level_Manager.count)
        {
            StartCoroutine(Level_Completed());
        }
    }

    IEnumerator Level_Completed()
    {
        if (IsPC)
        {
            UnlockMouse();
        }
        Debug.Log("Level" + level_Manager.count + " Completed");
        MiniMap.SetActive(false);
        Player.SetActive(false);
        WinPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
    }

    public static void UnlockMouse()
    {
        MouseLock.MouseLocked = false;
    }

    public static void LockMouse()
    {
        MouseLock.MouseLocked = true;
    }

}
