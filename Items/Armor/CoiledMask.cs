using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class CoiledMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Autonaut's Headgear");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Armor/CoiledMask_Glow");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 15, 0);
			Item.vanity = true;
			Item.rare = ItemRarityID.Green;
		}
		public override void DrawArmorColor(Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
		{
			glowMaskColor = Color.White;
		}
	}
}
