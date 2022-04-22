using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;


public class SleepingDart : MonoBehaviour
{

    public CharacterBaseBehavior baseScript;

    //General Variables
    public Camera playerCamera;
    private NavMeshAgent agent;
    private RaycastHit rayHit;
    private bool enemyOutOfRange;
    private Animator neralaAnimator;
    private bool addLineComponentOnce;

    private GameObject targetEnemy;
    private Vector3 targetDistance;

    //Ability Stats
    public float maximumRange;
    public int ammunition;


    // Start is called before the first frame update
    void Start()
    {
        addLineComponentOnce = true;
        enemyOutOfRange = false;

        neralaAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(baseScript.selectedCharacter)
        {
            if (baseScript.ability1Active)
            {
                if (addLineComponentOnce)
                {
                    addLineComponentOnce = false;
                    gameObject.AddComponent<LineRenderer>();
                }

                gameObject.DrawCircle(maximumRange * 6, .05f);

                if (Input.GetKeyDown(KeyCode.Mouse0) && ammunition > 0)
                {
                    Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out rayHit))
                    {
                        targetEnemy = rayHit.collider.gameObject;

                        if (rayHit.collider.tag == "Enemy")
                        {
                            targetDistance = CalculateAbsoluteDistance(rayHit.point);

                            if (targetDistance.magnitude >= maximumRange)
                            {
                                enemyOutOfRange = true;
                                agent.SetDestination(targetEnemy.transform.position);
                                
                                if (neralaAnimator != null)
                                {
                                    neralaAnimator.ResetTrigger("hasStopped");
                                    neralaAnimator.SetTrigger("isWalking");
                                }
                            } else
                            {
                                if (neralaAnimator != null)
                                {
                                    gameObject.transform.LookAt(targetEnemy.transform);
                                    gameObject.transform.rotation *= Quaternion.Euler(0, 90, 0);

                                    neralaAnimator.ResetTrigger("hasStopped");
                                    neralaAnimator.SetTrigger("sleepingDart");
                                }

                                // Set Sleepy Effect to Enemy 
                                Material tempMaterial = targetEnemy.GetComponent<MeshRenderer>().material;
                                tempMaterial.color = Color.green;
                                //

                                ammunition--;
                            }
                        }
                    }
                }


                if (enemyOutOfRange)
                {
                    targetDistance = CalculateAbsoluteDistance(targetEnemy.transform.position);

                    if (targetDistance.magnitude <= maximumRange)
                    {
                        agent.ResetPath();

                        if (neralaAnimator != null)
                        {
                            gameObject.transform.LookAt(targetEnemy.transform);
                            gameObject.transform.rotation *= Quaternion.Euler(0, 90, 0);

                            neralaAnimator.ResetTrigger("isWalking");
                            neralaAnimator.ResetTrigger("hasStopped");
                            neralaAnimator.SetTrigger("sleepingDart");
                        }

                        // Set Sleepy Effect to Enemy 
                        Material tempMaterial = targetEnemy.GetComponent<MeshRenderer>().material;
                        tempMaterial.color = Color.green;
                        //

                        ammunition--;
                        enemyOutOfRange = false;
                    }
                }

            } else
            {
                addLineComponentOnce = true;

            }
        } else
        {
            addLineComponentOnce = true;
        }

    }

    void OnGUI()
    {
        if(baseScript.selectedCharacter)
            if (baseScript.ability1Active) GUI.Box(new Rect(0, Screen.height - 25, 150, 25), "Sleeping Dart Active");
    }

    Vector3 CalculateAbsoluteDistance(Vector3 targetPos)
    {
        Vector3 distance = new Vector3(0f, 0f, 0f);

        distance.x = Mathf.Abs(transform.position.x - targetPos.x);
        distance.z = Mathf.Abs(transform.position.z - targetPos.z);

        return distance;
    }
}
