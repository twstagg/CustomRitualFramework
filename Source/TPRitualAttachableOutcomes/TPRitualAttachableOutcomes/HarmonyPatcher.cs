﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;



namespace TPRitualAttachableOutcomes
{
    [StaticConstructorOnStartup]
    static class HarmonyPatcher
    {

        static HarmonyPatcher()
        {
            Harmony harmony = new Harmony("TPRitualAttachableOutcomes.HarmonyPatches");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(RitualOutcomeEffectWorker_FromQuality))]
    [HarmonyPatch("ApplyAttachableOutcome")]
    public static class Patch_RitualOutcomeEffectWorker_FromQuality
    {
        public static void Prefix(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, OutcomeChance outcomeChance,  string extraLetterText, ref LookTargets letterLookTargets)
        {
            if(jobRitual == null || jobRitual.Ritual == null || jobRitual.Ritual.attachableOutcomeEffect == null || jobRitual.Ritual.attachableOutcomeEffect.GetModExtension<RitualAttachableOutcomeEffectDef_TP_Custom>() == null)
            {
                return;
            }

            RitualAttachableOutcomeEffectDef_TP_Custom customRitualAttachableOutcomeEffect = jobRitual.Ritual.attachableOutcomeEffect.GetModExtension<RitualAttachableOutcomeEffectDef_TP_Custom>();
            if(customRitualAttachableOutcomeEffect != null)
            {
                jobRitual.Ritual.attachableOutcomeEffect.onlyPositiveOutcomes = customRitualAttachableOutcomeEffect.onlyPositiveOutcomes;
            }

            return;
        }
    }

    [HarmonyPatch(typeof(Precept_Ritual), "get_RepeatPenaltyProgress")]
    internal class Patch_RepeatPenaltyProgress
    {
        private static bool Prefix(Precept_Ritual __instance, ref float __result)
        {
            int coolDownDays = 20;
            Precept_Ritual_Custom customPreceptRitual = __instance.def.GetModExtension<Precept_Ritual_Custom>();
            if(customPreceptRitual != null)
            {
                //Log.Message("customCooldown: " + customPreceptRitual.coolDownDays);
                if (customPreceptRitual.coolDownDays != coolDownDays)
                {
                    coolDownDays = customPreceptRitual.coolDownDays;
                }
            }
            __result = (float)__instance.TicksSinceLastPerformed / (60000f * (float)coolDownDays);
            //Log.Message("RepeatPenaltyProgress: " + __result);
            return false;
        }
    }

    [HarmonyPatch(typeof(Precept_Ritual), "get_RepeatPenaltyTimeLeft")]
    internal class Patch_RepeatPenaltyTimeLeft
    {
        private static bool Prefix(Precept_Ritual __instance, ref string __result)
        {
            int coolDownDays = 20;
            Precept_Ritual_Custom customPreceptRitual = __instance.def.GetModExtension<Precept_Ritual_Custom>();
            if (customPreceptRitual != null)
            {
                //Log.Message("customCooldown: " + customPreceptRitual.coolDownDays);
                if (customPreceptRitual.coolDownDays != 20)
                {
                    coolDownDays = customPreceptRitual.coolDownDays;
                }
            }
            int myTicks = 60000 * coolDownDays;
            __result = (myTicks - __instance.TicksSinceLastPerformed).ToStringTicksToPeriod();
            //Log.Message("RepeatPenaltyTimeLeft: " + __result);
            return false;
        }
    }

    [HarmonyPatch(typeof(Precept_Ritual), "get_RepeatPenaltyActive")]
    internal class Patch_RepeatPenaltyActive
    {
        private static bool Prefix(Precept_Ritual __instance, ref bool __result)
        {
            int coolDownDays = 20;
            Precept_Ritual_Custom customPreceptRitual = __instance.def.GetModExtension<Precept_Ritual_Custom>();
            if (customPreceptRitual != null)
            {
                //Log.Message("customCooldown: " + customPreceptRitual.coolDownDays);
                if (customPreceptRitual.coolDownDays != 20)
                {
                    coolDownDays = customPreceptRitual.coolDownDays;
                }
            }
            int myTicks = 60000 * coolDownDays;
            if (__instance.isAnytime && __instance.lastFinishedTick != -1 && __instance.def.useRepeatPenalty)
            {
                __result = __instance.TicksSinceLastPerformed < myTicks;
                //Log.Message("RitualPenaltyActive: " + __result);
            } else
            {
                return true;
            }
            return false;
        }
    }

