using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class NeralaPassive : MonoBehaviour
{
    private NavMeshAgent playerAgent;
    private CharacterBaseBehavior baseScript;

    private bool isUp;

    public LayerMask whatIsPassive;

    void Start()
    {
        playerAgent = GetComponent<NavMeshAgent>();
        baseScript = GetComponent<CharacterBaseBehavior>();

        isUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(baseScript.selectedCharacter)
        {

            Collider[] passiveSpot = Physics.OverlapSphere(transform.position, 5.0f, whatIsPassive);

            for (int i = 0; i < passiveSpot.Length; i++)
            {
                if (passiveSpot[i].gameObject.tag == "NeralaPassive")
                {
                    float toleranceRange = 1f;
                    Transform upPoint = passiveSpot[i].gameObject.transform.Find("upPoint");
                    Transform downPoint = passiveSpot[i].gameObject.transform.Find("downPoint");

                    if (transform.position.y - toleranceRange < upPoint.position.y && transform.position.y + toleranceRange < downPoint.position.y)
                        isUp = false;
                    else if (transform.position.y - toleranceRange > upPoint.position.y && transform.position.y + toleranceRange > downPoint.position.y)
                        isUp = true;

                    if (Input.GetKeyDown(KeyCode.F))
                    {

                        if (isUp)
                        {
                            baseScript.state = PlayerState.IDLE;
                            playerAgent.Warp(upPoint.position);
                            playerAgent.ResetPath();
                        } else
                        {
                            baseScript.state = PlayerState.IDLE;
                            playerAgent.Warp(downPoint.position);
                            playerAgent.ResetPath();
                        }
                    }
                }
            }
        }
    }
}
