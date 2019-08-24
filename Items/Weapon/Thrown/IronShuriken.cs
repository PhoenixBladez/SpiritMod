using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class IronShuriken : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Iron Shuriken");
		}


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Shuriken);
            item.width = 32;
            item.height = 32;           
            item.shoot = mod.ProjectileType("IronShurikenProjectile");
            item.useAnimation = 16;
            item.useTime = 16;
            item.shootSpeed = 9f;
            item.damage = 10;
            item.knockBack = 0f;
			item.value = Terraria.Item.buyPrice(0, 0, 0, 30);
            item.crit = 5;
            item.rare = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
        public virtual void Kill(Terraria.Projectile projectile, int timeLeft)
        {
            projectile.CloneDefaults(ProjectileID.Shuriken);
        }
    }
}
