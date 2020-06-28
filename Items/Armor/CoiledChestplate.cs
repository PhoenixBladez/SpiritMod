using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class CoiledChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Autonaut's Chestplate");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/CoiledChest_Glow");

		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.value = Terraria.Item.sellPrice(0, 0, 18, 0);
			item.rare = ItemRarityID.Green;
			item.vanity = true;
		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			glowMaskColor = Color.White;
		}
	}
}
