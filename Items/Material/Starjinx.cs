using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
	public class Starjinx : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starjinx Cluster");
			Tooltip.SetDefault("'Forged with the power of a billion stars!'");
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.maxStack = 999;
			item.value = Item.sellPrice(silver : 10);
			item.rare = ItemRarityID.Pink;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D glow = ModContent.GetTexture(Texture + "_Glow");
			Texture2D outline = ModContent.GetTexture(Texture + "_outline");
			float Timer = (float)Math.Sin(Main.GlobalTime * 3) / 2 + 0.5f;
			void DrawTex(Texture2D tex, float opacity, Vector2? offset = null) => spriteBatch.Draw(tex, item.Center + (offset ?? Vector2.Zero) - Main.screenPosition, null, 
				Color.White * opacity, rotation, tex.Size() / 2, scale, SpriteEffects.None, 0);

			for (int i = 0; i < 6; i++)
			{
				Vector2 drawPos = Vector2.UnitX.RotatedBy((i / 6f) * MathHelper.TwoPi) * Timer * 6;
				DrawTex(glow, 1 - Timer, drawPos);
				DrawTex(outline, 1 - Timer, drawPos + (Vector2.UnitY * 2));
			}
			DrawTex(glow, (Timer / 2) + 0.5f);
			DrawTex(outline, (Timer / 2) + 0.5f, Vector2.UnitY * 2);
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			if (Main.rand.Next(50) == 0)
				Particles.ParticleHandler.SpawnParticle(new Particles.StarParticle(item.Center + Main.rand.NextVector2Circular(7, 7), Main.rand.NextVector2Circular(1, 1), Color.White * 0.66f, SpiritMod.StarjinxColor(Main.GlobalTime), Main.rand.NextFloat(0.2f, 0.3f), 25));
		}
	}
}
