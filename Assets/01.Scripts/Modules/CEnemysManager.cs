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

    int m_nEnemyCount = 0;

    private int m_nTargetEnemyIndex = 0;

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
        m_nEnemyCount = Random.Range(cTowerInfo.GetMinCount(), cTowerInfo.GetMaxCount());

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
    }

    public GameObject GetEnemyGameObject(int nIndex)
    {
        return m_arrEnemy[nIndex];
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

        if( m_nTargetEnemyIndex >= GetEnemyCount() )
            return -1;
            
        Vector3 vecEnmeyPoz = vecPlayerPoz;
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
