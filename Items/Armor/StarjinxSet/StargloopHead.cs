using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Effects.Stargoop;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;
using System;

namespace SpiritMod.Items.Armor.StarjinxSet
{
	[AutoloadEquip(EquipType.Head)]
    public class StargloopHead : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stargloop Head");
			Tooltip.SetDefault("'Head beyond the clouds'");
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}

		public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.value = Item.sellPrice(gold : 2);
            Item.rare = ItemRarityID.Pink;
			Item.vanity = true;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair) => drawHair = drawAltHair = false;
		public override bool DrawHead() => false;
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor) => color = Color.White * 0f;

		public override void EquipFrameEffects(Player player, EquipType type)
		{
			float dir = Main.rand.NextFloat(-2f, -1.14f);

			float midMultiplier = 1.4f - Math.Abs(dir + 1.57f) * 1.5f;

			Vector2 velocity = dir.ToRotationVector2() * Main.rand.NextFloat(1f, 3f) * midMultiplier;

			Vector2 center = player.MountedCenter;

			Dust dust = Dust.NewDustPerfect(center - player.velocity + new Vector2(0, -10), ModContent.DustType<FriendlyStargoopDust>(), velocity + player.velocity * 0.5f, Scale: Main.rand.NextFloat(1.4f, 1.8f));
		}

		private float DrawTimer => (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2.5f) / 2) + 0.5f;

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Color color = Color.Purple;
			float opacity = MathHelper.Lerp(1f, 0.8f, DrawTimer) * ((255f - Item.alpha) / 255f);

			Vector2 itemCenter = new Vector2(Item.position.X - Main.screenPosition.X + Item.width / 2, Item.position.Y - Main.screenPosition.Y + Item.height - (TextureAssets.Item[Item.type].Value.Height / 2) + 2f);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

			Texture2D bloom = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient").Value;
			float bloomScale = 0.375f;
			spriteBatch.Draw(bloom, itemCenter, null, color * opacity, 0, bloom.Size() / 2, bloomScale, SpriteEffects.None, 0); //draw the bloom

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

			return true;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D glowmask = ModContent.Request<Texture2D>(Texture + "_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			float opacity = MathHelper.Lerp(0.7f, 1f, DrawTimer) * ((255f - Item.alpha) / 255f);

			Vector2 itemCenter = new Vector2(Item.position.X - Main.screenPosition.X + Item.width / 2, Item.position.Y - Main.screenPosition.Y + Item.height - (TextureAssets.Item[Item.type].Value.Height / 2) + 2f);
			int numtodraw = 6;
			for (int i = 0; i < numtodraw; i++) //draw pulsing glowmask effect
			{
				Vector2 offset = Vector2.UnitX.RotatedBy(MathHelper.TwoPi * (i / (float)numtodraw)) * DrawTimer * 6;
				float pulseOpacity = 1 - DrawTimer;
				pulseOpacity *= opacity;
				spriteBatch.Draw(glowmask, itemCenter + offset, null, Color.White * pulseOpacity, rotation, glowmask.Size() / 2, scale, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(glowmask, itemCenter, null, Color.White * opacity, rotation, glowmask.Size() / 2, scale, SpriteEffects.None, 0f); //draw the glowmask
		}
	}
}
