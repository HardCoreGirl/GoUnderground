using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

public class CPopupText : MonoBehaviour
{
    public string m_strDisplayText;

    private int m_nType = 0;
    private float m_fPower = 300;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        var txtDisplay = GetComponent<Text>();
        txtDisplay.DOFade(0f, 1.0f);
        transform.DOPunchScale(Vector3.one * 0.5f, 0.5f);

        if( m_nType < 100 )
        {
            transform.DOMove(transform.position + Vector3.up * 300, 1.0f).OnComplete(() => { Destroy(gameObject); });
        } else {
            txtDisplay.fontSize = txtDisplay.fontSize/2;
            transform.DOPunchScale(Vector3.one * 0.5f, 0.5f);
            Vector3 endPos = transform.position;
            endPos.x += (Random.insideUnitCircle * m_fPower).x;
            transform.DOJump(endPos, m_fPower, 1, 1.0f).OnComplete(() => {Destroy(gameObject);});
        }
    }

    public void InitPopupText(Vector3 vecPoz, string strMsg, int nType = 0, int nRed = -1, int nGreen = -1, int nBlue = -1)
    {
        var txtDisplay = GetComponent<Text>();
        this.gameObject.transform.position = vecPoz;
        txtDisplay.text = strMsg;
        m_nType = nType;

        int nRealRed = 255;
        int nRealGreen = 255;
        int nRealBlue = 255;

        int nDefaultFontSize = 60;

        int nColorType = nType % 100;
        if(nColorType == 0)  // 일반
        {
        } 
        else if( nColorType == 1 )   // 크리티컬
        {
            nRealRed = 255;
            nRealGreen = 0;
            nRealBlue = 0;
            nDefaultFontSize = 90;
        }
        else if( nColorType == 2 )   // 회복
        {
            nRealRed = 0;
            nRealGreen = 255;
            nRealBlue = 0;
        }


        if( nRed == -1 && nGreen == -1 && nBlue == -1) {}
        else 
        {
            nRealRed = nRed;
            nRealGreen = nGreen;
            nRealBlue = nBlue;
        }

        txtDisplay.fontSize = nDefaultFontSize;
        txtDisplay.color = new Color((float)nRealRed/255, (float)nRealGreen/255, (float)nRealBlue/255);        
    }

    public void SetColor(int nRed, int nGreen, int nBlue)
    {
        var txtDisplay = GetComponent<Text>();
        txtDisplay.color = new Color((float)nRed/255, (float)nGreen/255, (float)nBlue/255);
    }


}
