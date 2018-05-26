using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// This class is a simple example of how to build a controller that interacts with PlatformerMotor2D.
/// </summary>
[RequireComponent(typeof(PlatformerMotor2D))]
public class PlayerController2D : MonoBehaviour
{
    public Vector2 playerInitPos_Level1;
    public Vector2 playerInitPos_Level2;

    private static PlayerController2D _instance;
    
    private PlatformerMotor2D _motor;
    private bool _restored = true;
    private bool _enableOneWayPlatforms;
    private bool _oneWayPlatformsAreWalls;
    private bool _enableMove;
    private Animator m_Animator;
    private Damager meleeAttack;

    public static PlayerController2D GetInstance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<PlayerController2D>();
            }
            return _instance;
        }
    }

    public bool IsFacingRight {
        get { return transform.localScale.x > 0; }
    }

    public void EnableMovement() {
        _enableMove = true;
    }

    public void DisableMovement() {
        _enableMove = false;
    }

    public void ResetPosToOrigin() {
        transform.localPosition = Vector3.one;
    }

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void Start()
    {
        _motor = GetComponent<PlatformerMotor2D>();
        m_Animator = GetComponent<Animator>();
        meleeAttack = GetComponentInChildren<Damager>();

        EnableMovement();
        meleeAttack.gameObject.SetActive(false);
    }
    
    void FreedomStateSave(PlatformerMotor2D motor)
    {
        if (!_restored) // do not enter twice
            return;

        _restored = false;
        _enableOneWayPlatforms = _motor.enableOneWayPlatforms;
        _oneWayPlatformsAreWalls = _motor.oneWayPlatformsAreWalls;
    }
    // after leave freedom state for ladders
    void FreedomStateRestore(PlatformerMotor2D motor)
    {
        if (_restored) // do not enter twice
            return;

        _restored = true;
        _motor.enableOneWayPlatforms = _enableOneWayPlatforms;
        _motor.oneWayPlatformsAreWalls = _oneWayPlatformsAreWalls;
    }
    
    void Update()
    {
        if (!_enableMove) {
            if (!_motor.frozen)
                _motor.frozen = true;
            return;
        } else {
            if (_motor.frozen)
                _motor.frozen = false;
        }

        // use last state to restore some ladder specific values
        if (_motor.motorState != PlatformerMotor2D.MotorState.FreedomState)
        {
            // try to restore, sometimes states are a bit messy because change too much in one frame
            FreedomStateRestore(_motor);
        }

        // Jump?
        // If you want to jump in ladders, leave it here, otherwise move it down
        if (Input.GetButtonDown(PC2D.Input.JUMP))
        {
            _motor.Jump();
            _motor.DisableRestrictedArea();

            if (_motor.IsOnLadder())
                _motor.LadderAreaExit();
        }

        _motor.jumpingHeld = Input.GetButton(PC2D.Input.JUMP);

        // XY freedom movement
        if (_motor.motorState == PlatformerMotor2D.MotorState.FreedomState)
        {
            _motor.normalizedXMovement = Input.GetAxis(PC2D.Input.HORIZONTAL);
            _motor.normalizedYMovement = Input.GetAxis(PC2D.Input.VERTICAL);

            return; // do nothing more
        }

        // X axis movement
        if (Mathf.Abs(Input.GetAxis(PC2D.Input.HORIZONTAL)) > PC2D.Globals.INPUT_THRESHOLD)
        {
            _motor.normalizedXMovement = Input.GetAxis(PC2D.Input.HORIZONTAL);
        }
        else
        {
            _motor.normalizedXMovement = 0;
        }

        if (Input.GetAxis(PC2D.Input.VERTICAL) != 0)
        {
            bool up_pressed = Input.GetAxis(PC2D.Input.VERTICAL) > 0;
            if (_motor.IsOnLadder())
            {
                if (
                    (up_pressed && _motor.ladderZone == PlatformerMotor2D.LadderZone.Top)
                    ||
                    (!up_pressed && _motor.ladderZone == PlatformerMotor2D.LadderZone.Bottom)
                 )
                {
                    // do nothing!
                }
                // if player hit up, while on the top do not enter in freeMode or a nasty short jump occurs
                else
                {
                    // example ladder behaviour

                    _motor.FreedomStateEnter(); // enter freedomState to disable gravity
                    _motor.EnableRestrictedArea();  // movements is retricted to a specific sprite bounds

                    // now disable OWP completely in a "trasactional way"
                    FreedomStateSave(_motor);
                    _motor.enableOneWayPlatforms = false;
                    _motor.oneWayPlatformsAreWalls = false;

                    // start XY movement
                    _motor.normalizedXMovement = Input.GetAxis(PC2D.Input.HORIZONTAL);
                    _motor.normalizedYMovement = Input.GetAxis(PC2D.Input.VERTICAL);
                }
            }
        }
        else if (Input.GetAxis(PC2D.Input.VERTICAL) < -PC2D.Globals.FAST_FALL_THRESHOLD)
        {
            _motor.fallFast = false;
        }
        
        if(Input.GetKeyDown(PC2D.Input.START_SLIDE))
        {
            _motor.Dash();
            //m_Animator.SetTrigger("slide");
            m_Animator.Play("PlayerSlide");
        }

        if (Input.GetKeyDown(PC2D.Input.WAVE_SWORD)) {
            _motor.Wave();
            if (_motor.motorState == PlatformerMotor2D.MotorState.JumpWave) {
                m_Animator.SetTrigger("jumpWave");
                //Invoke("EnableMeleeAttack", 0.2f);
                meleeAttack.gameObject.SetActive(true);
            } else {
                m_Animator.SetTrigger("wave");
                meleeAttack.gameObject.SetActive(true);
            }
        }

        if (Input.GetKeyUp(PC2D.Input.WAVE_SWORD)) {
            meleeAttack.gameObject.SetActive(false);
            //meleeAttack.DisableDamage();
        }

        if (Input.GetKeyDown(PC2D.Input.THROW_DAGGER)) {
            _motor.Throw();
            if (_motor.motorState == PlatformerMotor2D.MotorState.JumpThrow) {
                m_Animator.SetTrigger("jumpThrow");
            } else {
                m_Animator.SetTrigger("throw");
            }
        }
    }

    private void EnableMeleeAttack() {
        //meleeAttack.EnableDamage();
        meleeAttack.gameObject.SetActive(true);
    }

    private void OnSceneChanged(Scene from, Scene to) {
        GameController.GetInstance.ResetUI();
        Debug.Log(string.Format("Scene changed from {0} to {1}", from.name, to.name));
        if (to.name == "_Start") {
            //  disable player movement
            DisableMovement();
            transform.GetComponent<PlatformerMotor2D>().frozen = true;
        } else {
            if (to.name == "level1") {
                Debug.Log("to.name == level1, playerInitPos is: " + playerInitPos_Level1);

                transform.localPosition = Vector3.zero;
                transform.parent.localPosition = this.playerInitPos_Level1;
            } else if (to.name == "level2") {
                transform.localPosition = Vector3.zero;
                transform.parent.localPosition = this.playerInitPos_Level2;
            }
            //EnableMovement();
            //transform.GetComponent<PlatformerMotor2D>().frozen = false;
        }
    }
}
