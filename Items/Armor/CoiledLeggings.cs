using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class CoiledLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Autonaut's Leggings");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Armor/CoiledLegs_Glow");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 18;
			item.value = Terraria.Item.sellPrice(0, 0, 25, 0);
			item.rare = ItemRarityID.Green;
			item.vanity = true;
		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			glowMaskColor = Color.White;
		}
	}
}
