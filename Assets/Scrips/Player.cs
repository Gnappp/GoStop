using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Bak
{
    GO,PI,GWANG,MUNG,TTI,
}
enum SpecialCombo
{
    GWONG3,GWONG3B,GWONG4,GWONG5,HONGDAN,CHUNGDAN,CHODAN,GODORI
}


public class Player
{
    private string name;
    private List<Bak> Bakstates; //현재상태를 저장(고박, 피박, 광박,멍박, 띠박)
    private List<SpecialCombo> comboStates;
    private int score; //점수계산을 받아 입력받음
    private int goNum; //플레이어 고 수
    private int multipleScore; //플레이어의 배수
    private List<Card> handCards; //현재 플레이어 손패
    private List<List<Card>> acquiredCards; //먹은 카드들 분류(족보로 분류)

    public Player(string _name)
    {
        name = _name;
        Bakstates = new List<Bak>();
        comboStates = new List<SpecialCombo>();
        score = 0;
        goNum = 0;
        multipleScore = 1;
        handCards = new List<Card>();
        acquiredCards = new List<List<Card>>();
        for (int i = 0; i < Jokbo.GetNames(typeof(Jokbo)).Length; i++) //족보별로 나두기위해 2차원배열을 사용
            acquiredCards.Add(new List<Card>());

    }

    private void  CalcurateScore(Card getCard)
    {
        int jokbo = (int)getCard.Get_jokbo();

        if ((Jokbo)jokbo == Jokbo.GWONG) //광 점수 작업
        {
            if (acquiredCards[jokbo].Count == 3)
            {
                for (int B_search = 0; B_search < acquiredCards[jokbo].Count; B_search++)
                {
                    if (acquiredCards[jokbo][B_search].Get_month() == 12)
                    {
                        comboStates.Add(SpecialCombo.GWONG3B);
                        score += 2;
                    }
                    else
                    {
                        comboStates.Add(SpecialCombo.GWONG3);
                        score += 3;
                    }
                }
            }
            else if (acquiredCards[jokbo].Count == 4)
            {
                for (int numofCombo = 0; numofCombo < comboStates.Count; numofCombo++)
                {
                    if (comboStates[numofCombo] == SpecialCombo.GWONG3B)
                    {
                        comboStates[numofCombo] = SpecialCombo.GWONG4;
                        score += 2;
                    }
                    else if (comboStates[numofCombo] == SpecialCombo.GWONG3)
                    {
                        comboStates[numofCombo] = SpecialCombo.GWONG4;
                        score += 1;
                    }
                }
            }
            else if (acquiredCards[jokbo].Count == 5)
            {
                for (int numofCombo = 0; numofCombo < comboStates.Count; numofCombo++)
                {
                    if (comboStates[numofCombo] == SpecialCombo.GWONG4)
                    {
                        comboStates[numofCombo] = SpecialCombo.GWONG5;
                        score += 1;
                    }
                }
            }
        }
        else if ((Jokbo)jokbo == Jokbo.GODORI)
        {
            if (acquiredCards[jokbo].Count == 3)
            {
                comboStates.Add(SpecialCombo.GODORI);
                score += 5;
            }
        }
        else if ((Jokbo)jokbo == Jokbo.HONGDAN)
        {
            if (acquiredCards[jokbo].Count == 3)
            {
                comboStates.Add(SpecialCombo.HONGDAN);
                score += 3;
            }
        }
        else if ((Jokbo)jokbo == Jokbo.CHODAN)
        {
            if (acquiredCards[jokbo].Count == 4)
            {
                comboStates.Add(SpecialCombo.CHODAN);
                score += 3;
            }
        }
        else if ((Jokbo)jokbo == Jokbo.CHUNGDAN)
        {
            if (acquiredCards[jokbo].Count == 3)
            {
                comboStates.Add(SpecialCombo.CHUNGDAN);
                score += 3;
            }
        }
    }

    public int CalurateMultipleScore()
    {

        return multipleScore;
    }

    public void AddAcquiredCard(List<Card> getCards) //내고나서 먹은 패
    {
        for(int numofCard = 0; numofCard<getCards.Count;numofCard++)
        {
            acquiredCards[(int)getCards[numofCard].Get_jokbo()].Add(getCards[numofCard]);
        }
    }




}

