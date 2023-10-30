using UnityEngine;

public class DoorTrigger3 : MonoBehaviour
{
    public bool tetiklendiMi;

    private void Update()
    {
        if (tetiklendiMi == true)
        {
            if (transform.position.y < 6f)
            {
                FindObjectOfType<Boss>().healthSlider.gameObject.SetActive(true);
                transform.Translate(0, (1f * Time.deltaTime), 0);
            }
            else if (transform.position.y > 6f)
            {
                Destroy(gameObject);
            }
        }
        
    }
}
