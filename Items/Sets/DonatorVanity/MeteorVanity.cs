using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using SpiritMod.Particles;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Sets.DonatorVanity
{
	[AutoloadEquip(EquipType.Head)]
	public class MeteorVanity : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor's Hat");
			Tooltip.SetDefault("'Great for impersonating patrons!'");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;

			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.vanity = true;
		}
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair) => drawAltHair = true;

	}
}
