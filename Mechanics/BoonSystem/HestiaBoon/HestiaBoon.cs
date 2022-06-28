using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Prim;
using Terraria.ID;

namespace SpiritMod.Mechanics.BoonSystem.HestiaBoon
{
	public class HestiaBoon : Boon
	{
		public override bool CanApply => true;
		public override string TexturePath => "SpiritMod/Mechanics/BoonSystem/HestiaBoon/HestiaBoon";
		public override Vector2 SigilSize => new Vector2(26, 34);

		public override void SetStats() => npc.lifeMax = npc.life = (int)(npc.lifeMax * 1.5f);

		public override void AI()
		{
			Lighting.AddLight(npc.Center, Color.Orange.ToVector3() * 0.3f);

			int ticker = 0;

			foreach(NPC newNPC in Main.npc) 
			{ 
				if (ticker >= 3) break;

				if (newNPC != null && newNPC.active)
				{
					newNPC.GetGlobalNPC<HestiaGlobalNPC>().drawHestiaRunes = false;

					if (Vector2.DistanceSquared(SigilPosition, newNPC.Center) <= 256 * 256 && newNPC.whoAmI != npc.whoAmI)
					{
						myNPCs[ticker] = newNPC;

						myNPCs[ticker].GetGlobalNPC<HestiaGlobalNPC>().drawHestiaRunes = true;

						ticker++;
					}
				}
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			DrawBeam(new Color(247, 195, 92, 0), new Color(247, 117, 42, 0));

			DrawBloom(spriteBatch, new Color(247, 195, 92) * 0.5f, 0.5f);

			for(int i = 0; i < 3; i++) 
			{ 
				if (myNPCs[i] != null && myNPCs[i].active && Vector2.DistanceSquared(SigilPosition, myNPCs[i].Center) <= 256 * 256 && (npc.ModNPC is IBoonable || npc.type == NPCID.Medusa))
				{
					DrawBoonBeam(myNPCs[i].Center);
				} 
				else
				{
					myNPCs[i] = null;
				}
			}

			DrawSigil(spriteBatch);
		}

		public override void OnDeath()
		{
			foreach (NPC myNPC in myNPCs)
			{
				if (myNPC == null) continue;

				var gnpc = myNPC.GetGlobalNPC<HestiaGlobalNPC>();
				if (gnpc == null) continue;

				gnpc.drawHestiaRunes = false;
			}

			DropOlympium(Main.rand.Next(3, 6));
		}

		public NPC[] myNPCs = new NPC[3];

		public void DrawBoonBeam(Vector2 targetPos)
		{
			Effect effect = SpiritMod.Instance.GetEffect("Effects/EmpowermentBeam");

			effect.Parameters["uTexture"].SetValue(SpiritMod.Instance.GetTexture("Textures/Trails/Trail_2"));
			effect.Parameters["progress"].SetValue(Main.GlobalTimeWrappedHourly / 3);
			effect.Parameters["uColor"].SetValue(new Color(247, 117, 42, 0).ToVector4());
			effect.Parameters["uSecondaryColor"].SetValue(new Color(247, 195, 92, 0).ToVector4());

			Vector2 dist = SigilPosition - targetPos;

			TrianglePrimitive tri = new TrianglePrimitive()
			{
				TipPosition = SigilPosition - Main.screenPosition,
				Rotation = (SigilPosition - targetPos).ToRotation() + MathHelper.Pi,
				Height = 100 + dist.Length() * 1.5f,
				Color = Color.White * 0.33f,
				Width = 100 + ((npc.width + npc.height) * 0.5f)
			};

			PrimitiveRenderer.DrawPrimitiveShape(tri, effect);
		}
	}

