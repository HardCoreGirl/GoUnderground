using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class CCharacterSkill : MonoBehaviour
{
    private CCharacter m_cCharacter;

    private int m_nPlayingSkill = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_cCharacter = new CCharacter();
        m_cCharacter = this.gameObject.GetComponent<CCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySkill(int nIndex)
    {
        Debug.Log("PlaySkill !!!!!!!!!!!!!!!");
        CUIInGameManager.Instance.ShowCutin();
        
        m_nPlayingSkill = nIndex;

        string strSkillFuncName = "ProcessSkill" + nIndex.ToString("00000");
        if( nIndex < 10000 )   // 일반 스킬 ( 물리 )
        {
            // StartCoroutine("ProcessSkill001");
        }
        else if( nIndex < 20000 ) // 캐스팅 스킬 ( 마법 )
        {

        }
        else    // 기타 ( 버프 )
        {
            SkillBuffAni();
        }

        StartCoroutine(strSkillFuncName);
    }

    public void PlayHitSkill(int nIndex)
    {
        // StartCoroutine("ProcessHitSkill001");
        string strHitSkillFuncName = "ProcessHitSkill" + nIndex.ToString("00000");
        StartCoroutine(strHitSkillFuncName);
    }

    IEnumerator ProcessSkill00001()
    {
        GameObject goCharacter = m_cCharacter.GetCharacterGameObject();

        goCharacter.transform.localRotation = Quaternion.Euler(0, 270f, 0);
        m_cCharacter.GetCharacterAnimator().Play("Skill_01_SwordShield");

        yield return new WaitForSeconds(1.2f);

        GameObject goTarget = this.gameObject.GetComponent<CCharacter>().GetTargetGameObject();
        goTarget.GetComponent<CCharacterSkill>().PlayHitSkill(1);

        yield return new WaitForSeconds(1.233f);

        float fMovingDist = goCharacter.transform.localPosition.x;

        goCharacter.transform.localPosition = Vector3.zero;

        Vector3 vecOrignalPoz = this.gameObject.transform.localPosition;
        Debug.Log("Moving Z Poz : " + fMovingDist);
        Debug.Log("Ori Z Poz : " + vecOrignalPoz.z);

        vecOrignalPoz.z += fMovingDist;

        // Dist : 0.9876709
        
        Debug.Log("Finish Z Poz : " + vecOrignalPoz.z);
        this.gameObject.transform.localPosition = vecOrignalPoz;

        goCharacter.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    IEnumerator ProcessHitSkill00001()
    {
        Debug.Log(this.gameObject.transform.position.z);
        Debug.Log(this.gameObject.transform.localPosition.z);
        this.gameObject.transform.DOMoveZ(this.gameObject.transform.position.z - 2f, 0.25f);

        yield return null;
    }

    IEnumerator ProcessSkill20000()
    {
        CSkillInfo cSkillInfo = CGameInfo.Instance.GetSkillInfo(m_nPlayingSkill);
        float fSkillTime = cSkillInfo.GetSkillTime();
        
        float fHealValue = cSkillInfo.GetSkillValue(0);

        float fProcessTime = 0;
        float fDelta = 0;

        float fHeal = 0;
        float fTotalHeal = 0;
        while(true)
        {
            fDelta = Time.deltaTime;
            fProcessTime += fDelta;

            if( fProcessTime >= fSkillTime )
                break;

            fHeal = fHealValue * (fDelta / fSkillTime);
            fTotalHeal += fHeal;

            this.gameObject.GetComponent<CCharacter>().Heal(fHeal);
            
            yield return new WaitForEndOfFrame();
        }
    }

    public void SkillBuffAni()
    {
        m_cCharacter.GetCharacterAnimator().Play("PowerUp_SwordShield");
    }

    public void SkillHeal()
    {
        // this.gameObject.GetComponent<CPlayerSkill>().PlaySkill(1);
        // this.gameObject.GetComponent<CPlayerSkill>().PlayHitSkill(1);
        // Debug.Log("Skill Heal!!!!!!!!!!!!!");
        // m_nNextSkill = 1;
        // m_aniPlayer.Play("PowerUp_SwordShield");
    }

    IEnumerator ProcessHeal()
    {
        yield return new WaitForEndOfFrame();
        // float fTotalTime = 0;
        // while(true)
        // {
        //     fTotalTime += Time.deltaTime;
        //     if(fTotalTime >= 2.5f )
        //     {
        //         break;
        //     }

        //     m_cCharacterInfo.Heal(10);
        //     yield return new WaitForEndOfFrame();
        // }
    }
}
