using UnityEngine;

public class PlaneTest : MonoBehaviour
{
    public Plane plane;

    void Start()
    {
        // D�zlemi ba�lang��ta olu�turun.
        plane = new Plane(Vector3.up, 0);
    }

    void Update()
    {
        // Nesnenin oldu�u pozisyondan d�zleme do�ru bir ���n olu�turun.
        Ray ray = new Ray(transform.position, Vector3.down);

        // I��n ile d�zlemin �arp��mas�n� kontrol edin.
        if (plane.Raycast(ray, out float hitDistance))
        {
            Debug.Log("Nesne d�zlemin �zerinde!");
        }
        else
        {
            Debug.Log("Nesne d�zlemin �zerinde de�il!");
        }
    }
}