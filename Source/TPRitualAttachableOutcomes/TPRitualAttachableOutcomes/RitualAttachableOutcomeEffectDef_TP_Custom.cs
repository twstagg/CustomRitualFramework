﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace TPRitualAttachableOutcomes
{
    [StaticConstructorOnStartup]
    class RitualAttachableOutcomeEffectDef_TP_Custom : DefModExtension
    {
        public List<int> triggerPositivityIndex =  new List<int>();
        public int cooldownDays = 20;
        public List<string> hediffToAdd;
        public float hediffSeverity = 0f;
        public string bodyPart;
        public List<string> hediffToRemove;
        public string thought;
        public string inspiration;
        public List<string> appliesTo;
        public bool appliesToRandom = false;
        public bool invertApply = false;
        public bool forEachPawn = false;
        public bool onlyPositiveOutcomes = true;
        public string item;
        public IntRange baseAmount;
        public IntRange amountPerPawn;
        public bool spawnNearRitual = false;
        public string weather;
        public string abilityToAdd;
        public string letterLabel;
        public string letterText;
        public string letterType = "positive";
        public bool randomFromNode = false;
        public float weight = 1f;
        public string incident = "";
        public IncidentParmsCustom incidentParms = new IncidentParmsCustom();
        public List<RitualAttachableOutcomeEffectDef_TP_Custom_Node> node;

        public void ExposeData()
        {
            Scribe_Collections.Look(ref triggerPositivityIndex, "triggerPositivityIndex");
            Scribe_Values.Look(ref  cooldownDays, "cooldownDays");
            Scribe_Collections.Look(ref hediffToAdd, "hediffToAdd");
            Scribe_Values.Look(ref hediffSeverity, "hediffSeverity");
            Scribe_Values.Look(ref bodyPart, "bodyPart");
            Scribe_Collections.Look(ref hediffToRemove, "hediffToRemove");
            Scribe_Values.Look(ref thought, "thought");
            Scribe_Values.Look(ref inspiration, "inspiration");
            Scribe_Collections.Look(ref appliesTo, "appliesTo");
            Scribe_Values.Look(ref appliesToRandom, "appliesToRandom");
            Scribe_Values.Look(ref invertApply, "invertApply");
            Scribe_Values.Look(ref forEachPawn, "forEachPawn");
            Scribe_Values.Look(ref onlyPositiveOutcomes, "onlyPositiveOutcomes");
            Scribe_Values.Look(ref item, "item");
            Scribe_Values.Look(ref baseAmount, "baseAmount");
            Scribe_Values.Look(ref amountPerPawn, "amountPerPawn");
            Scribe_Values.Look(ref spawnNearRitual, "spawnNearRitual");
            Scribe_Values.Look(ref weather, "weather");
            Scribe_Values.Look(ref abilityToAdd, "abilityToAdd");
            Scribe_Values.Look(ref letterLabel, "letterLabel");
            Scribe_Values.Look(ref letterText, "letterText");
            Scribe_Values.Look(ref letterType, "letterType");
            Scribe_Values.Look(ref randomFromNode, "randomFromNode");
            Scribe_Values.Look(ref weight, "weight");
            Scribe_Values.Look(ref incident, "incident");
            Scribe_Values.Look(ref incidentParms, "incidentParms");
            Scribe_Collections.Look(ref node, "node");
        }
    }

    [StaticConstructorOnStartup]
    class RitualAttachableOutcomeEffectDef_TP_Custom_Node : RitualAttachableOutcomeEffectDef_TP_Custom
    {

    }
}
