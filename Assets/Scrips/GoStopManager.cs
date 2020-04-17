using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnEndState //쪽,뻑 등등
{
    BALK,GET_BALK,KISS,CLEAR,DDADAK,GOKJIN
}
public enum Bak //피박 광박 등등
{
    PI, GWANG, MUNG, TTI,
}
public enum SpecialCombo //3광,5광 족보
{
    GWONG3, GWONG3B, GWONG4, GWONG5, HONGDAN, CHUNGDAN, CHODAN, GODORI, NULL
}
public enum Jokbo //먹은패 구별하기위해 족보별
{
    GWONG, GODORI, HONGDAN, CHODAN, CHUNGDAN, YEOLGGUT, PI
}

public class GoStopManager : MonoBehaviour
{
    private List<List<Card>> bottomCard; //바닥 패
    private Player player1;
    private Player player2;
    private List<SpecialCombo> combos;
    private GoStopRules rules;

    private void Start()
    {
        player1 = new Player("jin");
        player2 = new Player("Gna");
        Debug.Log(Jokbo.GetNames(typeof(Jokbo)).Length);
        rules = new GoStopRules();
    }

    void CreateDeck() //Card를 사용하여 화투 덱을만든다.
    {

    }

    void DeckToPlayers() //Deck에서 플레이어들에게 패를 돌린다.
    {

    }

    void DeckToBottom() //Deck에서 바닥에 패를 나둔다.
    {

    }

    public void TurnEnd(Card playHandCard, Card deckDrowCard,string playername) //플레이어 이름가지고 낸사람 찾기
    {
        int score;
        List<SpecialCombo> combos_ptr = new List<SpecialCombo>();
        List<TurnEndState> turnEndStates = new List<TurnEndState>();
        List<SpecialCombo> printCombos = new List<SpecialCombo>();
        if (playername == player1.Get_name())
        {
            player1.Delete_HandCard(playHandCard); //플레이어가 낸 패를 제거
            turnEndStates= rules.TurnEndBottomCardsSet(playHandCard, deckDrowCard, ref player1, ref player2, ref bottomCard); //쪽 등등
            combos_ptr = rules.CalcurateScore(player1.Get_acquiredCards(), out score); //플레이어가 먹은패를 토대로 점수계산
            player1.Set_score(score);
            player2.Set_Bakstates(rules.AddTurnWaitPlayerBak(player1.Get_acquiredCards(),player2.Get_acquiredCards())); //상대방 박을 설정하기 위해 상대플레이어를 넣음
            if(player1.Get_Bakstates().Count >0) // 내 박이 1개 이상일때 상대플레이어패를 넣고 박을 초기화 시켜넣는다.
                player1.Set_Bakstates(rules.RemoveTrunEndPlayerBak(player1.Get_acquiredCards(), player1.Get_Bakstates()));
        }
        else if (playername == player2.Get_name())
        {
            player2.Delete_HandCard(playHandCard);
            turnEndStates = rules.TurnEndBottomCardsSet(playHandCard, deckDrowCard, ref player2, ref player1, ref bottomCard);
            combos_ptr = rules.CalcurateScore(player1.Get_acquiredCards(), out score);
            player2.Set_score(score);
            player1.Set_Bakstates(rules.AddTurnWaitPlayerBak(player2.Get_acquiredCards(), player1.Get_acquiredCards()));
            if (player2.Get_Bakstates().Count > 0)
                player2.Set_Bakstates(rules.RemoveTrunEndPlayerBak(player2.Get_acquiredCards(), player2.Get_Bakstates()));
        }
        for(int i = 0; i<combos_ptr.Count;i++)
        {
            for(int a=combos.Count-1;a>=0;i--)
            {
                if(combos_ptr[i]==combos[a])
                {
                    printCombos.Add(combos[a]);
                    combos.RemoveAt(a);
                }
            }
        }


    }
    


}
