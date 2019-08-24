using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
namespace SpiritMod.Items.Weapon.Gun
{
    public class CoilPistol : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coil Pistol");
			Tooltip.SetDefault("Shoots out two weaker coiled bullets \n Occasionally burns foes");
		}


        public override void SetDefaults()
        {
            item.damage = 18;
            item.ranged = true;
            item.width = 24;
            item.height = 24;
            item.useTime = 13;
            item.useAnimation = 16;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 3;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 0, 22, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item11;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("CoilBullet1");
            item.shootSpeed = 16f;
            item.useAmmo = AmmoID.Bullet;
        }
		 public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = mod.ProjectileType("CoilBullet1");
            return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "TechDrive", 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}