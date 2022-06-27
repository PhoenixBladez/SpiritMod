
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Food
{
	public class CaesarSalad : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caesar Salad");
			Tooltip.SetDefault("Minor improvements to all stats\n'Maybe don't use a knife to eat this one'");
		}


		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 30;

			Item.buffType = BuffID.WellFed;
			Item.buffTime = 56000;
			Item.noMelee = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item2;
			Item.autoReuse = false;

		}
        public override bool CanUseItem(Player player)
        {
            player.AddBuff(BuffID.Regeneration, 3600);
            return true;
        }
	}
}
