using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;

namespace SupernovaMod.Content.Items.Weapons.Melee
{
    public class ZirconiumSword : ModItem
    {
        private readonly int _projIdSpark = ModContent.ProjectileType<Projectiles.Melee.ZicroniumSpark>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Zirconium Sword");
            // Tooltip.SetDefault("Release a blast of Zirconium Sparks every 4 hits.\nZirconium Sparks linger for a short while.");
        }
        public override void SetDefaults()
        {
            Item.damage = 16;
            Item.crit = 1;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 7;
            Item.value = Item.buyPrice(0, 3, 0, 0); // Another way to handle value of item.
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.scale += .05f;
            Item.DamageType = DamageClass.Melee;
        }

        private int _hits;
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
			_hits++;
			if (_hits < 4)
            {
                return;
            }
            SoundEngine.PlaySound(SoundID.Item93, new Vector2?(player.position));

            // Spark Explosion effect
            for (int j = 0; j <= Main.rand.Next(2, 4); j++)
            {
                Vector2 velocity = (Vector2.One * Main.rand.Next(2, 4)).RotatedByRandom(180);
                Projectile.NewProjectile(Item.GetSource_FromAI(), target.position.X, target.position.Y, velocity.X, velocity.Y, _projIdSpark, Item.damage / 2, 3, player.whoAmI);
            }
            _hits = 0;
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.ZirconiumBar>(), 8);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
