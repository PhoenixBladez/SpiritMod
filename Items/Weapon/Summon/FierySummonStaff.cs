using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
	public class FierySummonStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slagtern Staff");
			Tooltip.SetDefault("Summons a hovering slag lantern that lobs lava at nearby foes");

		}


		public override void SetDefaults()
		{
			item.damage = 19;
			item.summon = true;
			item.mana = 18;
			item.width = 36;
			item.height = 38;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 1.25f;
			item.value = 10000;
			item.UseSound = SoundID.Item73;
			item.rare = ItemRarityID.Orange;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<Projectiles.Summon.LavaRockSummon>();
            item.buffType = ModContent.BuffType<Buffs.Summon.LavaRockSummonBuff>();
            item.shootSpeed = 10f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<OvergrowthStaff>(), 1);
			recipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 14);
			recipe.AddIngredient(ItemID.Bone, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.AddBuff(ModContent.BuffType<Buffs.Summon.LavaRockSummonBuff>(), 3600);
            return player.altFunctionUse != 2; 
		}
	}
}