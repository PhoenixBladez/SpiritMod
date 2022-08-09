using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Arrow;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.BloodcourtSet
{
	public class Headsplitter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Headsplitter");
			Tooltip.SetDefault("Right click to release an explosion of vengeance\nUsing it too frequently will reduce its damage\nInflicts 'Surging Anguish'");
		}


		public override void SetDefaults()
		{
			Item.damage = 21;
			Item.DamageType = DamageClass.Melee;
			Item.width = 34;
			Item.height = 40;
			Item.useTime = 26;
			Item.useAnimation = 26;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<FlayedExplosion>();
			Item.shootSpeed = 12f;
			Item.autoReuse = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<DreamstrideEssence>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool(4)) {
				target.AddBuff(ModContent.BuffType<SurgingAnguish>(), 180);
			}
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (modPlayer.shootDelay < 150 && player.altFunctionUse == 2) {
				SoundEngine.PlaySound(SoundID.Item20, player.Center);
				damage = 1 + (int)((damage * 1.35f) / (MathHelper.Clamp((float)Math.Sqrt(modPlayer.shootDelay), 1, 180)));
				Projectile.NewProjectile(source, position.X, position.Y, 0, 0, ModContent.ProjectileType<FlayedExplosion>(), damage, 2.5f, Main.myPlayer);
				modPlayer.shootDelay = 180;
			}
			return false;
		}
		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			float cosRot = (float)Math.Cos(player.itemRotation - 0.78f * player.direction * player.gravDir);
			float sinRot = (float)Math.Sin(player.itemRotation - 0.78f * player.direction * player.gravDir);
			for (int i = 0; i < 1; i++) {
				float length = (Item.width * 1.2f - i * Item.width / 9) * Item.scale + 16; //length to base + arm displacement
				int dust = Dust.NewDust(new Vector2((float)(player.itemLocation.X + length * cosRot * player.direction), (float)(player.itemLocation.Y + length * sinRot * player.direction)), 0, 0, DustID.Blood, player.velocity.X * 0.9f, player.velocity.Y * 0.9f, 100, Color.Transparent, 1.8f);
				Main.dust[dust].velocity *= 0f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}