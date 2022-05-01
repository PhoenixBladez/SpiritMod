using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet
{
	public class IridescentScale : ModItem
	{
		private int subID = 0; //Controls the in-world sprite for this item

		public override void SetStaticDefaults() => Tooltip.SetDefault("Glints beautifully under the water");

		public override void SetDefaults()
		{
			subID = Main.rand.Next(3);

			item.value = 100;
			item.maxStack = 999;
			item.rare = ItemRarityID.Pink;
			item.width = 28;
			item.height = 28;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D tex = ModContent.GetTexture(Texture + "_World");
			spriteBatch.Draw(tex, item.position - Main.screenPosition, new Rectangle(0, 28 * subID, 28, 26), GetAlpha(lightColor) ?? lightColor, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}
