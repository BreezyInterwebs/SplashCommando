using System;
using BepInEx;
using EntityStates;
using R2API;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CustomSkillsTutorial
{
    [BepInDependency(R2API.ContentManagement.R2APIContentManager.PluginGUID)]
    [BepInDependency(LanguageAPI.PluginGUID)]

    [BepInPlugin(
        "com.MyName.IHerebyGrantPermissionToDeprecateMyModFromThunderstoreBecauseIHaveNotChangedTheName",
        "IHerebyGrantPermissionToDeprecateMyModFromThunderstoreBecauseIHaveNotChangedTheName",
        "1.0.0")]
    public class CustomSkillTutorial : BaseUnityPlugin
    {
        public void Awake()
        {
            // First we must load our survivor's Body prefab. For this tutorial, we are making a skill for Commando
            // If you would like to load a different survivor, you can find the key for their Body prefab at the following link
            // https://xiaoxiao921.github.io/GithubActionCacheTest/assetPathsDump.html
            GameObject commandoBodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoBody.prefab").WaitForCompletion();

            // We use LanguageAPI to add strings to the game, in the form of tokens
            // Please note that it is instead recommended that you use a language file.
            // More info in https://risk-of-thunder.github.io/R2Wiki/Mod-Creation/Assets/Localization/
            LanguageAPI.Add("COMMANDO_SECONDARY_CHARGEDSHOT_NAME", "Charged Shot");
            LanguageAPI.Add("COMMANDO_SECONDARY_CHARGEDSHOT_DESCRIPTION", $"Fire an overcharge blast for <style=cIsDamage>700% damage</style>.");

            // Now we must create a SkillDef
            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();

            //Check step 2 for the code of the CustomSkillsTutorial.MyEntityStates.SimpleBulletAttack class
            mySkillDef.activationState = new SerializableEntityStateType(typeof(CustomSkillsTutorial.MyEntityStates.Blast));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 5f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.cancelSprintingOnActivation = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.stockToConsume = 1;
            // For the skill icon, you will have to load a Sprite from your own AssetBundle
            mySkillDef.icon = null;
            mySkillDef.skillDescriptionToken = "COMMANDO_SECONDARY_CHARGEDSHOT_DESCRIPTION";
            mySkillDef.skillName = "COMMANDO_SECONDARY_CHARGEDSHOT_NAME";
            mySkillDef.skillNameToken = "COMMANDO_SECONDARY_CHARGEDSHOT_NAME";

            // This adds our skilldef. If you don't do this, the skill will not work.
            ContentAddition.AddSkillDef(mySkillDef);

            // Now we add our skill to one of the survivor's skill families
            // You can change component.primary to component.secondary, component.utility and component.special
            SkillLocator skillLocator = commandoBodyPrefab.GetComponent<SkillLocator>();
            SkillFamily skillFamily = skillLocator.secondary.skillFamily;

            // If this is an alternate skill, use this code.
            // Here, we add our skill as a variant to the existing Skill Family.
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            //GameObject commandoBodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoBody.prefab").WaitForCompletion();
            LanguageAPI.Add("COMMANDO_UTILITY_JUMP_NAME", "Blast Jump");
            LanguageAPI.Add("COMMANDO_UTILITY_JUMP_DESCRIPTION", $"Fly into the sky, dealing <style=cIsDamage>300% damage</style> where you stand.");
            SkillDef jumpUpDef = ScriptableObject.CreateInstance<SkillDef>();

            jumpUpDef.activationState = new SerializableEntityStateType(typeof(CustomSkillsTutorial.MyEntityStates.BlastUp));
            //jumpUpDef.activationStateMachineName = "Weapon";
            jumpUpDef.baseMaxStock = 1;
            jumpUpDef.baseRechargeInterval = 3f;
            jumpUpDef.beginSkillCooldownOnSkillEnd = true;
            jumpUpDef.canceledFromSprinting = false;
            jumpUpDef.cancelSprintingOnActivation = false;
            jumpUpDef.fullRestockOnAssign = true;
            jumpUpDef.interruptPriority = InterruptPriority.Skill;
            jumpUpDef.isCombatSkill = false;
            jumpUpDef.mustKeyPress = true;
            jumpUpDef.rechargeStock = 1;
            jumpUpDef.requiredStock = 1;
            jumpUpDef.stockToConsume = 1;
            // For the skill icon, you will have to load a Sprite from your own AssetBundle
            jumpUpDef.icon = null;
            jumpUpDef.skillDescriptionToken = "COMMANDO_UTILITY_JUMP_DESCRIPTION";
            jumpUpDef.skillName = "COMMANDO_UTILITY_JUMP_NAME";
            jumpUpDef.skillNameToken = "COMMANDO_UTILITY_JUMP_NAME";

            // This adds our skilldef. If you don't do this, the skill will not work.
            ContentAddition.AddSkillDef(jumpUpDef);

            // Now we add our skill to one of the survivor's skill families
            // You can change component.primary to component.secondary, component.utility and component.special
            skillLocator = commandoBodyPrefab.GetComponent<SkillLocator>();
            skillFamily = skillLocator.utility.skillFamily;

            // If this is an alternate skill, use this code.
            // Here, we add our skill as a variant to the existing Skill Family.
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = jumpUpDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(jumpUpDef.skillNameToken, false, null)
            };
        }
    }
}