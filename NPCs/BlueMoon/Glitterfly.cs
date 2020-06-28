using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlueMoon
{
	public class Glitterfly : ModNPC
	{
		int timer = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 150f;
		bool hat = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glitterfly");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Pixie];
		}

		public override void SetDefaults()
		{
			npc.width = 50;
			npc.height = 40;
			npc.damage = 45;
			npc.lifeMax = 460;
			npc.defense = 16;
			npc.knockBackResist = 0.1f;

			npc.noGravity = true;

			animationType = NPCID.Pixie;
			npc.HitSound = SoundID.NPCHit44;
			npc.DeathSound = SoundID.NPCDeath46;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 21;
			for(int k = 0; k < 20; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 1.5f * hitDirection, -1.5f, 0, default(Color), 0.57f);
				Dust.NewDust(npc.position, npc.width, npc.height, 159, 1.5f * hitDirection, -1.5f, 0, default(Color), 0.52f);
			}
			if(npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Glitterfly/Glitterfly1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Glitterfly/Glitterfly2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Glitterfly/Glitterfly3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Glitterfly/Glitterfly4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Glitterfly/Glitterfly5"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Glitterfly/Glitterfly6"), 1f);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return MyWorld.BlueMoon && NPC.CountNPCS(ModContent.NPCType<Glitterfly>()) < 3 ? 1f : 0f;
		}

		public override bool PreAI()
		{
			bool expertMode = Main.expertMode;
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.0f, 0.04f, 0.8f);

			Player player = Main.player[npc.target];

			if(npc.Center.X >= player.Center.X && moveSpeed >= Main.rand.Next(-60, -40)) // flies to players x position
				moveSpeed--;

			if(npc.Center.X <= player.Center.X && moveSpeed <= Main.rand.Next(40, 60))
				moveSpeed++;

			npc.velocity.X = moveSpeed * 0.13f;

			if(npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= Main.rand.Next(-40, -30)) //Flies to players Y position
			{
				moveSpeedY--;
				HomeY = Main.rand.NextFloat(160f, 180f);
			}

			if(npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= Main.rand.Next(30, 40))
				moveSpeedY++;

			npc.velocity.Y = moveSpeedY * 0.1f;
			++npc.ai[1];
			if(npc.ai[1] >= 10) {
				int proj = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("GlitterDust"), 0, 0, Main.myPlayer, 0, 0);
				npc.ai[1] = 0;
			}
			Vector2 center = npc.Center;
			float num8 = (float)player.miscCounter / 40f;
			float num7 = 1.0471975512f * 2;
			for(int k = 0; k < 3; k++) {
				{
					int num6 = Dust.NewDust(center, 0, 0, DustID.GoldCoin, 0f, 0f, 100, default(Color), 1.3f);
					Main.dust[num6].noGravity = true;
					Main.dust[num6].velocity = Vector2.Zero;
					Main.dust[num6].noLight = true;
					Main.dust[num6].position = center + (num8 * 6.28318548f + num7 * (float)k).ToRotationVector2() * 12f;
				}
			}
			int distance = (int)Math.Sqrt((npc.Center.X - player.Center.X) * (npc.Center.X - player.Center.X) + (npc.Center.Y - player.Center.Y) * (npc.Center.Y - player.Center.Y));
			if(distance < 540) {
				++npc.ai[0];
				if(npc.ai[0] == 140 || npc.ai[0] == 280 || npc.ai[0] == 320) {
					if(Main.netMode != NetmodeID.MultiplayerClient) {
						Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 43);
						Vector2 dir = Main.player[npc.target].Center - npc.Center;
						dir.Normalize();
						dir.X *= 9f;
						dir.Y *= 9f;
						float A = (float)Main.rand.Next(-200, 200) * 0.01f;
						float B = (float)Main.rand.Next(-200, 200) * 0.01f;
						int damage = expertMode ? 19 : 27;
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, dir.X + A, dir.Y + B, mod.ProjectileType("StarSting"), damage, 1, Main.myPlayer, 0, 0);
					}
				}
				if(npc.ai[0] >= 450) {
					HomeY -= 100f;
				}
				if(npc.ai[0] >= 456) {
					HomeY = 160f;
					npc.ai[0] = 0;
				}
			} else {
				npc.ai[0] = 0;
			}
			return true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/BlueMoon/Glitterfly_Glow"));
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if(Main.rand.Next(5) == 0)
				target.AddBuff(mod.BuffType("StarFlame"), 200);
		}

		public override void NPCLoot()
		{
			if(Main.rand.NextBool(5))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MoonStone>());
			if(Main.rand.NextBool(100))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<StopWatch>());
		}
	}
}