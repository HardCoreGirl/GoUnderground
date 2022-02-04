using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemysManager : MonoBehaviour
{
    #region SingleTon
    
    public static CEnemysManager _instance = null;

    public static CEnemysManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("CEnemysManager install null");

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

    public GameObject m_goEnemyRoot;

    GameObject[] m_arrEnemy = new GameObject[30];

    GameObject m_goBoss;

    int m_nEnemyCount = 0;

    private int m_nTargetEnemyIndex = 0;

    private bool m_bIsShowBoss = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitEnemy(CTowerInfo cTowerInfo)
    {
        // m_nEnemyCount = Random.Range(cTowerInfo.GetMinCount(), cTowerInfo.GetMaxCount());
        m_nEnemyCount = 1;

        Debug.Log("Enemy Count : " + m_nEnemyCount);

        for(int i = 0; i < m_nEnemyCount; i++)
        {
            int nEnemyGrade = Random.Range(cTowerInfo.GetMinGrade(), cTowerInfo.GetMaxGrade());
            CEnemyInfo cEnemyInfo = CGameInfo.Instance.GetEnemyRandomGrade(nEnemyGrade);
            CWeaponInfo cWeaponInfo = CGameInfo.Instance.GetWeaponInfo(cEnemyInfo.GetWeapon());
            int nHP = CGameInfo.Instance.GetHPByUnitInfo(cEnemyInfo.GetUnitIndex(), cEnemyInfo.GetLevel());

            m_arrEnemy[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Enemys/enemys"));
            m_arrEnemy[i].transform.parent = m_goEnemyRoot.transform;
            // m_arrEnemy[i].transform.localPosition = new Vector3(0, 0, (-5 * i) - 3);

            m_arrEnemy[i].transform.localPosition = new Vector3(0, 0, 99);
            
            if( i == 0 )
                m_arrEnemy[i].transform.localPosition = new Vector3(0, 0, -3);

            
            m_arrEnemy[i].GetComponent<CCharacter>().InitCharacter(DefineData.CHARACTER_TYPE_ENEMY, cEnemyInfo.GetUnitIndex(), cEnemyInfo.GetLevel(), nHP, new CWeaponEX(cWeaponInfo, null));
            
//            m_arrEnemy[i].
        }

        // Boss Loading ---
        CBossInfo cBossInfo = CGameInfo.Instance.GetBossInfo(1);
        Debug.Log("BossIndex : " + cBossInfo.GetIndex());
        Debug.Log("BossUnitIndex : " + cBossInfo.GetUnitIndex());
        Debug.Log("BossWeapon : " + cBossInfo.GetWeapon());
        
        CWeaponInfo cBossWeaponInfo = CGameInfo.Instance.GetWeaponInfo(cBossInfo.GetWeapon());
        int nBossHP = CGameInfo.Instance.GetHPByUnitInfo(cBossInfo.GetUnitIndex(), cBossInfo.GetLevel());
        m_goBoss = Instantiate(Resources.Load<GameObject>("Prefabs/Enemys/Enemy_9000"));
        m_goBoss.transform.parent = m_goEnemyRoot.transform;

        m_goBoss.transform.localPosition = new Vector3(0, 0, 99);

        m_goBoss.GetComponent<CCharacter>().InitCharacter(DefineData.CHARACTER_TYPE_ENEMY, cBossInfo.GetUnitIndex(), cBossInfo.GetLevel(), nBossHP, new CWeaponEX(cBossWeaponInfo, null));
    }

    public GameObject GetEnemyGameObject(int nIndex)
    {
        return m_arrEnemy[nIndex];
    }

    public GameObject GetBossGameObject()
    {
        return m_goBoss;
    }

    public int GetEnemyCount()
    {
        return m_nEnemyCount;
    }

    public int GetLevel(int nIndex)
    {
        return m_arrEnemy[nIndex].GetComponent<CEnemy>().GetLevel();
    }

    public void SetState(int nIndex, int nState)
    {
        m_arrEnemy[nIndex].GetComponent<CEnemy>().SetState(nState);
    }

    public int GetState(int nIndex)
    {
        return m_arrEnemy[nIndex].GetComponent<CEnemy>().GetState();
    }

    public int NextTarget(Vector3 vecPlayerPoz)
    {
        m_nTargetEnemyIndex++;

        Vector3 vecEnmeyPoz = vecPlayerPoz;

        if( m_nTargetEnemyIndex >= GetEnemyCount() )
        {
            // TODO : HardCoreGandhi 220119 - 보스 유무 체크 - 현재 하드 코딩이라서 무조건 생성
            // if( boss )
            if( !m_bIsShowBoss )    // 보수 출연 여부
            {
                vecEnmeyPoz.z -= 10;
                m_goBoss.transform.position = vecEnmeyPoz;
                m_bIsShowBoss = true;

                return 999; // 보스 출연히 타겟이 999
            }

            return -1;
            // return -1;
        }

        vecEnmeyPoz.z -= 5;
        m_arrEnemy[m_nTargetEnemyIndex].transform.position = vecEnmeyPoz;
        return m_nTargetEnemyIndex;
    }

    public float DamageEnemy(int nIndex, float fDamage, bool bIsCritical = false)
    {
        float fHP = m_arrEnemy[nIndex].GetComponent<CEnemy>().Damage(fDamage, bIsCritical);

        if( fHP <= 0 )
        {
            // TODO : Death
        }

        return fHP;
//        m_arrEnemy[nIndex].
    }

    public void MissEnemy(int nIndex)
    {
        m_arrEnemy[nIndex].GetComponent<CEnemy>().Miss();
    }
}
