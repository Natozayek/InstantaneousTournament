using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour, IDataPersistence//IDataPersistance needed for saving
{
    public static MovementController Instance { get; private set; }

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

    private ParticleSystem ps;



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
    public List<InteractSquare> l_Squares;

    public GameObject downBox;
    InteractSquare DownCollider;

    public GameObject upBox;
    InteractSquare UpCollider;

    public GameObject leftBox;
    InteractSquare LeftCollider;

    public GameObject rightBox;
    InteractSquare RightCollider;

    public bool canMoveToPosition = false;

    //To enter battle screen
    public GameObject battleS;

    public Bush selectedBush;

    public bool canMove;
    public AudioManager audioManager;
    public PokemonInventory pokemonInventory;
    public int pokeballsOwned = 6;
    public PositionChangeEnum positionChange = PositionChangeEnum.NONE;

    public NPCScript activeNpc;
    public ChatBox ChatBoxManager;
    public TimePanelManager TimePanel;

    public bool hasStartingPokemon = false;
    public bool hasPokemonAlive;
    public bool InTrainerBattle;
    #endregion


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        activeAnimation = spriteAnimRight;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
        canMove = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(GlobalData.Instance.TournamentTime == true)
        {
            canMove = false;
            if (InTrainerBattle == false)
            {
                activeAnimation = spriteAnimRight;
                SetDirection(Vector2.right, spriteAnimRight);
                StartCoroutine(GoToTrainerBattle());
            }

        }

        if(hasStartingPokemon == true)
        {
            hasPokemonAlive = pokemonInventory.HasPokemonsAlive();
            if (hasPokemonAlive == false)
            {
                if(BattleSceneManager.Instance.isTrainerBattle == false)
                {
                    activeAnimation = spriteAnimRight;
                    inBush = false;
                    BattleEnds();
                    GoToColiseo();
                    positionChange = PositionChangeEnum.DEFEAT;
                    pokemonInventory.HealAllPokemon();
                }
                else
                {
                    BattleSceneManager.Instance.EndGame();
                }

            }
        }



        if(StoryProgression.Instance.onStoryProgression == true)
        {
            canMove = false;
        }

        if (canMove == false)
        {
            speed = 0;

            if (Input.GetKeyDown(KeyCode.X))
            {
                if (pokemonInventory.inMenu == true)
                {
                    //canMove = true;
                    pokemonInventory.TooglePlayerMenu(false);
                }
            }
        }
        else
        {
            speed = speedValue;

            activeNpc = interactBox.GetComponent<InteractSquare>().npc;

            if (Input.GetKeyDown(KeyCode.X))
            {
                if (pokemonInventory.inMenu == false)
                {
                    //canMove = false;
                    pokemonInventory.TooglePlayerMenu(false);
                }
            }

            if (activeNpc != null)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (pokemonInventory.inMenu == false)
                    {
                        Interact();
                    }
                }
            }


            if (battleS.GetComponent<BattleSceneManager>().BattleMenuScreen.gameObject.activeSelf == false) // to avoid movement while fighting
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
                            canMoveToPosition = true;
                        }
                        else
                        {
                            canMoveToPosition = false;
                        }
                    }
                    else if (movement.y == -1) // Down
                    {
                        SetDirection(Vector2.down, spriteAnimDown);
                        interactBox = downBox;
                        if (DownCollider.isObstacle == false)
                        {
                            canMoveToPosition = true;
                        }
                        else
                        {
                            canMoveToPosition = false;
                        }
                    }
                    else if (movement.x == -1) // Left
                    {
                        SetDirection(Vector2.left, spriteAnimLeft);
                        interactBox = leftBox;
                        if (LeftCollider.isObstacle == false)
                        {
                            canMoveToPosition = true;
                        }
                        else
                        {
                            canMoveToPosition = false;
                        }
                    }
                    else if (movement.x == 1) //Right
                    {
                        SetDirection(Vector2.right, spriteAnimRight);
                        interactBox = rightBox;
                        if (RightCollider.isObstacle == false)
                        {
                            canMoveToPosition = true;
                        }
                        else
                        {
                            canMoveToPosition = false;
                        }
                    }
                    else//No Movement
                    {
                        SetDirection(Vector2.zero, activeAnimation);
                    }

                    //Start Movement
                    if (canMoveToPosition == true)
                    {
                        if (movement != Vector2.zero)
                        {
                            var targetPos = transform.position; //Makes a vector with targer position
                            targetPos.x += movement.x; //Add either 1 or -1 to X which is the tile on top or down
                            targetPos.y += movement.y; //Same but with Y either left or right

                            if (canMove == true)
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
        if (canMove == true)
        {
            while ((tPos - transform.position).sqrMagnitude > Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, tPos, speed * Time.deltaTime);
                yield return null;
            }
        }

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
        canMove = false;
        fader.fadeInBattle();
        StartCoroutine(GoToBattle());
    }

    public void Flee() ////TEST
    {
        if(GlobalData.Instance.TournamentTime == false)
        {
            if (battleS.GetComponent<BattleSceneManager>().InBattleProgresion == false)
            {
               
                BattleEnds();
            }
        }
    }

    public void BattleEnds()
    {
        battleS.GetComponent<BattleSceneManager>().ReturnPokemon().transform.parent = pokemonInventory.transform;
        battleS.GetComponent<BattleSceneManager>().PokemonPlayerFleeSupport();
        battleS.GetComponent<BattleSceneManager>().ToogleBattleMenu();
        pokemonInventory.resetAllBuffs();
        canMove = true;

        string sceneName;
        sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName);
        switch (sceneName)
        {
            case "MainScene1":
                audioManager.CrossFadeTO(AudioManager.TrackID.inTown);
                break;
            case "Coliseo":
                audioManager.CrossFadeTO(AudioManager.TrackID.inColiseo);
                break;
            case "Island":
                audioManager.CrossFadeTO(AudioManager.TrackID.inIsland);
                break;
            case "Cave":
                audioManager.CrossFadeTO(AudioManager.TrackID.inCave);
                break;
            case "CaveToWoods":
                audioManager.CrossFadeTO(AudioManager.TrackID.inCave2);
                break;
            case "Woods":
                audioManager.CrossFadeTO(AudioManager.TrackID.inWoods);
                break;
        }

    }

    public void GoToCave()
    {
        canMove = false;
        StopAllCoroutines();
        isMoving = false;
        fader.fadeIn();
        fader.StartCoroutine(fader.GoToCaveCoro());
        ResetSquares();
    }
    public void GoToCaveToWoods()
    {
        canMove = false;
        StopAllCoroutines();
        isMoving = false;
        fader.fadeIn();
        fader.StartCoroutine(fader.GoToCaveWoodsCoro());
        ResetSquares();
    }
    public void GoToWoods()
    {
        canMove = false;
        StopAllCoroutines();
        isMoving = false;
        fader.fadeIn();
        fader.StartCoroutine(fader.GoToWoodsCoro());
        ResetSquares();
    }
    public void GoToTown()
    {
        canMove = false;
        StopAllCoroutines();
        isMoving = false;
        fader.fadeIn();
        fader.StartCoroutine(fader.GoToTownCoro());
        ResetSquares();
    }
    public void GoToColiseo()
    {
        canMove = false;
        StopAllCoroutines();
        isMoving = false;
        fader.fadeIn();
        fader.StartCoroutine(fader.GoToColiseoCoro());
        ResetSquares();
    }

    public void GoToTournament()
    {
        canMove = false;
        StopAllCoroutines();
        isMoving = false;
        fader.fadeIn();
        fader.StartCoroutine(fader.GoToColiseoCoro());
        ResetSquares();
    }

    public void GoToIsland()
    {
        canMove = false;
        StopAllCoroutines();
        isMoving = false;
        fader.fadeIn();
        fader.StartCoroutine(fader.GoToIslandCoro());
        ResetSquares();
    }

    public void Interact()
    {
        canMove = false;
        if(activeNpc.isProfesor == true)
        {
            pokemonInventory.HealAllPokemon();
            StartCoroutine(InteractStart());
        }
        else if (activeNpc.isShop == true)
        {
            string text;
            if(GlobalData.Instance.monney >= 200)
            {
                text = activeNpc.mainChat;
                ChatBoxManager.ChatBoxActivateShop(text);
            }
            else
            {
                StartCoroutine(InteractStart());
            }
            
        }
        else if(activeNpc.isPokeball == true)
        {
            string text = activeNpc.mainChat;
            GameObject pokemon = activeNpc.pokemon;
            ChatBoxManager.ChatBoxActivatePokeball(text);
        }
        else
        {
            StartCoroutine(InteractStart());
        }

    }

    public void ResetSquares()
    {
        interactBox.GetComponent<InteractSquare>().isObstacle = false;
        foreach (InteractSquare Square in l_Squares)
        {
            Square.isObstacle = false;
        }
    }

    public void ChooseStartingPokemon()
    {
        if (hasStartingPokemon == false)
        {
            pokemonInventory.AddCapturedPokemon(activeNpc.pokemon);
            hasStartingPokemon = true;
        }    
    }

    public void StartColliseumBattle()
    {
        StartCoroutine(E_StartColiseumBattleBattle());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CaveNtrance"))
        {
            GoToCave();
            positionChange = PositionChangeEnum.MAINTOCAVE;
        }

        if (other.CompareTag("CaveExit"))
        {
            GoToTown();
            positionChange = PositionChangeEnum.CAVETOMAIN;
        }

        if (other.CompareTag("CaveToWoods"))
        {
            GoToCaveToWoods();
            positionChange = PositionChangeEnum.MAINTOCAVE2;
        }
        if (other.CompareTag("WoodsEntrance"))
        {
            GoToWoods();
            positionChange = PositionChangeEnum.CAVE2TOFOREST;
        }
        if (other.CompareTag("WoodsExit"))
        {
            GoToCaveToWoods();
            positionChange = PositionChangeEnum.FORESTTOCAVE2;
        }

        if (other.CompareTag("ColliseoEntrance"))
        {
            GoToColiseo();
            positionChange = PositionChangeEnum.MAINTOCOLISEUM;
        }

        if (other.CompareTag("ColliseoExit"))
        {
            GoToTown();
            positionChange = PositionChangeEnum.COLISEUMTOMAIN;
        }

        if (other.CompareTag("Cave2Exit"))
        {
            GoToTown();
            positionChange = PositionChangeEnum.CAVE2TOMAIN;
        }

        if (other.CompareTag("IslandEntrance"))
        {
            GoToIsland();
            positionChange = PositionChangeEnum.MAINTOISLAND;
        }

        if (other.CompareTag("IslandExit"))
        {
            GoToTown();
            positionChange = PositionChangeEnum.ISLANDTOMAIN;
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
        battleS.GetComponent<BattleSceneManager>().ToogleBattleMenu();
        audioManager.CrossFadeTO(AudioManager.TrackID.inBattle);
        pokemonInventory.ChoosePokemon();
        selectedBush.Encounter();
        fader.fadeOut();
    }

    public IEnumerator GoToTrainerBattle()
    {
        InTrainerBattle = true;
        positionChange = PositionChangeEnum.TOURNAMENT;
        StoryProgression.Instance.StoryProgresion();
        GoToTournament();

        //Debug.Log("P1");
        
        yield return new WaitForSeconds(0.5f);


    }

    public IEnumerator E_StartColiseumBattleBattle()
    {
        fader.fadeInBattle();

        yield return new WaitForSeconds(0.4f);

        //Debug.Log("P3");
        battleS.GetComponent<BattleSceneManager>().ToogleBattleMenu();
        battleS.GetComponent<BattleSceneManager>().isTrainerBattle = true;
        audioManager.CrossFadeTO(AudioManager.TrackID.inBattle);
        pokemonInventory.ChoosePokemon();
        fader.fadeOut();
    }

    public IEnumerator InteractStart()
    {
        ChatBoxManager.ChatBoxActivate(activeNpc.mainChat);

        yield return new WaitForSeconds(2.0f);

        ChatBoxManager.ChatBoxDeActivate();
        canMove = true;
    }
}
