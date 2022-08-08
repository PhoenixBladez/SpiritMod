using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Ghast
{
	public class IllusionistSpectre : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghast's Spectre");
			Main.npcFrameCount[NPC.type] = 5;
			NPCID.Sets.TrailCacheLength[NPC.type] = 3;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 90;
			NPC.lifeMax = 20;
			NPC.defense = 0;
			NPC.damage = 19;
			NPC.HitSound = SoundID.NPCHit11;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.knockBackResist = 0.75f;
			NPC.noGravity = true;
			NPC.netAlways = true;
			NPC.chaseable = false;
			NPC.noTileCollide = true;
			NPC.lavaImmune = true;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
				new FlavorTextBestiaryInfoElement("A convincing apparition summoned by Ghasts, made to do their bidding. It’s merely a mirror image of the real deal, don’t be fooled!"),
			});
		}

		int frame = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 100f;

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			Player player = Main.player[NPC.target];

			if (NPC.Center.X >= player.Center.X && moveSpeed >= -30) // flies to players x position
				moveSpeed--;
			if (NPC.Center.X <= player.Center.X && moveSpeed <= 30)
				moveSpeed++;

			NPC.velocity.X = moveSpeed * 0.15f;

			if (NPC.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -20) //Flies to players Y position
			{
				moveSpeedY--;
				HomeY = 125f;
			}

			if (NPC.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 20)
				moveSpeedY++;

			NPC.velocity.Y = moveSpeedY * 0.1f;
			if (Main.rand.NextBool(180))
				HomeY = -25f;
			Lighting.AddLight((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f), 0.3f, .3f, .3f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height * 0.5f));
			for (int k = 0; k < NPC.oldPos.Length; k++) {
				var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
				Color color = NPC.GetAlpha(drawColor) * (float)(((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
				spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
			}
			return true;
		}

		public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 100);

		public override void FindFrame(int frameHeight)
		{
			if (++NPC.frameCounter == 4)
			{
				NPC.frameCounter = 0;

				if (++frame >= 4)
					frame = 1;
			}

			NPC.frame.Y = frameHeight * frame;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int d1 = 180;
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, d1, 2.5f * hitDirection, -2.5f, 0, default, .74f);
			}
			if (NPC.life <= 0) {
				for (int k = 0; k < 30; k++) {
					int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, d1, 2.5f * hitDirection, -4.5f, 0, default, .74f);
					Main.dust[d].noGravity = true;
				}
			}
		}
	}
}