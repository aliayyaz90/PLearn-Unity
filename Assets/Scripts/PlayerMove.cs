using UnityEngine;
public class PlayerMove : MonoBehaviour
{
    public FixedJoystick joystick;
    public float SpeedMove = 5f;
    private CharacterController controller;
    public float GroundDistance = 0.3f;
    public Transform Ground;
    public LayerMask layermask;
    Vector3 velocity;
    public float jumpheight = 3f;
    Animator animator;
    public bool isGround;
    public bool Pressed;
    void Start()
    {
        Application.targetFrameRate = 60;
        controller=transform.parent.GetComponent<CharacterController>();
        animator = GetComponent<Animator>();  
    }    
    void Update()
    {
        velocity.y = -2f;
        Vector3 Move = transform.right * joystick.Horizontal + transform.forward * joystick.Vertical;
        controller.Move(Move * SpeedMove * Time.deltaTime);
        controller.Move(velocity*Time.deltaTime);
    }
    private void FixedUpdate()
    {
        if (joystick.Horizontal!=0 || joystick.Vertical != 0)
            animator.SetBool("run", true);
        else
            animator.SetBool("run", false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "alexGO6" && PlayerPrefs.GetInt("gameprogress")==5)
        {
            GameManagerModule2.instance.GetComponent<Lesson2_2>().enabled = true;
            GameManagerModule2.instance.GetComponent<Lesson2_2>().counter = 2;
            GameManagerModule2.instance.GetComponent<Lesson2_2>().Task2_2NextBtn();
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "patel" && PlayerPrefs.GetInt("gameprogress") == 6)
        {
            GameManagerModule2.instance.thompsonNPC.GetComponent<BoxCollider>().enabled = false;
            GameManagerModule2.instance.alexGO.SetActive(false); 
            GameManagerModule2.instance.PatelGO5.SetActive(false);             
        }
    }
}