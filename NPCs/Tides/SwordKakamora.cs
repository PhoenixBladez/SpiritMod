using SpiritMod.Projectiles.Hostile;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Tide;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Thrown;

namespace SpiritMod.NPCs.Tides
{
	public class SwordKakamora : ModNPC
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
			npc.damage = 28;
			npc.defense = 13;
			aiType = NPCID.SnowFlinx;
			npc.aiStyle = 3;
			npc.lifeMax = 120;
			npc.knockBackResist = .10f;
			npc.value = 200f;
			npc.noTileCollide = false;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath1;
		}
		int timer = 0;
		bool charging = false;
		bool rotating = false;
		int chargeDirection = -1; //-1 is left, 1 is right
		public override void AI()
		{
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            var list2 = Main.projectile.Where(x => x.Hitbox.Intersects(npc.Hitbox));
            foreach (var proj in list2)
            {
                if (proj.type == ModContent.ProjectileType<ShamanBolt>() && proj.active && npc.life < npc.lifeMax - 30)
                {
                    npc.life += 30;
                    npc.HealEffect(30, true);
                    proj.active = false;
                }
                else if (proj.type == ModContent.ProjectileType<ShamanBolt>() && proj.active && npc.life > npc.lifeMax - 30)
                {
                    npc.life += npc.lifeMax - npc.life;
                    npc.HealEffect(npc.lifeMax - npc.life, true);
                    proj.active = false;
                }
            }
            if (npc.wet)
            {
                npc.noGravity = true;
                if (npc.velocity.Y > -7)
                {
                    npc.velocity.Y -= .085f;
                }
				return;
            }
			else
            {
                npc.noGravity = false;
            }
            timer++;
			if(timer == 120) {
				charging = true;
				npc.velocity.X = 0;
				if(player.position.X > npc.position.X) {
					chargeDirection = 1;
					npc.spriteDirection = 1;
				} else {
					chargeDirection = -1;
					npc.spriteDirection = -1;
				}
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraDeath"));
			}
			if(charging) {
				if(timer == 180) {
					npc.velocity.X = 4.5f * chargeDirection;
					npc.velocity.Y = -7;
				}
				if(timer < 180) {
					npc.aiStyle = -1;
					npc.velocity.X = -0.6f * chargeDirection;
				} else {
					npc.aiStyle = 26;
					npc.spriteDirection = npc.direction;
				}
				if(Math.Abs(npc.velocity.X) < 3 && timer > 180) {
					charging = false;
					npc.aiStyle = 3;
					npc.rotation = 0;
					timer = 0;
				}
				if((chargeDirection == 1 && player.position.X < npc.position.X) || (chargeDirection == -1 && player.position.X > npc.position.X) && timer > 230) {
					npc.rotation += 0.1f * npc.velocity.X;
					npc.velocity.Y = 5;
				}
			} else {
				npc.aiStyle = 3;
				npc.spriteDirection = npc.direction;
				npc.rotation = 0;
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
        public override void NPCLoot()
        {
            if (Main.rand.NextBool(50))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CoconutGun>());
            }
            if (Main.rand.NextBool(50))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<TikiJavelin>());
            }
        }
        public override void FindFrame(int frameHeight)
		{
			if(charging) {
				npc.frameCounter += 0.1f;
			}
			npc.frameCounter += 0.2f;
			npc.frameCounter %= 4;
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		public override void HitEffect(int hitDirection, double damage)
        {
            int d = 207;
            int d1 = 207;
            for (int k = 0; k < 30; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
            }
            if (npc.life <= 0) {
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraDeath"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_GoreSword"), 1f);
                if (TideWorld.TheTide)
                {
                    TideWorld.TidePoints += 1;
                }
            } else {
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraHit"));
			}
		}
	}
}
