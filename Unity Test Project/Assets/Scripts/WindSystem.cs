using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSystem : MonoBehaviour
{
    private Vector2 WindVelocity = new Vector2(-50, 0);
    private float m_Time;

    float TurbulenceValue(Vector2 P)
    {
        return Mathf.Cos(P.y * P.x * m_Time * 200.0f) * 0.5f + 0.5f;
    }

    void UpdateWind(Rigidbody2D RB, Collider2D collider)
    {
        var Extents = collider.bounds.extents;
        var Area = Extents.x * Extents.y;

        var BaseInAirVelocity = WindVelocity * TurbulenceValue(RB.position) - RB.velocity.normalized * RB.velocity.sqrMagnitude;

        Area *= 0.5f;

        const float TotalDrag = 1.5f * 100;

        var WindForce = BaseInAirVelocity * (Area * TotalDrag) * Time.fixedDeltaTime;
        RB.AddForceAtPosition(WindForce, collider.bounds.center);
    }

    void UpdateWind(Rigidbody2D RB)
    {
        var Colliders = RB.GetComponents<Collider2D>();
        if (Colliders == null) return;
        foreach(var e in Colliders)
        {
            UpdateWind(RB, e);
        }
    }

    void UpdateWithFindType()
    {
        var foundPhysicsObjects = FindObjectsOfType<Rigidbody2D>();
        foreach(var RB in foundPhysicsObjects)
        {
            UpdateWind(RB);
        }
    }
    private void FixedUpdate()
    {
        m_Time += Time.fixedDeltaTime;

        UpdateWithFindType();
    }
}
