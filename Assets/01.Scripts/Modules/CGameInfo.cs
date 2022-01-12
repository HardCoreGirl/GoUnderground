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

    CUnitInfo[] m_arrUnitInfo;

    IDictionary<int, CSkillInfo> m_dicSkill;

    IDictionary<int, CWeaponInfo> m_dicWeapon;

    CTowerInfo[] m_arrTower;

    IDictionary<int, CEnemyInfo> m_dicEnemy;

    IDictionary<int, COptionInfo> m_dicOption;

    // Start is called before the first frame update
    void Start()
    {
        LoadUnitInfo();
        LoadWeaponInfo();
        LoadOptionInfo();
        LoadTowerInfo();
        LoadEnemyInfo();

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

    #region UnitInfo
    public void LoadUnitInfo()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Tables/unit_info");

        int nCount = data.Count;

        m_arrUnitInfo = new CUnitInfo[nCount];

        for(int i = 0; i < nCount; i++) {
            m_arrUnitInfo[i] = new CUnitInfo((int)data[i]["unit_index"], (int)data[i]["level"], (int)data[i]["hp"]);
        }
    }

    public int GetHPByUnitInfo(int nUnitIndex, int nLevel)
    {
        foreach(CUnitInfo cUnitInfo in m_arrUnitInfo)
        {
            if(cUnitInfo.GetUnitIndex() == nUnitIndex && cUnitInfo.GetLevel() == nLevel )
                return cUnitInfo.GetHP();
        }

        return 0;
    }
    #endregion

    #region WeaponInfo
    public void LoadWeaponInfo()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Tables/weapon_info");

        int nCount = data.Count;

        m_dicWeapon = new Dictionary<int, CWeaponInfo>();

        for(int i = 0; i < nCount; i++)
        {
            m_dicWeapon.Add((int)data[i]["weapon_index"], 
                            new CWeaponInfo((int)data[i]["weapon_index"], 
                                                (int)data[i]["weapon_type"], 
                                                (int)data[i]["min_level"], 
                                                (int)data[i]["min_damage"], 
                                                (int)data[i]["max_damage"]));
        }
    }

    public CWeaponInfo GetWeaponInfo(int nIndex)
    {
        // return m_dicWeapon[nIndex];
        CWeaponInfo cWeaponInfo;
        if(m_dicWeapon.TryGetValue(nIndex, out cWeaponInfo))
        {
            return cWeaponInfo;
        }

        return null;
    }
    #endregion

    #region OptionInfo
    public void LoadOptionInfo()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Tables/option_info");

        int nCount = data.Count;

        m_dicOption = new Dictionary<int, COptionInfo>();

        Debug.Log("Option Cnt : " + nCount);

        for(int i = 0; i < nCount; i++)
        {
            Debug.Log((int)data[i]["min_value"] + ", " + string.Format((string)data[i]["option_msg"], i));
            // m_dicOption.Add((int)data[i]["option_index"],
            //                     new COptionInfo((int)data[i]["option_index"],
            //                                     (int)data[i]["option_type"],
            //                                     (float)data[i]["min_value"],
            //                                     (float)data[i]["max_value"],
            //                                     (string)data[i]["option_msg"]));
            m_dicOption.Add((int)data[i]["option_index"],
                                new COptionInfo((int)data[i]["option_index"],
                                                (int)data[i]["option_type"],
                                                (float)((int)data[i]["min_value"]),
                                                (float)((int)data[i]["max_value"]),
                                                (string)data[i]["option_msg"]));
        }
    }

    public List<int> GetOptionList(int nOptionType)
    {
        List<int> listOptionIndex = new List<int>();
        List<int> keys = new List<int>(m_dicOption.Keys);
        foreach(int key in keys)
        {
            COptionInfo cOptionInfo = m_dicOption[key];
            if(cOptionInfo.GetOptionType() == nOptionType)
            {
                listOptionIndex.Add(key);
            }
        }
/*
        // listOptionIndex.Sort((a, b) => Mathf.RoundToInt(1 - 2 * Random.value));
        int nRandom1, nRandom2;
        int nTemp;

        for(int i = 0; i < listOptionIndex.Count; i++)
        {
            nRandom1 = Random.Range(0, listOptionIndex.Count);
            nRandom2 = Random.Range(0, listOptionIndex.Count);


            nTemp = listOptionIndex[nRandom1];
            listOptionIndex[nRandom1] = listOptionIndex[nRandom2];
            listOptionIndex[nRandom2] = nTemp;
        }
*/
        List<int> listRandomOption = CRandomShuffle.ShuffleList(listOptionIndex);

        for(int i = 0; i < listRandomOption.Count; i++)
        {
            Debug.Log("Option List: " + listRandomOption[i]);
        }


        // Debug.Log("Get Option Cnt : " + listOptionIndex.Count);

        // int nGetRandomCnt = Random.Range(0, listOptionIndex.Count);
        int nGetRandomCnt = Random.Range(0, 5);

        List<int> listOption = new List<int>();
        for(int i = 0; i < nGetRandomCnt; i++)
        {
            listOption.Add(listOptionIndex[i]);
        }
        return listOption;
    }

    public COptionInfo GetOptionInfo(int nOptionIndex)
    {
        COptionInfo cOptionInfo;
        if(m_dicOption.TryGetValue(nOptionIndex, out cOptionInfo))
        {
            return cOptionInfo;
        }

        return null;
    }
    #endregion

    #region TowerInfo
    public void LoadTowerInfo()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Tables/tower_info");
        int nCount = data.Count;

        m_arrTower = new CTowerInfo[nCount];

        for(int i = 0; i < nCount; i++)
        {
            m_arrTower[i] = new CTowerInfo((int)data[i]["tower_index"],
                                            (int)data[i]["tower_difficulty"],
                                            (int)data[i]["tower_floor"],
                                            (int)data[i]["reward_level"],
                                            (int)data[i]["enemy_min"],
                                            (int)data[i]["enemy_max"],
                                            (int)data[i]["grade_min"],
                                            (int)data[i]["grade_max"],
                                            (int)data[i]["shelter_count"],
                                            (int)data[i]["boss_index"]);
        }

    }

    public CTowerInfo GetTowerInfo(int nDifficulty, int nFloor)
    {
        foreach(CTowerInfo cTowerInfo in m_arrTower)
        {
            if(cTowerInfo.GetDifficulty() == nDifficulty && cTowerInfo.GetFloor() == nFloor)
                return cTowerInfo;
        }

        return null;
    }
    #endregion

    #region EnemyInfo
    public void LoadEnemyInfo()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Tables/enemy_info");
        int nCount = data.Count;

        m_dicEnemy = new Dictionary<int, CEnemyInfo>();

        for(int i = 0; i < nCount; i++)
        {
            m_dicEnemy.Add((int)data[i]["enemy_index"], 
                            new CEnemyInfo((int)data[i]["enemy_index"], 
                                                (int)data[i]["enemy_grade"], 
                                                (int)data[i]["unit_index"], 
                                                (int)data[i]["get_exp"], 
                                                (int)data[i]["get_gold"],
                                                (int)data[i]["enemy_level"],
                                                (int)data[i]["enemy_weapon"],
                                                (int)data[i]["enemy_shield"],
                                                (int)data[i]["enemy_helmet"],
                                                (int)data[i]["enemy_armor"]));
        }
    }

    public CEnemyInfo GetEnemyInfo(int nIndex)
    {
        CEnemyInfo cEnemyInfo;
        if(m_dicEnemy.TryGetValue(nIndex, out cEnemyInfo))
        {
            return cEnemyInfo;
        }

        return null;
    }

    public int GetEnemyGradeCount(int nGrade)
    {
        int nCount = 0;
        List<int> keys = new List<int>(m_dicEnemy.Keys);
        foreach(int key in keys)
        {
            CEnemyInfo cEnemy = m_dicEnemy[key];
            if( cEnemy.GetGrade() == nGrade )
                nCount++;
        }

        return nCount;
    }

    public CEnemyInfo GetEnemyRandomGrade(int nGrade)
    {
        int nEnemyCount = GetEnemyGradeCount(nGrade);

        int nRandomValue = Random.Range(0, nEnemyCount);

        int nCount = 0;

        List<int> keys = new List<int>(m_dicEnemy.Keys);
        foreach(int key in keys)
        {
            CEnemyInfo cEnemy = m_dicEnemy[key];
            if( cEnemy.GetGrade() == nGrade )
            {
                if( nRandomValue == nCount )
                    return cEnemy;

                nCount++;
            }
        }

        return null;
    }
    #endregion

    #region SkillInfo
    public CSkillInfo GetSkillInfo(int nIndex)
    {
        return m_dicSkill[nIndex];
    }
    #endregion
}

