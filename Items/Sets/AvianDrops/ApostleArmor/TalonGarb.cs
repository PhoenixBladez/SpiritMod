using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.AvianDrops.ApostleArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class TalonGarb : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Apostle's Garb");
			Tooltip.SetDefault("Increases magic and ranged damage by 7%\nIncreases movement speed by 10%");

		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 22;
			item.value = 10000;
			item.rare = ItemRarityID.Orange;
			item.defense = 5;
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = true;
			// The equipSlot is added in ExampleMod.cs --> Load hook
			equipSlot = mod.GetEquipSlot("TalonGarb_Legs", EquipType.Legs);
		}

		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
		}
		public override void UpdateEquip(Player player)
		{
			player.magicDamage += .07f;
			player.rangedDamage += .07f;
			player.moveSpeed += 0.10f;
		}
	}
}
