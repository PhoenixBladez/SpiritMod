
using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Hostile;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Tides
{
	public class LargeCrustecean : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Large Crustecean");
			Main.npcFrameCount[npc.type] = 9;
		}

		public override void SetDefaults()
		{
			npc.width = 80;
			npc.height = 82;
			npc.damage = 24;
			npc.defense = 4;
			aiType = NPCID.SnowFlinx;
			npc.aiStyle = 3;
			npc.lifeMax = 1000;
			npc.knockBackResist = .4f;
			npc.value = 200f;
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
			blockTimer++;
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
				if(Main.rand.NextBool(1500)) {
					Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraIdle3"));
				}
			}

		}

		public override void FindFrame(int frameHeight)
		{
			if(npc.collideY && !blocking) {
				npc.frameCounter += 0.2f;
				npc.frameCounter %= 6;
				int frame = (int)npc.frameCounter;
				npc.frame.Y = frame * frameHeight;
			}
			if(blocking) {
				npc.frameCounter += 0.05f;
				npc.frameCounter = MathHelper.Clamp((float)npc.frameCounter, 0, 2.9f);
				int frame = (int)npc.frameCounter;
				npc.frame.Y = (frame + 6) * frameHeight;
				if(npc.frameCounter > 2 && blockTimer % 5 == 0) {
					Projectile.NewProjectile(npc.Center.X + (npc.direction * 34), npc.Center.Y - 8, npc.direction * Main.rand.NextFloat(3, 6), 0 - Main.rand.NextFloat(1), ModContent.ProjectileType<LobsterBubbleSmall>(), npc.damage, 1, Main.myPlayer, 0, 0);
				}
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if(npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster5"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster6"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster7"), 1f);
			}
		}
	}
}
