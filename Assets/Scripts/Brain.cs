using Unity.VisualScripting;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

//Brain is the controller of the character and is sits between the character and the dna. So this reads the DNA and then determines what to do and then tells actual character what to do 

[RequireComponent(typeof(ThirdPersonCharacter))]
public class Brain : MonoBehaviour
{
    int DNALenght = 18;
    public float timeAlive = 0;
    public int timesJump = 0;

    float timeWalking = 0;

    //sequence of DNA
    public DNA dna { get; set; }

    private ThirdPersonCharacter m_Character;
    private Vector3 m_Move;
    private bool m_jump;
    public bool alive { get; set; } = true;//to stop record the timealive
    bool seeGround = true;

    private Vector3 initPos;
    public float totalDist = 0;

    Quaternion targetRotation;
    float rotationSpeed = 5;
    float speed = 10;

    public float pointsCatch = 0;
    public GameObject eyes;


    private void Start()
    {
        initPos = transform.position;
    }

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
        if (other.gameObject.CompareTag("dead"))
        {
            alive = false;
            timeAlive = 0;
            timeWalking = 0;
            distanceHit = 0;
            totalDist = 0;
        }
    }

    public float collWall = 0;
    float timeTouchWall = 0;

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            if (timeTouchWall >= 2)
            {
                collWall--;
                timeTouchWall = 0;
            }


        }
    }

    float distanceHit = 0;
    bool seeWall = false;
    bool seeEnemy = false;
    RaycastHit hit;
    RaycastHit hitFoward, hitFoward2;

    bool seeGround2 = false;
    bool seeGround3 = false;
    bool seeWall2 = false;
    bool seeWall3 = false;
    bool seeEnemy2 = false;
    bool seeEnemy3 = false;
    private void FixedUpdate()
    {


        if (!alive) return;
        timeTouchWall += Time.deltaTime;

        // Atualizar continuamente a posição atual do bot
        Vector3 currentPosition = transform.position;

        // Calcular a distância total percorrida
        totalDist = Vector3.Distance(initPos, currentPosition);

        distanceHit = 0;

        seeGround = false;
        seeGround2 = false;
        seeGround3 = false;
        seeWall2 = false;
        seeWall3 = false;
        seeWall = false;
        seeEnemy = false;
        seeEnemy2 = false;
        seeEnemy3 = false;
        distanceHit = 0;


        if (Physics.Raycast(eyes.transform.position, eyes.transform.forward * 10, out hit))
        {
            if (hit.collider.gameObject != null)
            {
                distanceHit = hit.distance;
            }

            if (hit.collider.gameObject.CompareTag("platform"))
            {
                seeGround = true;
            }

            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                seeWall = true;
            }

            if (hit.collider.gameObject.CompareTag("catch"))
            {
                seeEnemy = true;
                pointsCatch++;
            }

        }

        if (Physics.Raycast(eyes.transform.position, Quaternion.Euler(45, 0, 0) * eyes.transform.forward * 0.2f, out hitFoward))
        {

            if (hitFoward.collider != null && hitFoward.collider.gameObject.CompareTag("platform"))
            {
                seeGround2 = true;
            }

            if (hitFoward.collider != null && hitFoward.collider.gameObject.CompareTag("Wall"))
            {
                seeWall2 = true;
            }

            if (hitFoward.collider != null && hitFoward.collider.gameObject.CompareTag("catch"))
            {
                seeEnemy2 = true;
                pointsCatch++;
            }

        }

        if (Physics.Raycast(eyes.transform.position, Quaternion.Euler(45, 0, 0) * eyes.transform.forward * 1.5f, out hitFoward2))
        {
            if (hitFoward2.collider != null && hitFoward2.collider.gameObject.CompareTag("platform"))
            {
                seeGround3 = true;
            }

            if (hitFoward2.collider != null && hitFoward2.collider.gameObject.CompareTag("Wall"))
            {
                seeWall3 = true;
            }

            if (hitFoward2.collider != null && hitFoward2.collider.gameObject.CompareTag("catch"))
            {
                seeEnemy3 = true;
                pointsCatch++;
            }

        }
        Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 10, Color.red);
        Debug.DrawRay(eyes.transform.position, Quaternion.Euler(45, 0, 0) * eyes.transform.forward * 0.5f, Color.red);
        Debug.DrawRay(eyes.transform.position, Quaternion.Euler(45, 0, 0) * eyes.transform.forward * 1.5f, Color.red);




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
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(0) == 2)
            {
                turn = 1; //turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(0) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(0) == 4)
            {
                crouch = true; // crouch
            }
        }
        else if (seeWall)
        {
            if (dna.GetGene(1) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(1) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(1) == 2)
            {
                turn = 1; // turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(1) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(1) == 4)
            {
                crouch = true; // crouch
            }
        }
        else if (seeEnemy)
        {
            if (dna.GetGene(2) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(2) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(2) == 2)
            {
                turn = 1; // turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(2) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(2) == 4)
            {
                crouch = true; // crouch
            }
        }
        else if (!seeGround)
        {
            if (dna.GetGene(3) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(3) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(3) == 2)
            {
                turn = 1; //turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(3) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(3) == 4)
            {
                crouch = true; // crouch
            }
        }
        else if (!seeWall)
        {
            if (dna.GetGene(4) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(4) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(4) == 2)
            {
                turn = 1; // turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(4) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(4) == 4)
            {
                crouch = true; // crouch
            }
        }
        else if (!seeEnemy)
        {
            if (dna.GetGene(5) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(5) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(5) == 2)
            {
                turn = 1; // turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(5) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(5) == 4)
            {
                crouch = true; // crouch
            }
        }


        else if (seeGround2)
        {
            if (dna.GetGene(6) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(6) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(6) == 2)
            {
                turn = 1; //turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(6) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(6) == 4)
            {
                crouch = true; // crouch
            }
        }
        else if (seeWall2)
        {
            if (dna.GetGene(7) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(7) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(7) == 2)
            {
                turn = 1; // turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(7) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(7) == 4)
            {
                crouch = true; // crouch
            }
        }
        else if (seeEnemy2)
        {
            if (dna.GetGene(8) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(8) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(8) == 2)
            {
                turn = 1; // turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(8) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(8) == 4)
            {
                crouch = true; // crouch
            }
        }
        else if (!seeGround2)
        {
            if (dna.GetGene(9) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(9) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(9) == 2)
            {
                turn = 1; //turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(9) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(9) == 4)
            {
                crouch = true; // crouch
            }
        }
        else if (!seeWall2)
        {
            if (dna.GetGene(10) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(10) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(10) == 2)
            {
                turn = 1; // turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(10) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(10) == 4)
            {
                crouch = true; // crouch
            }
        }
        else if (!seeEnemy2)
        {
            if (dna.GetGene(11) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(11) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(11) == 2)
            {
                turn = 1; // turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(11) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(11) == 4)
            {
                crouch = true; // crouch
            }
        }


        else if (seeGround3)
        {
            if (dna.GetGene(12) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(12) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(12) == 2)
            {
                turn = 1; //turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(12) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(12) == 4)
            {
                crouch = true; // crouch
            }
        }
        else if (seeWall3)
        {
            if (dna.GetGene(13) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(13) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(13) == 2)
            {
                turn = 1; // turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(13) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(13) == 4)
            {
                crouch = true; // crouch
            }
        }
        else if (seeEnemy3)
        {
            if (dna.GetGene(14) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(14) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(14) == 2)
            {
                turn = 1; // turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(14) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(14) == 4)
            {
                crouch = true; // crouch
            }
        }
        else if (!seeGround3)
        {
            if (dna.GetGene(15) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(15) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(15) == 2)
            {
                turn = 1; //turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(15) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(15) == 4)
            {
                crouch = true; // crouch
            }
        }
        else if (!seeWall3)
        {
            if (dna.GetGene(16) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(16) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(16) == 2)
            {
                turn = 1; // turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(16) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(16) == 4)
            {
                crouch = true; // crouch
            }
        }
        else if (!seeEnemy3)
        {
            if (dna.GetGene(17) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(17) == 1)
            {
                turn = -1; // turn left
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(17) == 2)
            {
                turn = 1; // turn right
                targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(17) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(17) == 4)
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