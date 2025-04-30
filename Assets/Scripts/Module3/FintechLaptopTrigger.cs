using UnityEngine;

public class FintechLaptopTrigger : MonoBehaviour
{
    public string fintechToolName; // Assign in inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<Lesson3_14>().OpenMiniTask(fintechToolName);
        }
    }
}
