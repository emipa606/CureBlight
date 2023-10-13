using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace CureBlight;

[HarmonyPatch(typeof(Plant), nameof(Plant.GetGizmos))]
public static class Plant_GetGizmos
{
    public static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> values, Plant __instance)
    {
        if (values?.Any() == true)
        {
            foreach (var value in values)
            {
                yield return value;
            }
        }

        if (!__instance.Blighted)
        {
            yield break;
        }

        var designationDef = DefDatabase<DesignationDef>.GetNamedSilentFail("Vic_CureBlightDesign");
        var icon = ContentFinder<Texture2D>.Get("Vicaki/Designators/CureBlight");
        var designation = __instance.Map.designationManager.DesignationOn(__instance);
        if (designation == null || designation.def != designationDef)
        {
            yield return new Command_Action
            {
                defaultLabel = "Vic_DesignatorCureBlight".Translate(),
                defaultDesc = "Vic_DesignatorCureBlightDesc".Translate(),
                icon = icon,
                action = delegate
                {
                    __instance.Map.designationManager.AddDesignation(new Designation(__instance, designationDef));
                }
            };
        }

        yield return new Command_Action
        {
            defaultLabel = "Vic_DesignatorCureAllBlight".Translate(),
            defaultDesc = "Vic_DesignatorCureAllBlightDesc".Translate(),
            icon = icon,
            action = delegate
            {
                foreach (var thing in __instance.Map.listerThings.ThingsInGroup(ThingRequestGroup.Plant))
                {
                    var plant = (Plant)thing;
                    if (plant is { Blighted: true } && !__instance.Map.designationManager.HasMapDesignationOn(plant))
                    {
                        __instance.Map.designationManager.AddDesignation(new Designation(plant, designationDef));
                    }
                }
            }
        };
    }
}