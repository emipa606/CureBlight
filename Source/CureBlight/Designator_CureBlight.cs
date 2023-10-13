using RimWorld;
using UnityEngine;
using Verse;

namespace CureBlight;

public class Designator_CureBlight : Designator_Plants
{
    public Designator_CureBlight()
    {
        defaultLabel = "Vic_DesignatorCureBlight".Translate();
        defaultDesc = "Vic_DesignatorCureBlightDesc".Translate();
        icon = ContentFinder<Texture2D>.Get("Vicaki/Designators/CureBlight");
        soundDragSustain = SoundDefOf.Designate_DragStandard;
        soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
        useMouseIcon = true;
        soundSucceeded = SoundDefOf.Designate_CutPlants;
        designationDef = CBDefOf.Vic_CureBlightDesign;
    }

    public override AcceptanceReport CanDesignateThing(Thing t)
    {
        var result = base.CanDesignateThing(t);
        if (!result.Accepted)
        {
            return result;
        }

        if (((Plant)t).Blighted)
        {
            return true;
        }

        return "Vic_MessageMustDesignateBlighted".Translate();
    }
}