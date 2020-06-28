using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Summon.Artifact
{
	public class TerrorBark1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terror Bark");
			Tooltip.SetDefault("'A shard of undying nightmares'\nTakes up two minion slots\nSummons a Terror Fiend to shoot Wither bolts at foes\nWither bolts may inflict 'Blood Corruption'");
		}


		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 28;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Green;
			item.mana = 11;
			item.damage = 17;
			item.knockBack = 3;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("Terror1Summon");
			item.buffType = mod.BuffType("Terror1SummonBuff");
			item.buffTime = 3600;
			item.UseSound = SoundID.Item60;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Weapon");
			line.overrideColor = new Color(100, 0, 230);
			tooltips.Add(line);
		}
		/* public override void AddRecipes()
         {
             ModRecipe recipe = new ModRecipe(mod);
             recipe.AddIngredient(ModContent.ItemType<OddKeystone>(), 1);
             recipe.AddIngredient(ModContent.ItemType<RootPod>(), 1);
             recipe.AddIngredient(ModContent.ItemType<GildedIdol>(), 1);
             recipe.AddIngredient(ModContent.ItemType<DemonLens>(), 1);
             recipe.AddIngredient(ModContent.ItemType<PrimordialMagic>(), 50);
             recipe.AddTile(ModContent.TileType<CreationAltarTile>());
             recipe.SetResult(this, 1);
             recipe.AddRecipe();

         }
         */
	}
}