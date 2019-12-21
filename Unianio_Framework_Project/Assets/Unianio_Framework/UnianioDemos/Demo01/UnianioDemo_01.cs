using Unianio.Events;
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
        play<IdleAni>().Set(e.Human);
    }
}
