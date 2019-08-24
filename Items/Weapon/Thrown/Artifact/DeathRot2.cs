using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Weapon.Thrown.Artifact
{
	public class DeathRot2 : ModItem
    {
        int charger;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Death Rot");
            Tooltip.SetDefault("Hit enemies are afflicted by 'Pestilence,' which spreads to nearby enemies\nEvery fifth throw of the weapon leaves behind multiple clouds of Plague Miasma\nAttacks may release a swarm of Rot Seekers that explode into violent fumes");

        }


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 44;
            item.height = 44;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item100;
            item.thrown = true;
            item.crit = 3;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("DeathRot2Proj");
            item.useAnimation = 21;
            item.consumable = true;
            item.useTime = 21;
            item.shootSpeed = 11f;
            item.damage = 33;
            item.knockBack = 2.3f;
            item.value = Item.sellPrice(0, 6, 0, 50);
            item.rare = 4;
            item.autoReuse = true;
            item.maxStack = 1;
            item.consumable = false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Weapon");
            line.overrideColor = new Color(100, 0, 230);
            tooltips.Add(line);
        }
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            charger++;
            if (charger >= 5)
            {
                for (int I = 0; I < 2; I++)
                {
                    Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), mod.ProjectileType("Miasma"), damage, knockBack, player.whoAmI, 0f, 0f);
                }
                charger = 0;
            }
            if (Main.rand.Next(4) == 1)
            {
                for (int I = 0; I < 3; I++)
                {
                    Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-130, 130) / 100), speedY + ((float)Main.rand.Next(-130, 130) / 100), mod.ProjectileType("RotSeeker"), 22, knockBack, player.whoAmI, 0f, 0f);

                }
            }
                return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DeathRot1", 1);
            recipe.AddIngredient(null, "NecroticSkull", 1);
            recipe.AddIngredient(null, "TideStone", 1);
            recipe.AddIngredient(null, "StellarTech", 1);
            recipe.AddIngredient(null, "PrimordialMagic", 75);
            recipe.AddTile(null, "CreationAltarTile");
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
