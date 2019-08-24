using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;
namespace SpiritMod.Items.Weapon.Summon.Artifact
{
	public class TerrorBark3 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terror Bark");
			Tooltip.SetDefault("Takes up two minion slots\nSummons a Terror Fiend to charge at enemies\nTerror Fiends shoot homing Wither bolts at foes\nWither bolts may inflict 'Blood Corruption'\nAddition summons recieve a buff to their damage and knockback, signified by a Nightmare Eye that appears above the player\nDirect hits on enemies with the Terror Fiend may cause enemies to erupt into Nightmare Explosions");
		}


		public override void SetDefaults()
		{
            item.width = 62;
            item.height = 68;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 7;
            item.mana = 11;
            item.damage = 40;
            item.knockBack = 2;
            item.useStyle = 1;
            item.useTime = 30;
            item.useAnimation = 30;
            item.summon = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("Terror3Summon");
            item.buffType = mod.BuffType("Terror3SummonBuff");
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
            Projectile.NewProjectile(player.position.X, player.position.Y, speedX, speedY, mod.ProjectileType("NightmareEye1"), 0, 0, player.whoAmI, 0f, 0f);
            return true;
        }

       /* public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "TerrorBark2", 1);
            recipe.AddIngredient(null, "SearingBand", 1);
            recipe.AddIngredient(null, "CursedMedallion", 1);
            recipe.AddIngredient(null, "DarkCrest", 1);
            recipe.AddIngredient(null, "BatteryCore", 1);
            recipe.AddIngredient(null, "PrimordialMagic", 100);
            recipe.AddTile(null, "CreationAltarTile");
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }*/
    }
}