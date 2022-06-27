
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Food
{
	public class GoldenCaviar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golden Caviar");
			Tooltip.SetDefault("Minor improvements to all stats\n'It has an exquisite glow'");
		}


		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 99;
			Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useTime = Item.useAnimation = 30;
            Item.value = Terraria.Item.sellPrice(0, 0, 0, 50);
			Item.buffType = BuffID.WellFed;
			Item.buffTime = 180000;
			Item.noMelee = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item2;
			Item.autoReuse = false;

		}
        public override bool CanUseItem(Player player)
        {
            player.AddBuff(11, 7200);
            return true;
        }
    }
}
