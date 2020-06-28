
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Tides
{
	public class MangoJelly : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mango Jelly");
			Main.npcFrameCount[npc.type] = 8;
		}

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 50;
			npc.damage = 24;
			npc.defense = 6;
			npc.lifeMax = 200;
			npc.noGravity = true;
			npc.knockBackResist = .9f;
			npc.value = 200f;
			npc.alpha = 35;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath1;
		}
		bool shooting = false;
		//npc.ai[0]: base timer
		int xoffset = 0;
		bool createdLaser = false;
		float bloomCounter = 1;
		bool bloom = false;
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if(bloom) {
				Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition + new Vector2(xoffset, 6)) - new Vector2(-2, 8), null, new Color((int)(22.5f * bloomCounter), (int)(13.8f * bloomCounter), (int)(21.6f * bloomCounter), 0), 0f, new Vector2(50, 50), 0.125f * (bloomCounter + 3), SpriteEffects.None, 0f);
			}
		}
		public override void AI()
		{
			npc.TargetClosest();
			Player player = Main.player[npc.target];
			npc.ai[0]++;
			if(player.position.X > npc.position.X) {
				xoffset = 24;
			} else {
				xoffset = -24;
			}
			if(npc.ai[0] == 400) {
				shooting = true;
				createdLaser = false;
				npc.frameCounter = 0;
			}
			if(npc.ai[0] == 550) {
				bloom = false;
				bloomCounter = 1;
				Vector2 vel = new Vector2(30f, 0).RotatedBy((float)(Main.rand.Next(90) * Math.PI / 180));
				Projectile.NewProjectile(npc.position + vel + new Vector2(xoffset, 6), vel, ModContent.ProjectileType<MangoLaser>(), npc.damage, 0, npc.target);
				Projectile.NewProjectile(npc.position + vel.RotatedBy(1.57) + new Vector2(xoffset, 6), Vector2.Zero, ModContent.ProjectileType<MangoLaser>(), npc.damage, 0, npc.target);
				Projectile.NewProjectile(npc.position + vel.RotatedBy(3.14) + new Vector2(xoffset, 6), Vector2.Zero, ModContent.ProjectileType<MangoLaser>(), npc.damage, 0, npc.target);
				Projectile.NewProjectile(npc.position + vel.RotatedBy(4.71) + new Vector2(xoffset, 6), Vector2.Zero, ModContent.ProjectileType<MangoLaser>(), npc.damage, 0, npc.target);
			}
			if(npc.ai[0] >= 570) {
				shooting = false;
				npc.ai[0] = 0;
			}
			if(shooting) {
				npc.knockBackResist = 0;
				npc.velocity = Vector2.Zero;
				bloomCounter += 0.02f;
				jump = false;
				npc.rotation = 0f;
			} else {
				npc.knockBackResist = .9f;
				#region regular movement
				npc.velocity.X *= 0.99f;
				if(!jump) {
					if(npc.velocity.Y < 2.5f) {
						npc.velocity.Y += 0.1f;
					}
					if(player.position.Y < npc.position.Y && npc.ai[0] % 30 == 0) {
						jump = true;
						npc.velocity.X = xoffset / 1.25f;
						npc.velocity.Y = -9;
					}
				}
				if(jump) {
					npc.velocity *= 0.97f;
					if(Math.Abs(npc.velocity.X) < 0.125f) {
						jump = false;
					}
					npc.rotation = npc.velocity.ToRotation() + 1.57f;
				}
				#endregion
			}
		}
		bool jump = false;
		public override void FindFrame(int frameHeight)
		{
			Player player = Main.player[npc.target];
			if(!shooting) {
				if(player.position.Y < npc.position.Y) {
					npc.frameCounter += 0.10f;
				}
				npc.frameCounter += 0.05f;
				npc.frameCounter %= 4;
				int frame = (int)npc.frameCounter;
				npc.frame.Y = frame * frameHeight;
			} else {
				if(npc.frameCounter < 2.8f) {
					npc.frameCounter += 0.1f;
				} else if(!bloom && npc.frameCounter < 3.8f) {
					npc.frameCounter += 0.08f;
				}
				if(npc.frameCounter >= 2.8f && !createdLaser) {
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
