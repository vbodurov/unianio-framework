using Unianio;
using Unianio.Events;
using Unianio.Services;
using UnianioDemos.Demo01;
using UnityEngine;
using static Unianio.Static.fun;

public class UnianioDemo_01 : MonoBehaviour
{
    void Start()
    {
        subscribe<HumanRegistered>(OnHumanRegistered, this);
    }
    void OnHumanRegistered(HumanRegistered e)
    {
        // use this event to access human for the first time
    }
    public void Stand()
    {
        var human = get<IHumanManager>().GetHumanByPersona(humanNamed.John);
        play<IdleAni>().Set(human);
    }
    public void Walk()
    {
        var human = get<IHumanManager>().GetHumanByPersona(humanNamed.John);
        play<WalkAni>().Set(human);
    }
}
