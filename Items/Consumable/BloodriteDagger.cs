using SpiritMod.Items.Sets.StarplateDrops;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class BloodriteDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodrite Dagger");
			Tooltip.SetDefault("Use at nighttime to summon the Blood Moon");
		}

		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.rare = ItemRarityID.Blue;

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.HoldingUp;
			item.useTime = item.useAnimation = 20;

			item.noMelee = true;
			item.consumable = true;
			item.autoReuse = false;

			item.UseSound = SoundID.Item43;
		}

		public override bool CanUseItem(Player player)
		{
			if (Main.dayTime) {
				Main.NewText("The Blood Moon only emerges at night.", 150, 80, 80, true);
				return false;
			}
			if (Main.bloodMoon)
                return false;
			return true;
		}

		public override bool UseItem(Player player)
		{
			Main.NewText("The Blood Moon is Rising...", 220, 0, 51, true);
			Main.PlaySound(SoundID.Roar, (int)player.position.X, (int)player.position.Y, 0);
			if (!Main.dayTime)
				Main.bloodMoon = true;
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 5);
			recipe.anyIronBar = true;
			recipe.AddIngredient(ModContent.ItemType<Items.Sets.BloodcourtSet.DreamstrideEssence>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
