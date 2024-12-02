using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 2f;    // Tốc độ cuộn
    public float backgroundHeight;    // Chiều cao của background (kích thước theo trục Y)

    private Vector3 startPosition;    // Vị trí bắt đầu của background

    void Start()
    {
        // Lưu lại vị trí ban đầu của background
        startPosition = transform.position;

        // Tính chiều cao của background từ sprite hoặc đối tượng
        backgroundHeight = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        // Di chuyển background xuống dưới theo thời gian
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // Khi background đã ra khỏi màn hình (phần dưới cùng vượt qua vị trí ban đầu), đưa nó về lại phía trên
        if (transform.position.y < startPosition.y - backgroundHeight)
        {
            RepositionBackground();
        }
    }

    // Hàm để đưa background trở lại vị trí phía trên màn hình
    void RepositionBackground()
    {
        Vector3 offset = new Vector3(0, backgroundHeight * 2f, 0);  // Di chuyển background lên trên
        transform.position += offset;   // Dịch chuyển lên trên để tiếp tục cuộn
    }
}