    //RepeatQualityPenalty
    [HarmonyPatch(typeof(Precept_Ritual), "get_RepeatQualityPenalty")]
    internal class Patch_RepeatQualityPenalty
    {
        private static bool Prefix(Precept_Ritual __instance, ref float __result)
        {
            int coolDownDays = 20;
            Precept_Ritual_Custom customPreceptRitual = __instance.def.GetModExtension<Precept_Ritual_Custom>();
               
           // Log.Message("getting the real cooldown for " + __instance.def.defName);
            
            if (customPreceptRitual != null)
            {
                //Log.Message("customCooldown: " + customPreceptRitual.coolDownDays + " for ritual " + __instance.def.label); 
                if (customPreceptRitual.coolDownDays != coolDownDays)
                {
                    coolDownDays = customPreceptRitual.coolDownDays;
                }
            }
            float penaltyProgress = (float)__instance.TicksSinceLastPerformed / (60000f * (float)coolDownDays); 
            __result = Mathf.Lerp(-0.95f, 0f, penaltyProgress);
            float origResult = Mathf.Lerp(-0.95f, 0f, (float)__instance.TicksSinceLastPerformed / (60000f * 20f));
            //Log.Message("RepeatQualityPenalty: " + __result + " (instead of " + origResult + ")");
            return false;
        }
    }

   
    // TipMainPart
    [HarmonyPatch(typeof(Precept_Ritual), "TipMainPart")]
    internal class Patch_TipMainPart
    {
        private static string ColorizeWarning(TaggedString title)
        {
            return title.Resolve().Colorize(ColoredText.ThreatColor);
        }

