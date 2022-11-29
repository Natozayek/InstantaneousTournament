using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour, IDataPersistence//IDataPersistance needed for saving
{
    public Rigidbody2D rigidbody;
   // private new SpriteRenderer spriteRenderer;
    private Vector2 direction = Vector2.down;
    public float speed = 5f;
    float speedValue;

    [Header("Input")]
    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;

   
    [Header("Sprites")]
    public SpriteRendererController spriteAnimUp;
    public SpriteRendererController spriteAnimDown;
    public SpriteRendererController spriteAnimLeft;
    public SpriteRendererController spriteAnimRight;
    private SpriteRendererController activeAnimation;


    public Fader fader;

    

    #region Asper work 
    //Asper
    //Needed to know if it is Moving
    public bool isMoving = false;
    //A Vector used to see if there is any input to move X and Y will always be -1,0 or 1
    //and each tile is 1x1 so the distance will always be 1 tile
    Vector2 movement;
    //To check if it is still in bush for a random fight to happen, this bool is changed on the Bush script.
    public bool inBush = false;

    //for talking and interacting in the future
    public GameObject interactBox;
    InteractSquare InteractCollider; // for talking and interacting in the future

    //All of this are to make the character unable to move into objects while respecting the tile movement
    //Also will help on the futute with interaction stuff. I did try using only one that changed places when 
    //Player moved but got stuck with a movement bug
    public GameObject downBox;
    InteractSquare DownCollider;

    public GameObject upBox;
    InteractSquare UpCollider;

    public GameObject leftBox;
    InteractSquare LeftCollider;

    public GameObject rightBox;
    InteractSquare RightCollider;

    public bool canMove = false;

    //To enter battle screen
    public GameObject battleS;

    public Bush selectedBush;

    public bool inBattle = false;
    public AudioManager audioManager;
    #endregion


    private void Awake()
    {
       rigidbody = GetComponent<Rigidbody2D>();
       activeAnimation =  spriteAnimDown;
    }

    #region //Save data for location
    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = this.transform.position;
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
       //To get acces to each object script and know if they are colliding or not
        DownCollider = downBox.GetComponent<InteractSquare>();
        UpCollider = upBox.GetComponent<InteractSquare>();
        LeftCollider = leftBox.GetComponent<InteractSquare>();
        RightCollider = rightBox.GetComponent<InteractSquare>();
        interactBox = downBox;
        InteractCollider = interactBox.GetComponent<InteractSquare>();

        audioManager = GameObject.FindObjectOfType<AudioManager>();

        speedValue = speed;
      
    }

    // Update is called once per frame
    void Update()
    {
        if (inBattle == true)
        {
            speed = 0;
        }
        else
        {
            speed = speedValue;

            if (battleS.gameObject.activeSelf == false) // to avoid movement while fighting
            {
                if (!isMoving)
                {
                    //Check if there is any movement
                    movement.x = Input.GetAxisRaw("Horizontal");
                    movement.y = Input.GetAxisRaw("Vertical");

                    //No diagonal movement
                    if (movement.x != 0)
                    {
                        movement.y = 0;
                    }

                    //Change Direction and animation also will change the interaction box to the one
                    //the player is aiming for and will see if it can or cant move on that direction 
                    //I tried making it so that the box would change position when the player moved 
                    //but for some reason there was a bug.
                    if (movement.y == 1) //Up
                    {
                        SetDirection(Vector2.up, spriteAnimUp);
                        interactBox = upBox;
                        if (UpCollider.isObstacle == false)
                        {
                            canMove = true;
                        }
                        else
                        {
                            canMove = false;
                        }
                    }
                    else if (movement.y == -1) // Down
                    {
                        SetDirection(Vector2.down, spriteAnimDown);
                        interactBox = downBox;
                        if (DownCollider.isObstacle == false)
                        {
                            canMove = true;
                        }
                        else
                        {
                            canMove = false;
                        }
                    }
                    else if (movement.x == -1) // Left
                    {
                        SetDirection(Vector2.left, spriteAnimLeft);
                        interactBox = leftBox;
                        if (LeftCollider.isObstacle == false)
                        {
                            canMove = true;
                        }
                        else
                        {
                            canMove = false;
                        }
                    }
                    else if (movement.x == 1) //Right
                    {
                        SetDirection(Vector2.right, spriteAnimRight);
                        interactBox = rightBox;
                        if (RightCollider.isObstacle == false)
                        {
                            canMove = true;
                        }
                        else
                        {
                            canMove = false;
                        }
                    }
                    else//No Movement
                    {
                        SetDirection(Vector2.zero, activeAnimation);
                    }

                    //Start Movement
                    if (canMove == true)
                    {
                        if (movement != Vector2.zero)
                        {
                            var targetPos = transform.position; //Makes a vector with targer position
                            targetPos.x += movement.x; //Add either 1 or -1 to X which is the tile on top or down
                            targetPos.y += movement.y; //Same but with Y either left or right

                            if (inBattle == false)
                            {
                                StartCoroutine(Move(targetPos)); //Start Move Coroutine for tile base movement
                            }
                        }
                    }
                }
            }
        }
    }

    //Moving position to a new direction
    private void FixedUpdate()
    {

    }

    private void SetDirection(Vector2 newDirection, SpriteRendererController spriteController)
    {
        direction = newDirection;

        spriteAnimUp.enabled = spriteController == spriteAnimUp; 
        spriteAnimDown.enabled = spriteController == spriteAnimDown;
        spriteAnimLeft.enabled = spriteController == spriteAnimLeft;
        spriteAnimRight.enabled = spriteController == spriteAnimRight;

        activeAnimation = spriteController;
        activeAnimation.idle = direction == Vector2.zero;
    }

    IEnumerator Move(Vector3 tPos)
    {
        Vector3 sPos = transform.position;

        isMoving = true; //Now it is moving

        //Will move towards the new position until the diference in distance is
        //less than Epsilon (The smallest value that a float can have different from zero.)
        if(inBattle == false)
        {
            while ((tPos - transform.position).sqrMagnitude > Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, tPos, speed * Time.deltaTime);
                yield return null;
            }
        }

        //while ((tPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, tPos, speed * Time.deltaTime);
        //    yield return null;
        //}

        //Once it reaches now that is the new position for the character
        transform.position = tPos;
        //Is no longer moving
        isMoving = false;

        //If after moving the Character is still in a bush will be chances to start a battle
        if (inBush == true)
        {
            float Chance = Random.value; // a random number between 0 and 1.0
           // Debug.Log("Random: " + Chance);
            if (Chance < 0.1) // a 10% chance
            {
                ToBattle();
            }
            
        }

    }

    public void ToBattle()
    {
        inBattle = true;
        fader.fadeIn();
        StartCoroutine(GoToBattle());
    }

    public void Flee() ////TEST
    {
        battleS.gameObject.SetActive(false);
        audioManager.CrossFadeTO(AudioManager.TrackID.inTown);
        inBattle = false;
    }

    public void GoToCave()
    {
        
        fader.fadeIn();
        fader.StartCoroutine(fader.GoToCaveCoro());
    }

    public void GoToTown()
    {

        fader.fadeIn();
        fader.StartCoroutine(fader.GoToTownCoro());
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CaveNtrance"))
        {
            GoToCave();
        }

        if (other.CompareTag("CaveExit"))
        {
            GoToTown();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bush")
        {
            selectedBush = other.gameObject.GetComponent<Bush>();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bush")
        {
            selectedBush = null;
        }
    }

    public IEnumerator GoToBattle()
    {
        yield return new WaitForSeconds(0.4f);
        battleS.gameObject.SetActive(true);
        audioManager.CrossFadeTO(AudioManager.TrackID.inCave); //
        selectedBush.Encounter();
        fader.fadeOut();
    }
}
