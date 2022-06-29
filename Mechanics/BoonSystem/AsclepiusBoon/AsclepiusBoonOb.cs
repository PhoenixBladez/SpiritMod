using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using SpiritMod.Prim;
using SpiritMod.Particles;

namespace SpiritMod.Mechanics.BoonSystem.AsclepiusBoon
{
	public class AsclepiusBoonOrb : ModNPC
	{
		const int STARTTIME = 35;
		public const float HOME_DISTANCE = 600f;

		int counter;

		bool canHome = true;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("");
			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}
		public override void SetDefaults()
		{
			NPC.width = 26;
			NPC.height = 26;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 1;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.knockBackResist = 0f;
		}
		public override void AI()
		{
			NPC.rotation = NPC.velocity.ToRotation();
			/*if (Main.rand.Next(15) == 1)
			{
				StarParticle particle = new StarParticle(
				npc.Center + Main.rand.NextVector2Circular(10, 10),
				npc.velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * 0.1f,
				new Color(49, 212, 76),
				Main.rand.NextFloat(0.08f, 0.13f),
				Main.rand.Next(20, 40));

				ParticleHandler.SpawnParticle(particle);
			}*/

			if (counter == 0)
				NPC.velocity = NPC.ai[1].ToRotationVector2() * 10;
			counter++;
			if (counter < STARTTIME )
			if (counter < STARTTIME)
			{
				NPC.velocity.Y *= 0.96f;
				NPC.velocity.X *= 1.02f;
				NPC.velocity = NPC.velocity.RotatedBy(0.04f);
				return;
			}

			var target = Main.npc.Where(n => n.active && Vector2.Distance(n.Center, NPC.Center) < HOME_DISTANCE && n.whoAmI != (int)NPC.ai[0] && n.life < n.lifeMax && NPC.type != n.type).OrderBy(n => Vector2.Distance(n.Center, NPC.Center)).FirstOrDefault();
			if (target != default && canHome)
			{
				float rotDifference = ((((NPC.DirectionTo(target.Center).ToRotation() - NPC.velocity.ToRotation()) % 6.28f) + 9.42f) % 6.28f) - 3.14f;

				float lerper = (counter - STARTTIME) / 700f;
				NPC.velocity = NPC.velocity.RotatedBy(rotDifference * lerper);
				NPC.velocity = MathHelper.Lerp(NPC.velocity.Length(), 10, lerper) * Vector2.Lerp(Vector2.Normalize(NPC.velocity), NPC.DirectionTo(target.Center) * 10, lerper / 5f);

				NPC.velocity += NPC.DirectionTo(target.Center) * lerper;
				if (Vector2.Distance(target.Center, NPC.Center) < 25)
				{
					NPC.active = false;
					int heal = Math.Min(40, target.lifeMax - target.life);
					SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 29).WithPitchVariance(0.2f).WithVolume(.65f), NPC.Center);

					target.life += heal;
					target.HealEffect(heal, true);
				}
			}
			else
			{
				NPC.velocity.X *= 1.02f;
				NPC.velocity = NPC.velocity.RotatedBy(0.04f);
				if (counter > STARTTIME + 50)
				{
					canHome = false;
					NPC.scale -= 0.07f;
					if (NPC.scale < 0.07f)
						NPC.active = false;
				}
			}

		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.dedServ)
				return;

			SoundEngine.PlaySound(SoundID.Item27 with { PitchVariance = 0.3f, Volume = 0.6f }, NPC.Center); ;
			for (int i = 0; i < 15; i++)
			{
				Vector2 direction = Main.rand.NextVector2Circular(20, 20);
				StarParticle particle = new StarParticle(
				NPC.Center + direction,
				direction * 0.15f,
				new Color(49, 212, 76),
				Main.rand.NextFloat(0.08f, 0.23f),
				Main.rand.Next(20, 40));
				SoundEngine.PlaySound(SoundID.NPCHit3, NPC.Center);

				ParticleHandler.SpawnParticle(particle);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D tex = TextureAssets.Npc[NPC.type].Value;

			Vector2 origin = new Vector2(tex.Width / 2, tex.Height / 2);

			for (int k = NPC.oldPos.Length - 1; k >= 0; k--)
			{
				float mult = (float)(((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length));
				Vector2 drawPos = NPC.oldPos[k] + (new Vector2(NPC.width, NPC.height) / 2);
				Color color = Color.White * mult;
				float num108 = 4;
				float num107 = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
				float num106 = 0f;
				Color color29 = new Color(212 - NPC.alpha, 255 - NPC.alpha, 137 - NPC.alpha, 0).MultiplyRGBA(color);
				for (int num103 = 0; num103 < 4; num103++)
				{
					Color color28 = color29;
					color28 = NPC.GetAlpha(color28);
					color28 *= 1.5f - num107;
					color28 *= (float)Math.Pow((((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2), 1.5f);
					Vector2 vector29 = drawPos + ((float)num103 / (float)num108 * 6.28318548f + NPC.rotation + num106).ToRotationVector2() * (1.5f * num107 + 4f) - Main.screenPosition + new Vector2(0, NPC.gfxOffY) - NPC.velocity * (float)num103;
					spriteBatch.Draw(tex, vector29, null, color28 * .6f, NPC.rotation, origin, NPC.scale * (float)Math.Sqrt(mult), SpriteEffects.None, 0f);
				}
			}

			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			Effect effect = Mod.GetEffect("Effects/RotateSprite");
			effect.Parameters["rotation"].SetValue(-NPC.rotation);
			effect.CurrentTechnique.Passes[0].Apply();


			float xscale = MathHelper.Clamp(1, 1 + (NPC.velocity.Length() / 40f), 1.5f);
			float yscale = MathHelper.Clamp(1, 1 - (NPC.velocity.Length() / 40f), 0.66f);
			spriteBatch.Draw(tex, NPC.Center - Main.screenPosition, null, Color.White * 0.8f, NPC.rotation, origin, NPC.scale * new Vector2(xscale, yscale), SpriteEffects.None, 0f);

			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			return false;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;

	}	
}
