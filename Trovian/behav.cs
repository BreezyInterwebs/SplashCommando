using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;
//Since we are using effects from Commando's Barrage skill, we will also be using the associated namespace
//You can also use Addressables or LegacyResourcesAPI to load whichever effects you like

namespace CustomSkillsTutorial.MyEntityStates
{
    public class Blast : GenericProjectileBaseState
    {
        public float BaseDuration = 0f;
        public static float BaseDelayDuration = 0.0f;
        public float DamageCoefficient = 7f;
        GameObject novabomb = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageLightningBombProjectile.prefab").WaitForCompletion();
        public override void OnEnter()
        {
            projectilePrefab = novabomb;
            baseDuration = BaseDuration;
            baseDelayBeforeFiringProjectile = BaseDelayDuration;
            damageCoefficient = DamageCoefficient;
            force = 80f;  
            bloom = 10;
            base.OnEnter();
        }

        //This method runs once at the end
        //Here, we are doing nothing
        public override void OnExit()
        {
            base.OnExit();
        }

        //FixedUpdate() runs almost every frame of the skill
        //Here, we end the skill once it exceeds its intended duration
        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        //GetMinimumInterruptPriority() returns the InterruptPriority required to interrupt this skill
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }

    public class BlastUp : GenericProjectileBaseState
    {

    }
}
