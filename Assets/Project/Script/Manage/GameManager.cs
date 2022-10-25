using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Fungus;

public class GameManager : MonoBehaviour
{
    public Image playerImage;
    public Text playerName;
    public Slider hpSlider;
    public Slider spSlider;
    public Image as1;
    public Image as2;
    public Image as3;
    public Image ps1;
    public Image ps2;
    public Image ps3;
    private Player player;
    private Transform player_transform;
    public void SetPlayer(Player player)
    {
        this.player = player;
        player_transform = player.transform;
    }
    public CinemachineVirtualCamera _camera;
    public Flowchart talkFlowchart;

    private Sprite akane;
    private Sprite aoi;
    private Sprite panzyan;
    private Sprite chokominto;

    public bool isBossBattle;
    public bool isBossReady;
    [SerializeField]private float bossPlayerPositionX;
    [SerializeField] private float bossCameraX;
    [SerializeField] private float bossCameraY;
    [SerializeField] private float cameraSpeed=3.0f;

    private void Start()
    {
        akane = Resources.Load<Sprite>("akaneFace");
        aoi = Resources.Load<Sprite>("aoiFace");
        panzyan = Resources.Load<Sprite>("panzyanPanel");
        chokominto = Resources.Load<Sprite>("chokomintoPanel");
        ChangeToAkane();
        SetBossBattle();
    }

    private void Update()
    {
        /*Debug.Log(player_transform.position.x);
        if ((player_transform.position.x >= bossPlayerPositionX)&&!isBossBattle)
        {
            SetBossBattle();
        }
        if (isBossBattle&&!isBossReady)
        {
            MoveCamera();
            if (isBossReady)
            {
                talkFlowchart.gameObject.SetActive(true);
            }
        }*/
    }

    private void MoveCamera()
    {
        Vector3 position = _camera.transform.position;
        float x = position.x;
        float y = position.y;
        if (x <= bossCameraX)
        {
            x += cameraSpeed * Time.deltaTime;
        }
        else
        {
            x = bossCameraX;
        }
        if (y - bossCameraY <= -0.2f)
        {
            y += cameraSpeed * Time.deltaTime;
        }
        else if (y - bossCameraY >= 0.2f)
        {
            y -= cameraSpeed * Time.deltaTime;
        }
        else
        {
            y = bossCameraY;
        }
        _camera.transform.position = new Vector3(x, y,position.z);
        if (x == bossCameraX && y == bossCameraY)
        {
            isBossReady = true;
        }
    }

    private void SetBossBattle()
    {
       //isBossBattle = true;
        _camera.m_Lens.OrthographicSize = 8f;
        _camera.Follow = null;
    }

    public void ChangeToAkane()
    {
        playerImage.sprite = akane;
        playerName.text = "あかね";
        if (player != null)
        {
            SetSkillImage(player.akaneActiveSkills, player.akanePassiveSkills);
        }
        //_camera.m_Lens.OrthographicSize = 1f;
    }

    public void ChangeToAoi()
    {
        playerImage.sprite = aoi;
        playerName.text = "あおい";
        if (player != null)
        {
            SetSkillImage(player.aoiActiveSkills, player.aoiPassiveSkills);
        }
    }

    public void SetHp(int hp,int MaxHp)
    {
        float var = (float)hp / MaxHp;
        hpSlider.value = var;
    }

    public void SetSP(int sp, int MaxSp)
    {
        float var = (float)sp / MaxSp;
        spSlider.value = var;
    }

    public void SetSkillImage(SkillEnum[] activeSkills,SkillEnum[] passiveSkills)
    {
        for(int i = 0; i < activeSkills.Length; i++)
        {
            SetSkillImage(false, i+1, activeSkills[i]);
        }
        for (int i = 0; i < passiveSkills.Length; i++)
        {
            SetSkillImage(true, i+1, passiveSkills[i]);
        }
    }

    private void SetSkillImage(bool isPassive,int Number,SkillEnum skill)
    {
        if (isPassive)
        {
            switch (Number)
            {
                case 1:
                    SetImage(ps1, skill);
                    break;
                case 2:
                    SetImage(ps2, skill);
                    break;
                case 3:
                    SetImage(ps3, skill);
                    break;
            }
        }
        else
        {
            switch (Number)
            {
                case 1:
                    SetImage(as1, skill);
                    break;
                case 2:
                    SetImage(as2, skill);
                    break;
                case 3:
                    SetImage(as3, skill);
                    break;
            }
        }
    }

    private void SetImage(Image image,SkillEnum skill)
    {
        switch (skill)
        {
            case SkillEnum.PANZYAN:
                image.sprite = panzyan;
                break;
            case SkillEnum.CHOKOMINTO:
                image.sprite = chokominto;
                break;
            case SkillEnum.NONE:
                break;
            default:
                Debug.Log("GameManagerのSetImageでバグ");
                break;
        }
    }
}
