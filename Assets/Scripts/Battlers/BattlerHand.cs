using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlerHand : MonoBehaviour
{
    List<Card> list = new List<Card>();

    public bool isEmpty { get => list.Count == 0;}

    // listに追加して自分の子要素にする
    public void Add(Card card)
    {
        list.Add(card);
        card.transform.SetParent(transform);
    }

    public void Remove(Card card)
    {
        list.Remove(card);
    }

    //手札を並べなおす
    public void ResetPosition()
    {
        //Numberの小さい順に並べる
        list.Sort((card0, card1) => card0.Base.Number - card1.Base.Number);
        for (int i = 0; i < list.Count; i++)
        {
            float posX = (i - list.Count/2f) * 1.3f;
            list[i].transform.localPosition = new Vector3(posX, 0);
        }
    }

    public Card RandomRemove()
    {
        int r = Random.Range(0, list.Count);
        Card card = list[r];
        // 保守確認用 一回目に出すenemyのカードを設定する
        // Card card = list[7];
        Remove(card);
        return card;
    }
}
