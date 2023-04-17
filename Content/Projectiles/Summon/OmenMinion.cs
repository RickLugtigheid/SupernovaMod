using Microsoft.Xna.Framework;
using SupernovaMod.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SupernovaMod.Api;
using SupernovaMod.Content.Projectiles.BaseProjectiles;

namespace SupernovaMod.Content.Projectiles.Summon
{
    public class OmenMinion : SupernovaMinionProjectile
    {
        protected override int BuffType => ModContent.BuffType<Buffs.Summon.VerglasFlakeBuff>();

        private int _rotateSpeed = 3;

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Omen");

            // This is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
        }
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.tileCollide = false; // Makes the minion go through tiles freely

            // These below are needed for a minion weapon
            Projectile.usesLocalNPCImmunity = true;
            Projectile.friendly = true; // Only controls if it deals damage to enemies on contact (more on that later)
            Projectile.minion = true; // Declares this as a minion (has many effects)
            Projectile.DamageType = DamageClass.Summon; // Declares the damage type (needed for it to deal damage)
            Projectile.minionSlots = 1f; // Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
            Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 18;

            speed = 12;
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

		protected override void UpdateMovement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition)
		{
			base.UpdateMovement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);

            // Speed up the rotation when getting close to the enemy
            _rotateSpeed = foundTarget ? 12 : 4;
		}

		protected override void UpdateVisuals()
        {
			Projectile.rotation += MathHelper.ToRadians(_rotateSpeed);
		}
    }
}