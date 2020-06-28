using SpiritMod.Projectiles.Hostile;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Tides
{
	public class KakamoraRunner : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kakamora");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 42;
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

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			if(Main.rand.NextBool(1500)) {
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraIdle3"));
			}
			var list = Main.npc.Where(x => x.Hitbox.Intersects(npc.Hitbox));
			foreach(var npc2 in list) {
				if(npc2.type == ModContent.NPCType<LargeCrustecean>() && npc.Center.Y > npc2.Center.Y && npc2.active) {
					npc.velocity.X = npc2.direction * 7;
					npc.velocity.Y = -2;
					Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraHit"));
				}
				var list2 = Main.projectile.Where(x => x.Hitbox.Intersects(npc.Hitbox));
				foreach(var proj in list2) {
					if(proj.type == ModContent.ProjectileType<ShamanBolt>() && proj.active) {
						npc.life += 30;
						npc.HealEffect(30, true);
						proj.active = false;
					}
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			if(npc.collideY) {
				npc.frameCounter += 0.2f;
				npc.frameCounter %= Main.npcFrameCount[npc.type];
				int frame = (int)npc.frameCounter;
				npc.frame.Y = frame * frameHeight;
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if(npc.life <= 0) {
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraDeath"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore3"), 1f);
			} else {
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraHit"));
			}
		}
	}
}
