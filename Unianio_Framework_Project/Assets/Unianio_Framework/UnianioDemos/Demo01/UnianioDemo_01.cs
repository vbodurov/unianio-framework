using Unianio;
using Unianio.Events;
using Unianio.Services;
using UnianioDemos.Demo01;
using UnityEngine;
using static Unianio.Static.fun;

public class UnianioDemo_01 : MonoBehaviour
{
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
