using Unianio;
using Unianio.Extensions;
using UnityEngine;
using static Unianio.Static.fun;

public class Test1 : MonoBehaviour
{
    void Start()
    {
        var cube = GameObject.Find("Cube").transform;

        playEndless(a =>
        {
            cube.position = lerp(v3.zero, v3.one.By(5), sin(Time.time).FromMin11To01());
        });
    }
}
