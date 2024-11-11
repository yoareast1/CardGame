using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    //カードUI
    //ゲーム内の処理
    [SerializeField] Text nameText;
    [SerializeField] Text numberText;
    [SerializeField] Image icon;
    [SerializeField] Text descriptionText;
    [SerializeField] GameObject hidePanel;
    public CardBase Base {get; private set;}

    //関数を登録できる
    public UnityAction<Card> OnClickCard;

    public void Set(CardBase cardBase, bool isEnemy)
    {
        Base = cardBase;
        nameText.text = cardBase.Name;
        numberText.text = cardBase.Number.ToString();
        icon.sprite = cardBase.Icon;
        descriptionText.text = cardBase.Description;
        hidePanel.SetActive(isEnemy);
    }

    public void OnClick()
    {
        // Debug.Log("Battlerへ通知");
        OnClickCard?.Invoke(this);
    }


    public void OnPointerEnter()
    {
        // Debug.Log("Enter");
        transform.position += Vector3.up * 0.3f;
        transform.localScale = Vector3.one * 1.1f;
        GetComponentInChildren<Canvas>().sortingLayerName = "Overlay";
    }
    public void OnPointerExit()
    {
        // Debug.Log("Exit");
        transform.position -= Vector3.up * 0.3f;
        transform.localScale = Vector3.one;
        GetComponentInChildren<Canvas>().sortingLayerName = "Default";
    }
    public void Open()
    {
        hidePanel.SetActive(false);
    }
}
