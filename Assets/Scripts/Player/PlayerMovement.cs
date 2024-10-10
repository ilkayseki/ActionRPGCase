using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joy; // Joystick referansı
    public float speed; // Hareket hızı
    [SerializeField] private CharacterController characterController;
    public GrounderFBBIK grounder; // Final IK Grounder bileşeni
    private Animator animator; // Animator bileşeni

    private Transform target;
    
    [Inject]
    private NearestEnemyTracker nearestEnemyTracker;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (grounder == null)
        {
            grounder = GetComponent<GrounderFBBIK>();
        }

        animator = GetComponent<Animator>(); // Animator bileşenini alıyoruz
    }

    void Update()
    {
        // Joystick ve WASD değerlerini alıyoruz
        float joyHorizontalMove = joy.Horizontal;
        float joyVerticalMove = joy.Vertical;
        float horizontalInput = Input.GetAxis("Horizontal"); // Klavye yatay ekseni (A ve D tuşları)
        float verticalInput = Input.GetAxis("Vertical"); // Klavye dikey ekseni (W ve S tuşları)

        // Joystick veya klavyeden gelen hareketleri birleştiriyoruz
        Vector3 inputMovement = new Vector3(joyHorizontalMove + horizontalInput, 0, joyVerticalMove + verticalInput);
        inputMovement.Normalize();

        // Hedefe bakma işlemi
        LookAtTarget();
        
        Vector3 targetDirection = target.position - transform.position;
        targetDirection.y = 0; // Y eksenindeki farkı yok sayıyoruz
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        // Eğer joystick veya klavyeden bir hareket varsa
        if (inputMovement.magnitude > 0.1f)
        {
            // Yönlendirme işlemi
            Vector3 moveDirection = new Vector3(inputMovement.x, 0, inputMovement.z);
            //moveDirection = Camera.main.transform.TransformDirection(moveDirection); // Kameraya göre yönlendirme
            moveDirection.y = 0; // Y eksenindeki hareketi engelle

            // Hareketi gerçekleştiriyoruz
            characterController.Move(moveDirection * speed * Time.deltaTime);

            // Hedefin Z ekseninde karaktere göre nerede olduğunu kontrol ediyoruz
            float zDifference = target.position.z - transform.position.z;

            // Hedef ilerideyse animasyonları düz, gerideyse ters şekilde ayarla
            if (zDifference >= 0)
            {
                animator.SetFloat("VelocityX", inputMovement.x);
                animator.SetFloat("VelocityZ", inputMovement.z);
            }
            else
            {
                // Hedef gerideyse animasyonları ters çevir
                animator.SetFloat("VelocityX", -inputMovement.x);
                animator.SetFloat("VelocityZ", -inputMovement.z);
            }
        }
        else
        {
            // Hareket olmadığında animasyon parametrelerini sıfırla
            animator.SetFloat("VelocityX", 0);
            animator.SetFloat("VelocityZ", 0);
        }

        // Final IK ayak yerleştirmesi
        if (grounder != null)
        {
            grounder.weight = 1.0f; // Ayakların zemine adapte olmasını sağlar
        }
    }

    private void LookAtTarget()
    {
        target = nearestEnemyTracker.GetNearestEnemy();
        GetComponent<LookAtIK>().solver.target = target;
    }


    public void SetSpeed(int s)
    {
        speed = s;
    }
}
