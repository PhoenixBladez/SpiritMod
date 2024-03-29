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

		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = 3000;
			item.rare = ItemRarityID.Blue;
			item.defense = 3;
		}

		public override void UpdateEquip(Player player)
		{
			player.minionDamage += 0.03f;
			player.maxMinions += 1;
		}

		public override void DrawHair(ref bool drawHair, ref bool drawAltHair) => drawHair = true;
	}
}
