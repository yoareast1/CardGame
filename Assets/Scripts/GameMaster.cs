using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    [SerializeField] Battler player;
    [SerializeField] Battler enemy;
    [SerializeField] CardGenerator cardGenerator;
    // [SerializeField] GameObject submitButton;
    [SerializeField] GameUI gameUI;
    RuleBook ruleBook;

    private void Awake()
    {
        ruleBook = GetComponent<RuleBook>();
    }

    private void Start()
    {
        Setup();
        // Debug.Log("Start");
    }

    //カードを生成して配る
    void Setup()
    {
        gameUI.Init();
        player.Life = 4;
        enemy.Life = 4;
        gameUI.ShowLifes(player.Life, enemy.Life);
        gameUI.UpdateAddNumber(player.isAddNumber, enemy.isAddNumber);
        player.OnSubmitAction = SubmittedAction;
        enemy.OnSubmitAction = SubmittedAction;
        SendCardsTo(player,isEnemy: false);
        SendCardsTo(enemy,isEnemy: true);
    }

    void SubmittedAction()
    {
        // Debug.Log("submittedactionが実行されました");
        if (player.isSubmitted && enemy.isSubmitted)
        {
            // submitButton.SetActive(false);
            // cardの処理判定
            StartCoroutine(CardsBattle());
        }
        else if (player.isSubmitted)
        {
            // submitButton.SetActive(false);
            // enemyからカードを出す
            enemy.RandomSubmit();
        }
        else if (enemy.isSubmitted)
        {
            // playerの提出を待つ
        }
    }

    void SendCardsTo(Battler battler, bool isEnemy)
    {
        for (int i = 0; i < 8; i++)
        {
            Card card = cardGenerator.Spawn(i, isEnemy);
            // battler.Hand.Add(card);
            battler.SetCardToHand(card);
            // Debug.Log($"{card.Base.Type}が選択されました");
        }
        battler.Hand.ResetPosition();
    }

    //カードの勝利判定
    //ちょっと遅らせてから結果を表示：コルーチン
    //表示が終わったら、次のターンにうつる
    IEnumerator CardsBattle()
    {
        yield return new WaitForSeconds(1f);
        enemy.SubmitCard.Open();
        yield return new WaitForSeconds(0.8f);
        Result result = ruleBook.GetResult(player, enemy);
        // Debug.Log(result);
        switch (result)
        {
            case Result.TurnWin:
            case Result.GameWin:
                gameUI.ShowTurnResult("WIN");
                enemy.Life--;
                break;
            case Result.TurnWin2:
                gameUI.ShowTurnResult("WIN");
                enemy.Life -= 2;
                break;
            case Result.TurnLose:
            case Result.GameLose:
                gameUI.ShowTurnResult("LOSE");
                player.Life--;
                break;
            case Result.TurnLose2:
                gameUI.ShowTurnResult("LOSE");
                player.Life -= 2;
                break;
            case Result.TurnDraw:
                gameUI.ShowTurnResult("Draw");
                break;
        }
        Debug.Log($"<size=20>{player.SubmitCard.Base.Number}.{player.SubmitCard.Base.Type} vs {enemy.SubmitCard.Base.Number}.{enemy.SubmitCard.Base.Type} 結果：{result}</size>");
        gameUI.ShowLifes(player.Life, enemy.Life);
        // Debug.Log($"player.Life:{player.Life},enemy.Life:{enemy.Life}");
        yield return new WaitForSeconds(1f);

        if ((player.Hand.isEmpty)||(result == Result.GameWin) || (result == Result.GameLose) || (player.Life <= 0 || enemy.Life <= 0))
        {
            ShowResult(result);
        }
        else
        {
            SetupnextTurn();
        }
        // SetupnextTurn();
    }

    void ShowResult(Result result)
    {
        if (result == Result.GameWin)
        {
            gameUI.ShowGameResult("WIN");
        }
        else if (result == Result.GameLose)
        {
            gameUI.ShowGameResult("LOSE");
        }
        else if (player.Life <= 0 && enemy.Life <= 0)
        {
            gameUI.ShowGameResult("DRAW");
        }
        else if (player.Life <= 0)
        {
            gameUI.ShowGameResult("LOSE");
        }
        else if (enemy.Life <= 0)
        {
            gameUI.ShowGameResult("WIN");
        }
        else if (player.Life > enemy.Life)
        {
            gameUI.ShowGameResult("WIN");
        }
        else if (player.Life < enemy.Life)
        {
            gameUI.ShowGameResult("LOSE");
        }
        else
        {
            gameUI.ShowGameResult("DRAW");
        }
    }

    void SetupnextTurn()
    {
        player.SetupNextTurn();
        enemy.SetupNextTurn();
        gameUI.SetupNextTurn();
        // submitButton.SetActive(true);
        gameUI.UpdateAddNumber(player.isAddNumber,enemy.isAddNumber);

        if (enemy.isFirestSubmit)
        {
            enemy.isFirestSubmit = false;
            enemy.RandomSubmit();
            enemy.SubmitCard.Open();
        }
        if (player.isFirestSubmit)
        {
            //playerが先に表で出す
        }
    }

    public void OnRetryButton()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
    public void OnTitleButton()
    {
        SceneManager.LoadScene("Title");
    }
}
