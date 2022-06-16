using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UI;
using EKTemplate;

public class Student : MonoBehaviour
{
    public float speed;
    private Animator animator;
    private NavMeshAgent nav;
    public Transform targetPos;

    public bool canPlay;

    public GameObject progressBar;
    public float workProgress;
    public Image fill;
    public bool isWorking;

    public AreaOpener area;
    public Vector3 oldPos;
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    void Start()
    {
        canPlay = true;
        animator.SetTrigger("Walk");
    }

    void Update()
    {
        if (canPlay)
        {
            nav.destination = targetPos.position;

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
            }
            else if (area.paint)
            {
                transform.DOMove(area.transform.position + new Vector3(0, -0.5f, 0.32f), 0.1f);
                animator.SetTrigger("Paint");
            }
            canPlay = false;
            transform.DORotateQuaternion(targetPos.rotation, 0.25f).OnComplete(() =>
            {
                progressBar.SetActive(true);
                isWorking = true;
            });
        }
    }
    public void StopWork()
    {
        isWorking = false;
        targetPos = LevelManager.instance.exit.transform;
        nav.enabled = true;
        canPlay = true;
        animator.ResetTrigger("Idle");
        animator.SetTrigger("Walk");
    }
}
