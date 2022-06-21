using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UI;
using EKTemplate;

public class Student : MonoBehaviour
{
    public Table table;

    private Animator animator;
    private NavMeshAgent nav;
    public Transform targetPos;

    public bool canPlay;
    public bool isWorking;
    public bool haveItem;

    public GameObject progressBar;
    public float workProgress;
    public Image fill;

    public GameObject paintUI;
    public GameObject violinUI;
    public GameObject fluteUI;
    public GameObject sculpUI;

    public AreaOpener area;
    public Vector3 oldPos;

    public GameObject hatLast;
    public GameObject hatArt;

    public GameObject[] violinObjects;
    public GameObject[] sculpObjects;
    public GameObject fluteObjects;


    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    void Start()
    {
        canPlay = true;
        animator.SetTrigger("Walk");
        nav.destination = targetPos.position;
    }

    void Update()
    {
        progressBar.transform.parent.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (canPlay)
        {
            if (Mathf.Abs(transform.position.x - targetPos.position.x) < 0.1f && Mathf.Abs(transform.position.z - targetPos.position.z) < 0.1f)
            {
                StartWork();
            }
        }

        if (isWorking)
        {
            workProgress += 5f * Time.deltaTime;
            fill.fillAmount = workProgress / 100;
            if (workProgress >= 100)
            {
                animator.SetTrigger("Idle");
                transform.DOMove(oldPos, 0.1f).OnComplete(() =>
                {
                    StopWork();
                });
                
            }
        }
    }


    public void StartWork()
    {
        if (workProgress >= 100)
        {
            area.isReady = true;
            Destroy(this.gameObject);
        }
        else
        {

            nav.enabled = false;
            oldPos = transform.position;
            if (area.piano)
            {
                transform.DOMove(area.transform.position + new Vector3(0, -0.187f, 0.88f), 0.1f);
                animator.SetTrigger("Piano");
                haveItem = true;
            }
            else if (area.paint)
            {
                if (haveItem)
                {
                    transform.DOMove(area.transform.position + new Vector3(0, -0.5f, 0.32f), 0.1f);
                    animator.SetTrigger("Paint");
                    hatArt.SetActive(true);
                }
                else
                {
                    targetPos = area.table.gameObject.transform;
                    canPlay = true;
                    nav.enabled = true;
                    nav.destination = targetPos.position;
                    paintUI.SetActive(true);
                }
            }
            else if (area.violin)
            { 
                if (haveItem)
                {
                    animator.SetTrigger("Violin");
                    for (int i = 0; i < violinObjects.Length; i++)
                    {
                        violinObjects[i].SetActive(true);
                    }
                }
                else
                {
                    targetPos = area.table.gameObject.transform;
                    canPlay = true;
                    nav.enabled = true;
                    nav.destination = targetPos.position;
                    violinUI.SetActive(true);
                }
            }
            else if (area.flute)
            {
                if (haveItem)
                {
                    animator.SetTrigger("Flute");
                    fluteObjects.SetActive(true);
                }
                else
                {
                    targetPos = area.table.gameObject.transform;
                    canPlay = true;
                    nav.enabled = true;
                    nav.destination = targetPos.position;
                    fluteUI.SetActive(true);
                }
            }
            else if (area.sculp)
            {
                if (haveItem)
                {
                    animator.SetTrigger("Sculp");
                    for (int i = 0; i < sculpObjects.Length; i++)
                    {
                        sculpObjects[i].SetActive(true);
                    }
                }
                else
                {
                    targetPos = area.table.gameObject.transform;
                    canPlay = true;
                    nav.enabled = true;
                    nav.destination = targetPos.position;
                    sculpUI.SetActive(true);
                }
            }
            canPlay = false;
            transform.DORotateQuaternion(targetPos.rotation, 0.25f).OnComplete(() =>
            {
                if (haveItem)
                {
                    OpenProgressBar();
                    isWorking = true;
                }
                else
                {
                    canPlay = true;
                }
            });
        }
    }
    public void StopWork()
    {
        if (isWorking)
        {
            isWorking = false;
            for (int i = 0; i < violinObjects.Length; i++)
            {
                violinObjects[i].SetActive(false);
            }
            for (int i = 0; i < sculpObjects.Length; i++)
            {
                sculpObjects[i].SetActive(false);
            }
            fluteObjects.SetActive(false);
            //area.targetObject.SetActive(true);
            hatArt.SetActive(false);
            hatLast.SetActive(true);
            targetPos = LevelManager.instance.exit.transform;
            nav.enabled = true;
            canPlay = true;
            nav.destination = targetPos.position;
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Walk");

            GetMoney();
            

            

            progressBar.SetActive(false);
        }
       
    }

    public void GetMoney()
    {
        int money = 0;
        if (area.paint)
        {
            money = 20;
        }
        else if (area.violin)
        {
            money = 40;
        }
        else if (area.flute)
        {
            money = 40;
        }
        else if (area.piano)
        {
            money = 30;
        }
        else if (area.sculp)
        {
            money = 50;
        }
        GameManager.instance.AddMoney(money * 4);
        Instantiate(Resources.Load<GameObject>("particles/Money"), transform.position + Vector3.up, Quaternion.Euler(-90f, 0, 0));
    }

    public void OpenProgressBar()
    {
        progressBar.SetActive(true);
        paintUI.SetActive(false);
        violinUI.SetActive(false);
        fluteUI.SetActive(false);
        sculpUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CollectTable"))
        {
            if (haveItem == false)
            {
                nav.enabled = false;
                canPlay = false;
                
                animator.SetTrigger("Idle");
            } 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("CollectTable"))
        {
            if (haveItem == false && area.table.isBusy == false)
            {
                animator.SetTrigger("Idle");
                if (area.table.transform.childCount > 1)
                {
                    area.table.totalItem--;
                    GameObject paint = other.gameObject.transform.GetChild(other.gameObject.transform.childCount - 1).transform.gameObject;
                    paint.transform.parent = transform;
                    haveItem = true;
                    paint.transform.DOLocalMove(Vector3.zero + Vector3.up, 0.5f).OnComplete(() =>
                    {
                        Destroy(paint.gameObject);
                        animator.SetTrigger("Walk");
                        targetPos = area.transform;
                        canPlay = true;
                        nav.enabled = true;
                        nav.destination = targetPos.position;
                    });
                }
            }
        }
          
    }
}
