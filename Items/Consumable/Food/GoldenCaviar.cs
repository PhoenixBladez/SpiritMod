
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
			Tooltip.SetDefault("Major improvements to all stats\nEmits an aura of light\n'It has an exquisite glow'");
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 99;
			Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useTime = Item.useAnimation = 30;
            Item.value = Item.sellPrice(0, 0, 0, 50);
			Item.buffType = BuffID.WellFed3;
			Item.buffTime = 180000;
			Item.noMelee = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item2;
			Item.autoReuse = false;
		}

        public override bool CanUseItem(Player player)
        {
            player.AddBuff(BuffID.Shine, 7200);
            return true;
        }
    }
}
