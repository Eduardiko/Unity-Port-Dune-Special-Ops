using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZhibAbilities : MonoBehaviour
{
    //General Variables
    public Camera playerCamera;
    public Transform attackPoint;
    public Vector3 attackPointOffset;
    private RaycastHit rayHit;

    private bool knifeThrown;

    //Ability Stats
    public float maximumRange;
    public float fireRate;
    public int ammunition;

    //Knife
    public GameObject knifePrefab;
    private GameObject[] thrownKnifes;
    public float knifeVelocity;
    public float soundRange;

    // Start is called before the first frame update
    void Start()
    {
        thrownKnifes = new GameObject[ammunition];
    }

    // Update is called once per frame
    void Update()
    {
        knifeThrown = false;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out rayHit))
        {
            if (rayHit.collider.tag == "Enemy")
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && ammunition > 0)
                {
                    if (!knifeThrown)
                    {
                        knifeThrown = true;

                        Vector3 spawnPoint = attackPoint.position + (attackPoint.rotation * attackPointOffset);

                        for (int i = 0; i < thrownKnifes.Length; i++)
                        {
                            if (thrownKnifes[i] == null)
                            {
                                thrownKnifes[i] = Instantiate(knifePrefab, spawnPoint, attackPoint.rotation);
                                thrownKnifes[i].transform.LookAt(rayHit.collider.gameObject.transform);
                                break;
                            }
                        }
 
                        ammunition--;
                    }
                }
            }
        }

            KnifeUpdate(thrownKnifes);
        
    }

    void KnifeUpdate(GameObject[] thrownKnifes)
    {
        for (int i = 0; i < thrownKnifes.Length; i++)
        {
            if (thrownKnifes[i] != null)
            {
                thrownKnifes[i].transform.position += thrownKnifes[i].transform.rotation * Vector3.forward * knifeVelocity * Time.deltaTime;
            }
        }
    }
}
