using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Items.Weapon.Thrown {
public class PestilantShuriken : ModItem
{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pestilent Shuriken");
			Tooltip.SetDefault("Bounces three times, and can inflict Blighted Flames");
		}


	
    public override void SetDefaults()
    {
        item.damage = 34;
		item.consumable = true;
        item.thrown = true;
		item.noMelee = true;
		item.noUseGraphic = true;
        item.width = 22;
        item.height = 22;
        item.useTime = 15;
        item.useAnimation = 15;
        item.useStyle = 1;
		item.shootSpeed = 10f;
		item.shoot = mod.ProjectileType("PestilentShurikenProjectile");
        item.knockBack = 0;
		item.UseSound = SoundID.Item1;
		item.scale = 1f;
        item.value = Terraria.Item.sellPrice(0, 0, 2, 0);
        item.rare = 4;
        item.UseSound = SoundID.Item1;
        item.autoReuse = true;
		item.maxStack = 999;
    }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PutridPiece", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 150);
            recipe.AddRecipe();
        }
}}
