using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MovimentController : MonoBehaviour
{
    private Animator anim;
    private float speed;
    private float inputX, inputZ;
    private FlashLightController flashLightController;

    public float walkSpeed, runSpeed;
    public CameraController cameraController;
    public GameObject playerModel, attackModel, deathUI;
    public int clicksToTelease = 5;
    public float impulseForce = 20;

    private bool canWalk;
    private bool inAttack;
    private bool cancelAttack;
    private int clicks;
    private GameObject currentEnemy;
    private bool dead;

    void Start()
    {
        anim = GetComponent<Animator>();
        flashLightController = GetComponentInChildren<FlashLightController>();
        canWalk = true;
    }

    void Update()
    {
        SetInputs();

        if(canWalk || dead)
            ToWalk();

        if (dead)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }

        if (inAttack)
        {
            if (Input.GetMouseButtonDown(0))
            {
                clicks++;

                if (clicks == clicksToTelease)
                {
                    cancelAttack = true;
                    ReleaseAttack();
                    clicks = 0;
                }
            }
        }
    }

    void ToWalk()
    {
        anim.SetFloat("horizontal", this.inputX);
        anim.SetFloat("vertical", this.inputZ);
        anim.SetBool("run", Input.GetKey(KeyCode.LeftShift) && inputX == 0 && inputZ > 0);
        anim.SetBool("flashlight", flashLightController.light.enabled);

        transform.Translate(new Vector3(inputX,0, inputZ) * speed * Time.deltaTime);

        if (cameraController.cameraType == CameraController.CameraType.ThirdPerson && (this.inputX != 0 || this.inputZ != 0))
        {
            var direction = new Vector3(inputX, 0, inputZ);
            var camRot = Camera.main.transform.rotation;
            camRot.x = 0;
            camRot.z = 0;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(direction) * camRot, cameraController.thirdPersonRotationSpeed * Time.deltaTime);
        }
    }

    void SetInputs()
    {
        this.inputX = Input.GetAxis("Horizontal");
        this.inputZ = Input.GetAxis("Vertical");

        this.speed = Input.GetKey(KeyCode.LeftShift) && inputX == 0 && inputZ > 0 ? runSpeed : walkSpeed;
    }

    public void ReceiveAttack(GameObject enemy)
    {
        inAttack = true;
        playerModel.SetActive(false);
        attackModel.SetActive(true);
        canWalk = false;
        currentEnemy = enemy;
        currentEnemy.SetActive(false);

        StartCoroutine("DeathPlayer");
    }

    void ReleaseAttack()
    {
        //currentEnemy.SetActive(true);
        //currentEnemy.GetComponent<Rigidbody>().AddForce((-currentEnemy.transform.forward) * impulseForce, ForceMode.Impulse);


        playerModel.SetActive(true);
        attackModel.SetActive(false);
        canWalk = true;
        cancelAttack = true;
    }

    private IEnumerator DeathPlayer()
    {
        yield return new WaitForSeconds(5.0f);

        if (!cancelAttack)
        {
            deathUI.SetActive(true);
            dead = true;
            attackModel.GetComponentInChildren<Animator>().SetTrigger("death");
        }
    }
}
