using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.ToolsMisc.OreWarhammersSet
{
	public class ShellHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shell Hammer");
			Tooltip.SetDefault("'Lobs shells duuuude!'");
		}


		int shellCooldown;
		public override void SetDefaults()
		{
			Item.width = 60;
			Item.height = 60;
			Item.value = Item.sellPrice(0, 12, 20, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.damage = 67;
			Item.knockBack = 9;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.DamageType = DamageClass.Melee;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<ShellHammerProjectile>();
			Item.shootSpeed = 7;
			Item.UseSound = SoundID.Item1;
			this.shellCooldown = 240;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (shellCooldown > 0 || Main.tile[Player.tileTargetX, Player.tileTargetY].HasTile || Main.tile[Player.tileTargetX, Player.tileTargetY].WallType > 0)
				return false;
			return true;
		}
		public override void UpdateInventory(Player player)
		{
			shellCooldown--;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.TurtleShell);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 18);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}