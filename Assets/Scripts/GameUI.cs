using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameUI : MonoBehaviour
{
    public Image levelSlider;
    public Image currentLevelImg;
    public Text currentLevelText;
    public Image nextLevelImg;
    public Text nextLevelText;

    public GameObject settingBTN;
    public GameObject allBTN;

    public GameObject soundONBTN;
    public GameObject soundOFFBTN;
    public bool soundonoffBo;

    public bool buttonSettingBo;

    public GameObject homeUI;
    public GameObject gameUI;

    private PlayerController player;

    public Material playerMat; //PlayerMaterial
    void Start()
    {
        playerMat = FindObjectOfType<PlayerController>().transform.GetChild(0).GetComponent<MeshRenderer>().material;

        player = FindObjectOfType<PlayerController>();

        levelSlider.color = playerMat.color;

        currentLevelImg.color = playerMat.color;

        nextLevelImg.color = playerMat.color;

        soundONBTN.GetComponent<Button>().onClick.AddListener(call: (() => SoundManager.Instance.soundOnOff()));
        soundOFFBTN.GetComponent<Button>().onClick.AddListener(call: (() => SoundManager.Instance.soundOnOff()));

        UpdateLevelUI();
       
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !ignoreUI() && player.playerState == PlayerController.PlayerState.Preper)
        {
            player.playerState = PlayerController.PlayerState.Playing;
            homeUI.SetActive(false);
            gameUI.SetActive(true);
        }

        if (SoundManager.Instance.sound)
        {
            soundONBTN.SetActive(true);
            soundOFFBTN.SetActive(false);
        }
        else
        {
            soundONBTN.SetActive(false);
            soundOFFBTN.SetActive(true);
        }
    }

    private bool ignoreUI()
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        for (int i = 0; i < raycastResults.Count; i++)
        {
            if (raycastResults[i].gameObject.GetComponent<IgnoreGameUI>() != null)
            {
                raycastResults.RemoveAt(i);
                i--;
            }
        }

        return raycastResults.Count > 0;
    }

    public void LevelSliderFill(float fillAmount)
    {
        levelSlider.fillAmount = fillAmount;
    }

    public void SettingShow()
    {
        buttonSettingBo = !buttonSettingBo;
        allBTN.SetActive(buttonSettingBo);
    }

    public void UpdateLevelUI() 
    {
        currentLevelText.text = "" + PlayerPrefs.GetInt("Level");
        nextLevelText.text = "" + (PlayerPrefs.GetInt("Level") + 1);
    }
}
