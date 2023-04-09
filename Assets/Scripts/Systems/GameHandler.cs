using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using Kino;

///<summary>
/// The <c>GameHandler</c> class which handles the entire game
///</summary>
[System.Serializable]
public class GameHandler : MonoBehaviour
{
    ///<summary>
    /// The Instance of GameHandler (singleton)
    ///</summary>
    public static GameHandler Instance { get; private set; }

    public bool paused, cutscene, mouse, isInventory = false, HECHEATED = false, playerAlreadyDead = false;

    #region menu vars
    public AudioSource Music;
    public PauseMenu pMenu;
    public OptionsMenu oMenu;
    public DebugMenu dMenu;
    public GameObject deadMenu;
    #endregion
    
    #region player vars
    public InventorySlot[] gunSlot;
    public InventorySlot[] ammoSlot;
    public PlayerMovementAdvanced pm;
    public InventoryObject playerInventory;
    public PlayerHealth pHealth;
    public PlayerStuff pStuff;
    public GameObject dialogue;
    public CanvasGroup vHealth, vStamina;
    public PlayerInputController playerInput;
    [SerializeField] private GameObject ui_inventory;
    public AudioSource death;
    public GameObject cursor;
    #endregion

    #region input cooldown vars
    float cooldown = 0.5f;
    float lastPressTime = 0f;
    #endregion

    #region input vars
    public string curDevice;
    public float horizontalInput, verticalInput;
    public bool sprinting, crouching, jumping, g_left, g_right, equipping, firing, reloading, pausing, interacting;
    InputAction.CallbackContext move, look, sprint, crouch, jump, g_left_ctx, g_right_ctx, equip, fire, reload, pause, interact, inventory;
    #endregion

    #region camera vars
    public Camera cam;
    public float mouseX, mouseY;
    AnalogGlitch aGlitch;
    DigitalGlitch dGlitch;
    float intensity, scanLineJitter, verticalJump, horizontalShake, colorDrift;
    bool isGlitch;
    #endregion

    void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;

