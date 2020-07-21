using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CapacitorSet
{
	[AutoloadEquip(EquipType.Body)]
	public class CapacitorBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Capacitor's Robes");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/CapacitorSet/CapacitorBody_Glow");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = ItemRarityID.Green;

			item.vanity = true;
		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			glowMaskColor = Color.White;
		}
	}
}
