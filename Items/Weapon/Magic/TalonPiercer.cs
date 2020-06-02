using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class TalonPiercer : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Talon's Fury");
			Tooltip.SetDefault("Shoots feathers from off screen");
		}


        int charger;
        private Vector2 newVect;
        public override void SetDefaults()
        {
            item.damage = 25;
            item.magic = true;
            item.mana = 10;
            item.width = 46;
            item.height = 46;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 3.5f;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("BoneFeatherFriendly");
            item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			position -= new Vector2(speedX, speedY) * 100;
			speedX += (Main.rand.Next(-3,4) / 5f);
			speedY += (Main.rand.Next(-3,4) / 5f);
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Talon", 14);
            recipe.AddIngredient(null, "FossilFeather", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
