using Microsoft.Xna.Framework;
using SupernovaMod.Api;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SupernovaMod.Content.Items.Accessories
{
    public class MeteorBoots : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Meteor boots");
            Tooltip.SetDefault($"When you double tap 'down_button' in the air you will become a meteor!");
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 1;
            Item.value = BuyPrice.RarityBlue;
            Item.rare = ItemRarityID.Blue;
            Item.accessory = true;
        }

        int timer = 0;
        int power = 0; //power = damage
        float fallCheck;
        bool runTimer;
        bool falling;
        public override void UpdateAccessory(Player player, bool hideVisual = false)
        {
            //DO THIS:
            //you would do an accessory based on the SimpleModPlayer
            //and then use the PreHurt hook in ModPlayer
            //basically you need to reimplement vanilla code a bit to detect if you received fall damage
            //read the adaption guide

            #region dash
            downDashPlayer mp = player.GetModPlayer<downDashPlayer>();

            //If the dash is not active, immediately return so we don't do any of the logic for it
            if (!mp.DashActive)
			{
                return;
            }

            //This is where we set the afterimage effect.  You can replace these two lines with whatever you want to happen during the dash
            //Some examples include:  spawning dust where the player is, adding buffs, making the player immune, etc.
            //Here we take advantage of "player.eocDash" and "player.armorEffectDrawShadowEOCShield" to get the Shield of Cthulhu's afterimage effect
            player.eocDash = mp.DashTimer;
            player.armorEffectDrawShadowEOCShield = true;

            //set the dust of the trail
            int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + 2), player.width + 2, player.height + 2, DustID.t_Meteor, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 30, default(Color), 1);


            Main.dust[dust].noGravity = true; //this make so the dust has no gravity
            Main.dust[dust].velocity *= 0f;

			dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y + 2), player.width, player.height, DustID.FlameBurst, player.velocity.X * 0.2f, player.velocity.Y * 0.2f, 2, default(Color), .7f);
			Main.dust[dust].noGravity = true; //this make so the dust has no gravity

			//If the dash has just started, apply the dash velocity in whatever direction we wanted to dash towards
			if (mp.DashTimer == downDashPlayer.MAX_DASH_TIMER)
            {
                Vector2 newVelocity = player.velocity;

                //Only apply the dash velocity if our current speed in the wanted direction is less than DashVelocity
                if ((mp.DashDir == downDashPlayer.DashDown && player.velocity.Y < mp.DashVelocity))
                {
                    //Y-velocity is set here
                    //If the direction requested was DashUp, then we adjust the velocity to make the dash appear "faster" due to gravity being immediately in effect
                    //This adjustment is roughly 1.3x the intended dash velocity
                    float dashDirection = mp.DashDir == downDashPlayer.DashDown ? 1.5f : -1.3f;
                    newVelocity.Y = dashDirection * mp.DashVelocity;
                }

                player.velocity = newVelocity;
                int damage = Main.rand.Next(12, 22);
                Projectile.NewProjectile(Item.GetSource_FromAI(), player.position.X, player.position.Y - 0, Main.rand.NextFloat(0.01f, 1), 4, ProjectileID.Meteor3, damage, 7, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(Item.GetSource_FromAI(), player.position.X, player.position.Y - 25, 0, 4, ProjectileID.Meteor1, damage, 7, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(Item.GetSource_FromAI(), player.position.X, player.position.Y - 50, Main.rand.NextFloat(1, -0.01f), 4, ProjectileID.Meteor2, damage, 7, Main.myPlayer, 0f, 0f);
            }

            //Decrement the timers
            mp.DashTimer--;
            mp.DashDelay--;

            if (mp.DashDelay == 0)
            {
                //The dash has ended.  Reset the fields
                mp.DashDelay = downDashPlayer.MAX_DASH_DELAY;
                mp.DashTimer = downDashPlayer.MAX_DASH_TIMER;
                mp.DashActive = false;
            }
            #endregion
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteoriteBar, 20);
            recipe.AddIngredient(ItemID.Silk, 15);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
    public class downDashPlayer : ModPlayer
    {
        //These indicate what direction is what in the timer arrays used
        public static readonly int DashDown = 0;
        public static readonly int DashUp = 1;

        //The direction the player is currently dashing towards.  Defaults to -1 if no dash is ocurring.
        public int DashDir = -1;

        //The fields related to the dash accessory
        public bool DashActive = false;
        public int DashDelay = MAX_DASH_DELAY;
        public int DashTimer = MAX_DASH_TIMER;
        //The initial velocity.  10 velocity is about 37.5 tiles/second or 50 mph
        public readonly float DashVelocity = 50f;
        //These two fields are the max values for the delay between dashes and the length of the dash in that order
        //The time is measured in frames
        public static readonly int MAX_DASH_DELAY = 180;
        public static readonly int MAX_DASH_TIMER = 35;

        public override void ResetEffects()
        {
            //ResetEffects() is called not long after player.doubleTapCardinalTimer's values have been set

            //Check if the ExampleDashAccessory is equipped and also check against this priority:
            // If the Shield of Cthulhu, Master Ninja Gear, Tabi and/or Solar Armour set is equipped, prevent this accessory from doing its dash effect
            //The priority is used to prevent undesirable effects.
            //Without it, the player is able to use the ExampleDashAccessory's dash as well as the vanilla ones
            bool dashAccessoryEquipped = false;

            //This is the loop used in vanilla to update/check the not-vanity accessories
            for (int i = 3; i < 8 + Player.extraAccessorySlots; i++)
            {
                Item item = Player.armor[i];

                //Set the flag for the ExampleDashAccessory being equipped if we have it equipped OR immediately return if any of the accessories are
                // one of the higher-priority ones
                if (item.type == ModContent.ItemType<MeteorBoots>())
                    dashAccessoryEquipped = true;
                else if (item.type == ItemID.EoCShield || item.type == ItemID.MasterNinjaGear || item.type == ItemID.Tabi)
                    return;
            }

            //If we don't have the ExampleDashAccessory equipped or the player has the Solor armor set equipped, return immediately
            //Also return if the player is currently on a mount, since dashes on a mount look weird, or if the dash was already activated
            if (!dashAccessoryEquipped || Player.setSolar || Player.mount.Active || DashActive)
                return;

            //When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
            //If the timers are set to 15, then this is the first press just processed by the vanilla logic.  Otherwise, it's a double-tap
            if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[DashDown] < 15)
                DashDir = DashDown;
            else
                return;  //No dash was activated, return

            DashActive = true;

            //Here you'd be able to set an effect that happens when the dash first activates
            //Some examples include:  the larger smoke effect from the Master Ninja Gear and Tabi
        }
	}
}
