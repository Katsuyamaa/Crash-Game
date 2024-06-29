using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class MovePlane : MonoBehaviour
{
    public GameObject Plane;
    public RectTransform parentRectTransform;
    public float moveSpeed; // �leri hareket h�z�
    public float moveUp;
    public float moveDuration = 5f; // Hareket s�resi

    public bool isMoving { get; private set; } = false;
    private Vector3 startPosition;

    void Start()
    {
        // Plane objesinin ba�lang�� konumunu kaydet
        startPosition = Plane.transform.localPosition;
    }

    void Update()
    {
        if (isMoving)
        {
            // Yeni konumu hesapla
            Vector3 newPosition = Plane.transform.localPosition +
                                  (Vector3.right * moveSpeed + Vector3.up * moveUp) * Time.deltaTime;

            // �st parentin s�n�rlar�n� kontrol et
            if (IsWithinBounds(newPosition))
            {
                // Yeni konumu uygula
                Plane.transform.localPosition = newPosition;
            }
        }
    }

    public void StartMoving()
    {
        // Plane objesini ba�lang�� konumuna geri d�nd�r
        Plane.transform.localPosition = startPosition;
        isMoving = true;
        // Hareketi belirli bir s�re sonra durdur
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

        // Plane objesinin boyutlar�n� al�n
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
