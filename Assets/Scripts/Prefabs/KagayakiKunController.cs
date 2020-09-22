using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KagayakiKunController : MonoBehaviour
{
    const float LIMIT_Y = -15f;

    float INITIAL_SPEED;
    float MIN_SPEED;
    float REDUCE_SPEED;
    float INITIAL_ROTATION_SPEED;
    float INITIAL_SCALE_SPEED;
    float SPEED_MAX_RATE;
    float SCALE_MAX_RATE;

    float deltaTime;
    float moveSpeed;
    float rotationSpeed;
    float scaleSpeedX;
    float scaleSpeedY;

    Vector3 initialScale;

    // Start is called before the first frame update
    void Start()
    {
        SetStatus();

        moveSpeed = INITIAL_SPEED;
        rotationSpeed = GetRotationSpeed();
        scaleSpeedX = scaleSpeedY = GetScaleSpeed();

        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Data.IsGamePlay())
        {
            return;
        }

        CheckDestroy();

        deltaTime = Time.deltaTime;

        ReduceSpeed();
        Move();
    }

    void CheckDestroy()
    {
        if (!IsDestroy())
        {
            return;
        }

        Destroy(this.gameObject);
    }

    bool IsDestroy()
    {
        return transform.localPosition.y < LIMIT_Y;
    }

    void Move()
    {
        MoveY();
        Rotation();
        ChangeScale();
    }

    void MoveY()
    {
        Vector2 newPosition = transform.localPosition;

        newPosition.y -= moveSpeed * deltaTime;

        transform.localPosition = newPosition;
    }

    void ReduceSpeed()
    {
        moveSpeed -= REDUCE_SPEED;

        if (moveSpeed < MIN_SPEED)
        {
            moveSpeed = MIN_SPEED;
        }
    }

    void Rotation()
    {
        Vector3 currentRotation = transform.localEulerAngles;

        float newZ = currentRotation.z + rotationSpeed * deltaTime;

        transform.rotation = Quaternion.Euler(0f, 0f, newZ);
    }

    float GetRotationSpeed()
    {
        float min = INITIAL_ROTATION_SPEED;
        float max = INITIAL_ROTATION_SPEED * SPEED_MAX_RATE;

        float speed = Random.Range(min, max);

        if (Random.Range(0, 2) == 0)
        {
            speed *= -1f;
        }

        return speed;
    }

    void ChangeScale()
    {
        Vector3 scale = transform.localScale;

        scale.x += scaleSpeedX * deltaTime;
        if (IsReverseScale(scale.x, initialScale.x))
        {
            scaleSpeedX *= -1f;
        }

        scale.y += scaleSpeedY * deltaTime;
        if (IsReverseScale(scale.y, initialScale.y))
        {
            scaleSpeedY *= -1f;
        }

        transform.localScale = scale;
    }

    bool IsReverseScale(float currentSacle, float initScale)
    {
        if (currentSacle > initScale * SCALE_MAX_RATE)
        {
            return true;
        }

        if (currentSacle < initScale / SCALE_MAX_RATE)
        {
            return true;
        }

        return false;
    }

    float GetScaleSpeed()
    {
        float min = INITIAL_SCALE_SPEED;
        float max = INITIAL_SCALE_SPEED * SPEED_MAX_RATE;

        return Random.Range(min, max);
    }

    void SetStatus()
    {
        switch (Data.GAME_MODE)
        {
            case Data.GAME_MODE_B:
                SetStatusModeB();
                break;
            default:
                SetStatusModeA();
                break;
        }
    }

    void SetStatusModeA()
    {
        INITIAL_SPEED = 2f;
        MIN_SPEED = 0.8f;
        REDUCE_SPEED = 0.002f;
        INITIAL_ROTATION_SPEED = 5f;
        INITIAL_SCALE_SPEED = 0.2f;
        SPEED_MAX_RATE = 1.3f;
        SCALE_MAX_RATE = 1.1f;
    }

    void SetStatusModeB()
    {
        INITIAL_SPEED = 2f;
        MIN_SPEED = 0.8f;
        REDUCE_SPEED = 0.002f;
        INITIAL_ROTATION_SPEED = 5f;
        INITIAL_SCALE_SPEED = 1.2f;
        SPEED_MAX_RATE = 1.3f;
        SCALE_MAX_RATE = 1.1f;
    }
}
