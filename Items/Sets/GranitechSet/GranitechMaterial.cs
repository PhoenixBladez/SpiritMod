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

			item.value = 100;
			item.maxStack = 999;
			item.rare = ItemRarityID.Pink;
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			if (subID == 0)
				item.width = 24;
			else
				item.width = 28;

			if (subID == 2)
				item.height = 36;
			else
				item.height = 20;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D tex = mod.GetTexture("Items/Sets/GranitechSet/GranitechMaterialWorld" + subID);
			spriteBatch.Draw(tex, item.position - Main.screenPosition, null, lightColor, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}
