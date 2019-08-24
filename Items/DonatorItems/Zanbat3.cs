using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.DonatorItems
{
    public class Zanbat3 : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zanbat Sword");
			Tooltip.SetDefault("~Donator Item~");
		}


        int charger;
        public override void SetDefaults()
        {
            item.damage = 61;
            item.useTime = 12;
            item.useAnimation = 12;
            item.melee = true;
            item.width = 48;
            item.height = 48;
            item.useStyle = 1;
            item.knockBack = 7;
            item.value = 25700;
            item.rare = 4;
            item.crit = 5;
            item.shootSpeed = 11f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.useTurn = true;
            item.shoot = mod.ProjectileType("ZanbatProj");
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (Main.rand.Next(4) == 1)
            {
                int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
                return false;
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Zanbat2", 1);
            recipe.AddIngredient(ItemID.TitaniumSword, 1);
            recipe.AddIngredient(ItemID.SoulofMight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();

            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(null, "Zanbat2", 1);
            recipe1.AddIngredient(ItemID.AdamantiteSword, 1);
            recipe1.AddIngredient(ItemID.SoulofMight, 10);
            recipe1.AddTile(TileID.MythrilAnvil);
            recipe1.SetResult(this, 1);
            recipe1.AddRecipe();

        }
    }
}