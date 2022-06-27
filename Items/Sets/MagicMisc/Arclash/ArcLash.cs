using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Prim;
using System;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.MagicMisc.Arclash
{
	public class ArcLash : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arc Lash");
			Tooltip.SetDefault("Does more damage towards the end of its cycle");
		}
		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 5;
			Item.width = 44;
			Item.height = 46;
			Item.channel = true;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(0, 1, 10, 0);
			Item.UseSound = SoundID.Item15;
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<ArcLashProj>();
			Item.shootSpeed = 8;
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
        {
			Vector2 direction = velocity;
			position+= direction * 7;
			Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, player.whoAmI);
			return false;
        }

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] == 0;
	}
	public class ArcLashProj : ModProjectile
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Arc Lash");

		private readonly int cycletime = 40;
		private readonly float lengthmult = 50;
		private ref float Progress => ref Projectile.ai[0];

		private ref float Angle => ref Projectile.ai[1];

		Player Owner => Main.player[Projectile.owner];

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.aiStyle = -1;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = cycletime;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
			if (!Owner.channel)
				Projectile.Kill();

			if (Main.LocalPlayer == Owner) {
				Angle = Owner.AngleTo(Main.MouseWorld);
				Projectile.netUpdate = true;
			}

			Progress = lengthmult * (cycletime - Projectile.timeLeft)/cycletime;
			Owner.ChangeDir(Math.Sign(Projectile.Center.X - Owner.MountedCenter.X));
			Owner.itemRotation = MathHelper.WrapAngle(Angle - Owner.fullRotation - ((Owner.direction < 0) ? MathHelper.Pi : 0));
			Owner.itemTime = 2;
			Owner.itemAnimation = 2;

			if(Progress == cycletime/2) {
				Owner.CheckMana(Owner.HeldItem, Owner.HeldItem.mana, true);

				if (Owner.HeldItem.UseSound.HasValue)
					SoundEngine.PlaySound(Owner.HeldItem.UseSound.Value, Owner.MountedCenter);
			}

			Vector2 direction = Vector2.UnitX * 8;
			direction = direction.RotatedBy(Projectile.ai[1]);
			Vector2 position = Owner.MountedCenter + (direction * (7 + (Progress / 30)));
			Projectile.Center = position + (direction * Progress * 0.66f);

			if (Main.netMode != NetmodeID.Server && ++Projectile.localAI[0] == 1) {
				SpiritMod.primitives.CreateTrail(new ArclashPrimTrail(Projectile));
			}
        }

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => damage = (int)(Owner.HeldItem.damage * (int)Math.Pow(Progress, 0.3f));
	}
}