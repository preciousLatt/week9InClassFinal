using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 10f;       // Forward movement speed
    public float lateralSpeed = 5f;        // Left/right movement speed

    [Header("Fuel Settings")]
    public float maxFuel = 100f;
    public float fuel = 100f;
    public float fuelConsumptionRate = 5f; // Fuel consumed per second while moving
    public float fuelPickupAmount = 25f;

    void Update()
    {
        HandleMovement();
    }

    private void FixedUpdate()
    {
        Debug.Log(fuel);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right
        Vector3 move = Vector3.right * horizontal * lateralSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W) && fuel > 0)
        {
            move += Vector3.forward * forwardSpeed * Time.deltaTime;
            ConsumeFuel();
        }

        transform.Translate(move, Space.World);
    }

    void ConsumeFuel()
    {
        fuel -= fuelConsumptionRate * Time.deltaTime;
        fuel = Mathf.Clamp(fuel, 0, maxFuel);
    }

    public void AddFuel()
    {
        fuel += fuelPickupAmount;
    }
}