using SpiritMod.Items.Sets.StarplateDrops;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class BlueMoonSpawn : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Turquoise Lens");
			Tooltip.SetDefault("Use at nighttime to summon the Mystic Moon");
		}

		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.rare = ItemRarityID.Pink;
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
				Main.NewText("The moon isn't powerful in daylight.", 80, 80, 150);
				return false;
			}
			if (MyWorld.BlueMoon)
                return false;
			return true;
		}

		public override bool? UseItem(Player player)
		{
			Main.NewText("The Mystic Moon is Rising...", 0, 90, 220);
			SoundEngine.PlaySound(SoundID.Roar, player.Center);
			if (!Main.dayTime)
				MyWorld.BlueMoon = true;
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrystalShard, 6);
			recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Tiles.AsteroidBlock>(), 30);
			recipe.AddIngredient(ItemID.SoulofLight, 10);
			recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
