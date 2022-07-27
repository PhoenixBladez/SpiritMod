using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Items.ByBiome.Briar.Consumables
{
	public class Durian : ModItem
	{
		public override void SetStaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\n'What an awful smell!'");

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.value = Item.sellPrice(0, 0, 0, 50);
			Item.noUseGraphic = false;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 20;
			Item.noMelee = true;
			Item.consumable = true;
			Item.autoReuse = false;
			Item.UseSound = SoundID.Item2;
			Item.buffTime = 5 * 60 * 60;
			Item.buffType = BuffID.WellFed;
		}

		public override bool? UseItem(Player player)
		{
			player.AddBuff(BuffID.Stinky, 5 * 60 * 60);
			return true;
		}
	}
}
