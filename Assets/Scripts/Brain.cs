using Unity.VisualScripting;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

//Brain is the controller of the character and is sits between the character and the dna. So this reads the DNA and then determines what to do and then tells actual character what to do 

[RequireComponent(typeof(ThirdPersonCharacter))]
public class Brain : MonoBehaviour
{
    int DNALenght = 36;
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
    float speed = 40;

    public float pointsCatch = 0;
    public GameObject eyes, eyes2;


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
        dna = new DNA(DNALenght, 360);
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
    float timeCatchEnemy = 0;
    public float catchEnemy = 0;

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
        if (other.gameObject.CompareTag("catch"))
        {
            if (timeCatchEnemy >= 1)
            {
                catchEnemy++;
                timeCatchEnemy = 0;
            }


        }
    }

    float distanceHit = 0;
    bool seeWall = false;
    bool seeEnemy = false;
    RaycastHit hit;
    RaycastHit hitFoward, hitFoward2, hitFoward3, hitFoward4, hitFoward5, hitFoward6;

    bool seeGround2 = false;
    bool seeGround3 = false;
    bool seeWall2 = false;
    bool seeWall3 = false;
    bool seeEnemy2 = false;
    bool seeEnemy3 = false;

    bool seeGround4 = false;
   bool seeWall4 = false;
    bool seeEnemy4 = false;

    bool seeGround5 =false, seeGround6 =false;
    bool seeEnemy5 =false, seeEnemy6 =false;
     bool seeWall5 =false, seeWall6 =false;

    private void FixedUpdate()
    {


        if (!alive) return;
        timeTouchWall += Time.deltaTime;
        timeCatchEnemy += Time.deltaTime;

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
        seeGround4=false;
        seeWall4 = false;
        seeEnemy4=false;
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


         if (Physics.Raycast(eyes2.transform.position,  eyes2.transform.forward * 10f, out hitFoward3))
        {
            if (hitFoward3.collider != null && hitFoward3.collider.gameObject.CompareTag("platform"))
            {
                seeGround4 = true;
            }

            if (hitFoward3.collider != null && hitFoward3.collider.gameObject.CompareTag("Wall"))
            {
                seeWall4 = true;
            }

            if (hitFoward3.collider != null && hitFoward3.collider.gameObject.CompareTag("catch"))
            {
                seeEnemy4 = true;
                pointsCatch++;
            }

        }




          if (Physics.Raycast(eyes2.transform.position,   Quaternion.Euler(0, -20, 0)* eyes2.transform.right * 0.7f, out hitFoward4))
        {
            if (hitFoward4.collider != null && hitFoward4.collider.gameObject.CompareTag("platform"))
            {
                seeGround5 = true;
            }

            if (hitFoward4.collider != null && hitFoward4.collider.gameObject.CompareTag("Wall"))
            {
                seeWall5 = true;
            }

            if (hitFoward4.collider != null && hitFoward4.collider.gameObject.CompareTag("catch"))
            {
                seeEnemy5 = true;
                pointsCatch++;
            }

        }

          if (Physics.Raycast(eyes2.transform.position,  Quaternion.Euler(0, 180, 0)* eyes2.transform.right * 0.7f, out hitFoward5))
        {
            if (hitFoward5.collider != null && hitFoward5.collider.gameObject.CompareTag("platform"))
            {
                seeGround6 = true;
            }

            if (hitFoward5.collider != null && hitFoward5.collider.gameObject.CompareTag("Wall"))
            {
                seeWall6 = true;
            }

            if (hitFoward5.collider != null && hitFoward5.collider.gameObject.CompareTag("catch"))
            {
                seeEnemy6 = true;
                pointsCatch++;
            }

        }



        
        /*Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 10, Color.red);
        Debug.DrawRay(eyes.transform.position, Quaternion.Euler(45, 0, 0) * eyes.transform.forward * 0.5f, Color.red);
        Debug.DrawRay(eyes.transform.position, Quaternion.Euler(45, 0, 0) * eyes.transform.forward * 1.5f, Color.red);
        Debug.DrawRay(eyes2.transform.position, eyes2.transform.forward * 10f, Color.red);*/
        Debug.DrawRay(eyes2.transform.position,  Quaternion.Euler(0, -50, 0)* eyes2.transform.right * 0.7f, Color.green);
        Debug.DrawRay(eyes2.transform.position,  Quaternion.Euler(0, 250, 0)* eyes2.transform.right * 0.7f, Color.green);




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
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(0) == 2)
            {
                turn = 1; //turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(0) == 5){
                turn=0;
            }
             else if(dna.GetGene(0) == 6){
                move=0;
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
                           // //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(1) == 2)
            {
                turn = 1; // turn right
                ////targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(1) == 5){
                turn=0;
            }
             else if(dna.GetGene(1) == 6){
                move=0;
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
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(2) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(2) == 5){
                turn=0;
            }
             else if(dna.GetGene(2) == 6){
                move=0;
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
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(3) == 2)
            {
                turn = 1; //turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(3) == 5){
                turn=0;
            }
             else if(dna.GetGene(3) == 6){
                move=0;
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
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(4) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(4) == 5){
                turn=0;
            }
             else if(dna.GetGene(4) == 6){
                move=0;
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
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(5) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(5) == 5){
                turn=0;
            }
             else if(dna.GetGene(5) == 6){
                move=0;
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
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(6) == 2)
            {
                turn = 1; //turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(6) == 5){
                turn=0;
            }
             else if(dna.GetGene(6) == 6){
                move=0;
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
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(7) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(7) == 5){
                turn=0;
            }
             else if(dna.GetGene(7) == 6){
                move=0;
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
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(8) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(8) == 5){
                turn=0;
            }
             else if(dna.GetGene(8) == 6){
                move=0;
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
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(9) == 2)
            {
                turn = 1; //turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(9) == 5){
                turn=0;
            }
             else if(dna.GetGene(9) == 6){
                move=0;
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
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(10) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(10) == 5){
                turn=0;
            }
             else if(dna.GetGene(10) == 6){
                move=0;
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
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(11) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(11) == 5){
                turn=0;
            }
             else if(dna.GetGene(11) == 6){
                move=0;
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
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(12) == 2)
            {
                turn = 1; //turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(12) == 5){
                turn=0;
            }
             else if(dna.GetGene(12) == 6){
                move=0;
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
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(13) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(13) == 5){
                turn=0;
            }
             else if(dna.GetGene(13) == 6){
                move=0;
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
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(14) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(14) == 5){
                turn=0;
            }
             else if(dna.GetGene(14) == 6){
                move=0;
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
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(15) == 2)
            {
                turn = 1; //turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(15) == 5){
                turn=0;
            }
             else if(dna.GetGene(15) == 6){
                move=0;
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
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(16) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(16) == 5){
                turn=0;
            }
             else if(dna.GetGene(16) == 6){
                move=0;
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
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(17) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
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
            else if(dna.GetGene(17) == 5){
                turn=0;
            }
             else if(dna.GetGene(17) == 6){
                move=0;
            }
        }




        else if (seeGround4)
        {
            if (dna.GetGene(18) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(18) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(18) == 2)
            {
                turn = 1; //turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(18) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(18) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(18) == 5){
                turn=0;
            }
             else if(dna.GetGene(18) == 6){
                move=0;
            }
        }
        else if (seeWall4)
        {
            if (dna.GetGene(19) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(19) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(19) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(19) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(19) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(19) == 5){
                turn=0;
            }
             else if(dna.GetGene(19) == 6){
                move=0;
            }
        }
        else if (seeEnemy4)
        {
            if (dna.GetGene(20) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(20) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(20) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(20) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(20) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(20) == 5){
                turn=0;
            }
             else if(dna.GetGene(20) == 6){
                move=0;
            }
        }
        else if (!seeGround4)
        {
            if (dna.GetGene(21) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(21) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(21) == 2)
            {
                turn = 1; //turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(21) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(21) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(21) == 5){
                turn=0;
            }
             else if(dna.GetGene(21) == 6){
                move=0;
            }
        }
        else if (!seeWall4)
        {
            if (dna.GetGene(22) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(22) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(22) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(22) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(22) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(22) == 5){
                turn=0;
            }
             else if(dna.GetGene(22) == 6){
                move=0;
            }
        }
        else if (!seeEnemy4)
        {
            if (dna.GetGene(23) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(23) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(23) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(23) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(23) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(23) == 5){
                turn=0;
            }
             else if(dna.GetGene(23) == 6){
                move=0;
            }
        }




         else if (seeGround5)
        {
            if (dna.GetGene(24) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(24) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(24) == 2)
            {
                turn = 1; //turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(24) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(24) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(24) == 5){
                turn=0;
            }
             else if(dna.GetGene(24) == 6){
                move=0;
            }
        }
        else if (seeWall5)
        {
            if (dna.GetGene(25) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(25) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(25) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(25) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(25) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(25) == 5){
                turn=0;
            }
             else if(dna.GetGene(25) == 6){
                move=0;
            }
        }
        else if (seeEnemy5)
        {
            if (dna.GetGene(26) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(26) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(26) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(26) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(26) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(26) == 5){
                turn=0;
            }
             else if(dna.GetGene(26) == 6){
                move=0;
            }
        }
        else if (!seeGround5)
        {
            if (dna.GetGene(27) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(27) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(27) == 2)
            {
                turn = 1; //turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(27) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(27) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(27) == 5){
                turn=0;
            }
             else if(dna.GetGene(27) == 6){
                move=0;
            }
        }
        else if (!seeWall5)
        {
            if (dna.GetGene(28) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(28) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(28) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(28) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(28) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(28) == 5){
                turn=0;
            }
             else if(dna.GetGene(28) == 6){
                move=0;
            }
        }
        else if (!seeEnemy5)
        {
            if (dna.GetGene(29) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(29) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(29) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(29) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(29) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(29) == 5){
                turn=0;
            }
             else if(dna.GetGene(29) == 6){
                move=0;
            }
        }



         else if (seeGround6)
        {
            if (dna.GetGene(30) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(30) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(30) == 2)
            {
                turn = 1; //turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(30) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(30) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(30) == 5){
                turn=0;
            }
             else if(dna.GetGene(30) == 6){
                move=0;
            }
        }
        else if (seeWall6)
        {
            if (dna.GetGene(31) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(31) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(31) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(31) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(31) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(31) == 5){
                turn=0;
            }
             else if(dna.GetGene(31) == 6){
                move=0;
            }
        }
        else if (seeEnemy6)
        {
            if (dna.GetGene(32) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(32) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(32) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(32) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(32) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(32) == 5){
                turn=0;
            }
             else if(dna.GetGene(32) == 6){
                move=0;
            }
        }
        else if (!seeGround6)
        {
            if (dna.GetGene(33) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(33) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(33) == 2)
            {
                turn = 1; //turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(33) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(33) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(33) == 5){
                turn=0;
            }
             else if(dna.GetGene(33) == 6){
                move=0;
            }
        }
        else if (!seeWall6)
        {
            if (dna.GetGene(34) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(34) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(34) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(34) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(34) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(34) == 5){
                turn=0;
            }
             else if(dna.GetGene(34) == 6){
                move=0;
            }
        }
        else if (!seeEnemy6)
        {
            if (dna.GetGene(35) == 0)
            {
                move = 1; // move foward
                timeWalking++;
            }
            else if (dna.GetGene(35) == 1)
            {
               turn = -1; // turn left
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 10, 0);
            }
            else if (dna.GetGene(35) == 2)
            {
                turn = 1; // turn right
                //targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 10, 0);
            }
            else if (dna.GetGene(35) == 3)
            {
                m_jump = true; // jump
                timesJump++;
            }
            else if (dna.GetGene(35) == 4)
            {
                crouch = true; // crouch
            }
            else if(dna.GetGene(35) == 5){
                turn=0;
            }
             else if(dna.GetGene(35) == 6){
                move=0;
            }
        }

        targetRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + turn, 0);
        // interpolation of the rotation

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        // movement calculation
        Vector3 forwardMovement = move * transform.forward;
        Vector3 rightMovement = turn * transform.right;

        // use the moviment in the character
        m_Move = forwardMovement + rightMovement;
        m_Character.Move(m_Move * speed *Time.deltaTime, crouch, m_jump);

        m_jump = false;
        if (alive)
            timeAlive += Time.deltaTime;

    }


}