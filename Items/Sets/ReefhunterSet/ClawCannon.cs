using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.ReefhunterSet.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet
{
	public class ClawCannon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Claw Cannon");
			Tooltip.SetDefault("'Clannon...clawnon...clawanon...'");
		}

		public override void SetDefaults()
		{
			Item.damage = 18;
			Item.width = 38;
			Item.height = 26;
			Item.useTime = Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 0, 5, 0);
			Item.rare = ItemRarityID.Blue;
			Item.crit = 6;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.shootSpeed = 15f;
			Item.UseSound = SoundID.Item20;
			Item.shoot = ModContent.ProjectileType<Cannonbubble>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			SoundEngine.PlaySound(SoundID.Item85, position);

			for (int i = 0; i < 5; ++i)
				Dust.NewDust(position, 0, 0, DustID.BubbleBurst_Blue, velocity.X * Main.rand.NextFloat(0.15f, 0.25f), velocity.Y * Main.rand.NextFloat(0.15f, 0.25f), 0, default, Main.rand.NextFloat(0.5f, 1f));

			for(int i = 0; i < 5; ++i)
				Dust.NewDust(position, 0, 0, ModContent.DustType<Dusts.BubbleDust>(), velocity.X * Main.rand.NextFloat(1.5f, 2.25f), velocity.Y * Main.rand.NextFloat(1.5f, 2.25f), 0, default, Main.rand.NextFloat(1.5f, 2f));
			return true;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<IridescentScale>(), 14);
			recipe.AddIngredient(ModContent.ItemType<SulfurDeposit>(), 5);
			recipe.AddIngredient(ItemID.IronBar, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
