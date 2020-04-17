using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Player
{
    private string name;
    private List<Bak> Bakstates; //현재상태를 저장(고박, 피박, 광박,멍박, 띠박)
    private int score; //점수계산을 받아 입력받음
    private int goNum; //플레이어 고 수
    private int multipleScore; //플레이어의 배수
    private int piscore;
    private List<Card> handCards; //현재 플레이어 손패
    private List<List<Card>> acquiredCards; //먹은 카드들 분류(족보로 분류)

    public Player(string _name)
    {
        name = _name;
        Bakstates = new List<Bak>();
        score = 0;
        piscore = 0; //피 갯수를 계산하고 출력하여하기 때문에 변수만듬
        goNum = 0;
        multipleScore = 1;
        handCards = new List<Card>();
        acquiredCards = new List<List<Card>>();
        for (int i = 0; i < Jokbo.GetNames(typeof(Jokbo)).Length; i++) //족보별로 나두기위해 2차원배열을 사용
            acquiredCards.Add(new List<Card>());
    }
    public int CalurateMultipleScore()
    {

        return multipleScore;
    }
    public List<Bak> Get_Bakstates() { return Bakstates; }

    public void Set_Bakstates(List<Bak> _baks) { Bakstates = _baks; }

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
    public List<List<Card>> Get_acquiredCards() { return acquiredCards; }

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

    public string Get_name() { return name; }

    public void Set_score(int _score) { score = _score; }








}

