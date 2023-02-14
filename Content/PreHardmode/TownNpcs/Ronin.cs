using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Personalities;
using System.Collections.Generic;
using Terraria.GameContent.Bestiary;

namespace Supernova.Content.PreHardmode.TownNpcs
{
    [AutoloadHead]
    public class Ronin : ModNPC
    {
        public override string Texture => "Supernova/Content/PreHardmode/TownNpcs/Ronin";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ronin");    //the name displayed when hovering over the npc ingame.
            Main.npcFrameCount[NPC.type] = 25; //this defines how many frames the npc sprite sheet has
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 150; //this defines the npc danger detect range
            NPCID.Sets.AttackType[NPC.type] = 0; //this is the attack type,  0 (throwing), 1 (shooting), or 2 (magic). 3 (melee) 
            NPCID.Sets.AttackTime[NPC.type] = 10; //this defines the npc attack speed
            NPCID.Sets.AttackAverageChance[NPC.type] = 10;//this defines the npc atack chance
            NPCID.Sets.HatOffsetY[NPC.type] = 4; //this defines the party hat position

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                // Influences how the NPC looks in the Bestiary
                Velocity = .2f // Draws the NPC in the bestiary as if its walking +1 tiles in the x directions
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement(""),
            });
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true; //This defines if the npc is a town Npc or not
            NPC.friendly = true;  //this defines if the npc can hurt you or not()
            NPC.width = 18; //the npc sprite width
            NPC.height = 46;  //the npc sprite height
            NPC.aiStyle = 7; //this is the npc ai style, 7 is Pasive Ai
            NPC.defense = 25;  //the npc defense
            NPC.lifeMax = 250;// the npc life
            NPC.HitSound = SoundID.NPCHit1;  //the npc sound when is hit
            NPC.DeathSound = SoundID.NPCDeath1;  //the npc sound when he dies
            NPC.knockBackResist = 0.5f;  //the npc knockback resistance
            AnimationType = NPCID.Guide;  //this copy the guide animation

            NPC.Happiness
                .SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike)
                .SetBiomeAffection<JungleBiome>(AffectionLevel.Dislike)
                .SetBiomeAffection<HallowBiome>(AffectionLevel.Hate)
                .SetBiomeAffection<MushroomBiome>(AffectionLevel.Hate)
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Love)
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Like)
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Like)
                .SetBiomeAffection<OceanBiome>(AffectionLevel.Like)
                .SetNPCAffection(NPCID.Dryad, AffectionLevel.Love)
                .SetNPCAffection(NPCID.Guide, AffectionLevel.Like)
                .SetNPCAffection(NPCID.WitchDoctor, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate);
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money) // Whether or not the conditions have been met for this town NPC to be able to move into town.
        {
            if (NPC.downedBoss1)  //so after the EoC is killed this town npc can spawn
                return true;
             return false;
        }

        public override bool CheckConditions(int left, int right, int top, int bottom)    //Allows you to define special conditions required for this town NPC's house
        {
            return true;  //so when a house is available the npc will  spawn
        }
		public override List<string> SetNPCNameList()
		{
            return new List<string>()
            {
                "Yusuke",
                "Sojiro",
                "Sasaki",
                "Ryu",
                "Kyo",
                "Jin",
                "Ren"
            };
		}

        public override void SetChatButtons(ref string button, ref string button2)  //Allows you to set the text for the buttons that appear on this town NPC's chat window. 
        {
            button = "Buy";   //this defines the buy button name
        }
        public override void OnChatButtonClicked(bool firstButton, ref bool shop) //Allows you to make something happen whenever a button is clicked on this town NPC's chat window. The firstButton parameter tells whether the first button or second button (button and button2 from SetChatButtons) was clicked. Set the shop parameter to true to open this NPC's shop.
        {
            if (firstButton)
                shop = true;   //so when you click on buy button opens the shop
        }

        public override void SetupShop(Chest shop, ref int nextSlot)       //Allows you to add items to this town NPC's shop. Add an item by setting the defaults of shop.item[nextSlot] then incrementing nextSlot.
        {
            if (NPC.downedSlimeKing)
            {
                shop.item[nextSlot].SetDefaults(ItemID.NinjaPants);
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.NinjaShirt); 
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.NinjaHood);  
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.Katana); 
                nextSlot++;
            }

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Melee.Kama>());  //this is an example of how to add a modded item
            nextSlot++;

            /*if (NPC.downedBoss2)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("samuraiAmulet"));  //this is an example of how to add a modded item
                nextSlot++;
            }*/
            if (NPC.downedBoss3)   //this make so when Skeletron is killed the town npc will sell this
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Throwing.Kunai>());
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.WormholePotion);
                nextSlot++;
            }
            /*if(Main.hardMode == true)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("Zorna"));
                nextSlot++;
				shop.item[nextSlot].SetDefaults(mod.ItemType("Magnitrix"));
				nextSlot++;
                if (Main.bloodMoon == true)
                {
                    shop.item[nextSlot].SetDefaults(mod.ItemType("BrokenSwordShards"));
                    nextSlot++;
                }
            }*/
        }

        public override string GetChat()       //Allows you to give this town NPC a chat message when a player talks to it.
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();

            chat.Add("Hello young one", .75);
            chat.Add("How can I help you?");
            chat.Add("You will never know how I got my hands on the ninja armour", .5);
            chat.Add("Did you come to train with me?");
            chat.Add("A katana is a fine weapon");
            if(!NPC.downedBoss3) chat.Add("Come back when you have killed Skeletron for some more items!", .27);
            return chat;
        }
        public override void TownNPCAttackStrength(ref int damage, ref float knockback)//  Allows you to determine the damage and knockback of this town NPC attack
        {
            damage = 10;         // npc damage
            knockback = 0.8f;   //npc knockback
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)  //Allows you to determine the cooldown between each of this town NPC's attack. The cooldown will be a number greater than or equal to the first parameter, and less then the sum of the two parameters.
        {
            cooldown = 1;
            randExtraCooldown = 1;
        }
        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)//Allows you to determine the projectile type of this town NPC's attack, and how long it takes for the projectile to actually appear
        {
            projType = ModContent.ProjectileType<Global.Projectiles.Thrown.KunaiProj>();
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)//Allows you to determine the speed at which this town NPC throws a projectile when it attacks. Multiplier is the speed of the projectile, gravityCorrection is how much extra the projectile gets thrown upwards, and randomOffset allows you to randomize the projectile's velocity in a square centered around the original velocity
        {
            multiplier = 17f;
            randomOffset = 2f;
        }
    }
}