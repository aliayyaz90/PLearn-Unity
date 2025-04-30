using UnityEngine;
public class PatelRunner : MonoBehaviour
{
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        transform.GetChild(0).GetComponent<Animator>().SetBool("run", true);
    }
    private void FixedUpdate()
    {
        transform.Translate(0, 0, 4*Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (PlayerPrefs.GetInt("gameprogress") == 5)
        {
            if (other.gameObject.tag == "patelGO1")
                transform.rotation = Quaternion.Euler(0, 90, 0);
            else if (other.gameObject.tag == "patelGO2")
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (other.gameObject.tag == "patelGO3")
            {
                transform.GetChild(0).GetComponent<Animator>().SetBool("run", false);
                GetComponent<PatelRunner>().enabled = false;
                GetComponent<BoxCollider>().enabled = false;
                other.gameObject.SetActive(false);
            }
        }
    }
}