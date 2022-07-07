using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.ByBiome.Spirit.Consumables
{
	public class LuminBerry : ModItem
	{
		public override void SetStaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\n'They feel almost glassy...'");

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
		}

		public override bool CanUseItem(Player player)
		{
			player.AddBuff(BuffID.WellFed, 5 * 60 * 60);
			return true;
		}
	}
}
