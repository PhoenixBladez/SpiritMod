using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
namespace SpiritMod.Items.Weapon.Gun
{
    public class OrionPistol : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pistol of Orion");
            Tooltip.SetDefault("Converts bullets into Orion Bullets\nOrion Bullets leave lingering stars in their wake");

        }


        public override void SetDefaults()
        {
            item.damage = 24;
            item.ranged = true;
            item.width = 24;
            item.height = 24;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 0;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 0, 42, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("OrionBullet");
            item.shootSpeed = 6f;
            item.useAmmo = AmmoID.Bullet;
        }
		 public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("OrionBullet"), 23, knockBack, player.whoAmI);
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FlintlockPistol, 1);
            recipe.AddIngredient(null, "SteamParts", 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}