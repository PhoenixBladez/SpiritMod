
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Food
{
	public class GhostPepper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghost Pepper");
			Tooltip.SetDefault("Minor improvements to all stats\n'Will you take the risk?'");
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
			Item.UseSound = SoundID.Item3;
			Item.autoReuse = false;
		}

		public override bool CanUseItem(Player player)
		{
			player.AddBuff(BuffID.Inferno, 5400);
            player.AddBuff(BuffID.OnFire, 420);
            return true;
		}
	}
}
