using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Battler : MonoBehaviour
{
    [SerializeField] BattlerHand hand;
    [SerializeField] SubmitPosition submitPosition;
    [SerializeField] GameObject submitButton;
    public bool isSubmitted { get; private set;}
    public bool isFirestSubmit { get; set;}
    public bool isAddNumberMode { get; set;}
    public int isAddNumber { get; private set;}
    public UnityAction OnSubmitAction;

    public BattlerHand Hand { get => hand; }
    public Card SubmitCard { get => submitPosition.SubmitCard; }
    public int Life { get; set;}

    public void SetCardToHand(Card card)
    {
        hand.Add(card);
        card.OnClickCard = SelectedCard;
    }

    void SelectedCard(Card card)
    {
        if(isSubmitted)
        {
            return;
        }
        // Debug.Log(card.Base.Number);
        // Debug.Log("カードが選択されました");
        if (submitPosition.SubmitCard)
        {
            hand.Add(submitPosition.SubmitCard);
        }
        hand.Remove(card);
        submitPosition.Set(card);
        hand.ResetPosition();
        submitButton?.SetActive(true);
    }

    public void OnSubmitButton()
    {
        if(submitPosition.SubmitCard)
        {
        //GameMasterに通知
        //カードの決定　⇒　変更はできない（決定ボタンを押せない/カードの交換はできない）
        isSubmitted = true;
        OnSubmitAction?.Invoke();
        submitButton?.SetActive(false);
        }

    }

    public void RandomSubmit()
    {
        Card card = hand.RandomRemove();
        submitPosition.Set(card);
        // Debug.Log($"ランダムで選択しました =>{card.Base.Type}");
        isSubmitted = true;
        OnSubmitAction?.Invoke();
        hand.ResetPosition();
    }

    public void SetupNextTurn()
    {
        isSubmitted = false;
        submitPosition.DeleteCard();
        if(isAddNumberMode)
        {
            isAddNumberMode = false;
            isAddNumber = 2;
        }
        else
        {
            isAddNumber = 0;
        }
    }

}
