using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField]
    GameObject lifeSupportWarning;

    [SerializeField]
    Text lifeSupportTimer;

    [SerializeField]
    GameObject warpText;

    [SerializeField]
    GameObject warpDamaged;

    [SerializeField]
    GameObject lifeSupportDamaged;

    [SerializeField]
    GameObject outOfFuel;

    [SerializeField]
    GameObject youDied;

    [SerializeField]
    GameObject happyEnding;

    [SerializeField]
    GameObject sadEnding;

    [SerializeField]
    GameObject verySadEnding;

    [SerializeField]
    GameObject quitMenu;

    int lastTimeLeft = int.MaxValue;

    private MenuState state = MenuState.NONE;

    MotherShip motherShip;

    enum MenuState
    {
        QUITMENU,
        WIN,
        LOSE,
        NONE
    }

    // Start is called before the first frame update
    void Start()
    {
        motherShip = GameObject.FindGameObjectWithTag("MotherShip").GetComponent<MotherShip>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state == MenuState.NONE)
            {
                state = MenuState.QUITMENU;
                ShowMenu();
            }
            else if (state == MenuState.QUITMENU)
            {
                HideMenu();
                state = MenuState.NONE;
            }
            else
            {
                Application.Quit();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (state != MenuState.NONE)
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (state == MenuState.QUITMENU)
            {
                Application.Quit();
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Win();
        }
    }

    public void UpdateLifeSupportTimer(float timeLeft)
    {
        int nowLeft = (int)(timeLeft * 100f);
        if (lastTimeLeft > nowLeft)
        {
            lifeSupportTimer.text = nowLeft.ToString() + "%";
            lastTimeLeft = nowLeft;
        }
    }

    public void ShowLifeSupportWarning()
    {
        lastTimeLeft = int.MaxValue;
        lifeSupportWarning.SetActive(true);
    }

    public void HideLifeSupportWarning()
    {
        lifeSupportWarning.SetActive(false);
    }

    public void ShowWarpText()
    {
        warpText.SetActive(true);
    }

    public void HideWarpText()
    {
        warpText.SetActive(false);
    }

    public void ShowWarpDamaged()
    {
        warpDamaged.SetActive(true);
    }

    public void HideWarpDamaged()
    {
        warpDamaged.SetActive(false);
    }

    public void ShowLifeSupportDamaged()
    {
        lifeSupportDamaged.SetActive(true);
    }

    public void HideLifeSupportDamaged()
    {
        lifeSupportDamaged.SetActive(false);
    }

    public void ShowOutOfFuel()
    {
        outOfFuel.SetActive(true);
    }

    public void HideOutOfFuel()
    {
        outOfFuel.SetActive(false);
    }

    public void HappyEnding()
    {
        state = MenuState.WIN;
        happyEnding.SetActive(true);
    }

    public void SadEnding()
    {
        state = MenuState.WIN;
        sadEnding.SetActive(true);
    }

    public void VerySadEnding()
    {
        state = MenuState.WIN;
        verySadEnding.SetActive(true);
    }

    public void YouDied()
    {
        state = MenuState.LOSE;
        youDied.SetActive(true);
    }

    public void ShowMenu()
    {
        quitMenu.SetActive(true);
    }

    public void HideMenu()
    {
        quitMenu.SetActive(false);
    }

    public void Win()
    {
        switch(motherShip.countLifeSupports())
        {
            case 3:
                HappyEnding();
                break;
            case 2:
                SadEnding();
                break;
            default:
                VerySadEnding();
                break;
        }
    }
}
