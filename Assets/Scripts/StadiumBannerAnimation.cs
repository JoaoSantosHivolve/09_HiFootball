using UnityEngine;

public class StadiumBannerAnimation : MonoBehaviour
{
    public Material banner1;
    public Material banner2;
    public float speed;

    private Vector2 uvOffset1 = Vector2.zero;
    private Vector2 uvOffset2 = Vector2.zero;
    private const string TEXTURE_NAME = "_MainTex";

    private void LateUpdate()
    {
        uvOffset1 += (Time.deltaTime * new Vector2(speed, 0));
        uvOffset1.y = 0.3f;
        uvOffset2 += (Time.deltaTime * new Vector2(speed, 0));
        uvOffset2.y = 0.18f;

        banner1.SetTextureOffset(TEXTURE_NAME, uvOffset1);
        banner2.SetTextureOffset(TEXTURE_NAME, uvOffset2);
    }
}
