using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.NPCs.Reach
{
	public class ReachmanPassive : ModNPC
	{
		public static int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Feral Hunter");
			Main.npcFrameCount[npc.type] = 1;
			NPCID.Sets.TownCritter[npc.type] = true;
		}

		public override void SetDefaults()
		{
			npc.friendly = false;
			npc.townNPC = true;
			npc.width = 32;
			npc.height = 48;
			npc.aiStyle = 0;
			npc.damage = 12;
			npc.defense = 9999;
			npc.lifeMax = 46;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0f;
		}
		public override void AI()
		{
			Player target = Main.player[npc.target];
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.46f, 0.32f, .1f);

			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			if(distance <= 160 || npc.life < npc.lifeMax) {
				npc.Transform(ModContent.NPCType<Reachman>());
				for(int i = 0; i < 10; i++) {
					int num = Dust.NewDust(npc.position, npc.width, npc.height, DustID.GoldCoin, 0f, -2f, 0, default(Color), 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].scale *= .25f;
					if(Main.dust[num].position != npc.Center)
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 4f;
				}
			}
			if(Main.netMode != NetmodeID.MultiplayerClient) {
				npc.homeless = false;
				npc.homeTileX = -1;
				npc.homeTileY = -1;
				npc.netUpdate = true;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture;
			texture = Main.npcTexture[npc.type];
			spriteBatch.Draw
			(
				Main.extraTexture[60],
				new Vector2
				(
					npc.position.X - Main.screenPosition.X + npc.width * 0.5f - 40,
					npc.position.Y - Main.screenPosition.Y + npc.height - texture.Height * 0.5f - 25
				),
				new Microsoft.Xna.Framework.Rectangle?(),
				new Color(237, 175, 40, 0),
				0f,
				texture.Size() * 0.5f,
				1f,
				SpriteEffects.None,
				0f
			);
			return true;
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 3;
			int d1 = 7;
			for(int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
			}

			if(npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Reach1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Reach2"), 1f);
			}
		}
	}
}
