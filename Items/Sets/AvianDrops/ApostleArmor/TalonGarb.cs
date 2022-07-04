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

			ArmorIDs.Body.Sets.HidesArms[Type] = false;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 22;
			Item.value = 10000;
			Item.rare = ItemRarityID.Orange;
			Item.defense = 5;
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = true;
			// The equipSlot is added in ExampleMod.cs --> Load hook
			equipSlot = EquipLoader.GetEquipSlot(Mod, "TalonGarb_Legs", EquipType.Legs);
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Magic) += .07f;
			player.GetDamage(DamageClass.Ranged) += .07f;
			player.moveSpeed += 0.10f;
		}
	}
}
