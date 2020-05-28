using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class FangTome : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tome of the Thousand Fangs");
			Tooltip.SetDefault("Summon a set of gnashing teeth\nInflicts 'Surging Anguish'");
		}

		static int offsetLength = 23;
		public override void SetDefaults()
		{
			item.damage = 25;
			item.magic = true;
			item.mana = 12;
			item.width = 38;
			item.height = 38;
			item.useTime = 35;
			item.useAnimation = 35;
			item.useStyle = 5;
			Item.staff[item.type] = true; 
			item.noMelee = true;
            item.knockBack = 13;
			item.rare = 2;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.shoot = mod.ProjectileType("TeethTop");
			item.value = Item.sellPrice(0, 1, 0, 0);
            item.shootSpeed = 8f;
            item.crit = 6;
        }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Vector2 value18 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
            position = value18;
			Projectile.NewProjectile(position.X, (position.Y - offsetLength) - 12, 0, 0, mod.ProjectileType("TeethTop"), damage, knockBack, Main.myPlayer, offsetLength);
			Projectile.NewProjectile(position.X + 4, (position.Y + offsetLength) + 12, 0, 0, mod.ProjectileType("TeethBottom"), damage, knockBack, Main.myPlayer, offsetLength);
			return false;
		}
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BloodFire", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}