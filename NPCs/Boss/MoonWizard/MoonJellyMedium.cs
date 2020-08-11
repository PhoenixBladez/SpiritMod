
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Accessory;
using System;

namespace SpiritMod.NPCs.Boss.MoonWizard
{
	public class MoonJellyMedium : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Wizard");
			Main.npcFrameCount[npc.type] = 7;
		}

		public override void SetDefaults()
		{
			npc.width = 46;
			npc.height = 72;
			npc.damage = 18;
			npc.defense = 6;
			npc.lifeMax = 50;
			npc.noGravity = true;
			npc.value = 90f;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.DD2_GoblinHurt;
			npc.DeathSound = SoundID.NPCDeath22;
			npc.noGravity = true;
        }
		int xoffset = 0;
		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.ai[0]++;
			if(player.position.X > npc.position.X) {
				xoffset = 16;
			} else {
				xoffset = -16;
			}
			npc.velocity.X *= 0.99f;
				if(npc.ai[1] == 0) {
					if(npc.velocity.Y < 2.5f) {
						npc.velocity.Y += 0.1f;
					}
					if(player.position.Y < npc.position.Y && npc.ai[0] % 30 == 0) {
						npc.ai[1] = 1;
						npc.netUpdate = true;
						npc.velocity.X = xoffset / 1.25f;
						npc.velocity.Y = -6;
					}
				}
				if(npc.ai[1] == 1) {
					npc.velocity *= 0.97f;
					if(Math.Abs(npc.velocity.X) < 0.125f) {
						npc.ai[1] = 0;
						npc.netUpdate = true;
					}
					npc.rotation = npc.velocity.ToRotation() + 1.57f;
				}
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
	}
}
