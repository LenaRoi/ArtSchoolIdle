using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EKTemplate;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private AreaOpener area;

    public bool canPlay;
    public bool isRunning;

    public float speed;

    public int itemCount;

    public Rigidbody rb;
    private Animator animator;

    private float timer;

    public Transform allTransforms;

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
            other.gameObject.transform.DOLocalMove(allTransforms.transform.GetChild(itemCount).localPosition , 0.25f);
            other.gameObject.transform.DOLocalRotateQuaternion(allTransforms.transform.GetChild(itemCount).localRotation , 0.25f);
            other.gameObject.transform.DOScale(allTransforms.transform.GetChild(itemCount).localScale , 0.25f);
            itemCount++;
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
                    GameManager.instance.money--;
                    area.cost--;
                    timer = 0.002f;
                }
                else
                {
                    timer -= 1 * Time.deltaTime;
                }

                if (area.cost == 1)
                {
                    area.OpenArea();
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }
}