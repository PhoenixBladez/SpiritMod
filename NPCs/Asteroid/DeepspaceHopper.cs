using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor.AstronautVanity;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Asteroid
{
	public class DeepspaceHopper : ModNPC
	{
		Vector2 direction9 = Vector2.Zero;
		//private bool shooting;
		private int timer = 300;
		private int distance = 300;
		//private bool inblock = true;
		//Vector2 target = Vector2.Zero;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shockhopper");
			Main.npcFrameCount[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.width = 54;
			npc.height = 24;
			npc.damage = 15;
			npc.defense = 9;
			npc.lifeMax = 68;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.value = 130f;
			npc.knockBackResist = .45f;
			npc.aiStyle = -1;
			npc.noGravity = true;
			npc.noTileCollide = false;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for(int k = 0; k < 12; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 226, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, 226, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}
			if(npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hopper/Hopper1"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hopper/Hopper2"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hopper/Hopper3"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hopper/Hopper4"));
				for(int i = 0; i < 15; i++) {
					int num = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, -2f, 0, default, 2f);
					Main.dust[num].noGravity = true;
					Dust expr_62_cp_0 = Main.dust[num];
					expr_62_cp_0.position.X += (Main.rand.Next(-50, 51) / 20) - 1.5f;
					Dust expr_92_cp_0 = Main.dust[num];
					expr_92_cp_0.position.Y += (Main.rand.Next(-50, 51) / 20) - 1.5f;
					Main.dust[num].scale = 0.4f;
					if(Main.dust[num].position != npc.Center) {
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 3f;
					}
				}
			}
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
			if(npc.alpha != 255) {
				GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Asteroid/DeepspaceHopper_Glow"));
			}
		}

		public override bool PreAI()
		{
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];

			float num5 = npc.position.X + (npc.width / 2) - player.position.X - (player.width / 2);
			float num6 = npc.position.Y + npc.height - 59f - player.position.Y - (player.height / 2);
			float num7 = (float)Math.Atan2(num6, num5) + 1.57f;
			if(!(timer >= 100 && timer <= 130)) {
				if(num7 < 0f) {
					num7 += 6.283f;
				} else if(num7 > 6.283) {
					num7 -= 6.283f;
				}
				float num8 = 0.1f;
				if(npc.rotation < num7) {
					if((num7 - npc.rotation) > 3.1415) {
						npc.rotation -= num8;
					} else {
						npc.rotation += num8;
					}
				} else if(npc.rotation > num7) {
					if((npc.rotation - num7) > 3.1415) {
						npc.rotation += num8;
					} else {
						npc.rotation -= num8;
					}
				}
				if(npc.rotation > num7 - num8 && npc.rotation < num7 + num8) {
					npc.rotation = num7;
				}
				if(npc.rotation < 0f) {
					npc.rotation += 6.283f;
				} else if(npc.rotation > 6.283) {
					npc.rotation -= 6.283f;
				}
				if(npc.rotation > num7 - num8 && npc.rotation < num7 + num8) {
					npc.rotation = num7;
				}
			}
			npc.spriteDirection = npc.direction;
			timer++;
			if(timer >= 280) {
				int angle = Main.rand.Next(360);
				double anglex = Math.Sin(angle * (Math.PI / 180));
				double angley = Math.Cos(angle * (Math.PI / 180));
				npc.position.X = player.Center.X + (int)(distance * anglex);
				npc.position.Y = player.Center.Y + (int)(distance * angley);
				if(Main.tile[(int)(npc.position.X / 16), (int)(npc.position.Y / 16)].active()) {
					npc.alpha = 255;
				} else {
					timer = 0;
					npc.alpha = 0;
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);
					for(int i = 0; i < 15; i++) {
						int num = Dust.NewDust(npc.position, npc.width, npc.height, 226, 0f, -2f, 0, default, 2f);
						Main.dust[num].noGravity = true;
						Dust expr_62_cp_0 = Main.dust[num];
						expr_62_cp_0.position.X += ((Main.rand.Next(-50, 51) / 20) - 1.5f);
						Dust expr_92_cp_0 = Main.dust[num];
						expr_92_cp_0.position.Y += ((Main.rand.Next(-50, 51) / 20) - 1.5f);
						Main.dust[num].scale = 0.4f;
						if(Main.dust[num].position != npc.Center) {
							Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 3f;
						}
					}
				}
			} else {
				npc.velocity.X = 0;
				npc.velocity.Y = 0;
			}
			if(timer == 100) {
				Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 8);
				direction9 = player.Center - npc.Center;
				direction9.Normalize();
			}
			if(timer >= 100 && timer <= 130) {
				{
					int dust = Dust.NewDust(npc.Center, npc.width, npc.height, 226);
					Main.dust[dust].velocity *= -1f;
					Main.dust[dust].scale *= .8f;
					Main.dust[dust].noGravity = true;
					Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-80, 81), (float)Main.rand.Next(-80, 81));
					vector2_1.Normalize();
					Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
					Main.dust[dust].velocity = vector2_2;
					vector2_2.Normalize();
					Vector2 vector2_3 = vector2_2 * 34f;
					Main.dust[dust].position = npc.Center - vector2_3;
				}
			}
			if(npc.alpha != 255) {
				if(Main.rand.NextFloat() < 0.5f) {
					// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
					Vector2 position = new Vector2(npc.Center.X - 10, npc.Center.Y);
					Dust.NewDustPerfect(position, 226, new Vector2(0f, -6.421053f).RotatedBy(npc.rotation), 0, new Color(255, 0, 0), 0.6578947f);
				}
				if(Main.rand.NextFloat() < 0.5f) {
					// You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
					Vector2 position = new Vector2(npc.Center.X + 10, npc.Center.Y);
					Dust.NewDustPerfect(position, 226, new Vector2(0f, -6.421053f).RotatedBy(npc.rotation), 0, new Color(255, 0, 0), 0.6578947f);
				}
				if(timer == 130) //change to frame related later
				{
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 91);
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction9.X * 30, direction9.Y * 30, ModContent.ProjectileType<HopperLaser>(), 19, 1, Main.myPlayer);
				}
			}
			return false;
		}
		public override void NPCLoot()
		{
			if(Main.rand.NextBool(70)) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GateStaff>(), 1);
			}
			if(Main.rand.NextBool(400)) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GravityModulator>());
			}
			int[] lootTable = { 
				ModContent.ItemType<AstronautLegs>(),
				ModContent.ItemType<AstronautHelm>(),
				ModContent.ItemType<AstronautBody>()
			};
			if(Main.rand.NextBool(40)) {
				int loot = Main.rand.Next(lootTable.Length);
				npc.DropItem(lootTable[loot]);
			}
		}
	}
}
