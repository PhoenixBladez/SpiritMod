using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.SlagSet;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.SwordsMisc.EternalSwordTree
{
	public class DemonfireSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flameberge Sword");
			Tooltip.SetDefault("Shoots out a fiery bolt");

		}

		public override void SetDefaults()
		{
			Item.damage = 49;
			Item.useTime = 19;
			Item.useAnimation = 19;
			Item.DamageType = DamageClass.Melee;
			Item.width = 60;
			Item.height = 64;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Lime;
			Item.shootSpeed = 6;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.shoot = ModContent.ProjectileType<FlambergeProjectile>();
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(4)) {
				target.AddBuff(BuffID.OnFire, 180);
			}
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(5) == 0)
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Torch);
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ChlorophyteBar, 5);
            recipe.AddIngredient(ModContent.ItemType<CarvedRock>(), 10);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}