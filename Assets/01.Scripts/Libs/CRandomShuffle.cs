using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CRandomShuffle
{
    // 리스트 섞기
    public static List<T> ShuffleList<T>(List<T> list)
    {
        int nRandom1, nRandom2;
        T tTemp;

        for(int i = 0; i < list.Count; i++)
        {
            nRandom1 = Random.Range(0, list.Count);
            nRandom2 = Random.Range(0, list.Count);

            tTemp = list[nRandom1];
            list[nRandom1] = list[nRandom2];
            list[nRandom2] = tTemp;
        }

        return list;
    }   

    // 배열 섞기
    public static T[] ShuffleArray<T>(T[] array)
    {
        int nRandom1, nRandom2;
        T tTemp;

        for(int i = 0; i < array.Length; i++)
        {
            nRandom1 = Random.Range(0, array.Length);
            nRandom2 = Random.Range(0, array.Length);

            tTemp = array[nRandom1];
            array[nRandom1] = array[nRandom2];
            array[nRandom2] = tTemp;
        }

        return array;
    }
}
