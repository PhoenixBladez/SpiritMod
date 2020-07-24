
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Tide;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Magic;

namespace SpiritMod.NPCs.Tides
{
	public class MangoJelly : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mang-O War");
			Main.npcFrameCount[npc.type] = 8;
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 50;
			npc.damage = 30;
			npc.defense = 6;
			npc.lifeMax = 225;
			npc.noGravity = true;
			npc.knockBackResist = .03f;
			npc.value = 200f;
			npc.alpha = 35;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit25;
			npc.DeathSound = SoundID.NPCDeath28;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.MangoWarBanner>();
		}
		//npc.ai[0]: base timer
		int xoffset = 0;
		bool createdLaser = false;
		float bloomCounter = 1;
		bool bloom = false;
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (bloom) {
				Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition) - new Vector2(-2, 8), null, new Color((int)(22.5f * bloomCounter), (int)(13.8f * bloomCounter), (int)(21.6f * bloomCounter), 0), 0f, new Vector2(50, 50), 0.125f * (bloomCounter + 3), SpriteEffects.None, 0f);
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 173;
			for (int k = 0; k < 20; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
			}
			if (npc.life <= 0) {
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MangoJelly/MangoJelly1"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MangoJelly/MangoJelly2"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MangoJelly/MangoJelly3"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MangoJelly/MangoJelly4"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MangoJelly/MangoJelly5"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MangoJelly/MangoJelly6"), 1f);
					if (TideWorld.TheTide && TideWorld.TidePoints < 99) {
						if (TideWorld.TidePoints < 98) {
							TideWorld.TidePoints += 2;
						}
						else {
							TideWorld.TidePoints += 1;
						}
					}
				}
			}
		}

		public override void AI()
		{
			npc.TargetClosest();
			Player player = Main.player[npc.target];
			npc.ai[0]++;
			if (player.position.X > npc.position.X) {
				xoffset = 24;
			}
			else {
				xoffset = -24;
			}
			if (npc.ai[0] == 400) {
				npc.ai[2] = 1;
				createdLaser = false;
				npc.frameCounter = 0;
				npc.netUpdate = true;
			}
			if (npc.ai[0] == 550) {
				bloom = false;
				bloomCounter = 1;
				Vector2 vel = new Vector2(30f, 0).RotatedBy((float)(Main.rand.Next(90) * Math.PI / 180));
				Main.PlaySound(2, npc.Center, 91);
				Projectile.NewProjectile(npc.position + vel + new Vector2(xoffset, 6), vel, ModContent.ProjectileType<MangoLaser>(), npc.damage, 0, npc.target);
				Projectile.NewProjectile(npc.position + vel.RotatedBy(1.57) + new Vector2(xoffset, 6), Vector2.Zero, ModContent.ProjectileType<MangoLaser>(), npc.damage / 3, 0, npc.target);
				Projectile.NewProjectile(npc.position + vel.RotatedBy(3.14) + new Vector2(xoffset, 6), Vector2.Zero, ModContent.ProjectileType<MangoLaser>(), npc.damage / 3, 0, npc.target);
				Projectile.NewProjectile(npc.position + vel.RotatedBy(4.71) + new Vector2(xoffset, 6), Vector2.Zero, ModContent.ProjectileType<MangoLaser>(), npc.damage / 3, 0, npc.target);
				npc.netUpdate = true;
			}
			if (npc.ai[0] >= 570) {
				npc.ai[2] = 0;
				npc.ai[0] = 0;
				npc.netUpdate = true;
			}
			if (npc.ai[2] == 1) { //shooting
				float num395 = Main.mouseTextColor / 200f - 0.35f;
				num395 *= 0.2f;
				npc.scale = num395 + 0.95f;
				npc.knockBackResist = 0;
				npc.velocity = Vector2.Zero;
				bloomCounter += 0.02f;
				jump = false;
				npc.rotation = 0f;
			}
			else {
				npc.knockBackResist = .9f;
				#region regular movement
				npc.velocity.X *= 0.99f;
				if (npc.ai[1] == 0) { //not jumping
					if (npc.velocity.Y < 2.5f) {
						npc.velocity.Y += 0.1f;
					}
					if (player.position.Y < npc.position.Y && npc.ai[0] % 30 == 0) {
						npc.ai[1] = 1;
						npc.velocity.X = xoffset / 1.25f;
						npc.velocity.Y = -6;
						npc.netUpdate = true;
					}
				}
				if (npc.ai[1] == 1) { //jumping
					npc.velocity *= 0.97f;
					if (Math.Abs(npc.velocity.X) < 0.125f) {
						npc.ai[1] = 0;
						npc.netUpdate = true;
					}
					npc.rotation = npc.velocity.ToRotation() + 1.57f;
				}
				#endregion
			}
		}
		public override void NPCLoot()
		{
			if (Main.rand.NextBool(25)) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MagicConch>());
			}
			if (Main.rand.NextBool(25)) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MangoJellyStaff>());
			}
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height * 0.5f));
			for (int k = 0; k < npc.oldPos.Length; k++) {
				var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
				Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
				spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
			}
			return true;
		}
		bool jump = false;
		public override void FindFrame(int frameHeight)
		{
			Player player = Main.player[npc.target];
			if (npc.ai[2] == 0) {
				if (player.position.Y < npc.position.Y) {
					npc.frameCounter += 0.10f;
				}
				npc.frameCounter += 0.05f;
				npc.frameCounter %= 4;
				int frame = (int)npc.frameCounter;
				npc.frame.Y = frame * frameHeight;
			}
			else {
				if (npc.frameCounter < 2.8f) {
					npc.frameCounter += 0.1f;
				}
				else if (!bloom && npc.frameCounter < 3.8f) {
					npc.frameCounter += 0.08f;
				}
				if (npc.frameCounter >= 2.8f && !createdLaser) {
					bloom = true;
					createdLaser = true;
				}
				npc.frameCounter %= 4;
				int frame = (int)npc.frameCounter + 4;
				npc.frame.Y = frame * frameHeight;
			}
		}
	}
}
