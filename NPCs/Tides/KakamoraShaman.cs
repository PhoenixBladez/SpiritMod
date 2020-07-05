
using Microsoft.Xna.Framework;
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
	public class KakamoraShaman : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kakamora Shaman");
			Main.npcFrameCount[npc.type] = 7;
		}

		public override void SetDefaults()
		{
			npc.width = 58;
			npc.height = 54;
			npc.damage = 24;
			npc.defense = 14;
			aiType = NPCID.SnowFlinx;
			npc.aiStyle = 3;
			npc.lifeMax = 180;
			npc.knockBackResist = 0.1f;
			npc.value = 400f;
			npc.noTileCollide = false;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath1;
		}
		bool blocking = false;
		int blockTimer = 0;
		public override void AI()
        {
            npc.TargetClosest(true);
			Player player = Main.player[npc.target];
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
			blockTimer++;
			if(blockTimer == 200) {
                Main.PlaySound(SoundID.DD2_WitherBeastAuraPulse, npc.Center);
                npc.frameCounter = 0;
				healed = false;
				npc.velocity.X = 0;
			}
			if(blockTimer > 200) {
				blocking = true;
			}
			if(blockTimer > 350) {
				blocking = false;
				blockTimer = 0;
				npc.frameCounter = 0;
			}
			if(blocking) {
				npc.aiStyle = 0;
                npc.noGravity = false;
				if(player.position.X > npc.position.X) {
					npc.spriteDirection = 1;
				} else {
					npc.spriteDirection = -1;
				}
			} else {
                npc.spriteDirection = npc.direction;
				npc.aiStyle = 3;
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
            if (Main.rand.NextBool(15))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<MagicConch>());
            }
        }
        public override void FindFrame(int frameHeight)
		{
			if((npc.collideY || npc.wet) && !blocking) {
				npc.frameCounter += 0.2f;
				npc.frameCounter %= 4;
				int frame = (int)npc.frameCounter;
				npc.frame.Y = frame * frameHeight;
			}
			if (npc.wet)
			{
				return;
			}
			if(blocking) {
				npc.frameCounter += 0.05f;
				if(npc.frameCounter > 2 && !healed) {
					var list = Main.npc;
					foreach(var npc2 in list) {
						if(npc2.type == ModContent.NPCType<KakamoraRunner>() || npc2.type == ModContent.NPCType<KakamoraShielder>() || npc2.type == ModContent.NPCType<KakamoraShielderRare>() || npc2.type == ModContent.NPCType<SpearKakamora>() || npc2.type == ModContent.NPCType<SwordKakamora>()) {
							if(Math.Abs(npc2.position.X - npc.position.X) < 500 && npc2.active && npc2.life < npc2.lifeMax) //500 is distance away he heals
							{
								int bolt = Terraria.Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-66, 66), npc.Center.Y - Main.rand.Next(60, 120), 0, 0, ModContent.ProjectileType<ShamanBolt>(), 0, 0);
								Projectile p = Main.projectile[bolt];
								Vector2 direction = npc2.Center - p.Center;
								direction.Normalize();
								direction *= 4;
								p.velocity = direction;
								p.ai[0] = direction.X;
								p.ai[1] = direction.Y;
							}
						}
					}
					for(int j = 0; j < 25; j++) {
						Dust.NewDustPerfect(new Vector2(npc.Center.X + npc.direction * 22, npc.Center.Y), 173, new Vector2(Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-16, 0)));
					}
					healed = true;
				}
				npc.frameCounter = MathHelper.Clamp((float)npc.frameCounter, 0, 2.9f);
				int frame = (int)npc.frameCounter;
				npc.frame.Y = (frame + 4) * frameHeight;
			}
		}
		bool healed = false;
		public override void HitEffect(int hitDirection, double damage)
        {
            int d = 207;
            int d1 = 207;
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
            }
            if (npc.life <= 0) {
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraDeath"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore3"), 1f);
                if (TideWorld.TheTide && TideWorld.TidePoints < 99)
                {
                    TideWorld.TidePoints += 1;
                }
            } else 
			{
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraHit"));
			}
		}
	}
}
