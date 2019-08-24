using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class BlizzardScepter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blizzard Scepter");
			Tooltip.SetDefault("Rains down icicles");
		}


		public override void SetDefaults()
		{
			item.damage = 51;
			item.magic = true;
			item.mana = 8;
			item.width = 40;
			item.height = 40;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = 5;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
            item.rare = 6;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("StarfallProjectile");
			item.shootSpeed = 14f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			for (int i = 0; i < 5; ++i)
			{
			    if(Main.myPlayer == player.whoAmI)
                {
				    Vector2 mouse = Main.MouseWorld;
				    Projectile.NewProjectile(mouse.X + Main.rand.Next(-80, 80), player.Center.Y - 470 + Main.rand.Next(-50, 50), 0, Main.rand.Next(15,25), mod.ProjectileType("BlizzardSpike"), damage, knockBack, player.whoAmI);
			    }
			}
			return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "IcyEssence", 14);
            recipe.AddTile(null, "EssenceDistorter");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}