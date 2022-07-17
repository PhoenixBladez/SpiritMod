using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class BloodriteDagger : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodrite Dagger");
			Tooltip.SetDefault("Use at nighttime to summon the Blood Moon");
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = Item.useAnimation = 20;
			Item.noMelee = true;
			Item.consumable = true;
			Item.autoReuse = false;
			Item.UseSound = SoundID.Item43;
		}

		public override bool CanUseItem(Player player)
		{
			if (Main.dayTime) {
				Main.NewText("The Blood Moon only emerges at night.", 150, 80, 80);
				return false;
			}
			if (Main.bloodMoon)
                return false;
			return true;
		}

		public override bool? UseItem(Player player)
		{
			Main.NewText("The Blood Moon is Rising...", 220, 0, 51);
			SoundEngine.PlaySound(SoundID.Roar, player.Center);
			if (!Main.dayTime)
				Main.bloodMoon = true;
			return null;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 5);
			recipe.AddIngredient(ModContent.ItemType<Items.Sets.BloodcourtSet.DreamstrideEssence>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
