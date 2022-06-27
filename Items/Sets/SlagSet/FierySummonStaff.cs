using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SlagSet
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
			Item.damage = 38;
			Item.DamageType = DamageClass.Summon;
			Item.mana = 18;
			Item.width = 36;
			Item.height = 38;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 1.25f;
			Item.value = 10000;
			Item.UseSound = SoundID.Item73;
			Item.rare = ItemRarityID.Orange;
			Item.shoot = ModContent.ProjectileType<Projectiles.Summon.LavaRockSummon>();
            Item.buffType = ModContent.BuffType<Buffs.Summon.LavaRockSummonBuff>();
            Item.shootSpeed = 10f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			player.AddBuff(ModContent.BuffType<Buffs.Summon.LavaRockSummonBuff>(), 3600);
            return player.altFunctionUse != 2; 
		}

		public override bool CanUseItem(Player player)       
		{
			for (int i = 0; i < 1000; ++i) {
				if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot) {
					return false;
				}
			}
			return true;
		}
	}
}