using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendPlayInfo 
{
    public string name;
    public List<Bak> Bakstates; //현재상태를 저장(고박, 피박, 광박,멍박, 띠박)
    public int score; //점수계산을 받아 입력받음
    public int goNum; //플레이어 고 수
    public bool[] shake;
    public int multipleScore; //플레이어의 배수
    public int piscore;
    public List<List<Card>> acquiredCards; //먹은 카드들 분류(족보로 분류)
    public int handCardsCount; //현재 플레이어 손패

    public SendPlayInfo(string na,List<Bak> baks,int sc,int goN,bool[] sh,int mul,int pis,List<List<Card>> ac,int hcc)
    {
        name = na;
        Bakstates = baks;
        score = sc;
        goNum = goN;
        shake = sh;
        multipleScore = mul;
        piscore = pis;
        acquiredCards = ac;
        handCardsCount = hcc;
    }
}
