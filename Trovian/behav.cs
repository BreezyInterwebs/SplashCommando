using EntityStates;
using RoR2;
using RoR2.Skills;
using BepInEx;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;
using R2API;
using UnityEngine.Networking;
using JetBrains.Annotations;

//Since we are using effects from Commando's Barrage skill, we will also be using the associated namespace
//You can also use Addressables or LegacyResourcesAPI to load whichever effects you like
[assembly: HG.Reflection.SearchableAttribute.OptIn]
namespace TrovianSkills.EntityStates
{

    public class Blast : GenericProjectileBaseState
    {
        public float BaseDuration = 0f;
        public static float BaseDelayDuration = 0.0f;
        public float DamageCoefficient = 7f;
        GameObject viendbomb = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterBigProjectile.prefab").WaitForCompletion();
        public override void OnEnter()
        {
            projectilePrefab = viendbomb;
            baseDuration = BaseDuration;
            baseDelayBeforeFiringProjectile = BaseDelayDuration;
            damageCoefficient = DamageCoefficient;
            force = 80f;
            bloom = 10;
            base.OnEnter();
            Util.PlaySound("Play_huntress_R_snipe_shoot", base.gameObject);
        }
        public override void OnExit()
        {
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }

    public class BlastUp : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Util.PlaySound("Play_huntress_R_snipe_shoot", base.gameObject);
            characterMotor.Motor.ForceUnground();
            if (characterMotor.velocity.y > 0)
            {
                characterMotor.velocity.y += 30f;
            } else
            {
                characterMotor.velocity.y = 30f;
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= 0.3f && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        // Token: 0x06000BC5 RID: 3013 RVA: 0x00030F0F File Offset: 0x0002F10F
        public override void OnExit()
        {
            base.OnExit();
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
    public class Overcharge : BaseState
    {
        public float Duration = 5f;
        private GenericSkill primarySkillSlot;
        SkillDef test;
        public void Awake()
        {
        }
        public override void OnEnter()
        {
            Console.print("Special Enter");
            if (test == null)
            {
                GameObject commandoBodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoBody.prefab").WaitForCompletion();
                SkillDef primaryOverrideSkillDef = ScriptableObject.CreateInstance<SkillDef>();
                primaryOverrideSkillDef.activationState = new SerializableEntityStateType(typeof(TrovianSkills.EntityStates.OverrideM1));
                primaryOverrideSkillDef.activationStateMachineName = "Weapon";
                primaryOverrideSkillDef.baseMaxStock = 1;
                primaryOverrideSkillDef.baseRechargeInterval = 0f;
                primaryOverrideSkillDef.beginSkillCooldownOnSkillEnd = true;
                primaryOverrideSkillDef.canceledFromSprinting = false;
                primaryOverrideSkillDef.cancelSprintingOnActivation = true;
                primaryOverrideSkillDef.fullRestockOnAssign = true;
                primaryOverrideSkillDef.interruptPriority = InterruptPriority.Any;
                primaryOverrideSkillDef.isCombatSkill = true;
                primaryOverrideSkillDef.mustKeyPress = false;
                primaryOverrideSkillDef.rechargeStock = 1;
                primaryOverrideSkillDef.requiredStock = 1;
                primaryOverrideSkillDef.stockToConsume = 1;
                primaryOverrideSkillDef.skillDescriptionToken = "COMMANDO_SPECIAL_OVERCHARGED_DESCRIPTION";
                primaryOverrideSkillDef.skillName = "COMMANDO_SPECIAL_OVERCHARGED_NAME";
                primaryOverrideSkillDef.skillNameToken = "COMMANDO_SPECIAL_OVERCHARGED_NAME";
                primaryOverrideSkillDef.icon = null;
                ContentAddition.AddSkillDef(primaryOverrideSkillDef);
                SkillLocator skillLocator = commandoBodyPrefab.GetComponent<SkillLocator>();
                SkillFamily skillFamily = skillLocator.primary.skillFamily;
                test = primaryOverrideSkillDef;
            }
            else
            {
            }
            base.OnEnter();
            if (base.isAuthority)
            {
                base.skillLocator.primary.SetSkillOverride(this, test, GenericSkill.SkillOverridePriority.Default);
            }
        }
        public override void OnExit()
        {
            Console.print("Special Exit");
            if (base.isAuthority)
            {
                base.skillLocator.primary.UnsetSkillOverride(this, test, GenericSkill.SkillOverridePriority.Default);
            }
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            Console.print("Special Update");
            base.FixedUpdate();
            /*            if (base.fixedAge >= this.Duration && base.isAuthority)
                        {
                            this.outer.SetNextStateToMain();
                            return;
                        }*/
        }
    }
    public class OverrideM1 : BaseState
    {
        public static float BaseDuration = 0f;
        public static float BaseDelayDuration = 0.0f;
        public static float DamageCoefficient = 7f;
        public float fireCountdown;
        public static GameObject viendbomb = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterBigProjectile.prefab").WaitForCompletion();
        public override void OnEnter()
        {
            base.OnEnter();
        }
        public override void OnExit()
        {
            Console.print("Charge Exit");
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            Console.print("Charge Update");
            base.FixedUpdate();
            this.fireCountdown -= Time.fixedDeltaTime;
            if (this.fireCountdown <= 0f)
            {
                /*                GenericProjectileBaseState gpbs = new GenericProjectileBaseState()
                                {
                                    projectilePrefab = viendbomb,
                                    baseDuration = BaseDuration,
                                    baseDelayBeforeFiringProjectile = BaseDelayDuration,
                                    damageCoefficient = DamageCoefficient,
                                };*/
                this.fireCountdown = 0.25f / base.attackSpeedStat;
                Util.PlaySound("Play_huntress_R_snipe_shoot", base.gameObject);
                Ray aimRay = base.GetAimRay();
                FireProjectileInfo fpi = new FireProjectileInfo
                {
                    projectilePrefab = viendbomb,
                    position = base.characterBody.corePosition,
                    rotation = Quaternion.LookRotation(aimRay.direction),
                    owner = this.gameObject,
                    damage = 7 * base.characterBody.baseDamage,
                    force = 0f,
                    crit = false,
                };
                ProjectileManager.instance.FireProjectile(fpi);

            }
            if (base.fixedAge >= 5 && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            Console.print("Charge interrupt check");
            return InterruptPriority.Skill;
        }
    }

    [BepInDependency(LanguageAPI.PluginGUID)]
    [BepInDependency(ItemAPI.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class ExamplePlugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "AuthorName";
        public const string PluginName = "ExamplePlugin";
        public const string PluginVersion = "1.0.0";

        public static ItemDef myItemDef = ScriptableObject.CreateInstance<ItemDef>();
        public void Awake()
        {
            Console.print("start1");
            myItemDef.name = "VelocityLeecher";
            myItemDef.nameToken = "EXAMPLE_VLEECH_NAME";
            myItemDef.pickupToken = "EXAMPLE_VLEECH_PICKUP";
            myItemDef.descriptionToken = "EXAMPLE_VLEECH_DESC";
            myItemDef.loreToken = "EXAMPLE_VLEECH_LORE";
            Console.print("start2");

            // The tier determines what rarity the item is:
            // Tier1=white, Tier2=green, Tier3=red, Lunar=Lunar, Boss=yellow,
            // and finally NoTier is generally used for helper items, like the tonic affliction
#pragma warning disable Publicizer001 // Accessing a member that was not originally public. Here we ignore this warning because with how this example is setup we are forced to do this
            myItemDef._itemTierDef = Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/Tier1Def.asset").WaitForCompletion();
#pragma warning restore Publicizer001
            // Instead of loading the itemtierdef directly, you can also do this like below as a workaround
            // myItemDef.deprecatedTier = ItemTier.Tier2;

            // You can create your own icons and prefabs through assetbundles, but to keep this boilerplate brief, we'll be using question marks.
            myItemDef.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
            myItemDef.pickupModelPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();

            // Can remove determines
            // if a shrine of order,
            // or a printer can take this item,
            // generally true, except for NoTier items.
            myItemDef.canRemove = true;

            // Hidden means that there will be no pickup notification,
            // and it won't appear in the inventory at the top of the screen.
            // This is useful for certain noTier helper items, such as the DrizzlePlayerHelper.
            myItemDef.hidden = false;

            // You can add your own display rules here,
            // where the first argument passed are the default display rules:
            // the ones used when no specific display rules for a character are found.
            // For this example, we are omitting them,
            // as they are quite a pain to set up without tools like https://thunderstore.io/package/KingEnderBrine/ItemDisplayPlacementHelper/
            var displayRules = new ItemDisplayRuleDict(null);

            // Then finally add it to R2API
            ItemAPI.Add(new CustomItem(myItemDef, displayRules));

            // But now we have defined an item, but it doesn't do anything yet. So we'll need to define that ourselves.
            // GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            Console.print("item added");
        }
        private void GlobalEventManager_onServerDamageDealt(DamageReport report)
        {
            var attackerCharacterBody = report.attackerBody;

            // We need an inventory to do check for our item
            Console.print("inventory check");
            if (attackerCharacterBody.inventory)
            {
                // Store the amount of our item we have
                var garbCount = attackerCharacterBody.inventory.GetItemCount(myItemDef.itemIndex);
                if (garbCount > 0)
                {
                    if (attackerCharacterBody.characterMotor.velocity.y < 0)
                    {
                        attackerCharacterBody.characterMotor.velocity.y = -0.05f;
                    }
                    Console.print("set velocity");
                }
            }
        }

        // The Update() method is run on every frame of the game.
        private void Update()
        {
            return;
        }
    }
}