            DontDestroyOnLoad(Instance);
        }

        if(playerInput == null)
            playerInput = new PlayerInputController();
    }

    void Start()
    {
        aGlitch = cam.gameObject.GetComponent<AnalogGlitch>();
        dGlitch = cam.gameObject.GetComponent<DigitalGlitch>();
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if(GetComponent<PlayerInput>() != null)
        {
            curDevice = GetComponent<PlayerInput>().currentControlScheme.ToLower();
        }

        #region input checkers (mostly bools but sometimes float)
        if (!paused && !cutscene)
        {
            if(horizontalInput != move.ReadValue<Vector2>().x) { horizontalInput = move.ReadValue<Vector2>().x; }
            if (verticalInput != move.ReadValue<Vector2>().y) { verticalInput = move.ReadValue<Vector2>().y; }
            if (mouseX != look.ReadValue<Vector2>().x) { mouseX = look.ReadValue<Vector2>().x; }
            if (mouseY != look.ReadValue<Vector2>().y) { mouseY = look.ReadValue<Vector2>().y; }
            if (sprinting != sprint.ReadValue<float>() > 0.1f) { sprinting = sprint.ReadValue<float>() > 0.1f; }
            if (crouching != crouch.ReadValue<float>() > 0.1f) { crouching = crouch.ReadValue<float>() > 0.1f; }
            if (jumping != jump.ReadValue<float>() > 0.1f) { jumping = jump.ReadValue<float>() > 0.1f; }
            if (g_left != g_left_ctx.ReadValue<float>() > 0.1f) { g_left = g_left_ctx.ReadValue<float>() > 0.1f; }
            if (g_right != g_right_ctx.ReadValue<float>() > 0.1f) { g_right = g_right_ctx.ReadValue<float>() > 0.1f; }
            if (equipping != equip.ReadValue<float>() > 0.1f) { equipping = equip.ReadValue<float>() > 0.1f; }
            if (firing != fire.ReadValue<float>() > 0.1f) { firing = fire.ReadValue<float>() > 0.1f; }
            if (reloading != reload.ReadValue<float>() > 0.1f) { reloading = reload.ReadValue<float>() > 0.1f; }
            pausing = pause.ReadValue<float>() > 0.7f;
            if (interacting != interact.ReadValue<float>() > 0.1f) { interacting = interact.ReadValue<float>() > 0.1f; }
        }
        else if(paused || cutscene)
        {
            if(horizontalInput != 0) { horizontalInput = 0; }
            if(verticalInput != 0) { verticalInput = 0; }
            if(mouseX != 0) { mouseX = 0; }
            if(mouseY != 0) { mouseY = 0; }
            
            if(sprinting != false && crouching != false && jumping != false && g_left != false && g_right != false && equipping != false && firing != false && reloading != false && pausing != false && interacting != false)
            {
                sprinting = false;
                crouching = false;
                jumping = false;
                g_left = false;
                g_right = false;
                equipping = false;
                firing = false;
                reloading = false;
                //pausing = false;
                interacting = false;
            }
            
        }
        #endregion

        #region null checker
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;

            DontDestroyOnLoad(this);
            DontDestroyOnLoad(Instance);
        }

        if (Music == null)
        {
            if (GameObject.Find("Music"))
            {
                Music = GameObject.Find("Music").GetComponent<AudioSource>();
                if (Music != null)
                {
                    Music.Play();
                    Music.loop = true;
                }
            }
        }

        if (deadMenu == null) { deadMenu = GameObject.Find("DeathScreen"); }
        if (cam == null) { cam = GameObject.Find("PlayerCam").GetComponent<Camera>(); aGlitch = cam.gameObject.GetComponent<AnalogGlitch>(); dGlitch = cam.gameObject.GetComponent<DigitalGlitch>(); }

        if(vHealth == null) { vHealth = GameObject.Find("HealthVignette").GetComponent<CanvasGroup>(); }
        if (vStamina == null) { vStamina = GameObject.Find("StaminaVignette").GetComponent<CanvasGroup>(); }

        if (pm == null) { pm = GameObject.Find("Player").GetComponent<PlayerMovementAdvanced>(); }
        if(pHealth == null) { pHealth = GameObject.Find("Player").GetComponent<PlayerHealth>(); }
        if (pStuff == null) { pStuff = pm.gameObject.GetComponent<PlayerStuff>(); }
        if(dialogue == null) { dialogue = GameObject.Find("Dialogue"); }
        if(death == null) 
        {
            AudioSource[] sources = (AudioSource[])FindObjectsOfType(typeof(AudioSource));
            foreach(AudioSource source in sources)
            {
                if(source.clip.name == "death")
                {
                    //print("found death source!");
                    death = source;
                }
            }
            
        }
        #endregion

        #region glitch
        if (dGlitch != null)
        {
            if (dGlitch.intensity != intensity) { dGlitch.intensity = intensity; }
        }
        if (aGlitch != null)
        {
            if (aGlitch.scanLineJitter != scanLineJitter) { aGlitch.scanLineJitter = scanLineJitter; }
            if (aGlitch.verticalJump != verticalJump) { aGlitch.verticalJump = verticalJump; }
            if (aGlitch.horizontalShake != horizontalShake) { aGlitch.horizontalShake = horizontalShake; }
            if (aGlitch.colorDrift != colorDrift) { aGlitch.colorDrift = colorDrift; }
        }
        #endregion

        #region pausing
        if (pMenu.isPaused || oMenu.isOptions || playerAlreadyDead || dialogue.activeSelf)
        {
            paused = true;
            
            if(!isGlitch)
            {
                if(playerAlreadyDead)
                {
                    SetGlitch(1f, 0.4f, 0.2f, 0.2f, 0.4f);
                    Time.timeScale = 1f;
                }
                else
                {
                    SetGlitch(0.084f, 0.4f, 0.2f, 0.2f, 0.4f);
                    Time.timeScale = 0f;
                }
                
                isGlitch = true;
            }

            if(pMenu.isPaused) 
            {
                pMenu.pMenu.SetActive(true);
                oMenu.gO.SetActive(false);
                deadMenu.SetActive(false);
            } 
            else if(oMenu.isOptions) 
            {
                pMenu.pMenu.SetActive(false);
                oMenu.gO.SetActive(true);
                deadMenu.SetActive(false);
            }
            else if(playerAlreadyDead)
            {
                pMenu.pMenu.SetActive(false);
                oMenu.gO.SetActive(false);
                deadMenu.SetActive(true);
            }
            else
            {
                pMenu.pMenu.SetActive(false);
                oMenu.gO.SetActive(false);
                deadMenu.SetActive(false);
            }

            dMenu.gO.SetActive(false);
            isInventory = false;
            ui_inventory.SetActive(false);
        }
        else
        {
            if (isGlitch)
            {
                SetGlitch(0, 0, 0, 0, 0);
                isGlitch = false;
                Time.timeScale = 1f;
            }
            paused = false;
        }
        #endregion

        #region mouseLock (debug)
        if (mouse || paused)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            //Time.timeScale = 0f;
        } 
        else if(!mouse || !paused)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            //Time.timeScale = 1f;
        }
        else if(isInventory && !paused && !mouse)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.M) && !paused)
        {
            mouse = !mouse;
        }
        
        if(Input.GetKeyDown(KeyCode.Escape) && mouse)
        {
            mouse = false;
        }
        #endregion

        #region inventory
        if (isInventory)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKeyDown(KeyCode.I) && !paused)
        {
            float currentTime = Time.time;

            float diffSecs = currentTime - lastPressTime;
            if (diffSecs >= cooldown)
            {
                lastPressTime = currentTime;
                isInventory = !isInventory;
            }
            
        }

        ui_inventory.SetActive(isInventory);
        #endregion

        if(GameObject.Find("cheaterSign") != null)
        {
            if(HECHEATED) 
            {
                GameObject.Find("cheaterSign").GetComponent<CanvasGroup>().alpha = 1f;  
            }
            else
            {
                GameObject.Find("cheaterSign").GetComponent<CanvasGroup>().alpha = 0f;  
            }   
        }
    }

    public void Die()
    {
        if(!playerAlreadyDead)
        {
            playerAlreadyDead = true;
            if(death != null)
            {
                death.Play();
                deadMenu.SetActive(true);
            }
        }
    }

    public void ResetDie()
    {
        death.Stop();
        playerAlreadyDead = false;
        deadMenu.SetActive(false);
        SetGlitch(0,0,0,0,0);
    }

    public void SetGlitch(float _intensity, float _scanLineJitter, float _verticalJump, float _horizontalShake, float _colorDrift)
    {
        intensity = _intensity;
        scanLineJitter = _scanLineJitter;
        verticalJump = _verticalJump;
        horizontalShake = _horizontalShake;
        colorDrift = _colorDrift;
    }

    #region InputAction.CallbackContext hell
    public void Interact(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        interact = ctx;
    }
    public void Move(InputAction.CallbackContext ctx)
    {
        if(!ctx.performed) { return; }
        move = ctx;
    }

    public void Look(InputAction.CallbackContext ctx)
    {
        if(!ctx.performed) { return; }
        look = ctx;
    }

    public void Sprint(InputAction.CallbackContext ctx)
    {
        if(!ctx.performed) {return;}
        sprint = ctx;
    }

    public void Crouch(InputAction.CallbackContext ctx)
    {
        if(!ctx.performed) {return;}
        crouch = ctx;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if(!ctx.performed) { return; }
        jump = ctx;
    }

    public void G_Left(InputAction.CallbackContext ctx)
    {
        if(!ctx.performed) { return; }
        g_left_ctx = ctx;
    }

    public void G_Right(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        g_right_ctx = ctx;
    }

    public void Equip(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        equip = ctx;
    }

    public void Reload(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        reload = ctx;
    }
    public void Fire(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        fire = ctx;
    }

    public void Pause(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) { return; }
        pause = ctx;
    }

    public void Inventory(InputAction.CallbackContext ctx)
    {
        if(!ctx.performed) { return; }
        inventory = ctx;
    }
    #endregion

    public static void ClampToWindow(Vector2 MyMouse, RectTransform panelRectTransform, RectTransform parentRectTransform)
    {
        panelRectTransform.transform.position = MyMouse;

        Vector2 pos = panelRectTransform.localPosition;

        Vector2 minPosition = parentRectTransform.rect.min - panelRectTransform.rect.min;
        Vector2 maxPosition = parentRectTransform.rect.max - panelRectTransform.rect.max;

        pos.x = Mathf.Clamp(panelRectTransform.localPosition.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(panelRectTransform.localPosition.y, minPosition.y, maxPosition.y);

        panelRectTransform.localPosition = pos;
    }

    public static int GetActiveScene()
    {
        return SceneManager.GetActiveScene().buildIndex - 2;
    }

    public static string NameFromIndex(int BuildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }
}

///<summary>
///Room class (do i even need this?)
///</summary>
[System.Serializable]
public class Room 
{
    public string ID;
    public int checkpoint;
    public int scene;

    public Room(string _ID, int _checkpoint, int _scene)
    {
        ID = _ID;
        checkpoint = _checkpoint;
        scene = _scene;
    }
}