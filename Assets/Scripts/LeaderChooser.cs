using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderChooser : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] enemies;
    private bool skip;
    private int value;
    private List<float> distances;
    [SerializeField] private GameObject player;
    void Start()
    {
        distances = new List<float>();
    }

    // Update is called once per frame
    void Update()
    {
        skip = false;
        distances.Clear();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in enemies)
        {
            distances.Add(Distance(e, player));
        }
        enemies[ShortestDistance()].GetComponent<EnemyController>().SetLeader();
    }

    private float Distance(GameObject enemy, GameObject player)
    {
        return Mathf.Sqrt(((player.transform.position.x - enemy.transform.position.x) * (player.transform.position.x - enemy.transform.position.x)) +
            ((player.transform.position.z - enemy.transform.position.z) * (player.transform.position.z - enemy.transform.position.z)));
    }

    private int ShortestDistance()
    {
        float min = distances[0];
        int index = 0;
        for (int i = 1; i < distances.Count; i++)
        {
            if (distances[i] < min)
            {
                min = distances[i];
                index = i;
            }
        }
        return index;
    }
}
