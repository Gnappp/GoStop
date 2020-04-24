using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoStopRules
{
    //점수 계산
    public List<SpecialCombo> CalcurateScore(List<List<Card>> playerAcquiredCards, out int playerScore, out int piScore)
    {
        int score = 0;
        int piscore = 0;
        List<SpecialCombo> combos = new List<SpecialCombo>();
        for (int numofjokbo = 0; numofjokbo < Jokbo.GetNames(typeof(Jokbo)).Length; numofjokbo++)
        {
            switch ((Jokbo)numofjokbo) //족보 먹은패 체크
            {
                case Jokbo.GWONG:
                    if (playerAcquiredCards[numofjokbo].Count == 3) //삼광
                    {
                        for (int B_search = 0; B_search < playerAcquiredCards[numofjokbo].Count; B_search++)
                        {
                            if (playerAcquiredCards[numofjokbo][B_search].Get_month() == 12)
                            {
                                combos.Add(SpecialCombo.GWONG3B);
                                score += 2;
                            }
                            else
                            {
                                combos.Add(SpecialCombo.GWONG3);
                                score += 3;
                            }
                        }
                    }
                    else if (playerAcquiredCards[numofjokbo].Count == 4) //사광
                    {
                        combos.Add(SpecialCombo.GWONG4);
                        score += 4;
                    }
                    else if (playerAcquiredCards[numofjokbo].Count == 5) //오광
                    {
                        combos.Add(SpecialCombo.GWONG5);
                        score += 15;
                    }
                    break;
                case Jokbo.GODORI:
                    if (playerAcquiredCards[numofjokbo].Count == 3)
                    {
                        combos.Add(SpecialCombo.GODORI);
                        score += 5;
                    }
                    break;
                case Jokbo.HONGDAN:
                    if (playerAcquiredCards[numofjokbo].Count == 3)
                    {
                        combos.Add(SpecialCombo.HONGDAN);
                        score += 3;
                    }
                    break;
                case Jokbo.CHODAN:
                    if (playerAcquiredCards[numofjokbo].Count == 4)
                    {
                        combos.Add(SpecialCombo.CHODAN);
                        score += 3;
                    }
                    break;
                case Jokbo.CHUNGDAN:
                    if (playerAcquiredCards[numofjokbo].Count == 3)
                    {
                        combos.Add(SpecialCombo.CHUNGDAN);
                        score += 3;
                    }
                    break;
                case Jokbo.PI:
                    for (int piofnum = 0; piofnum < playerAcquiredCards[numofjokbo].Count; piofnum++)
                    {
                        if (playerAcquiredCards[numofjokbo][piofnum].Get_ssangpi())
                            piscore += 2;
                        else
                            piscore += 1;
                    }
                    if (piscore >= 10)
                    {
                        score = piscore - 9;
                    }
                    break;
                case Jokbo.YEOLGGUT:
                    int GGUTscore = playerAcquiredCards[(int)Jokbo.YEOLGGUT].Count + playerAcquiredCards[(int)Jokbo.GODORI].Count;
                    if (GGUTscore >= 5)
                    {
                        score = GGUTscore - 4;
                    }
                    break;
            } //족보 먹은패 체크
        }
        int TTIscore = playerAcquiredCards[(int)Jokbo.CHODAN].Count + playerAcquiredCards[(int)Jokbo.CHUNGDAN].Count + playerAcquiredCards[(int)Jokbo.HONGDAN].Count; //청단 홍단 초단 띠의 수를 더함
        if (TTIscore >= 5)
        {
            score = TTIscore - 4;
        }
        playerScore = score;
        piScore = piscore;
        return combos;
    }

    //턴 종료한 유저기준으로 상대먹은 카드를 보고 상대박을 셋해준다.
    public List<Bak> AddTurnWaitPlayerBak(List<List<Card>> turnendPlayerCards, List<List<Card>> turnwaitPlayerCards)
    {
        List<Bak> bak_ptr = new List<Bak>();
        for (int numofjokbo = 0; numofjokbo < Jokbo.GetNames(typeof(Jokbo)).Length; numofjokbo++)
        {
            switch ((Jokbo)numofjokbo)
            {
                case Jokbo.GWONG:
                    if (turnendPlayerCards[numofjokbo].Count >= 3)
                    {
                        if (turnwaitPlayerCards[numofjokbo].Count == 0)
                            bak_ptr.Add(Bak.GWANG);
                    }
                    break;
                case Jokbo.YEOLGGUT:
                    if (turnendPlayerCards[numofjokbo].Count + turnendPlayerCards[(int)Jokbo.GODORI].Count >= 7)
                    {
                        if (turnwaitPlayerCards[numofjokbo].Count + turnwaitPlayerCards[(int)Jokbo.GODORI].Count == 0)
                            bak_ptr.Add(Bak.MUNG);
                    }
                    break;
                case Jokbo.CHODAN:
                    if (turnendPlayerCards[numofjokbo].Count + turnendPlayerCards[(int)Jokbo.CHUNGDAN].Count + turnendPlayerCards[(int)Jokbo.HONGDAN].Count >= 5)
                    {
                        if (turnwaitPlayerCards[numofjokbo].Count + turnwaitPlayerCards[(int)Jokbo.CHUNGDAN].Count + turnwaitPlayerCards[(int)Jokbo.HONGDAN].Count == 0)
                            bak_ptr.Add(Bak.TTI);
                    }
                    break;
                case Jokbo.PI:
                    if (turnendPlayerCards[numofjokbo].Count >= 10)
                    {
                        if (turnwaitPlayerCards[numofjokbo].Count > 0 && turnwaitPlayerCards[numofjokbo].Count < 6)
                            bak_ptr.Add(Bak.PI);
                    }
                    break;
            }
        }
        return bak_ptr;
    }

    //턴 종료한 유저가 박이 있었다면 제거
    public List<Bak> RemoveTrunEndPlayerBak(List<List<Card>> turnendPlayerCards, List<Bak> turnendPlayerBaks)
    {
        List<Bak> bak_ptr = new List<Bak>();

        for (int numofbak = turnendPlayerBaks.Count - 1; numofbak >= 0; numofbak--)
        {
            switch (turnendPlayerBaks[numofbak])
            {
                case Bak.GWANG:
                    if (turnendPlayerCards[(int)Jokbo.GWONG].Count == 0)
                        turnendPlayerBaks.RemoveAt(numofbak);
                    break;
                case Bak.MUNG:
                    if (turnendPlayerCards[(int)Jokbo.GODORI].Count + turnendPlayerCards[(int)Jokbo.YEOLGGUT].Count == 0)
                        turnendPlayerBaks.RemoveAt(numofbak);
                    break;
                case Bak.TTI:
                    if (turnendPlayerCards[(int)Jokbo.CHODAN].Count + turnendPlayerCards[(int)Jokbo.CHUNGDAN].Count + turnendPlayerCards[(int)Jokbo.HONGDAN].Count == 0)
                        turnendPlayerBaks.RemoveAt(numofbak);
                    break;
                case Bak.PI:
                    if (turnendPlayerCards[(int)Jokbo.PI].Count > 0 && turnendPlayerCards[(int)Jokbo.PI].Count < 6)
                        turnendPlayerBaks.RemoveAt(numofbak);
                    break;
            }
        }

        return turnendPlayerBaks;
    }

    public void playHandCardAddTobottomCard(List<Card> playHandCard, ref List<List<Card>> bottomCard)
    {
        if (playHandCard[0].Get_month() != 13 || playHandCard[0].Get_month() != 0)
        {
            for (int NumofbottomCard = 0; NumofbottomCard < bottomCard.Count; NumofbottomCard++)
            {
                if (bottomCard[NumofbottomCard][0].Get_month() == playHandCard[0].Get_month())
                {
                    for (int NumofplayHandCard = 0; NumofplayHandCard < playHandCard.Count; NumofplayHandCard++)
                        bottomCard[NumofbottomCard].Add(playHandCard[NumofplayHandCard]);
                }
                else if (bottomCard.Count - 1 == NumofbottomCard)
                {
                    bottomCard.Add(playHandCard);
                }
            }
        }
        else if (playHandCard[0].Get_month() == 13)
            bottomCard.Add(playHandCard);
        else if (playHandCard[0].Get_month() == 0)
            return;
    }
    private void SupportplayHandCardTobottomCard(List<Card> playHandCard, ref List<List<Card>> bottomCard, ref List<TurnEndState> turnEndStates)
    {
        if (playHandCard[0].Get_month() != 13 || playHandCard[0].Get_month() != 0)
        {
            for (int NumofbottomCard = 0; NumofbottomCard < bottomCard.Count; NumofbottomCard++)
            {
                if (bottomCard[NumofbottomCard][0].Get_month() == playHandCard[0].Get_month())
                {
                    for (int NumofplayHandCard = 0; NumofplayHandCard < playHandCard.Count; NumofplayHandCard++)
                        bottomCard[NumofbottomCard].Add(playHandCard[NumofplayHandCard]);
                }
                else if (bottomCard.Count - 1 == NumofbottomCard)
                {
                    bottomCard.Add(playHandCard);
                }
            }
        }
        else if (playHandCard[0].Get_month() == 13)
            bottomCard.Add(playHandCard);
        else if (playHandCard[0].Get_month() == 0)
            return;
    }

    public bool deckDrowCardAddTobottomCard(List<Card> playHandCard, Card deckDrowCard, ref List<List<Card>> bottomCard)
    {
        if (deckDrowCard.Get_month() != 13) //드로우카드 != 조커
        {
            for (int NumofbottomCard = 0; NumofbottomCard < bottomCard.Count; NumofbottomCard++)
            {
                if (bottomCard[NumofbottomCard][0].Get_month() == deckDrowCard.Get_month())
                {
                    bottomCard[NumofbottomCard].Add(deckDrowCard);
                    return true;
                }
                else if (bottomCard.Count - 1 == NumofbottomCard)
                {
                    List<Card> ptr = new List<Card>();
                    ptr.Add(deckDrowCard);
                    bottomCard.Add(ptr);
                    return true;
                }
            }
        }

        else if (deckDrowCard.Get_month() == 13) //드로우카드 == 조커
        {
            if (playHandCard[0].Get_month() != 0) //낸카드 != 더미카드
                //내카드가 붙은곳으로 붙여야함
            {
                if (playHandCard[0].Get_month() != 13 || playHandCard[0].Get_month() != 0)
                {
                    for (int NumofbottomCard = 0; NumofbottomCard < bottomCard.Count; NumofbottomCard++)
                    {
                        if (bottomCard[NumofbottomCard][0].Get_month() == playHandCard[0].Get_month())
                        {
                            bottomCard[NumofbottomCard].Add(deckDrowCard);
                            return false;
                        }
                    }
                }
            }
            else if (playHandCard[0].Get_month() == 0) //낸카드 == 더미
                //낸카드가 없으므로 바닥에 나둠 (나중에 회수)
            {
                List<Card> ptr = new List<Card>();
                ptr.Add(deckDrowCard);
                bottomCard.Add(ptr);
                return false;
            }
        }
        return true;
    }

    public List<TurnEndState> TurnEndBottomCardsSet(Card playHandCard, Card deckDrowCard, ref Player turnendPlayer, ref Player turnwaitPlayer, ref List<List<Card>> bottomCard)
    //--두장이상일때 선택하게하기--
    {
        List<TurnEndState> turnEndStates = new List<TurnEndState>();
        for (int numofbottom = bottomCard.Count - 1; numofbottom >= 0; numofbottom--)
        {
            //따닥 DDADAK
            if (bottomCard[numofbottom].Count == 2)
            {
                if (playHandCard.Get_month() == deckDrowCard.Get_month() && playHandCard.Get_month() == bottomCard[numofbottom][0].Get_month())
                {
                    turnendPlayer.AddAcquiredCard(bottomCard[numofbottom]);
                    turnendPlayer.AddAcquiredCard(playHandCard);
                    turnendPlayer.AddAcquiredCard(deckDrowCard);
                    turnendPlayer.AddAcquiredCard(turnwaitPlayer.LosePI());
                    if (playHandCard.Get_month() == 5)
                        turnEndStates.Add(TurnEndState.GOKJIN);
                    turnEndStates.Add(TurnEndState.DDADAK);
                    playHandCard = null;
                    deckDrowCard = null;
                    bottomCard.RemoveAt(numofbottom);
                }
                break;
            }

            //뻑 BALK
            if (playHandCard != null && deckDrowCard != null)
            {
                if (playHandCard.Get_month() == deckDrowCard.Get_month() && playHandCard.Get_month() == bottomCard[numofbottom][0].Get_month())
                {
                    bottomCard[numofbottom].Add(playHandCard);
                    bottomCard[numofbottom].Add(deckDrowCard);
                    turnEndStates.Add(TurnEndState.BALK);
                    playHandCard = null;
                    deckDrowCard = null;
                    break;
                }
            }

            //먹기(손패)
            if (playHandCard != null)
            {
                if (bottomCard[numofbottom].Count == 3)//뻑먹기 GET_BALK
                {
                    if (bottomCard[numofbottom][0].Get_month() == playHandCard.Get_month())
                    {
                        turnendPlayer.AddAcquiredCard(bottomCard[numofbottom]);
                        turnendPlayer.AddAcquiredCard(playHandCard);
                        turnendPlayer.AddAcquiredCard(turnwaitPlayer.LosePI());
                        playHandCard = null;
                        bottomCard.RemoveAt(numofbottom);
                        turnEndStates.Add(TurnEndState.GET_BALK);
                    }
                }
                else if (bottomCard[numofbottom].Count == 2) //두장이상일때 선택
                {
                    turnEndStates.Add(TurnEndState.P_SELECT_CARD);

                    playHandCard = null;
                }
                if (bottomCard[numofbottom][0].Get_month() == playHandCard.Get_month()) //일반
                {
                    turnendPlayer.AddAcquiredCard(playHandCard);
                    turnendPlayer.AddAcquiredCard(bottomCard[numofbottom]);
                    playHandCard = null;
                    bottomCard.RemoveAt(numofbottom);
                }
            }


            //먹기(드로우)
            if (deckDrowCard != null)
            {
                if (bottomCard[numofbottom].Count == 3)//뻑먹기 GET_BALK
                {
                    if (bottomCard[numofbottom][0].Get_month() == deckDrowCard.Get_month())
                    {
                        turnendPlayer.AddAcquiredCard(bottomCard[numofbottom]);
                        turnendPlayer.AddAcquiredCard(deckDrowCard);
                        turnendPlayer.AddAcquiredCard(turnwaitPlayer.LosePI());
                        deckDrowCard = null;
                        bottomCard.RemoveAt(numofbottom);
                        turnEndStates.Add(TurnEndState.GET_BALK);
                    }
                }
                else if (bottomCard[numofbottom].Count == 2) //두장이상일때 선택
                {
                    turnEndStates.Add(TurnEndState.D_SELECT_CARD);

                    deckDrowCard = null;
                }
                else if (bottomCard[numofbottom][0].Get_month() == deckDrowCard.Get_month())
                {
                    turnendPlayer.AddAcquiredCard(deckDrowCard);
                    turnendPlayer.AddAcquiredCard(bottomCard[numofbottom]);
                    deckDrowCard = null;
                    bottomCard.RemoveAt(numofbottom);
                }
            }
        }

        if (deckDrowCard != null && playHandCard != null)
        {
            //쪽 KISS
            if (playHandCard.Get_month() == deckDrowCard.Get_month())
            {
                turnendPlayer.AddAcquiredCard(playHandCard);
                turnendPlayer.AddAcquiredCard(deckDrowCard);
                turnendPlayer.AddAcquiredCard(turnwaitPlayer.LosePI());
                turnEndStates.Add(TurnEndState.KISS);
                playHandCard = null;
                deckDrowCard = null;
            }
        }

        //처리하지 못한카드들 바닥에 두기
        if (playHandCard != null)
        {
            List<Card> ptr = new List<Card>();
            ptr.Add(playHandCard);
            bottomCard.Add(ptr);
            playHandCard = null;
        }
        if (deckDrowCard != null)
        {
            List<Card> ptr = new List<Card>();
            ptr.Add(deckDrowCard);
            bottomCard.Add(ptr);
            deckDrowCard = null;
        }

        //싹쓸 CLEAR
        if (bottomCard.Count == 0)
        {
            turnendPlayer.AddAcquiredCard(turnwaitPlayer.LosePI());
            turnEndStates.Add(TurnEndState.CLEAR);
        }

        return turnEndStates;
    }
    
    public List<TurnEndState> TurnEndBottomCardsSet(List<Card> playHandCard, Card deckDrowCard, ref Player turnendPlayer, ref Player turnwaitPlayer, ref List<List<Card>> bottomCard)
    {
        List<TurnEndState> turnEndStates = new List<TurnEndState>();

        for (int numofbottom = bottomCard.Count - 1; numofbottom >= 0; numofbottom--)
        {
            if(bottomCard[numofbottom][0].Get_month()==playHandCard[0].Get_month()) //폭탄
            {
                turnEndStates.Add(TurnEndState.BOOM);
                turnendPlayer.AddAcquiredCard(bottomCard[numofbottom]);
                turnendPlayer.AddAcquiredCard(playHandCard);
                turnendPlayer.AddAcquiredCard(turnwaitPlayer.LosePI());
                playHandCard = null;
                bottomCard.RemoveAt(numofbottom);
            }

            if (deckDrowCard != null)
            {
                if (bottomCard[numofbottom][0].Get_month() == deckDrowCard.Get_month())
                {
                    if (bottomCard[numofbottom].Count == 2) //두장이상일때 선택
                    {
                        turnEndStates.Add(TurnEndState.D_SELECT_CARD);
                        deckDrowCard = null;
                    }

                    else if (bottomCard[numofbottom].Count == 3)//뻑먹기 GET_BALK
                    {
                        if (bottomCard[numofbottom][0].Get_month() == deckDrowCard.Get_month())
                        {
                            turnendPlayer.AddAcquiredCard(bottomCard[numofbottom]);
                            turnendPlayer.AddAcquiredCard(deckDrowCard);
                            turnendPlayer.AddAcquiredCard(turnwaitPlayer.LosePI());
                            deckDrowCard = null;
                            bottomCard.RemoveAt(numofbottom);
                            turnEndStates.Add(TurnEndState.GET_BALK);
                        }
                    }
                    else if (bottomCard[numofbottom][0].Get_month() == deckDrowCard.Get_month())
                    {
                        turnendPlayer.AddAcquiredCard(deckDrowCard);
                        turnendPlayer.AddAcquiredCard(bottomCard[numofbottom]);
                        deckDrowCard = null;
                        bottomCard.RemoveAt(numofbottom);
                    }
                }
            }
        }

        if (deckDrowCard != null)
        {
            List<Card> ptr = new List<Card>();
            ptr.Add(deckDrowCard);
            bottomCard.Add(ptr);
            deckDrowCard = null;
        }

        //싹쓸 CLEAR
        if (bottomCard.Count == 0)
        {
            turnendPlayer.AddAcquiredCard(turnwaitPlayer.LosePI());
            turnEndStates.Add(TurnEndState.CLEAR);
        }

        return turnEndStates;
    }
}
