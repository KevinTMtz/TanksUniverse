using UnityEngine;
using UnityEngine.UI;

public class ShotForceBar : MonoBehaviour
{
    public Slider slider;

    public void SetForce(float force)
    {
        slider.value = (int) force;
    }
}
