using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Supernova.Content.Projectiles.BaseProjectiles;

namespace Supernova.Content.Npcs.Bosses.HarbingerOfAnnihilation
{
    public class HarbingersKnellProjectile : SupernovaMinionProjectile
    {
        protected override int BuffType => ModContent.BuffType<Buffs.Minion.HarbingersKnellBuff>();

        private int _rotateSpeed = 5;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Omen");

            // Sets the amount of frames this minion has on its spritesheet
            //Main.projFrames[Projectile.type] = 4;

            // This is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 28;
            Projectile.tileCollide = false; // Makes the minion go through tiles freely

            // These below are needed for a minion weapon
            Projectile.usesLocalNPCImmunity = true;
            Projectile.friendly = true; // Only controls if it deals damage to enemies on contact (more on that later)
            Projectile.minion = true; // Declares this as a minion (has many effects)
            Projectile.DamageType = DamageClass.Summon; // Declares the damage type (needed for it to deal damage)
            Projectile.minionSlots = 1f; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles
        }

        // Here you can decide if your minion breaks things like grass or pots
        public override bool? CanCutTiles()
        {
            return false;
        }

        // This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
        public override bool MinionContactDamage()
        {
            return true;
        }
        protected override void GeneralBehavior(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition)
        {
            base.GeneralBehavior(owner, out vectorToIdlePosition, out distanceToIdlePosition);
        }

        protected override void UpdateMovement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition)
        {
            base.UpdateMovement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);

            // Speed up the rotation when getting close to the enemy
            if (foundTarget)
            {
                _rotateSpeed = 8;
            }
            else
            {
                _rotateSpeed = 5;
            }
        }

        protected override void UpdateVisuals()
        {
            //base.UpdateVisuals();
            Projectile.rotation += MathHelper.ToRadians(_rotateSpeed);
        }
    }
}