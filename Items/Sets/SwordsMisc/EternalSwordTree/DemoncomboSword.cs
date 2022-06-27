using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.SpiritSet;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.SwordsMisc.EternalSwordTree
{
	public class DemoncomboSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eternal Sword");
			Tooltip.SetDefault("Shoots out apparitions of blades");

		}


		public override void SetDefaults()
		{
			Item.damage = 84;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.DamageType = DamageClass.Melee;
			Item.width = 60;
			Item.height = 64;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 9;
			Item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.shootSpeed = 12;
			Item.UseSound = SoundID.Item70;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.shoot = ModContent.ProjectileType<DemonProj>();
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0)
				target.AddBuff(BuffID.CursedInferno, 180);
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(5) == 0)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.ShadowbeamStaff);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
			}
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DemoniceSword>(), 1);
			recipe.AddIngredient(ModContent.ItemType<DemonfireSword>(), 1);
			recipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}