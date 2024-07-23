using System;
using BepInEx;
using EntityStates;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using TrovianSkills.EntityStates;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TrovianSkills
{
    [BepInDependency(R2API.ContentManagement.R2APIContentManager.PluginGUID)]
    [BepInDependency(LanguageAPI.PluginGUID)]

    [BepInPlugin(
        "com.MyName.IHerebyGrantPermissionToDeprecateMyModFromThunderstoreBecauseIHaveNotChangedTheName",
        "IHerebyGrantPermissionToDeprecateMyModFromThunderstoreBecauseIHaveNotChangedTheName",
        "1.0.0")]
    public class CustomSkillTutorial : BaseUnityPlugin
    {
        private AssetBundleCreateRequest trovianAssets;
        private Sprite chargedIcon;
        private Sprite jumpIcon;
        private Sprite overchargeIcon;
        private Sprite stallIcon;
        public void Awake()
        {
            this.trovianAssets = AssetBundle.LoadFromFileAsync(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.Info.Location),"AssetBundles", "trovianskills"));
            this.chargedIcon = this.trovianAssets.assetBundle.LoadAssetAsync<Sprite>("blast").asset as Sprite;
            this.jumpIcon = this.trovianAssets.assetBundle.LoadAssetAsync<Sprite>("jump").asset as Sprite;
            this.overchargeIcon = this.trovianAssets.assetBundle.LoadAssetAsync<Sprite>("overcharge").asset as Sprite;
            this.stallIcon = this.trovianAssets.assetBundle.LoadAssetAsync<Sprite>("stall").asset as Sprite;


            GameObject commandoBodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoBody.prefab").WaitForCompletion();
            LanguageAPI.Add("COMMANDO_SECONDARY_CHARGEDSHOT_NAME", "Charged Shot");
            LanguageAPI.Add("COMMANDO_SECONDARY_CHARGEDSHOT_DESCRIPTION", $"Fire a single charged shot for <style=cIsDamage>700% damage</style>.");
            SkillDef chargedShotDef = ScriptableObject.CreateInstance<SkillDef>();
            chargedShotDef.activationState = new SerializableEntityStateType(typeof(TrovianSkills.EntityStates.Blast));
            chargedShotDef.activationStateMachineName = "Weapon";
            chargedShotDef.baseMaxStock = 1;
            chargedShotDef.baseRechargeInterval = 5f;
            chargedShotDef.beginSkillCooldownOnSkillEnd = true;
            chargedShotDef.canceledFromSprinting = false;
            chargedShotDef.cancelSprintingOnActivation = true;
            chargedShotDef.fullRestockOnAssign = true;
            chargedShotDef.interruptPriority = InterruptPriority.Skill;
            chargedShotDef.isCombatSkill = true;
            chargedShotDef.mustKeyPress = true;
            chargedShotDef.rechargeStock = 1;
            chargedShotDef.requiredStock = 1;
            chargedShotDef.stockToConsume = 1;
            chargedShotDef.icon = chargedIcon;
            chargedShotDef.skillDescriptionToken = "COMMANDO_SECONDARY_CHARGEDSHOT_DESCRIPTION";
            chargedShotDef.skillName = "COMMANDO_SECONDARY_CHARGEDSHOT_NAME";
            chargedShotDef.skillNameToken = "COMMANDO_SECONDARY_CHARGEDSHOT_NAME";
            ContentAddition.AddSkillDef(chargedShotDef);
            SkillLocator skillLocator = commandoBodyPrefab.GetComponent<SkillLocator>();
            SkillFamily skillFamily = skillLocator.secondary.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = chargedShotDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(chargedShotDef.skillNameToken, false, null)
            };


            LanguageAPI.Add("COMMANDO_UTILITY_JUMP_NAME", "Blast Jump");
            LanguageAPI.Add("COMMANDO_UTILITY_JUMP_DESCRIPTION", $"Fly into the sky.");
            SkillDef jumpUpDef = ScriptableObject.CreateInstance<SkillDef>();
            jumpUpDef.activationState = new SerializableEntityStateType(typeof(TrovianSkills.EntityStates.BlastUp));
            jumpUpDef.activationStateMachineName = "Weapon";
            jumpUpDef.baseMaxStock = 1;
            jumpUpDef.baseRechargeInterval = 3f;
            jumpUpDef.beginSkillCooldownOnSkillEnd = true;
            jumpUpDef.canceledFromSprinting = false;
            jumpUpDef.cancelSprintingOnActivation = false;
            jumpUpDef.fullRestockOnAssign = true;
            jumpUpDef.interruptPriority = InterruptPriority.PrioritySkill;
            jumpUpDef.isCombatSkill = false;
            jumpUpDef.mustKeyPress = true;
            jumpUpDef.rechargeStock = 1;
            jumpUpDef.requiredStock = 1;
            jumpUpDef.stockToConsume = 1;
            jumpUpDef.icon = jumpIcon;
            jumpUpDef.skillDescriptionToken = "COMMANDO_UTILITY_JUMP_DESCRIPTION";
            jumpUpDef.skillName = "COMMANDO_UTILITY_JUMP_NAME";
            jumpUpDef.skillNameToken = "COMMANDO_UTILITY_JUMP_NAME";
            ContentAddition.AddSkillDef(jumpUpDef);
            skillLocator = commandoBodyPrefab.GetComponent<SkillLocator>();
            skillFamily = skillLocator.utility.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = jumpUpDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(jumpUpDef.skillNameToken, false, null)
            };


            LanguageAPI.Add("COMMANDO_SPECIAL_OVERCHARGED_NAME", "Overcharged");
            LanguageAPI.Add("COMMANDO_SPECIAL_OVERCHARGED_DESCRIPTION", $"For 5 seconds, all attacks are overcharge shots.");
            SkillDef overchargeDef = ScriptableObject.CreateInstance<SkillDef>();
            overchargeDef.activationState = new SerializableEntityStateType(typeof(TrovianSkills.EntityStates.Overcharge));
            overchargeDef.activationStateMachineName = "Weapon";
            overchargeDef.baseMaxStock = 1;
            overchargeDef.baseRechargeInterval = 15f;
            overchargeDef.beginSkillCooldownOnSkillEnd = true;
            overchargeDef.canceledFromSprinting = false;
            overchargeDef.cancelSprintingOnActivation = true;
            overchargeDef.fullRestockOnAssign = true;
            overchargeDef.interruptPriority = InterruptPriority.Skill;
            overchargeDef.isCombatSkill = true;
            overchargeDef.mustKeyPress = false;
            overchargeDef.rechargeStock = 1;
            overchargeDef.requiredStock = 1;
            overchargeDef.stockToConsume = 1;
            overchargeDef.icon = overchargeIcon;
            overchargeDef.skillDescriptionToken = "COMMANDO_SPECIAL_OVERCHARGED_DESCRIPTION";
            overchargeDef.skillName = "COMMANDO_SPECIAL_OVERCHARGED_NAME";
            overchargeDef.skillNameToken = "COMMANDO_SPECIAL_OVERCHARGED_NAME";
            ContentAddition.AddSkillDef(overchargeDef);
            skillLocator = commandoBodyPrefab.GetComponent<SkillLocator>();
            skillFamily = skillLocator.special.skillFamily;
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = overchargeDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(overchargeDef.skillNameToken, false, null)
            };


            LanguageAPI.Add("COMMANDO_PASSIVE_LEECH_NAME", "Velocity Leecher");
            LanguageAPI.Add("COMMANDO_PASSIVE_LEECH_DESCRIPTION", $"Slow your descent upon damaging an enemy.");
            PassiveItemSkillDef vleechDef = ScriptableObject.CreateInstance<PassiveItemSkillDef>();
            vleechDef.passiveItem = ExamplePlugin.myItemDef;
            vleechDef.baseMaxStock = 1;
            vleechDef.baseRechargeInterval = 0f;
            vleechDef.beginSkillCooldownOnSkillEnd = true;
            vleechDef.canceledFromSprinting = false;
            vleechDef.cancelSprintingOnActivation = false;
            vleechDef.fullRestockOnAssign = true;
            vleechDef.interruptPriority = InterruptPriority.Any;
            vleechDef.isCombatSkill = false;
            vleechDef.mustKeyPress = false;
            vleechDef.rechargeStock = 1;
            vleechDef.requiredStock = 1;
            vleechDef.stockToConsume = 1;
            vleechDef.icon = stallIcon;
            vleechDef.skillDescriptionToken = "COMMANDO_PASSIVE_LEECH_DESCRIPTION";
            vleechDef.skillName = "COMMANDO_PASSIVE_LEECH_NAME";
            vleechDef.skillNameToken = "COMMANDO_PASSIVE_LEECH_NAME";
            ContentAddition.AddSkillDef(vleechDef);
            GenericSkill uniqueSkill = commandoBodyPrefab.AddComponent<GenericSkill>();
            uniqueSkill.skillName = commandoBodyPrefab.name + "UniqueSkill";
            SkillFamily uniqueFamily = ScriptableObject.CreateInstance<SkillFamily>();
            (uniqueFamily as ScriptableObject).name = commandoBodyPrefab.name + "UniqueFamily";
            uniqueSkill.SetFieldValue("_skillFamily", uniqueFamily);
            uniqueFamily.variants = new SkillFamily.Variant[0];
            Array.Resize(ref uniqueFamily.variants, uniqueFamily.variants.Length + 1);
            uniqueFamily.variants[uniqueFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = vleechDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(vleechDef.skillNameToken, false, null)
            };
            ContentAddition.AddSkillFamily(uniqueFamily);
        }
    }
}