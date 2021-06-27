using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.SpiritSet;
using SpiritMod.Items.Sets.SlagSet;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Summon
{
	public class CoreController : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Core Controller");
			Tooltip.SetDefault("Summons a Molten Tank to fight for you!\nCounts as a Sentry");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 28;
			item.value = Item.buyPrice(gold: 10, silver: 50);
			item.rare = ItemRarityID.Yellow;
			item.crit = 4;
			item.mana = 7;
			item.damage = 55;
			item.knockBack = 1;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<TankMinion>();
			item.UseSound = SoundID.Item44;
		}

		public override bool CanUseItem(Player player)
		{
			player.FindSentryRestingSpot(item.shoot, out int worldX, out int worldY, out _);
			worldX /= 16;
			worldY /= 16;
			worldY--;
			return !WorldGen.SolidTile(worldX, worldY);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.FindSentryRestingSpot(type, out int worldX, out int worldY, out int pushYUp);
			Projectile.NewProjectile(worldX, worldY - pushYUp, speedX, speedY, type, damage, knockBack, player.whoAmI);
			player.UpdateMaxTurrets();
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 12);
			recipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}