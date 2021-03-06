using System.Collections;
using UnityEngine;
using UnityEngine.AI;


public class SmokeBomb : MonoBehaviour
{

    private SmokeBombAbility smokeBombScript;

    public LayerMask whatIsPlayer;
    public float firingAngle = 45.0f;
    public float gravity;

    [HideInInspector] public float smokeRange;
    private float timeOfEffect;
    private float elapse_time;

    public bool groundHit;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.rotation = Quaternion.identity;

        GameObject go = GameObject.Find("Nerala");
        smokeBombScript = go.GetComponent<SmokeBombAbility>();
        smokeRange = smokeBombScript.smokeEffectRange;
        timeOfEffect = smokeBombScript.timeOfEffect;
        elapse_time = 0f;
        groundHit = false;

        StartCoroutine(SimulateProjectile());
    }

    void Update()
    {
        if(groundHit)
        {
            while (elapse_time < timeOfEffect)
            {
                elapse_time += Time.deltaTime;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                return;
            }

            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        Collider[] affectedCharacters = Physics.OverlapSphere(transform.position, smokeRange, whatIsPlayer);
        
        for(int i = 0; i < affectedCharacters.Length; i++)
        {
            CharacterBaseBehavior cB = affectedCharacters[i].gameObject.GetComponent<CharacterBaseBehavior>();
            cB.invisible = true;
        }
    }
    IEnumerator SimulateProjectile()
    {

        // Calculate distance to target
        float target_Distance = Vector3.Distance(gameObject.transform.position, smokeBombScript.targetPosition);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * 1.25f * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        gameObject.transform.rotation = Quaternion.LookRotation(smokeBombScript.targetPosition - gameObject.transform.position);

        float elapse_time = 0;

        while (elapse_time < flightDuration - 0.3)
        {
            gameObject.transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;

            yield return null;
        }

        groundHit = true;
    }


}
