using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			item.width = 14;
			item.height = 18;
			item.maxStack = 999;
			item.value = Item.sellPrice(silver : 10);
			item.rare = ItemRarityID.Pink;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, item, Main.itemTexture[item.type], rotation, scale);
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			if (Main.rand.Next(20) == 0)
				Gore.NewGore(item.Center, Main.rand.NextVector2Circular(3, 3), mod.GetGoreSlot("Gores/StarjinxGore"), 0.5f);
		}
	}
}
