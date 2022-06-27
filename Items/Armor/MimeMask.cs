using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class MimeMask : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mime Mask");
			Tooltip.SetDefault("Increases summon damage by 3%\nIncreases your max number of minions");

			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = 3000;
			Item.rare = ItemRarityID.Blue;
			Item.defense = 3;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Summon) += 0.03f;
			player.maxMinions += 1;
		}
	}
}
