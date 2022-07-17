
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Food
{
	public class PopRocks : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pop Rocks");
			Tooltip.SetDefault("Minor improvements to all stats\nEmits an aura of light\n'It zips around your mouth!'");
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
			Item.buffTime = 18000;
			Item.noMelee = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item2;
			Item.autoReuse = false;
		}

		public override bool CanUseItem(Player player)
		{
			player.AddBuff(BuffID.Shine, 7200);
			return base.CanUseItem(player);
		}
	}
}
