using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using SpiritMod.Prim;
using SpiritMod.Particles;

namespace SpiritMod.Mechanics.BoonSystem.AsclepiusBoon
{
	public class AsclepiusBoonOrb : ModNPC
	{
		const int STARTTIME = 35;

		int counter;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("");
			NPCID.Sets.TrailCacheLength[npc.type] = 5;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SetDefaults()
		{
			npc.width = 26;
			npc.height = 26;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 1;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.knockBackResist = 0f;
		}
		public override void AI()
		{
			if (Main.rand.Next(8) == 1)
			{
				StarParticle particle = new StarParticle(
				npc.Center + Main.rand.NextVector2Circular(10, 10),
				npc.velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * 0.1f,
				new Color(48, 195, 43),
				Main.rand.NextFloat(0.08f, 0.23f),
				Main.rand.Next(20, 40));

				ParticleHandler.SpawnParticle(particle);
			}

			if (counter == 0)
				npc.velocity = npc.ai[1].ToRotationVector2() * 10;
			counter++;
			if (counter < STARTTIME )
			if (counter < STARTTIME)
			{
				npc.velocity.Y *= 0.96f;
				npc.velocity.X *= 1.02f;
				npc.velocity = npc.velocity.RotatedBy(0.04f);
				return;
			}

			var target = Main.npc.Where(n => n.active && Vector2.Distance(n.Center, npc.Center) < 600 && n.whoAmI != (int)npc.ai[0] && n.life < n.lifeMax && npc.type != n.type).OrderBy(n => Vector2.Distance(n.Center, npc.Center)).FirstOrDefault();
			if (target != default)
			{
				float rotDifference = ((((npc.DirectionTo(target.Center).ToRotation() - npc.velocity.ToRotation()) % 6.28f) + 9.42f) % 6.28f) - 3.14f;

				float lerper = Math.Min(0.1f, (counter - STARTTIME) / 700f);
				npc.velocity = npc.velocity.RotatedBy(rotDifference * lerper);
				float velLength = npc.velocity.Length();
				npc.velocity.Normalize();
				npc.velocity *= MathHelper.Lerp(velLength, 10, lerper);
				npc.velocity = Vector2.Lerp(npc.velocity, npc.DirectionTo(target.Center) * 10, lerper / 5f);
				npc.velocity += npc.DirectionTo(target.Center) * lerper;
				if (Vector2.Distance(target.Center, npc.Center) < 25)
				{
					npc.active = false;
					target.life += 40;
					target.HealEffect(40, true);
				}
			}
			else
			{
				npc.velocity.X *= 1.02f;
				npc.velocity = npc.velocity.RotatedBy(0.04f);
				if (counter > STARTTIME + 50)
				{
					npc.scale -= 0.07f;
					if (npc.scale < 0.07f)
						npc.active = false;
				}
			}

		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 15; i++)
			{
				Vector2 direction = Main.rand.NextVector2Circular(20, 20);
				StarParticle particle = new StarParticle(
				npc.Center + direction,
				direction * 0.15f,
				new Color(48, 195, 43),
				Main.rand.NextFloat(0.08f, 0.23f),
				Main.rand.Next(20, 40));

				ParticleHandler.SpawnParticle(particle);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.npcTexture[npc.type];

			Vector2 origin = new Vector2(tex.Width / 2, tex.Height / 2);

			spriteBatch.Draw(tex, npc.Center - Main.screenPosition, null, Color.White * 0.8f, npc.rotation, origin, npc.scale, SpriteEffects.None, 0f);
			for (int k = npc.oldPos.Length - 1; k >= 0; k--)
			{
				float mult = (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length));
				Vector2 drawPos = npc.oldPos[k] + (new Vector2(npc.width, npc.height) / 2);
				Color color = Color.White * mult;
				float num108 = 4;
				float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
				float num106 = 0f;
				Color color29 = new Color(212 - npc.alpha, 255 - npc.alpha, 137 - npc.alpha, 0).MultiplyRGBA(color);
				for (int num103 = 0; num103 < 4; num103++)
				{
					Color color28 = color29;
					color28 = npc.GetAlpha(color28);
					color28 *= 1.5f - num107;
					color28 *= (float)Math.Pow((((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2), 1.5f);
					Vector2 vector29 = drawPos + ((float)num103 / (float)num108 * 6.28318548f + npc.rotation + num106).ToRotationVector2() * (1.5f * num107 + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)num103;
					spriteBatch.Draw(tex, vector29, null, color28 * .6f, npc.rotation, origin, npc.scale * (float)Math.Sqrt(mult), SpriteEffects.None, 0f);
				}
			}

			return false;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;

	}	
}
