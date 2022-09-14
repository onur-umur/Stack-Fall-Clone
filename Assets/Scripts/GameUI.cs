using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Image levelSlider;
    public Image currentLevelImg;
    public Image nextLevelImg;
    public Material playerMat;
    public GameObject settingBTN;
    public GameObject allBTN;
    public bool buttonSettingBo;
    public GameObject SoundOnBTN;
    public GameObject SoundOffBTN;
    public bool soundOnOffBo;
    public GameObject homeUI;
    public GameObject gameUI;

    private PlayerController player;
    private object ray;

    void Start()
    {
        playerMat = FindObjectOfType<PlayerController>().transform.GetChild(0).GetComponent<MeshRenderer>().material;


        levelSlider.transform.parent.GetComponent<Image>().color = playerMat.color + Color.gray;

        levelSlider.color = playerMat.color;
        currentLevelImg.color = playerMat.color;
        nextLevelImg.color = playerMat.color;

        SoundOnBTN.GetComponent<Button>().onClick.AddListener(call: (() => SoundManager.instance.SoundOnOff()));
        SoundOffBTN.GetComponent<Button>().onClick.AddListener(call: (() => SoundManager.instance.SoundOnOff()));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !ignoreUI() && player.playerstate == PlayerController.PlayerState.Prepare)
        {
            player.playerstate = PlayerController.PlayerState.Playing;
            homeUI.SetActive(false);
            gameUI.SetActive(true);

        }



        if (SoundManager.instance.sound)
        {
            SoundOnBTN.SetActive(true);
            SoundOffBTN.SetActive(false);
        }
        else
        {
            SoundOnBTN.SetActive(false);
            SoundOffBTN.SetActive(true);

        }


    }
    
    private bool ignoreUI()
    {
        
        PointerEventData pointerEventData=new PointerEventData(EventSystem.current);
        pointerEventData.position=Input.mousePosition;
        List<RaycastResult> raycastResult = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResult);
        for(int i =0;  i < raycastResult.Count; i++)
        {
            if (raycastResult[i].gameObject.GetComponent<IgnoreGameUI>() != null)
            {
                raycastResult.RemoveAt(i);
                i--;
            }
        }
        return raycastResult.Count > 0;
    }
    public void LevelSliderFill(float fillAmount)
    {
        levelSlider.fillAmount = fillAmount;

    }

    public void settingShow()
    {
        buttonSettingBo = !buttonSettingBo;
        allBTN.SetActive(buttonSettingBo);


    }


}
