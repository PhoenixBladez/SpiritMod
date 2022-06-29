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
		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starjinx Cluster");
			Tooltip.SetDefault("'Forged with the power of a billion stars!'");
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 999;
			Item.value = Item.sellPrice(silver : 10);
			Item.rare = ItemRarityID.Pink;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D glow = ModContent.Request<Texture2D>(Texture + "_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Texture2D outline = ModContent.Request<Texture2D>(Texture + "_outline", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			float Timer = (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3) / 2 + 0.5f;
			void DrawTex(Texture2D tex, float opacity, Vector2? offset = null) => spriteBatch.Draw(tex, Item.Center + (offset ?? Vector2.Zero) - Main.screenPosition, null, Color.White * opacity, rotation, tex.Size() / 2, scale, SpriteEffects.None, 0);

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
				Particles.ParticleHandler.SpawnParticle(new Particles.StarParticle(Item.Center + Main.rand.NextVector2Circular(7, 7), Main.rand.NextVector2Circular(1, 1), Color.White * 0.66f, SpiritMod.StarjinxColor(Main.GlobalTimeWrappedHourly), Main.rand.NextFloat(0.2f, 0.3f), 25));
		}
	}
}
