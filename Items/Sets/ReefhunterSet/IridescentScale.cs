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

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Iridescent Shard");
			Tooltip.SetDefault("'Glints beautifully under the water'");
		}

		public override void SetDefaults()
		{
			subID = Main.rand.Next(3);

			Item.value = 100;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.Blue;
			Item.width = 28;
			Item.height = 28;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D tex = ModContent.Request<Texture2D>(Texture + "_World");
			spriteBatch.Draw(tex, Item.position - Main.screenPosition, new Rectangle(0, 28 * subID, 28, 26), GetAlpha(lightColor) ?? lightColor, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}
