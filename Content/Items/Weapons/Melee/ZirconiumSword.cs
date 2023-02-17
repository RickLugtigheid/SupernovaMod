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
        private readonly int _projIdSpark = ModContent.ProjectileType<Projectiles.Magic.ZicroniumSpark>();
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            DisplayName.SetDefault("Zirconium Sword");
            Tooltip.SetDefault("May release a blast of Zirconium Spark when striking an enemy.\nZirconium Sparks linger for a short while.");
        }
        public override void SetDefaults()
        {
            Item.damage = 13;
            Item.crit = 4;
            Item.width = 48;
            Item.height = 48;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4;
            Item.value = Item.buyPrice(0, 3, 0, 0); // Another way to handle value of item.
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;

            Item.DamageType = DamageClass.Melee;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (!Main.rand.NextBool(5))
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
