using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using SpiritMod.Buffs;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.BossBags;
using SpiritMod.NPCs.Boss.MoonWizardTwo.Projectiles;
using SpiritMod.Prim;
using SpiritMod.Utilities;

namespace SpiritMod.NPCs.Boss.MoonWizardTwo
{
	[AutoloadBossHead]
	public class MoonWizardTwo : ModNPC
	{
		public int cooldownCounter = 50;

		private bool phaseTwo = false;
		private bool attacking => cooldownCounter <= 0;

		private float distanceToPlayer => (npc.Center - Main.player[npc.target].Center).Length();

		private Vector2 projectileStart => npc.Center - new Vector2(0, 60);

		private float trueFrame = 0;
		private int attackCounter = 0;
		private int preAttackCounter = 0;

		private Vector2 dashDirection = Vector2.Zero;
		private Vector2 teleportLocation = Vector2.Zero;
		private float dashDistance = 0f;
		private Projectile currentProjectile;

		const int NUMBEROFATTACKS = 10;
		private enum CurrentAttack
		{
			DashToPlayer = 0,
			CreateGems = 1,
			CloneAttack = 2,
			DashToPlayerAgain = 3,
			JellyfishRapidFire = 4,
			SmallBubbles = 5,
			BubbleKick = 6,
			InwardPull = 7,
			SineBalls = 8,
			SkyStrikes = 9,
		}

		const int NUMBEROFATTACKSP2 = 1;
		private enum CurrentAttackP2
		{
			
		}
		private CurrentAttack currentAttack
		{
			get
			{
				return (CurrentAttack)(int)npc.ai[1];
			}
			set
			{
				npc.ai[1] = (int)value;
			}
		}

