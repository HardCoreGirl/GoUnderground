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

    // Start is called before the first frame update
    void Start()
    {
        m_nEnemyCount = Random.Range(5, m_arrEnemy.Length);

        for(int i = 0; i < m_nEnemyCount; i++)
        {
            m_arrEnemy[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Enemys/enemys"));
            m_arrEnemy[i].transform.parent = m_goEnemyRoot.transform;
            m_arrEnemy[i].transform.localPosition = new Vector3(0, 0, (-5 * i) - 3);
            
//            m_arrEnemy[i].
        }

        // m_arrEnemy[0].GetComponent<CEnemy>().PlayEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
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
