using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using EKTemplate;

public class AreaOpener : MonoBehaviour
{
    public Table table;
    public int cost;
    public bool isReady;
    public GameObject targetObject;
    public GameObject border;
    public GameObject particle;
    public TextMeshPro costText;

    private int newStudentTimer;

    public bool paint;
    public bool piano;
    public bool violin;
    public bool flute;
    public bool sculp;


    void Start()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
       // OpenArea();
    }
    private void Update()
    {
        costText.text = cost.ToString("0");
        if (isReady)
        {
            isReady = false;
            newStudentTimer = Random.Range(1, 5);
            StartCoroutine(GetNewStudent());
        }
    }

    public void FirstInteract()
    {
        border.transform.DOScale(new Vector3(0.35f, 0.35f, 0.35f), 0.2f).SetLoops(2, LoopType.Yoyo);
    }
    public void OpenArea()
    {
        GetComponent<BoxCollider>().enabled = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.gameObject.SetActive(false);
        }

        if (targetObject != null)
        {
            Vector3 scale = targetObject.transform.localScale;
            targetObject.transform.localScale = Vector3.zero;
            targetObject.SetActive(true);
            targetObject.transform.DOScale(scale, 0.75f).OnComplete(() =>
            {
                particle.SetActive(true);
                isReady = true;
            });
        }
        else
        {
            particle.SetActive(true);
            isReady = true;
        }
        
    }

    IEnumerator GetNewStudent()
    {
        yield return new WaitForSeconds(newStudentTimer);
        GameObject newStudent = Instantiate(Resources.Load<GameObject>("Student"), LevelManager.instance.exit.transform.position, LevelManager.instance.exit.transform.rotation);
        newStudent.GetComponent<Student>().targetPos = transform;
        newStudent.GetComponent<Student>().area = this;
    }
}
