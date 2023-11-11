using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SupernovaMod.Content.Buffs.Cooldowns;
using SupernovaMod.Content.Items.Weapons.BaseWeapons;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class Gallant : SupernovaGunItem
    {
        private const int MAX_STORED_SHOTS = 6;
        private int _storedShots = MAX_STORED_SHOTS;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Gallant");
            // Tooltip.SetDefault("Can shoot 6 bullets before having to reload.\nRight-click to manualy reload.");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 22;
            Item.width = 50;
            Item.height = 24;
            Item.crit = 1;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(0, 4, 30, 0);
            Item.autoReuse = false;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item41;
            Item.shootSpeed = 8;

            Item.useAmmo = AmmoID.None;

            Item.scale = .75f;

            Gun.spread = 2;
        }

        public override bool CanUseItem(Player player)
        {
            // Check if our Gallant is cooling down
            //
            if (player.HasBuff(ModContent.BuffType<ReloadDebuff>()))
            {
                return false;
            }
            // We can not shoot and reload at the same time
            if (player.altFunctionUse == ItemAlternativeFunctionID.ActivatedAndUsed)
            {
                // Dont reload when we have 6 rounds loaded
                if (_storedShots != MAX_STORED_SHOTS)
                {
                    Reload(player);
                }
                return false;
            }
            return base.CanUseItem(player);
        }

        public override bool? UseItem(Player player)
        {
            _storedShots--;
            // After 6 shots the player will have to reload
            //
            if (_storedShots <= 0)
            {
                // Check if reloading was unsuccessfull,
                // and if any bullets where loaded.
                //
                if (!Reload(player) && _storedShots == 0)
                {
                    return false;
                }
            }
            return base.UseItem(player);
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D bulletTexture = TextureAssets.Item[ItemID.MusketBall].Value;
            // Draw stored bullets to let the player know how many are left
            //
            for (int i = 0; i < _storedShots; i++)
            {
                spriteBatch.Draw(bulletTexture,
                    position: new Vector2(position.X + bulletTexture.Width / 2 * (i - 3), position.Y + bulletTexture.Height * 1.2f),
                    sourceRectangle: new Rectangle(0, 0, bulletTexture.Width, bulletTexture.Height),
                    Color.White,
                    rotation: 0f,
                    origin: Vector2.Zero,
                    scale: new Vector2(.5f, .5f),
                    SpriteEffects.None,
                    layerDepth: 1
                );
            }
        }

        /// <summary>
        /// We can not shoot when reloading
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override bool CanShoot(Player player) => player.altFunctionUse == ItemAlternativeFunctionID.None;

        public override bool AltFunctionUse(Player player) => true;
        /// <summary>
        /// Handles our reload
        /// </summary>
        /// <param name="player"></param>
        private bool Reload(Player player)
        {
            SoundEngine.PlaySound(SoundID.Item149);
            player.AddBuff(ModContent.BuffType<ReloadDebuff>(), Item.useTime * 10); // The use time will be the ammount of seconds needed for cooldown

            Item.useAmmo = AmmoID.Bullet;

            // Consume ammo and increase _storedShots until fully filled or no ammo left
            //
            for (; _storedShots < MAX_STORED_SHOTS; _storedShots++)
            {
                // Try to get ammo
                Item ammo = player.ChooseAmmo(Item);
                if (ammo == null)
                {
                    return false;
                }
                if (ammo.type != ItemID.EndlessMusketPouch)
                {
                    player.ConsumeItem(ammo.type);
                }
            }
            Item.useAmmo = AmmoID.None;
            return false;
        }

        #region Save stored ammo
        public override void SaveData(TagCompound tag)
        {
            tag["ammoStored"] = _storedShots;
        }
        public override void LoadData(TagCompound tag)
        {
            _storedShots = tag.GetInt("ammoStored");
        }
        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(_storedShots);
        }
        public override void NetReceive(BinaryReader reader)
        {
            _storedShots = reader.ReadInt32();
        }
        #endregion

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GoldBar, 7);
            recipe.AddIngredient(ItemID.FlintlockPistol);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.FirearmManual>());
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PlatinumBar, 7);
            recipe.AddIngredient(ItemID.FlintlockPistol);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.FirearmManual>());
            recipe.Register();
        }
    }
}