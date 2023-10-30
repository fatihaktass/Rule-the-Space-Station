using UnityEngine;
using UnityEngine.UI;

public class MouseInput : MonoBehaviour
{
    public Transform player;
    public Slider sensivitySlider;
    public float sensivitySpeed = 200f;
    public Text sensivityText;
    public int sensInt;
    float xRotation;



    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;        
    }

    private void Start()
    {
        sensivitySlider.value = PlayerPrefs.GetFloat("Sensivity");
    }

    // Update is called once per frame
    void Update()
    {      

        float mouseXPos = Input.GetAxis("Mouse X") * sensivitySpeed * Time.deltaTime;
        float mouseYPos = Input.GetAxis("Mouse Y") * sensivitySpeed * Time.deltaTime;

        xRotation -= mouseYPos;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        player.Rotate(Vector3.up * mouseXPos);

    }

    // Bunu levelimizdeki slidera tanýmlýyoruz.

    public void SensivitySettings()
    {
        sensivitySpeed = sensivitySlider.value;
        PlayerPrefs.SetFloat("Sensivity", sensivitySpeed);
        sensInt = (int)Mathf.Round(sensivitySpeed);
        sensivityText.text = sensInt.ToString();
    }
}
