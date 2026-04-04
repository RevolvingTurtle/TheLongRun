using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public Transform bg1;
    public Transform bg2;

    public float scrollMultiplier = 0.4f;
    public float spriteWidth = 20f;

    void Update()
    {
        if (GameManager.I == null || !GameManager.I.isRunning) return;

        float moveAmount = GameManager.I.scrollSpeed * scrollMultiplier * Time.deltaTime;

        bg1.position += Vector3.left * moveAmount;
        bg2.position += Vector3.left * moveAmount;

        if (bg1.position.x <= -spriteWidth)
            bg1.position = new Vector3(bg2.position.x + spriteWidth, bg1.position.y, bg1.position.z);

        if (bg2.position.x <= -spriteWidth)
            bg2.position = new Vector3(bg1.position.x + spriteWidth, bg2.position.y, bg2.position.z);
    }
}