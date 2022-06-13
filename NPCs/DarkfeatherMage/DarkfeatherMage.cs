using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 66;
			npc.height = 60;
			npc.damage = 16;
			npc.defense = 15;
			npc.rarity = 3;
			npc.lifeMax = 600;
            npc.knockBackResist = .05f;
            npc.noGravity = true;
			npc.value = 1320f;
			npc.buffImmune[BuffID.Confused] = true;
			npc.noTileCollide = false;
			npc.HitSound = SoundID.DD2_GoblinBomberDeath;
			npc.DeathSound = SoundID.DD2_GoblinBomberHurt;
		}

        public override bool PreNPCLoot()
        {
            Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/DownedMiniboss"));
            return true;
        }

        public override void AI()
		{
			npc.spriteDirection = npc.direction;
            Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.Center.Y + (float)(npc.height / 2)) / 16f), 0.162f*2, 0.209f*2, .02f*2);
            Player player = Main.player[npc.target];

            npc.ai[0]++;
            if (!Main.player[npc.target].dead && npc.ai[1] < 2f)
            {
                if (npc.collideX)
                {
                    npc.velocity.X = npc.oldVelocity.X * -0.5f;
                    if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f)
                        npc.velocity.X = 2f;

                    if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f)
                        npc.velocity.X = -2f;
                }
                if (npc.collideY)
                {
                    npc.velocity.Y = npc.oldVelocity.Y * -0.5f;
                    if (npc.velocity.Y > 0f && npc.velocity.Y < 1f)
                        npc.velocity.Y = 1f;

                    if (npc.velocity.Y < 0f && npc.velocity.Y > -1f)
                        npc.velocity.Y = -1f;
                }
                npc.TargetClosest(true);
                if (npc.direction == -1 && npc.velocity.X > -4f)
                {
                    npc.velocity.X = npc.velocity.X - 0.21f;
                    if (npc.velocity.X > 4f)
                        npc.velocity.X = npc.velocity.X - 0.21f;
                    else if (npc.velocity.X > 0f)
                        npc.velocity.X = npc.velocity.X - 0.05f;

                    if (npc.velocity.X < -4f)
                        npc.velocity.X = -4f;
                }
                else if (npc.direction == 1 && npc.velocity.X < 4f)
                {
                    npc.velocity.X = npc.velocity.X + 0.21f;

                    if (npc.velocity.X < -4f)
                        npc.velocity.X = npc.velocity.X + 0.21f;
                    else if (npc.velocity.X < 0f)
                        npc.velocity.X = npc.velocity.X + 0.05f;

                    if (npc.velocity.X > 4f)
                        npc.velocity.X = 4f;
                }
                float num3225 = Math.Abs(npc.position.X + (float)(npc.width / 2) - (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2)));
                float num3224 = Main.player[npc.target].position.Y - (float)(npc.height / 2);
                if (num3225 > 50f)
                    num3224 -= 150f;

                if (npc.position.Y < num3224)
                {
                    npc.velocity.Y = npc.velocity.Y + 0.05f;
                    if (npc.velocity.Y < 0f)
                        npc.velocity.Y = npc.velocity.Y + 0.01f;
                }
                else
                {
                    npc.velocity.Y = npc.velocity.Y - 0.05f;
                    if (npc.velocity.Y > 0f)
                        npc.velocity.Y = npc.velocity.Y - 0.01f;
                }

                if (npc.velocity.Y < -3f)
                    npc.velocity.Y = -3f;

                if (npc.velocity.Y > 3f)
                    npc.velocity.Y = 3f;
            }
            Vector2 direction = Main.player[npc.target].Center - npc.Center;

            if (npc.ai[0] == 180)
            {
				switch (Main.rand.Next(4))
                {
                    case 0:
                        npc.ai[1] = 1f;
                        npc.netUpdate = true;
                        break;
                    case 1:
                        pos = new Vector2(npc.Center.X + Main.rand.Next(-150, 150), npc.Center.Y);
                        npc.ai[1] = 2f;
                        npc.netUpdate = true;
                        break;
                    case 2:
                        npc.velocity.Y -= 8f;
                        if (Math.Sign(npc.Center.X - player.Center.X) < 0)
                            num384 = Main.player[npc.target].position.X + 200 + (Main.player[npc.target].width / 2) - npc.Center.X;
                        else
                            num384 = Main.player[npc.target].position.X - 200 + (Main.player[npc.target].width / 2) - npc.Center.X;
                        npc.ai[1] = 3f;
                        break;
                    case 3:
                        npc.ai[1] = 4f;
                        break;
                }
            }
			if (npc.ai[1] == 1f)
            {
                if (npc.ai[0] % 90 == 0)
                {
                    Teleport();
                    Main.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, npc.Center);
                }
				if (npc.ai[0] > 400f)
                {
                    npc.ai[0] = 0f;
                    npc.ai[1] = 0f;
                    npc.netUpdate = true;
                }
            }
			if (npc.ai[1] == 2f)
            {
                npc.noTileCollide = true;
                Vector2 target = Vector2.Zero;
                double deg = (double)npc.ai[2]; //The degrees, you can multiply npc.ai[1] to make it orbit faster, may be choppy depending on the value
                double rad = deg * (Math.PI / 180); //Convert degrees to radians
                double dist = 250; //Distance away from the player
                
                /*Position the npc based on where the player is, the Sin/Cos of the angle times the /
                /distance for the desired distance away from the player minus the npc's width   /
                /and height divided by two so the center of the npc is at the right place.     */
                target.X = pos.X - (int)(Math.Cos(rad) * dist) - npc.width / 2;
                target.Y = pos.Y - (int)(Math.Sin(rad) * dist) - npc.height / 2;

                //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
                npc.ai[2] += 2.5f;
                Vector2 Vel = target - npc.Center;
                Vel.Normalize();
                Vel *= 6f;
                npc.velocity = Vel;
                if (npc.ai[0] % 30 == 0 && npc.ai[0] != 180 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Main.PlaySound(SoundID.DD2_LightningBugZap, new Vector2(npc.Center.X + 18 * npc.spriteDirection, npc.Center.Y + 12));
                    for (int j = 0; j < 24; j++)
                    {
                        Vector2 vector2 = Vector2.UnitX * -npc.width / 2f;
                        vector2 += -Utils.RotatedBy(Vector2.UnitY, (j * MathHelper.Pi / 6f), default) * new Vector2(16f);
                        vector2 = Utils.RotatedBy(vector2, (npc.rotation - MathHelper.PiOver2), default) * 1.3f;
                        int num8 = Dust.NewDust(new Vector2(npc.Center.X + 18 * npc.spriteDirection, npc.Center.Y + 12), 0, 0, DustID.Teleporter, 0f, 0f, 160, new Color(209, 255, 0), 1f);
                        Main.dust[num8].shader = GameShaders.Armor.GetSecondaryShader(67, Main.LocalPlayer);
                        Main.dust[num8].position = npc.Center + vector2;
                        Main.dust[num8].velocity = npc.velocity * 0.1f;
                        Main.dust[num8].velocity = Vector2.Normalize(npc.Center - npc.velocity * 3f - Main.dust[num8].position) * 1.25f;
                    }
                    direction.Normalize();
                    direction.X *= 8f;
                    direction.Y *= 8f;
                    bool expertMode = Main.expertMode;
                    int damage = expertMode ? 11 : 18;
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X, direction.Y, ModContent.ProjectileType<DarkfeatherBoltRegular>(), damage, 1, Main.myPlayer, 0, 0);

                }
                if (npc.ai[0] > 420f)
                {
                    npc.noTileCollide = false;
                    npc.ai[0] = 0f;
                    npc.ai[1] = 0f;
                    npc.netUpdate = true;
                }
            }
            if (npc.ai[1] == 3f)
            {
                float num383 = 9f;
                float num385 = Main.player[npc.target].Center.Y - npc.Center.Y;
                float num386 = (float)Math.Sqrt((double)(num384 * num384 + num385 * num385));
                num386 = num383 / num386;
                npc.velocity.X = num384 * num386;
                npc.velocity.Y *= 0f;
				if (npc.ai[0] % 22 == 0 && npc.ai[0] != 180)
                {
                    Main.PlaySound(SoundLoader.customSoundType, npc.Center, mod.GetSoundSlot(SoundType.Custom, "Sounds/CoilRocket"));
                    int damage = Main.expertMode ? 13 : 20;
                    Projectile.NewProjectile(npc.Center.X + 21 * npc.direction, npc.Center.Y + 12, Main.rand.Next(0, 2) * npc.spriteDirection, -1, ModContent.ProjectileType<DarkfeatherBomb>(), damage, 1, Main.myPlayer, 0, 0);
                }
                if (npc.ai[0] > 270)
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 0f;
                    npc.netUpdate = true;
                }
            }
			if (npc.ai[1] == 4f)
            {
                npc.noTileCollide = true;
                if (npc.ai[0] % 45 == 0)
                {
                    direction.Normalize();
                    Main.PlaySound(SoundID.DD2_WyvernDiveDown, npc.Center);
                    direction.X = direction.X * Main.rand.Next(12, 16);
                    direction.Y = direction.Y * Main.rand.Next(6, 9);
                    npc.velocity.X = direction.X;
                    npc.velocity.Y = direction.Y;
                }
				if (npc.ai[0] % 50 == 0)
                {
                    Main.PlaySound(SoundID.DD2_LightningBugZap, new Vector2(npc.Center.X + 18 * npc.spriteDirection, npc.Center.Y + 12));
                    for (int j = 0; j < 24; j++)
                    {
                        Vector2 vector2 = Vector2.UnitX * -npc.width / 2f;
                        vector2 += -Utils.RotatedBy(Vector2.UnitY, ((float)j * 3.141591734f / 6f), default) * new Vector2(16f, 16f);
                        vector2 = Utils.RotatedBy(vector2, (npc.rotation - 1.57079637f), default) * 1.3f;
                        int num8 = Dust.NewDust(new Vector2(npc.Center.X + 21 * npc.spriteDirection, npc.Center.Y + 12), 0, 0, DustID.ChlorophyteWeapon, 0f, 0f, 160, new Color(209, 255, 0), .86f);
                        Main.dust[num8].shader = GameShaders.Armor.GetSecondaryShader(69, Main.LocalPlayer);
                        Main.dust[num8].position = new Vector2(npc.Center.X + 21 * npc.spriteDirection, npc.Center.Y + 12) + vector2;
                        Main.dust[num8].velocity = npc.velocity * 0.1f;
                        Main.dust[num8].noGravity = true;
                        Main.dust[num8].velocity = Vector2.Normalize(npc.Center - npc.velocity * 3f - Main.dust[num8].position) * 1.25f;
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
                            Projectile.NewProjectile(npc.Center.X + 21 * npc.direction, npc.Center.Y + 12, direction.X + A, direction.Y + B, ModContent.ProjectileType<DarkfeatherBoltRegular>(), damage, 1, Main.myPlayer, 0, 0);
                        }
                    }
                }
				if (npc.ai[0] > 300)
                {
                    npc.noTileCollide = false;
                    npc.ai[1] = 0f;
                    npc.ai[0] = 0f;
                    npc.netUpdate = true;
                }
            }
            int num184 = (int)(npc.Center.X / 16f);
			int num185 = (int)(npc.Center.Y / 16f);

            if (npc.life > npc.lifeMax * .15f)
            {
                if (Main.tile[num184, num185] != null && (Main.tile[num184, num185].nactive() && (Main.tileSolid[Main.tile[num184, num185].type] || Main.tileSolidTop[Main.tile[num184, num185].type])) && (npc.ai[1] != 2f && npc.ai[1] != 4f))
                {
                    Teleport();
                    Main.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, npc.Center);
                }
            }
			else
            {
                if (Main.tile[num184, num185] != null && (Main.tile[num184, num185].nactive() && (Main.tileSolid[Main.tile[num184, num185].type] || Main.tileSolidTop[Main.tile[num184, num185].type])) && (npc.ai[1] != 2f || npc.ai[1] != 4f))
                {
                    Teleport();
                    Main.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, npc.Center);
                }
            }
        }

        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessory.DarkfeatherVisage.DarkfeatherVisage>());
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Mechanics.Fathomless_Chest.Mystical_Dice>());
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			bool valid = spawnInfo.player.ZoneOverworldHeight && !NPC.AnyNPCs(ModContent.NPCType<DarkfeatherMage>()) && (spawnInfo.spawnTileX < Main.maxTilesX / 3 || spawnInfo.spawnTileX > Main.maxTilesX / 1.5f) && NPC.downedBoss2;
			if (!valid)
				return 0f;
			if (QuestManager.GetQuest<ManicMage>().IsActive)
				return 0.5f;
			return 0.0005f;
		}

		public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Darkfeather/DarkfeatherMage3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Darkfeather/DarkfeatherMage4"), 1f);
                for (int k = 0; k < 6; k++)
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Darkfeather/DarkfeatherMage1"), Main.rand.NextFloat(.6f, 1f));
                for (int z = 0; z < 2; z++)
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Darkfeather/DarkfeatherMage2"), Main.rand.NextFloat(.8f, 1f));
            }
        }

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);

		public void Teleport()
        {
            float swirlSize = 1.664f;
            Player player = Main.player[npc.target];
            npc.position.X = player.position.X + Main.rand.Next(-150, 150);
            npc.position.Y = player.position.Y - 300f;
            npc.netUpdate = true;
            float Closeness = 50f;
            degrees += 2.5f;
            for (float swirlDegrees = degrees; swirlDegrees < 160 + degrees; swirlDegrees += 7f)
            {
                Closeness -= swirlSize; //It closes in
                double radians = swirlDegrees * (Math.PI / 180);
                Vector2 eastPosFar = npc.Center + new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));
                Vector2 westPosFar = npc.Center - new Vector2(Closeness * (float)Math.Sin(radians), Closeness * (float)Math.Cos(radians));
                Vector2 northPosFar = npc.Center + new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
                Vector2 southPosFar = npc.Center - new Vector2(Closeness * (float)Math.Sin(radians + 1.57), Closeness * (float)Math.Cos(radians + 1.57));
				Vector2[] pos = new Vector2[] { eastPosFar, westPosFar, northPosFar, southPosFar };
				for (int i = 0; i < pos.Length; ++i)
				{
					int d4 = Dust.NewDust(pos[i], 2, 2, DustID.Teleporter, 0f, 0f, 0, new Color(209, 255, 0), 1f);
					Main.dust[d4].shader = GameShaders.Armor.GetSecondaryShader(67, Main.LocalPlayer);
				}

                Vector2 eastPosClose = npc.Center + new Vector2((Closeness - 30f) * (float)Math.Sin(radians), (Closeness - 30f) * (float)Math.Cos(radians));
                Vector2 westPosClose = npc.Center - new Vector2((Closeness - 30f) * (float)Math.Sin(radians), (Closeness - 30f) * (float)Math.Cos(radians));
                Vector2 northPosClose = npc.Center + new Vector2((Closeness - 30f) * (float)Math.Sin(radians + 1.57), (Closeness - 30f) * (float)Math.Cos(radians + 1.57));
                Vector2 southPosClose = npc.Center - new Vector2((Closeness - 30f) * (float)Math.Sin(radians + 1.57), (Closeness - 30f) * (float)Math.Cos(radians + 1.57));
				pos = new Vector2[] { eastPosClose, westPosClose, northPosClose, southPosClose };
				for (int i = 0; i < pos.Length; ++i)
				{
					int d = Dust.NewDust(eastPosClose, 2, 2, DustID.Teleporter, 0f, 0f, 0, new Color(209, 255, 0), 1f);
					Main.dust[d].shader = GameShaders.Armor.GetSecondaryShader(67, Main.LocalPlayer);
				}
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            var effects = npc.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
            drawOrigin.Y += 13f;
            Vector2 position1 = npc.Bottom - Main.screenPosition;
            Texture2D texture2D2 = Main.glowMaskTexture[239];
            float num11 = (float)((double)Main.GlobalTime % 1.0 / 1.0);
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
            Vector2 position3 = position1 + new Vector2(22.0f * npc.spriteDirection, -1f);
            Color color3 = new Color(137, 209, 61) * 1.6f;
            Main.spriteBatch.Draw(texture2D2, position3, new Rectangle?(r2), color3, npc.rotation, drawOrigin, npc.scale * 0.275f, SpriteEffects.FlipHorizontally, 0.0f);
            float num15 = 1f + num11 * 0.75f;
            Main.spriteBatch.Draw(texture2D2, position3, new Rectangle?(r2), color3 * num12, npc.rotation, drawOrigin, npc.scale * 0.275f * num15, SpriteEffects.FlipHorizontally, 0.0f);
            float num16 = 1f + num13 * 0.75f;
            Main.spriteBatch.Draw(texture2D2, position3, new Rectangle?(r2), color3 * num14, npc.rotation, drawOrigin, npc.scale * 0.275f * num16, SpriteEffects.FlipHorizontally, 0.0f);
            return false;
        }

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/DarkfeatherMage/DarkfeatherMage_Glow"));

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
	}
}
