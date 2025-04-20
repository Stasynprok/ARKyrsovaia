using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlatformType
{
    Common,
    Dead,
    Boost,
    Finish,
    Collapsing
}

public class PlatformBuff : MonoBehaviour
{
    public PlatformType Type;
    public int Buff;

    public void OnPlatformTouch()
    {
        if (Type == PlatformType.Collapsing)
        {
            gameObject.SetActive(false);
        }
    }
}

