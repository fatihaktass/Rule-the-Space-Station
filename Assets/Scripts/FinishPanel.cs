using UnityEngine;

public class FinishPanel : MonoBehaviour
{
    public bool isActive;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isActive == true)
        {
            animator.SetBool("finalOpen", true);
            Invoke("quitEx", 30f);
        }
    }

    void quitEx()
    {
        Application.Quit();
        Debug.Log("bitti");
    }
}
