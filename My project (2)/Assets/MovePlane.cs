using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class MovePlane : MonoBehaviour
{
    public GameObject Plane;
    public RectTransform parentRectTransform;
    public float moveSpeed; // Ýleri hareket hýzý
    public float moveUp;
    public float moveDuration = 5f; // Hareket süresi

    public bool isMoving { get; private set; } = false;
    private Vector3 startPosition;

    void Start()
    {
        // Plane objesinin baþlangýç konumunu kaydet
        startPosition = Plane.transform.localPosition;
    }

    void Update()
    {
        if (isMoving)
        {
            // Yeni konumu hesapla
            Vector3 newPosition = Plane.transform.localPosition +
                                  (Vector3.right * moveSpeed + Vector3.up * moveUp) * Time.deltaTime;

            // Üst parentin sýnýrlarýný kontrol et
            if (IsWithinBounds(newPosition))
            {
                // Yeni konumu uygula
                Plane.transform.localPosition = newPosition;
            }
        }
    }

    public void StartMoving()
    {
        // Plane objesini baþlangýç konumuna geri döndür
        Plane.transform.localPosition = startPosition;
        isMoving = true;
        // Hareketi belirli bir süre sonra durdur
        StartCoroutine(StopMovingAfterTime(moveDuration));
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    private IEnumerator StopMovingAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        if(isMoving==false)
        {
            StopMoving();
        }
        
    }

    private bool IsWithinBounds(Vector3 position)
    {
        Rect parentRect = parentRectTransform.rect;

        // Plane objesinin boyutlarýný alýn
        RectTransform planeRectTransform = Plane.GetComponent<RectTransform>();
        Vector2 planeSize = planeRectTransform.sizeDelta;

        // Konumu kontrol et
        bool withinXBounds = (position.x + planeSize.x / 2 <= parentRect.width / 2) &&
                             (position.x - planeSize.x / 2 >= -parentRect.width / 2);
        bool withinYBounds = (position.y + planeSize.y / 2 <= parentRect.height / 2) &&
                             (position.y - planeSize.y / 2 >= -parentRect.height / 2);

        return withinXBounds && withinYBounds;
    }
}
