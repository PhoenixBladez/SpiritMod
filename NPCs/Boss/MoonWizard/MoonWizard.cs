using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.NPCs.Boss.MoonWizard.Projectiles;
using SpiritMod.Projectiles.Hostile;
using Terraria;
using SpiritMod.Buffs;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.MoonWizardDrops;
using SpiritMod.Items.Sets.MoonWizardDrops.JellynautHelmet;
using System.IO;
using SpiritMod.NPCs.MoonjellyEvent;
using SpiritMod.Items.Consumable;
using SpiritMod.Utilities;
using System.Collections.Generic;
using SpiritMod.Items.Placeable.MusicBox;
using SpiritMod.Items.Equipment;
using SpiritMod.Items.Consumable.Potion;

namespace SpiritMod.NPCs.Boss.MoonWizard
{
	[AutoloadBossHead]
	public class MoonWizard : ModNPC, IBCRegistrable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Jelly Wizard");
			Main.npcFrameCount[npc.type] = 21;
			NPCID.Sets.TrailCacheLength[npc.type] = 10;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 1600;
			npc.defense = 10;
			npc.value = 40000;
			npc.aiStyle = -1;
            bossBag = ModContent.ItemType<MJWBag>();
            npc.knockBackResist = 0f;
			npc.width = 17;
			npc.height = 35;
			npc.damage = 30;
            npc.scale = 2f;
			npc.lavaImmune = true;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit13;
			npc.DeathSound = SoundID.NPCDeath2;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/MoonJelly");
            npc.boss = true;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => npc.lifeMax = (int)(npc.lifeMax * 0.8f * bossLifeScale);
		float trueFrame = 0;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(attackCounter);
			writer.Write(timeBetweenAttacks);
			writer.Write(phaseTwo);
			writer.Write(jellyRapidFire);
			writer.WriteVector2(dashDirection);
			writer.Write(dashDistance);
			writer.WriteVector2(TeleportPos);
			writer.Write(numMoves);
			writer.Write(node);
			writer.Write(SkyPos);
			writer.Write(SkyPosY);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			attackCounter = reader.ReadInt32();
			timeBetweenAttacks = reader.ReadInt32();
			phaseTwo = reader.ReadBoolean();
			jellyRapidFire = reader.ReadBoolean();
			dashDirection = reader.ReadVector2();
			dashDistance = reader.ReadSingle();
			TeleportPos = reader.ReadVector2();
			numMoves = reader.ReadInt32();
			node = reader.ReadInt32();
			SkyPos = reader.ReadInt32();
			SkyPosY = reader.ReadInt32();
		}
		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color color, float opacity, float startScale, float endScale) => DrawAfterImage(spriteBatch, offset, trailLengthModifier, color, color, opacity, startScale, endScale);
        public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color startColor, Color endColor, float opacity, float startScale, float endScale)
        {
            SpriteEffects spriteEffects = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (int i = 1; i < 10; i++)
            {
                Color color = Color.Lerp(startColor, endColor, i / 10f) * opacity;
                spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/MoonWizard_Afterimage"), new Vector2(npc.Center.X, npc.Center.Y) + offset - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)i * trailLengthModifier, npc.frame, color, npc.rotation, npc.frame.Size() * 0.5f, MathHelper.Lerp(startScale, endScale, i / 10f), spriteEffects, 0f);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (jellyRapidFire)
            {
                Color color1 = Lighting.GetColor((int)((double)npc.position.X + (double)npc.width * 0.5) / 16, (int)(((double)npc.position.Y + (double)npc.height * 0.5) / 16.0));
                Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);

                int r1 = (int)color1.R;
                drawOrigin.Y += 30f;
                drawOrigin.Y += 8f;
                --drawOrigin.X;
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
                Vector2 position3 = position1 + new Vector2(0.0f, -46f);
                Color color3 = new Color(87, 238, 255) * .8f;
                Main.spriteBatch.Draw(texture2D2, position3, new Rectangle?(r2), color3, npc.rotation, drawOrigin, npc.scale * .6f, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
                float num15 = 1f + num11 * 0.75f;
                Main.spriteBatch.Draw(texture2D2, position3, new Rectangle?(r2), color3 * num12, npc.rotation, drawOrigin, npc.scale * .6f * num15, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
                float num16 = 1f + num13 * 0.75f;
                Main.spriteBatch.Draw(texture2D2, position3, new Rectangle?(r2), color3 * num14, npc.rotation, drawOrigin, npc.scale * .6f * num16, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
                Texture2D texture2D3 = Main.extraTexture[89];
                Rectangle r3 = texture2D3.Frame(1, 1, 0, 0);
                drawOrigin = r3.Size() / 2f;
                Vector2 scale = new Vector2(0.75f, 1f + num16) * 1.5f;
                float num17 = 1f + num13 * 0.75f;

            }

            float num395 = Main.mouseTextColor / 200f - 0.35f;
            num395 *= 0.2f;
            float num366 = num395 + 2.45f;
            if (npc.velocity != Vector2.Zero || phaseTwo) {
                DrawAfterImage(Main.spriteBatch, new Vector2(0f, -18f), 0.5f, Color.White * .7f, Color.White * .1f, 0.75f, num366, 1.65f);
            }
			return false;
		}


		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var effects = npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], new Vector2(npc.Center.X, npc.Center.Y - 18) - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			DrawSpecialGlow(spriteBatch, lightColor);
		}
		public void DrawSpecialGlow(SpriteBatch spriteBatch, Color drawColor)
        {
            float num108 = 4;
            float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
            float num106 = 0f;

            SpriteEffects spriteEffects3 = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 vector33 = new Vector2(npc.Center.X, npc.Center.Y - 18) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity;
			Color color29 = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.LightBlue);
            for (int num103 = 0; num103 < 4; num103++)
            {
				Color color28 = color29;
                color28 = npc.GetAlpha(color28);
                color28 *= 1f - num107;
                Vector2 vector29 = new Vector2(npc.Center.X, npc.Center.Y -18) + ((float)num103 / (float)num108 * 6.28318548f + npc.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)num103;
                Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/MoonWizard_Glow"), vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
            }
            Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/MoonWizard_Glow"), vector33, npc.frame, color29, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);

        }

        int attackCounter;
		int timeBetweenAttacks = 120;
		
		bool phaseTwo = false;
		//0-3: idle
		//4-9 propelling
		//10-13 skirt up
		//14-21: turning
		//22-28: kick
		public override void AI()
        {
            Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);
            npc.TargetClosest();
			if (npc.ai[0] == 0) {
				attackCounter++;
				npc.velocity = Vector2.Zero; 
				npc.rotation = 0;
                if (npc.life > 250)
                {
                    UpdateFrame(0.15f, 0, 3);
                }
                else
                {
                    UpdateFrame(0.15f, 54, 61);
                }
                if (!phaseTwo && npc.life < npc.lifeMax / 3) {
					phaseTwo = true;
                    npc.ai[0] = 9;
                }
				if (attackCounter > timeBetweenAttacks) {
					attackCounter = 0;
					if (phaseTwo)
                    {
                        npc.ai[0] = Main.rand.Next(9) + 1;
                    }
					else
				    {
						npc.ai[0] = Main.rand.Next(7) + 1;
                    }

					npc.netUpdate = true;

					if (npc.ai[0] == 7 && Main.player[npc.target].velocity.Y > 0) {
						npc.ai[0] = 8;
					}

					Player player = Main.player[npc.target];
                    if (!player.active || player.dead || Main.dayTime)
                    {
                        npc.TargetClosest(false);
                        npc.velocity.Y = -200;
                        npc.active = false;
                    }
                    Vector2 dist = player.Center - npc.Center;
					if (dist.Length() > 1000) 
					{
						switch (Main.rand.Next(2)) {
							case 0:
								npc.ai[0] = 5;
								break;
							case 1:
								npc.ai[0] = 2;
								break;
						}

						npc.netUpdate = true;
					}
                    
				}
			}
			else 
			{
				switch (npc.ai[0]) {
					case 1: //teleport dash
						TeleportDash();
						break;
					case 2:
						Dash();
						break;
					case 3:
						SmashAttack();
						break;
					case 4:
						TeleportMove();
						break;
					case 5:
						SineAttack();
						break;
					case 6:
						KickAttack();
						break;
					case 7:
						SkyStrikeLeft();
						break;
					case 8:
						SkyStrikeRight();
						break;
                    case 9:
                        JellyfishRapidFire();
                        break;

				}

			}
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Width = 60;
			npc.frame.X = ((int)trueFrame % 3) * npc.frame.Width;
			npc.frame.Y = (((int)trueFrame - ((int)trueFrame % 3)) / 3) * npc.frame.Height;
		}

		public void UpdateFrame(float speed, int minFrame, int maxFrame)
		{
			trueFrame += speed;
			if (trueFrame < minFrame) 
			{
				trueFrame = minFrame;
			}
			if (trueFrame > maxFrame) 
			{
				trueFrame = minFrame;
			}
		}
		public override void BossLoot(ref string name, ref int potionType) => potionType = mod.ItemType("MoonJelly");
		public override void NPCLoot()
        {
            {
                if (Main.expertMode)
                {
                    npc.DropBossBags();
                    return;
                }

                npc.DropItem(ModContent.ItemType<MJWMask>(), 1f / 7);
                npc.DropItem(ModContent.ItemType<MJWTrophy>(), 1f / 10);
                int[] lootTable = {
                ModContent.ItemType<Moonshot>(),
                ModContent.ItemType<Moonburst>(),
                ModContent.ItemType<JellynautBubble>(),
                ModContent.ItemType<MoonjellySummonStaff>()
				};
                int loot = Main.rand.Next(lootTable.Length);
				int lunazoastack = Main.rand.Next(5, 7);
                npc.DropItem(lootTable[loot]);
				if (lootTable[loot] == ModContent.ItemType<Moonshot>())
					lunazoastack += Main.rand.Next(55, 75);

				npc.DropItem(ModContent.ItemType<TinyLunazoaItem>(), lunazoastack);
            }
        }

        public override void HitEffect(int hitDirection, double damage)
		{
            for (int k = 0; k < 20; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.27f);
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Granite, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.87f);
            }
			if (npc.life < 100)
            {
                Main.PlaySound(SoundID.NPCHit, npc.Center, 27);
                for (int i = 0; i < Main.rand.Next(1, 3); i++)
                {
                    int p = Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(2.2f)), mod.ProjectileType("MoonBubble"), 0, 3);
                    Main.projectile[p].scale = Main.rand.NextFloat(.2f, .4f);
                }
            }
			if (npc.life <= 0)
            {
                Gore.NewGore(new Vector2(npc.Center.X, npc.Center.Y - 50), new Vector2(0, 3), mod.GetGoreSlot("Gores/WizardHat_Gore"));
                for (int i = 0; i < Main.rand.Next(9, 15); i++)
                    Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(2.2f)), mod.ProjectileType("MoonBubble"), 0, 3);
            }
        }

		Vector2 dashDirection = Vector2.Zero;
		float dashDistance = 0f;
		int numMoves = -1;
		int node = 0;
        #region attacks
        bool jellyRapidFire = false;
		void JellyfishRapidFire()
        {
            jellyRapidFire = true;
            attackCounter++;
			if (attackCounter == 1)
            {
                Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/BossSFX/MoonWizard_Laugh2"));
            }
            if (attackCounter % 20 == 0 && attackCounter < 180 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Vector2 vector2_2 = Vector2.UnitY.RotatedByRandom(1.57079637050629f) * new Vector2(5f, 3f);
                bool expertMode = Main.expertMode;
                int p = Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-60, 60), npc.Center.Y + Main.rand.Next(-60, 60), vector2_2.X, vector2_2.Y, ModContent.ProjectileType<JellyfishOrbiter>(), NPCUtils.ToActualDamage(40, 1.5f), 0.0f, Main.myPlayer, 0.0f, (float)npc.whoAmI);
                Main.projectile[p].scale = Main.rand.NextFloat(.6f, 1f);
            }
            UpdateFrame(0.15f, 54, 61);
            if (attackCounter > 220)
            {
                jellyRapidFire = false;
                npc.ai[0] = 1;
                
                timeBetweenAttacks = 60;
                attackCounter = 0;
            }
			
        }
		int SkyPos = 0;
		int SkyPosY = 0;
		void SkyStrikeLeft()
        {
            if (npc.life > 250)
            {
                UpdateFrame(0.15f, 1, 3);
            }
            else
            {
                UpdateFrame(0.15f, 54, 61);
            }
			Player player = Main.player[npc.target];

			if (attackCounter == 0) {
                Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/BossSFX/MoonWizard_Laugh1"));
                SkyPos = (int)(player.position.X - 1000);
				SkyPosY = (int)(player.position.Y - 500);
			}
			if (attackCounter % 4 == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
				Vector2 strikePos = new Vector2(SkyPos, SkyPosY);
				Projectile.NewProjectile(strikePos, new Vector2(0, 0), mod.ProjectileType("SkyMoonZapper"), npc.damage / 3, 0, npc.target);
				SkyPos += 170;
			}
			if (attackCounter == 60) {
				npc.ai[0] = 1;
                
                timeBetweenAttacks = 0;
				attackCounter = 0;
			}
			attackCounter++;
		}
		void SkyStrikeRight()
		{
            if (npc.life > 250)
            {
                UpdateFrame(0.15f, 1, 3);
            }
            else
            {
                UpdateFrame(0.15f, 54, 61);
            }
            Player player = Main.player[npc.target];
			if (attackCounter == 0) {
				SkyPos = (int)(player.position.X + 1000);
				SkyPosY = (int)(player.position.Y - 500);
			}
			if (attackCounter % 4 == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
				Vector2 strikePos = new Vector2(SkyPos, SkyPosY);
				Projectile.NewProjectile(strikePos, new Vector2(0, 0), mod.ProjectileType("SkyMoonZapper"), npc.damage / 3, 0, npc.target);
				SkyPos -= 170;
			}
			if (attackCounter == 60) {
				npc.ai[0] = 1;
                
                timeBetweenAttacks = 0;
				attackCounter = 0;
			}
			attackCounter++;
		}
		void SineAttack()
		{
			Player player = Main.player[npc.target];
			UpdateFrame(0.15f, 10, 13);
			if (attackCounter == 10)
            {
                Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/BossSFX/MoonWizard_Laugh1"));
            }
			attackCounter++;
			if (attackCounter % 30 == 0) {
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 15);
			}
			if (attackCounter >= 30) 
			{
				if (attackCounter % 80 == 40) 
				{
					Vector2 direction = player.Center - (npc.Center - new Vector2(0, 60));
					direction.Normalize();
					direction *= 5;
					int proj = Projectile.NewProjectile(npc.Center - new Vector2(0, 60), direction, mod.ProjectileType("SineBall"), npc.damage / 2, 3, npc.target, 180);
					int p1 =Projectile.NewProjectile(npc.Center - new Vector2(0, 60), direction, mod.ProjectileType("SineBall"), npc.damage / 2, 3, npc.target, 0, proj + 1);
                    Main.projectile[proj].hostile = true;
                    Main.projectile[p1].hostile = true;
                    Main.projectile[proj].friendly = false;
                    Main.projectile[p1].friendly = false;
                }
				if (attackCounter > 260) 
				{
					npc.ai[0] = 1;
                     
                    timeBetweenAttacks = 30;
					attackCounter = 0;
				}
			}
		}
		void SmashAttack()
		{
			Player player = Main.player[npc.target];
			npc.rotation = 3.14f;
			if (attackCounter < 30 && attackCounter > 10) 
			{
				npc.position.X = player.position.X;
				npc.position.Y = player.position.Y - 300; 
			}
			attackCounter++;
			if (attackCounter < 30) 
			{
				UpdateFrame(0.2f, 4, 9);
			}
			else 
			{
				UpdateFrame(0.4f, 4, 9);
				if (attackCounter == 55) {
                    Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/BossSFX/MoonWizard_Attack"));
                    npc.velocity.Y = 13;
                    npc.velocity.Y += 1.6f;
					if (phaseTwo) {
						npc.velocity.Y = 20;
						attackCounter++;
					}
                    
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 81);
				}
			}
			if ((Main.tile[(int)(npc.Center.X / 16), (int)(npc.Center.Y / 16)].collisionType == 1 || attackCounter > 75) && Main.netMode != NetmodeID.MultiplayerClient)
			{
				for (int i = 0; i < Main.rand.Next(9, 15); i++) {
					Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.NextFloat(-10, 10), Main.rand.NextFloat(3.2f)), mod.ProjectileType("MoonBubble"), NPCUtils.ToActualDamage(40, 1.5f), 3);
				}
				Teleport();
				npc.ai[0] = 0;
                
				timeBetweenAttacks = 20;
				attackCounter = 0;
			}
		}
		void ZapJellies()
		{
			UpdateFrame(0.15f, 10, 13);
			if (attackCounter > 20) {
				for (int j = 0; j < Main.npc.Length; j++) {
					if (Main.npc[j].type == mod.NPCType("MoonJellyMedium") && Main.npc[j].active && attackCounter % 5 == 0) {
						NPC other = Main.npc[j];
						Vector2 direction9 = other.Center - npc.Center;
						int distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y * direction9.Y));
						direction9.Normalize();
						if (distance < 1000) 
						{
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                if (attackCounter < 60)
                                {
                                    int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)direction9.X * 15, (float)direction9.Y * 15, mod.ProjectileType("MoonPredictorTrail"), 0, 0);
                                    Main.projectile[proj].timeLeft = (int)(distance / 15);
                                }
                                else
                                {
                                    int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)direction9.X * 30, (float)direction9.Y * 30, mod.ProjectileType("MoonLightning"), 30, 0);
                                    Main.projectile[proj].timeLeft = (int)(distance / 30);
                                    DustHelper.DrawElectricity(npc.Center, other.Center, 226, 0.3f, 30, default, 0.15f);
                                    other.ai[2] = 1;
                                }
                            }
						}
					}
				}
			}
			attackCounter++;
			if (attackCounter > 120) {
				npc.ai[0] = 0;
                
                timeBetweenAttacks = 35;
				attackCounter = 0;
			}
		}

		void TeleportDash()
		{
			Player player = Main.player[npc.target];
			UpdateFrame(0.15f, 4, 9);
			float speed = 16f;
			if (phaseTwo) {
				speed = 21;
			}
			if (attackCounter == 20) {
				dashDirection = player.Center - npc.Center;
				dashDistance = dashDirection.Length();
				dashDirection.Normalize();
				dashDirection *= speed;
				npc.velocity = dashDirection;
                
				if (Main.netMode != NetmodeID.MultiplayerClient)
					node = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("LightningNode"), npc.damage / 5, 0, Main.myPlayer, npc.whoAmI + 1);
				npc.rotation = npc.velocity.ToRotation() + 1.57f;
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 81);
			}
			if (attackCounter < 20) 
			{
				dashDirection = player.Center - npc.Center;
				npc.rotation = dashDirection.ToRotation() + 1.57f;
			}
			attackCounter++;
			if (attackCounter > Math.Abs(dashDistance / speed) + 40) {
				Main.projectile[node].Kill();
				Teleport();
				npc.ai[0] = 0;
                
                timeBetweenAttacks = 35;
				if (phaseTwo) {
					timeBetweenAttacks = 34;
				}
				attackCounter = 0;
			}
		}
		void CreateNodes()
		{
			UpdateFrame(0.15f, 10, 13);
			Player player = Main.player[npc.target];
			if (attackCounter == 30 && Main.netMode != NetmodeID.MultiplayerClient) 
			{
				for (int i = 0; i < 4; i++) 
				{
					npc.ai[3] = Main.rand.Next(360);
					double anglex = Math.Sin(npc.ai[3] * (Math.PI / 180));
					double angley = Math.Abs(Math.Cos(npc.ai[3] * (Math.PI / 180)));
					Vector2 angle = new Vector2((float)anglex, (float)angley);
					Projectile.NewProjectile(player.position - (angle * Main.rand.Next(100, 400)), Vector2.Zero, mod.ProjectileType("MoonBlocker"), npc.damage / 2, 0, player.whoAmI);
					npc.netUpdate = true;
				}
			}
			attackCounter++;
			if (attackCounter > 55) 
			{
				npc.ai[0] = 0;
                
                timeBetweenAttacks = 8;
				attackCounter = 0;
			}
		}
		void Dash()
		{
			Player player = Main.player[npc.target];
			UpdateFrame(0.15f, 4, 9);
			float speed = 13f;
			if (attackCounter == 0) {
				Main.PlaySound(SoundID.DD2_MonkStaffSwing, npc.position);
				int distance = Main.rand.Next(300, 500);
				npc.ai[3] = Main.rand.Next(360);
				double anglex = Math.Sin(npc.ai[3] * (Math.PI / 180));
				double angley = Math.Abs(Math.Cos(npc.ai[3] * (Math.PI / 180)));
				Vector2 angle = new Vector2((float)anglex,(float)angley);
				dashDirection = (player.Center - (angle * distance)) - npc.Center;
				dashDistance = dashDirection.Length();
				dashDirection.Normalize();
				dashDirection *= speed;
				npc.velocity = dashDirection;
                
				if (Main.netMode != NetmodeID.MultiplayerClient)
					node = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("LightningNode"), npc.damage / 5, 0, Main.myPlayer, npc.whoAmI + 1);

				npc.netUpdate = true;
            }
			attackCounter++;
			npc.rotation = npc.velocity.ToRotation() + 1.57f;
			if (attackCounter > Math.Abs(dashDistance / speed)) {
				Main.projectile[node].Kill();
                if (phaseTwo) {
                    npc.velocity = Vector2.Zero;
                    attackCounter = 0;
                    npc.ai[0] = 3;
                    
                    timeBetweenAttacks = 25;
                }
				else
                {
                    npc.ai[0] = 0;
                    
                    timeBetweenAttacks = 30;
                    attackCounter = 0;
                }
			}
		}
		void KickAttack()
		{
			if (attackCounter == 0) 
			{
				npc.spriteDirection = npc.direction;
			}
			attackCounter++;
			if (attackCounter < 70) {
				UpdateFrame(0.2f, 14, 28);
			}
			else 
			{
                if (npc.life > 250)
                {
                    UpdateFrame(0.15f, 0, 3);
                }
                else
                {
                    UpdateFrame(0.15f, 54, 61);
                }
			}
			if (attackCounter > 90) {
				npc.ai[0] = 1;
                
                timeBetweenAttacks = 0;
				attackCounter = 0;
			}
			if (attackCounter == 60) {

                for (int k = 0; k < 18; k++)
                {

                    Dust d = Dust.NewDustPerfect(new Vector2(npc.Center.X + 75 * npc.spriteDirection, npc.Center.Y - 30), 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.75f);
                    d.noGravity = true;
                }
                Main.PlaySound(SoundID.NPCKilled, npc.position, 28);

                int Ball = Projectile.NewProjectile(npc.Center.X + 75 * npc.spriteDirection, npc.Center.Y - 30, npc.spriteDirection * 3.5f, -2f, mod.ProjectileType("WizardBall"), NPCUtils.ToActualDamage(50, 1.5f), 3f, 0);
                Main.projectile[Ball].ai[0] = npc.whoAmI;
                Main.projectile[Ball].ai[1] = Main.rand.Next(7, 9);
				Main.projectile[Ball].netUpdate = true;
			}
		}
		Vector2 TeleportPos = Vector2.Zero;
		void TeleportMove()
		{
			Player player = Main.player[npc.target];
			if (numMoves == -1) {
				numMoves = 2;
			}
			if (attackCounter == 0) 
			{
				int distance = Main.rand.Next(100, 200);
				double anglex = Math.Sin(npc.ai[3] * (Math.PI / 180));
				double angley = Math.Cos(npc.ai[3] * (Math.PI / 180));
				TeleportPos = player.Center + new Vector2((float)anglex * distance, (float)angley * distance); // REVISE
			}
            attackCounter++;
            if (attackCounter > 30) {
                UpdateFrame(0.155f, 29, 37);
            }
			else if (attackCounter < 30 && numMoves != 2)
			{
                UpdateFrame(0.2f, 38, 49);
            }
			else if (attackCounter < 30 && numMoves == 2)
            {
                if (npc.life > 250)
                {
                    UpdateFrame(0.15f, 1, 3);
                }
                else
                {
                    UpdateFrame(0.15f, 54, 61);
                }
            }
			Dust.NewDustPerfect(TeleportPos, 226, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-6, 0)));

			if (attackCounter > 82) {
				attackCounter = 0;
                int a = Gore.NewGore(new Vector2(npc.Center.X, npc.Center.Y - 50), new Vector2(0, 3), mod.GetGoreSlot("Gores/WizardHat_Gore"));
                Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/BossSFX/MoonWizard_Taunt"));
                npc.position = TeleportPos - new Vector2(npc.width / 2, npc.height / 2);
				Main.PlaySound(SoundID.Item, npc.position, 94);
				numMoves--;
				for (int i = 0; i < 20; i++) {
                    int p = Projectile.NewProjectile(npc.Center, Vector2.One.RotatedBy(0.314 * i) * 3.95f, mod.ProjectileType("MoonBubble"), NPCUtils.ToActualDamage(40, 1.5f), 0, npc.target, 1);
                    Main.projectile[p].timeLeft = 180;
                }
				if (numMoves == 0) {
					npc.ai[0] = 0;
                    
                    timeBetweenAttacks = 90;
					attackCounter = 0;
					numMoves = -1;
				}
			}
		}

		void Teleport()
		{
			Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 66);
			Player player = Main.player[npc.target];
			int distance = Main.rand.Next(300, 500);
			bool teleported = false;
			while (!teleported) {
				npc.ai[3] = Main.rand.Next(360);
				double anglex = Math.Sin(npc.ai[3] * (Math.PI / 180));
				double angley = Math.Cos(npc.ai[3] * (Math.PI / 180));
				npc.position.X = player.Center.X + (int)(distance * anglex);
				npc.position.Y = player.Center.Y + (int)(distance * angley);
				if (Main.tile[(int)(npc.position.X / 16), (int)(npc.position.Y / 16)].active() || Main.tile[(int)(npc.Center.X / 16), (int)(npc.Center.Y / 16)].active()) {
					npc.alpha = 255;
				}
				else {
					teleported = true;
					npc.alpha = 0;
				}

				npc.netUpdate = true;
			}
			for (int i = 0; i < 50; i++) {
				Dust.NewDustPerfect(npc.Center, 226, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-6, 0)));
			}
		}
        #endregion
        public override bool PreNPCLoot()
        {
            MyWorld.downedMoonWizard = true;
			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.WorldData);
			npc.PlayDeathSound("MJWDeathSound");

            return true;
        }

		public void RegisterToChecklist(out BossChecklistDataHandler.EntryType entryType, out float progression,
			out string name, out Func<bool> downedCondition, ref BossChecklistDataHandler.BCIDData identificationData,
			ref string spawnInfo, ref string despawnMessage, ref string texture, ref string headTextureOverride,
			ref Func<bool> isAvailable)
		{
			entryType = BossChecklistDataHandler.EntryType.Boss;
			progression = 2.5f;
			name = "Moon Jelly Wizard";
			downedCondition = () => MyWorld.downedMoonWizard;
			identificationData = new BossChecklistDataHandler.BCIDData(
				new List<int> {
					ModContent.NPCType<MoonWizard>()
				},
				new List<int> {
					ModContent.ItemType<DreamlightJellyItem>()
				},
				new List<int> {
					ModContent.ItemType<MJWTrophy>(),
					ModContent.ItemType<MJWMask>(),
					ModContent.ItemType<MJWBox>()
				},
				new List<int> {
					ModContent.ItemType<Cornucopion>(),
					ModContent.ItemType<MoonjellySummonStaff>(),
					ModContent.ItemType<Moonburst>(),
					ModContent.ItemType<JellynautBubble>(),
					ModContent.ItemType<Moonshot>(),
					ModContent.ItemType<MoonJelly>()
				});
			spawnInfo =
				$"Use a [i:{ModContent.ItemType<DreamlightJellyItem>()}] anywhere at nighttime. A [i:{ModContent.ItemType<DreamlightJellyItem>()}] can be caught with a bug net during the Jelly Deluge, and is non-consumable";
			texture = "SpiritMod/Textures/BossChecklist/MoonWizardTexture";
			headTextureOverride = "SpiritMod/NPCs/Boss/MoonWizard/MoonWizard_Head_Boss";
		}
	}
}
