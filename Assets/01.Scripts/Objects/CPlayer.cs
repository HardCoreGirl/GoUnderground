using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayer : MonoBehaviour
{
    #region SingleTon
    public static CPlayer _instance = null;

    public static CPlayer Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("CPlayer install null");

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

    public Animator m_aniPlayer;

    CCharacterInfo m_cCharacterInfo;

    int m_nTargetIndex = 0;

    float m_fAtkSpeed = 1;

    float m_fRunSpeed = 3;

    private int m_nNextSkill = -1;   // 스킬 인덱스 -1 : 어떤 스킬도 없음, 0 : 스킬 사용으로 힛 애니 없음

    // Start is called before the first frame update
    void Start()
    {
        CWeaponInfo cWeaponInfo = new CWeaponInfo();
        cWeaponInfo.SetDamage(0, DefineData.TEST_MY_WEAPON_MIN);
        cWeaponInfo.SetDamage(1, DefineData.TEST_MY_WEAPON_MAX);

        m_cCharacterInfo = new CCharacterInfo(0, DefineData.MY_LEVEL, cWeaponInfo);

        // StartCoroutine("ProcessPlay");
        // m_aniPlayer.Play("Entry");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayPlayer()
    {
        StartCoroutine("ProcessPlay");
    }

    public int GetLevel()
    {
        return m_cCharacterInfo.GetLevel();
    }

    public float GetMaxHP()
    {
        return m_cCharacterInfo.GetMaxHP();
    }

    public float GetHP()
    {
        return m_cCharacterInfo.GetHP();
    }

    public float GetAttackInterval()
    {
        float fResultSpeed = 0.7f; // m_fAtkSpeed;

        return fResultSpeed;
    }

    public float GetAttackFinishInterval()
    {
        float fResultSpeed = 0.8f;

        return fResultSpeed;
    }

    public float GetAttakIdleInterval()
    {
        float fResultSpeed = 0.5f;

        return fResultSpeed;
    }

    public float GetAttackRate()
    {
        // 최소 값 1
        // 최종 명중도
        return 0;
    }

    public bool IsAttack(int nEnemyLevel)
    {
        float fRate = (float)m_cCharacterInfo.GetLevel() / ((float)m_cCharacterInfo.GetLevel() + (float)nEnemyLevel) * ((GetAttackRate() + 100) / 100);

        Debug.Log("MyLevel : " + m_cCharacterInfo.GetLevel() + ", EnemyLevel : " + nEnemyLevel + ", AttackRate : " + GetAttackRate());
        Debug.Log("Rate : " + fRate);

        if(fRate < 0.05)
            fRate = 0.05f;  // 최소 확률
        if(fRate > 0.95)
            fRate = 0.95f; // 최대 확률

        float fRandomRate = Random.Range(0, 1f);

        Debug.Log("Rate : " + fRate + ", RandomRate : " + fRandomRate);

        if(fRandomRate > fRate)
            return false;

        return true;
    }

    public bool IsCritical()
    {
        float fRate = 0.05f;
        // TODO : 무기에 따른 크리티컬 확률
        fRate = DefineData.TEST_CRITICAL_RATE;

        float fRandomRate = Random.Range(0, 1f);

        if(fRandomRate > fRate)
            return false;

        return true;
    }

    public float GetCriticalDamage(float fDamage)
    {
        float fResultDamage = fDamage + fDamage;
        // TODO : 무기에 따른 크리티컬 데미지 추가
        return fResultDamage;
    }

    public Animator GetPlayerAnimator()
    {
        return m_aniPlayer;
    }

    public GameObject GetPlayerGameObject()
    {
        return m_goPlayer;
    }

    public void Miss()
    {
        CUIInGameManager.Instance.ShowPopupMsgByWorld(this.gameObject.transform.position, "MISS", 100);
    }
    
    public float Damage(float fDamage, bool bIsCritical = false)
    {
        // StopCoroutine("ProcessAttack");

        float fHP = m_cCharacterInfo.Damage(fDamage);
        
        if( bIsCritical )
        {
            CUIInGameManager.Instance.ShowPopupMsgByWorld(this.gameObject.transform.position, fDamage.ToString(), 101);
        }
        else 
        {
            CUIInGameManager.Instance.ShowPopupMsgByWorld(this.gameObject.transform.position, fDamage.ToString(), 100);
        }

        if( fHP <= 0 )
        {
            // TODO : Dead
            fHP = 0;
            StopCoroutine("ProcessAttack");
            StartCoroutine("ProcessDeath");
        } else {
            if( m_nNextSkill != 0 )
            {
                StopCoroutine("ProcessAttack");
                StartCoroutine("ProcessHit");
            }
        }

        Debug.Log("Player HP : " + fHP);

        return fHP;
    }

    IEnumerator ProcessHit()
    {
        m_aniPlayer.Play("Hit_SwordShield");

        yield return new WaitForSeconds(0.7f);

        StartCoroutine("ProcessAttack");
    }

    IEnumerator ProcessPlay()
    {
        // TODO : 플레이전에 모션처리
        yield return new WaitForSeconds(1f);
        StartCoroutine("ProcessAttack");
    }

    IEnumerator ProcessAttack()
    {
        int nEnemyCount = CEnemysManager.Instance.GetEnemyCount();
        // m_goPlayer.transform.localRotation = Quaternion.Euler(0, 90, 0);

        // for(int i = 0; i < nEnemyCount; i++)
        {
            // Debug.Log("Damage Enemy Index : " + i);
            while(true)
            {
                if(m_nNextSkill == 1)
                {
                    m_nNextSkill = 0;
                    m_aniPlayer.Play("PowerUp_SwordShield");
                    StartCoroutine("ProcessHeal");
                    yield return new WaitForSeconds(2.5f);
                    m_nNextSkill = -1;
                }

                m_aniPlayer.Play("Slash_SwordShield");
                yield return new WaitForSeconds(GetAttackInterval());

                float fHP = 1;

                if( CEnemysManager.Instance.GetState(0) == DefineData.STATE_READY )
                    CEnemysManager.Instance.SetState(0, DefineData.STATE_ACTIVE);

                if(IsAttack(CEnemysManager.Instance.GetLevel(0)))
                {
                    Debug.Log("Attack!!!!!!!!!!!!!!!!!!!!!");
                    float fDamage = m_cCharacterInfo.GetAttackDamage();
                    if( IsCritical() )
                    {
                        fDamage = GetCriticalDamage(fDamage);
                        Debug.Log("Critical : " + fDamage);
                        fHP = CEnemysManager.Instance.DamageEnemy(0, fDamage, true);
                    } else
                    {
                        
                        fHP = CEnemysManager.Instance.DamageEnemy(0, fDamage);
                    }
                } else {
                    Debug.Log("Miss!!!!!!!!!!!!!!!!!!!!!");
                    CEnemysManager.Instance.MissEnemy(0);
                }
                
                yield return new WaitForSeconds(GetAttackFinishInterval());

                yield return new WaitForSeconds(GetAttakIdleInterval());

                if( fHP <= 0 )
                {
                    // Run();
                    StartCoroutine("ProcessRun");
                    break;
                }
            }
            
        }

        // for(int i = 0; i < 30; i++)
        // {
        //     Debug.Log("Attack : " + m_cCharacterInfo.GetAttackDamage());
        //     yield return new WaitForSeconds(1);
        // }
    }

    public void Run()
    {
        // m_goPlayer.transform.localRotation = Quaternion.Euler(0, 0, 0);
        // m_aniPlayer.Play("Run_SwordShield");
        m_aniPlayer.Play("Run_Standard");
    }

    IEnumerator ProcessRun()
    {
        // m_goPlayer.transform.localRotation = Quaternion.Euler(0, 0, 0);
        Vector3 vecStart = m_goPlayer.transform.localPosition;
        Vector3 vecFinish = vecStart;
        vecFinish.z += 3;

        // m_aniPlayer.Play("Run_SwordShield");
        m_aniPlayer.Play("Run_Standard");
        float fDelta;
        while(true)
        {
            Vector3 vecNow = m_goPlayer.transform.localPosition;
            fDelta = Time.deltaTime;
            vecNow.z += (fDelta * m_fRunSpeed);
            m_goPlayer.transform.localPosition = vecNow;

            if( m_goPlayer.transform.localPosition.z >= vecFinish.z )
            {
                // Vector3 vecFinish = m_goPlayer.transform.localPosition;
                // vecFinish.z = vecStart.z + 3;
                // m_goPlayer.transform.localPosition = vecFinish;
                m_goPlayer.transform.localPosition = vecFinish;

                Debug.Log("Finish Run!!!");

                // m_aniPlayer.enabled = false;
                m_aniPlayer.Play("Idle");
                // m_aniPlayer.enabled = true;
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    #region Skills
    public void SkillHeal()
    {
        // this.gameObject.GetComponent<CPlayerSkill>().PlaySkill(1);
        this.gameObject.GetComponent<CPlayerSkill>().PlayHitSkill(1);
        Debug.Log("Skill Heal!!!!!!!!!!!!!");
        m_nNextSkill = 1;
        // m_aniPlayer.Play("PowerUp_SwordShield");
    }

    IEnumerator ProcessHeal()
    {
        float fTotalTime = 0;
        while(true)
        {
            fTotalTime += Time.deltaTime;
            if(fTotalTime >= 2.5f )
            {
                break;
            }

            m_cCharacterInfo.Heal(10);
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion
}
