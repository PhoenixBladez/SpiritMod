using Microsoft.Xna.Framework;
using SpiritMod.Buffs;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.InfernonDrops
{
	public class InfernalSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Char Fury");
			Tooltip.SetDefault("Shoots out two blazes that cause foes to combust, multiple hits causing the combustion to deal more damage");
		}


		public override void SetDefaults()
		{
			Item.width = 52;
			Item.height = 64;
			Item.rare = ItemRarityID.Pink;
			Item.damage = 43;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = Item.useAnimation = 25;
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ModContent.ProjectileType<CombustionBlaze>();
			Item.shootSpeed = 3f;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(2) == 0)
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Torch);
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(2) == 0)
				target.AddBuff(ModContent.BuffType<StackingFireBuff>(), 300);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			for (int I = 0; I < 2; I++) {
				Projectile.NewProjectile(source, position.X, position.Y, velocity.X * (Main.rand.Next(500, 900) / 100), velocity.Y * (Main.rand.Next(500, 900) / 100), ModContent.ProjectileType<CombustionBlaze>(), Item.damage / 6 * 5, Item.knockBack, player.whoAmI);
			}
			return false;
		}
	}
}
