using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.NPCs.SamuraiGhost
{
	public class SamuraiPassive : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantom Samurai");
			Main.npcFrameCount[npc.type] = 9;
			NPCID.Sets.TownCritter[npc.type] = true;
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.friendly = false;
			npc.townNPC = false;
			npc.width = 76;
			npc.height = 76;
			npc.aiStyle = 0;
			npc.damage = 12;
			npc.defense = 9999;
			npc.lifeMax = 50;
			npc.noGravity = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0f;
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= 3;
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		public override void AI()
		{
			npc.TargetClosest(true);
			Player target = Main.player[npc.target];

			if (npc.localAI[0] == 0f) {
				npc.localAI[0] = npc.Center.Y;
			}
			if (npc.Center.Y >= npc.localAI[0]) {
				npc.localAI[1] = -1f;
			}
			if (npc.Center.Y <= npc.localAI[0] - 2f) {
				npc.localAI[1] = 1f;
			}
			npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y + 0.009f * npc.localAI[1], -.5f, .5f);
			int distance = (int)(npc.Center - target.Center).Length();
			if (distance <= 540 || npc.life < npc.lifeMax && !target.dead) {
				npc.Transform(ModContent.NPCType<SamuraiHostile>());
			}
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				npc.homeless = false;
				npc.homeTileX = -1;
				npc.homeTileY = -1;
				npc.netUpdate = true;
			}
		}
	}
}
