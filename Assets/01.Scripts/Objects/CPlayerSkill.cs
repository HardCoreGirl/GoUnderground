using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class CPlayerSkill : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySkill(int nIndex)
    {
        Debug.Log("11111");

        StartCoroutine("ProcessSkill001");
    }

    public void PlayHitSkill(int nIndex)
    {
        StartCoroutine("ProcessHitSkill001");
    }

    IEnumerator ProcessSkill001()
    {
        this.gameObject.GetComponent<CPlayer>().GetPlayerAnimator().Play("Skill_01_SwordShield");
        yield return new WaitForSeconds(2.433f);
        GameObject goPlayer = this.gameObject.GetComponent<CPlayer>().GetPlayerGameObject();

        float fMovingDist = goPlayer.transform.localPosition.z;

        goPlayer.transform.localPosition = Vector3.zero;

        Vector3 vecOrignalPoz = this.gameObject.transform.localPosition;
        vecOrignalPoz.z -= fMovingDist;
        this.gameObject.transform.localPosition = vecOrignalPoz;
    }

    IEnumerator ProcessHitSkill001()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.transform.DOMoveZ(-70, 2f);

    }
}