	public class HestiaGlobalNPC : GlobalNPC
	{
		public bool drawHestiaRunes;
		public float runeAlpha;

		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (drawHestiaRunes)
			{
				if (npc != null && npc.active)
				{
					for (int j = 0; j < 3; j++)
					{
						HestiaRune rune = runes[j];

						if (rune.inFront == true)
						{
							Texture2D tex2 = ModContent.Request<Texture2D>("SpiritMod/Mechanics/BoonSystem/HestiaBoon/HestiaRune");

							Color color = new Color(247, 117, 42, 0);

							Texture2D glow = Terraria.GameContent.TextureAssets.Extra[49].Value;
							color.A = 0;

							float glowScale = 1 + ((float)Math.Sin(Main.GameUpdateCount) / 4);

							spriteBatch.Draw(glow, npc.Center + new Vector2(rune.x, rune.y) - Main.screenPosition, null,
								color * glowScale, 0, glow.Size() / 2, npc.scale * 0.33f, SpriteEffects.None, 0f);

							Main.spriteBatch.Draw(tex2, npc.Center + new Vector2(rune.x, rune.y) - Main.screenPosition, new Rectangle(0, 22 * j, 22, 22), Color.White * runeAlpha, 0f, new Vector2(11, 11), 1f, SpriteEffects.None, 0f);
						}

						//runes[j] = rune;
					}
				}
			}
		}

		public int baseDamage;
		public int baseDefense;

		public bool setStats = false;

		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if(!setStats)
			{
				baseDamage = npc.damage;
				baseDefense = npc.defense;

				setStats = true;
			}

			if (drawHestiaRunes) runeAlpha += 0.05f;
			else runeAlpha -= 0.05f;

			runeAlpha = MathHelper.Clamp(runeAlpha, 0f, 1f);

			if (npc != null && npc.active && runeAlpha > 0f && (npc.ModNPC is IBoonable || npc.type == NPCID.Medusa))
			{
				npc.damage = (int)(baseDamage * 1.2f);
				npc.defense = baseDefense + 15;


				Vector2 pos = npc.Center;
				for (int i = 0; i < 4; i++)
				{
					Vector2 offsetPositon = Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * i) * 2;
					Main.spriteBatch.Draw(TextureAssets.Npc[npc.type].Value, pos + offsetPositon - Main.screenPosition, npc.frame, new Color(247, 117, 42, 0) * runeAlpha, npc.rotation, npc.frame.Size() * 0.5f, npc.scale, npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
				}

				for (int j = 0; j < 3; j++)
				{
					HestiaRune rune = runes[j];

					rune.i = j;

					rune.Update();

					if (rune.inFront == false)
					{
						Texture2D tex = ModContent.Request<Texture2D>("SpiritMod/Mechanics/BoonSystem/HestiaBoon/HestiaRune");

						Color color = new Color(247, 117, 42, 0);

						Texture2D glow = Terraria.GameContent.TextureAssets.Extra[49].Value;
						color.A = 0;

						float glowScale = 1 + ((float)Math.Sin(Main.GameUpdateCount) / 4);

						spriteBatch.Draw(glow, npc.Center + new Vector2(rune.x, rune.y) - Main.screenPosition, null,
							color * glowScale, 0, glow.Size() / 2, npc.scale * 0.33f, SpriteEffects.None, 0f);

						Main.spriteBatch.Draw(tex, npc.Center + new Vector2(rune.x, rune.y) - Main.screenPosition, new Rectangle(0, 22 * j, 22, 22), Color.White * runeAlpha, 0f, new Vector2(11, 11), 1f, SpriteEffects.None, 0f);
					}

					runes[j] = rune;
				}
			}
			else
			{
				npc.damage = baseDamage;
				npc.defense = baseDefense;
			}

			return true;
		}

		public override bool InstancePerEntity => true;

		public HestiaRune[] runes = new HestiaRune[3];

		public struct HestiaRune
		{
			public float x;
			public float y;

			public int i;

			public bool inFront;

			HestiaRune(float _x, float _y, int _i, bool _inFront)
			{
				x = _x;
				y = _y;
				i = _i;
				inFront = _inFront;
			}

			public void Update()
			{
				float ticks = (Main.GameUpdateCount + (80 * i)) % 240;

				if (ticks >= 0 && ticks < 120) inFront = true;
				else inFront = false;

				y = 8f * (float)Math.Sin((ticks / 240f) * MathHelper.TwoPi) + 4f * (float)Math.Sin((ticks / 120f) * MathHelper.TwoPi);
				x = -40 * (float)Math.Cos((ticks / 240f) * MathHelper.TwoPi);
			}
		}
	}
}