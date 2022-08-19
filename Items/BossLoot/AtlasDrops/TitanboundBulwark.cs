using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.AtlasDrops
{
	[AutoloadEquip(EquipType.Shield)]
	public class TitanboundBulwark : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titanbound Bulwark");
			Tooltip.SetDefault("As your health decreases, your mana regeneration increases\nReduces damage taken by 10%\nIncreases life regeneration");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 51, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.accessory = true;
			Item.defense = 2;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			float manaBoost = (float)(player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2 * 80f;
			player.manaRegen += (int)manaBoost;
			player.endurance += .1f;
			player.lifeRegen += 3;
		}
	}
}
