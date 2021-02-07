
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
			item.width = item.height = 22;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 99;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTime = item.useAnimation = 30;

			item.buffType = BuffID.WellFed;
			item.buffTime = 56000;
			item.noMelee = true;
			item.consumable = true;
			item.UseSound = SoundID.Item2;
			item.autoReuse = false;

		}
        public override bool CanUseItem(Player player)
        {
            player.AddBuff(BuffID.Regeneration, 3600);
            return true;
        }
	}
}
