using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWeaponEX
{
    CWeaponInfo m_cBaseWeapon;

    List<int> m_listOption;

    List<CFinalOption> m_listFinalOption;

    int m_nFinalDamageMin;
    int m_nFinalDamageMax;

    // public CWeaponEX(CWeaponInfo cBaseWeapon, List<int> listOption)
    public CWeaponEX(CWeaponInfo cBaseWeapon, List<CFinalOption> listFinalOption)
    {
        m_cBaseWeapon = cBaseWeapon;
        m_listFinalOption = listFinalOption;

        m_nFinalDamageMin = m_cBaseWeapon.GetDamage(0);
        m_nFinalDamageMax = m_cBaseWeapon.GetDamage(1);

        if( listFinalOption != null )
        {
            for(int i = 0; i < m_listFinalOption.Count; i++)
            {
                // COptionInfo cOptionInfo = CGameInfo.Instance.GetOptionInfo(listFinalOption[i].get);
                // int nRandomValue = Random.Range((int)cOptionInfo.GetValue(0), (int)cOptionInfo.GetValue(1) + 1);

                // if( listOption[i] == 1 || listOption[i] == 5)
                //     m_nFinalDamageMin += nRandomValue;
                // if( listOption[i] == 2 || listOption[i] == 6)
                //     m_nFinalDamageMin += (int)(m_cBaseWeapon.GetDamage(0) * nRandomValue / 100);
                // if( listOption[i] == 3 || listOption[i] == 5)
                //     m_nFinalDamageMax += nRandomValue;
                // if( listOption[i] == 4 || listOption[i] == 6)
                //     m_nFinalDamageMax += (int)(m_cBaseWeapon.GetDamage(1) * nRandomValue / 100);
                CFinalOption cFinalOption = listFinalOption[i];

                if( cFinalOption.GetOptionType() == 1 || cFinalOption.GetOptionType() == 5)
                    m_nFinalDamageMin += (int)cFinalOption.GetValue();
                if( cFinalOption.GetOptionType() == 2 || cFinalOption.GetOptionType() == 6)
                    m_nFinalDamageMin += (int)(m_cBaseWeapon.GetDamage(0) * cFinalOption.GetValue() / 100);
                if( cFinalOption.GetOptionType() == 3 || cFinalOption.GetOptionType() == 5)
                    m_nFinalDamageMax += (int)cFinalOption.GetValue();
                if( cFinalOption.GetOptionType() == 4 || cFinalOption.GetOptionType() == 6)
                    m_nFinalDamageMax += (int)(m_cBaseWeapon.GetDamage(1) * cFinalOption.GetValue() / 100);
            }

            if( m_nFinalDamageMin > m_nFinalDamageMax )
                m_nFinalDamageMin = m_nFinalDamageMax;
        }
    }

    public CWeaponInfo GetBaseWeapon()
    {
        return m_cBaseWeapon;
    }

    public List<int> GetListOption()
    {
        return m_listOption;
    }

    public int GetFinalDamage(int nIndex)
    {
        if( nIndex == 0 ) 
            return m_nFinalDamageMin;

        return m_nFinalDamageMax;
    }
}
