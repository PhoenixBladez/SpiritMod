using Microsoft.Xna.Framework;
using SpiritMod.Items.Armor;
using SpiritMod.Items.Sets.BloodcourtSet;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SummonsMisc.TwoFaceMask
{
	public class MimeSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Two-Faced Mask");
			Tooltip.SetDefault("Summons either a Soul of Happiness or Sadness at the cursor position with a left or right click\nThe Soul of Happiness shoots out beams at foes\nThe Soul of Sadness shoots out homing tears at foes");
		}

		public override void SetDefaults()
		{
			Item.damage = 17;
			Item.DamageType = DamageClass.Summon;
			Item.mana = 10;
			Item.width = 44;
			Item.height = 48;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 5;
			Item.value = 20000;
			Item.rare = ItemRarityID.Orange;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<HappySoul>();
			Item.shootSpeed = 0f;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (player.altFunctionUse == 2)
				type = ModContent.ProjectileType<SadSoul>();
			else
				type = ModContent.ProjectileType<HappySoul>();
			position = Main.MouseWorld;
			Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
			player.UpdateMaxTurrets();
			return false;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ModContent.ItemType<MimeMask>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<DreamstrideEssence>(), 8);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.Register();
		}
	}
}