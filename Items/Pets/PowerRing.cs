using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Pets
{
	public class PowerRing : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ring of Willpower");
			Tooltip.SetDefault("Summons a Lantern Power Battery to light the way");
		}

		public override void SetDefaults()
		{

			item.damage = 0;
			item.useStyle = 1;
			item.shoot = mod.ProjectileType("Lantern");
			item.width = 16;
			item.height = 30;
			item.useAnimation = 20;
			item.useTime = 20;
			item.rare = 3;
			item.noMelee = true;
			item.value = Item.sellPrice(0, 3, 50, 0);
			item.buffType = mod.BuffType("LanternBuff");
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}

		public override bool CanUseItem(Player player)
		{
			return player.miscEquips[1].IsAir;
		}

		public override void AddRecipes()
		{

			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ItemID.MeteoriteBar, 10);
			modRecipe.AddIngredient(ItemID.FallenStar, 3);
			modRecipe.AddIngredient(ItemID.Emerald, 1);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}