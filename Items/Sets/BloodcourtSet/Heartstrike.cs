using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.BloodcourtSet
{
	public class Heartstrike : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heartstrike");
			Tooltip.SetDefault("Right click after 5 shots to launched a flayed arrow\nEnemies hit by flayed arrows will explode upon death, or 5 seconds later");
		}


		int counter = 0;
		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 24;
			Item.height = 46;
			Item.useTime = 31;
			Item.useAnimation = 31;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.Shuriken;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 1.5f;
			Item.value = 22500;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shootSpeed = 8f;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (player.altFunctionUse == 2) {
				type = ModContent.ProjectileType<FlayedShot>();
				if (counter > 0) {
					return false;
				}
				else {
					counter = 5;
				}

			}
			else {
				counter--;
			}
			if (counter == 0)
			{
				SoundEngine.PlaySound(SoundID.Item20);
				for (int i = 0; i < 7; i++)
				{
					int num = Dust.NewDust(player.position, player.width, player.height, DustID.Blood, 0f, -2f, 0, default, 2f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].scale *= .25f;
					if (Main.dust[num].position != player.Center)
						Main.dust[num].velocity = player.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
			return true;
		}
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2) {
				if (counter > 0) {
					return false;
				}
				else {
					return true;
				}

			}
			return true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, 0);
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<DreamstrideEssence>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}