using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;
namespace SpiritMod.Items.Weapon.Summon.Artifact
{
	public class TerrorBark2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terror Bark");
			Tooltip.SetDefault("Takes up two minion slots\nSummons a Terror Fiend to shoot Wither bolts at foes\nWither bolts may inflict 'Blood Corruption'\nTerror Fiends may also shoot out more powerful, homing Wither Bolts\nOther summons recieve a buff to their damage and knockback, signified by a Nightmare Eye that appears above the player");
		}


		public override void SetDefaults()
		{
            item.width = 56;
            item.height = 66;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 4;
            item.mana = 11;
            item.damage = 27;
            item.knockBack = 3;
            item.useStyle = 1;
            item.useTime = 30;
            item.useAnimation = 30;
            item.summon = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("Terror2Summon");
            item.buffType = mod.BuffType("Terror2SummonBuff");
            item.buffTime = 3600;
            item.UseSound = SoundID.Item60;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Weapon");
            line.overrideColor = new Color(100, 0, 230);
            tooltips.Add(line);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(player.position.X, player.position.Y, speedX, speedY, mod.ProjectileType("NightmareEye"), 0, 0, player.whoAmI, 0f, 0f);
            return true;
        }

      /*  public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "TerrorBark1", 1);
            recipe.AddIngredient(null, "NecroticSkull", 1);
            recipe.AddIngredient(null, "TideStone", 1);
            recipe.AddIngredient(null, "StellarTech", 1);
            recipe.AddIngredient(null, "PrimordialMagic", 100);
            recipe.AddTile(null, "CreationAltarTile");
            recipe.SetResult(this, 1);
            recipe.AddRecipe();

        }*/
    }
}