
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.Wings)]
	public class SkarmoryWings : ModItem
	{
		public int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bladewings");
			Tooltip.SetDefault("'It seems to originate from a metallic organism of unknown origin'\nGrants flight and slow fall\nLeaves behind a trail of quicksilver that homes in on foes");

			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new Terraria.DataStructures.WingStats(90, 9.4f, 1);
		}

		public override void SetDefaults()
		{
			Item.width = 47;
			Item.height = 37;
			Item.value = 60000;
			Item.accessory = true;
			Item.rare = ItemRarityID.Yellow;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.wingTimeMax = 137;
			if (Main.rand.NextBool(4)) {

				Dust.NewDust(player.position, player.width, player.height, DustID.SilverCoin);
			}
			timer++;
			if (timer == 200) {
				int proj2 = Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center.X, player.Center.Y, 0, 0, ModContent.ProjectileType<QuicksilverBolt>(), 40, 0, Main.myPlayer);
				timer = 0;
			}

		}
		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.65f;
			ascentWhenRising = 0.075f;
			maxCanAscendMultiplier = 1.1f;
			maxAscentMultiplier = 2.3f;
			constantAscend = 0.095f;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ItemID.AdamantiteBar, 5);
			modRecipe.AddIngredient(ItemID.Ectoplasm, 5);
			modRecipe.AddIngredient(ItemID.SoulofFlight, 20);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.Register();

			Recipe modRecipe1 = CreateRecipe(1);
			modRecipe1.AddIngredient(ItemID.TitaniumBar, 5);
			modRecipe1.AddIngredient(ItemID.Ectoplasm, 5);
			modRecipe1.AddIngredient(ItemID.SoulofFlight, 20);
			modRecipe1.AddTile(TileID.MythrilAnvil);
			modRecipe1.Register();
		}
	}
}