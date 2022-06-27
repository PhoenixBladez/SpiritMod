using SpiritMod.Items.Sets.StarplateDrops;
using Terraria;
using Terraria.Audio;
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
				Main.NewText("The Blood Moon only emerges at night.", 150, 80, 80, true);
				return false;
			}
			if (Main.bloodMoon)
                return false;
			return true;
		}

		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			Main.NewText("The Blood Moon is Rising...", 220, 0, 51, true);
			SoundEngine.PlaySound(SoundID.Roar, (int)player.position.X, (int)player.position.Y, 0);
			if (!Main.dayTime)
				Main.bloodMoon = true;
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBar, 5);
			recipe.anyIronBar = true;
			recipe.AddIngredient(ModContent.ItemType<Items.Sets.BloodcourtSet.DreamstrideEssence>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
