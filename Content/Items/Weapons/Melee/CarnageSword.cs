using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;

namespace Supernova.Content.Items.Weapons.Melee
{
    public class CarnageSword : ModItem
    {
        private const int HEAL_AMOUNT = 2;
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Carnage Sword");
            //Tooltip.SetDefault("Killing enemies will heal the player by " + 4 * HEAL_AMOUNT + '.');
            Tooltip.SetDefault("Landing crits on enemies will heal the player by " + 3 * HEAL_AMOUNT + "hp.");
        }
        public override void SetDefaults()
        {
            Item.damage = 17;
            Item.crit = 7;
            Item.width = 52;
            Item.height = 62;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(4))
            {
                // Emit dusts when the sword is swung 
                Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Blood, Scale: 1.4f);
            }
        }
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            // Check if the npc will be killed by this strike
            //
            //if ((target.life - damage) <= 0)
            if (crit)
            {
                for (int i = 0; i < 3; i++)
                {
                    // Get a random starting velocity so not all projectiles will start the same direction.
                    Vector2 startVelocity = new Vector2(
                        Main.rand.Next(-10, 10),
                        Main.rand.Next(-10, 10)
                    );

                    // Heal the player by healAmount
                    //
                    Projectile.NewProjectile(player.GetSource_FromThis(), target.Center, startVelocity, ProjectileID.SpiritHeal, 1, 0, player.whoAmI, 0, HEAL_AMOUNT);
                }
            }

            base.ModifyHitNPC(player, target, ref damage, ref knockBack, ref crit);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Materials.BloodShards>(), 8);
            recipe.AddIngredient(ModContent.ItemType<Materials.BoneFragment>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
