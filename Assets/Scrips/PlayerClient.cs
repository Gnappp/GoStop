using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClient : MonoBehaviour
{
    public string name;
    private Player player;

    public void Start()
    {
        player = new Player(name);
    }

    public void RequireFromManager(Player p,SendPlayInfo otherPlayer,List<SpecialCombo> specialCombo,List<TurnEndState> turnEndStates,
        bool turn,List<Card> playCards,List<Card> drowCard, int reqireNum)
    {

    }

    public void RequireSelcectCard(List<TurnEndState> turnEndStates)
    {

    }
}
