using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class Player : MonoBehaviour
{
    PlayerInput playerInput;
    public GameObject circle;
    Vector2 mousePosition;
    public Camera cam;
   
    void Awake()
    {
        playerInput = new PlayerInput();
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
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isPlaying == false)
        {
            return;
        }
        HandleSquareMovement();
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
                GameManager.instance.UpdateCurrentDreamValue(-GameManager.instance.dreamGain);
                break;
        }
        GameManager.instance.spawnedDreams.Remove(dream.gameObject);
        Destroy(dream.gameObject);
        dream.SpawnPoofParticles();
    }
    
}
