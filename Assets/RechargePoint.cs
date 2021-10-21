using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RechargePoint : MonoBehaviour
{
    bool isUsed;
    GameObject aliveGraphics;
    GameObject brokenGraphics;

    private void Awake()
    {
        aliveGraphics = transform.Find("Graphics_Alive").gameObject;
        brokenGraphics = transform.Find("Graphics_Broken").gameObject;
    }

    private void Start()
    {
        brokenGraphics?.SetActive(false);
        isUsed = false;
    }

    private void OnEnable()
    {
        isUsed = false;
    }
    public bool IsUsed()
    {
        return isUsed;

    }

    public void UseRechargePoint()
    {
        aliveGraphics?.SetActive(false);
        brokenGraphics?.SetActive(true);
        isUsed = true;
    }

    private void OnDisable()
    {
        aliveGraphics?.SetActive(true);
        brokenGraphics?.SetActive(true);
    }
}
