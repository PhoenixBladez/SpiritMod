using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.NPCs.Yurei
{
	public class PagodaGhostPassive : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yuurei");
			Main.npcFrameCount[NPC.type] = 4;
			NPCID.Sets.TownCritter[NPC.type] = true;
			NPCID.Sets.TrailCacheLength[NPC.type] = 3;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.friendly = false;
			NPC.townNPC = false;
			NPC.width = 32;
			NPC.height = 48;
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
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}
		public override void AI()
		{
			Player target = Main.player[NPC.target];

			if (NPC.localAI[0] == 0f) {
				NPC.localAI[0] = NPC.Center.Y;
				NPC.netUpdate = true; //localAI probably isnt affected by this... buuuut might as well play it safe
			}
			if (NPC.Center.Y >= NPC.localAI[0]) {
				NPC.localAI[1] = -1f;
				NPC.netUpdate = true;
			}
			if (NPC.Center.Y <= NPC.localAI[0] - 2f) {
				NPC.localAI[1] = 1f;
				NPC.netUpdate = true;
			}
			NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y + 0.009f * NPC.localAI[1], -.5f, .5f);
			int distance = (int)Math.Sqrt((NPC.Center.X - target.Center.X) * (NPC.Center.X - target.Center.X) + (NPC.Center.Y - target.Center.Y) * (NPC.Center.Y - target.Center.Y));
			if (distance <= 540 || NPC.life < NPC.lifeMax && !target.dead) {
				NPC.Transform(ModContent.NPCType<PagodaGhostHostile>());
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height * 0.5f));
			for (int k = 0; k < NPC.oldPos.Length; k++) {
				var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
				Color color = NPC.GetAlpha(drawColor) * (float)(((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
				spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
			}
			return true;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 100);
		}
	}
}
