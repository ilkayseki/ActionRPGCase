using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joy; // Joystick referansı
    public float speed; // Hareket hızı
    public Transform target; // Hedef referansı
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

        // Hedefe bakma işlemi
        Vector3 targetDirection = target.position - transform.position;
        targetDirection.y = 0; // Y eksenindeki farkı yok sayıyoruz
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        // Eğer joystickten bir hareket varsa
        if (joyMovement.magnitude > 0.1f)
        {
            // Yönlendirme işlemi
            Vector3 moveDirection = new Vector3(joyMovement.x, 0, joyMovement.z);
            moveDirection = Camera.main.transform.TransformDirection(moveDirection); // Kameraya göre yönlendirme
            moveDirection.y = 0; // Y eksenindeki hareketi engelle

            // Hareketi gerçekleştiriyoruz
            characterController.Move(moveDirection * speed * Time.deltaTime);

            // Hedefin Z ekseninde karaktere göre nerede olduğunu kontrol ediyoruz
            float zDifference = target.position.z - transform.position.z;

            // Hedef ilerideyse animasyonları düz, gerideyse ters şekilde ayarla
            if (zDifference >= 0)
            {
                animator.SetFloat("VelocityX", joyHorizontalMove);
                animator.SetFloat("VelocityZ", joyVerticalMove);
            }
            else
            {
                // Hedef gerideyse animasyonları ters çevir
                animator.SetFloat("VelocityX", -joyHorizontalMove);
                animator.SetFloat("VelocityZ", -joyVerticalMove);
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
}
