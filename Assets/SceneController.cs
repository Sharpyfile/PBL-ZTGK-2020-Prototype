using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public Canvas tutorial;
    public Attack player;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (tutorial.gameObject.active == true)
                tutorial.gameObject.SetActive(false);
            else
                tutorial.gameObject.SetActive(true);
        }

        if (player.CurrentHp <= 0.0f)
            SceneManager.LoadScene("SampleSceneStrongAttack");

        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
            SceneManager.LoadScene("SampleSceneStrongAttack");

        if (Input.GetKeyUp(KeyCode.R))
            SceneManager.LoadScene("SampleSceneStrongAttack");

    }
}
