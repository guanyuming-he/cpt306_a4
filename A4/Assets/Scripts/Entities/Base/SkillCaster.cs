using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A skill caster is someone who is able to cast spells.
/// 
/// </summary>
public class SkillCaster : MonoBehaviour
{
    /*********************************** Fields ***********************************/
    // He can select from these spells.
    private List<ConcreteSkill> preparedSkills;
    // He can only cast this skill.
    // if this is out of the range, then it means he has not selected a skill yet.
    private int indSelectedSkill;

    /*********************************** Ctor ***********************************/
    public SkillCaster()
    {
        // By default, no skill is prepared,
        preparedSkills = new List<ConcreteSkill>();
        // and none is selected.
        indSelectedSkill = -1;
    }

    /*********************************** Observers ***********************************/

    /// <summary>
    /// Used more than once, hence this method.
    /// </summary>
    private bool isIndInPreparedSkillsRange(int ind)
    {
        return 0 <= ind && ind < preparedSkills.Count;
    }
    
    /// <returns>true iff a skill is selected.</returns>
    public bool isAnySkillSelected()
    {
        return isIndInPreparedSkillsRange(indSelectedSkill);
    }

    public int getIndSelectedSkill() { return indSelectedSkill; }

    /// <returns>
    /// the selected skill, or null if currently no skill is selected.
    /// </returns>
    public ConcreteSkill getSelectedSkill()
    {   
        if (!isAnySkillSelected())
        {
            return null;
        }

        // as returns null if the convertion is not possible.
        return preparedSkills[indSelectedSkill];
    }

    /// <typeparam name="T">a derived type of ConcreteSkill</typeparam>
    /// <returns>
    /// the selected skill as T, or null if currently no skill is selected
    /// </returns>
    /// <exception cref="System.ArgumentException">
    /// if T is not a type of the selected skill.
    /// </exception>
    public T getSelectedSkill<T>() where T : ConcreteSkill
    {
        return getSelectedSkill() as T;
    }

    /*********************************** Mutators ***********************************/
    /// <summary>
    /// Prepare these skills.
    /// Will clear previously prepared skills first.
    /// </summary>
    public void prepareSkills(List<ConcreteSkill> skills)
    {
        if(preparedSkills.Count != 0)
        {
            preparedSkills.Clear();
        }

        // copy the list.
        preparedSkills = new List<ConcreteSkill>(skills);
    }

    /// <summary>
    /// Attempts to cast the currently selected skill.
    /// If no spell is selected, then it does nothing.
    /// </summary>
    /// <exception cref="System.InvalidOperationException">
    /// if the currently selected skill cannot be casted
    /// </exception>
    protected void castSkill(Vector3 position, GameObject target)
    {
        ConcreteSkill skill = getSelectedSkill();
        if(skill == null)
        // none is selected.
        {
            return;
        }

        // this will throw System.InvalidOperationException if it cannot be released.
        skill.release(position, target); 
    }

    /// <summary>
    /// Select a skill.
    /// Note: Do NOT use this to deselect skills (so that none is selected).
    /// Instead, use deselectSkill().
    /// </summary>
    /// <param name="indNewSkill">index of the new skill.</param>
    /// <exception cref="System.ArgumentException">
    /// if the index is out of the prepared range.
    /// </exception>
    protected void selectSkill(int indNewSkill)
    {
        if(!isIndInPreparedSkillsRange(indNewSkill))
        {
            throw new System.ArgumentException("the index is out of range.");
        }

        // change the ind
        // but don't forget to have the selection effects displayed
        {
            ConcreteSkill oldSkill = getSelectedSkill();
            if(oldSkill == null)
            {
                // no skill was selected.
            }
            else
            {
                // the deselection effect.
                oldSkill.onDeselected();
            }

            indSelectedSkill = indNewSkill;
            ConcreteSkill newSkill = getSelectedSkill();
            Utility.MyDebugAssert(newSkill != null, "terrible logical error happened.");
            // the selection effect
            newSkill.onSelected();
        }
    }

    /// <summary>
    /// Deselect the currently selected skill so that none is selected after the call.
    /// </summary>
    protected void deselectSkill()
    {
        if(!isAnySkillSelected())
        {
            return;
        }

        // play the deselection effect
        var curSkill = getSelectedSkill();
        Utility.MyDebugAssert(curSkill != null, "terrible logical error happened.");
        curSkill.onDeselected();

        // change the index
        indSelectedSkill = -1;
    }
}