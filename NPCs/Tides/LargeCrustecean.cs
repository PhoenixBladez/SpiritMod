using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Hostile;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Tide;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Magic;

namespace SpiritMod.NPCs.Tides
{
	public class LargeCrustecean : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble Brute");
			Main.npcFrameCount[npc.type] = 9;
		}

		public override void SetDefaults()
		{
			npc.width = 80;
			npc.height = 82;
			npc.damage = 25;
			npc.defense = 4;
			aiType = NPCID.WalkingAntlion;
			npc.aiStyle = 3;
			npc.lifeMax = 375;
			npc.knockBackResist = .2f;
			npc.value = 200f;
			npc.noTileCollide = false;
			npc.HitSound = SoundID.NPCHit18;
			npc.DeathSound = SoundID.NPCDeath5;
		}
		bool blocking = false;
		int blockTimer = 0;
		public override void AI()
		{
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			blockTimer++;
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
			if(blockTimer == 200) {
				//   Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraThrow"));
				npc.frameCounter = 0;
				npc.velocity.X = 0;
			}
			if(blockTimer > 250) {
				blocking = true;
			}
			if(blockTimer > 350) {
				blocking = false;
				blockTimer = 0;
				npc.frameCounter = 0;
			}
			if(blocking) {
				npc.aiStyle = 0;
				if(player.position.X > npc.position.X) {
					npc.spriteDirection = 1;
				} else {
					npc.spriteDirection = -1;
				}
			} else {
                npc.spriteDirection = npc.direction;
				npc.aiStyle = 3;
			}

		}
        public override void NPCLoot()
        {
            if (Main.rand.NextBool(8))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PumpBubbleGun>());
            }
        }
        public override void FindFrame(int frameHeight)
		{
			if((npc.collideY || npc.wet) && !blocking) {
				npc.frameCounter += 0.2f;
				npc.frameCounter %= 6;
				int frame = (int)npc.frameCounter;
				npc.frame.Y = frame * frameHeight;
			}
			if (npc.wet)
			{
				return;
			}
			if(blocking) {
				npc.frameCounter += 0.05f;
				npc.frameCounter = MathHelper.Clamp((float)npc.frameCounter, 0, 2.9f);
				int frame = (int)npc.frameCounter;
				npc.frame.Y = (frame + 6) * frameHeight;
				if(npc.frameCounter > 2 && blockTimer % 5 == 0) {
                    Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 85);
                    Projectile.NewProjectile(npc.Center.X + (npc.direction * 34), npc.Center.Y - 4, npc.direction * Main.rand.NextFloat(3, 6), 0 - Main.rand.NextFloat(1), ModContent.ProjectileType<LobsterBubbleSmall>(), npc.damage/2, 1, Main.myPlayer, 0, 0);
				}
			}
		}
		public override void HitEffect(int hitDirection, double damage)
        {
            int d = 5;
            int d1 = 5;
            for (int k = 0; k < 30; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), 1.14f);
            }
            if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster5"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster6"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster7"), 1f);
                if (TideWorld.TheTide)
                {
                    TideWorld.TidePoints += 1;
                }
            }
		}
	}
}
