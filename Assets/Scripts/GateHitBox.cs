using UnityEngine;

public class GateHitBox : MonoBehaviour
{
    private const float MAX_SIDE_INCREMENT = 0.50f;
    private const float MIN_SIDE_INCREMENT = 0.30f;
    private const float MIN_UP_DISTANCE = 0.25f;
    private const float MAX_UP_DISTANCE = 1.0f;

    public int testInt;
    public float timer;

    // TEST PURPOSES
    private void Update()
    {
        //timer += Time.deltaTime;
        //if (Input.GetKeyDown(KeyCode.Space))
        ////if(timer >= 0.25f)
        //{
        //    timer = 0;
        //    var test = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    test.transform.position = GetRandomPosition(testInt);
        //    Destroy(test,1f);
        //}
    }

    public Vector3 GetRandomPosition(int difficulty)
    {
        var basePos = transform.position;

        var side = Random.value > 0.5f ? Vector3.right : Vector3.left;
        var minSideDistance = 0.00f + difficulty * MIN_SIDE_INCREMENT;
        var maxSideDistance = 0.50f + difficulty * MAX_SIDE_INCREMENT;
        var sideDistance = Random.Range(minSideDistance, maxSideDistance);

        var up = Vector3.up;
        var upDistance = Random.Range(MIN_UP_DISTANCE, MAX_UP_DISTANCE);
        if (difficulty == 0) upDistance += 1;
        if (difficulty == 1) upDistance += 0.5f;

        return basePos + (side * sideDistance) + (up * upDistance);
    }
}