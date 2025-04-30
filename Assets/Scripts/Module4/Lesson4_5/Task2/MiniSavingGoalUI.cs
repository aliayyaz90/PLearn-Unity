using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MiniSavingGoalUI : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text progressText;
    public Slider progressSlider;

    public void UpdateUI(MiniSavingGoal goal)
    {
        titleText.text = goal.title;
        progressText.text = $"{goal.savedAmount} / {goal.targetAmount} saved ({goal.turnsUsed}/{goal.duration} turns)";
        progressSlider.maxValue = goal.targetAmount;
        progressSlider.value = goal.savedAmount;

        if (goal.isComplete)
        {
            progressText.text = "✅ Goal Complete!";
        }
    }
}
