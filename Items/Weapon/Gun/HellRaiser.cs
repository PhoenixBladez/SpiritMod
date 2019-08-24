using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
    public class HellRaiser : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hell Raiser");
			Tooltip.SetDefault("Shoots out a high velocity, fiery bullet");
		}


        public override void SetDefaults()
        {
            item.damage = 70;
            item.ranged = true;
            item.width = 58;
            item.height = 32;
            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 8;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 12, 0, 0);
            item.rare = 6;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("HellBullet");
            item.shootSpeed = 19f;
            item.useAmmo = AmmoID.Bullet;
            item.crit = 18;
        }
		 public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = mod.ProjectileType("HellBullet");
            return true;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FieryEssence", 14);
            recipe.AddTile(null, "EssenceDistorter");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
}