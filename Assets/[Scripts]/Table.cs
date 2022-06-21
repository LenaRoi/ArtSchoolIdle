using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Table : MonoBehaviour
{
    public Transform[] areas;
    public GameObject car;
    public Vector3 carPos;

    public int totalItem;

    public bool paint;
    public bool paintStack;

    public bool violin;
    public bool violinStack;

    public bool flute;
    public bool fluteStack;

    public bool sculp;
    public bool sculpStack;

    public bool isReady;
    public bool isBusy;
    void Start()
    {
        isReady = true;
       // GetOrder();
    }

    public void GetOrder()
    {
        carPos = car.transform.position;
        car.transform.DOMoveX(car.transform.position.x + 15f, 2f).OnComplete(() =>
        {
            GetNewObjects();
        });
    }

    public void GetNewObjects()
    {
        if (paint)
        {
            for (int i = 0; i < areas.Length; i++)
            {
                GameObject newPaint = Instantiate(Resources.Load<GameObject>("PaintTool"), car.transform.position + Vector3.up * 2, areas[i].transform.rotation);
                newPaint.transform.DOMove(areas[i].position, 0.5f);
            }
            car.transform.DOMove(car.transform.position, 0.5f).OnComplete(() =>
            {
                car.transform.DOMove(carPos, 2);
            });
            
        }
        if (violin)
        {
            for (int i = 0; i < areas.Length; i++)
            {
                GameObject newPaint = Instantiate(Resources.Load<GameObject>("Violin"), car.transform.position + Vector3.up * 2, areas[i].transform.rotation);
                newPaint.transform.DOMove(areas[i].position, 0.5f);
            }
            car.transform.DOMove(car.transform.position, 0.5f).OnComplete(() =>
            {
                car.transform.DOMove(carPos, 2);
            });
        }

        if (flute)
        {
            for (int i = 0; i < areas.Length; i++)
            {
                GameObject newPaint = Instantiate(Resources.Load<GameObject>("Flute"), car.transform.position + Vector3.up * 2, areas[i].transform.rotation);
                newPaint.transform.DOMove(areas[i].position, 0.5f);
            }
            car.transform.DOMove(car.transform.position, 0.5f).OnComplete(() =>
            {
                car.transform.DOMove(carPos, 2);
            });
        }

        if (sculp)
        {
            for (int i = 0; i < areas.Length; i++)
            {
                GameObject newPaint = Instantiate(Resources.Load<GameObject>("Sculp"), car.transform.position + Vector3.up * 2, areas[i].transform.rotation);
                newPaint.transform.DOMove(areas[i].position, 0.5f);
            }
            car.transform.DOMove(car.transform.position, 0.5f).OnComplete(() =>
            {
                car.transform.DOMove(carPos, 2);
            });
        }
    }
}
