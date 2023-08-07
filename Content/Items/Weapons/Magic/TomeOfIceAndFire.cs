using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SupernovaMod.Api;
using Terraria.GameContent.Creative;

namespace SupernovaMod.Content.Items.Weapons.Magic
{
    public class TomeOfIceAndFire : ModShotgun
    {
        public override float SpreadAngle => 8;

        public override int GetShotAmount() => 5;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Tome of Frost and Fire");

            // Tooltip.SetDefault("Shoots fire and frostburn");
        }

        private bool _frostFlame = true;

        public override void SetDefaults()
        {
            Item.damage = 27;  //The damage stat for the Weapon.
            Item.crit = 4;
            Item.knockBack = 3;
            Item.noMelee = true;  //Setting to True allows the weapon sprite to stop doing damage, so only the projectile does the damge
            Item.noUseGraphic = false;
            Item.channel = true;                            //Channel so that you can held the weapon
            Item.rare = ItemRarityID.Orange;   //The color the title of your Weapon when hovering over it ingame
            Item.width = 28;   //The size of the width of the hitbox in pixels.
            Item.height = 30;    //The size of the height of the hitbox in pixels.
            Item.UseSound = SoundID.Item20;
            Item.useTime = 23;
            Item.useAnimation = 23;
            Item.shootSpeed = 5.5f;
            Item.mana = 7;
            Item.useStyle = ItemUseStyleID.Shoot;  
            Item.value = Item.sellPrice(0, 10, 0, 0);//	How much the item is worth, in copper coins, when you sell it to a merchant. It costs 1/5th of this to buy it back from them. An easy way to remember the value is platinum, gold, silver, copper or PPGGSSCC (so this item price is 3gold)
            Item.autoReuse = true;
            Item.DamageType = DamageClass.Magic;

			// TODO: Check why, and if still necessary
			UpdateShootType();
		}

		public override bool CanUseItem(Player player)
        {
            UpdateShootType();
            return base.CanUseItem(player);
        }

        private void UpdateShootType()
        {
			if (_frostFlame)
			{
				Item.shoot = ModContent.ProjectileType<Projectiles.Magic.FrostFlame>();
				_frostFlame = false;
			}
			else
			{
				//Item.shoot = ProjectileID.Flames;
				Item.shoot = ModContent.ProjectileType<Projectiles.Magic.BlazeBoltFlames>();
				_frostFlame = true;
			}
		}

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(player, target, hit, damageDone);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Book);
            recipe.AddIngredient(ModContent.ItemType<BlazeBolt>());
            recipe.AddIngredient(ModContent.ItemType<IceBolt>());
            recipe.AddIngredient(ItemID.Bone, 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }

    }
}
