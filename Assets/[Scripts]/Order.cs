using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour
{
    public bool paint;
    public bool violin;
    public bool flute;
    public bool sculp;

    public Table table;
    public int cost;

    public float proggress;
    public Image fill;

    public bool isAvailable;

    private void Start()
    {
        isAvailable = true;
    }

    private void Update()
    {
        fill.fillAmount = proggress / 100;
        if (!isAvailable)
        {
            proggress -= 35 * Time.deltaTime;
            if (proggress <= 0)
            {
                isAvailable = true;
                fill.GetComponent<Image>().color = new Color32(0, 255, 11, 150);
            }
        }
    }

    public void GetBusy()
    {
        isAvailable = false;
        fill.GetComponent<Image>().color = new Color32(255, 19, 0, 150);
    }
}
