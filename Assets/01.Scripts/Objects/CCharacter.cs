using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCharacter : MonoBehaviour
{
    public int m_nAniType = 0;

    public GameObject m_goCharacter;

    public Animator m_aniCharacter;

    private GameObject m_goTarget;

    CCharacterInfo m_cCharacterInfo;

    private string m_strAttackAniName;
    private string m_strHitAniName;
    private string m_strRunAniName;
    // private string m_strSkill01AniName;

    private int m_nState = DefineData.STATE_READY;   // 0 : Ready, 1 : Active, 2 : Death
    private int m_nStandBySkill = 0;   
    private int m_nHitState = DefineData.HIT_STATE_NORMAL;    // 0 : Normal, 1 : 

    // Start is called before the first frame update
    void Start()
    {
        CWeaponInfo cWeaponInfo = new CWeaponInfo();
        cWeaponInfo.SetDamage(0, DefineData.TEST_MY_WEAPON_MIN);
        cWeaponInfo.SetDamage(1, DefineData.TEST_MY_WEAPON_MAX);

        m_cCharacterInfo = new CCharacterInfo(0, DefineData.MY_LEVEL, cWeaponInfo, m_nAniType);

        // m_strRunAniName = "Run_Standard";
        m_strRunAniName = "Run_SwordShield";
        if( m_cCharacterInfo.GetAniType() == 0 )
        {
            m_strAttackAniName = "Slash";
            m_strHitAniName = "Hit";
        } else {
            m_strAttackAniName = "Slash_SwordShield";
            m_strHitAniName = "Hit_SwordShield";
        }

        // m_strSkill01AniName = "m_strSkill01AniName";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Character Data -----
    public GameObject GetCharacterGameObject()
    {
        return m_goCharacter;
    }

    public Animator GetCharacterAnimator()
    {
        return m_aniCharacter;
    }

    public int GetState()
    {
        return m_nState;
    }

    public void SetTarget(GameObject goTarget)
    {
        m_goTarget = goTarget;
        m_nState = DefineData.STATE_ACTIVE;
    }

    public GameObject GetTargetGameObject()
    {
        return m_goTarget;
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

    public float GetDamage()
    {
        return m_cCharacterInfo.GetAttackDamage();
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
        float fResultSpeed = 0.3f;

        return fResultSpeed;
    }
    
    public float GetAttackRate()
    {
        // 최소 값 1
        // 최종 명중도
        return 0;
    }

    public bool IsAttack()
    {

        float fRate = (float)m_cCharacterInfo.GetLevel() / ((float)m_cCharacterInfo.GetLevel() + (float)m_goTarget.GetComponent<CCharacter>().GetLevel()) * ((GetAttackRate() + 100) / 100);

        Debug.Log("MyLevel : " + m_cCharacterInfo.GetLevel() + ", EnemyLevel : " + m_goTarget.GetComponent<CCharacter>().GetLevel() + ", AttackRate : " + GetAttackRate());
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
    #endregion

    public void Attack()
    {
        Debug.Log("Hit!!!!!!!!!!!!!!!!!!!");
        StartCoroutine("ProcessAttack");
    }

    IEnumerator ProcessAttack()
    {
        while(true)
        {
            if( m_nStandBySkill == 0)
            {
                // 일반 공격
                if( m_cCharacterInfo.IsAggressive() )
                {
                    float fDist = m_goCharacter.transform.position.z - m_goTarget.transform.position.z;
                    Debug.Log("Dist : " + fDist);
                    if( fDist < 0 )
                        fDist += -1;

                    if( fDist > DefineData.FIGHT_DISTANCE )
                    {
                        Run();
                        break;
                    }
                }

                m_aniCharacter.Play(m_strAttackAniName);

                yield return new WaitForSeconds(GetAttackInterval());

                if(IsAttack())
                {
                    float fDamage = GetDamage();
                    bool bIsCritical = IsCritical();
                    if( bIsCritical )
                        fDamage = GetCriticalDamage(fDamage);

                    m_goTarget.GetComponent<CCharacter>().Hit(this.gameObject, 100, bIsCritical);
                }
                else 
                {
                    m_goTarget.GetComponent<CCharacter>().Miss(this.gameObject);
                }

                yield return new WaitForSeconds(GetAttackFinishInterval());

                yield return new WaitForSeconds(GetAttakIdleInterval());
            }
            else
            {
                // TODO : 스킬 플레이
                Debug.Log("Skill : " + m_nStandBySkill);
                CSkillInfo cSkillInfo = CGameInfo.Instance.GetSkillInfo(m_nStandBySkill);

                Skill(m_nStandBySkill);

                yield return new WaitForSeconds(cSkillInfo.GetSkillTime());

                Debug.Log("Finish Skill!!");

                m_nStandBySkill = 0;
                m_nHitState = DefineData.HIT_STATE_NORMAL;
            }
        }

        // StartCoroutine("ProcessAttack");
    }

    public void Hit(GameObject goAttacker, float fDamage, bool bIsCritical = false)
    {
        float fHP = m_cCharacterInfo.Damage(fDamage);

        if( bIsCritical )
            CUIInGameManager.Instance.ShowPopupMsgByWorld(this.gameObject.transform.position, fDamage.ToString(), 101);
        else
            CUIInGameManager.Instance.ShowPopupMsgByWorld(this.gameObject.transform.position, fDamage.ToString(), 100);

        if( GetState() == DefineData.STATE_READY )
        {
            SetTarget(goAttacker);
        }

        if( m_nHitState == DefineData.HIT_STATE_NORMAL )
        {
            StopCoroutine("ProcessAttack");
            StartCoroutine("ProcessHit");
        }
    }

    IEnumerator ProcessHit()
    {
        m_aniCharacter.Play(m_strHitAniName);

        yield return new WaitForSeconds(0.7f);

        StartCoroutine("ProcessAttack");
    }

    public void Miss(GameObject goAttacker)
    {
        CUIInGameManager.Instance.ShowPopupMsgByWorld(this.gameObject.transform.position, "MISS", 100);

        if( GetState() == DefineData.STATE_READY )
        {
            SetTarget(goAttacker);
            Attack();
        }
    }

    public void Run()
    {
        Debug.Log("Run!!!!");
        StartCoroutine("ProcessRun");
    }

    IEnumerator ProcessRun()
    {
        m_goCharacter.transform.localRotation = Quaternion.Euler(0, 270f, 0);
        m_aniCharacter.Play(m_strRunAniName);

        while(true)
        {
            Vector3 vecNowPoz = this.gameObject.transform.position;
            vecNowPoz.z -= Time.deltaTime * DefineData.RUN_SPEED;
            this.gameObject.transform.position = vecNowPoz;

            float fDist = m_goCharacter.transform.position.z - m_goTarget.transform.position.z;
            // Debug.Log(m_goCharacter.transform.position.z + ", " + m_goTarget.transform.position.z);
            if( fDist < 0 )
                fDist += -1;

            if( fDist < DefineData.FIGHT_DISTANCE )
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        m_goCharacter.transform.localRotation = Quaternion.Euler(0, 0, 0);
        m_aniCharacter.Play("Idle");

        Attack();
    }

    public float Heal(float fHeal)
    {
        return m_cCharacterInfo.Heal(fHeal);
    }

    public void StandBySkill(int nIndex)
    {
        m_nStandBySkill = nIndex;
    }

    public void Skill(int nIndex)
    {
        CSkillInfo cSkillInfo = CGameInfo.Instance.GetSkillInfo(nIndex);
        m_nHitState = cSkillInfo.GetSkillType();
        this.gameObject.GetComponent<CCharacterSkill>().PlaySkill(nIndex);
    }
}

