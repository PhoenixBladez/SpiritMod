using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
namespace SpiritMod.Items.Weapon.Gun
{
    public class TidalGun : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Blaster");
			Tooltip.SetDefault("Converts bullets into fluctuating Tidal Energy \n Occasionally inflicts Tidal Ebb, reducing enemy damage and life");
		}


        public override void SetDefaults()
        {
            item.damage = 22;
            item.ranged = true;
            item.width = 24;
            item.height = 24;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 3;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 0, 32, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("TidalShard");
            item.shootSpeed = 8f;
            item.useAmmo = AmmoID.Bullet;
        }
		 public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = mod.ProjectileType("TidalShard");
            return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Coral, 6);
            recipe.AddIngredient(null, "PearlFragment", 10);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}