        private static bool Prefix(Precept_Ritual __instance, ref string __result)
        {
            int coolDownDays = 20;
            Precept_Ritual_Custom customPreceptRitual = __instance.def.GetModExtension<Precept_Ritual_Custom>();

            //Log.Message("getting the real cooldown for " + __instance.def.defName);

            if (customPreceptRitual != null)
            {
                //Log.Message("customCooldown: " + customPreceptRitual.coolDownDays + " for ritual " + __instance.def.label);
                if (customPreceptRitual.coolDownDays != coolDownDays)
                {
                    coolDownDays = customPreceptRitual.coolDownDays;
                } 
                else // so far the only reason to override this is for a non-default cooldown, so if it is the default cooldown just go back to the original
                {
                    return true;
                }
            }
            float penaltyProgress = (float)__instance.TicksSinceLastPerformed / (60000f * (float)coolDownDays);

            StringBuilder tmpCompsDesc = new StringBuilder();
            
            if (__instance.RepeatPenaltyActive)
            {
                float num = (float)Mathf.RoundToInt(__instance.RepeatPenaltyProgress * (float)coolDownDays * 10f) / 10f;
                float num2 = (float)Mathf.RoundToInt((1f - __instance.RepeatPenaltyProgress) * (float)coolDownDays * 10f) / 10f;
                tmpCompsDesc.AppendLine(ColorizeWarning("RitualRepeatPenaltyTip".Translate(coolDownDays, num, __instance.RepeatQualityPenalty.ToStringPercent(), num2)));
                tmpCompsDesc.AppendLine();
            }
            if (!__instance.Description.NullOrEmpty())
            {
                tmpCompsDesc.Append(__instance.Description);
            }
            if (__instance.outcomeEffect != null)
            {
                StringBuilder stringBuilder = new StringBuilder(__instance.outcomeEffect.def.Description);
                if (!__instance.outcomeEffect.def.extraPredictedOutcomeDescriptions.NullOrEmpty())
                {
                    foreach (string extraPredictedOutcomeDescription in __instance.outcomeEffect.def.extraPredictedOutcomeDescriptions)
                    {
                        stringBuilder.Append(" " + extraPredictedOutcomeDescription.Formatted(__instance.shortDescOverride ?? __instance.def.label));
                    }
                }
                if (__instance.attachableOutcomeEffect != null)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.AppendLine();
                    }
                    stringBuilder.AppendInNewLine(__instance.attachableOutcomeEffect.DescriptionForRitualValidated(__instance));
                }
                if (stringBuilder.Length > 0)
                {
                    tmpCompsDesc.AppendLine();
                    tmpCompsDesc.AppendInNewLine(stringBuilder.ToString());
                }
            }
            __result = tmpCompsDesc.ToString();
            return false;
        }
    }

    [HarmonyPatch(typeof(IncidentWorker))]
    [HarmonyPatch("TryExecute")]
    public class Patch_IncidentWorker_TryExecute
    {
        public static void Postfix(IncidentParms parms, IncidentWorker __instance, ref bool __result)
        {
            if (__result)
            {

                foreach (Ideo ideo in Find.FactionManager.OfPlayer.ideos.AllIdeos)
                {
                    foreach (Precept p in ideo.PreceptsListForReading)
                    {

                        if (p is Precept_Ritual ritual)
                        {
                            foreach (RitualObligationTrigger rot in ritual.obligationTriggers)
                            {
                                if (rot is RitualObligationTrigger_Event rotEvent)
                                {
                                    rotEvent.Notify_Event(__instance.def);
                                }
                            }
                        }
                    }
                }

            }
        }
    }

    [HarmonyPatch(typeof(RitualOutcomeEffectWorker))]
    [HarmonyPatch("MakeMemory")]
    public class Patch_RitualOutcomeEffectWorker_MakeMemory
    {
        public static void Prefix(Pawn p, LordJob_Ritual ritual, ThoughtDef overrideDef = null)
        {
            int ii = 0;
            bool found = false;
            if(ritual == null || ritual.Ritual == null
                || ritual.Ritual.activeObligations == null || ritual.Ritual.sourcePattern == null
                || ritual.Ritual.activeObligations.Count == 0)
            {
                return;
            }
            foreach(RitualObligation r in ritual.Ritual.activeObligations)
            {
                foreach(RitualObligationTriggerProperties t in ritual.Ritual.sourcePattern.ritualObligationTriggers)
                {
                    if(t is RitualObligationTrigger_EventProperties rotEvent)
                    {
                        // I'm just going to go ahead and assume that if this ritual had an event trigger and the ritual occurred, we can remove at least one of the corresponding obligations
                        ritual.Ritual.activeObligations.RemoveAt(ii);
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    break;
                }
                ii++;
            }
        }
    }

    // Command_Ritual
    // DrawIcon
    [HarmonyPatch(typeof(Command_Ritual))]
    [HarmonyPatch("DrawIcon")]
    public class Patch_Command_Ritual_DrawIcon
    {
        private static void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms, Command_Ritual instance)
        {
            Texture2D badTex = instance.icon;
            if (badTex == null)
            {
                badTex = BaseContent.BadTex;
            }
            rect.position += new Vector2(instance.iconOffset.x * rect.size.x, instance.iconOffset.y * rect.size.y);
            if (!instance.disabled || parms.lowLight)
            {
                GUI.color = instance.IconDrawColor;
            }
            else
            {
                GUI.color = instance.IconDrawColor.SaturationChanged(0f);
            }
            if (parms.lowLight)
            {
                GUI.color = GUI.color.ToTransparent(0.6f);
            }
            Widgets.DrawTextureFitted(rect, badTex, instance.iconDrawScale * 0.85f, instance.iconProportions, instance.iconTexCoords, instance.iconAngle, buttonMat);
            GUI.color = Color.white;
        }
        public static bool Prefix(Command_Ritual __instance, Rect rect, Material buttonMat, GizmoRenderParms parms, Precept_Ritual ___ritual, Texture2D ___CooldownBarTex, IntVec2 ___PenaltyIconSize)
        {
            int coolDownDays = 20;
            Precept_Ritual_Custom customPreceptRitual = ___ritual.def.GetModExtension<Precept_Ritual_Custom>();

            //Log.Message("getting the real cooldown for " + __instance.def.defName);

            if (customPreceptRitual != null)
            {
                //Log.Message("customCooldown: " + customPreceptRitual.coolDownDays + " for ritual " + __instance.def.label);
                if (customPreceptRitual.coolDownDays != coolDownDays)
                {
                    coolDownDays = customPreceptRitual.coolDownDays;
                }
                else // so far the only reason to override this is for a non-default cooldown, so if it is the default cooldown just go back to the original
                {
                    return true;
                }
            }

            // some day this may be cleaned up but more likely 20 years from now after I've been killed in a freak skydiving accident, some young modder will come across it and say 'wtf'
            Texture2D ___PenaltyArrowTex = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/QualityPenalty");
            float cooldownTicks = 60000f * coolDownDays;

            Patch_Command_Ritual_DrawIcon.DrawIcon(rect, buttonMat, parms, __instance);
            if (___ritual.RepeatPenaltyActive)
            {
                float value = Mathf.InverseLerp(cooldownTicks, 0f, ___ritual.TicksSinceLastPerformed);
                Widgets.FillableBar(rect.ContractedBy(1f), Mathf.Clamp01(value), ___CooldownBarTex, null, doBorder: false);
                Verse.Text.Font = GameFont.Tiny;
                Verse.Text.Anchor = UnityEngine.TextAnchor.UpperCenter;
                float num = (float)(cooldownTicks - ___ritual.TicksSinceLastPerformed) / 60000f;
                Widgets.Label(label: "PeriodDays".Translate((!(num >= 1f)) ? ((float)(int)(num * 10f) / 10f) : ((float)Mathf.RoundToInt(num))), rect: rect);
                Text.Anchor = TextAnchor.UpperLeft;
                GUI.DrawTexture(new Rect(rect.xMax - (float)___PenaltyIconSize.x, rect.yMin + 4f, ___PenaltyIconSize.x, ___PenaltyIconSize.z), ___PenaltyArrowTex);
            }

            return false;
        }
    }

    /*[HarmonyPatch(typeof(Precept_Ritual), "get_RepeatPenaltyDurationDays")]
    internal class Patch_RepeatPenaltyDurationDays
    {
        private static bool Prefix(Precept_Ritual __instance, ref int __result)
        {
            int coolDownDays = 20;
            Precept_Ritual_Custom customPreceptRitual = __instance.def.GetModExtension<Precept_Ritual_Custom>();
            if (customPreceptRitual != null)
            {
                if (customPreceptRitual.coolDownDays != 20)
                {
                    coolDownDays = customPreceptRitual.coolDownDays;
                }
            }
            __result = coolDownDays;
            return false;
        }
    }*/
}
