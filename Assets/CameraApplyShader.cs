using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class CameraApplyShader : MonoBehaviour
{
    public float intensity;
    private Material material1;
    private Material material2;

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
        material1 = new Material(Shader.Find("Hidden/PillOne"));
        material2 = new Material(Shader.Find("Hidden/PillTwo"));
        
    }


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        if (player.pickup == null)
        {
            Graphics.Blit(source, destination);
            return;
        }
        if (player.pickup.GetComponent<PickupStatistics>().pillName == "SpeedPill")
        {
            Graphics.Blit(source, destination, material2);
        }
        else if (player.pickup.GetComponent<PickupStatistics>().pillName == "SlowPill")
        {
            material1.SetFloat("_bwBlend", 0.9f);
            Graphics.Blit(source, destination, material1);
        }






    }
}
