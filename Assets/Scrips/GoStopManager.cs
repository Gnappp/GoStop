using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurnEndState //쪽,뻑 등등
{
    BALK, GET_BALK, KISS, CLEAR, DDADAK, BOOM, GOKJIN, JOKER, D_SELECT_CARD, P_SELECT_CARD, SELECT_GOSTOP, GO,STOP
}
public enum SpecialCombo //3광,5광 족보
{
    GWONG3, GWONG3B, GWONG4, GWONG5, HONGDAN, CHUNGDAN, CHODAN, GODORI, NULL
}
public enum Bak //피박 광박 등등
{
    PI, GWANG, MUNG, TTI,
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
    private PlayerClient _playerClient1;
    public GameObject playerClient2;
    private PlayerClient _playerClient2;

    private List<Card> playHandCard_ptr =null;
    private List<Card> deckDrowCard_ptr = null;
    private int stillPiStack = 0;

    private void Start()
    {
        player1 = new Player("jin");
        player2 = new Player("Gna");
        rules = new GoStopRules();
        _playerClient1 = playerClient1.GetComponent<PlayerClient>();
    }

    public void RequestSelectCard(Card playHandCard, string playername) //한장
    {
        List<TurnEndState> turnEndStates = new List<TurnEndState>();
        if (playHandCard.Get_month() != 13) //낸카드 != 조커 경우 일반적인 플레이
        {
            playHandCard_ptr.Add(playHandCard);
            if(playHandCard_ptr[0].Get_month() !=0) //낸카드 != 더미카드 경우 바닥카드에 추가하지 않는다.
                rules.playHandCardAddTobottomCard(playHandCard_ptr, ref bottomCard);

            Card deckDrowCard = DeckDrow();
            while (!rules.deckDrowCardAddTobottomCard(playHandCard_ptr, deckDrowCard, ref bottomCard)) //제대로 된경우 return true 다시 드로우 return false
            {
                deckDrowCard_ptr.Add(deckDrowCard);
                deckDrowCard = DeckDrow();                
            }
            deckDrowCard_ptr.Add(deckDrowCard);

            // 핸드카드와 드로우카드를 이용하여 먹을카드 체크후, 선택해야 할것이 있으면
            // require를 보내 체크하고, 받으면 
        }

        else if(playHandCard.Get_month() == 13) //낸카드 == 조커 드로우 진행하지 않음
        {
            if(playername == player1.name)
            {
                playHandCardIsJOKER(ref player1, ref player2, playHandCard);
                //SendPlayInfo(string na, List < Bak > baks, int sc, int goN, bool[] sh, int mul, int pis, List < List < Card >> ac, int hcc)
                SendPlayInfo sendInfo = new SendPlayInfo(player2.name, player2.Bakstates, player2.score, player2.goNum, player2.shake, player2.multipleScore,
                    player2.piscore, player2.acquiredCards, player2.Get_HandCardCount());
                _playerClient1.RequireFromManager(player1, sendInfo, null, null, true,null,null, 1);
            }
            else if (playername == player2.name)
            {
                playHandCardIsJOKER(ref player2, ref player1, playHandCard);
                SendPlayInfo sendInfo = new SendPlayInfo(player1.name, player1.Bakstates, player1.score, player1.goNum, player1.shake, player1.multipleScore,
                    player1.piscore, player1.acquiredCards, player1.Get_HandCardCount());
                _playerClient2.RequireFromManager(player2, sendInfo, null, null, false,null,null, 1);
            }
        }
    }

    public void playHandCardIsJOKER(ref Player playCardPlayer, ref Player turnWaitPlayer,Card joker)
    {
        playCardPlayer.AddAcquiredCard(joker);
        playCardPlayer.AddAcquiredCard(turnWaitPlayer.LosePI());

        int score;
        int piscore;
        rules.CalcurateScore(playCardPlayer.acquiredCards, out score,out piscore);
        playCardPlayer.score = score;
        playCardPlayer.piscore = piscore;
        rules.CalcurateScore(turnWaitPlayer.acquiredCards, out score, out piscore);
        turnWaitPlayer.score = score;
        turnWaitPlayer.piscore = piscore;

        turnWaitPlayer.Bakstates = rules.AddTurnWaitPlayerBak(playCardPlayer.acquiredCards, turnWaitPlayer.acquiredCards);
        if (playCardPlayer.Bakstates.Count > 0)
            playCardPlayer.Bakstates = rules.RemoveTrunEndPlayerBak(playCardPlayer.acquiredCards, playCardPlayer.Bakstates);

        playCardPlayer.Delete_HandCard(joker);
        playCardPlayer.CreateDummyCard();
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
//public void RequestSelectCard(List<Card> playHandCards, string playername) //폭탄 전용
//{
//    Card deckDrowCard = DeckDrow();
//    playHandCard_ptr = playHandCards;

//    List<TurnEndState> turnEndStates = new List<TurnEndState>();
//    if (playername == player1.Get_name())
//    {
//        for (int numofplayHandCard = 0; numofplayHandCard < playHandCards.Count; numofplayHandCard++)
//            player1.Delete_HandCard(playHandCards[numofplayHandCard]); //플레이어가 낸 패를 제거

//        turnEndStates = rules.TurnEndBottomCardsSet(playHandCards, deckDrowCard, ref player1, ref player2, ref bottomCard); //쪽 등등
//        //특이사항 없을시 계산
//        if (turnEndStates.Count != 0)
//        {
//            playerClient1.GetComponent<PlayerClient>().RequireSelcectCard(turnEndStates);
//        }
//    }
//    else if (playername == player2.Get_name())
//    {
//        for (int numofplayHandCard = 0; numofplayHandCard < playHandCards.Count; numofplayHandCard++)
//            player2.Delete_HandCard(playHandCards[numofplayHandCard]);

//        turnEndStates = rules.TurnEndBottomCardsSet(playHandCards, deckDrowCard, ref player2, ref player1, ref bottomCard);
//        //특이사항 없을시 계산
//    }
//}
//public void TurnEnd(string playername)
//{
//    if (playername == player1.name)
//    {
//        TurnEndCalcurate(ref player1, ref player2);
//    }
//    else if (playername == player2.name)
//    {
//        TurnEndCalcurate(ref player2, ref player1);
//    }
//}

//private void TurnEndCalcurate(ref Player turnEndPlayer, ref Player turnwaitPlayer)
//{
//    int score;
//    int piscore;
//    List<SpecialCombo> combos_ptr = new List<SpecialCombo>();
//    combos_ptr = rules.CalcurateScore(player1.acquiredCards, out score, out piscore); //플레이어가 먹은패를 토대로 점수계산
//    turnEndPlayer.score = score;
//    turnwaitPlayer.Bakstates = rules.AddTurnWaitPlayerBak(player1.acquiredCards, player2.acquiredCards); //상대방 박을 설정하기 위해 상대플레이어를 넣음
//    if (turnEndPlayer.Bakstates.Count > 0) // 내 박이 1개 이상일때 상대플레이어패를 넣고 박을 초기화 시켜넣는다.
//        turnwaitPlayer.Bakstates = rules.RemoveTrunEndPlayerBak(player1.acquiredCards, player1.Bakstates);

//    for (int i = 0; i < combos_ptr.Count; i++)
//    {
//        for (int a = combos.Count - 1; a >= 0; i--)
//        {
//            if (combos_ptr[i] == combos[a])
//            {
//                printCombos.Add(combos[a]);
//                combos.RemoveAt(a);
//            }
//        }
//    }
//}