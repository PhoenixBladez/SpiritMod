using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon
{
	public class ShellHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shell Hammer");
			Tooltip.SetDefault("Lobs shells duuuude!");
		}


        int shellCooldown;
		public override void SetDefaults()
		{
            item.width = 60;
            item.height = 60;
            item.value = Item.sellPrice(0, 12, 20, 0);
            item.rare = 8;
            item.damage = 67;
            item.knockBack = 9;
            item.useStyle = 1;
            item.useTime = 35;
			item.useAnimation = 35;
            item.melee = true;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("ShellHammerProjectile");
            item.shootSpeed = 4;
            item.UseSound = SoundID.Item1;
            this.shellCooldown = 240;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (shellCooldown > 0 || Main.tile[Player.tileTargetX, Player.tileTargetY].active() || Main.tile[Player.tileTargetX, Player.tileTargetY].wall > 0)
                return false;
            return true;
        }
        public override void UpdateInventory(Player player)
        {
            shellCooldown--;
        }
        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TurtleShell);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 18);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}