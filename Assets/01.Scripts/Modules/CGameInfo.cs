using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameInfo : MonoBehaviour
{
    #region SingleTon
    public static CGameInfo _instance = null;

    public static CGameInfo Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("CGameInfo install null");

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

    IDictionary<int, CSkillInfo> m_dicSkill;

    // Start is called before the first frame update
    void Start()
    {
        m_dicSkill = new Dictionary<int, CSkillInfo>();

        CSkillInfo cSkillInfo = new CSkillInfo();

        // Slash
        cSkillInfo.nSkillIndex = 1;
        cSkillInfo.nSkillType = DefineData.SKILL_STATE_NO_HIT_NO_CANCEL;
        cSkillInfo.fSkillTime = 2.5f;
        cSkillInfo.fSkillValue[0] = 3f; // 데미지 업 ( 기본 데미지 * 3 )
        cSkillInfo.fSkillValue[1] = 1f; // 스턴 시간
        cSkillInfo.fSkillValue[2] = 0;

        m_dicSkill.Add(1, cSkillInfo);

        // 힐
        cSkillInfo.nSkillIndex = 20000;
        cSkillInfo.nSkillType = DefineData.SKILL_STATE_HIT_NO_CANCEL;
        cSkillInfo.fSkillTime = 2.5f;
        cSkillInfo.fSkillValue[0] = 1000f;
        cSkillInfo.fSkillValue[1] = 0;
        cSkillInfo.fSkillValue[2] = 0;

        m_dicSkill.Add(20000, cSkillInfo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CSkillInfo GetSkillInfo(int nIndex)
    {
        return m_dicSkill[nIndex];
    }
}

public class CCharacterInfo
{
    int m_nIndex;
    int m_nLevel;

    CWeaponInfo m_cWeaponInfo;

    int m_nExp;

    float m_fMaxHP;

    float m_fHP;

    private bool m_bIsAggressive;

    private int m_nAniType;

    public CCharacterInfo(int nIndex, int nLevel, CWeaponInfo cWeaponInfo, int nAnyType = 0)
    {
        m_nIndex = nIndex;
        m_nLevel = nLevel;
        m_cWeaponInfo = cWeaponInfo;

        m_fHP = (float)m_nLevel * 50;
        m_fMaxHP = m_fHP;

        if( nIndex < 1000 )
            m_bIsAggressive = true;
        else
            m_bIsAggressive = false;

        m_nAniType = nAnyType;
    }

    public int GetLevel()
    {
        return m_nLevel;
    }

    public float GetMaxHP()
    {
        return m_fMaxHP;
    }

    public float GetHP()
    {
        return m_fHP;
    }

    public bool IsAggressive()
    {
        return m_bIsAggressive;
    }

    public float GetAttackDamage()
    {
        int nRandomDamage = Random.Range(m_cWeaponInfo.GetDamage(0), m_cWeaponInfo.GetDamage(1) + 1);

        return (float)nRandomDamage;
    }

    public float Damage(float fDamage)
    {
        m_fHP -= fDamage;
        if( m_fHP < 0 )
            m_fHP = 0;

        return m_fHP;
    }

    public float Heal(float fHeal)
    {
        m_fHP += fHeal;

        if(m_fHP > m_fMaxHP)
            m_fHP = m_fMaxHP;

        return m_fHP;
    }

    public int GetAniType()
    {
        return m_nAniType;
    }
}


public class CWeaponInfo
{
    public int[] arrDamage = new int[2];

    public void SetDamage(int nType, int nDamage)
    {
        arrDamage[nType] = nDamage;
    }

    public int GetDamage(int nType)
    {
        return arrDamage[nType];
    }
}

public class CSkillInfo
{
    public int nSkillIndex = 0;
    public int nSkillType = 0;
    public float fSkillTime = 0;

    public float[] fSkillValue = new float[3];

    public int GetSkillIndex()
    {
        return nSkillIndex;
    }

    public int GetSkillType()
    {
        return nSkillType;
    }

    public float GetSkillTime()
    {
        return fSkillTime;
    }

    public float GetSkillValue(int nIndex)
    {
        return fSkillValue[nIndex];
    }
}
