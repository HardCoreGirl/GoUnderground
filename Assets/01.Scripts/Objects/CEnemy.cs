using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemy : MonoBehaviour
{
    CCharacterInfo m_cCharacterInfo;

    public Animator m_aniEnemy;
    public CPopupText m_cPopupText;

    private int m_nState = 0;   // 0 : Ready, 1 : Active, 2 : Death

    // Start is called before the first frame update
    void Start()
    {
        CWeaponInfo cWeaponInfo = new CWeaponInfo();
        cWeaponInfo.SetDamage(0, DefineData.TEST_ENEMY_WEAPON_MIN);
        cWeaponInfo.SetDamage(1, DefineData.TEST_ENEMY_WEAPON_MAX);

        m_cCharacterInfo = new CCharacterInfo(1000, DefineData.ENEMY_LEVEL, cWeaponInfo);

        Debug.Log("Load Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEnemy()
    {
        StartCoroutine("ProcessPlay");
    }

    public int GetLevel()
    {
        return m_cCharacterInfo.GetLevel();
    }

    public void SetState(int nState)
    {
        if( m_nState == 0 )
        {
            StartCoroutine("ProcessAttack");
        }

        m_nState = nState;
    }

    public int GetState()
    {
        return m_nState;
    }

    public float Damage(float fDamage, bool bIsCritical = false, int nSkillIndex = 0)
    {
        // StopCoroutine("ProcessAttack");
        float fHP = m_cCharacterInfo.Damage(fDamage);

        if( bIsCritical )
        {
            CUIInGameManager.Instance.ShowPopupMsgByWorld(this.gameObject.transform.position, fDamage.ToString(), 101);
        } else 
        {
            CUIInGameManager.Instance.ShowPopupMsgByWorld(this.gameObject.transform.position, fDamage.ToString(), 100);
        }
        
        StopCoroutine("ProcessAttack");

        if( fHP <= 0 )
        {
            // TODO : Dead
            fHP = 0;
            StartCoroutine("ProcessDeath");
        } else {
            StartCoroutine("ProcessHit");
        }

        Debug.Log("Enemy HP : " + fHP);

        return fHP;
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


    public void Miss()
    {
        CUIInGameManager.Instance.ShowPopupMsgByWorld(this.gameObject.transform.position, "MISS", 100);
    }

    IEnumerator ProcessHit()
    {
        m_aniEnemy.Play("Hit");

        yield return new WaitForSeconds(0.7f);

        StartCoroutine("ProcessAttack");
    }

    public int Death()
    {
        m_aniEnemy.Play("Death");
        return 0;
    }

    IEnumerator ProcessDeath()
    {
        m_aniEnemy.Play("Death");

        yield return new WaitForSeconds(3f);

        this.gameObject.SetActive(false);
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

    IEnumerator ProcessPlay()
    {
        yield return new WaitForSeconds(3.5f);

        StartCoroutine("ProcessAttack");
    }

    IEnumerator ProcessAttack()
    {
        while(true)
        {
            m_aniEnemy.Play("Slash");
            yield return new WaitForSeconds(GetAttackInterval());

            float fHP = 1;

            if(IsAttack(CPlayer.Instance.GetLevel()))
            {
                Debug.Log("Enmey Attack!!!!!!!!!!!!!!!!!!!!!");
                float fDamage = m_cCharacterInfo.GetAttackDamage();
                if( IsCritical() )
                {
                    fDamage = GetCriticalDamage(fDamage);
                    Debug.Log("Critical : " + fDamage);
                    fHP = CPlayer.Instance.Damage(fDamage, true);
                }
                else
                {
                    fHP = CPlayer.Instance.Damage(fDamage);
                }
            } else {
                CPlayer.Instance.Miss();
                Debug.Log("Enmey Miss!!!!!!!!!!!!!!!!!!!!!");
            }

            yield return new WaitForSeconds(GetAttackFinishInterval());

            // m_aniEnemy.Rebind();
            // m_aniEnemy.Play("Idle");
            yield return new WaitForSeconds(GetAttakIdleInterval());
        }
        
    }
}
