using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using SupernovaMod.Content.Items.Weapons.BaseWeapons;
using SupernovaMod.Api;
using Microsoft.Xna.Framework.Graphics;
using SupernovaMod.Content.Buffs.Cooldowns;
using System.IO;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader.IO;

namespace SupernovaMod.Content.Items.Weapons.Ranged
{
    public class CarnageRifle : SupernovaGunItem
    {
        private const int MAX_STORED_SHOTS = 3;
        private int _storedShots = MAX_STORED_SHOTS;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // DisplayName.SetDefault("Carnage Rifle");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, -1.5f);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 40;
            Item.width = 58;
            Item.crit = 2;
            Item.height = 20;
            Item.useAnimation = 21;
            Item.useTime = 21;
            Item.knockBack = 6.4f;
            Item.autoReuse = false;
            Item.value = BuyPrice.RarityOrange;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item36;
            Item.shootSpeed = 14;

            Item.useAmmo = AmmoID.None;

            Gun.recoil = .8f;
            Gun.useStyle = GunUseStyle.PumpAction;
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
            player.AddBuff(ModContent.BuffType<ReloadDebuff>(), 50);

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
            recipe.AddIngredient(ModContent.ItemType<Materials.BloodShards>(), 5);
            recipe.AddIngredient(ModContent.ItemType<Materials.BoneFragment>(), 7);
            recipe.AddIngredient(ItemID.Musket);
            recipe.AddIngredient(ModContent.ItemType<Materials.FirearmManual>(), 2);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}