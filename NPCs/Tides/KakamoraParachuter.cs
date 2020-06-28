
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Tides
{
	public class KakamoraParachuter : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kakamora");
			Main.npcFrameCount[npc.type] = 3;
		}

		public override void SetDefaults()
		{
			npc.width = 46;
			npc.height = 60;
			npc.damage = 24;
			npc.defense = 6;
			npc.lifeMax = 200;
			npc.noGravity = true;
			npc.knockBackResist = .9f;
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
			Player player = Main.player[npc.target];
			if(player.position.X > npc.position.X) {
				npc.velocity.X = MathHelper.Clamp(npc.velocity.X + 0.2f, -4, 4);
			} else {
				npc.velocity.X = MathHelper.Clamp(npc.velocity.X - 0.2f, -4, 4);
			}
			npc.velocity.Y = 1;
			if(npc.collideY) {
				switch(Main.rand.Next(4)) {
					case 0:
						npc.Transform(ModContent.NPCType<KakamoraRunner>());
						break;
					case 1:
						npc.Transform(ModContent.NPCType<SpearKakamora>());
						break;
					case 2:
						npc.Transform(ModContent.NPCType<SwordKakamora>());
						break;
					case 3:
						npc.Transform(ModContent.NPCType<KakamoraShielder>());
						break;
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if(npc.life <= 0) {
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraDeath"));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_GoreGlider"), 1f);
			} else {
				Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraHit"));
			}
		}
	}
}
