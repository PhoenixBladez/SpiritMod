using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Voidgazer
{
	public class Voidgazer : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voidgazer");
			Main.npcFrameCount[npc.type] = 4;
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SetDefaults()
		{
			npc.width = 88;
			npc.height = 60;
			npc.damage = 20;
			npc.defense = 15;
			npc.lifeMax = 120;
			npc.noGravity = true;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 360f;
			npc.rarity = 2;
			npc.knockBackResist = .45f;
		}
		public override void AI()
		{
			float f1 = (float) ((double) npc.localAI[0] % 6.28318548202515 - 3.14159274101257);
			float num11 = (float) Math.IEEERemainder((double) npc.localAI[1], 1.0);
			if ((double) num11 < 0.0)
				++num11;			
            float num12 = (float) Math.Floor((double) npc.localAI[1]);
			float max = 0.999f;
			float f2;
			float amount = 0.1f;
			float num16;
			float num17;
            Player player = Main.player[npc.target];
            if (player.active && !player.dead)
            {
                f2 = npc.AngleTo(player.Center);
				num16 = MathHelper.Clamp(num11 + 0.05f, 0.0f, max);
				num17 = num12 + (float) Math.Sign(6f - num12);
            }
            else
			{
				f2 = npc.AngleTo(Main.screenPosition + new Vector2((float) Main.mouseX, (float) Main.mouseY));
				num16 = MathHelper.Clamp(num11 + 0.05f, 0.0f, max);
				num17 = num12 + (float) Math.Sign(6f - num12);
			}

			Vector2 rotationVector2 = f2.ToRotationVector2();
			npc.localAI[0] = (float) ((double) Vector2.Lerp(f1.ToRotationVector2(), rotationVector2, amount).ToRotation() + (double) 3 * 6.28318548202515 + 3.14159274101257);
			npc.localAI[1] = num17 + num16;
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= 4;
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[npc.target];
			Vector2 position1 = npc.position + new Vector2((float)npc.width, (float)npc.height + 4) / 2f + Vector2.UnitY * npc.gfxOffY - Main.screenPosition;
			Microsoft.Xna.Framework.Color alpha = npc.GetAlpha(lightColor);
			int num1 = (int) ((double) npc.localAI[0] / 6.28318548202515);
            float f = (float) ((double) npc.localAI[0] % 6.28318548202515 - 3.14159274101257);
            float num2 = (float) Math.IEEERemainder((double) npc.localAI[1], 1.0);
            if ((double) num2 < 0.0)
              ++num2;
            int num3 = (int) Math.Floor((double) npc.localAI[1]);
            float num4 = 5f;
            float scale = (float) (1.0 + (double) num3 * 0.0199999995529652);
            if ((double) num1 == 1.0)
              num4 = 7f;
            Vector2 vector2 = f.ToRotationVector2() * num2 * num4 * npc.scale;
            Texture2D texture2D2 = mod.GetTexture("NPCs/Voidgazer/Voidgazer_Eye");
            Texture2D texture2D3 = mod.GetTexture("NPCs/Voidgazer/Voidgazer_Glow");
            Main.spriteBatch.Draw(texture2D2, position1 + vector2, new Microsoft.Xna.Framework.Rectangle?(), Color.White, npc.rotation, texture2D2.Size() / 2f, scale, SpriteEffects.None, 0.0f);
			
			int height = Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type];
			int y2 = height * npc.frame.Y;
			Vector2 position = (npc.position - (0.5f * npc.velocity) + new Vector2((float)npc.width, (float)npc.height) / 2f + Vector2.UnitY * npc.gfxOffY - Main.screenPosition).Floor();
			Main.spriteBatch.Draw(texture2D3, position, new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, y2, texture2D3.Width, height)), Color.White, npc.rotation, new Vector2((float)texture2D3.Width / 2f, (float)height / 2f), 1f, SpriteEffects.None, 0.0f);
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(5) == 1) {
				target.AddBuff(BuffID.Bleeding, 300);
			}
		}
	}
}
