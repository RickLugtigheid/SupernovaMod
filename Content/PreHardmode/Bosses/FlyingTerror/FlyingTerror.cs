using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Supernova.Api;
using Supernova.Common;
using Supernova.Common.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Supernova.Content.PreHardmode.Bosses.FlyingTerror
{
	[AutoloadBossHead]
	public class FlyingTerror : ModBossNpc
    {
        private int _stage = 1;
        private int _dashFrame = 29;
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flying Terror");
            Main.npcFrameCount[NPC.type] = 5;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                // Influences how the NPC looks in the Bestiary
                PortraitScale = .75f,
                Scale = .75f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A scourge of the night, veraciously out for the hunt. Always looking for its next prey in its neverending pursuit to satiate its flesh craving. Its emissaries scour the underground searching for more victims to nourish their gluttonous master.\r\n"),
            });
        }

        public override void SetDefaults()
        {
            _glowColor = _glowDefault;
            NPC.aiStyle = -1; // Will not have any AI from any existing AI styles. 
            NPC.lifeMax = 3400; // The Max HP the boss has on Normal
            NPC.damage = 36; // The base damage value the boss has on Normal
            NPC.defense = 12; // The base defense on Normal
            NPC.knockBackResist = 0f; // No knockback
            NPC.width = 200;
            NPC.height = 100;
            NPC.value = 10000;
            NPC.npcSlots = 1f; // The higher the number, the more NPC slots this NPC takes.
            NPC.boss = true; // Is a boss
            NPC.lavaImmune = false; // Not hurt by lava
            NPC.noGravity = true; // Not affected by gravity
            NPC.noTileCollide = true; // Will not collide with the tiles. 
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            Music = MusicID.Boss1;
            //bossBag = Mod.Find<ModItem>("FlyingTerrorBag").Type; // Needed for the NPC to drop loot bag.
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * bossLifeScale);
            NPC.defense = (int)(NPC.defense + numPlayers);
            NPC.damage  = (int)(NPC.damage * 1.35f);
        }

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			// Add a BossBag (automatically checks for expert mode)
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<FlyingTerrorBag>()));

			// All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			// Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<TerrorTuft>()));

			// For settings if the boss has been downed
			SupernovaBosses.downedFlyingTerror = true;

            base.ModifyNPCLoot(npcLoot);
		}

		bool isFireBreath = false;
        public override void AI()
		{
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest(true);
			}
			bool dead = Main.player[NPC.target].dead;
            if (Main.dayTime | dead)
            {
                NPC.velocity.Y = NPC.velocity.Y - 0.04f;
                if (NPC.timeLeft > 10)
                {
                    NPC.timeLeft = 10;
                }
            }
            else
            {
                // Handle movement
                Move();

                // Add particle effects
                if (Main.rand.NextBool(5))
                {
                    Vector2 position = new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height * 0.25f);
                    int width = NPC.width;
                    int height = (int)((float)NPC.height * 0.5f);
                    int dustId = Dust.NewDust(position, width, height, ModContent.DustType<Global.Dusts.TerrorDust>(), NPC.velocity.X, 2f, 0, default(Color), 1f);
                    Dust dust0 = Main.dust[dustId];
                    dust0.velocity.X = dust0.velocity.X * 0.5f;
                    Dust dust1 = Main.dust[dustId];
                    dust1.velocity.Y = dust1.velocity.Y * 0.1f;
                }

                // Handle attack patterns
                NPC.ai[0]++;
                if (NPC.ai[0] < 340 && NPC.ai[1] == 0)
                {
                    if (_stage != 4 && NPC.ai[0] % (120 - (100 * (_stage / 10))) == 0)
                        Shoot(_stage / 30);
                    else if (_stage == 4 && NPC.ai[0] % 70 == 0)
                    {
                        Vector2 position = new Vector2(NPC.position.X + (NPC.direction == 1 ? NPC.width : 0), NPC.position.Y + 60);
                        float rotation = (float)Math.Atan2(position.Y - (targetPlayer.position.Y + (targetPlayer.height * 0.2f)), position.X - (targetPlayer.position.X + (targetPlayer.width * 0.15f)));
                        Vector2[] speeds = Mathf.RandomSpread(new Vector2((float)(-(Math.Cos(rotation) * 16)) * .7f, (float)(-(Math.Sin(rotation) * 16))) * .7f, 8 * .0525f, 5);
                        for (int i = 0; i < 3; ++i)
                        {
                            for (int x = 0; x < 5; x++)
                            {
                                int dust = Dust.NewDust(position, 100, 100, ModContent.DustType<Global.Dusts.TerrorDust>(), -speeds[i].X, -speeds[i].Y, 80, default(Color), Main.rand.NextFloat(.9f, 1.4f));   //this defines the flames dust and color, change DustID to wat dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                                Main.dust[dust].noGravity = false; //this make so the dust has no gravity
                                Main.dust[dust].velocity *= Main.rand.NextFloat(.3f, .5f);
                            }
                            SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), position.X, position.Y, speeds[i].X * Main.rand.NextFloat(.5f, 1.2f), speeds[i].Y * Main.rand.NextFloat(.5f, 1.2f), ModContent.ProjectileType<TerrorProj>(), 40, 1.25f);
                        }
                    }
                }
                else if (NPC.ai[0] > (_stage == 4 ? 540 : 480) && NPC.ai[1] == 0)
                    NPC.ai[1] = 1;
            }
		}

        private void Shoot(float extraSpread)
		{
            targetPlayer = Main.player[NPC.target];
            int type = ModContent.ProjectileType<TerrorProj>();
            SoundEngine.PlaySound(SoundID.Item20, NPC.Center);

            Vector2 position = new Vector2(NPC.position.X + (NPC.direction == 1 ? NPC.width : 0), NPC.position.Y - 10);
            //Screen.Debug(NPC.direction);
            float rotation = (float)Math.Atan2(position.Y - (targetPlayer.position.Y + (targetPlayer.height * 0.2f)), position.X - (targetPlayer.position.X + (targetPlayer.width * 0.15f))) * Main.rand.NextFloat(.9f - extraSpread, 1.1f + extraSpread);

            Vector2 velocity = new Vector2((float)(-(Math.Cos(rotation) * 18)) * .7f, (float)(-(Math.Sin(rotation) * 18)) * .7f);
            Projectile.NewProjectile(NPC.GetSource_FromAI(), position, velocity, type, 35, 0f, 0);

            for (int x = 0; x < 5; x++)
            {
                int dust = Dust.NewDust(position, 140, 140, ModContent.DustType<Global.Dusts.TerrorDust>(), velocity.X, velocity.Y, 80, default(Color), Main.rand.NextFloat(.9f, 1.6f));
                Main.dust[dust].noGravity = false;
                Main.dust[dust].velocity *= Main.rand.NextFloat(.3f, .5f);
            }
        }

        private void Move()
		{
            NPC.spriteDirection = NPC.direction;
            switch (NPC.ai[1])
            {
                // Main movement
                case 0:
                    float speed = Main.expertMode ? .065f : .055f;
                    if (_stage > 1)
                        speed *= 1.25f;
                    float targetHight = 300;

                    Vector2 vector444 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                    float targetX = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector444.X;
                    float targetY = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - targetHight - vector444.Y;

                    if (NPC.velocity.X < targetX + 20)
                    {
                        NPC.velocity.X = NPC.velocity.X + speed;
                        NPC.direction = 1;
                        if (NPC.velocity.X < 0f && targetX > 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X + speed;
                        }
                    }
                    else if (NPC.velocity.X > targetX - 20)
                    {
                        NPC.velocity.X = NPC.velocity.X - speed;
                        NPC.direction = -1;
                        if (NPC.velocity.X > 0f && targetX < 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X - speed;
                        }
                    }
                    if (NPC.velocity.Y < targetY)
                    {
                        NPC.velocity.Y = NPC.velocity.Y + speed;
                        if (NPC.velocity.Y < 0f && targetY > 0f)
                        {
                            NPC.velocity.Y = NPC.velocity.Y + speed;
                        }
                    }
                    else if (NPC.velocity.Y > targetY)
                    {
                        NPC.velocity.Y = NPC.velocity.Y - speed;
                        if (NPC.velocity.Y > 0f && targetY < 0f)
                        {
                            NPC.velocity.Y = NPC.velocity.Y - speed;
                        }
                    }
                    break;
                // Dive state 1:
                // Fly above the player for x time
                case 1:
                    speed = .18f;
                    targetHight = 350;

                    vector444 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                    targetX = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector444.X;
                    targetY = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - targetHight - vector444.Y;

                    if (NPC.velocity.X < targetX)
                    {
                        NPC.velocity.X = NPC.velocity.X + speed;
                        NPC.direction = 1;
                        if (NPC.velocity.X < 0f && targetX > 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X + speed;
                        }
                    }
                    else if (NPC.velocity.X > targetX)
                    {
                        NPC.velocity.X = NPC.velocity.X - speed;
                        NPC.direction = -1;
                        if (NPC.velocity.X > 0f && targetX < 0f)
                        {
                            NPC.velocity.X = NPC.velocity.X - speed;
                        }
                    }
                    if (NPC.velocity.Y < targetY)
                    {
                        NPC.velocity.Y = NPC.velocity.Y + speed;
                        if (NPC.velocity.Y < 0f && targetY > 0f)
                        {
                            NPC.velocity.Y = NPC.velocity.Y + speed;
                        }
                    }
                    else if (NPC.velocity.Y > targetY)
                    {
                        NPC.velocity.Y = NPC.velocity.Y - speed;
                        if (NPC.velocity.Y > 0f && targetY < 0f)
                        {
                            NPC.velocity.Y = NPC.velocity.Y - speed;
                        }
                    }

                    // Clamp the velocity to make sure we aren't going to fast
                    int clamp = 14;
                    NPC.velocity = Vector2.Clamp(NPC.velocity,
                        new Vector2(-clamp, -clamp),
                        new Vector2(clamp, clamp));

                    if (
                        // Check if the player isn't above us
                        (NPC.position.Y < Main.player[NPC.target].position.Y) &&
                        // Check if we are within a x distance of the player
                        (NPC.position.X == Main.player[NPC.target].position.X || NPC.position.X > Main.player[NPC.target].position.X - (NPC.width * .75f) && NPC.position.X < Main.player[NPC.target].position.X + (NPC.width * .25f)))
                    {
                        NPC.ai[2] = 0;
                        NPC.ai[1] = 2;
                    }
                    break;
                // Dive state 2:
                // Dash down
                case 2:
                    if (NPC.ai[2] == 1)
                    {
                        _playAnimation = false;

                        NPC.velocity.X *= .13f;
                        //_glowColor = new Color(255, 80, 80);
                        SoundEngine.PlaySound(/*36*/ in SoundID.Roar, NPC.position/*, 0, 1f, 0f*/);
                        speed = 16;
                        if (_stage > 2)
                            speed *= 1.05f;
                        if (Main.expertMode)
                            speed *= 1.025f;
                        targetY = Main.player[NPC.target].position.Y - 200;
                        float num3516 = (float)Math.Sqrt((double)(targetY * targetY));
                        num3516 = speed / num3516;
                        NPC.velocity.Y = targetY * num3516;
                        NPC.ai[1] = 2f;
                        NPC.netUpdate = true;
                        if (NPC.netSpam > 10)
                        {
                            NPC.netSpam = 10;
                        }
                    }
                    NPC.ai[2]++;
                    if (NPC.ai[2] > 140)
                    {
                        _glowColor = _glowDefault;
                        NPC.position.Y = Main.player[NPC.target].position.Y - 600;
                        NPC.velocity = Vector2.Zero;
                        NPC.ai[2] = 0;
                        NPC.ai[0] = 0;
                        _playAnimation = true;

                        if (_stage > 2)
						{
                            if (!isFireBreath)
							{
                                isFireBreath = true;
                                NPC.ai[1] = 0;
                            }
                            else NPC.ai[1] = 3;
                        }
                        else NPC.ai[1] = 0;
                    }
                    break;
                // Dragons breath:
                // Fly to the left or right side of the player,
                // Fly down a bit and stand still/hover
                // Shoot dragons breath at the player
                case 3:
                    speed = Main.expertMode ? .075f : .065f; // set default speed
                    targetHight = 250;

                    vector444 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                    targetX = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - (NPC.ai[3] * 110) - vector444.X;
                    targetY = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - targetHight - vector444.Y;

                    NPC.ai[2]++;
                    if (NPC.ai[2] == 1)
                    {
                        NPC.ai[3] = NPC.direction;

                        NPC.position.Y = Main.player[NPC.target].position.Y - 600;
                        NPC.position.X = Main.player[NPC.target].position.X - (NPC.ai[3] * 200);
                    }
                    else if (NPC.ai[2] > 1 && NPC.ai[2] < 300)
                    {
                        // Dragons breath
                        float shootSpeed = 7;
                        if (NPC.ai[2] > 160 && NPC.ai[2] < 245)
                        {
                            speed = .007f;
                            if (NPC.ai[2] % 4 == 0)
                            {
                                SoundEngine.PlaySound(SoundID.Item20, NPC.position);
                                Vector2 vector8 = new Vector2(NPC.position.X + (NPC.direction == 1 ? NPC.width - 50 : 50), NPC.position.Y + 60);

                                float rotation = (float)Math.Atan2(vector8.Y - (targetPlayer.Center.Y), vector8.X - (targetPlayer.Center.X)) * Main.rand.NextFloat(.85f, 1.15f);

                                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector8.X, vector8.Y, (float)(-(Math.Cos(rotation) * shootSpeed)), (float)(-(Math.Sin(rotation) * shootSpeed)), ProjectileID.FlamesTrap /*ModContent.ProjectileType<TerrorBreath>()*/, (int)(NPC.damage / 1.5f), 0f);
                                Main.projectile[proj].friendly = false;
                            }
                        }

                        // Movement
                        if (NPC.velocity.X < targetX)
                        {
                            NPC.velocity.X = NPC.velocity.X + speed;
                            NPC.direction = 1;
                            if (NPC.velocity.X < 0f && targetX > 0f)
                            {
                                NPC.velocity.X = NPC.velocity.X + speed;
                            }
                        }
                        else if (NPC.velocity.X > targetX)
                        {
                            NPC.velocity.X = NPC.velocity.X - speed;
                            NPC.direction = -1;
                            if (NPC.velocity.X > 0f && targetX < 0f)
                            {
                                NPC.velocity.X = NPC.velocity.X - speed;
                            }
                        }
                        if (NPC.velocity.Y < targetY)
                        {
                            NPC.velocity.Y = NPC.velocity.Y + speed;
                            if (NPC.velocity.Y < 0f && targetY > 0f)
                            {
                                NPC.velocity.Y = NPC.velocity.Y + speed;
                            }
                        }
                        else if (NPC.velocity.Y > targetY)
                        {
                            NPC.velocity.Y = NPC.velocity.Y - speed;
                            if (NPC.velocity.Y > 0f && targetY < 0f)
                            {
                                NPC.velocity.Y = NPC.velocity.Y - speed;
                            }
                        }

                        clamp = 9;
                        NPC.velocity = Vector2.Clamp(NPC.velocity,
                            new Vector2(-clamp, -clamp),
                            new Vector2(clamp, clamp));
                    }
                    else if (NPC.ai[2] > 300)
                    {
                        NPC.ai[1] = 0;
                        NPC.ai[2] = 0;
                        isFireBreath = false;
                    }
                    break;
            }

        }

        private void SetStage()
		{
            if (NPC.life <= (NPC.lifeMax * .35f))
                _stage = 4;
            else if (NPC.life <= (NPC.lifeMax * .65f))
                _stage = 3;
            else if (NPC.life <= (NPC.lifeMax * .8f))
                _stage = 2;
        }

        private Color _glowDefault = new Color(180, 180, 180, 245);
        private Color _glowColor;
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
            /*Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            //Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 1.4f / Main.npcFrameCount[NPC.type]);

            Vector2 drawPos = NPC.oldPos[6] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
            Color color = NPC.GetAlpha(_glowColor) * ((float)(NPC.oldPos.Length - 6) / (float)NPC.oldPos.Length);
            spriteBatch.Draw(texture, drawPos, NPC.frame, color, NPC.rotation, drawOrigin, 1, effects, 1);

            //Vector2 drawOrigin2 = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
            Vector2 drawOrigin2 = new Vector2(texture.Width / 2, texture.Height / 1.4f / Main.npcFrameCount[NPC.type]);

            Vector2 drawPos2 = NPC.oldPos[3] - Main.screenPosition + drawOrigin2 + new Vector2(0f, NPC.gfxOffY);
            Color color2 = NPC.GetAlpha(_glowColor) * ((float)(NPC.oldPos.Length - 3) / (float)NPC.oldPos.Length);
            spriteBatch.Draw(texture, drawPos2, NPC.frame, color2, NPC.rotation, drawOrigin, 1, effects, 1);*/

            // Main glowmask
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 drawPos = NPC.Center - Main.screenPosition;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 1.4f / Main.npcFrameCount[NPC.type]);

            SpriteEffects effects = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(texture, drawPos, NPC.frame, _glowDefault, 0f, drawOrigin, 1f, effects, 0f);

            /*texture = _glowMaskEyes;
            drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);

            effects = npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(texture, drawPos, npc.frame, Color.Purple, 0f, drawOrigin, 1f, effects, 1f);*/

            /*texture = ModContent.GetTexture(this.GetPath() + "/GlowMask_Eyes");
            // https://forums.terraria.org/index.php?threads/glowmaskapi-a-library-to-help-you-easily-add-a-glowmask-to-your-items.78033/
            // https://forums.terraria.org/index.php?threads/a-beginners-guide-to-shaders.86128/
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    npc.position.X - Main.screenPosition.X + npc.width * 0.5f,
                    npc.position.Y - Main.screenPosition.Y + npc.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                npc.rotation,
                texture.Size() * 0.5f,
                npc.scale,
                SpriteEffects.None,
                0f
            );*/

            SetStage();
            return NPC.IsABestiaryIconDummy;
		}
        private bool _playAnimation = true;
		public override void FindFrame(int frameHeight)
        {
            if (_playAnimation)
			{
                NPC.frameCounter += 1;
                NPC.frameCounter %= 30;

                int frame = (int)(NPC.frameCounter / 6.7);
                if (frame >= Main.npcFrameCount[NPC.type] - 1) frame = 0;
                NPC.frame.Y = frame * frameHeight;
            }
            else
			{
                int frame = (int)(_dashFrame / 6.7);
                NPC.frame.Y = frame * frameHeight;
            }
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }
    }
}
