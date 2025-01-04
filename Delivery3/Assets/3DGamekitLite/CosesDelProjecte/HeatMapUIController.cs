using UnityEngine;
using UnityEngine.UI;

public class HeatMapUIController : MonoBehaviour
{
    [SerializeField] private HeatmapLoader heatmapLoader;

    // Métodos vinculados a los toggles de la UI
    public void TogglePlayerMove(bool isVisible)
    {
        heatmapLoader.SetHeatmapVisibility("PlayerMove", isVisible);
    }

    public void ToggleDeath(bool isVisible)
    {
        heatmapLoader.SetHeatmapVisibility("Death", isVisible);
    }

    public void ToggleDamaged(bool isVisible)
    {
        heatmapLoader.SetHeatmapVisibility("Damaged", isVisible);
    }

    public void ToggleVulnerable(bool isVisible)
    {
        heatmapLoader.SetHeatmapVisibility("Vulnerable", isVisible);
    }

    public void ToggleHitInvulnerable(bool isVisible)
    {
        heatmapLoader.SetHeatmapVisibility("HitInvulnerable", isVisible);
    }
}
