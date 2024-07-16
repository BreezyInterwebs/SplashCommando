using EntityStates;
using RoR2;
using RoR2.Skills;
using BepInEx;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;
using R2API;
using UnityEngine.Networking;

//Since we are using effects from Commando's Barrage skill, we will also be using the associated namespace
//You can also use Addressables or LegacyResourcesAPI to load whichever effects you like

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
            characterMotor.velocity.y = 30f;
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
            } else
            {
            }
            base.OnEnter();
            this.primarySkillSlot = (base.skillLocator ? base.skillLocator.primary : null);
            this.primarySkillSlot.SetSkillOverride(this, test, GenericSkill.SkillOverridePriority.Contextual);
        }
        public override void OnExit()
        {
            this.primarySkillSlot.UnsetSkillOverride(this, test, GenericSkill.SkillOverridePriority.Contextual);
            base.OnExit();
            Console.print("exited from overcharge skill");
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.Duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
    }

    public class OverrideM1 : GenericProjectileBaseState
    {
        public float BaseDuration = 0.5f;
        public static float BaseDelayDuration = 0.0f;
        public float DamageCoefficient = 7f;
        GameObject viendbomb = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterBigProjectile.prefab").WaitForCompletion();
        public override void OnEnter()
        {
            projectilePrefab = viendbomb;
            baseDuration = BaseDuration / base.attackSpeedStat;
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
    }
}
