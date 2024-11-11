/**************************************************************
　　　　　　　カード詳細設定
***************************************************************/
// 名前空間設定
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// ScriptableObjectのインスタンス生成
[CreateAssetMenu]
public class CardBase : ScriptableObject
{
    // gameObjectの紐づけ（名称、種類、強さ値、アイコンイラスト、複数行の効果説明）
    [SerializeField] new string name;
    [SerializeField] CardType type;
    [SerializeField] int number;
    [SerializeField] Sprite icon;
    [TextArea]
    [SerializeField] string description;

    // プロパティ設定（他ファイルから参照できるようにする）
    public string Name { get => name; }
    public CardType Type { get => type; }
    public int Number { get => number; }
    public Sprite Icon { get => icon; }
    public string Description { get => description; }
}

// カード種類の設定
public enum CardType
{
    Clown,
    Princess,
    Spy,
    Assassin,
    Minister,
    Magician,
    Shogun,
    Prince
}