using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using SpiritMod.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace SpiritMod.Items.Weapon.Bow.Artifact
{
    public class StarWeaver4 : ModItem
    {
        int charger;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Weaver");
			Tooltip.SetDefault("'You are the Artifact Keeper of the Weaver'\nConverts arrows into three Astral Bolts\nAstral Bolts may split into five damaging shards of energy\nCritical hits with Astral Bolts cause homing Astral Arrows to rain from the sky\nRight click to shoot out two explosive Burning Stars\n25% chance not to consume ammo");
		}

        public override void SetDefaults()
        {
            item.damage = 62;
            item.noMelee = true;
            item.ranged = true;
            item.width = 36;
            item.height = 82;
            item.useTime = 18;
            item.useAnimation = 18;
            item.useStyle = 5;
            item.shoot = 1;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 1.75f;
            item.value = Terraria.Item.sellPrice(0, 10, 0, 50);
            item.rare = 10;
            item.crit = 8;
            item.UseSound = SoundID.Item77;
            item.autoReuse = true;
            item.useTurn = false;
            item.shootSpeed = 10f;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Weapon");
            line.overrideColor = new Color(100, 0, 230);
            tooltips.Add(line);
            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
                }
            }
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
        public override bool ConsumeAmmo(Player player)
        {
            if (Main.rand.Next(25) == 0)
            {
                return false;
            }
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {

                charger++;
                if (charger >= 1)
                {
                    {
                        for (int I = 0; I < 2; I++)
                        {
                            Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-102, 102) / 100), speedY + ((float)Main.rand.Next(-102, 102) / 100), mod.ProjectileType("Stars4"), 175, knockBack, player.whoAmI, 0f, 0f);
                        };
                    }
                }
                return false;
            }
            else
            {
                charger = 0;
                for (int I = 0; I < 3; I++)
                {
                    Projectile.NewProjectile(position.X, position.Y,  speedX + ((float)Main.rand.Next(-102, 102) / 100), speedY + ((float)Main.rand.Next(-102, 102) / 100), mod.ProjectileType("StarPin1"), damage, knockBack, player.whoAmI, 0f, 0f);
                };
            }
            return false;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useTime = 40;
                item.useAnimation = 40;
                return true;
            }
            else
            {
                item.useTime = 18;
                item.useAnimation = 18;
                return true;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "StarWeaver3", 1);
            recipe.AddIngredient(null, "RadiantEmblem", 1);
            recipe.AddIngredient(null, "PlanteraBloom", 1);
            recipe.AddIngredient(null, "ApexFeather", 1);
            recipe.AddIngredient(null, "UnrefinedRuneStone", 1);
            recipe.AddIngredient(null, "Catalyst", 1);
            recipe.AddIngredient(null, "PrimordialMagic", 150);
            recipe.AddTile(null, "CreationAltarTile");
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}