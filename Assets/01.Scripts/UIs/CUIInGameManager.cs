using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

using TMPro;

using DG.Tweening;



public class CUIInGameManager : MonoBehaviour
{
    #region SingleTon
    public static CUIInGameManager _instance = null;

    public static CUIInGameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("CUIInGameManager install null");

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

    public GameObject[] m_arrUIBoard = new GameObject[2];
    public CPopupText m_cPopupText;

    public TextMeshProUGUI m_tmpHP;
    public Slider m_sliderHP;

    public InputField m_ifUserLevel;

    public GameObject m_goCutin;

    public GameObject m_goBossWarning;

    public GameObject[] m_arrResult = new GameObject[2];

    private int m_nState = 0;   

    #region Board Dev
    public GameObject m_goBtnStart;

    public Text m_txtWeaponName;
    public Text m_txtWeaponDamage;
    public Text m_txtWeaponLevel;

    public Text[] m_arrWeaponOption = new Text[4];

    public TextMeshProUGUI m_tmpWeaponDamage;    
    public TextMeshProUGUI[] m_arrWeaponOptions = new TextMeshProUGUI[4];

    private CWeaponEX m_cWeaponEX;
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        m_nState = DefineData.INGAME_READY;

        ShowBoard(DefineData.UI_INGAME_BOARD_DEV);
        InitDevBoard();

        HideResult();
        m_goCutin.SetActive(false);
        m_goBossWarning.SetActive(false);

        m_goBtnStart.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if( m_nState == DefineData.INGAME_READY )
            return;

        float fMaxHP = CGameEngine.Instance.GetPlayer().GetComponent<CCharacter>().GetMaxHP();
        float fHP = CGameEngine.Instance.GetPlayer().GetComponent<CCharacter>().GetHP();
        float fRemainHP = fHP / fMaxHP;

        if(fRemainHP >= 1)
            fRemainHP = 1f;

        m_sliderHP.value = fRemainHP;

