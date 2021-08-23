using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Armor.OperativeSet
{
	[AutoloadEquip(EquipType.Body)]
	public class OperativeBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Operative's Jacket");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/OperativeSet/OperativeBody_Glow");

		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.value = Terraria.Item.buyPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.LightRed;
			item.vanity = true;
		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			glowMaskColor = new Color (200, 200, 200, 100);
		}
	}
}
