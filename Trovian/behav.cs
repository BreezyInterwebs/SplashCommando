using EntityStates;
using RoR2;
using BepInEx;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;
using R2API;

[assembly: HG.Reflection.SearchableAttribute.OptIn]
namespace TrovianSkills.EntityStates
{
    public class Blast : BaseState
    {
        public static GameObject viendbomb = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterBigProjectile.prefab").WaitForCompletion();
        public static GameObject nanobomb = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageLightningBombProjectile.prefab").WaitForCompletion();
        public static int counter = 0;

        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            FireProjectileInfo fpiVB = new FireProjectileInfo
            {
                projectilePrefab = viendbomb,
                position = base.characterBody.corePosition,
                rotation = Quaternion.LookRotation(aimRay.direction),
                owner = this.gameObject,
                damage = 7 * base.characterBody.damage,
                force = 0f,
                crit = base.RollCrit(),
            };
            FireProjectileInfo fpiNB = new FireProjectileInfo
            {
                projectilePrefab = nanobomb,
                position = base.characterBody.corePosition,
                rotation = Quaternion.LookRotation(aimRay.direction),
                owner = this.gameObject,
                damage = 7 * base.characterBody.damage,
                force = 0f,
                crit = base.RollCrit(),
            };
            if (counter >= 100)
            {
                counter = 0;
            }
            if (counter % 2 == 0)
            {
                base.PlayAnimation("Gesture Additive, Right", "FirePistol, Right");
                Util.PlaySound("Play_huntress_R_snipe_shoot", base.gameObject);
                ProjectileManager.instance.FireProjectile(fpiVB);
            } else
            {
                base.PlayAnimation("Gesture Additive, Left", "FirePistol, Left");
                Util.PlaySound("Play_huntress_R_snipe_shoot", base.gameObject);
                ProjectileManager.instance.FireProjectile(fpiNB);
            }
            counter++;
        }
        public override void OnExit()
        {
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= 0.1f && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
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
                characterMotor.velocity.y += 20f;
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

        public override void OnExit()
        {
            base.OnExit();
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }
    }
    public class Overcharge : BaseState
    {
        public static float BaseDuration = 0f;
        public static float BaseDelayDuration = 0.0f;
        public static float DamageCoefficient = 7f;
        public float fireCountdown;
        public static GameObject viendbomb = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterBigProjectile.prefab").WaitForCompletion();
        public static GameObject nanobomb = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageLightningBombProjectile.prefab").WaitForCompletion();
        public static int counter = 0;
        public override void OnEnter()
        {
            base.OnEnter();
            Util.PlaySound("Play_loader_shift_activate", base.gameObject);
        }
        public override void OnExit()
        {
            counter = 0;
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            Ray aimRay = base.GetAimRay();
            FireProjectileInfo fpiVB = new FireProjectileInfo
            {
                projectilePrefab = viendbomb,
                position = base.characterBody.corePosition,
                rotation = Quaternion.LookRotation(aimRay.direction),
                owner = this.gameObject,
                damage = 7 * base.characterBody.damage,
                force = 0f,
                crit = base.RollCrit(),
            };
            FireProjectileInfo fpiNB = new FireProjectileInfo
            {
                projectilePrefab = nanobomb,
                position = base.characterBody.corePosition,
                rotation = Quaternion.LookRotation(aimRay.direction),
                owner = this.gameObject,
                damage = 7 * base.characterBody.damage,
                force = 0f,
                crit = base.RollCrit(),
            };
            base.FixedUpdate();
            this.fireCountdown -= Time.fixedDeltaTime;
            if (this.fireCountdown <= 0f)
            {
                this.fireCountdown = 0.40f / base.attackSpeedStat;
                if (counter % 3 == 0)
                {
                    base.PlayAnimation("Gesture Additive, Left", "FirePistol, Left");
                    ProjectileManager.instance.FireProjectile(fpiNB);
                }
                else
                {
                    base.PlayAnimation("Gesture Additive, Right", "FirePistol, Right");
                    Util.PlaySound("Play_huntress_R_snipe_shoot", base.gameObject);
                    ProjectileManager.instance.FireProjectile(fpiVB);
                }
                counter++;
            }
            if (base.fixedAge >= 5 && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
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
            myItemDef.name = "VelocityLeecher";
            myItemDef.nameToken = "EXAMPLE_VLEECH_NAME";
            myItemDef.pickupToken = "EXAMPLE_VLEECH_PICKUP";
            myItemDef.descriptionToken = "EXAMPLE_VLEECH_DESC";
            myItemDef.loreToken = "EXAMPLE_VLEECH_LORE";
            myItemDef._itemTierDef = Addressables.LoadAssetAsync<ItemTierDef>("RoR2/Base/Common/NoTierDef.asset").WaitForCompletion();
#pragma warning restore Publicizer001
            myItemDef.pickupIconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
            myItemDef.pickupModelPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();
            myItemDef.canRemove = false;
            myItemDef.hidden = true;
            var displayRules = new ItemDisplayRuleDict(null);
            ItemAPI.Add(new CustomItem(myItemDef, displayRules));
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
        }
        private void GlobalEventManager_onServerDamageDealt(DamageReport report)
        {
            var attackerCharacterBody = report.attackerBody;
            if (attackerCharacterBody.inventory)
            {
                var garbCount = attackerCharacterBody.inventory.GetItemCount(myItemDef.itemIndex);
                if (garbCount > 0)
                {
                    if (attackerCharacterBody.characterMotor.velocity.y < 0)
                    {
                        attackerCharacterBody.characterMotor.velocity.y = -0.05f;
                    }
                }
            }
        }
        private void Update()
        {
            return;
        }
    }
}
