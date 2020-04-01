using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject player;
    public Attack playerAttack;
    public float distance;
    public float shakeTime;
    private float tempShakeTime;
    public float returnTime;
    private float tempReturnTime;

    private Vector3 startPosition;
    private Vector3 newPos;



    // Start is called before the first frame update
    void Start()
    {
        newPos = new Vector3();
        tempReturnTime = returnTime;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Attacked?: " + playerAttack.performedAttack.ToString());
        
        if (playerAttack.performedAttack)
        {
            newPos = player.transform.position;
            newPos.y = distance;
            tempShakeTime = 0;
            startPosition = player.transform.position;
            startPosition.y = distance;
            Random.InitState(System.Environment.TickCount);
            float x = Random.Range(-0.1f, 0.1f);
            Random.InitState(System.Environment.TickCount);
            float y = Random.Range(-0.1f, 0.1f);
            Vector2 randPos = new Vector2(x, y);
            newPos.Set(newPos.x + randPos.x, newPos.y, newPos.z + randPos.y);
            playerAttack.performedAttack = false;
        }
            

        if (tempShakeTime < shakeTime)
        {
            tempShakeTime += Time.deltaTime;
            Debug.Log(tempShakeTime);
            transform.position = Vector3.Lerp(startPosition, newPos, tempShakeTime);
            tempReturnTime = 0;
            
        }
        else if (tempReturnTime < returnTime)
        {
            tempReturnTime += Time.deltaTime;
            transform.position = Vector3.Lerp(newPos, startPosition, tempReturnTime);
        }
        else
        {
            newPos = player.transform.position;
            newPos.y = distance;
            transform.position = newPos;
        }
       
    }

}
