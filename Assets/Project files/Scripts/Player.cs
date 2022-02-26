using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class Player : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction painaAction;
    public GameObject circle;
    Vector2 mousePosition;
    bool onkoPainettu;
    bool isActive;
    public Camera cam;
    public int points = 0;
    public GameObject dreamObject;
    float timer = 2;
    public float dreamPosXmax = 2.7f;
    public float dreamPosYmax = 6f;
    public Transform dreamTarget;
    public TMP_Text scoreText;

    void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Paina.performed += HandlePaina;
        playerInput.Player.Paina.Enable();
        playerInput.Player.HiirenLiike.performed += HandleHiirenLiike;
        playerInput.Player.HiirenLiike.Enable();
        playerInput.Player.PickDream.performed += HandlePickDream;
        playerInput.Player.PickDream.Enable();
      //  playerInput.Player.MouseOver.performed += HandleOikeaKlik;
      //  playerInput.Player.MouseOver.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText(points);
    }

    // Update is called once per frame
    void Update()
    {
        if(onkoPainettu == false)
        {
            HandleSquareMovement();
            
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            float dreamPosX = Random.Range(-dreamPosXmax, dreamPosXmax);
            Vector2 dreamPos = new Vector2(dreamPosX, dreamPosYmax);
            GameObject dreamObj = Instantiate(dreamObject, dreamPos, Quaternion.identity);
            dreamObj.GetComponent<Dream>().Init(dreamTarget, this);
            timer = 2;
        }     


    }

    void HandlePaina(InputAction.CallbackContext context)
    {
        
    }

    void HandleHiirenLiike(InputAction.CallbackContext context)
    {
        Vector2 mPos = context.ReadValue<Vector2>();
        mousePosition = cam.ScreenToWorldPoint(mPos);
    }

    void HandleSquareMovement()
    {
        circle.transform.position = mousePosition;
    }

    void HandlePickDream(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward);
        if (!hit.transform)
        {
            return;
        }
        if (!hit.transform.GetComponent<Dream>())
        {
            return;
        }
        Dream dream = hit.transform.GetComponent<Dream>();
        switch (dream.GetDreamType())
        {
            case DreamType.BAD:
                break;
            case DreamType.GOOD:
                points--;
                break;
        }
        Destroy(dream.gameObject);
        UpdateScoreText(points);
    }

    public void UpdateScoreText(int value)
    {
        scoreText.text = "Good dreams" + "\n" + value.ToString();
    }
      
    /* void HandleVasenKlikkausStop(InputAction.CallbackContext context)
    {
        print("Reetta");
        onkoPainettu = !onkoPainettu;
        // playerInput.Player.HiirenLiike.Disable();
        if (isActive == false)
        {
            ChangeColor(Color.white);
            isActive = true;
        }
    }void HandleOikeaKlik(InputAction.CallbackContext context)
    {  

        if (onkoPainettu == false && isActive == true)
        {
            print("jes");
            ChangeColor(hoverColor);
            isActive = false;
        } 
        else if(onkoPainettu ==false && isActive == false)
            {
            ChangeColor(Color.white);
            isActive = true; 
        }
    }

    void ChangeColor(Color color)
    {
        SpriteRenderer spriteRenderer = square.GetComponent<SpriteRenderer>();
        spriteRenderer.color = color;
        
    } */
}
