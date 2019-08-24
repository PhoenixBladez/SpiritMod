using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	public class ShadowPulse : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Punnapha's Essence");
			Tooltip.SetDefault("Converts arrows into shadowflame arrows, as well as a bouncing pulse\nShoots out five arrows at once \n~Donator Item~ 'You feel like the Daughter of Chthulhu...'");
		}


		public override void SetDefaults()
		{
            item.damage = 74;
            item.noMelee = true;
            item.ranged = true;
            item.width = 40;
            item.height = 50;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useStyle = 5;
            item.shoot = 495;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 4;
            item.value = 521000;
            item.rare = 11;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shootSpeed = 6.7f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, 357, damage, knockBack, player.whoAmI);
            Projectile newProj = Main.projectile[proj];
            newProj.friendly = true;
            newProj.hostile = false;

            for (int I = 0; I < 5; I++)
            {
                Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-180, 180) / 100), speedY + ((float)Main.rand.Next(-180, 180) / 100), 495, damage, knockBack, player.whoAmI, 0f, 0f);
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ShadowFlameBow, 1);
            recipe.AddIngredient(ItemID.Tsunami, 1);
            recipe.AddIngredient(ItemID.PulseBow, 1);
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}