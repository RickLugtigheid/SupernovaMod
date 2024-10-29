using Microsoft.Xna.Framework;
using SupernovaMod.Content.Buffs.DamageOverTime;
using SupernovaMod.Content.Projectiles.BaseProjectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Projectiles.Summon
{
    // TODO: This was accidentally removed but retrieved through source code,
    // so please change the code back to the original state...

    public class ShadowWisp : SupernovaMinionProjectile
    {
        protected override int BuffType => -1;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionTargettingFeature[((ModProjectile)this).Projectile.type] = true;
            Main.projPet[((ModProjectile)this).Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[((ModProjectile)this).Projectile.type] = false;
            ProjectileID.Sets.CultistIsResistantTo[((ModProjectile)this).Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            ((Entity)((ModProjectile)this).Projectile).width = 8;
            ((Entity)((ModProjectile)this).Projectile).height = 8;
            ((ModProjectile)this).Projectile.tileCollide = false;
            ((ModProjectile)this).Projectile.usesLocalNPCImmunity = true;
            ((ModProjectile)this).Projectile.friendly = true;
            ((ModProjectile)this).Projectile.minion = true;
            ((ModProjectile)this).Projectile.DamageType = DamageClass.Default;
            ((ModProjectile)this).Projectile.minionSlots = 0f;
            ((ModProjectile)this).Projectile.penetrate = 1;
            ((ModProjectile)this).Projectile.usesLocalNPCImmunity = true;
            ((ModProjectile)this).Projectile.localNPCHitCooldown = 16;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        protected override void UpdateVisuals()
        {
            //IL_001c: Unknown result type (might be due to invalid IL or missing references)
            //IL_006d: Unknown result type (might be due to invalid IL or missing references)
            //IL_0073: Unknown result type (might be due to invalid IL or missing references)
            base.UpdateVisuals();
            if (Utils.NextBool(Main.rand, 3))
            {
                int DustID2 = Dust.NewDust(((Entity)((ModProjectile)this).Projectile).Center, ((Entity)((ModProjectile)this).Projectile).width * 2, ((Entity)((ModProjectile)this).Projectile).height * 2, 27, ((Entity)((ModProjectile)this).Projectile).velocity.X * 0.5f, ((Entity)((ModProjectile)this).Projectile).velocity.Y * 0.5f, 20, default(Color), Utils.NextFloat(Main.rand, 0.75f, 1f));
                Main.dust[DustID2].noGravity = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            target.AddBuff(ModContent.BuffType<BlackFlames>(), 120, false);
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            target.AddBuff(ModContent.BuffType<BlackFlames>(), 120, true, false);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor.A = 127;
            return base.PreDraw(ref lightColor);
        }
    }

}
