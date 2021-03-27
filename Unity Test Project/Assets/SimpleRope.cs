using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRope : MonoBehaviour
{
    public GameObject m_LinkPrefab = null;
    public float m_OverlapPercentage = 0.2f;
    public GameObject m_DestinationObject = null;
    public int m_HingeID = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Debug
        Debug.Assert(GetComponent<Rigidbody2D>() != null);
        Debug.Assert(m_LinkPrefab != null);
        Debug.Assert(m_LinkPrefab.GetComponent<Rigidbody2D>() != null);
        Debug.Assert(m_LinkPrefab.GetComponent<HingeJoint2D>() != null);
        Debug.Assert(m_DestinationObject.GetComponent<HingeJoint2D>() != null);

        //How long our sprite chain length is
        var offSetForJoinPlacement = m_LinkPrefab.GetComponent<SpriteRenderer>().size.y / 2;
        offSetForJoinPlacement -= offSetForJoinPlacement * m_OverlapPercentage;

        //Initial Position
        var SpriteRecomputedHeight = m_LinkPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        SpriteRecomputedHeight -= (SpriteRecomputedHeight / 2.0f) * m_OverlapPercentage;

        //Determine how many links need to add
        var DestinationHingeJoints = m_DestinationObject.GetComponents<HingeJoint2D>();
        var V = m_DestinationObject.transform.TransformPoint(
            new Vector3(DestinationHingeJoints[m_HingeID].anchor.x
            , DestinationHingeJoints[m_HingeID].anchor.y
            , 0)) - this.transform.position;


        var nLinks = (int)(V.magnitude / SpriteRecomputedHeight + 0.5f) - 1;

        //Create Links
        var PrevRB = GetComponent<Rigidbody2D>();
        for (int i = 0; i < nLinks; i++)
        {
            var chainObj = Instantiate(m_LinkPrefab
                , this.transform.position
                + new Vector3(0, -SpriteRecomputedHeight * i, 0)
                , this.transform.rotation);

            var Hinge = chainObj.GetComponent<HingeJoint2D>();
            var RB = chainObj.GetComponent<Rigidbody2D>();

            Hinge.autoConfigureConnectedAnchor = false;
            Hinge.connectedBody = PrevRB;
            Hinge.anchor = new Vector2(0, offSetForJoinPlacement);

            if (i != 0) Hinge.connectedAnchor = new Vector2(0, -offSetForJoinPlacement);

            PrevRB = RB;
        }

        //Last Link hook it up to the destination object
        DestinationHingeJoints[m_HingeID].connectedBody = PrevRB;
    }

    private void OnDrawGizmos()
    {
        if (m_DestinationObject == null) return;

        var DestinationHingeJoints = m_DestinationObject.GetComponents<HingeJoint2D>();
        if (DestinationHingeJoints == null) return;
        if (m_HingeID >= DestinationHingeJoints.Length) return;

        var v = m_DestinationObject.transform.TransformPoint(new Vector3(DestinationHingeJoints[m_HingeID].anchor.x
            , DestinationHingeJoints[m_HingeID].anchor.y
            , 0));

        //Sphere for the ground collision
        Gizmos.DrawSphere(v, 0.1f);
    }
}
