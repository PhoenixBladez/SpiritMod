
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Projectiles.Hostile;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Weapon.Flail;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Boss;
using SpiritMod.Tide;
using System.IO;

namespace SpiritMod.NPCs.Boss.MoonWizard
{
	[AutoloadBossHead]
	public class MoonWizard : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Jelly Wizard");
			Main.npcFrameCount[npc.type] = 10;
			NPCID.Sets.TrailCacheLength[npc.type] = 10;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 1400;
			npc.defense = 10;
			npc.value = 400f;
			npc.aiStyle = -1;
			npc.knockBackResist = 0f;
			npc.width = 34;
			npc.height = 70;
			npc.damage = 30;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.boss = true;
			music = MusicID.Boss4;
		}

		float trueFrame = 0;
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.velocity != Vector2.Zero) {
				drawAfterimage(spriteBatch, drawColor);
			}
			return false;
		}
		public void drawAfterimage(SpriteBatch spriteBatch, Color drawColor) //we have access to this method already
		{
			Texture2D texture = Main.npcTexture[npc.type];
			Microsoft.Xna.Framework.Color AfterimageColor = new Microsoft.Xna.Framework.Color((int)sbyte.MaxValue, (int)sbyte.MaxValue, (int)sbyte.MaxValue, 0).MultiplyRGBA(new Color(75, 231, 255, 150)) * 2f;
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (npc.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			for (int index = 1; index < 10; ++index) {
				Main.spriteBatch.Draw(texture, new Vector2(npc.Center.X, npc.Center.Y - 18) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)index * 0.5f, npc.frame, AfterimageColor, npc.rotation, npc.frame.Size() / 2, MathHelper.Lerp(npc.scale * 1.5f, 0.8f, (float)index / 10), spriteEffects, 0.0f);
			}
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var effects = npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], new Vector2(npc.Center.X, npc.Center.Y - 18) - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			drawSpecialGlow(spriteBatch, lightColor);
		}
		public void drawSpecialGlow(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (npc.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			Texture2D texture = Main.npcTexture[npc.type];
			float num99 = (float)(Math.Cos((double)Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 4.0 + 0.5);
			Vector2 bb = new Vector2(npc.Center.X, npc.Center.Y - 18) - Main.screenPosition - new Vector2((float)texture.Width / 3, (float)(texture.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f + npc.frame.Size() / 2 * npc.scale + new Vector2(0.0f, npc.gfxOffY);
			Microsoft.Xna.Framework.Color color2 = new Microsoft.Xna.Framework.Color((int)sbyte.MaxValue - npc.alpha, (int)sbyte.MaxValue - npc.alpha, (int)sbyte.MaxValue - npc.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.White);
			Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/MoonWizard_Glow"), bb, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color2, npc.rotation, npc.frame.Size() / 2, npc.scale, spriteEffects, 0.0f);
			for (int index2 = 0; index2 < 4; ++index2) {
				Microsoft.Xna.Framework.Color GlowColor = npc.GetAlpha(color2) * (1f - num99);
				Vector2 GlowPosition = new Vector2(npc.Center.X, npc.Center.Y - 18) + ((float)((double)index2 / (double)4 * 6.28318548202515) + npc.rotation).ToRotationVector2() * (float)(4.0 * (double)num99 + 2.0) - Main.screenPosition - new Vector2((float)texture.Width / 3, (float)(texture.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f + npc.frame.Size() / 2 * npc.scale + new Vector2(0.0f, npc.gfxOffY);
				Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boss/MoonWizard/MoonWizard_Glow"), GlowPosition, new Microsoft.Xna.Framework.Rectangle?(npc.frame), GlowColor, npc.rotation, npc.frame.Size() / 2, npc.scale, spriteEffects, 0.0f);
			}
		}
		int attackCounter;
		int timeBetweenAttacks = 150;
		//0-3: idle
		//4-9 propelling
		//10-13 skirt up
		//14-21: turning
		//22-28: kick
		public override void AI()
		{
			npc.TargetClosest();
			if (npc.ai[0] == 0) {
				attackCounter++;
				npc.velocity = Vector2.Zero;
				npc.rotation = 0;
				UpdateFrame(0.15f, 0, 3);
				if (attackCounter > timeBetweenAttacks) {
					attackCounter = 0;
					npc.ai[0] = Main.rand.Next(9) + 1;

					int numJellies = 0;
					if (npc.ai[0] == 3 || npc.ai[0] == 8) { //attack regulations - hardcoded!
						for (int j = 0; j < Main.npc.Length; j++) 
						{
							if (Main.npc[j].type == mod.NPCType("MoonJellyMedium") && Main.npc[j].active)
							{
								numJellies++;
							}
						}
						if (numJellies > 3) {
							npc.ai[0] = 8;
						}
						if (numJellies == 0) {
							npc.ai[0] = 3;
						}
					}
					Player player = Main.player[npc.target];
					Vector2 dist = player.Center - npc.Center;
					if (dist.Length() > 600) 
					{
						switch (Main.rand.Next(2)) {
							case 0:
								npc.ai[0] = 5;
								break;
							case 1:
								npc.ai[0] = 2;
								break;
						}

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
						TeleportMove();
						break;
					case 4:
						KickAttack();
						break;
					case 5:
						SmashAttack();
						break;
					case 6:
						SineAttack();
						break;
					case 7:
						CreateNodes();
						break;
					case 8:
						ZapJellies();
						break;
					case 9:
						SpazAttack();
						break;

				}

			}
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Width = 120;
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
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.HealingPotion;
		}
		public override void NPCLoot()
		{
			
		}
        public override bool PreNPCLoot()
        {
			return true;
        }

        public override void HitEffect(int hitDirection, double damage)
		{
			
		}

		Vector2 dashDirection = Vector2.Zero;
		float dashDistance = 0f;
		int numMoves = -1;
		int node = 0;
		#region attacks

		int dashCounter = 35;
		int actualDashCounter = 0;
		Vector2 dash1 = Vector2.Zero;
		Vector2 dash2 = Vector2.Zero;
		Vector2 dash3 = Vector2.Zero;
		Vector2 dash4 = Vector2.Zero;
		Vector2 dash5 = Vector2.Zero;
		Vector2 dashFrom = Vector2.Zero;
		void SpazAttack() //if you are debugging, please have mercy on me. I really didn't know any other way to code this. I know it's extreme spagetti code, and I'm sorry.
		{
			int speed = 25;
			if (attackCounter == 0) {
				dash1 = Vector2.One * 999;
				dash2 = Vector2.One * 999;
				dash3 = Vector2.One * 999;
				dash4 = Vector2.One * 999;
				dash5 = Vector2.One * 999;
				actualDashCounter = 0;
				dashCounter = 35;
				dashFrom = npc.Center;
			}
			Player player = Main.player[npc.target];
			attackCounter++;
			if (attackCounter == 30 && dash1 == Vector2.One * 999) 
			{
				dashCounter = attackCounter;
				SpazPredictor(ref dash1, dashFrom);
			}
			if (attackCounter == dashCounter + (int)dash1.Length() && dash2 == Vector2.One * 999) {
				dashCounter = attackCounter;
				SpazPredictor(ref dash2, dashFrom + (dash1 * speed));
			}
			if (attackCounter == dashCounter + (int)dash2.Length() && dash3 == Vector2.One * 999) {
				dashCounter = attackCounter;
				SpazPredictor(ref dash3, dashFrom + (dash2 * speed) + (dash1 * speed));
			}
			if (attackCounter == dashCounter + (int)dash3.Length() && dash4 == Vector2.One * 999) {
				dashCounter = attackCounter;
				SpazPredictor(ref dash4, dashFrom + (dash3 * speed) + (dash1 * speed) + (dash2 * speed));
			}
			if (attackCounter == dashCounter + (int)dash4.Length() && dash5 == Vector2.One * 999) {
				dashCounter = attackCounter;
				SpazPredictor(ref dash5, dashFrom + (dash4 * speed) + (dash1 * speed) + (dash2 * speed) + (dash3 * speed));
			}


			int proj = 0;
			Vector2 dashNormal = Vector2.Zero;
			if (dash1 != Vector2.One * 999 && attackCounter % 5 == 0 && actualDashCounter <= 1) {
				dashNormal = dash1;
				dashNormal.Normalize();
				proj = Projectile.NewProjectile(dashFrom, dashNormal * speed, mod.ProjectileType("MoonPredictorTrail"), 0, 0);
				Main.projectile[proj].timeLeft = (int)(dash1.Length());
			}
			if (dash2 != Vector2.One * 999 && attackCounter % 5 == 0 && actualDashCounter <= 2) {
				dashNormal = dash2;
				dashNormal.Normalize();
				proj = Projectile.NewProjectile(dashFrom + (dash1 * speed), dashNormal * speed, mod.ProjectileType("MoonPredictorTrail"), 0, 0);
				Main.projectile[proj].timeLeft = (int)(dash2.Length());
			}
			if (dash3 != Vector2.One * 999 && attackCounter % 5 == 0 && actualDashCounter <= 3) {
				dashNormal = dash3;
				dashNormal.Normalize();
				proj = Projectile.NewProjectile(dashFrom + (dash1 * speed) + (dash2 * speed), dashNormal * speed, mod.ProjectileType("MoonPredictorTrail"), 0, 0);
				Main.projectile[proj].timeLeft = (int)(dash3.Length());
			}
			if (dash4 != Vector2.One * 999 && attackCounter % 5 == 0 && actualDashCounter <= 4) {
				dashNormal = dash4;
				dashNormal.Normalize();
				proj = Projectile.NewProjectile(dashFrom + (dash1 * speed) + (dash2 * speed) + (dash3 * speed), dashNormal * speed, mod.ProjectileType("MoonPredictorTrail"), 0, 0);
				Main.projectile[proj].timeLeft = (int)(dash4.Length());
			}
			if (dash5 != Vector2.One * 999 && attackCounter % 5 == 0 && actualDashCounter <= 5) {
				dashNormal = dash5;
				dashNormal.Normalize();
				proj = Projectile.NewProjectile(dashFrom + (dash1 * speed) + (dash2 * speed) + (dash3 * speed) + (dash4 * speed), dashNormal * speed, mod.ProjectileType("MoonPredictorTrail"), 0, 0);
				Main.projectile[proj].timeLeft = (int)(dash5.Length());
			}

			if (attackCounter == dashCounter + (int)dash5.Length() + 15 && actualDashCounter == 0) {
				dashCounter = attackCounter;
				dashNormal = dash1;
				dashNormal.Normalize();
				npc.velocity = dashNormal * speed;
				actualDashCounter = 1;
			}
			if (attackCounter == dashCounter + (int)dash1.Length() && actualDashCounter == 1) {
				dashCounter = attackCounter;
				dashNormal = dash2;
				dashNormal.Normalize();
				npc.velocity = dashNormal * speed;
				actualDashCounter = 2;
			}
			if (attackCounter == dashCounter + (int)dash2.Length() && actualDashCounter == 2) {
				dashCounter = attackCounter;
				dashNormal = dash3;
				dashNormal.Normalize();
				npc.velocity = dashNormal * speed;
				actualDashCounter = 3;
			}
			if (attackCounter == dashCounter + (int)dash3.Length() && actualDashCounter == 3) {
				dashCounter = attackCounter;
				dashNormal = dash4;
				dashNormal.Normalize();
				npc.velocity = dashNormal * speed;
				actualDashCounter = 4;
			}
			if (attackCounter == dashCounter + (int)dash4.Length() && actualDashCounter == 4) {
				dashCounter = attackCounter;
				dashNormal = dash5;
				dashNormal.Normalize();
				npc.velocity = dashNormal * speed;
				actualDashCounter = 5;
			}
			if (attackCounter == dashCounter + (int)dash5.Length() && actualDashCounter == 5) {
				npc.ai[0] = 0;
				timeBetweenAttacks =15;
				attackCounter = 0;
			}
			if (actualDashCounter != 0) 
			{
				npc.rotation = npc.velocity.ToRotation() + 1.57f;
				UpdateFrame(0.15f, 4, 9);
			}
			else 
			{
				UpdateFrame(0.15f, 1, 3);
			}
		}
		void SpazPredictor(ref Vector2 dash, Vector2 Position)
		{
			int speed = 25;
			Player player = Main.player[npc.target];
			int distance = Main.rand.Next(50,250);
			npc.ai[3] = Main.rand.Next(360);
			double anglex = Math.Sin(npc.ai[3] * (Math.PI / 180));
			double angley = Math.Cos(npc.ai[3] * (Math.PI / 180));
			Vector2 angle = new Vector2((float)anglex, (float)angley);
			angle.Normalize();
			angle *= distance;
			dash = (player.Center + angle) - Position;
			Vector2 dashNormal = dash;
			dashNormal.Normalize();
			int proj = Projectile.NewProjectile(Position, dashNormal * speed, mod.ProjectileType("MoonPredictorTrail"), 0, 0);
			Main.projectile[proj].timeLeft = (int)(dash.Length() / speed);
			dash /= speed;
		}
		void SineAttack()
		{
			Player player = Main.player[npc.target];
			UpdateFrame(0.15f, 10, 13);
			attackCounter++;
			if (attackCounter >= 80) 
			{
				if (attackCounter % 80 == 3) 
				{
					Vector2 direction = player.Center - (npc.Center - new Vector2(0, 60));
					direction.Normalize();
					direction *= 5;
					int proj = Projectile.NewProjectile(npc.Center - new Vector2(0, 60), direction, mod.ProjectileType("SineBall"), npc.damage / 2, 3, npc.target, 180);
					Projectile.NewProjectile(npc.Center - new Vector2(0, 60), direction, mod.ProjectileType("SineBall"), npc.damage / 2, 3, npc.target, 0, proj + 1);
				}
				if (attackCounter > 270) 
				{
					npc.ai[0] = 0;
					timeBetweenAttacks = 0;
					attackCounter = 0;
				}
			}
		}
		void SmashAttack()
		{
			Player player = Main.player[npc.target];
			npc.rotation = 3.14f;
			if (attackCounter < 30) 
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
				if (attackCounter > 45) {
					npc.velocity.Y = 25;
				}
			}
			if (Main.tile[(int)(npc.Center.X / 16), (int)(npc.Center.Y / 16)].collisionType == 1 || attackCounter > 60) 
			{
				for (int i = 0; i < Main.rand.Next(9, 15); i++) {
					Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.NextFloat(-10, 10), Main.rand.NextFloat(3)), mod.ProjectileType("MoonBubble"), npc.damage / 2, 3);
				}
				Teleport();
				npc.ai[0] = 0;
				timeBetweenAttacks = 35;
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
						if (distance < 800) 
						{
							if (attackCounter < 60) {
								int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)direction9.X * 15, (float)direction9.Y * 15, mod.ProjectileType("MoonPredictorTrail"), 0, 0);
								Main.projectile[proj].timeLeft = (int)(distance / 15);
							}
							else {
								int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)direction9.X * 30, (float)direction9.Y * 30, mod.ProjectileType("MoonLightning"), 30, 0);
								Main.projectile[proj].timeLeft = (int)(distance / 30);
								DustHelper.DrawElectricity(npc.Center, other.Center, 226, 0.3f, 30, default, 0.15f);
								other.ai[2] = 1;
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
			float speed = 15f;
			if (attackCounter == 20) {
				dashDirection = player.Center - npc.Center;
				dashDistance = dashDirection.Length();
				dashDirection.Normalize();
				dashDirection *= speed;
				npc.velocity = dashDirection;
				node = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("LightningNode"), npc.damage / 3, 0, Main.myPlayer, npc.whoAmI + 1);
				npc.rotation = npc.velocity.ToRotation() + 1.57f;
			}
			if (attackCounter < 20) 
			{
				dashDirection = player.Center - npc.Center;
				npc.rotation = dashDirection.ToRotation() + 1.57f;
			}
			attackCounter++;
			if (attackCounter > Math.Abs(dashDistance / speed) + 40) {
				Main.projectile[node].active = false;
				Teleport();
				npc.ai[0] = 0;
				timeBetweenAttacks = 35;
				attackCounter = 0;
			}
		}
		void CreateNodes()
		{
			UpdateFrame(0.15f, 10, 13);
			Player player = Main.player[npc.target];
			if (attackCounter == 30) 
			{
				for (int i = 0; i < 4; i++) 
				{
					npc.ai[3] = Main.rand.Next(360);
					double anglex = Math.Sin(npc.ai[3] * (Math.PI / 180));
					double angley = Math.Abs(Math.Cos(npc.ai[3] * (Math.PI / 180)));
					Vector2 angle = new Vector2((float)anglex, (float)angley);
					Projectile.NewProjectile(player.position - (angle * Main.rand.Next(100, 400)), Vector2.Zero, mod.ProjectileType("MoonBlocker"), npc.damage / 2, 0, player.whoAmI);
				}
			}
			attackCounter++;
			if (attackCounter > 100) 
			{
				npc.ai[0] = 0;
				timeBetweenAttacks = 25;
				attackCounter = 0;
			}
		}
		void Dash()
		{
			Player player = Main.player[npc.target];
			UpdateFrame(0.15f, 4, 9);
			float speed = 10f;
			if (attackCounter == 0) {
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
				node = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("LightningNode"), npc.damage / 3, 0, Main.myPlayer, npc.whoAmI + 1);
			}
			attackCounter++;
			npc.rotation = npc.velocity.ToRotation() + 1.57f;
			if (attackCounter > Math.Abs(dashDistance / speed)) {
				Main.projectile[node].active = false;
				npc.ai[0] = 0;
				timeBetweenAttacks = 10;
				attackCounter = 0;
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
				UpdateFrame(0.15f, 0, 3);
			}
			if (attackCounter > 200) {
				npc.ai[0] = 0;
				timeBetweenAttacks = 60;
				attackCounter = 0;
			}
			if (attackCounter == 20) {
				int Ball = Projectile.NewProjectile(npc.Center.X + 75 * npc.spriteDirection, npc.Center.Y - 30, 0f, 0f, mod.ProjectileType("WizardBall"), 20, 3f, 0);
				Main.projectile[Ball].ai[0] = npc.whoAmI;
				Main.projectile[Ball].ai[1] = Main.rand.Next(7,9);
			}
		}
		void TeleportMove()
		{
			UpdateFrame(0.15f, 10, 13);
			if (numMoves == -1) {
				numMoves = 3;
			}
			if (attackCounter == 0) 
			{
				numMoves--;
				NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("MoonJellyMedium"));
				Teleport();
			}
			attackCounter++;
			if (attackCounter > 40) {
				attackCounter = 0;
				if (numMoves == 0) {
					npc.ai[0] = 0;
					timeBetweenAttacks = 35;
					attackCounter = 0;
					numMoves = -1;
				}
			}
		}

		void Teleport()
		{
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
			}
		}
		#endregion
	}
}
