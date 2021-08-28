using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies
{
	public class Pulsar : ModNPC, IStarjinxEnemy
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pulsar");
			Main.npcFrameCount[npc.type] = 9;
		}
		public override void SetDefaults()
		{
			npc.lifeMax = 500;
			npc.defense = 25;
			npc.value = 600f;
			npc.alpha = 0;
			npc.friendly = false;
			npc.knockBackResist = 0f;
			npc.HitSound = SoundID.DD2_CrystalCartImpact;
			npc.DeathSound = SoundID.DD2_WitherBeastDeath;
			npc.width = 66;
			npc.height = 88;
			npc.damage = 45;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}
		public override bool CheckDead()
		{
			return false;
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			Main.PlaySound(SoundID.NPCHit, npc.Center, 4);
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, .934f);
			}
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, 99);
				Gore.NewGore(npc.position, npc.velocity, 99);
				Gore.NewGore(npc.position, npc.velocity, 99);
				Main.PlaySound(SoundID.Item, npc.Center, 14);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Pulsar/Pulsar1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Pulsar/Pulsar2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Pulsar/Pulsar3"), 1f);
			}
		}
		public override void AI()
		{
			npc.velocity *= 0.99f;
			npc.ai[0]++;
			npc.ai[1]++;
			if (npc.ai[0] >= 150 && !Filters.Scene["PulsarShockwave"].IsActive())
			{
				Main.PlaySound(SoundID.DD2_WyvernDiveDown, npc.Center);
				Projectile.NewProjectile(npc.Center - new Vector2(npc.rotation * 40, 0), Vector2.Zero, ModContent.ProjectileType<PulsarShockwave>(), 0, 0, npc.target);
				npc.ai[0] = 1;
				npc.ai[2] = 0;
				npc.netUpdate = true;
			}
			if (npc.ai[0] % 5 == 0)
			{
				int d = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Flare);
				Main.dust[d].fadeIn += .5f;
			}
			if (npc.ai[1] % 240 == 0)
			{
				Main.PlaySound(SoundID.DD2_WitherBeastAuraPulse, npc.Center);
			}
			if (npc.ai[0] >= 120 && npc.ai[0] < 150)
			{
				npc.ai[2] = 1;
			}
			npc.rotation = (float)Math.Sin(npc.ai[1] / 30f) / 2;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.ai[2] != 0)
			{
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (npc.spriteDirection == 1)
					spriteEffects = SpriteEffects.FlipHorizontally;
				Texture2D texture = Main.npcTexture[npc.type];
				float num99 = (float)(Math.Cos((double)Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 4.0 + 0.5);
				Microsoft.Xna.Framework.Color AfterimageColor = new Microsoft.Xna.Framework.Color((int)sbyte.MaxValue, (int)sbyte.MaxValue, (int)sbyte.MaxValue, 0).MultiplyRGBA(new Color(255, 111, 33, 150)) * 5f;
				Vector2 GlowPosition = new Vector2(npc.Center.X - 24, npc.Center.Y - 42) - Main.screenPosition - new Vector2((float)texture.Width / 3, (float)(texture.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f + npc.frame.Size() / 2 * npc.scale + new Vector2(0.0f, npc.gfxOffY);
				for (int index2 = 0; index2 < 4; ++index2)
				{
					Microsoft.Xna.Framework.Color GlowColor = npc.GetAlpha(AfterimageColor) * (1f - num99);
					Vector2 GlowPosition2 = new Vector2(npc.Center.X - 24, npc.Center.Y - 42) + ((float)((double)index2 / (double)4 * 6.28318548202515) + npc.rotation).ToRotationVector2() * (float)(8.0 * (double)num99 + 2.0) - Main.screenPosition - new Vector2((float)texture.Width / 3, (float)(texture.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f + npc.frame.Size() / 2 * npc.scale + new Vector2(0.0f, npc.gfxOffY);
					Main.spriteBatch.Draw(texture, GlowPosition2, new Microsoft.Xna.Framework.Rectangle?(npc.frame), GlowColor, npc.rotation, new Vector2(33, 0), npc.scale, spriteEffects, 0.0f);
				}
				Main.spriteBatch.Draw(texture, GlowPosition, new Microsoft.Xna.Framework.Rectangle?(npc.frame), AfterimageColor, npc.rotation, new Vector2(33, 0), npc.scale, spriteEffects, 0.0f);
			}
			Main.spriteBatch.Draw(Main.npcTexture[npc.type], npc.position + new Vector2(33, 0) - Main.screenPosition, npc.frame, drawColor, npc.rotation, new Vector2(33, 0), npc.scale, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/Enemies/Pulsar_Glow"), npc.position + new Vector2(33, 0) - Main.screenPosition, npc.frame, Color.White, npc.rotation, new Vector2(33, 0), npc.scale, SpriteEffects.None, 0f);

			return false;

		}
		public void DrawPathfinderOutline(SpriteBatch spriteBatch) => PathfinderOutlineDraw.DrawAfterImage(spriteBatch, npc, npc.frame, new Vector2(0, -44), Color.White, 0.75f, 1, 1.4f, new Vector2(33, 0));
	}
}