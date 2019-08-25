using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	class DemonTail : ModItem
	{
		public static readonly int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Demon Tail");
			Tooltip.SetDefault("Summons an eldrich abomination to follow you \n~Donator Item~");
		}

		public override void SetDefaults()
		{
			item.UseSound = SoundID.Item2;
			item.useStyle = 4;
			item.useAnimation = 20;
			item.useTime = 20;

			item.width = 22;
			item.height = 32;

			item.value = Terraria.Item.sellPrice(0, 0, 54, 0);
			item.rare = 4;
			item.noMelee = true;

			item.buffType = LoomingPresence._type;
			item.shoot = Projectiles.DonatorItems.DemonicBlob._type;
		}

		public override bool CanUseItem(Player player)
		{
			player.AddBuff(item.buffType, 10);
			return true;
		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BlackLens);
			recipe.AddIngredient(ItemID.DemonScythe);
			recipe.AddIngredient(ItemID.WaterCandle);
			recipe.AddIngredient(ItemID.SoulofNight, 3);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
