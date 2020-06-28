
using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Hostile;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Tides
{
	public class SpearKakamora : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kakamora");
			Main.npcFrameCount[npc.type] = 7;
		}

		public override void SetDefaults()
		{
			npc.width = 52;
			npc.height = 38;
			npc.damage = 24;
			npc.defense = 4;
			aiType = NPCID.SnowFlinx;
			npc.aiStyle = 3;
			npc.lifeMax = 120;
			npc.knockBackResist = .70f;
			npc.value = 200f;
			npc.noTileCollide = false;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath1;
		}
		int timer = 0;
		bool charging = false;
		bool rotating = false;
		int chargeDirection = -1; //-1 is left, 1 is right
		bool throwing = false;
		bool thrownCoconut = false;
		public override void AI()
		{
			Player player = Main.player[npc.target];
			var list2 = Main.projectile.Where(x => x.Hitbox.Intersects(npc.Hitbox));
			foreach(var proj in list2) {
				if(proj.type == ModContent.ProjectileType<ShamanBolt>() && proj.active) {
					npc.life += 30;
					npc.HealEffect(30, true);
					proj.active = false;
				}
			}
			if(!throwing || charging) {
				timer++;
			}
			if(Main.rand.NextBool(1500)) {
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraIdle3"));
			}
			if(Main.rand.NextBool(150) && !charging && timer > 50) {
				if(!throwing) {
					throwing = true;
					npc.frameCounter = 0;
					Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraThrow"));
				}
			}
			if(throwing) {
				npc.aiStyle = -1;
				npc.velocity.X = 0;
				if(player.position.X > npc.position.X) {
					npc.spriteDirection = 1;
				} else {
					npc.spriteDirection = -1;
				}
			}
			if(timer == 170) {
				charging = true;
				npc.velocity.X = 0;
				if(player.position.X > npc.position.X) {
					chargeDirection = 1;
					npc.spriteDirection = 1;
				} else {
					chargeDirection = -1;
					npc.spriteDirection = -1;
				}
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraIdle2"));
			}
			if(charging) {
				if(timer == 230) {
					npc.velocity.X = 4 * chargeDirection;
					npc.velocity.Y = -7;
					Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraIdle1"));
				}
				if(timer < 230) {
					npc.aiStyle = -1;
					npc.velocity.X = -0.6f * chargeDirection;
				} else {
					npc.aiStyle = 26;
					npc.spriteDirection = npc.direction;
				}
				if(Math.Abs(npc.velocity.X) < 3 && timer > 230) {
					charging = false;
					npc.aiStyle = 3;
					npc.rotation = 0;
					timer = 0;
				}
				if((chargeDirection == 1 && player.position.X < npc.position.X) || (chargeDirection == -1 && player.position.X > npc.position.X) && timer > 230) {
					npc.rotation += 0.1f * npc.velocity.X;
					npc.velocity.Y = 5;
				}
			} else if(!throwing) {
				npc.rotation = 0;
				npc.aiStyle = 3;
				npc.spriteDirection = npc.direction;
				var list = Main.npc.Where(x => x.Hitbox.Intersects(npc.Hitbox));
				foreach(var npc2 in list) {
					if(npc2.type == ModContent.NPCType<LargeCrustecean>() && npc.Center.Y > npc2.Center.Y && npc2.active) {
						npc.velocity.X = npc2.direction * 7;
						npc.velocity.Y = -2;
						Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraHit"));
					}
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			if(!throwing) {
				if(npc.collideY) {
					if(charging) {
						npc.frameCounter += 0.1f;
					}
					npc.frameCounter += 0.2f;
					npc.frameCounter %= 4;
					int frame = (int)npc.frameCounter;
					npc.frame.Y = frame * frameHeight;
				}
			} else {
				npc.frameCounter += 0.08f;
				if(npc.frameCounter >= 3f) {
					throwing = false;
					npc.frameCounter = 0;
					npc.aiStyle = 3;
					thrownCoconut = false;
				}
				if(npc.frameCounter >= 1.3f && !thrownCoconut) {
					Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 1);
					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					direction.Normalize();
					direction *= 7f;
					float A = (float)Main.rand.Next(-50, 50) * 0.02f;
					float B = (float)Main.rand.Next(-50, 50) * 0.02f;
					int p = Projectile.NewProjectile(npc.Center.X + (npc.direction * 12), npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<CoconutHostile>(), npc.damage, 1, Main.myPlayer, 0, 0);
					thrownCoconut = true;
				}
				npc.frameCounter %= 3;
				int frame = (int)npc.frameCounter + 4;
				npc.frame.Y = frame * frameHeight;
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if(npc.life <= 0) {
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraDeath"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_GoreSpear"), 1f);
			} else {
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraHit"));
			}
		}
	}
}
