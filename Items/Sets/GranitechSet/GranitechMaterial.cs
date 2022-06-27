using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GranitechSet
{
	public class GranitechMaterial : ModItem
	{
		private int subID = 0; //Controls the in-world sprite for this item

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("G-Tek Components");
			Tooltip.SetDefault("'An impressive combination of magic and science'");
		}

		public override void SetDefaults()
		{
			subID = Main.rand.Next(3);

			Item.value = 100;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.Pink;
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			if (subID == 0)
				Item.width = 24;
			else
				Item.width = 28;

			if (subID == 2)
				Item.height = 36;
			else
				Item.height = 20;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D tex = Mod.GetTexture("Items/Sets/GranitechSet/GranitechMaterialWorld" + subID);
			spriteBatch.Draw(tex, Item.position - Main.screenPosition, null, lightColor, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}
