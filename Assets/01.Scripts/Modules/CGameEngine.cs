using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameEngine : MonoBehaviour
{
    #region SingleTon
    public static CGameEngine _instance = null;

    public static CGameEngine Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("CGameEngine install null");

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            _instance = null;
        }
    }
    #endregion

    public GameObject m_goPlayer;

    private Vector3 m_vecOriginCameraPoz;

    private CWeaponEX m_cWeaponEX;

    private int m_nPlayState;

    // Start is called before the first frame update
    void Start()
    {
        // CPlayer.Instance.PlayPlayer();
        m_vecOriginCameraPoz = Camera.main.transform.localPosition; 
        // Vector3 vecGamePoz = m_vecOriginCameraPoz;
        // vecGamePoz.z += 1f;
        // Camera.main.transform.localPosition = vecGamePoz;

        // Debug.Log(string.Format("{0} 테스트입니다.", 20));
        
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 vecPlayerPoz = m_goPlayer.transform.localPosition;
        Vector3 vecPlayerObjectPoz = m_goPlayer.GetComponent<CPlayer>().GetPlayerGameObject().transform.localPosition;
        Vector3 vecNewPoz = m_vecOriginCameraPoz;
        vecNewPoz.z = (vecPlayerPoz.z + vecPlayerObjectPoz.x);
        Camera.main.transform.localPosition = vecNewPoz;
    }

    public void GameStart()
    {
        // int nLevel = int.Parse(CUIInGameManager.Instance.m_ifUserLevel.text);
        int nLevel = 3;
        int nHP = CGameInfo.Instance.GetHPByUnitInfo(0, nLevel);

        CTowerInfo cTowerInfo = CGameInfo.Instance.GetTowerInfo(1, DefineData.TEST_FLOOR);

        CEnemysManager.Instance.InitEnemy(cTowerInfo);
        // CPlayer.Instance.PlayPlayer();   

        Debug.Log("User Level : " + nLevel);
        
        CWeaponInfo cWeaponInfo = CGameInfo.Instance.GetWeaponInfo(DefineData.TEST_MY_WEAPON_INDEX);

        m_goPlayer.GetComponent<CCharacter>().InitCharacter(DefineData.CHARACTER_TYPE_PLAYER, 0, nLevel, nHP, m_cWeaponEX);
        m_goPlayer.GetComponent<CCharacter>().SetTarget(CEnemysManager.Instance.GetEnemyGameObject(0));
        m_goPlayer.GetComponent<CCharacter>().Attack();

        CUIInGameManager.Instance.SetState(DefineData.INGAME_PLAY);
    }

    public void PlaySkill(int nIndex)
    {
        int nSkillIndex = 20000;
        if(nIndex == 0)
            nSkillIndex = 20000;
        if(nIndex == 1)
            nSkillIndex = 1;

        m_goPlayer.GetComponent<CCharacter>().StandBySkill(nSkillIndex);
    }

    public GameObject GetPlayer()
    {
        return m_goPlayer;
    }

    public void SetWeaponEX(CWeaponEX cWeaponEX)
    {
        m_cWeaponEX = cWeaponEX;
    }
}
