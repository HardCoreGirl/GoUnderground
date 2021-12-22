using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

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

    public CPopupText m_cPopupText;

    public Text m_txtHP;

    public Image m_imgHP;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float fMaxHP = CGameEngine.Instance.GetPlayer().GetComponent<CCharacter>().GetMaxHP();
        float fHP = CGameEngine.Instance.GetPlayer().GetComponent<CCharacter>().GetHP();
        float fRemainHP = fHP / fMaxHP;

        if(fRemainHP >= 1)
            fRemainHP = 1f;

        m_imgHP.fillAmount = fRemainHP;

        string strHP = ((int)fHP).ToString() + " / " + ((int)fMaxHP).ToString();
        m_txtHP.text = strHP;
        /*
        float fMaxHP = CPlayer.Instance.GetMaxHP();
        float fHP = CPlayer.Instance.GetHP();
        float fRemainHP = fHP / fMaxHP;

        if(fRemainHP >= 1)
            fRemainHP = 1f;

        m_imgHP.fillAmount = fRemainHP;
        */
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

    public void OnClickPlay()
    {
        CGameEngine.Instance.GameStart();
    }

    public void OnClickSkill(int nIndex)
    {
        CGameEngine.Instance.PlaySkill(nIndex);
        // CPlayer.Instance.SkillHeal();
    }
}
