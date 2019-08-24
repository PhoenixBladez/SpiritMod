using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.Items.Weapon.Magic
{
    public class BirbStaffMage : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gandharva's Fury");
			Tooltip.SetDefault("'The fury of the ancients reaides within'\nCalls down a Celestial Flare from the sky");
		}


        public override void SetDefaults()
        {
            item.damage = 74;
            item.magic = true;
            item.mana = 12;
            item.width = 44;
            item.height = 48;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 0.75f;
            item.value = 32000;
            item.rare = 8;
            item.UseSound = SoundID.Item92;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("GodFlare");
            item.shootSpeed = 15f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
            int amount = 1;
            for (int i = 0; i < amount; ++i)
            {
                Vector2 pos = new Vector2(mouse.X + player.width * 0.5f + Main.rand.Next(-100, 101), mouse.Y - 600f);
                pos.X = (pos.X * 10f + mouse.X) / 11f + (float)Main.rand.Next(-70, 71);
                pos.Y -= 150;
                float spX = mouse.X + player.width * 0.5f + Main.rand.Next(-200, 201) - mouse.X;
                float spY = mouse.Y - pos.Y;
                if (spY < 0f)
                    spY *= -1f;
                if (spY < 20f)
                    spY = 20f;

                float length = (float)Math.Sqrt((double)(spX * spX + spY * spY));
                length = 12 / length;
                spX *= length;
                spY *= length;
                spX = spX + (float)Main.rand.Next(-40, 41) * 0.02f;
                spY = spY + (float)Main.rand.Next(-40, 41) * 0.06f;
                spX *= (float)Main.rand.Next(75, 150) * 0.006f;
                pos.X += (float)Main.rand.Next(-20, 21);
                Projectile.NewProjectile(pos.X, pos.Y, spX, spY, type, damage, 2, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "WorshipCrystal", 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}