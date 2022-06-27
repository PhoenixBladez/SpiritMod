using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using SpiritMod.Particles;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Sets.DonatorVanity
{
	[AutoloadEquip(EquipType.Head)]
	public class LightNovasVanity : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("LightNovas's Mask");
			Tooltip.SetDefault("'Great for impersonating patrons!'");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;

			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.vanity = true;
		}
	}
}
