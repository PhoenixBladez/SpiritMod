using SpiritMod.Items.Material;
using SpiritMod.Tide;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class BlackPearl : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seabreak Pearl");
			Tooltip.SetDefault("'Coveted by ancient horrors...'\nSummons The Tide\nCan only be used near the ocean");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.rare = ItemRarityID.Orange;
			item.maxStack = 99;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.useTime = 30;
			item.useAnimation = 30;
			item.reuseDelay = 10;
			item.noMelee = true;
			item.consumable = true;
			item.autoReuse = false;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.UseSound = SoundID.Item43;
		}

		public override bool CanUseItem(Player player)
		{
			if ((player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && (!Main.pumpkinMoon && !Main.snowMoon))
				return false;
			if (!player.ZoneBeach || TideWorld.TheTide) {
				Main.NewText("The Tide only ebbs by the calm of the sea.", 85, 172, 247);
				return false;
			}
			return true;
		}

		public override bool UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				TideWorld.TheTide = true;
				TideWorld.TideWaveIncrease();
				TideWorld.SendPacket(mod);
			}

			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Coral, 5);
			recipe.AddIngredient(ItemID.Bone, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
