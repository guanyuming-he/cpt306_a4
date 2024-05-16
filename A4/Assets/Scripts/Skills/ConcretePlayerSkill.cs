using UnityEngine;

/// <summary>
/// A concrete player skill is a mono script
///     1. attached to the player through the skill store/inventory in the UI.
///     2. is selected & deselected at runtime by the player
/// 
/// </summary>
public class ConcretePlayerSkill : MonoBehaviour, IPlayerSkill
{
    public void onSkillDeselected()
    {
        throw new System.NotImplementedException();
    }

    public void onSkillReleased()
    {
        throw new System.NotImplementedException();
    }

    public void onSkillSelected()
    {
        throw new System.NotImplementedException();
    }
}
