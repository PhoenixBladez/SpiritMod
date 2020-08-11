/*
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
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 150;
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
		}

		float alphaCounter = 0;
		float trueFrame = 0;
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var effects = npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], new Vector2(npc.Center.X, npc.Center.Y - 18) - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
		}
		int attackCounter;
		int timeBetweenAttacks = 150;
		int numAttacks = 1;

		int attackIndex = 0;
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
					npc.ai[0] = Main.rand.Next(7) + 1;
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
			if (attackCounter < 20) 
			{
				npc.position.X = player.position.X;
				npc.position.Y = player.position.Y - 300; 
			}
			attackCounter++;
			if (attackCounter < 40) 
			{
				UpdateFrame(0.2f, 4, 9);
			}
			else 
			{
				UpdateFrame(0.4f, 4, 9);
				if (attackCounter > 60) {
					npc.velocity.Y = 25;
				}
			}
			if (Main.tile[(int)(npc.Center.X / 16), (int)(npc.Center.Y / 16)].collisionType == 1 || attackCounter > 120) 
			{
				for (int i = 0; i < Main.rand.Next(5, 7); i++) {
					Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(3)), mod.ProjectileType("MoonBubble"), npc.damage / 2, 3);
				}
				Teleport();
				npc.ai[0] = 0;
				timeBetweenAttacks = 50;
				attackCounter = 0;
			}
		}
		void TeleportDash()
		{
			Player player = Main.player[npc.target];
			UpdateFrame(0.15f, 4, 9);
			float speed = 10f;
			if (attackCounter == 0) {
				dashDirection = player.Center - npc.Center;
				dashDistance = dashDirection.Length();
				dashDirection.Normalize();
				dashDirection *= speed;
				npc.velocity = dashDirection;
				node = Projectile.NewProjectile(npc.position, Vector2.Zero, mod.ProjectileType("LightningNode"), npc.damage / 3, 0, Main.myPlayer, npc.whoAmI + 1);
			}
			attackCounter++;
			npc.rotation = npc.velocity.ToRotation() + 1.57f;
			if (attackCounter > Math.Abs(dashDistance / speed) + 15) {
				Main.projectile[node].active = false;
				Teleport();
				npc.ai[0] = 0;
				timeBetweenAttacks = 50;
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
			if (attackCounter > 200) 
			{
				npc.ai[0] = 0;
				timeBetweenAttacks = 50;
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
				timeBetweenAttacks = 50;
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
				timeBetweenAttacks = 120;
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
					timeBetweenAttacks = 50;
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
}*/
