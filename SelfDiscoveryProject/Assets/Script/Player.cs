using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;
    private enum Status { Idle, Charging, Discharging };
    private Status energyState;

    public float gravity;
    public float speed;
    private float appliedSpeed;

    public float nextJumpDelay;
    private float currentNextJumpDelay;
    public float jumpHeight;
    public float lowEnergyJumpModifier;
    private bool isGrounded;
    public Transform feetPosition;
    public float checkRadius;
    public LayerMask groundType;

    public float maxEnergy;
    public float currentEnergy;
    public float energyDecreasePerSecond;
    public float energyIncreasePerSecond;
    public Light playerLight;
    public float playerLightMinRange = 3;
    public float playerLightMaxRange = 6;

    Vector3 velocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        appliedSpeed = speed;
        currentEnergy = maxEnergy;
        float energyPercentage = (currentEnergy * maxEnergy) / 100;
        float playerLightRangeDifference = playerLightMaxRange - playerLightMinRange;
        playerLight.range = playerLightMinRange + ((playerLightRangeDifference * energyPercentage) / 100);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Glide();
        EnergManager();
    }

    private void EnergManager()
    {
        //Se sto volando oppure sono vicino ad oggetti che mi caricano/scaricano
        //cambio valore energia
        //altrimenti
        //valore energia rimane immutato

        if (!isGrounded)
        {
            energyState = Status.Discharging;
        }
        else
        {
            energyState = Status.Idle;
        }

        switch (energyState)
        {
            case Status.Discharging:
                currentEnergy += energyDecreasePerSecond*Time.deltaTime;
                break;
            case Status.Charging:
                currentEnergy += energyIncreasePerSecond*Time.deltaTime;
                break;
        }
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        if (currentEnergy < 0)
        {
            currentEnergy = 0;
        }
        float energyPercentage = (currentEnergy * maxEnergy)/100;
        float playerLightRangeDifference = playerLightMaxRange - playerLightMinRange;
        float newPlayerLightRange = playerLightMinRange + ((playerLightRangeDifference * energyPercentage) / 100);
        playerLight.range = Mathf.Lerp(playerLight.range, newPlayerLightRange, Time.deltaTime);
    }
    private void Glide()
    {
        if (currentEnergy > 1)
        {
            Jump(1);
        }
        if (currentEnergy <= 1)
        {
            Jump(lowEnergyJumpModifier);
        }
    }

    private void Jump(float jumpMoodifier)
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Time.time > currentNextJumpDelay)
            {
                velocity.y = Mathf.Sqrt(jumpHeight/jumpMoodifier * -2 * gravity);
                currentNextJumpDelay = Time.time + nextJumpDelay*jumpMoodifier;
            }
        }
    }
    private void Movement()
    {
        isGrounded = Physics.CheckSphere(feetPosition.position, checkRadius, groundType);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1;
        }
        float x = Input.GetAxis("Horizontal");

        Vector3 move = transform.right * x;

        controller.Move(move * appliedSpeed * Time.deltaTime);

        velocity.y += gravity + Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(feetPosition.transform.position, checkRadius);
        GUI.color = Color.white;
        var styleGui = new GUIStyle(GUI.skin.label);
        styleGui.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect(0, 0, 150, 100), "" + currentEnergy, styleGui);
        GUI.Label(new Rect(30, 0, 250, 100), "" + energyState, styleGui);
    }
}
