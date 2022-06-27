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
			Main.npcFrameCount[NPC.type] = 9;
			NPCID.Sets.TownCritter[NPC.type] = true;
			NPCID.Sets.TrailCacheLength[NPC.type] = 3;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.friendly = false;
			NPC.townNPC = false;
			NPC.width = 76;
			NPC.height = 76;
			NPC.aiStyle = 0;
			NPC.damage = 12;
			NPC.defense = 9999;
			NPC.lifeMax = 50;
			NPC.noGravity = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
		}
		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= 3;
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}
		public override void AI()
		{
			NPC.TargetClosest(true);
			Player target = Main.player[NPC.target];

			if (NPC.localAI[0] == 0f) {
				NPC.localAI[0] = NPC.Center.Y;
			}
			if (NPC.Center.Y >= NPC.localAI[0]) {
				NPC.localAI[1] = -1f;
			}
			if (NPC.Center.Y <= NPC.localAI[0] - 2f) {
				NPC.localAI[1] = 1f;
			}
			NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y + 0.009f * NPC.localAI[1], -.5f, .5f);
			int distance = (int)(NPC.Center - target.Center).Length();
			if (distance <= 540 || NPC.life < NPC.lifeMax && !target.dead) {
				NPC.Transform(ModContent.NPCType<SamuraiHostile>());
			}
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				NPC.homeless = false;
				NPC.homeTileX = -1;
				NPC.homeTileY = -1;
				NPC.netUpdate = true;
			}
		}
	}
}
