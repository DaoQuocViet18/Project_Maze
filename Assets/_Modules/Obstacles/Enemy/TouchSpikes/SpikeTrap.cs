using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private const string ANIM_RAISE = "Raise";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger(ANIM_RAISE);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
