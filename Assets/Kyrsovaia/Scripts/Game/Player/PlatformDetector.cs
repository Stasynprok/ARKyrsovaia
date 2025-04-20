using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformDetector : MonoBehaviour
{
    [SerializeField] private string _tagPlatforms;
    public UnityEvent<int> onDetectPlatform = new UnityEvent<int>();
    public UnityEvent onDetectFinishPlatform = new UnityEvent();
    public UnityEvent onDetectDeadPlatform = new UnityEvent();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == _tagPlatforms)
        {
            PlatformBuff buff = other.GetComponent<PlatformBuff>();

            if (buff)
            {
                int buffOnPlatform = buff.Buff;

                switch (buff.Type)
                {
                    case PlatformType.Finish:
                        OnDetectFinishPlatform();
                        break;
                    case PlatformType.Collapsing:
                        OnDetectBuffPlatform(buffOnPlatform);
                        break;
                    case PlatformType.Dead:
                        OnDetectDeadPlatform();
                        break;
                    case PlatformType.Boost:
                        OnDetectBuffPlatform(buffOnPlatform);
                        break;
                    case PlatformType.Common:
                        OnDetectBuffPlatform(buffOnPlatform);
                        break;

                }
                buff.OnPlatformTouch();
                onDetectPlatform?.Invoke(buff.Buff);
            }
        }
    }

    private void OnDetectBuffPlatform(int buff)
    {
        onDetectPlatform?.Invoke(buff);
    }

    private void OnDetectFinishPlatform()
    {
        onDetectFinishPlatform?.Invoke();
    }

    private void OnDetectDeadPlatform()
    {
        onDetectDeadPlatform?.Invoke();
    }
}
