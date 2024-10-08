using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joy; // Joystick referansı
    public float speed; // Hareket hızı
    [SerializeField] private CharacterController characterController;
    public GrounderFBBIK grounder; // Final IK Grounder bileşeni
    private Animator animator; // Animator bileşeni

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
        // Joystick değerlerini alıyoruz
        float joyHorizontalMove = joy.Horizontal;
        float joyVerticalMove = joy.Vertical;

        // Hareket yönünü oluşturuyoruz
        Vector3 joyMovement = new Vector3(joyHorizontalMove, 0, joyVerticalMove);
        joyMovement.Normalize();

        // Eğer joystickten bir hareket varsa
        if (joyMovement.magnitude > 0.1f)
        {
            // Yönlendirme işlemi
            Vector3 moveDirection = new Vector3(joyMovement.x, 0, joyMovement.z);
            moveDirection = Camera.main.transform.TransformDirection(moveDirection); // Kameraya göre yönlendirme
            moveDirection.y = 0; // Y eksenindeki hareketi engelle

            // Hareketi gerçekleştiriyoruz
            characterController.Move(moveDirection * speed * Time.deltaTime);

            // Animator parametrelerini güncelliyoruz
            animator.SetFloat("VelocityX", joyHorizontalMove);
            animator.SetFloat("VelocityZ", joyVerticalMove);
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
}