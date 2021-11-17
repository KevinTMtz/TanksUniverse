using UnityEngine;
using UnityEngine.UI;

public class ShotForceBar : MonoBehaviour
{
    public Slider slider;

    public void SetForce(int force)
    {
        slider.value = force;
    }
}
