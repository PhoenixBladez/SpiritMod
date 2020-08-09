
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
			item.width = item.height = 22;
			item.rare = 3;
			item.maxStack = 99;
			item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.useTime = item.useAnimation = 30;
            item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
			item.buffType = BuffID.WellFed;
			item.buffTime = 180000;
			item.noMelee = true;
			item.consumable = true;
			item.UseSound = SoundID.Item2;
			item.autoReuse = false;

		}
        public override bool CanUseItem(Player player)
        {
            player.AddBuff(11, 7200);
            return true;
        }
    }
}
