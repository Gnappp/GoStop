using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnEndState //쪽,뻑 등등
{
    BALK, GET_BALK, KISS, CLEAR, DDADAK, BOOM, GOKJIN, JOKER, D_SELECT_CARD, P_SELECT_CARD, SELECT_GOSTOP, GO,STOP
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
    private List<Card> deck;
    private Player player1;
    private Player player2;
    private List<SpecialCombo> combos;
    private GoStopRules rules;
    private bool turn; //true = player1, false = player2
    public GameObject playerClient1;
    private PlayerClient test;
   // public GameObject playerClient2;

    private void Start()
    {
        player1 = new Player("jin");
        player2 = new Player("Gna");
        rules = new GoStopRules();

    }



    public void RequestSelectCard(Card playHandCard, string playername) //한장
    {
        Card deckDrowCard = DeckDrow();
        List<TurnEndState> turnEndStates = new List<TurnEndState>();
        if (playername == player1.Get_name())
        {
            turnEndStates = rules.TurnEndBottomCardsSet(playHandCard, deckDrowCard, ref player1, ref player2, ref bottomCard); //쪽 등등
            ///특이사항 없을시 계산
        }
        else if (playername == player2.Get_name())
        {
            player2.Delete_HandCard(playHandCard);
            turnEndStates = rules.TurnEndBottomCardsSet(playHandCard, deckDrowCard, ref player2, ref player1, ref bottomCard);
            //특이사항 없을시 계산

        }

    }

    public void RequestSelectCard(List<Card> playHandCards, string playername) //폭탄 전용
    {
        Card deckDrowCard = DeckDrow();
        List<TurnEndState> turnEndStates = new List<TurnEndState>();
        if (playername == player1.Get_name())
        {
            for (int numofplayHandCard = 0; numofplayHandCard < playHandCards.Count; numofplayHandCard++)
                player1.Delete_HandCard(playHandCards[numofplayHandCard]); //플레이어가 낸 패를 제거

            turnEndStates = rules.TurnEndBottomCardsSet(playHandCards, deckDrowCard, ref player1, ref player2, ref bottomCard); //쪽 등등
            //특이사항 없을시 계산
            if(turnEndStates.Count!=0)
            {
                playerClient1.GetComponent<PlayerClient>().RequireSelcectCard(turnEndStates);
            }
        }
        else if (playername == player2.Get_name())
        {
            for (int numofplayHandCard = 0; numofplayHandCard < playHandCards.Count; numofplayHandCard++)
                player2.Delete_HandCard(playHandCards[numofplayHandCard]);

            turnEndStates = rules.TurnEndBottomCardsSet(playHandCards, deckDrowCard, ref player2, ref player1, ref bottomCard);
            //특이사항 없을시 계산
        }
    }

    public void TurnEnd(string playername)
    {
        if (playername == player1.Get_name())
        {
            TurnEndCalcurate(ref player1, ref player2);
        }
        else if (playername == player2.Get_name())
        {
            TurnEndCalcurate(ref player2, ref player1);
        }
    }

            private void TurnEndCalcurate(ref Player turnEndPlayer, ref Player turnwaitPlayer)
    {
        int score;
        List<SpecialCombo> combos_ptr = new List<SpecialCombo>();
        combos_ptr = rules.CalcurateScore(player1.Get_acquiredCards(), out score); //플레이어가 먹은패를 토대로 점수계산
        turnEndPlayer.Set_score(score);
        turnwaitPlayer.Set_Bakstates(rules.AddTurnWaitPlayerBak(player1.Get_acquiredCards(), player2.Get_acquiredCards())); //상대방 박을 설정하기 위해 상대플레이어를 넣음
        if (turnEndPlayer.Get_Bakstates().Count > 0) // 내 박이 1개 이상일때 상대플레이어패를 넣고 박을 초기화 시켜넣는다.
            turnwaitPlayer.Set_Bakstates(rules.RemoveTrunEndPlayerBak(player1.Get_acquiredCards(), player1.Get_Bakstates()));

        //for (int i = 0; i < combos_ptr.Count; i++)
        //{
        //    for (int a = combos.Count - 1; a >= 0; i--)
        //    {
        //        if (combos_ptr[i] == combos[a])
        //        {
        //            printCombos.Add(combos[a]);
        //            combos.RemoveAt(a);
        //        }
        //    }
        //}
    }

    private Card DeckDrow() 
    {
        Card drowCard = deck[deck.Count - 1];
        deck.RemoveAt(deck.Count - 1);
        return drowCard;
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
}