        string strHP = ((int)fHP).ToString() + " / " + ((int)fMaxHP).ToString();
        m_tmpHP.text = strHP;
        /*
        float fMaxHP = CPlayer.Instance.GetMaxHP();
        float fHP = CPlayer.Instance.GetHP();
        float fRemainHP = fHP / fMaxHP;

        if(fRemainHP >= 1)
            fRemainHP = 1f;
        */
    }

    public void ShowBoard(int nIndex)
    {
        for(int i = 0; i < m_arrUIBoard.Length; i++)
        {
            m_arrUIBoard[i].SetActive(false);
        }

        m_arrUIBoard[nIndex].SetActive(true);
    }

    public void ShowPopupMsgByWorld(Vector3 vecWorldPosition, string strMsg, int nType = 0)
    {
        CPopupText cPopupText = GameObject.Instantiate<CPopupText>(m_cPopupText, this.gameObject.transform);
        cPopupText.InitPopupText(Camera.main.WorldToScreenPoint(vecWorldPosition),
                    strMsg, nType);
        cPopupText.Play();

        // inst.m_strDisplayText = strMsg;
        // inst.SetColor(0, 0, 255);
        // inst.transform.position = Camera.main.WorldToScreenPoint(vecWorldPosition);
        // inst.transform.position = new Vector3(0, 0, 0);
    }

    public void ShowCutin()
    {
        StartCoroutine("ProcessCutin");
    }

    IEnumerator ProcessCutin()
    {
        m_goCutin.SetActive(true);
        m_goCutin.GetComponent<Image>().DOFade(1f, 1f);

        yield return new WaitForSeconds(2f);

        m_goCutin.GetComponent<Image>().DOFade(0f, 0.5f);
        m_goCutin.SetActive(false);
    }

    public void ShowBossWarning()
    {
        StartCoroutine("ProcessShowBossWarning");
    }

    IEnumerator ProcessShowBossWarning()
    {
        yield return new WaitForSeconds(2f);
        m_goBossWarning.SetActive(true);
        yield return new WaitForSeconds(1f);
        m_goBossWarning.SetActive(false);
    }

    public void ShowResult()
    {
        HideResult();

        if( m_nState == DefineData.INGAME_SUCCESS )
        {
            m_arrResult[0].SetActive(true);
        } else if ( m_nState == DefineData.INGAME_FAIL )
        {
            m_arrResult[1].SetActive(true);
        }
    }

    public void HideResult()
    {
        for(int i = 0; i < m_arrResult.Length; i++)
        {
            m_arrResult[i].SetActive(false);
        }
    }

    public void SetState(int nState)
    {
        m_nState = nState;
    }

    public int GetState()
    {
        return m_nState;
    }

    public void OnClickPlay()
    {
        ShowBoard(DefineData.UI_INGAME_BOARD_PLAYING);
        CGameEngine.Instance.GameStart();
    }

    public void OnClickSkill(int nIndex)
    {
        CGameEngine.Instance.PlaySkill(nIndex);
        // CPlayer.Instance.SkillHeal();
    }

    #region Board Dev
    public void OnClickChangeWeapon()
    {
        m_goBtnStart.SetActive(true);

        for(int i = 0; i < m_arrWeaponOption.Length; i++)
        {
            m_arrWeaponOption[i].text = "";
            m_arrWeaponOptions[i].text = "";
        }

        List<int> listOption = CGameInfo.Instance.GetOptionList(1);

        CWeaponInfo cWeaponInfo = CGameInfo.Instance.GetWeaponInfo(2);

        int nMinDamage = cWeaponInfo.GetDamage(0);
        int nMaxDamage = cWeaponInfo.GetDamage(1);

        int nOptionMinDamage = nMinDamage;
        int nOptionMaxDamage = nMaxDamage;

        List<CFinalOption> listFinalOption = new List<CFinalOption>();

        for(int i = 0; i < listOption.Count; i++)
        {
            COptionInfo cOptionInfo = CGameInfo.Instance.GetOptionInfo(listOption[i]);
            // Debug.Log("Option Index : " + listOption[i]);
            int nRandomValue = Random.Range((int)cOptionInfo.GetValue(0), (int)cOptionInfo.GetValue(1) + 1);
            string strOptionMsg = string.Format(cOptionInfo.GetOptionMsg(), nRandomValue);
            m_arrWeaponOption[i].text = strOptionMsg;
            m_arrWeaponOptions[i].text = strOptionMsg;

            listFinalOption.Add(new CFinalOption(cOptionInfo.GetOptionIndex(), cOptionInfo.GetOptionType(), (float)nRandomValue));

            if( listOption[i] == 1 || listOption[i] == 5)
                nOptionMinDamage += nRandomValue;
            if( listOption[i] == 2 || listOption[i] == 6)
                nOptionMinDamage += (int)(nMinDamage * nRandomValue / 100);
            if( listOption[i] == 3 || listOption[i] == 5)
                nOptionMaxDamage += nRandomValue;
            if( listOption[i] == 4 || listOption[i] == 6)
                nOptionMaxDamage += (int)(nMaxDamage * nRandomValue / 100);
        }

        if( nOptionMinDamage > nOptionMaxDamage )
            nOptionMinDamage = nOptionMaxDamage;

        CWeaponEX cWeaponEX = new CWeaponEX(cWeaponInfo, listFinalOption);
        // CWeaponEX cWeaponEX = new CWeaponEX(cWeaponInfo, null);
        CGameEngine.Instance.SetWeaponEX(cWeaponEX);

        m_txtWeaponDamage.text = "피해: " + nOptionMinDamage.ToString() + " - " + nOptionMaxDamage.ToString();
        m_tmpWeaponDamage.text = "Damage: " + nOptionMinDamage.ToString() + " - " + nOptionMaxDamage.ToString();
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene("InGame");
    }

    public void InitDevBoard()
    {
        for(int i = 0; i < m_arrWeaponOption.Length; i++)
        {
            m_arrWeaponOption[i].text = "";
        }
    }
    #endregion
}
