using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.InputSystem;

//inspired by Joss Lab Codes and my 2d Warhammer game project codes. this script control the player's move, change state, and Abilities
// taojin sha
//2024.10.18
public class PlayerController : MonoBehaviour
{
    [Header("Player move Settings")]
    private PlayerInput playerInput;
    private Vector3 moveInput;
    private Animator animator;
    private bool isFacingRight = true;
    public float moveSpeed = 5f;
    [Header("Player Encounter Settings")]
    public Vector2 EncounterStarPoint;
    public bool EndEncounter=false;
    public bool RestLoaction=false;
    public float SafeZonedistance=3f;
    private float randomRange = 0;
    public bool enterZone = false;
    public float RandomZoomLIMT = 1f;
    public float RandomZoomMax = 3f;
  
    [Header("Player Walk Sound Settings")]
    public  AudioManager audioManager;
    private bool zoneswitch = false;


    public event Action OnEncountered;

    private void Awake()
    {
        playerInput = new PlayerInput();
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component missing!");
        }

       

    }

    #region Player Moves in game
    private void OnEnable()
    {
        playerInput.Player.Move.performed += OnMovePerformed;
        playerInput.Player.Move.canceled += OnMoveCanceled;
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.Move.performed -= OnMovePerformed;
        playerInput.Player.Move.canceled -= OnMoveCanceled;
        playerInput.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        moveInput = new Vector3(input.x, input.y, 0);

        if (input.y > 0)
        {
            animator.SetBool("moveUP", true);
            animator.SetBool("moveDOWN", false);
        }
        else if (input.y < 0)
        {
            animator.SetBool("moveDOWN", true);
            animator.SetBool("moveUP", false);
        }

        if(input.x != 0) 
        {
            animator.SetBool("moveLeftRight", true);
        }

        if (input.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (input.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector3.zero;
        animator.SetBool("moveUP", false);
        animator.SetBool("moveDOWN", false);
        animator.SetBool("moveLeftRight", false);
    }

    private void MovePlayer()
    {
        Vector3 move = moveInput * Time.deltaTime * moveSpeed;
        transform.position += move;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; 
        transform.localScale = localScale;
      
    }
    #endregion

    public void NeedUpdate()
    {
        MovePlayer();
        ZoneState();
        WalkSound();
       
    }
    #region player trigger different zone (encounterd zone and foot sound zone)
    private void OnTriggerEnter2D(Collider2D other)
    {
        //check for Enter EncounterZone
        if (other.CompareTag("EncounterZone"))
        {
            Debug.Log("EnterEncounterZone");
            EncounterStarPoint= transform.position;
            Debug.Log("EncounterStarPoint" + EncounterStarPoint);
            randomRange = UnityEngine.Random.Range(RandomZoomLIMT, RandomZoomMax);
            Debug.Log("randomRange" + randomRange);
            enterZone = true;
        }
        //check for Enter another foot sound zone
        if (other.CompareTag("SoundZone"))
        {
            zoneswitch = true;
        }
        }
    private void OnTriggerExit2D(Collider2D other)
    {
        //check for Exit EncounterZone
        if (other.CompareTag("EncounterZone"))
        {
            Debug.Log("Player left the encounter zone");
            enterZone = false;  
        }
        //check for Exit another foot sound zone
        if (other.CompareTag("SoundZone"))
        {
            zoneswitch = false;
        }
    }

    private void ZoneState()
    {
        if (enterZone)
        {
            if (EndEncounter == false)
            {
                float distance = Vector3.Distance(EncounterStarPoint, transform.position);
                if (distance > randomRange )
                {
                  
                    OnEncountered();
                    Debug.Log("ZoneState触发了交战！");
                }
            }
            else if (EndEncounter == true)
            {
                if (RestLoaction == false)
                {
                    RestLoaction = true;
                    EncounterStarPoint = transform.position;
                    randomRange = UnityEngine.Random.Range(2f, 7f);
                    Debug.Log("随机范围值: " + randomRange);
                }
                float distance = Vector3.Distance(EncounterStarPoint, transform.position);

                if (distance > SafeZonedistance)
                {
                    Debug.Log("可以触发随机交战");
                    EndEncounter = false;
                    RestLoaction = false;
                }
            }
        }
    }

    private void WalkSound()
    {
        if(zoneswitch == false)
        {
            if (audioManager != null && moveInput != Vector3.zero && audioManager.playing==false)
            {
                audioManager.ManageSound("Grass", true);
            }
            else if (audioManager != null && moveInput == Vector3.zero)
            {
                audioManager.ManageSound("Grass", false);
            }
        }
        else if (zoneswitch == true)
        {
            if (audioManager != null && moveInput != Vector3.zero&& audioManager.playing==false)
            {
                audioManager.ManageSound("dir", true);
            }
            else if (audioManager != null && moveInput == Vector3.zero)
            {
                audioManager.ManageSound("dir", false);
            }
        }
    }
    #endregion
}
