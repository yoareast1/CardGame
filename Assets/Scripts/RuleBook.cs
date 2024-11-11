using UnityEngine;

public class RuleBook : MonoBehaviour
{
    //*カード効果
    //・マジシャン（カード効果）がいれば数字対決
    //・密偵（次カードを先に出す）がいるなら追加効果
    //・道化（このターン引き分け）がいるなら引き分け
    //・暗殺者がいて、王子がいないなら数字効果反転
    //・王子と姫がいるならGameの勝利判定
    //・ここまでくれば数字対決
    //*/
    public Result GetResult(Battler player, Battler enemy)
    {
        if (player.SubmitCard.Base.Type == CardType.Magician || enemy.SubmitCard.Base.Type == CardType.Magician)
        {
            Debug.Log("魔法使いが場に出ています。");
            return NumberBattle(player, enemy, ministerEffect: false, false);
        }

        if ((player.SubmitCard.Base.Type == CardType.Spy) && (enemy.SubmitCard.Base.Type != CardType.Spy))
        {
            Debug.Log("敵先出し");
            enemy.isFirestSubmit = true;
        }
        if ((enemy.SubmitCard.Base.Type == CardType.Spy) && (player.SubmitCard.Base.Type != CardType.Spy))
        {
            Debug.Log("自分先出し");
            player.isFirestSubmit = true;
        }
        if (player.SubmitCard.Base.Type == CardType.Shogun)
        {
            Debug.Log("自分将軍フラグ");
            player.isAddNumberMode = true;
        }
        if (enemy.SubmitCard.Base.Type == CardType.Shogun)
        {
            Debug.Log("敵将軍フラグ");
            enemy.isAddNumberMode = true;
        }

        if (player.SubmitCard.Base.Type == CardType.Clown || enemy.SubmitCard.Base.Type == CardType.Clown)
        {
            Debug.Log("道化が場に出ています。");
            return Result.TurnDraw;
        }
        if ((player.SubmitCard.Base.Type == CardType.Assassin && enemy.SubmitCard.Base.Type != CardType.Prince)
            || (enemy.SubmitCard.Base.Type == CardType.Assassin && player.SubmitCard.Base.Type != CardType.Prince))
        {
            Debug.Log("暗殺者が場に出ています（王子以外で対戦）");
            return NumberBattle(player, enemy, ministerEffect: true, true);
        }
        if (player.SubmitCard.Base.Type == CardType.Princess && enemy.SubmitCard.Base.Type == CardType.Prince)
        {
            Debug.Log("王子が場に出ています。");
            return Result.GameWin;

        }
        if (enemy.SubmitCard.Base.Type == CardType.Princess && player.SubmitCard.Base.Type == CardType.Prince)
        {
            Debug.Log("姫が場に出ています。");
            return Result.GameLose;

        }
        Debug.Log("通常対戦します");
        return NumberBattle(player, enemy, ministerEffect: true, false);
    }

    Result NumberBattle(Battler player, Battler enemy, bool ministerEffect, bool reverseEffect)
    {
        if (ministerEffect == false)
        {
            if (player.SubmitCard.Base.Number + player.isAddNumber > enemy.SubmitCard.Base.Number + enemy.isAddNumber )
            {
                if (reverseEffect)
                {
                    // Debug.Log("反転有");
                    return Result.TurnLose;
                }
                return Result.TurnWin;
            }
            else if (player.SubmitCard.Base.Number + player.isAddNumber  < enemy.SubmitCard.Base.Number + enemy.isAddNumber )
            {
                if (reverseEffect)
                {
                    // Debug.Log("反転有");
                    return Result.TurnWin;
                }
                return Result.TurnLose;
            }
            return Result.TurnDraw;
        }
        else
        {
            if (player.SubmitCard.Base.Number + player.isAddNumber  > enemy.SubmitCard.Base.Number + enemy.isAddNumber )
            {
                if (reverseEffect)
                {
                    // Debug.Log("反転有");
                    if (player.SubmitCard.Base.Type == CardType.Minister)
                    {
                        return Result.TurnLose2;
                    }
                    return Result.TurnLose;
                }
                if (player.SubmitCard.Base.Type == CardType.Minister)
                {
                    return Result.TurnWin2;
                }
                return Result.TurnWin;
            }
            else if (player.SubmitCard.Base.Number + player.isAddNumber  < enemy.SubmitCard.Base.Number + enemy.isAddNumber )
            {
                if (reverseEffect)
                {
                    // Debug.Log("反転有");
                    if (enemy.SubmitCard.Base.Type == CardType.Minister)
                    {
                        return Result.TurnWin2;
                    }
                    return Result.TurnWin;
                }
                if (enemy.SubmitCard.Base.Type == CardType.Minister)
                {
                    return Result.TurnLose2;
                }
                return Result.TurnLose;
            }
            return Result.TurnDraw;
        }
    }
}

public enum Result
{
    TurnWin,
    TurnLose,
    TurnDraw,
    TurnWin2,
    TurnLose2,
    GameWin,
    GameLose,
    GameDraw
}