		private CurrentAttackP2 currentAttackP2
		{
			get
			{
				return (CurrentAttackP2)(int)npc.ai[1];
			}
			set
			{
				npc.ai[1] = (int)value;
			}
		}


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mystic Moon Jelly Wizard");
			Main.npcFrameCount[npc.type] = 21;
			NPCID.Sets.TrailCacheLength[npc.type] = 10;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.friendly = false;
			npc.lifeMax = 12000;
			npc.defense = 20;
			npc.value = 40000;
			npc.aiStyle = -1;
            bossBag = ModContent.ItemType<MJWBag>();
            npc.knockBackResist = 0f;
			npc.width = 17;
			npc.height = 35;
			npc.damage = 0;
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

		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color color, float opacity, float startScale, float endScale) => DrawAfterImage(spriteBatch, offset, trailLengthModifier, color, color, opacity, startScale, endScale);
		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color startColor, Color endColor, float opacity, float startScale, float endScale)
		{
			SpriteEffects spriteEffects = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			for (int i = 1; i < 10; i++)
			{
				Color color = Color.Lerp(startColor, endColor, i / 10f) * opacity;
				spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizardTwo/MoonWizardTwo_Afterimage"), new Vector2(npc.Center.X, npc.Center.Y) + offset - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)i * trailLengthModifier, npc.frame, color, npc.rotation, npc.frame.Size() * 0.5f, MathHelper.Lerp(startScale, endScale, i / 10f), spriteEffects, 0f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.hide)
				return false;
			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.2f;
			float num366 = num395 + 2.45f;
			if (npc.velocity != Vector2.Zero || phaseTwo)
			{
				DrawAfterImage(Main.spriteBatch, new Vector2(0f, -18f), 0.5f, Color.White * .7f, Color.White * .1f, 0.75f, num366, 1.65f);
			}
			return false;
		}


		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (npc.hide)
				return;
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
				Vector2 vector29 = new Vector2(npc.Center.X, npc.Center.Y - 18) + ((float)num103 / (float)num108 * 6.28318548f + npc.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)num103;
				Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizardTwo/MoonWizardTwo_Glow"), vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
			}
			Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizardTwo/MoonWizardTwo_Glow"), vector33, npc.frame, color29, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);

		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => npc.lifeMax = (int)(npc.lifeMax * 0.8f * bossLifeScale);
		//0-3: idle
		//4-9 propelling
		//10-13 skirt up
		//14-21: turning
		//22-28: kick
		//29-38: Teleport
		//39-51:Teleport part 2
		//54-61: Weird float

		public override void AI()
		{
			Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);
			npc.TargetClosest();
			if (!attacking)
			{
				if (!phaseTwo && npc.life < npc.lifeMax / 2.5f)
				{
					cooldownCounter = 90;
					phaseTwo = true;
					npc.ai[1] = 0;
				}
				if (phaseTwo)
					npc.ai[1] %= NUMBEROFATTACKSP2; //make sure the current attack is within the index
				else
					npc.ai[1] %= NUMBEROFATTACKS; //make sure the current attack is within the index
				attackCounter = 0;
				preAttackCounter = 0;
				cooldownCounter--;
				npc.velocity = Vector2.Zero;
				npc.rotation = 0;
				UpdateFrame(0.15f, 0, 3);
			}
			else
			{
				bool attackStart = false;
				if (!phaseTwo)
				{
					switch (currentAttack)
					{
						case CurrentAttack.InwardPull:
							attackStart = DoInwardPull();
							break;
						case CurrentAttack.SineBalls:
							attackStart = DoSineBalls();
							break;
						case CurrentAttack.SkyStrikes:
							attackStart = DoSkyStrikes();
							break;
						case CurrentAttack.DashToPlayer:
							attackStart = DoDashToPlayer(false);
							break;
						case CurrentAttack.CreateGems:
							attackStart = DoCreateGems();
							break;
						case CurrentAttack.CloneAttack:
							attackStart = DoCloneAttack();
							break;
						case CurrentAttack.DashToPlayerAgain:
							attackStart = DoDashToPlayer(true);
							break;
						case CurrentAttack.JellyfishRapidFire:
							attackStart = DoJellyfishRapidFire();
							break;
						case CurrentAttack.SmallBubbles:
							attackStart = DoSmallBubbles();
							break;
						case CurrentAttack.BubbleKick:
							attackStart = DoBubbleKick();
							break;
					}
				}
				else
				{
					switch(currentAttackP2)
					{ 
					
					}
				}
				if (attackStart)
					attackCounter++;
				else
					preAttackCounter++;
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

		void Teleport(bool dust = true, int distance = -1)
		{
			Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 66);
			Player player = Main.player[npc.target];
			if (distance == -1)
				distance = Main.rand.Next(300, 500);
			bool teleported = false;
			while (!teleported)
			{
				npc.ai[3] = Main.rand.Next(360);
				double anglex = Math.Sin(npc.ai[3] * (Math.PI / 180));
				double angley = Math.Cos(npc.ai[3] * (Math.PI / 180));
				npc.position.X = player.Center.X + (int)(distance * anglex);
				npc.position.Y = player.Center.Y + (int)(distance * angley);
				if (Main.tile[(int)(npc.position.X / 16), (int)(npc.position.Y / 16)].active() || Main.tile[(int)(npc.Center.X / 16), (int)(npc.Center.Y / 16)].active())
				{
					npc.alpha = 255;
				}
				else
				{
					teleported = true;
					npc.alpha = 0;
				}

				npc.netUpdate = true;
			}
			if (dust)
			{
				for (int i = 0; i < 25; i++)
				{
					Dust.NewDustPerfect(npc.Center, 205, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-6, 0)));
					Dust.NewDustPerfect(npc.Center, 226, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-6, 0)));
				}
			}
		}

		void Teleport(Vector2 position)
		{
			Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 66);
			Player player = Main.player[npc.target];
			npc.Center = position;
			npc.netUpdate = true;
			for (int i = 0; i < 25; i++)
			{
				Dust.NewDustPerfect(npc.Center, 205, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-6, 0)));
				Dust.NewDustPerfect(npc.Center, 226, new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-6, 0)));
			}
		}


		#region phase 1 attacks

		private bool DoInwardPull()
		{
			Player player = Main.player[npc.target];
			UpdateFrame(0.15f, 10, 13);
			if (attackCounter == 70)
			{
				Vector2 direction = player.Center - projectileStart;
				int proj = Projectile.NewProjectile(projectileStart, Vector2.Zero, ModContent.ProjectileType<MysticWall>(), 35, 3, npc.target, 1);
				Projectile projectileOne = Projectile.NewProjectileDirect(projectileStart, Vector2.Zero, ModContent.ProjectileType<MysticWall>(), 35, 3, npc.target, -1, proj + 1);
				Projectile projectileTwo = Main.projectile[proj];

				if (projectileOne.modProjectile is MysticWall modproj)
				{
					modproj.Parent = npc;
					modproj.InitialDistance = (int)direction.Length() + 1000;
				}
				if (projectileTwo.modProjectile is MysticWall modproj2)
				{
					modproj2.Parent = npc;
					modproj2.InitialDistance = (int)direction.Length() + 1000;
				}
				SpiritMod.primitives.CreateTrail(new MSineOrbPrimTrail(projectileOne, projectileTwo));
			}
			if (attackCounter > 30)
			{
				Vector2 direction = player.Center - projectileStart;
				direction.Normalize();
				Dust.NewDustPerfect(npc.Center + (direction.RotatedBy(Main.rand.NextFloat(-0.6f, 0.6f)) * 60), 205, direction.RotatedBy(-3.14f) * 4).noGravity = true;
			}
			return true;
		}
		private bool DoSineBalls()
		{
			Player player = Main.player[npc.target];
			UpdateFrame(0.15f, 10, 13);
			if (attackCounter % 60 == 30)
			{
				Vector2 direction = player.Center - projectileStart;
				direction.Normalize();
				int proj = Projectile.NewProjectile(projectileStart, direction, ModContent.ProjectileType<MysticSineBall>(), 35, 3, npc.target, 180);
				Projectile projectileOne = Projectile.NewProjectileDirect(projectileStart, direction, ModContent.ProjectileType<MysticSineBall>(), 35, 3, npc.target, 0, proj + 1);
				Projectile projectileTwo = Main.projectile[proj];

				SpiritMod.primitives.CreateTrail(new MSineOrbPrimTrail(projectileOne, projectileTwo));
			}
			if (attackCounter > 220)
			{
				npc.ai[1]++;
				cooldownCounter = 50;
			}
			if (preAttackCounter > 30)
				return true;
			return false;
		}
		private int SkyPos = 0;
		private int SkyPosY = 0;
		private int sweepDirection = 1;
		private bool DoSkyStrikes()
		{
			Player player = Main.player[npc.target];
			UpdateFrame(0.15f, 0, 3);
			if (attackCounter % 60 == 0)
			{
				SkyPosY = (int)(player.position.Y - 500);
				sweepDirection = 0 - sweepDirection;
				if (sweepDirection == 1)
					SkyPos = (int)(player.position.X - 1000);
				else
					SkyPos = (int)(player.position.X + 950);
			}
			if (attackCounter % 2 == 0 && Main.netMode != NetmodeID.MultiplayerClient && attackCounter % 60 < 40)
			{
				Vector2 strikePos = new Vector2(SkyPos, SkyPosY);
				Projectile proj = Projectile.NewProjectileDirect(strikePos, new Vector2(0, 0), ModContent.ProjectileType<MysticSkyMoonZapper>(), 30, 0, npc.target);
				SpiritMod.primitives.CreateTrail(new MoonLightningPrimTrail(proj));
				if (sweepDirection == 1) //left -> right
					SkyPos += 100;
				else //right -> left
					SkyPos -= 100;
			}
			if (attackCounter == 179)
			{
				npc.ai[1]++;
				cooldownCounter = 30;
			}

			return true;
		}

		private bool DoDashToPlayer(bool prediction)
		{
			Player player = Main.player[npc.target];
			UpdateFrame(0.15f, 4, 9);
			float speed = 24f;
			if (phaseTwo)
			{
				speed = 35;
			}
			if (attackCounter == 30)
			{
				npc.damage = 110;
				if (prediction)
					dashDirection = (player.Center + (player.velocity * 20)) - npc.Center;
				else
					dashDirection = player.Center - npc.Center;
				dashDistance = dashDirection.Length();
				dashDirection.Normalize();
				dashDirection *= speed;
				npc.velocity = dashDirection;

				npc.rotation = npc.velocity.ToRotation() + 1.57f;
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 81);
			}
			if (attackCounter < 30)
			{
				if (prediction)
					dashDirection = (player.Center + (player.velocity * 20)) - npc.Center;
				else
					dashDirection = player.Center - npc.Center;
				npc.rotation = dashDirection.ToRotation() + 1.57f;
			}
			if (attackCounter > Math.Abs(dashDistance / speed) + 50)
			{
				Teleport();
				npc.ai[1]++;
				cooldownCounter = 60;
				npc.damage = 0;
			}
			return true;
		}

		private bool DoCreateGems()
		{
			if (attackCounter == 45)
			{
				for (int i = 0; i < 3; i++)
					NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 30, ModContent.NPCType<AzureGemProj>(), 0, npc.whoAmI, i * 2.094f);
			}
			if (attackCounter > 155)
			{
				UpdateFrame(0.15f, 0, 3);
				if (attackCounter > 400)
				{
					npc.ai[1]++;
					cooldownCounter = 30;
				}
			}
			else
				UpdateFrame(0.15f, 54, 61);
			return true;
		}
		private bool DoCloneAttack()
		{
			Player player = Main.player[npc.target];
			int teleportValue = 46;
			int numClones = 6;
			int cloneCooldown = 40;
			if (preAttackCounter < teleportValue)
			{
				UpdateFrame(0.155f, 29, 37);
				return false;
			}
			if (preAttackCounter == teleportValue)
			{
				Gore.NewGore(new Vector2(npc.Center.X, npc.Center.Y - 50), new Vector2(0, 3), mod.GetGoreSlot("Gores/WizardHat_Gore"));
				npc.hide = true;
				npc.immortal = true;
				return false;
			}
			if (preAttackCounter > teleportValue)
			{
				if (attackCounter % cloneCooldown == 0)
				{
					Teleport(false, 1000);
					int clone = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<MysticMJWClone>());
					SpiritMod.primitives.CreateTrail(new MJWClonePrimTrail(Main.npc[clone], player, Color.Purple));
				}
				if (attackCounter > numClones * cloneCooldown && attackCounter % cloneCooldown == cloneCooldown - 1)
				{
					Teleport(false, 1000);
					SpiritMod.primitives.CreateTrail(new MJWClonePrimTrail(npc, player, Color.Cyan));
					npc.hide = false;
					npc.immortal = false;
					npc.ai[1]++;
					cooldownCounter = 2;
				}
			}
			return true;
		}

		private bool DoJellyfishRapidFire()
		{
			UpdateFrame(0.15f, 54, 61);
			if (attackCounter > 220)
			{
				npc.ai[1]++;
				cooldownCounter = 60;
			}
			if (attackCounter == 1)
			{
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/BossSFX/MoonWizard_Laugh2"));
			}
			if (attackCounter % 15 == 0 && attackCounter < 180 && Main.netMode != NetmodeID.MultiplayerClient)
			{
				Vector2 vector2_2 = Vector2.UnitY.RotatedByRandom(1.57079637050629f) * new Vector2(5f, 3f);
				bool expertMode = Main.expertMode;
				int p = Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-60, 60), npc.Center.Y + Main.rand.Next(-60, 60), vector2_2.X, vector2_2.Y, ModContent.ProjectileType<MysticJellyfishOrbiter>(), NPCUtils.ToActualDamage(70, 1.5f), 0.0f, Main.myPlayer, 0.0f, (float)npc.whoAmI);
				Main.projectile[p].scale = Main.rand.NextFloat(.6f, 1f);
			}
			return true;
		}
		private bool DoSmallBubbles()
		{
			int numberOfAttacks = 6;
			int attackLength = 55;
			Player player = Main.player[npc.target];
			UpdateFrame(0.3f, 14, 28);
			int localCounter = attackCounter % attackLength;
			npc.spriteDirection = npc.direction;
			if (localCounter == 2)
			{
				trueFrame = 14;
				float posX = player.Center.X + ((attackCounter % (attackLength * 2) > attackLength ? 1 : -1)  * 500);
				float posY = player.Center.Y - Main.rand.Next(200);
				Teleport(new Vector2(posX, posY));
			}
			if (localCounter == 30)
			{
				for (int k = 0; k < 18; k++)
					Dust.NewDustPerfect(new Vector2(npc.Center.X + 75 * npc.spriteDirection, npc.Center.Y - 30), 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.75f).noGravity = true;
				Main.PlaySound(4, npc.position, 28);

				Vector2 startPos = new Vector2(npc.Center.X + 75 * npc.spriteDirection, npc.Center.Y - 30);
				Vector2 arcVel = ArcVelocityHelper.GetArcVel(startPos, player.Center, 0.3f, 300, 500, 50, 100).RotatedBy(Main.rand.NextFloat(-0.2f,0.2f));
				int Ball = Projectile.NewProjectile(startPos, arcVel, ModContent.ProjectileType<MysticWizardBallSmall>(), NPCUtils.ToActualDamage(100, 1.5f), 3f, 0);
				Main.projectile[Ball].ai[0] = npc.whoAmI;
				Main.projectile[Ball].ai[1] = Main.rand.Next(3, 5);
				Main.projectile[Ball].netUpdate = true;
			}
			if (attackCounter > attackLength * numberOfAttacks)
			{
				npc.ai[1]++;
				cooldownCounter = 60;
				Teleport();
			}
			return true;
		}

		private bool DoBubbleKick()
		{
			Player player = Main.player[npc.target];
			if (attackCounter == 0)
			{
				UpdateFrame(0.15f, 4, 9);
				float speed = 28f;
				if (phaseTwo)
				{
					speed = 35;
				}
				if (preAttackCounter == 15)
				{
					dashDirection = player.Center - npc.Center;
					dashDirection += Math.Sign(dashDirection.X) * new Vector2(500, 0);
					dashDirection -= new Vector2(0, 200);
					dashDistance = dashDirection.Length();
					dashDirection.Normalize();
					dashDirection *= speed;
					npc.velocity = dashDirection;

					npc.rotation = npc.velocity.ToRotation() + 1.57f;
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 81);
				}
				if (preAttackCounter < 15)
				{
					dashDirection = player.Center - npc.Center;
					dashDirection += Math.Sign(dashDirection.X) * new Vector2(500, 0);
					dashDirection -= new Vector2(0, 200);
					npc.rotation = dashDirection.ToRotation() + 1.57f;
				}
				if (preAttackCounter > Math.Abs(dashDistance / speed) + 15)
				{
					return true;
				}
			}
			if (attackCounter > 0 && attackCounter < 20)
			{
				npc.rotation = 0;
				npc.velocity = Vector2.Zero;
				UpdateFrame(0.15f, 0, 3);
				npc.spriteDirection = npc.direction;
			}
			if (attackCounter >= 20) //actual kicking begins
			{
				if (attackCounter < 80)
					UpdateFrame(0.2f, 14, 28);
				else
				{
					npc.ai[1]++;
					cooldownCounter = 35;
				}
				if (attackCounter == 73)
				{
					for (int k = 0; k < 18; k++)
						Dust.NewDustPerfect(new Vector2(npc.Center.X + 75 * npc.spriteDirection, npc.Center.Y - 30), 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.75f).noGravity = true;
					Main.PlaySound(4, npc.position, 28);

					int Ball = Projectile.NewProjectile(npc.Center.X + 75 * npc.spriteDirection, npc.Center.Y - 30, npc.spriteDirection * 3.5f, -2f, ModContent.ProjectileType<MysticWizardBall>(), NPCUtils.ToActualDamage(100, 1.5f), 3f, 0);
					Main.projectile[Ball].ai[0] = npc.whoAmI;
					Main.projectile[Ball].ai[1] = Main.rand.Next(7, 9);
					Main.projectile[Ball].netUpdate = true;
				}
			}

			if (attackCounter > 0)
				return true;
			return false;
		}
		#endregion

		#region phase 2 attacks

		#endregion
	}
}
