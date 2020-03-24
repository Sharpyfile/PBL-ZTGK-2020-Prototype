using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class CameraApplyShader : MonoBehaviour
{
    public float intensity;

    private float maxGreen = 1.0f;
    private float minGreen = 0.0f;
    private float currentGreen;
    private float greenModifier = 0.75f;

    private Material material1;
    private Material material2;
    private Material material3;
    private Material material4;
    private Material material5;

    public GameObject pl;
    private PlayerController player;

    // Start is called before the first frame update
    private void Start()
    {
        player = pl.GetComponent<PlayerController>();
        if (player == null)
            Debug.Log("Null on plCTRL");
    }
    void Awake()
    {
        material1 = new Material(Shader.Find("Hidden/SlowPillShader"));
        material2 = new Material(Shader.Find("Hidden/SpeedPillShader"));
        material3 = new Material(Shader.Find("Hidden/ResistancePillShader"));
        material4 = new Material(Shader.Find("Hidden/DamagePillShader"));
        material5 = new Material(Shader.Find("Hidden/HealthPillShader"));
        
    }

    private void FixedUpdate()
    {
        currentGreen += greenModifier * Time.deltaTime;
        
        if (currentGreen > maxGreen)
        {
            greenModifier *= -1.0f;
        }
        else if (minGreen > currentGreen)
        {
            greenModifier *= -1.0f;
        }

    }


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        if (player.pickup == null)
        {
            Graphics.Blit(source, destination);
            return;
        }
        else if (player.pickup.GetComponent<PickupStatistics>().pillName == "SlowPill")
        {
            material1.SetFloat("_bwBlend", 0.9f);
            Graphics.Blit(source, destination, material1);
        }
        else if (player.pickup.GetComponent<PickupStatistics>().pillName == "SpeedPill")
        {
            Graphics.Blit(source, destination, material2);
        }
        else if (player.pickup.GetComponent<PickupStatistics>().pillName == "ResistancePill")
        {
            Graphics.Blit(source, destination, material3);
        }
        else if (player.pickup.GetComponent<PickupStatistics>().pillName == "DamagePill")
        {

            Graphics.Blit(source, destination, material4);
        }
        else if (player.pickup.GetComponent<PickupStatistics>().pillName == "HealthPill")
        {
            material5.SetFloat("_currentGreen", currentGreen);
            Graphics.Blit(source, destination, material5);
        }
    }
}
