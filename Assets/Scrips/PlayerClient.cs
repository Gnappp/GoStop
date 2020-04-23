using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClient : MonoBehaviour
{
    public string name;
    private List<Bak> Bakstates; //현재상태를 저장(고박, 피박, 광박,멍박, 띠박)
    private int score; //점수계산을 받아 입력받음
    private int goNum; //플레이어 고 수
    private bool[] shake;
    private int multipleScore; //플레이어의 배수
    private int piscore;
    private List<Card> handCards; //현재 플레이어 손패
    private List<List<Card>> acquiredCards; //먹은 카드들 분류(족보로 분류)


    public void RequireSelcectCard(List<TurnEndState> turnEndStates)
    {

    }
}
