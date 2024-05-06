using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

//Brain is the controller of the character and is sits between the character and the dna. So this reads the DNA and then determines what to do and then tells actual character what to do 

[RequireComponent(typeof(ThirdPersonCharacter))]
public class Brain : MonoBehaviour
{
    public int DNALenght = 2;
    public float timeAlive = 0;

    public float timeWalking = 0;

    //sequence of DNA
    public DNA dna { get; set; }

    private ThirdPersonCharacter m_Character;
    private Vector3 m_Move;
    private bool m_jump;
    public bool alive { get; set; } = true;//to stop record the timealive
    bool seeGround = true;

    Quaternion targetRotation;
    float rotationSpeed = 5;
    float speed = 10;

    public GameObject eyes;

    public void Init()
    {
        //intitialize DNA, the DNA is just one gene
        //0 foward
        //1 back
        //2 left 
        //3 right
        //4 jump
        //5 crouch

        //initialize a random dna with one value and the range can be from 0 to 5, 
        dna = new DNA(DNALenght, 5);
        m_Character = GetComponent<ThirdPersonCharacter>();
        timeAlive = 0;
        alive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "dead")
        {
            alive = false;
            timeAlive = 0;
            timeWalking = 0;
        }
    }
    
    private void FixedUpdate()
    {


        if (!alive) return;

        
        seeGround = false;
        RaycastHit hit;

        if (Physics.Raycast(eyes.transform.position, eyes.transform.forward * 10, out hit))
        {
            if (hit.collider.gameObject.tag == "platform")
            {
                seeGround = true;
            }
        }
        Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 10, Color.red);




        float turn = 0;// right and left moviment, 1 = right, -1 = left
        float move = 0;//forward and backward movement, 1 = foward, -1 = backward
        bool crouch = false;
        //get genes at position 0 to simplify the algorithm

        if (seeGround)
        {
            if (dna.GetGene(0) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(0) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 90, 0);
            }
            else if (dna.GetGene(0) == 2)
            {
                turn = 1; //turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 90, 0);
            }
            else if (dna.GetGene(0) == 3)
            {
                m_jump = true; // jump
            }
            else if (dna.GetGene(0) == 4)
            {
                crouch = true; // crouch
            }
        }
        else
        {
            if (dna.GetGene(1) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(1) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 90, 0);
            }
            else if (dna.GetGene(1) == 2)
            {
                turn = 1; // turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 90, 0);
            }
            else if (dna.GetGene(1) == 3)
            {
                m_jump = true; // jump
            }
            else if (dna.GetGene(1) == 4)
            {
                crouch = true; // crouch
            }
        }

        // interpolation of the rotation
        if (targetRotation != Quaternion.identity)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // movement calculation
        Vector3 forwardMovement = move * transform.forward;
        Vector3 rightMovement = turn * transform.right;

        // use the moviment in the character
        m_Move = forwardMovement + rightMovement;
        m_Character.Move(m_Move * speed, crouch, m_jump);

        m_jump = false;
        if (alive)
            timeAlive += Time.deltaTime;

    }


}