public class CUnitInfo
{
    int m_nUnitIndex;
    int m_nLevel;
    int m_nHP;

    public CUnitInfo(int nUnitIndex, int nLevel, int nHP)
    {
        m_nUnitIndex = nUnitIndex;
        m_nLevel = nLevel;
        m_nHP = nHP;
    }

    public int GetUnitIndex() { return m_nUnitIndex; }
    public int GetLevel() { return m_nLevel; }
    public int GetHP() { return m_nHP; }
}

public class CCharacterInfo
{
    int m_nIndex;
    int m_nLevel;

    // CWeaponInfo m_cWeaponInfo;

    CWeaponEX m_cWeaponEX;

    int m_nExp;

    float m_fMaxHP;

    float m_fHP;

    private bool m_bIsAggressive;

    private int m_nAniType;

    // public CCharacterInfo(int nUnitIndex, int nLevel, int nHP, CWeaponInfo cWeaponInfo, int nAnyType = 0)
    public CCharacterInfo(int nUnitIndex, int nLevel, int nHP, CWeaponEX cWeaponEX, int nAnyType = 0)
    {
        m_nIndex = nUnitIndex;
        m_nLevel = nLevel;
        // m_cWeaponInfo = cWeaponInfo;
        m_cWeaponEX = cWeaponEX;

        // int nHP = CGameInfo.Instance.GetHPByUnitInfo(nUnitIndex, nLevel);

        m_fHP = (float)nHP;
        m_fMaxHP = m_fHP;

        if( nUnitIndex < 1000 )
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
        // int nRandomDamage = Random.Range(m_cWeaponInfo.GetDamage(0), m_cWeaponInfo.GetDamage(1) + 1);
        int nRandomDamage = Random.Range(m_cWeaponEX.GetFinalDamage(0), m_cWeaponEX.GetFinalDamage(1) + 1);

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
    public int m_nIndex = 0;
    public int m_nType = 0;
    public int m_nMinLevel = 0;
    public int[] arrDamage = new int[2];

    public CWeaponInfo(int nIndex, int nType, int nMinLevel, int nMinDamage, int nMaxDamage)
    {
        m_nIndex = nIndex;
        m_nType = nType;
        m_nMinLevel = nMinLevel;
        arrDamage[0] = nMinDamage;
        arrDamage[1] = nMaxDamage;
    }

    public int GetIndex()
    {
        return m_nIndex;
    }

    public int GetWeaponType()
    {
        return m_nType;
    }

    public int GetMinLevel()
    {
        return m_nMinLevel;
    }

    // public void SetDamage(int nType, int nDamage)
    // {
    //     arrDamage[nType] = nDamage;
    // }

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

#region Tower Info
public class CTowerInfo 
{
    public int m_nIndex;
    public int m_nDifficulty;
    public int m_nFloor;
    public int m_nRewardLevel;
    public int m_nMinCount;
    public int m_nMaxCount;
    public int m_nMinGrade;
    public int m_nMaxGrade;
    public int m_nShelterCount;
    public int m_nBossIndex;

    public CTowerInfo(int nIndex, int nDifficulty, int nFloor, int nRewardLevel, int nMinCount, int nMaxCount, int nMinGrade, int nMaxGrade, int nShelterCount, int nBossIndex)
    {
        m_nIndex = nIndex;
        m_nDifficulty = nDifficulty;
        m_nFloor = nFloor;
        m_nRewardLevel = nRewardLevel;
        m_nMinCount = nMinCount;
        m_nMaxCount = nMaxCount;
        m_nMinGrade = nMinGrade;
        m_nMaxGrade = nMaxGrade;
        m_nShelterCount = nShelterCount;
        m_nBossIndex = nBossIndex;
    }

    public int GetIndex()
    {
        return m_nIndex;
    }

    public int GetDifficulty()
    {
        return m_nDifficulty;
    }

    public int GetFloor()
    {
        return m_nFloor;
    }

    public int GetRewardLevel()
    {
        return m_nRewardLevel;
    }

    public int GetMinCount()
    {
        return m_nMinCount;
    }

    public int GetMaxCount()
    {
        return m_nMaxCount;
    }

    public int GetMinGrade()
    {
        return m_nMinGrade;
    }

    public int GetMaxGrade()
    {
        return m_nMaxGrade;
    }

    public int GetShelterCount()
    {
        return m_nShelterCount;
    }

    public int GetBossCount()
    {
        return m_nBossIndex;
    }
}
#endregion

#region Enemy Info
public class CEnemyInfo
{
    public int m_nIndex;
    public int m_nGrade;
    public int m_nUnitIndex;
    public int m_nGetExp;
    public int m_nGetGold;
    public int m_nLevel;
    public int m_nWeapon;

    public CEnemyInfo(int nIndex, int nGrade, int nUnitIndex, int nGetExp, int nGetGold, int nLevel, int nWeapon, int nShield = 0, int nHelmet = 0, int nArmor = 0)
    {
        m_nIndex = nIndex;
        m_nGrade = nGrade;
        m_nUnitIndex = nUnitIndex;
        m_nGetExp = nGetExp;
        m_nGetGold = nGetGold;
        m_nLevel = nLevel;
        m_nWeapon = nWeapon;
    }

    public int GetIndex()
    {
        return m_nIndex;
    }

    public int GetGrade()
    {
        return m_nGrade;
    }

    public int GetUnitIndex()
    {
        return m_nUnitIndex;
    }

    public int GetExp()
    {
        return m_nGetExp;
    }

    public int GetGold()
    {
        return m_nGetGold;
    }

    public int GetLevel()
    {
        return m_nLevel;
    }

    public int GetWeapon()
    {
        return m_nWeapon;
    }
}
#endregion


#region OptionInfo
public class COptionInfo
{
    public int m_nIndex;

    public int m_nOptionType;

    public float[] m_arrValue;

    public string m_strOptionMsg;

    public COptionInfo(int nIndex, int nOptionType, float fValue01, float fValue02, string strOptionMsg)
    {
        m_arrValue = new float[2];

        m_nIndex = nIndex;
        m_nOptionType = nOptionType;

        m_arrValue[0] = fValue01;
        m_arrValue[1] = fValue02;

        m_strOptionMsg = strOptionMsg;
        
    }

    public int GetOptionIndex()
    {
        return m_nIndex;
    }

    public int GetOptionType()
    {
        return m_nOptionType;
    }

    public float GetValue(int nIndex)
    {
        return m_arrValue[nIndex];
    }

    public string GetOptionMsg()
    {
        return m_strOptionMsg;
    }
}

public class CFinalOption
{    
    public int m_nIndex;

    public int m_nOptionType;

    public float m_fValue;

    public CFinalOption(int nIndex, int nOptionType, float fValue)
    {
        m_nIndex = nIndex;
        m_nOptionType = nOptionType;

        m_fValue = fValue;
    }

    public int GetOptionType()
    {
        return m_nOptionType;
    }

    public float GetValue()
    {
        return m_fValue;
    }
}
#endregion