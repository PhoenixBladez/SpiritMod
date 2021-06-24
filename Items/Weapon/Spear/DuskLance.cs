using SpiritMod.Items.Sets.DuskingDrops;
using SpiritMod.Projectiles.Held;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Spear
{
	public class DuskLance : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusk Lance");
			Tooltip.SetDefault("Occasionally shoots out an apparition that inflicts Shadowflame");
		}
		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.width = 24;
			item.height = 24;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.melee = true;
			item.autoReuse = true;
			item.noMelee = true;
			item.useAnimation = 28;
			item.useTime = 28;
			item.shootSpeed = 5.5f;
			item.knockBack = 6f;
			item.damage = 38;
			item.value = Item.sellPrice(0, 3, 60, 0);
			item.rare = ItemRarityID.LightRed;
			item.shoot = ModContent.ProjectileType<DuskLanceProj>();
		}
		public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[item.shoot] > 0)
				return false;
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DarkLance, 1);
			recipe.AddIngredient(ModContent.ItemType<DuskStone>(), 4);
			recipe.AddIngredient(ItemID.SoulofNight, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
