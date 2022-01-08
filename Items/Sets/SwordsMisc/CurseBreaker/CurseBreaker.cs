using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;
using System;
using SpiritMod.Utilities;
using Terraria;
using Terraria.Enums;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;

namespace SpiritMod.Items.Sets.SwordsMisc.CurseBreaker
{
	public class CurseBreaker : ModItem
	{
		public bool ChargeReady => charge % 3 == 2;

		private int charge;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curse Breaker");
			Tooltip.SetDefault("Every third swing curses nearby enemies \n Strike again to break the curse, dealing extra damage");
		}

		public override void SetDefaults()
		{
			item.damage = 100;
			item.melee = true;
			item.width = 36;
			item.height = 44;
			item.useTime = 12;
			item.useAnimation = 12;
			item.reuseDelay = 20;
			item.channel = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 10f;
			item.value = Item.sellPrice(0, 1, 80, 0);
			item.crit = 4;
			item.rare = ItemRarityID.Pink;
			item.shootSpeed = 1f;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<CurseBreakerProj>();
			item.noUseGraphic = true;
			item.noMelee = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 direction = new Vector2(speedX, speedY);
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 1).WithPitchVariance(0.5f), player.Center);
			Projectile proj = Projectile.NewProjectileDirect(position + (direction * 20) + (direction.RotatedBy(-1.57f * player.direction) * 20), Vector2.Zero, type, damage, knockBack, player.whoAmI);
			var mp = proj.modProjectile as CurseBreakerProj;
			mp.Phase = charge % 3;
			charge++;
			return false;
		}
	}

	internal class CurseBreakerProj : ModProjectile
	{
		public const float SwingRadians = MathHelper.Pi * 0.75f; //Total radians of the sword's arc

		public int Phase;

		public Player Player => Main.player[projectile.owner];

		public bool Empowered => Phase == 2;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duelist's Legacy");
			Main.projFrames[projectile.type] = 1;//11, 11, 9, 19
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.Size = new Vector2(150, 250);
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 16;
			projectile.ownerHitCheck = true;
		}
	}
}