using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.AtlasDrops
{
	public class AtlasEye : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Atlas Eye");
			Tooltip.SetDefault("Under 50% health, defense is increased by 20, but movement speed is reduced by 1/3\nReduces damage taken by 12%");
		}



		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 28;
			Item.rare = ItemRarityID.Lime;
			Item.expert = true;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.defense = 2;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.statLife < player.statLifeMax2 / 2) {
				player.moveSpeed *= 0.66f;
				player.statDefense += 20;
			}
			player.endurance += 0.12f;
		}
	}
}
