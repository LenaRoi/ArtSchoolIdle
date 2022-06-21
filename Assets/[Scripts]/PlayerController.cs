using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EKTemplate;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private AreaOpener area;
    public Table table;
    public Order order;

    public bool canPlay;
    public bool isRunning;
    public bool helping;

    public float speed;

    public int itemCount;
    public GameObject[] allItems;

    public int paintCount;
    public GameObject[] allPaints;

    public int violinCount;
    public GameObject[] allViolin;

    public int fluteCount;
    public GameObject[] allFlute;

    public int sculpCount;
    public GameObject[] allSculp;

    public Rigidbody rb;
    private Animator animator;

    private float timer;

    public Transform allTransforms;

    public Student student;

    public float helpRatio;

    public int value2;
    public float speed2;

    public GameObject targetObject;

    private void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.money += 5000;
        }
        if (helping)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (student.workProgress + helpRatio < 100)
                {
                    student.workProgress += helpRatio;
                }
                else
                {
                    student.workProgress = 100;
                }
            }

            if (student.isWorking == false)
            {
                CameraManager.instance.GetNormal();
                helping = false;
                canPlay = true;
                LevelManager.instance.tutor.CloseTutorial();
            }
        }
    }

    void FixedUpdate()
    {
        if (canPlay)
        {
            Vector3 movement = new Vector3(InputManager.instance.input.x, 0, InputManager.instance.input.y);
            rb.velocity = movement * speed;

            if ((InputManager.instance.input.x != 0 || InputManager.instance.input.y != 0) && !isRunning)
            {
                animator.ResetTrigger("Carry");
                animator.ResetTrigger("Idle");
                if (itemCount > 0)
                {
                    animator.SetTrigger("CarryWalk");
                }
                else
                {
                    animator.SetTrigger("Walk");
                }
                isRunning = true;
            }
            else if ((InputManager.instance.input.x == 0 && InputManager.instance.input.y == 0) && isRunning)
            {
                animator.ResetTrigger("Walk");
                animator.ResetTrigger("CarryWalk");
                if (itemCount > 0)
                {
                    animator.SetTrigger("Carry");
                }
                else
                {
                    animator.SetTrigger("Idle");
                }
                isRunning = false;
            }

            if (isRunning)
            {
                float angle = Vector3.SignedAngle(Vector3.forward, movement, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.up), 15 * Time.fixedDeltaTime);
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Area"))
        {
            area = other.gameObject.GetComponent<AreaOpener>();
            timer = 0.25f;
            area.FirstInteract();
        }
        if (other.gameObject.CompareTag("PaintTool"))
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.transform.parent = transform;
            other.gameObject.transform.DOLocalMove(allTransforms.transform.GetChild(itemCount).localPosition, 0.25f);
            other.gameObject.transform.DOLocalRotateQuaternion(allTransforms.transform.GetChild(itemCount).localRotation, 0.25f);
            other.gameObject.transform.DOScale(allTransforms.transform.GetChild(itemCount).localScale, 0.25f);
            allItems[itemCount] = other.gameObject;
            itemCount++;

            allPaints[paintCount] = other.gameObject;
            paintCount++;
        }
        if (other.gameObject.CompareTag("Violin"))
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.transform.parent = transform;
            other.gameObject.transform.DOLocalMove(allTransforms.transform.GetChild(itemCount).localPosition + new Vector3(0,0,-0.05f), 0.25f);
            other.gameObject.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 135, 0), 0.25f);
            other.gameObject.transform.DOScale(allTransforms.transform.GetChild(itemCount).localScale, 0.25f);
            allItems[itemCount] = other.gameObject;
            itemCount++;

            allViolin[violinCount] = other.gameObject;
            violinCount++;
        }
        if (other.gameObject.CompareTag("Flute"))
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.transform.parent = transform;
            other.gameObject.transform.DOLocalMove(allTransforms.transform.GetChild(itemCount).localPosition + new Vector3(-0.25f, 0, -0.2f), 0.25f);
            other.gameObject.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 225, 0), 0.25f);
            other.gameObject.transform.DOScale(allTransforms.transform.GetChild(itemCount).localScale, 0.25f);
            allItems[itemCount] = other.gameObject;
            itemCount++;

            allFlute[fluteCount] = other.gameObject;
            fluteCount++;
        }
        if (other.gameObject.CompareTag("Sculp"))
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.transform.parent = transform;
            other.gameObject.transform.DOLocalMove(allTransforms.transform.GetChild(itemCount).localPosition + new Vector3(0,0,-0.15f), 0.25f);
            other.gameObject.transform.DOLocalRotateQuaternion(allTransforms.transform.GetChild(itemCount).localRotation, 0.25f);
            other.gameObject.transform.DOScale(allTransforms.transform.GetChild(itemCount).localScale, 0.25f);
            allItems[itemCount] = other.gameObject;
            itemCount++;

            allSculp[sculpCount] = other.gameObject;
            sculpCount++;
        }
        if (other.gameObject.CompareTag("CollectTable"))
        {
            table = other.gameObject.GetComponent<Table>();
            table.isBusy = true;
            timer = 0.05f;
        }
        if (other.gameObject.CompareTag("Order"))
        {
            order = other.gameObject.GetComponent<Order>();
        }
        if (other.gameObject.CompareTag("Student"))
        {
            student = other.gameObject.GetComponent<Student>();
            if (student.workProgress > 0 && student.workProgress < 100 && itemCount == 0)
            {
                CameraManager.instance.HelpCam();
                LevelManager.instance.tutor.HelpTutorial();
                canPlay = false;
                helping = true;
                animator.SetTrigger("Help");
            }
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Area"))
        {
            if (area.cost > 0 && GameManager.instance.money > 0)
            {
                if (timer <= 0)
                {
                    GameManager.instance.AddMoney(-1);
                    area.cost--;
                    timer = 0.002f;
                }
                else
                {
                    timer -= 1 * Time.deltaTime;
                }

                if (area.cost == 0)
                {
                    area.OpenArea();
                }
            }
        }
        if (other.gameObject.CompareTag("CollectTable"))
        {
            if (table.paintStack)
            {
                if (paintCount > 0 && table.totalItem < 160)
                {
                    if (timer <= 0)
                    {
                        itemCount--;
                        paintCount--;
                        timer = 0.25f;
                        targetObject = allPaints[paintCount];
                        targetObject.transform.parent = null;
                        targetObject.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0.1f);
                        targetObject.transform.DOMove(table.areas[table.totalItem].transform.position, 0.1f).OnComplete(() =>
                        {
                            targetObject.transform.parent = other.gameObject.transform;


                            for (int i = 0; i < allItems.Length; i++)
                            {
                                if (allItems[i] == targetObject)
                                {
                                    allItems[i] = null;
                                }
                            }
                            targetObject = null;
                            allPaints[paintCount] = null;
                            CarryControl();
                        });
                        table.totalItem++;
                    }
                    else
                    {
                        timer -= 1 * Time.deltaTime;
                    }
                }
            }

            if (table.violinStack)
            {
                if (violinCount > 0 && table.totalItem < 160)
                {
                    if (timer <= 0)
                    {
                        itemCount--;
                        violinCount--;
                        timer = 0.25f;
                        targetObject = allViolin[violinCount];
                        targetObject.transform.parent = null;
                        targetObject.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0.1f);
                        targetObject.transform.DOMove(table.areas[table.totalItem].transform.position, 0.1f).OnComplete(() =>
                        {
                            targetObject.transform.parent = other.gameObject.transform;


                            for (int i = 0; i < allItems.Length; i++)
                            {
                                if (allItems[i] == targetObject)
                                {
                                    allItems[i] = null;
                                }
                            }
                            targetObject = null;
                            allViolin[violinCount] = null;
                            CarryControl();
                        });
                        table.totalItem++;
                    }
                    else
                    {
                        timer -= 1 * Time.deltaTime;
                    }
                }
            }
            if (table.fluteStack)
            {
                if (fluteCount > 0 && table.totalItem < 160)
                {
                    if (timer <= 0)
                    {
                        itemCount--;
                        fluteCount--;
                        timer = 0.25f;
                        targetObject = allFlute[fluteCount];
                        targetObject.transform.parent = null;
                        targetObject.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0.1f);
                        targetObject.transform.DOMove(table.areas[table.totalItem].transform.position, 0.1f).OnComplete(() =>
                        {
                            targetObject.transform.parent = other.gameObject.transform;


                            for (int i = 0; i < allItems.Length; i++)
                            {
                                if (allItems[i] == targetObject)
                                {
                                    allItems[i] = null;
                                }
                            }
                            targetObject = null;
                            allFlute[fluteCount] = null;
                            CarryControl();
                        });
                        table.totalItem++;
                    }
                    else
                    {
                        timer -= 1 * Time.deltaTime;
                    }
                }             
            }
            if (table.sculpStack)
            {
                if (sculpCount > 0 && table.totalItem < 160)
                {
                    if (timer <= 0)
                    {
                        itemCount--;
                        sculpCount--;
                        timer = 0.25f;
                        targetObject = allSculp[sculpCount];
                        targetObject.transform.parent = null;
                        targetObject.transform.DORotateQuaternion(Quaternion.Euler(-90f, -90f, 0), 0.1f);
                        targetObject.transform.DOMove(table.areas[table.totalItem].transform.position, 0.1f).OnComplete(() =>
                        {
                            targetObject.transform.parent = other.gameObject.transform;


                            for (int i = 0; i < allItems.Length; i++)
                            {
                                if (allItems[i] == targetObject)
                                {
                                    allItems[i] = null;
                                }
                            }
                            targetObject = null;
                            allSculp[sculpCount] = null;
                            CarryControl();
                        });
                        table.totalItem++;
                    }
                    else
                    {
                        timer -= 1 * Time.deltaTime;
                    }
                }
            }
        }
        if (other.gameObject.CompareTag("Order"))
        {
            if (order.proggress >= 100 && GameManager.instance.money >= order.cost && order.isAvailable)
            {
                order.GetBusy();
                order.table.GetOrder();
                GameManager.instance.AddMoney(-order.cost);
            }
            else
            {
                if (GameManager.instance.money >= order.cost && order.isAvailable)
                {
                    order.proggress += 50 * Time.deltaTime;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CollectTable"))
        {
            table.isBusy = false;
        }
        if (other.gameObject.CompareTag("Order"))
        {
            if (order.isAvailable)
            {
                order.proggress = 0;
            }
        }
    }


    public void CarryControl()
    {
        for (int i = 0; i < 198; i++)
        {
            if (allItems[i] == null)
            {
                if (allItems[i + 1] != null)
                {
                    allItems[i] = allItems[i + 1];
                    allItems[i + 1] = null;
                }
                else if (allItems[i + 2] != null)
                {
                    allItems[i] = allItems[i + 2];
                    allItems[i + 2] = null;
                }
                //else if (allItems[i + 3] != null)
                //{
                //    allItems[i] = allItems[i + 3];
                //    allItems[i + 3] = null;
                //}
                //else if (allItems[i + 4] != null)
                //{
                //    allItems[i] = allItems[i + 4];
                //    allItems[i + 4] = null;
                //}
                //else if (allItems[i + 5] != null)
                //{
                //    allItems[i] = allItems[i + 5];
                //    allItems[i + 5] = null;
                //}
                //else if (allItems[i + 6] != null)
                //{
                //    allItems[i] = allItems[i + 6];
                //    allItems[i + 6] = null;
                //}
                //else if (allItems[i + 7] != null)
                //{
                //    allItems[i] = allItems[i + 7];
                //    allItems[i + 7] = null;
                //}
                //else if (allItems[i + 8] != null)
                //{
                //    allItems[i] = allItems[i + 8];
                //    allItems[i + 8] = null;
                //}
                //else if (allItems[i + 9] != null)
                //{
                //    allItems[i] = allItems[i + 9];
                //    allItems[i + 9] = null;
                //}
                //else if (allItems[i + 10] != null)
                //{
                //    allItems[i] = allItems[i + 10];
                //    allItems[i + 10] = null;
                //}
                //else if (allItems[i + 11] != null)
                //{
                //    allItems[i] = allItems[i + 11];
                //    allItems[i + 11] = null;
                //}
                //else if (allItems[i + 12] != null)
                //{
                //    allItems[i] = allItems[i + 12];
                //    allItems[i + 12] = null;
                //}
                //else if (allItems[i + 13] != null)
                //{
                //    allItems[i] = allItems[i + 13];
                //    allItems[i + 13] = null;
                //}
                //else if (allItems[i + 14] != null)
                //{
                //    allItems[i] = allItems[i + 14];
                //    allItems[i + 14] = null;
                //}
                //else if (allItems[i + 15] != null)
                //{
                //    allItems[i] = allItems[i + 15];
                //    allItems[i + 15] = null;
                //}
                //else if (allItems[i + 16] != null)
                //{
                //    allItems[i] = allItems[i + 16];
                //    allItems[i + 16] = null;
                //}
                //else if (allItems[i + 17] != null)
                //{
                //    allItems[i] = allItems[i + 17];
                //    allItems[i + 17] = null;
                //}
                //else if (allItems[i + 18] != null)
                //{
                //    allItems[i] = allItems[i + 18];
                //    allItems[i + 18] = null;
                //}
            }
        }

        for (int i = 0; i < itemCount; i++)
        {
            allItems[i].transform.DOLocalMoveY(allTransforms.transform.GetChild(i).transform.localPosition.y , 0.05f);
        }
    }
}