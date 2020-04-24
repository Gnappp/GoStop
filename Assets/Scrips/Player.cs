using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string name;
    public List<Bak> Bakstates; //현재상태를 저장(고박, 피박, 광박,멍박, 띠박)
    public int score; //점수계산을 받아 입력받음
    public int goNum; //플레이어 고 수
    public bool[] shake;
    public int multipleScore; //플레이어의 배수
    public int piscore;
    public List<List<Card>> acquiredCards { get; } //먹은 카드들 분류(족보로 분류)

    public List<Card> handCards { get; } //현재 플레이어 손패

    public Player(string _name)
    {
        name = _name;
        Bakstates = new List<Bak>();
        score = 0;
        piscore = 0; //피 갯수를 계산하고 출력하여하기 때문에 변수만듬
        goNum = 0;
        shake = new bool[2]; shake[0] = false; shake[1] = false;
        multipleScore = 1;
        handCards = new List<Card>();
        acquiredCards = new List<List<Card>>();
        for (int i = 0; i < Jokbo.GetNames(typeof(Jokbo)).Length; i++) //족보별로 나두기위해 2차원배열을 사용
            acquiredCards.Add(new List<Card>());
    }
     

    public Card LosePI() //패 빼앗길때
    {
        if (acquiredCards[(int)Jokbo.PI].Count == 0)
            return null;
        else if (acquiredCards[(int)Jokbo.PI].Count == 1)
        {
            acquiredCards[(int)Jokbo.PI].RemoveAt(0);
            return acquiredCards[(int)Jokbo.PI][0];
        }
        else
        {
            for (int numofpi = acquiredCards[(int)Jokbo.PI].Count - 1; numofpi >= 0; numofpi--)
            {
                if (!acquiredCards[(int)Jokbo.PI][numofpi].Get_ssangpi())
                {
                    acquiredCards[(int)Jokbo.PI].RemoveAt(numofpi);
                    return acquiredCards[(int)Jokbo.PI][numofpi];
                }
            }
        }
        return null;
    }

    public void AddAcquiredCard(List<Card> getCards) //내고나서 먹은 패
    {
        for (int numofCard = 0; numofCard < getCards.Count; numofCard++)
        {
            acquiredCards[(int)getCards[numofCard].Get_jokbo()].Add(getCards[numofCard]);
        }
    }
    public void AddAcquiredCard(Card getCard)
    {
        acquiredCards[(int)getCard.Get_jokbo()].Add(getCard);
    }
   
    public int Get_HandCardCount()
    {
        return handCards.Count;
    }

    public void Delete_HandCard(Card card)
    {
        for(int numofcard = 0; numofcard<handCards.Count;numofcard++)
        {
            if(handCards[numofcard]==card)
            {
                handCards.RemoveAt(numofcard);
                break;
            }
        }
    }

    public void CreateDummyCard()
    {
        Card dummy = new Card(13, Jokbo.PI, false, 1);
        handCards.Add(dummy);
    }

}

