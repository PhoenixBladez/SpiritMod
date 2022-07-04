using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using SpiritMod.NPCs.DarkfeatherMage.Projectiles;
using System;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;

namespace SpiritMod.NPCs.DarkfeatherMage
{
	public class DarkfeatherMage : ModNPC
	{
        Vector2 pos;
		float degrees = 0;
		float num384 = 0f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Darkfeather Mage");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.width = 66;
			NPC.height = 60;
			NPC.damage = 16;
			NPC.defense = 15;
			NPC.rarity = 3;
			NPC.lifeMax = 600;
            NPC.knockBackResist = .05f;
            NPC.noGravity = true;
			NPC.value = 1320f;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.noTileCollide = false;
			NPC.HitSound = SoundID.DD2_GoblinBomberDeath;
			NPC.DeathSound = SoundID.DD2_GoblinBomberHurt;
		}

        public override bool PreKill()
        {
            SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/DownedMiniboss"), NPC.Center);
            return true;
        }

        public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
            Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.Center.Y + (float)(NPC.height / 2)) / 16f), 0.162f*2, 0.209f*2, .02f*2);
            Player player = Main.player[NPC.target];

            NPC.ai[0]++;
            if (!Main.player[NPC.target].dead && NPC.ai[1] < 2f)
            {
                if (NPC.collideX)
                {
                    NPC.velocity.X = NPC.oldVelocity.X * -0.5f;
                    if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
                        NPC.velocity.X = 2f;

                    if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
                        NPC.velocity.X = -2f;
                }
                if (NPC.collideY)
                {
                    NPC.velocity.Y = NPC.oldVelocity.Y * -0.5f;
                    if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f)
                        NPC.velocity.Y = 1f;

                    if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f)
                        NPC.velocity.Y = -1f;
                }
                NPC.TargetClosest(true);
                if (NPC.direction == -1 && NPC.velocity.X > -4f)
                {
                    NPC.velocity.X = NPC.velocity.X - 0.21f;
                    if (NPC.velocity.X > 4f)
                        NPC.velocity.X = NPC.velocity.X - 0.21f;
                    else if (NPC.velocity.X > 0f)
                        NPC.velocity.X = NPC.velocity.X - 0.05f;

                    if (NPC.velocity.X < -4f)
                        NPC.velocity.X = -4f;
                }
                else if (NPC.direction == 1 && NPC.velocity.X < 4f)
                {
                    NPC.velocity.X = NPC.velocity.X + 0.21f;

                    if (NPC.velocity.X < -4f)
                        NPC.velocity.X = NPC.velocity.X + 0.21f;
                    else if (NPC.velocity.X < 0f)
                        NPC.velocity.X = NPC.velocity.X + 0.05f;

                    if (NPC.velocity.X > 4f)
                        NPC.velocity.X = 4f;
                }
                float num3225 = Math.Abs(NPC.position.X + (float)(NPC.width / 2) - (Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2)));
                float num3224 = Main.player[NPC.target].position.Y - (float)(NPC.height / 2);
                if (num3225 > 50f)
                    num3224 -= 150f;

                if (NPC.position.Y < num3224)
                {
                    NPC.velocity.Y = NPC.velocity.Y + 0.05f;
                    if (NPC.velocity.Y < 0f)
                        NPC.velocity.Y = NPC.velocity.Y + 0.01f;
                }
                else
                {
                    NPC.velocity.Y = NPC.velocity.Y - 0.05f;
                    if (NPC.velocity.Y > 0f)
                        NPC.velocity.Y = NPC.velocity.Y - 0.01f;
                }

                if (NPC.velocity.Y < -3f)
                    NPC.velocity.Y = -3f;

                if (NPC.velocity.Y > 3f)
                    NPC.velocity.Y = 3f;
            }
            Vector2 direction = Main.player[NPC.target].Center - NPC.Center;

            if (NPC.ai[0] == 180)
            {
				switch (Main.rand.Next(4))
                {
                    case 0:
                        NPC.ai[1] = 1f;
                        NPC.netUpdate = true;
                        break;
                    case 1:
                        pos = new Vector2(NPC.Center.X + Main.rand.Next(-150, 150), NPC.Center.Y);
                        NPC.ai[1] = 2f;
                        NPC.netUpdate = true;
                        break;
                    case 2:
                        NPC.velocity.Y -= 8f;
                        if (Math.Sign(NPC.Center.X - player.Center.X) < 0)
                            num384 = Main.player[NPC.target].position.X + 200 + (Main.player[NPC.target].width / 2) - NPC.Center.X;
                        else
                            num384 = Main.player[NPC.target].position.X - 200 + (Main.player[NPC.target].width / 2) - NPC.Center.X;
                        NPC.ai[1] = 3f;
                        break;
                    case 3:
                        NPC.ai[1] = 4f;
                        break;
                }
            }
			if (NPC.ai[1] == 1f)
            {
                if (NPC.ai[0] % 90 == 0)
                {
                    Teleport();
                    SoundEngine.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, NPC.Center);
                }
				if (NPC.ai[0] > 400f)
                {
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 0f;
                    NPC.netUpdate = true;
                }
            }
			if (NPC.ai[1] == 2f)
            {
                NPC.noTileCollide = true;
                Vector2 target = Vector2.Zero;
                double deg = (double)NPC.ai[2]; //The degrees, you can multiply npc.ai[1] to make it orbit faster, may be choppy depending on the value
                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                double dist = 250; //Distance away from the player
                
                /*Position the npc based on where the player is, the Sin/Cos of the angle times the /
                /distance for the desired distance away from the player minus the npc's width   /
                /and height divided by two so the center of the npc is at the right place.     */
                target.X = pos.X - (int)(Math.Cos(rad) * dist) - NPC.width / 2;
                target.Y = pos.Y - (int)(Math.Sin(rad) * dist) - NPC.height / 2;

                //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
                NPC.ai[2] += 2.5f;
                Vector2 Vel = target - NPC.Center;
                Vel.Normalize();
                Vel *= 6f;
                NPC.velocity = Vel;
                if (NPC.ai[0] % 30 == 0 && NPC.ai[0] != 180 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, new Vector2(NPC.Center.X + 18 * NPC.spriteDirection, NPC.Center.Y + 12));
                    for (int j = 0; j < 24; j++)
                    {
                        Vector2 vector2 = Vector2.UnitX * -NPC.width / 2f;
                        vector2 += -Utils.RotatedBy(Vector2.UnitY, (j * MathHelper.Pi / 6f), default) * new Vector2(16f);
                        vector2 = Utils.RotatedBy(vector2, (NPC.rotation - MathHelper.PiOver2), default) * 1.3f;
                        int num8 = Dust.NewDust(new Vector2(NPC.Center.X + 18 * NPC.spriteDirection, NPC.Center.Y + 12), 0, 0, DustID.Teleporter, 0f, 0f, 160, new Color(209, 255, 0), 1f);
                        Main.dust[num8].shader = GameShaders.Armor.GetSecondaryShader(67, Main.LocalPlayer);
                        Main.dust[num8].position = NPC.Center + vector2;
                        Main.dust[num8].velocity = NPC.velocity * 0.1f;
                        Main.dust[num8].velocity = Vector2.Normalize(NPC.Center - NPC.velocity * 3f - Main.dust[num8].position) * 1.25f;
                    }
                    direction.Normalize();
                    direction.X *= 8f;
                    direction.Y *= 8f;
                    bool expertMode = Main.expertMode;
                    int damage = expertMode ? 11 : 18;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X, direction.Y, ModContent.ProjectileType<DarkfeatherBoltRegular>(), damage, 1, Main.myPlayer, 0, 0);
                }
                if (NPC.ai[0] > 420f)
                {
                    NPC.noTileCollide = false;
                    NPC.ai[0] = 0f;
                    NPC.ai[1] = 0f;
                    NPC.netUpdate = true;
                }
            }
            if (NPC.ai[1] == 3f)
            {
                float num383 = 9f;
                float num385 = Main.player[NPC.target].Center.Y - NPC.Center.Y;
                float num386 = (float)Math.Sqrt((double)(num384 * num384 + num385 * num385));
                num386 = num383 / num386;
                NPC.velocity.X = num384 * num386;
                NPC.velocity.Y *= 0f;
				if (NPC.ai[0] % 22 == 0 && NPC.ai[0] != 180)
                {
                    SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/CoilRocket"), NPC.Center);
                    int damage = Main.expertMode ? 13 : 20;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + 21 * NPC.direction, NPC.Center.Y + 12, Main.rand.Next(0, 2) * NPC.spriteDirection, -1, ModContent.ProjectileType<DarkfeatherBomb>(), damage, 1, Main.myPlayer, 0, 0);
                }
                if (NPC.ai[0] > 270)
                {
                    NPC.ai[1] = 0f;
                    NPC.ai[0] = 0f;
                    NPC.netUpdate = true;
                }
            }
			if (NPC.ai[1] == 4f)
            {
                NPC.noTileCollide = true;
                if (NPC.ai[0] % 45 == 0)
                {
                    direction.Normalize();
                    SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, NPC.Center);
                    direction.X = direction.X * Main.rand.Next(12, 16);
                    direction.Y = direction.Y * Main.rand.Next(6, 9);
                    NPC.velocity.X = direction.X;
                    NPC.velocity.Y = direction.Y;
                }
				if (NPC.ai[0] % 50 == 0)
                {
                    SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, new Vector2(NPC.Center.X + 18 * NPC.spriteDirection, NPC.Center.Y + 12));
                    for (int j = 0; j < 24; j++)
                    {
                        Vector2 vector2 = Vector2.UnitX * -NPC.width / 2f;
                        vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(16f, 16f);
                        vector2 = Utils.RotatedBy(vector2, (NPC.rotation - 1.57079637f), default) * 1.3f;
                        int num8 = Dust.NewDust(new Vector2(NPC.Center.X + 21 * NPC.spriteDirection, NPC.Center.Y + 12), 0, 0, DustID.ChlorophyteWeapon, 0f, 0f, 160, new Color(209, 255, 0), .86f);
                        Main.dust[num8].shader = GameShaders.Armor.GetSecondaryShader(69, Main.LocalPlayer);
                        Main.dust[num8].position = new Vector2(NPC.Center.X + 21 * NPC.spriteDirection, NPC.Center.Y + 12) + vector2;
                        Main.dust[num8].velocity = NPC.velocity * 0.1f;
                        Main.dust[num8].noGravity = true;
                        Main.dust[num8].velocity = Vector2.Normalize(NPC.Center - NPC.velocity * 3f - Main.dust[num8].position) * 1.25f;
                    }
                    direction.Normalize();
                    direction.X *= 9f;
                    direction.Y *= 9f;
                    bool expertMode = Main.expertMode;
                    int damage = expertMode ? 11 : 18;
                    int amountOfProjectiles = Main.rand.Next(2, 4);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < amountOfProjectiles; ++i)
                        {
                            float A = (float)Main.rand.Next(-150, 150) * 0.01f;
                            float B = (float)Main.rand.Next(-150, 150) * 0.01f;
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + 21 * NPC.direction, NPC.Center.Y + 12, direction.X + A, direction.Y + B, ModContent.ProjectileType<DarkfeatherBoltRegular>(), damage, 1, Main.myPlayer, 0, 0);
                        }
                    }
                }
				if (NPC.ai[0] > 300)
                {
                    NPC.noTileCollide = false;
                    NPC.ai[1] = 0f;
                    NPC.ai[0] = 0f;
                    NPC.netUpdate = true;
                }
            }
            int num184 = (int)(NPC.Center.X / 16f);
			int num185 = (int)(NPC.Center.Y / 16f);

            if (NPC.life > NPC.lifeMax * .15f)
            {
                if ((Main.tile[num184, num185].HasUnactuatedTile && (Main.tileSolid[Main.tile[num184, num185].TileType] || Main.tileSolidTop[Main.tile[num184, num185].TileType])) && (NPC.ai[1] != 2f && NPC.ai[1] != 4f))
                {
                    Teleport();
                    SoundEngine.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, NPC.Center);
                }
            }
			else
            {
                if ((Main.tile[num184, num185].HasUnactuatedTile && (Main.tileSolid[Main.tile[num184, num185].TileType] || Main.tileSolidTop[Main.tile[num184, num185].TileType])) && (NPC.ai[1] != 2f || NPC.ai[1] != 4f))
                {
                    Teleport();
                    SoundEngine.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, NPC.Center);
                }
            }
        }

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<Mechanics.Fathomless_Chest.Mystical_Dice>();
			npcLoot.AddCommon<Items.Accessory.DarkfeatherVisage.DarkfeatherVisage>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			bool valid = spawnInfo.Player.ZoneOverworldHeight && !NPC.AnyNPCs(ModContent.NPCType<DarkfeatherMage>()) && (spawnInfo.SpawnTileX < Main.maxTilesX / 3 || spawnInfo.SpawnTileX > Main.maxTilesX / 1.5f);
			if (!valid)
				return 0f;
			if (QuestManager.GetQuest<ManicMage>().IsActive)
				return 0.5f;
			return 0.0005f;
		}

		public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("DarkfeatherMage3").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("DarkfeatherMage").Type, 1f);
                for (int k = 0; k < 6; k++)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("DarkfeatherMage1").Type, Main.rand.NextFloat(.6f, 1f));
                for (int z = 0; z < 2; z++)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("DarkfeatherMage2").Type, Main.rand.NextFloat(.8f, 1f));
            }
        }

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => NPC.lifeMax = (int)(NPC.lifeMax * 0.5f * bossLifeScale);

		public void Teleport()
        {
            float swirlSize = 1.664f;
            Player player = Main.player[NPC.target];
            NPC.position.X = player.position.X + Main.rand.Next(-150, 150);
            NPC.position.Y = player.position.Y - 300f;
            NPC.netUpdate = true;
            float Closeness = 50f;
            degrees += 2.5f;
            for (float swirlDegrees = degrees; swirlDegrees < 160 + degrees; swirlDegrees += 7f)
            {
                Closeness -= swirlSize; //It closes in
                double radians = swirlDegrees * (Math.PI / 180);
                Vector2 eastPosFar = NPC.Center + new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));
                Vector2 westPosFar = NPC.Center - new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));
                Vector2 northPosFar = NPC.Center + new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
                Vector2 southPosFar = NPC.Center - new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
				Vector2[] pos = new Vector2[] { eastPosFar, westPosFar, northPosFar, southPosFar };
				for (int i = 0; i < pos.Length; ++i)
				{
					int d4 = Dust.NewDust(pos[i], 2, 2, DustID.Teleporter, 0f, 0f, 0, new Color(209, 255, 0), 1f);
					Main.dust[d4].shader = GameShaders.Armor.GetSecondaryShader(67, Main.LocalPlayer);
				}

                Vector2 eastPosClose = NPC.Center + new Vector2((Closeness - 30f) * (float)Math.Sin(radians), (Closeness - 30f) * (float)Math.Cos(radians));
                Vector2 westPosClose = NPC.Center - new Vector2((Closeness - 30f) * (float)Math.Sin(radians), (Closeness - 30f) * (float)Math.Cos(radians));
                Vector2 northPosClose = NPC.Center + new Vector2((Closeness - 30f) * (float)Math.Sin(radians + 1.57), (Closeness - 30f) * (float)Math.Cos(radians + 1.57));
                Vector2 southPosClose = NPC.Center - new Vector2((Closeness - 30f) * (float)Math.Sin(radians + 1.57), (Closeness - 30f) * (float)Math.Cos(radians + 1.57));
				pos = new Vector2[] { eastPosClose, westPosClose, northPosClose, southPosClose };
				for (int i = 0; i < pos.Length; ++i)
				{
					int d = Dust.NewDust(eastPosClose, 2, 2, DustID.Teleporter, 0f, 0f, 0, new Color(209, 255, 0), 1f);
					Main.dust[d].shader = GameShaders.Armor.GetSecondaryShader(67, Main.LocalPlayer);
				}
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            var effects = NPC.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
            Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height / Main.npcFrameCount[NPC.type]) * 0.5f);
            drawOrigin.Y += 13f;
            Vector2 position1 = NPC.Bottom - Main.screenPosition;
            Texture2D texture2D2 = TextureAssets.GlowMask[239].Value;
            float num11 = (float)((double)Main.GlobalTimeWrappedHourly % 1.0 / 1.0);
            float num12 = num11;
            if ((double)num12 > 0.5)
                num12 = 1f - num11;
            if ((double)num12 < 0.0)
                num12 = 0.0f;
            float num13 = (float)(((double)num11 + 0.5) % 1.0);
            float num14 = num13;
            if ((double)num14 > 0.5)
                num14 = 1f - num13;
            if ((double)num14 < 0.0)
                num14 = 0.0f;
            Rectangle r2 = texture2D2.Frame(1, 1, 0, 0);
            drawOrigin = r2.Size() / 2f;
            Vector2 position3 = position1 + new Vector2(22.0f * NPC.spriteDirection, -1f);
            Color color3 = new Color(137, 209, 61) * 1.6f;
            Main.spriteBatch.Draw(texture2D2, position3, new Rectangle?(r2), color3, NPC.rotation, drawOrigin, NPC.scale * 0.275f, SpriteEffects.FlipHorizontally, 0.0f);
            float num15 = 1f + num11 * 0.75f;
            Main.spriteBatch.Draw(texture2D2, position3, new Rectangle?(r2), color3 * num12, NPC.rotation, drawOrigin, NPC.scale * 0.275f * num15, SpriteEffects.FlipHorizontally, 0.0f);
            float num16 = 1f + num13 * 0.75f;
            Main.spriteBatch.Draw(texture2D2, position3, new Rectangle?(r2), color3 * num14, NPC.rotation, drawOrigin, NPC.scale * 0.275f * num16, SpriteEffects.FlipHorizontally, 0.0f);
            return false;
        }

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/DarkfeatherMage/DarkfeatherMage_Glow").Value);

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.25f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}
	}
}
